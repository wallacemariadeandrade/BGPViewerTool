using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BGPViewerCore.Service
{
    public class ArinWebApi
    {
        private const string BASE_ENDPOINT_URL = "http://whois.arin.net/rest";
        private Tuple<string,string>[] headers = new Tuple<string,string>[] { 
            new Tuple<string , string>("Accept", "application/json")
        };

        private string BuildAsnDetailsEndpoint(int asNumber) => $"{BASE_ENDPOINT_URL}/asn/{asNumber}";
        private string BuildIpDetailsEndpoint(string ipAddress) => $"{BASE_ENDPOINT_URL}/ip/{ipAddress}";
        
        protected JsonDocument ErrorResult(string content)
        {
            var result = @"{ ""error_title"": ""<error_title>"", ""error_message"": ""<error_message>"" }";

            var startTitleIndex = content.IndexOf("<th>");
            var endTitleIndex = content.IndexOf("</th>");
            var title = content.Substring(startTitleIndex, endTitleIndex - startTitleIndex).Replace("<th>", "");

            var startMessageIndex = content.IndexOf("<td>");
            var endMessageIndex = content.IndexOf("</td>");
            var message = content.Substring(startMessageIndex, endMessageIndex - startMessageIndex).Replace("<td>", "");

            return JsonDocument.Parse(result.Replace("<error_title>", title).Replace("<error_message>", message));
        }

        protected JsonDocument ErrorResult(Stream content)
        {
            using(var reader = new StreamReader(content))
            {
                return ErrorResult(reader.ReadToEnd());
            }           
        }

        public virtual JsonDocument RetrieveAsnDetails(int asNumber)
        {
            var content = WebService.GetContentFrom(BuildAsnDetailsEndpoint(asNumber), headers);
            try
            {
                return JsonDocument.Parse(content);
            }
            catch (JsonException)
            {
                return ErrorResult(content);
            }
        }
        
        public virtual async Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber)
        {
            using (var stream = await WebService.GetContentStreamFromAsync(BuildAsnDetailsEndpoint(asNumber), headers))
            {       
                try
                {
                    return await JsonDocument.ParseAsync(stream);
                }
                catch (JsonException)
                {
                    return ErrorResult(stream);
                }
            }
        }

        public virtual JsonDocument RetrieveIpDetails(string ipAddress)
        {
            var content = WebService.GetContentFrom(BuildIpDetailsEndpoint(ipAddress), headers);
            try
            {
                return JsonDocument.Parse(content);
            }
            catch (JsonException)
            {
                return ErrorResult(content);
            }
        }

        public virtual async Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress)
        {
            using (var stream = await WebService.GetContentStreamFromAsync(BuildIpDetailsEndpoint(ipAddress), headers))
            {
                try
                {
                    return await JsonDocument.ParseAsync(stream);
                }
                catch (JsonException)
                {
                    return ErrorResult(stream);
                }
            }
        }
    }
}