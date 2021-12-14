using System.Text.Json;
using System.Threading.Tasks;
using BGPViewerCore.Service;

namespace BGPViewerCore.UnitTests.RipeStatServiceTests
{
    public class RipeStatMockWebApi : RipeStatWebApi
    {
        private JsonDocument Error() => JsonDocument.Parse(@"{ ""messages"": [ [ ""error"", ""An error has occurred."" ] ], ""see_also"": [], ""version"": ""1.3"", ""data_call_name"": ""as-overview"", ""data_call_status"": ""supported - based on 2.1"", ""cached"": false, ""data"": {}, ""query_id"": ""20211214102018-40d28a05-e60e-4321-8399-e27d0e17a282"", ""process_time"": 11, ""server_id"": ""app120"", ""build_version"": ""live.2021.12.10.55"", ""status"": ""error"", ""status_code"": 400, ""time"": ""2021-12-14T10:20:18.465498"" }");
        public override JsonDocument RetrieveAsnDetails(int asNumber)
        {
            if(asNumber == 1299)
                return JsonDocument.Parse(@"{ ""messages"": [], ""see_also"": [], ""version"": ""1.3"", ""data_call_name"": ""as-overview"", ""data_call_status"": ""supported - based on 2.1"", ""cached"": false, ""data"": { ""type"": ""as"", ""resource"": ""1299"", ""block"": { ""resource"": ""1-1876"", ""desc"": ""Assigned by ARIN"", ""name"": ""IANA 16-bit Autonomous System (AS) Numbers Registry"" }, ""holder"": ""TWELVE99 - Telia Company AB"", ""announced"": true, ""query_starttime"": ""2021-12-13T16:00:00"", ""query_endtime"": ""2021-12-13T16:00:00"" }, ""query_id"": ""20211214003817-09165af2-7e35-4145-b14b-190a8ea7715a"", ""process_time"": 130, ""server_id"": ""app111"", ""build_version"": ""live.2021.12.10.55"", ""status"": ""ok"", ""status_code"": 200, ""time"": ""2021-12-14T00:38:17.356572"" }");
            if(asNumber == 264075)
                return JsonDocument.Parse(@"{ ""messages"": [], ""see_also"": [], ""version"": ""1.3"", ""data_call_name"": ""as-overview"", ""data_call_status"": ""supported - based on 2.1"", ""cached"": false, ""data"": { ""type"": ""as"", ""resource"": ""264075"", ""block"": { ""resource"": ""263680-264604"", ""desc"": ""Assigned by LACNIC"", ""name"": ""IANA 32-bit Autonomous System (AS) Numbers Registry"" }, ""holder"": ""K1 Telecom e Multimidia LTDA"", ""announced"": true, ""query_starttime"": ""2021-12-14T00:00:00"", ""query_endtime"": ""2021-12-14T00:00:00"" }, ""query_id"": ""20211214101331-1e196b72-5b09-452b-a5ee-07200f0f62d8"", ""process_time"": 78, ""server_id"": ""app145"", ""build_version"": ""live.2021.12.10.55"", ""status"": ""ok"", ""status_code"": 200, ""time"": ""2021-12-14T10:13:31.351844"" }");
            return Error();
        }

        public override Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber)
            => Task.FromResult(RetrieveAsnDetails(asNumber));

        public override JsonDocument RetrieveIpDetails(string ipAddress)
        {
            if(ipAddress == "191.241.64.0")
                return JsonDocument.Parse(@"{ ""messages"": [], ""see_also"": [], ""version"": ""1.0"", ""data_call_name"": ""network-info"", ""data_call_status"": ""supported"", ""cached"": false, ""data"": { ""asns"": [ ""53181"" ], ""prefix"": ""191.241.64.0/24"" }, ""query_id"": ""20211214105900-963b39b6-58ab-451d-a2d5-dc924ebbeea8"", ""process_time"": 83, ""server_id"": ""app126"", ""build_version"": ""live.2021.12.10.55"", ""status"": ""ok"", ""status_code"": 200, ""time"": ""2021-12-14T10:59:01.012383"" }");
            if(ipAddress == "2001:db8::")
                return JsonDocument.Parse(@"{ ""messages"": [], ""see_also"": [], ""version"": ""1.0"", ""data_call_name"": ""network-info"", ""data_call_status"": ""supported"", ""cached"": false, ""data"": { ""asns"": [ ""131111"" ], ""prefix"": ""2000::/3"" }, ""query_id"": ""20211214112026-f2f13557-909d-4e4e-9545-d8664e61ad33"", ""process_time"": 80, ""server_id"": ""app116"", ""build_version"": ""live.2021.12.10.55"", ""status"": ""ok"", ""status_code"": 200, ""time"": ""2021-12-14T11:20:26.104886"" }");
            return Error();
        }
    }
}