using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hud.Data.Service.Models;

namespace Hud.Data.Service.Tests
{
    [TestClass]
    public class HudDataRepositoryTest
    {
        private MockHudContext _context;
        private IHudDataRepository _target;

        [TestInitialize]
        public void Initialize()
        {
            // initialize test mongo context
            _context = new MockHudContext();
            _target = new HudDataRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // clean up test mongo context
            _context.Cleanup();
        }

        [TestMethod]
        public void HudDataRepository_GetZipCbsaItems_Success()
        {
            // arrange
            var mappings = new []{
                new ZipCbsaItem{ ZipCode = "10001", CbsaCode = "99999"},
                new ZipCbsaItem{ ZipCode = "10001", CbsaCode = "88888"},
                new ZipCbsaItem{ ZipCode = "20001", CbsaCode = "77777"},
                new ZipCbsaItem{ ZipCode = "30001", CbsaCode = "66666"}
            };
            _context.ZipCollection.InsertMany(mappings);

            // act
            var result = _target.GetZipCbsaItems(10001).Result.ToArray();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("10001", result[0].ZipCode);
            Assert.AreEqual("99999", result[0].CbsaCode);
            Assert.AreEqual("10001", result[1].ZipCode);
            Assert.AreEqual("88888", result[1].CbsaCode);
        }

        [TestMethod]
        public void HudDataRepository_GetCbsaMsaItems_SearchForMatchingCBSA_Success()
        {
            // arrange
            var mappings = new[]{
                new CbsaMsaItem{ CbsaCode = "00002", MDiv = "50003", MsaName = "MSA 002", Lsad = "LSAD 002",
                    PopulationEstimates = new[]
                    {
                        new PopulationEstimateItem{ Year = 2014, PopulationEstimate = 201400},
                        new PopulationEstimateItem{ Year = 2015, PopulationEstimate = 201500}
                    } },
                new CbsaMsaItem{ CbsaCode = "60002", MDiv = "60003", MsaName = "Name60004", Lsad = "Name60005",
                    PopulationEstimates = new[]
                    {
                        new PopulationEstimateItem{ Year = 2014, PopulationEstimate = 211400},
                        new PopulationEstimateItem{ Year = 2015, PopulationEstimate = 211500}
                    } },
                new CbsaMsaItem{ CbsaCode = "50002", MDiv = "70003", MsaName = "Name70004"},
            };
            _context.MsaCollection.InsertMany(mappings);

            // act
            var result = _target.GetCbsaMsaItems(new CbsaMsaItemsSearchRequest
            {
                CbsaCode = 2,
                Lsad = "LSAD 002"
            }).Result.ToArray();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("00002", result[0].CbsaCode);
            Assert.AreEqual("LSAD 002", result[0].Lsad);
            Assert.AreEqual("MSA 002", result[0].MsaName);
            Assert.AreEqual(2, result[0].PopulationEstimates.Length);
        }

        [TestMethod]
        public void HudDataRepository_GetCbsaMsaItems_SearchForMatchingMDIV_Success()
        {
            // arrange
            var mappings = new[]{
                new CbsaMsaItem{ CbsaCode = "00002", MDiv = "50003", MsaName = "MSA 002", Lsad = "LSAD 002",
                    PopulationEstimates = new[]
                    {
                        new PopulationEstimateItem{ Year = 2014, PopulationEstimate = 201400},
                        new PopulationEstimateItem{ Year = 2015, PopulationEstimate = 201500}
                    } },
                new CbsaMsaItem{ CbsaCode = "60002", MDiv = "60003", MsaName = "Name60004", Lsad = "Name60005",
                    PopulationEstimates = new[]
                    {
                        new PopulationEstimateItem{ Year = 2014, PopulationEstimate = 211400},
                        new PopulationEstimateItem{ Year = 2015, PopulationEstimate = 211500}
                    } },
                new CbsaMsaItem{ CbsaCode = "50002", MDiv = "70003", MsaName = "Name70004"},
            };
            _context.MsaCollection.InsertMany(mappings);

            // act
            var result = _target.GetCbsaMsaItems(new CbsaMsaItemsSearchRequest
            {
                MDiv = 50003,
                Lsad = "LSAD 002"
            }).Result.ToArray();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("00002", result[0].CbsaCode);
            Assert.AreEqual("LSAD 002", result[0].Lsad);
            Assert.AreEqual("MSA 002", result[0].MsaName);
            Assert.AreEqual(2, result[0].PopulationEstimates.Length);
        }

        [TestMethod]
        public void HudDataRepository_UpdateZipMapping_Success()
        {
            // arrange
            var mappings = new[]{
                new ZipCbsaItem{ ZipCode = "10001", CbsaCode = "10002"},
                new ZipCbsaItem{ ZipCode = "20001", CbsaCode = "20002"},
                new ZipCbsaItem{ ZipCode = "30001", CbsaCode = "30002"}
            };

            // act
            _target.UpdateZipMapping(mappings).Wait();

            // assert
            var result = _target.GetZipCbsaItems(0).Result.ToList();
            Assert.IsNotNull(result);
            Assert.AreEqual(mappings.Length, result.Count());
            for (int index = 0; index < mappings.Length; index++)
            {
                Assert.AreEqual(mappings[index].ZipCode, result[index].ZipCode);
                Assert.AreEqual(mappings[index].CbsaCode, result[index].CbsaCode);
            }
        }

        [TestMethod]
        public void HudDataRepository_UpdateStatisticalAreaMapping_Success()
        {
            // arrange
            var mappings = new[]{
                new CbsaMsaItem{ CbsaCode = "50002", MDiv = "50003", MsaName = "Name50004", Lsad = "Name50005",
                    PopulationEstimates = new[]
                    {
                        new PopulationEstimateItem{ Year = 2014, PopulationEstimate = 201400},
                        new PopulationEstimateItem{ Year = 2015, PopulationEstimate = 201500}
                    } },
                new CbsaMsaItem{ CbsaCode = "60002", MDiv = "60003", MsaName = "Name60004", Lsad = "Name60005",
                    PopulationEstimates = new[]
                    {
                        new PopulationEstimateItem{ Year = 2014, PopulationEstimate = 211400},
                        new PopulationEstimateItem{ Year = 2015, PopulationEstimate = 211500}
                    } },
                new CbsaMsaItem{ CbsaCode = "50002", MDiv = "70003", MsaName = "Name70004"},
            };

            // act
            _target.UpdateStatisticalAreaMapping(mappings).Wait();

            // assert
            var result = _target.GetCbsaMsaItems(new CbsaMsaItemsSearchRequest
            {
                CbsaCode = 50002,
                Lsad = "NAME50005"
            }).Result.ToList();
            Assert.IsNotNull(result);
            var expectedMappings = mappings.Where(q => q.CbsaCode == "50002" && q.Lsad == "Name50005").ToList();
            Assert.AreEqual(expectedMappings.Count(), result.Count());
            for (int index = 0; index < result.Count(); index++)
            {
                Assert.AreEqual(expectedMappings[index].CbsaCode, result[index].CbsaCode);
                Assert.AreEqual(expectedMappings[index].MDiv, result[index].MDiv);
                Assert.AreEqual(expectedMappings[index].MsaName, result[index].MsaName);
                Assert.AreEqual(expectedMappings[index].PopulationEstimates?.Length, result[index].PopulationEstimates?.Length);
            }
        }
    }
}
