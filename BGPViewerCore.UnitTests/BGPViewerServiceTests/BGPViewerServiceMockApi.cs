using System.Text.Json;
using System.Threading.Tasks;
using BGPViewerCore.Service;

namespace BGPViewerCore.UnitTests.BGPViewerServiceTests
{
    public class BGPViewerMockApi : IBGPViewerApi
    {
        public JsonDocument RetrieveAsnDetails(int asNumber)
        {
            if(asNumber == 6762)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asn"":6762,""name"":""SEABONE-NET"",""description_short"":""TELECOM ITALIA SPARKLE S.p.A."",""description_full"":[""TELECOM ITALIA SPARKLE S.p.A.""],""country_code"":""IT"",""website"":""https:\/\/www.tisparkle.com\/"",""email_contacts"":[""abuse@seabone.net"",""peering@seabone.net"",""tech@seabone.net"",""francesco.chiappini@telecomitalia.it""],""abuse_contacts"":[""abuse@seabone.net""],""looking_glass"":""https:\/\/gambadilegno.noc.seabone.net\/lg\/"",""traffic_estimation"":""1-5Tbps"",""traffic_ratio"":""Balanced"",""owner_address"":[""Via Macchia Palocco 223"",""00125"",""Rome"",""ITALY""],""rir_allocation"":{""rir_name"":""RIPE"",""country_code"":""IT"",""date_allocated"":""1996-09-12 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":null,""description"":null,""whois_server"":null,""date_assigned"":null},""date_updated"":""2020-08-24 03:46:00""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""335.47 ms""}}");
            if(asNumber == 53181)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asn"":53181,""name"":null,""description_short"":null,""description_full"":[],""country_code"":"""",""website"":null,""email_contacts"":[],""abuse_contacts"":[],""looking_glass"":null,""traffic_estimation"":null,""traffic_ratio"":null,""owner_address"":[],""rir_allocation"":{""rir_name"":""Lacnic"",""country_code"":""BR"",""date_allocated"":""2010-08-18 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":null,""description"":null,""whois_server"":null,""date_assigned"":null},""date_updated"":""2020-08-22 04:57:28""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""307.7 ms""}}");
            return JsonDocument.Parse(@"{""status"":""error"",""status_message"":""Could not find ASN"",""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""5027.25 ms""}}");
        }

        public Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber) => Task.FromResult(RetrieveAsnDetails(asNumber));

        public JsonDocument RetrieveAsnDownstreams(int asNumber)
        {
            if(asNumber == 52908)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_downstreams"":[{""asn"":267360,""name"":null,""description"":null,""country_code"":""""},{""asn"":268699,""name"":null,""description"":null,""country_code"":""""}],""ipv6_downstreams"":[]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""98.38 ms""}}");
            return JsonDocument.Parse(@"{""status"":""error"",""status_message"":""Could not find ASN"",""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""5027.25 ms""}}");
        }

        public Task<JsonDocument> RetrieveAsnDownstreamsAsync(int asNumber)
            => Task.FromResult(RetrieveAsnDownstreams(asNumber));

        public JsonDocument RetrieveAsnIxs(int asNumber)
        {
            if(asNumber == 53181)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":[{""ix_id"":94,""name"":""IX.br (PTT.br) São Paulo"",""name_full"":""IX.br (PTT.br) São Paulo"",""country_code"":""BR"",""city"":""São Paulo\/SP"",""ipv4_address"":""187.16.222.222"",""ipv6_address"":""2001:12f8::222:222"",""speed"":10000},{""ix_id"":100,""name"":""IX.br (PTT.br) Rio de Janeiro"",""name_full"":""IX.br (PTT.br) Rio de Janeiro"",""country_code"":""BR"",""city"":""Rio de Janeiro\/RJ"",""ipv4_address"":""45.6.52.153"",""ipv6_address"":""2001:12f8:0:2::153"",""speed"":20000},{""ix_id"":100,""name"":""IX.br (PTT.br) Rio de Janeiro"",""name_full"":""IX.br (PTT.br) Rio de Janeiro"",""country_code"":""BR"",""city"":""Rio de Janeiro\/RJ"",""ipv4_address"":""45.6.52.210"",""ipv6_address"":""2001:12f8:0:2::210"",""speed"":20000},{""ix_id"":100,""name"":""IX.br (PTT.br) Rio de Janeiro"",""name_full"":""IX.br (PTT.br) Rio de Janeiro"",""country_code"":""BR"",""city"":""Rio de Janeiro\/RJ"",""ipv4_address"":""45.6.53.32"",""ipv6_address"":""2001:12f8:0:2::53:32"",""speed"":100000}],""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""37.86 ms""}}");
            if(asNumber == 268003)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":[]}");
            return JsonDocument.Parse(@"{""status"":""error"",""status_message"":""Could not find ASN"",""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""5027.25 ms""}}");
        }

        public Task<JsonDocument> RetrieveAsnIxsAsync(int asNumber)
            => Task.FromResult(RetrieveAsnIxs(asNumber));

        public JsonDocument RetrieveAsnPeers(int asNumber)
        {
            if(asNumber == 268374)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_peers"":[{""asn"":53181,""name"":null,""description"":null,""country_code"":""""},{""asn"":6939,""name"":""HURRICANE"",""description"":""Hurricane Electric LLC"",""country_code"":""US""},{""asn"":39533,""name"":""asympto"",""description"":""Asympto Networks Kft."",""country_code"":""CH""},{""asn"":60501,""name"":""SIRIUSTEC-IT"",""description"":""Sirius Technology SRL"",""country_code"":""IT""},{""asn"":13786,""name"":""SEABRAS-1"",""description"":""Seabras 1 USA, LLC"",""country_code"":""US""},{""asn"":28186,""name"":null,""description"":null,""country_code"":""""},{""asn"":23106,""name"":null,""description"":null,""country_code"":""""},{""asn"":52863,""name"":null,""description"":null,""country_code"":""""},{""asn"":52873,""name"":null,""description"":null,""country_code"":""""},{""asn"":267613,""name"":null,""description"":null,""country_code"":""""},{""asn"":52320,""name"":""GlobeNet Cabos Submarinos Colombia, S.A.S."",""description"":""GlobeNet Cabos Submarinos Colombia, S.A.S."",""country_code"":""CO""},{""asn"":41047,""name"":""NL-MLAB"",""description"":""MLAB Open Source Community"",""country_code"":""NL""},{""asn"":57463,""name"":""NetIX"",""description"":""NetIX Communications Ltd."",""country_code"":""BG""}],""ipv6_peers"":[{""asn"":53181,""name"":null,""description"":null,""country_code"":""""},{""asn"":6939,""name"":""HURRICANE"",""description"":""Hurricane Electric LLC"",""country_code"":""US""},{""asn"":57463,""name"":""NetIX"",""description"":""NetIX Communications Ltd."",""country_code"":""BG""},{""asn"":61597,""name"":null,""description"":null,""country_code"":""""},{""asn"":13786,""name"":""SEABRAS-1"",""description"":""Seabras 1 USA, LLC"",""country_code"":""US""},{""asn"":23106,""name"":null,""description"":null,""country_code"":""""},{""asn"":52873,""name"":null,""description"":null,""country_code"":""""},{""asn"":267613,""name"":null,""description"":null,""country_code"":""""}]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""84.23 ms""}}");
            if(asNumber == 264075)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_peers"":[{""asn"":53181,""name"":null,""description"":null,""country_code"":""""}],""ipv6_peers"":[{""asn"":53181,""name"":null,""description"":null,""country_code"":""""}]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""69.02 ms""}}");
            return JsonDocument.Parse(@"{""status"":""error"",""status_message"":""Could not find ASN"",""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""5027.25 ms""}}");
        }

