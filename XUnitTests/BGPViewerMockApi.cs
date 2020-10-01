using System.Text.Json;
using BGPViewerCore.Service;

namespace Xunit
{
    public class BGPViewerMockApi : IBGPViewerApi
    {
        public JsonDocument RetrieveAsnDetails(int asNumber)
        {
            if(asNumber == 6762)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asn"":6762,""name"":""SEABONE-NET"",""description_short"":""TELECOM ITALIA SPARKLE S.p.A."",""description_full"":[""TELECOM ITALIA SPARKLE S.p.A.""],""country_code"":""IT"",""website"":""https:\/\/www.tisparkle.com\/"",""email_contacts"":[""abuse@seabone.net"",""peering@seabone.net"",""tech@seabone.net"",""francesco.chiappini@telecomitalia.it""],""abuse_contacts"":[""abuse@seabone.net""],""looking_glass"":""https:\/\/gambadilegno.noc.seabone.net\/lg\/"",""traffic_estimation"":""1-5Tbps"",""traffic_ratio"":""Balanced"",""owner_address"":[""Via Macchia Palocco 223"",""00125"",""Rome"",""ITALY""],""rir_allocation"":{""rir_name"":""RIPE"",""country_code"":""IT"",""date_allocated"":""1996-09-12 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":null,""description"":null,""whois_server"":null,""date_assigned"":null},""date_updated"":""2020-08-24 03:46:00""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""335.47 ms""}}");
            
            return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""asn"":53181,""name"":null,""description_short"":null,""description_full"":[],""country_code"":"""",""website"":null,""email_contacts"":[],""abuse_contacts"":[],""looking_glass"":null,""traffic_estimation"":null,""traffic_ratio"":null,""owner_address"":[],""rir_allocation"":{""rir_name"":""Lacnic"",""country_code"":""BR"",""date_allocated"":""2010-08-18 00:00:00"",""allocation_status"":""allocated""},""iana_assignment"":{""assignment_status"":null,""description"":null,""whois_server"":null,""date_assigned"":null},""date_updated"":""2020-08-22 04:57:28""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""307.7 ms""}}");
        }

        public JsonDocument RetrieveAsnPeers(int asNumber)
        {
            if(asNumber == 268374)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_peers"":[{""asn"":53181,""name"":null,""description"":null,""country_code"":""""},{""asn"":6939,""name"":""HURRICANE"",""description"":""Hurricane Electric LLC"",""country_code"":""US""},{""asn"":39533,""name"":""asympto"",""description"":""Asympto Networks Kft."",""country_code"":""CH""},{""asn"":60501,""name"":""SIRIUSTEC-IT"",""description"":""Sirius Technology SRL"",""country_code"":""IT""},{""asn"":13786,""name"":""SEABRAS-1"",""description"":""Seabras 1 USA, LLC"",""country_code"":""US""},{""asn"":28186,""name"":null,""description"":null,""country_code"":""""},{""asn"":23106,""name"":null,""description"":null,""country_code"":""""},{""asn"":52863,""name"":null,""description"":null,""country_code"":""""},{""asn"":52873,""name"":null,""description"":null,""country_code"":""""},{""asn"":267613,""name"":null,""description"":null,""country_code"":""""},{""asn"":52320,""name"":""GlobeNet Cabos Submarinos Colombia, S.A.S."",""description"":""GlobeNet Cabos Submarinos Colombia, S.A.S."",""country_code"":""CO""},{""asn"":41047,""name"":""NL-MLAB"",""description"":""MLAB Open Source Community"",""country_code"":""NL""},{""asn"":57463,""name"":""NetIX"",""description"":""NetIX Communications Ltd."",""country_code"":""BG""}],""ipv6_peers"":[{""asn"":53181,""name"":null,""description"":null,""country_code"":""""},{""asn"":6939,""name"":""HURRICANE"",""description"":""Hurricane Electric LLC"",""country_code"":""US""},{""asn"":57463,""name"":""NetIX"",""description"":""NetIX Communications Ltd."",""country_code"":""BG""},{""asn"":61597,""name"":null,""description"":null,""country_code"":""""},{""asn"":13786,""name"":""SEABRAS-1"",""description"":""Seabras 1 USA, LLC"",""country_code"":""US""},{""asn"":23106,""name"":null,""description"":null,""country_code"":""""},{""asn"":52873,""name"":null,""description"":null,""country_code"":""""},{""asn"":267613,""name"":null,""description"":null,""country_code"":""""}]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""84.23 ms""}}");
            
            if(asNumber == 264075)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_peers"":[{""asn"":53181,""name"":null,""description"":null,""country_code"":""""}],""ipv6_peers"":[{""asn"":53181,""name"":null,""description"":null,""country_code"":""""}]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""69.02 ms""}}");
            
            throw new System.Exception("Out of testing scope.");
        }

