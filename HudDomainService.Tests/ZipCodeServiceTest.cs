using Hud.Data.Service;
using Hud.Data.Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Hud.Domain.Service.Tests
{
    [TestClass]
    public class ZipCodeServiceTest
    {
        private IZipCodeService _target;
        private Mock<IHudDataRepository> _hudDataRepository;

        private const string _msaDescription = "Metropolitan Statistical Area";

        [TestInitialize]
        public void Initialize()
        {
            _hudDataRepository = new Mock<IHudDataRepository>();
            _target = new ZipCodeService(_hudDataRepository.Object);
        }

        [TestMethod]
        public void ZipCodeService_GetZipDetails_ZipCodeHasNoMatchingCBSACode_ReturnsNull()
        {
            // arrange
            _hudDataRepository.Setup(x => x.GetZipCbsaItems(It.IsAny<int>()))
                .ReturnsAsync(new List<ZipCbsaItem> { new ZipCbsaItem {CbsaCode = "99999" } });

            // act
            var result = _target.GetZipDetails(10001).Result;

            // assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ZipCodeService_GetZipDetails_ZipCodeHasInvalidCBSACode_ReturnsNull()
        {
            // arrange
            _hudDataRepository.Setup(x => x.GetZipCbsaItems(It.IsAny<int>()))
                .ReturnsAsync(new List<ZipCbsaItem> { new ZipCbsaItem { CbsaCode = "ABCDE" } });

            // act
            var result = _target.GetZipDetails(10001).Result;

            // assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ZipCodeService_GetZipDetails_CBSAMatchesAlternateCode_ReturnsZipDetailsUsingAlternateCode()
        {
            // arrange
            _hudDataRepository.Setup(x => x.GetZipCbsaItems(It.IsAny<int>()))
                .ReturnsAsync(new List<ZipCbsaItem> { new ZipCbsaItem { ZipCode = "10001", CbsaCode = "12345" } });

            var alternateCodeSearchResult = new [] { new CbsaMsaItem {MDiv = "12345", CbsaCode = "56789"} };
            var cbsaCodeSearchResult = new [] { new CbsaMsaItem {
                    CbsaCode = "56789", MsaName = "Test MSA Name",
                    PopulationEstimates = new[]
                    {
                        new PopulationEstimateItem{Year=2014, PopulationEstimate=11111 },
                        new PopulationEstimateItem{Year=2015, PopulationEstimate=22222 },
                    }}};

            _hudDataRepository.Setup(x => x.GetCbsaMsaItems(
                It.Is<CbsaMsaItemsSearchRequest>(s => s.MDiv == 12345 || (s.CbsaCode == 56789 && s.Lsad.Equals(_msaDescription)))))
                .ReturnsAsync( (CbsaMsaItemsSearchRequest c) => c.MDiv != null ? alternateCodeSearchResult : cbsaCodeSearchResult);

            // act
            var result = _target.GetZipDetails(10001).Result;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("10001", result.ZipCode);
            Assert.AreEqual("56789", result.CbsaCode);
            Assert.AreEqual("Test MSA Name", result.MsaName);
            Assert.AreEqual(2, result.PopulationEstimates.Length);
            Assert.AreEqual(2014, result.PopulationEstimates[0].Year);
            Assert.AreEqual(11111, result.PopulationEstimates[0].PopulationEstimate);
            Assert.AreEqual(2015, result.PopulationEstimates[1].Year);
            Assert.AreEqual(22222, result.PopulationEstimates[1].PopulationEstimate);
        }

        [TestMethod]
        public void ZipCodeService_GetZipDetails_CBSAMatchesPrimaryCode_ReturnsZipDetailsUsingPrimaryCode()
        {
            // arrange
            _hudDataRepository.Setup(x => x.GetZipCbsaItems(It.IsAny<int>()))
                .ReturnsAsync(new List<ZipCbsaItem> { new ZipCbsaItem { ZipCode = "10001", CbsaCode = "12345" } });

            _hudDataRepository.Setup(x => x.GetCbsaMsaItems(
                It.Is<CbsaMsaItemsSearchRequest>(s => s.CbsaCode == 12345 && s.Lsad.Equals(_msaDescription))))
                .ReturnsAsync(new List<CbsaMsaItem> { new CbsaMsaItem {CbsaCode = "12345", MsaName = "Test MSA Name",
                    PopulationEstimates = new[]
                    {
                        new PopulationEstimateItem{Year=2014, PopulationEstimate=11111 },
                        new PopulationEstimateItem{Year=2015, PopulationEstimate=22222 },
                    }
                } });

            // act
            var result = _target.GetZipDetails(10001).Result;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("10001", result.ZipCode);
            Assert.AreEqual("12345", result.CbsaCode);
            Assert.AreEqual("Test MSA Name", result.MsaName);
            Assert.AreEqual(2, result.PopulationEstimates.Length);
            Assert.AreEqual(2014, result.PopulationEstimates[0].Year);
            Assert.AreEqual(11111, result.PopulationEstimates[0].PopulationEstimate);
            Assert.AreEqual(2015, result.PopulationEstimates[1].Year);
            Assert.AreEqual(22222, result.PopulationEstimates[1].PopulationEstimate);
        }

        [TestMethod]
        public void ZipCodeService_GetZipDetails_ZipCodeIsLessThan5Digits_ReturnsZipDetailsWith5DigitZipCode()
        {
            // arrange
            _hudDataRepository.Setup(x => x.GetZipCbsaItems(It.IsAny<int>()))
                .ReturnsAsync(new List<ZipCbsaItem> { new ZipCbsaItem { ZipCode = "00012", CbsaCode = "12345" } });

            _hudDataRepository.Setup(x => x.GetCbsaMsaItems(
                It.Is<CbsaMsaItemsSearchRequest>(s => s.CbsaCode == 12345 && s.Lsad.Equals(_msaDescription))))
                .ReturnsAsync(new List<CbsaMsaItem> { new CbsaMsaItem {CbsaCode = "12345" } });

            // act
            var result = _target.GetZipDetails(12).Result;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("00012", result.ZipCode);
            Assert.AreEqual("12345", result.CbsaCode);
        }
    }
}
