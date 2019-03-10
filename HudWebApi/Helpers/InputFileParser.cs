using Hud.Domain.Service.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hud.Application.Service
{
    internal abstract class InputFileParser<T> where T : MappingItem
    {
        public async Task<T[]> GetMappings(HttpRequestMessage request)
        {
            // todo return an error code here and have the controller throw the http exception
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //todo save the uploaded file on the server, maintain a history of uploads, and current version being used etc

            var provider = new MultipartMemoryStreamProvider();
            await request.Content.ReadAsMultipartAsync(provider);
            var file = provider.Contents[0];
            var bytes = await file.ReadAsByteArrayAsync();

            return ParseData(Encoding.Default.GetString(bytes));
        }

        public abstract T[] ParseData(string data);

        // can move to a separate helper
        static Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);

        public static string[] SplitCSV(string input)
        {
            var list = new List<string>();
            string curr = null;
            foreach (Match match in csvSplit.Matches(input))
            {
                curr = match.Value;
                if (0 == curr.Length)
                {
                    list.Add("");
                }

                list.Add(curr.TrimStart(','));
            }

            return list.ToArray();
        }

    }
}