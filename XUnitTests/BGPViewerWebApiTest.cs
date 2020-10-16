using System.Linq;
using System.Text.Json;
using BGPViewerCore.Service;

namespace Xunit
{
    public class BGPViewerWebApiTest
    {  
        private BGPViewerWebApi _api = new BGPViewerWebApi();

        // Timestamp can be different for each query, so we will remove it
        private string RemoveTimestampElement(string jsonRawText)
        {
            var index = jsonRawText.IndexOf("date_updated");
            return jsonRawText.Remove(index-1, @"""date_updated"":""2020-08-24 03:46:00""".Count());
        }

        [Fact]
        public void RetrieveAsnDetails()
        {
            var document = _api.RetrieveAsnDetails(53181);
            var mockData = JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asn"":53181,""name"":null,""description_short"":null,""description_full"":[],""country_code"":"""",""website"":null,""email_contacts"":[],""abuse_contacts"":[],""looking_glass"":null,""traffic_estimation"":null,""traffic_ratio"":null,""owner_address"":[],""rir_allocation"":{""rir_name"":""Lacnic"",""country_code"":""BR"",""date_allocated"":""2010-08-18 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":""assigned"",""description"":""Assigned by LACNIC"",""whois_server"":""whois.lacnic.net"",""date_assigned"":null},""date_updated"":""2020-08-22 04:57:28""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""704.01 ms""}}");
            Assert.Equal(
                RemoveTimestampElement(document.RootElement.GetProperty("data").GetRawText()), 
                RemoveTimestampElement(mockData.RootElement.GetProperty("data").GetRawText())
            );
        }
    }
}