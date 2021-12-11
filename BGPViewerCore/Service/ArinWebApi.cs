using System;
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
        protected JsonDocument DefaultReturn() => JsonDocument.Parse(@"{}");
        
        public virtual JsonDocument RetrieveAsnDetails(int asNumber)
        {
            try
            {
                return JsonDocument.Parse(WebService.GetContentFrom(BuildAsnDetailsEndpoint(asNumber), headers));
            }
            catch (JsonException)
            {
                return DefaultReturn();
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
                    return DefaultReturn();
                }
            }
        }

        public virtual JsonDocument RetrieveIpDetails(string ipAddress)
        {
            try
            {
                return JsonDocument.Parse(WebService.GetContentFrom(BuildIpDetailsEndpoint(ipAddress), headers));
            }
            catch (JsonException)
            {
                return DefaultReturn();
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
                    return DefaultReturn();
                }
            }
        }
    }
}