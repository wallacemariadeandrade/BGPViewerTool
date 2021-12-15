using System;
using System.Collections.Generic;
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

        protected async Task<JsonDocument> ErrorResultAsync(Stream content)
        {
            using(var reader = new StreamReader(content))
            {
                return ErrorResult(await reader.ReadToEndAsync());
            }           
        }

        private JsonDocument ParseOperation(string url, IEnumerable<Tuple<string,string>> headers = null)
        {
            try
            {
                return JsonDocument.Parse(WebService.GetContentFrom(url, headers));
            }
            catch (System.Net.WebException ex)
            {
                using(var stream = ex.Response.GetResponseStream())
                {
                    return ErrorResult(stream);
                }
            }
        }

        private async Task<JsonDocument> ParseOperationAsync(string url, IEnumerable<Tuple<string,string>> headers = null) 
        {
            try
            {
                using (var stream = await WebService.GetContentStreamFromAsync(url, headers))
                {
                    return await JsonDocument.ParseAsync(stream);    
                }
            }
            catch (System.Net.WebException ex)
            {
                using(var stream = ex.Response.GetResponseStream())
                {
                    return await ErrorResultAsync(stream);
                }
            }
        }

        public virtual JsonDocument RetrieveAsnDetails(int asNumber)
            => ParseOperation(BuildAsnDetailsEndpoint(asNumber), headers);
        
        public virtual Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber)
            => ParseOperationAsync(BuildAsnDetailsEndpoint(asNumber), headers);

        public virtual JsonDocument RetrieveIpDetails(string ipAddress)
            => ParseOperation(BuildIpDetailsEndpoint(ipAddress), headers);

        public virtual Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress)
            => ParseOperationAsync(BuildIpDetailsEndpoint(ipAddress), headers);
    }
}