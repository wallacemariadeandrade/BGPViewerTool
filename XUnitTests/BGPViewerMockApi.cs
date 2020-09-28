using System.Text.Json;
using BGPViewerCore.Service;

namespace Xunit
{
    public class BGPViewerMockApi : IBGPViewerApi
    {
        public JsonDocument RetrieveAsnDetails(int asNumber)
        {
            if(asNumber == 6762)
            {
                return JsonDocument.Parse(
                    @"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asn"":6762,""name"":""SEABONE-NET"",""description_short"":""TELECOM ITALIA SPARKLE S.p.A."",""description_full"":[""TELECOM ITALIA SPARKLE S.p.A.""],""country_code"":""IT"",""website"":""https:\/\/www.tisparkle.com\/"",""email_contacts"":[""abuse@seabone.net"",""peering@seabone.net"",""tech@seabone.net"",""francesco.chiappini@telecomitalia.it""],""abuse_contacts"":[""abuse@seabone.net""],""looking_glass"":""https:\/\/gambadilegno.noc.seabone.net\/lg\/"",""traffic_estimation"":""1-5Tbps"",""traffic_ratio"":""Balanced"",""owner_address"":[""Via Macchia Palocco 223"",""00125"",""Rome"",""ITALY""],""rir_allocation"":{""rir_name"":""RIPE"",""country_code"":""IT"",""date_allocated"":""1996-09-12 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":null,""description"":null,""whois_server"":null,""date_assigned"":null},""date_updated"":""2020-08-24 03:46:00""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""335.47 ms""}}"
                );
            }
            return JsonDocument.Parse(
                @"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asn"":53181,""name"":null,""description_short"":null,""description_full"":[],""country_code"":"""",""website"":null,""email_contacts"":[],""abuse_contacts"":[],""looking_glass"":null,""traffic_estimation"":null,""traffic_ratio"":null,""owner_address"":[],""rir_allocation"":{""rir_name"":""Lacnic"",""country_code"":""BR"",""date_allocated"":""2010-08-18 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":null,""description"":null,""whois_server"":null,""date_assigned"":null},""date_updated"":""2020-08-22 04:57:28""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""307.7 ms""}}"
            );
        }
    }
}