        public JsonDocument RetrieveAsnPrefixes(int asNumber)
        {
            if(asNumber == 264075)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_prefixes"":[{""prefix"":""143.208.20.0\/22"",""ip"":""143.208.20.0"",""cidr"":22,""roa_status"":""None"",""name"":""K1 Telecom e Multimidia LTDA"",""description"":""K1 Telecom e Multimidia LTDA"",""country_code"":""BR"",""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}}],""ipv6_prefixes"":[{""prefix"":""2804:2a7c::\/32"",""ip"":""2804:2a7c::"",""cidr"":32,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}}]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""185.29 ms""}}");
            
            if (asNumber == 268374)
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_prefixes"":[{""prefix"":""45.239.24.0\/22"",""ip"":""45.239.24.0"",""cidr"":22,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.24.0\/24"",""ip"":""45.239.24.0"",""cidr"":24,""roa_status"":""None"",""name"":""Telnet Servicos e Comi\u00c2\u00bf\u00c2\u00bdrcio em Informi\u00c2\u00bf\u00c2\u00bdtica Ltda."",""description"":""Telnet Servicos e Comi\u00c2\u00bf\u00c2\u00bdrcio em Informi\u00c2\u00bf\u00c2\u00bdtica Ltda."",""country_code"":""BR"",""parent"":{""prefix"":""45.239.24.0\/22"",""ip"":""45.239.24.0"",""cidr"":22,""rir_name"":""Lacnic"",""allocation_status"":""unknown""}},{""prefix"":""45.239.24.0\/23"",""ip"":""45.239.24.0"",""cidr"":23,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.25.0\/24"",""ip"":""45.239.25.0"",""cidr"":24,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.26.0\/24"",""ip"":""45.239.26.0"",""cidr"":24,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.26.0\/23"",""ip"":""45.239.26.0"",""cidr"":23,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}},{""prefix"":""45.239.27.0\/24"",""ip"":""45.239.27.0"",""cidr"":24,""roa_status"":""None"",""name"":null,""description"":null,""country_code"":null,""parent"":{""prefix"":null,""ip"":null,""cidr"":null,""rir_name"":null,""allocation_status"":""unknown""}}],""ipv6_prefixes"":[{""prefix"":""2804:5030::\/32"",""ip"":""2804:5030::"",""cidr"":32,""roa_status"":""None"",""name"":""Telnet Servicos e Comi\u00c2\u00bf\u00c2\u00bdrcio em Informi\u00c2\u00bf\u00c2\u00bdtica Ltda."",""description"":""Telnet Servicos e Comi\u00c2\u00bf\u00c2\u00bdrcio em Informi\u00c2\u00bf\u00c2\u00bdtica Ltda."",""country_code"":""BR"",""parent"":{""prefix"":""2804:5030::\/32"",""ip"":""2804:5030::"",""cidr"":32,""rir_name"":""Lacnic"",""allocation_status"":""unknown""}}]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""366.7 ms""}}");
            
            return JsonDocument.Parse(
                @"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_prefixes"":[{""prefix"":""103.121.176.0\/24"",""ip"":""103.121.176.0"",""cidr"":24,""roa_status"":""None"",""name"":""GARENATW-NET"",""description"":""Garena (TAIWAN) CO. LTD."",""country_code"":""TW"",""parent"":{""prefix"":""103.121.176.0\/22"",""ip"":""103.121.176.0"",""cidr"":22,""rir_name"":""APNIC"",""allocation_status"":""unknown""}},{""prefix"":""103.121.177.0\/24"",""ip"":""103.121.177.0"",""cidr"":24,""roa_status"":""None"",""name"":""GARENATW-NET"",""description"":""Garena (TAIWAN) CO. LTD."",""country_code"":""TW"",""parent"":{""prefix"":""103.121.176.0\/22"",""ip"":""103.121.176.0"",""cidr"":22,""rir_name"":""APNIC"",""allocation_status"":""unknown""}},{""prefix"":""103.121.178.0\/24"",""ip"":""103.121.178.0"",""cidr"":24,""roa_status"":""None"",""name"":""GARENATW-NET"",""description"":""Garena (TAIWAN) CO. LTD."",""country_code"":""TW"",""parent"":{""prefix"":""103.121.176.0\/22"",""ip"":""103.121.176.0"",""cidr"":22,""rir_name"":""APNIC"",""allocation_status"":""unknown""}}],""ipv6_prefixes"":[]},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""159.15 ms""}}"
            );
        }

        public JsonDocument RetrieveAsnUpstreams(int asNumber)
        {
            if(asNumber == 52575) 
                return JsonDocument.Parse(@"{""status"":""ok"",""status_message"":""Query was successful"",""data"":{""ipv4_upstreams"":[{""asn"":265185,""name"":null,""description"":null,""country_code"":""""}],""ipv6_upstreams"":[],""ipv4_graph"":""https:\/\/api.bgpview.io\/assets\/graphs\/AS52575_IPv4.svg"",""ipv6_graph"":""https:\/\/api.bgpview.io\/assets\/graphs\/AS52575_IPv6.svg"",""combined_graph"":""https:\/\/api.bgpview.io\/assets\/graphs\/AS52575_Combined.svg""},""@meta"":{""time_zone"":""UTC"",""api_version"":1,""execution_time"":""141 ms""}}");
            throw new System.Exception("Out of testing scope.");
        }
    }
}