        public Task<JsonDocument> RetrieveAsnPeersAsync(int asNumber)
            => Task.FromResult(RetrieveAsnPeers(asNumber));

        public JsonDocument RetrieveAsnPrefixes(int asNumber)
        {
            if(asNumber == 264075)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_prefixes"":[{""prefix"":""143.208.20.0\/22"",""ip"":""143.208.20.0"",""cidr"":22,""roa_status"":""None"",""name"":""K1 Telecom e Multimidia LTDA"",""description"":""K1 Telecom e Multimidia LTDA"",""country_code"":""BR"",""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}}],""ipv6_prefixes"":[{""prefix"":""2804:2a7c::\/32"",""ip"":""2804:2a7c::"",""cidr"":32,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}}]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""185.29 ms""}}");
            if (asNumber == 268374)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_prefixes"":[{""prefix"":""45.239.24.0\/22"",""ip"":""45.239.24.0"",""cidr"":22,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.24.0\/24"",""ip"":""45.239.24.0"",""cidr"":24,""roa_status"":""None"",""name"":""Telnet Servicos e Comi\u00c2\u00bf\u00c2\u00bdrcio em Informi\u00c2\u00bf\u00c2\u00bdtica Ltda."",""description"":""Telnet Servicos e Comi\u00c2\u00bf\u00c2\u00bdrcio em Informi\u00c2\u00bf\u00c2\u00bdtica Ltda."",""country_code"":""BR"",""parent"":{""prefix"":""45.239.24.0\/22"",""ip"":""45.239.24.0"",""cidr"":22,""rir_name"":""Lacnic"",""allocation_status"":""unknown""}},{""prefix"":""45.239.24.0\/23"",""ip"":""45.239.24.0"",""cidr"":23,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.25.0\/24"",""ip"":""45.239.25.0"",""cidr"":24,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.26.0\/24"",""ip"":""45.239.26.0"",""cidr"":24,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.26.0\/23"",""ip"":""45.239.26.0"",""cidr"":23,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.27.0\/24"",""ip"":""45.239.27.0"",""cidr"":24,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}}],""ipv6_prefixes"":[{""prefix"":""2804:5030::\/32"",""ip"":""2804:5030::"",""cidr"":32,""roa_status"":""None"",""name"":""Telnet Servicos e Comi\u00c2\u00bf\u00c2\u00bdrcio em Informi\u00c2\u00bf\u00c2\u00bdtica Ltda."",""description"":""Telnet Servicos e Comi\u00c2\u00bf\u00c2\u00bdrcio em Informi\u00c2\u00bf\u00c2\u00bdtica Ltda."",""country_code"":""BR"",""parent"":{""prefix"":""2804:5030::\/32"",""ip"":""2804:5030::"",""cidr"":32,""rir_name"":""Lacnic"",""allocation_status"":""unknown""}}]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""366.7 ms""}}");
            if(asNumber == 131630)
                return JsonDocument.Parse(
                    @"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_prefixes"":[{""prefix"":""103.121.176.0\/24"",""ip"":""103.121.176.0"",""cidr"":24,""roa_status"":""None"",""name"":""GARENATW-NET"",""description"":""Garena (TAIWAN) CO. LTD."",""country_code"":""TW"",""parent"":{""prefix"":""103.121.176.0\/22"",""ip"":""103.121.176.0"",""cidr"":22,""rir_name"":""APNIC"",""allocation_status"":""unknown""}},{""prefix"":""103.121.177.0\/24"",""ip"":""103.121.177.0"",""cidr"":24,""roa_status"":""None"",""name"":""GARENATW-NET"",""description"":""Garena (TAIWAN) CO. LTD."",""country_code"":""TW"",""parent"":{""prefix"":""103.121.176.0\/22"",""ip"":""103.121.176.0"",""cidr"":22,""rir_name"":""APNIC"",""allocation_status"":""unknown""}},{""prefix"":""103.121.178.0\/24"",""ip"":""103.121.178.0"",""cidr"":24,""roa_status"":""None"",""name"":""GARENATW-NET"",""description"":""Garena (TAIWAN) CO. LTD."",""country_code"":""TW"",""parent"":{""prefix"":""103.121.176.0\/22"",""ip"":""103.121.176.0"",""cidr"":22,""rir_name"":""APNIC"",""allocation_status"":""unknown""}}],""ipv6_prefixes"":[]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""159.15 ms""}}"
                );
            return JsonDocument.Parse(@"{""status"":""error"",""status_message"":""Could not find ASN"",""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""5027.25 ms""}}");
        }

        public Task<JsonDocument> RetrieveAsnPrefixesAsync(int asNumber)
            => Task.FromResult(RetrieveAsnPrefixes(asNumber));

        public JsonDocument RetrieveAsnUpstreams(int asNumber)
        {
            if(asNumber == 52575) 
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_upstreams"":[{""asn"":265185,""name"":null,""description"":null,""country_code"":""""}],""ipv6_upstreams"":[],""ipv4_graph"":""https:\/\/api.bgpview.io\/assets\/graphs\/AS52575_IPv4.svg"",""ipv6_graph"":""https:\/\/api.bgpview.io\/assets\/graphs\/AS52575_IPv6.svg"",""combined_graph"":""https:\/\/api.bgpview.io\/assets\/graphs\/AS52575_Combined.svg""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""141 ms""}}");
            return JsonDocument.Parse(@"{""status"":""error"",""status_message"":""Could not find ASN"",""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""5027.25 ms""}}");
        }

        public Task<JsonDocument> RetrieveAsnUpstreamsAsync(int asNumber)
            => Task.FromResult(RetrieveAsnUpstreams(asNumber));

        public JsonDocument RetrieveIpDetails(string ipAddress)
        {
            if(ipAddress == "143.208.20.0")
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ip"":""143.208.20.0"",""ptr_record"":""143-208-20-0.k1fibra.net.br"",""prefixes"":[{""prefix"":""143.208.20.0\/22"",""ip"":""143.208.20.0"",""cidr"":22,""asn"":{""asn"":264075,""name"":""IANA-ASSIGNED"",""description"":""IANA-ASSIGNED"",""country_code"":null},""name"":""K1 Telecom e Multimidia LTDA"",""description"":""K1 Telecom e Multimidia LTDA"",""country_code"":""BR""}],""rir_allocation"":{""rir_name"":""Lacnic"",""country_code"":""BR"",""ip"":""143.208.20.0"",""cidr"":22,""prefix"":""143.208.20.0\/22"",""date_allocated"":""2015-10-22 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":""legacy"",""description"":""Administered by ARIN"",""whois_server"":""whois.arin.net"",""date_assigned"":null},""maxmind"":{""country_code"":""BR"",""city"":""Cachoeiras de Macacu""}},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""1960.61 ms""}}");
            if(ipAddress == "170.245.37.10")
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ip"":""170.245.37.10"",""ptr_record"":null,""prefixes"":[{""prefix"":""170.245.37.0\/24"",""ip"":""170.245.37.0"",""cidr"":24,""asn"":{""asn"":266021,""name"":""IANA-ASSIGNED"",""description"":""IANA-ASSIGNED"",""country_code"":null},""name"":null,""description"":null,""country_code"":null},{""prefix"":""170.245.36.0\/22"",""ip"":""170.245.36.0"",""cidr"":22,""asn"":{""asn"":266021,""name"":""IANA-ASSIGNED"",""description"":""IANA-ASSIGNED"",""country_code"":null},""name"":""SIC INFORMi\u00c2\u00bf\u00c2\u00bdTICA E TECNOLOGIA - EIRELI - ME"",""description"":""SIC INFORMi\u00c2\u00bf\u00c2\u00bdTICA E TECNOLOGIA - EIRELI - ME"",""country_code"":""BR""}],""rir_allocation"":{""rir_name"":""Lacnic"",""country_code"":""BR"",""ip"":""170.245.36.0"",""cidr"":22,""prefix"":""170.245.36.0\/22"",""date_allocated"":""2017-01-26 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":""legacy"",""description"":""Administered by ARIN"",""whois_server"":""whois.arin.net"",""date_assigned"":null},""maxmind"":{""country_code"":""BR"",""city"":""Sao Joao da Barra""}},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""1261.41 ms""}}");
            return JsonDocument.Parse(@"{""status"":""error"",""status_message"":""Malformed input"",""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""2095.67 ms""}}");
        }

        public Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress)
            => Task.FromResult(RetrieveIpDetails(ipAddress));

        public JsonDocument RetrievePrefixDetails(string prefix, byte cidr)
        {
            if(prefix == "143.208.20.0" && cidr == 22)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""prefix"":""143.208.20.0\/22"",""ip"":""143.208.20.0"",""cidr"":22,""asns"":[{""asn"":264075,""name"":""IANA-"",""description"":null,""country_code"":null,""prefix_upstreams"":[{""asn"":53181,""name"":""IANA-"",""description"":null,""country_code"":null}]}],""name"":""K1 Telecom e Multimidia LTDA"",""description_short"":""K1 Telecom e Multimidia LTDA"",""description_full"":[""K1 Telecom e Multimidia LTDA""],""email_contacts"":[],""abuse_contacts"":[],""owner_address"":[],""country_codes"":{""whois_country_code"":""BR"",""rir_allocation_country_code"":""BR"",""maxmind_country_code"":""BR""},""rir_allocation"":{""rir_name"":""Lacnic"",""country_code"":""BR"",""ip"":""143.208.20.0"",""cidr"":22,""prefix"":""143.208.20.0\/22"",""date_allocated"":""2015-10-22 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":null,""description"":null,""whois_server"":null,""date_assigned"":null},""maxmind"":{""country_code"":""BR"",""city"":""Cachoeiras de Macacu""},""related_prefixes"":[{""prefix"":""143.208.23.0\/24"",""ip"":""143.208.23.0"",""cidr"":24,""asn"":264075,""name"":null,""description"":null,""country_code"":null}],""date_updated"":""2020-09-19 10:16:45""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""8477.62 ms""}}");
            if(prefix == "177.75.40.0" && cidr == 21)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""prefix"":""177.75.40.0\/21"",""ip"":""177.75.40.0"",""cidr"":21,""asns"":[{""asn"":53040,""name"":""IANA-"",""description"":null,""country_code"":null,""prefix_upstreams"":[{""asn"":53181,""name"":""IANA-"",""description"":null,""country_code"":null}]}],""name"":null,""description_short"":null,""description_full"":[],""email_contacts"":[],""abuse_contacts"":[],""owner_address"":[],""country_codes"":{""whois_country_code"":null,""rir_allocation_country_code"":""BR"",""maxmind_country_code"":""BR""},""rir_allocation"":{""rir_name"":""Lacnic"",""country_code"":""BR"",""ip"":""177.75.40.0"",""cidr"":21,""prefix"":""177.75.40.0\/21"",""date_allocated"":""2011-09-13 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":null,""description"":null,""whois_server"":null,""date_assigned"":null},""maxmind"":{""country_code"":""BR"",""city"":""Itaborai""},""related_prefixes"":[],""date_updated"":""2020-08-27 15:29:28""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""4320.03 ms""}}");
            return JsonDocument.Parse(@"{""status"":""error"",""status_message"":""Prefix not found in BGP table or malformed"",""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""2388.59 ms""}}");
        }

        public Task<JsonDocument> RetrievePrefixDetailsAsync(string prefix, byte cidr)
            => Task.FromResult(RetrievePrefixDetails(prefix, cidr));

        public JsonDocument RetrieveSearchBy(string queryTerm)
        {
            if(queryTerm == "53181")
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asns"":[{""asn"":53181,""name"":null,""description"":null,""country_code"":null,""email_contacts"":[],""abuse_contacts"":[],""rir_name"":""Lacnic""}],""ipv4_prefixes"":[],""ipv6_prefixes"":[],""internet_exchanges"":[]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""699.19 ms""}}");
            if(queryTerm == "empty")
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asns"":[],""ipv4_prefixes"":[],""ipv6_prefixes"":[],""internet_exchanges"":[]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""402.39 ms""}}");
            if(queryTerm == "3356")
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asns"":[{""asn"":3356,""name"":""LEVEL3"",""description"":""Level 3 Parent, LLC"",""country_code"":""US"",""email_contacts"":[""ipaddressing@level3.com""],""abuse_contacts"":[""ipaddressing@level3.com""],""rir_name"":""ARIN""}],""ipv4_prefixes"":[{""prefix"":""12.130.205.0\/24"",""ip"":""12.130.205.0"",""cidr"":24,""name"":""ATTWH-12-130-205-0-24-1011223356"",""country_code"":""US"",""description"":""CI - Macsteel Service Centers Usa, Inc SID-18233"",""email_contacts"":[""rm-mhops-ap@intl.att.com"",""abuse@att.net""],""abuse_contacts"":[""abuse@att.net""],""rir_name"":""ARIN"",""parent_prefix"":""12.0.0.0\/8"",""parent_ip"":""12.0.0.0"",""parent_cidr"":8},{""prefix"":""65.51.86.0\/24"",""ip"":""65.51.86.0"",""cidr"":24,""name"":""CVNET-41335600"",""country_code"":""US"",""description"":""Christie's Inc"",""email_contacts"":[""hostmaster@cv.net"",""abuse@cv.net""],""abuse_contacts"":[""abuse@cv.net""],""rir_name"":""ARIN"",""parent_prefix"":""65.51.0.0\/16"",""parent_ip"":""65.51.0.0"",""parent_cidr"":16}],""ipv6_prefixes"":[],""internet_exchanges"":[]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""483.48 ms""}}");
            return JsonDocument.Parse(@"{""status"":""error"",""status_message"":""Out of testing scope"",""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""2388.59 ms""}}");
        }

        public Task<JsonDocument> RetrieveSearchByAsync(string queryTerm)
        {
            throw new System.NotImplementedException();
        }
    }
}