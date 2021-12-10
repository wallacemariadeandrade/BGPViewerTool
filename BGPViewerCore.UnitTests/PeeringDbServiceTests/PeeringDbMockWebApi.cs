using System.Text.Json;
using System.Threading.Tasks;
using BGPViewerCore.Service;

namespace BGPViewerCore.UnitTests.PeeringDbServiceTests
{
    public class PeeringDbMockWebApi : IBGPViewerApi
    {
        private JsonDocument Empty() => JsonDocument.Parse(@"{""data"": [], ""meta"": {}}");
        public JsonDocument RetrieveAsnDetails(int asNumber)
        {
            if(asNumber == 16509)
                return JsonDocument.Parse(@"{""data"": [{""id"": 1418, ""org_id"": 1520, ""name"": ""Amazon.com"", ""aka"": ""Amazon Web Services"", ""name_long"": """", ""website"": ""http://www.amazon.com"", ""asn"": 16509, ""looking_glass"": """", ""route_server"": """", ""irr_as_set"": ""AS-AMAZON"", ""info_type"": ""Enterprise"", ""info_prefixes4"": 6000, ""info_prefixes6"": 2000, ""info_traffic"": """", ""info_ratio"": ""Balanced"", ""info_scope"": ""Global"", ""info_unicast"": true, ""info_multicast"": false, ""info_ipv6"": true, ""info_never_via_route_servers"": false, ""ix_count"": 129, ""fac_count"": 133, ""notes"": ""AWS Peering: https://peering.aws/\n\n\nPeering requests:\n\nWhen submitting a peering request, please address the specific regional contact listed below for the location of your request (i.e. peering requests for London should use peering-emea@amazon.com while peering requests for Singapore should use peering-apac@amazon.com). This will ensure your request is processed and addressed in a timely fashion. Please do not copy contacts not meant for peering policy in the location of your request.\n\n-----\n\nOperational issues:\n\nIf you experience connectivity issues to Amazon, please first visit: http://ec2-reachability.amazonaws.com/.\nWhen contacting our Operational alias below, please include trace/debug information (source and destination address) and details for the affected prefixes. This will reduce troubleshooting time. Please do not contact policy addresses for operational support.\n\n----\n\nInformational:\n\nFor details about our network, routing and interconnection options, please see https://peering.aws.\nA list of our IP ranges with source location information is maintained here: https://ip-ranges.amazonaws.com/ip-ranges.json.\nPlease use the Maintenance alias to notify us regarding maintenance activities on your end and do not copy other aliases into these inquiries.  \n\n----\n\nRPKI:\n\nAWS drops all routes deemed to be RPKI invalid on incoming eBGP sessions."", ""netixlan_updated"": ""2021-12-06T16:51:30.597577Z"", ""netfac_updated"": ""2021-11-23T16:21:38.116605Z"", ""poc_updated"": ""2020-12-01T12:29:55Z"", ""policy_url"": ""https://aws.amazon.com/peering/policy"", ""policy_general"": ""Selective"", ""policy_locations"": ""Preferred"", ""policy_ratio"": false, ""policy_contracts"": ""Not Required"", ""allow_ixp_update"": false, ""created"": ""2007-12-20T16:39:54Z"", ""updated"": ""2021-10-20T12:54:19Z"", ""status"": ""ok""}], ""meta"": {}}");
            if(asNumber == 53181)
                return JsonDocument.Parse(@"{""data"": [{""id"": 10605, ""org_id"": 14463, ""name"": ""K2 Telecom e Multimidia"", ""aka"": ""K2TELECOM"", ""name_long"": ""K2 Telecom e Multimidia LTDA ME"", ""website"": ""https://www.k2telecom.com.br"", ""asn"": 53181, ""looking_glass"": ""https://lg.k2telecom.net.br/"", ""route_server"": ""https://bgp.he.net/AS53181"", ""irr_as_set"": ""AS-K2TELECOM"", ""info_type"": ""NSP"", ""info_prefixes4"": 800, ""info_prefixes6"": 800, ""info_traffic"": ""300-500Gbps"", ""info_ratio"": ""Heavy Inbound"", ""info_scope"": ""Regional"", ""info_unicast"": true, ""info_multicast"": false, ""info_ipv6"": true, ""info_never_via_route_servers"": false, ""ix_count"": 3, ""fac_count"": 8, ""notes"": ""* MANRS for Network Operators *\n\nhttps://www.manrs.org/isps/participants/?gv_search=53181&mode=any"", ""netixlan_updated"": ""2021-08-19T14:21:48.872261Z"", ""netfac_updated"": ""2021-09-10T15:44:55.409125Z"", ""poc_updated"": ""2021-08-19T14:20:38.856675Z"", ""policy_url"": """", ""policy_general"": ""Selective"", ""policy_locations"": ""Not Required"", ""policy_ratio"": false, ""policy_contracts"": ""Not Required"", ""allow_ixp_update"": false, ""created"": ""2016-06-22T05:38:18Z"", ""updated"": ""2021-12-06T15:04:28Z"", ""status"": ""ok""}], ""meta"": {}}");
            return Empty();
        }

        public Task<JsonDocument> RetrieveAsnDetailsAsync(int asNumber)
            => Task.FromResult(RetrieveAsnDetails(asNumber));

        public JsonDocument RetrieveAsnDownstreams(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnDownstreamsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnIxs(int asNumber)
        {
            if(asNumber == 60068)
                return JsonDocument.Parse(@"{""data"": [{""id"": 67926, ""net_id"": 10839, ""ix_id"": 100, ""name"": ""MSK-IX Moscow: MSK-IX peering network"", ""ixlan_id"": 100, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""195.208.210.117"", ""ipaddr6"": ""2001:7f8:20:101::210:117"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-06-15T09:30:28Z"", ""updated"": ""2021-06-15T09:30:28Z"", ""status"": ""ok""}, {""id"": 65361, ""net_id"": 10839, ""ix_id"": 119, ""name"": ""Equinix S\u00e3o Paulo: Equinix IX - SP Metro"", ""ixlan_id"": 119, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""64.191.233.100"", ""ipaddr6"": ""2001:504:0:7:0:6:68:1"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-03-15T16:00:17Z"", ""updated"": ""2021-03-15T16:00:17Z"", ""status"": ""ok""}, {""id"": 57101, ""net_id"": 10839, ""ix_id"": 125, ""name"": ""Equinix Hong Kong"", ""ixlan_id"": 125, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""36.255.56.106"", ""ipaddr6"": ""2001:de8:7::6:68:1"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2020-04-15T14:57:43Z"", ""updated"": ""2020-11-25T15:13:37Z"", ""status"": ""ok""}, {""id"": 31313, ""net_id"": 10839, ""ix_id"": 158, ""name"": ""Equinix Singapore"", ""ixlan_id"": 158, ""notes"": """", ""speed"": 200000, ""asn"": 60068, ""ipaddr4"": ""27.111.229.28"", ""ipaddr6"": ""2001:de8:4::6:68:1"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2016-11-07T19:22:20Z"", ""updated"": ""2021-03-01T13:28:23Z"", ""status"": ""ok""}, {""id"": 63702, ""net_id"": 10839, ""ix_id"": 158, ""name"": ""Equinix Singapore"", ""ixlan_id"": 158, ""notes"": """", ""speed"": 200000, ""asn"": 60068, ""ipaddr4"": ""27.111.230.50"", ""ipaddr6"": ""2001:de8:4::6:68:2"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-01-04T12:04:40Z"", ""updated"": ""2021-03-01T13:28:23Z"", ""status"": ""ok""}, {""id"": 23682, ""net_id"": 10839, ""ix_id"": 167, ""name"": ""Equinix Tokyo"", ""ixlan_id"": 167, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""203.190.230.12"", ""ipaddr6"": ""2001:de8:5::6:68:1"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2015-07-02T00:00:00Z"", ""updated"": ""2020-10-19T15:47:15Z"", ""status"": ""ok""}, {""id"": 62893, ""net_id"": 10839, ""ix_id"": 171, ""name"": ""IX.br (PTT.br) S\u00e3o Paulo: ATM/MPLA"", ""ixlan_id"": 171, ""notes"": """", ""speed"": 300000, ""asn"": 60068, ""ipaddr4"": ""187.16.210.94"", ""ipaddr6"": ""2001:12f8::210:94"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2020-11-26T10:26:26Z"", ""updated"": ""2021-03-15T16:00:33Z"", ""status"": ""ok""}, {""id"": 70564, ""net_id"": 10839, ""ix_id"": 326, ""name"": ""B-IX"", ""ixlan_id"": 326, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""185.1.30.77"", ""ipaddr6"": null, ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-10-05T10:57:29Z"", ""updated"": ""2021-10-05T10:57:29Z"", ""status"": ""ok""}, {""id"": 71114, ""net_id"": 10839, ""ix_id"": 327, ""name"": ""DTEL-IX: PUBLIC"", ""ixlan_id"": 327, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""193.25.181.202"", ""ipaddr6"": ""2001:7f8:63::1:ca"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-11-02T08:35:06Z"", ""updated"": ""2021-11-02T08:35:18Z"", ""status"": ""ok""}, {""id"": 66931, ""net_id"": 10839, ""ix_id"": 358, ""name"": ""DATAIX"", ""ixlan_id"": 358, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""178.18.227.42"", ""ipaddr6"": ""2a03:5f80:4::227:42"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-05-05T13:28:03Z"", ""updated"": ""2021-05-05T13:28:03Z"", ""status"": ""ok""}, {""id"": 61471, ""net_id"": 10839, ""ix_id"": 429, ""name"": ""SGIX: Peering LAN"", ""ixlan_id"": 429, ""notes"": """", ""speed"": 20000, ""asn"": 60068, ""ipaddr4"": ""103.16.102.185"", ""ipaddr6"": ""2001:de8:12:100::185"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2020-10-10T15:01:12Z"", ""updated"": ""2021-01-04T12:03:38Z"", ""status"": ""ok""}, {""id"": 70906, ""net_id"": 10839, ""ix_id"": 481, ""name"": ""THINX Warsaw"", ""ixlan_id"": 481, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""212.91.0.106"", ""ipaddr6"": ""2001:7f8:60::2:9535:1"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-10-22T13:12:07Z"", ""updated"": ""2021-10-22T13:12:07Z"", ""status"": ""ok""}, {""id"": 70764, ""net_id"": 10839, ""ix_id"": 655, ""name"": ""GigaNET Kiev: Global exchange"", ""ixlan_id"": 655, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""185.1.63.212"", ""ipaddr6"": ""2001:7f8:6c::467"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-10-15T13:37:41Z"", ""updated"": ""2021-10-15T13:37:41Z"", ""status"": ""ok""}, {""id"": 70563, ""net_id"": 10839, ""ix_id"": 699, ""name"": ""NetIX: NetIX"", ""ixlan_id"": 699, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""193.218.0.135"", ""ipaddr6"": ""2001:67c:29f0::6:68:1"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-10-05T10:53:25Z"", ""updated"": ""2021-10-05T10:53:25Z"", ""status"": ""ok""}, {""id"": 30349, ""net_id"": 10839, ""ix_id"": 713, ""name"": ""Peering.cz"", ""ixlan_id"": 713, ""notes"": """", ""speed"": 200000, ""asn"": 60068, ""ipaddr4"": ""91.213.211.13"", ""ipaddr6"": ""2001:7f8:7f::13"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2016-09-08T15:26:10Z"", ""updated"": ""2020-10-12T11:43:06Z"", ""status"": ""ok""}, {""id"": 34861, ""net_id"": 10839, ""ix_id"": 713, ""name"": ""Peering.cz"", ""ixlan_id"": 713, ""notes"": """", ""speed"": 200000, ""asn"": 60068, ""ipaddr4"": ""91.213.211.105"", ""ipaddr6"": ""2001:7f8:7f::105"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2017-05-24T07:59:13Z"", ""updated"": ""2020-10-12T11:43:06Z"", ""status"": ""ok""}, {""id"": 58561, ""net_id"": 10839, ""ix_id"": 713, ""name"": ""Peering.cz"", ""ixlan_id"": 713, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""91.213.211.27"", ""ipaddr6"": ""2001:7f8:7f::27"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2020-06-11T14:53:48Z"", ""updated"": ""2020-10-19T19:12:01Z"", ""status"": ""ok""}, {""id"": 58562, ""net_id"": 10839, ""ix_id"": 713, ""name"": ""Peering.cz"", ""ixlan_id"": 713, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""91.213.211.192"", ""ipaddr6"": ""2001:7f8:7f::192"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2020-06-11T14:56:01Z"", ""updated"": ""2020-10-19T19:12:01Z"", ""status"": ""ok""}, {""id"": 58563, ""net_id"": 10839, ""ix_id"": 713, ""name"": ""Peering.cz"", ""ixlan_id"": 713, ""notes"": """", ""speed"": 40000, ""asn"": 60068, ""ipaddr4"": ""91.213.211.29"", ""ipaddr6"": ""2001:7f8:7f::29"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2020-06-11T14:56:33Z"", ""updated"": ""2020-06-11T14:56:33Z"", ""status"": ""ok""}, {""id"": 58564, ""net_id"": 10839, ""ix_id"": 713, ""name"": ""Peering.cz"", ""ixlan_id"": 713, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""91.213.211.28"", ""ipaddr6"": ""2001:7f8:7f::28"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2020-06-11T14:57:39Z"", ""updated"": ""2020-06-11T14:57:39Z"", ""status"": ""ok""}, {""id"": 66299, ""net_id"": 10839, ""ix_id"": 713, ""name"": ""Peering.cz"", ""ixlan_id"": 713, ""notes"": """", ""speed"": 200000, ""asn"": 60068, ""ipaddr4"": ""91.213.211.200"", ""ipaddr6"": ""2001:7f8:7f::200"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-04-13T08:27:38Z"", ""updated"": ""2021-04-13T08:27:38Z"", ""status"": ""ok""}, {""id"": 52107, ""net_id"": 10839, ""ix_id"": 1308, ""name"": ""LSIX: Main"", ""ixlan_id"": 1308, ""notes"": """", ""speed"": 40000, ""asn"": 60068, ""ipaddr4"": ""185.1.32.46"", ""ipaddr6"": ""2001:7f8:8f::a560:68:1"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2019-09-12T08:50:42Z"", ""updated"": ""2020-10-19T19:13:25Z"", ""status"": ""ok""}, {""id"": 39132, ""net_id"": 10839, ""ix_id"": 1842, ""name"": ""Speed-IX: SPEED-IX"", ""ixlan_id"": 1842, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""185.1.95.47"", ""ipaddr6"": ""2001:7f8:b7::a506:68:1"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2018-01-19T15:48:14Z"", ""updated"": ""2020-02-19T04:08:21Z"", ""status"": ""ok""}, {""id"": 70210, ""net_id"": 10839, ""ix_id"": 1977, ""name"": ""CLOUD-IX MSK"", ""ixlan_id"": 1977, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""31.28.19.120"", ""ipaddr6"": ""2a00:13c0:3:1::1f1c:1378"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-09-17T14:16:29Z"", ""updated"": ""2021-09-17T14:16:29Z"", ""status"": ""ok""}, {""id"": 70209, ""net_id"": 10839, ""ix_id"": 1981, ""name"": ""CLOUD-IX KIEV"", ""ixlan_id"": 1981, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""31.28.16.25"", ""ipaddr6"": ""2a00:13c0:3:3::1f1c:1019"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-09-17T14:15:38Z"", ""updated"": ""2021-09-17T14:15:38Z"", ""status"": ""ok""}, {""id"": 50214, ""net_id"": 10839, ""ix_id"": 2494, ""name"": ""CSL Thai-IX Singapore: Main"", ""ixlan_id"": 2494, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""27.254.19.252"", ""ipaddr6"": ""2404:b0:13:b:0:6:68:1"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2019-06-13T10:38:45Z"", ""updated"": ""2020-02-19T04:09:16Z"", ""status"": ""ok""}, {""id"": 63421, ""net_id"": 10839, ""ix_id"": 2761, ""name"": ""EdgeIX - Sydney: Main"", ""ixlan_id"": 2761, ""notes"": """", ""speed"": 10000, ""asn"": 60068, ""ipaddr4"": ""202.77.88.64"", ""ipaddr6"": ""2001:df0:680:5::40"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2020-12-17T11:03:25Z"", ""updated"": ""2020-12-17T11:03:25Z"", ""status"": ""ok""}, {""id"": 66655, ""net_id"": 10839, ""ix_id"": 3149, ""name"": ""IXPlay Global Peers"", ""ixlan_id"": 3149, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""185.1.90.40"", ""ipaddr6"": null, ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-04-23T14:54:40Z"", ""updated"": ""2021-04-23T14:54:40Z"", ""status"": ""ok""}, {""id"": 70539, ""net_id"": 10839, ""ix_id"": 3562, ""name"": ""1-IX Internet Exchange"", ""ixlan_id"": 3562, ""notes"": """", ""speed"": 100000, ""asn"": 60068, ""ipaddr4"": ""185.1.213.45"", ""ipaddr6"": ""2001:7f8:115::45"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-10-04T10:39:43Z"", ""updated"": ""2021-10-05T10:52:56Z"", ""status"": ""ok""}], ""meta"": {}}");
            if(asNumber == 53181)
                return JsonDocument.Parse(@"{""data"": [{""id"": 30017, ""net_id"": 10605, ""ix_id"": 171, ""name"": ""IX.br (PTT.br) S\u00e3o Paulo: ATM/MPLA"", ""ixlan_id"": 171, ""notes"": """", ""speed"": 100000, ""asn"": 53181, ""ipaddr4"": ""187.16.222.222"", ""ipaddr6"": ""2001:12f8::222:222"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2016-08-18T17:22:36Z"", ""updated"": ""2021-08-26T12:40:18Z"", ""status"": ""ok""}, {""id"": 68197, ""net_id"": 10605, ""ix_id"": 171, ""name"": ""IX.br (PTT.br) S\u00e3o Paulo: ATM/MPLA"", ""ixlan_id"": 171, ""notes"": """", ""speed"": 100000, ""asn"": 53181, ""ipaddr4"": ""187.16.212.67"", ""ipaddr6"": ""2001:12f8::212:67"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-06-24T19:29:15Z"", ""updated"": ""2021-08-19T14:21:49Z"", ""status"": ""ok""}, {""id"": 30018, ""net_id"": 10605, ""ix_id"": 177, ""name"": ""IX.br (PTT.br) Rio de Janeiro: ATM/MPLA"", ""ixlan_id"": 177, ""notes"": """", ""speed"": 20000, ""asn"": 53181, ""ipaddr4"": ""45.6.52.153"", ""ipaddr6"": ""2001:12f8:0:2::153"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2016-08-18T17:23:58Z"", ""updated"": ""2017-12-27T11:42:33Z"", ""status"": ""ok""}, {""id"": 35091, ""net_id"": 10605, ""ix_id"": 177, ""name"": ""IX.br (PTT.br) Rio de Janeiro: ATM/MPLA"", ""ixlan_id"": 177, ""notes"": """", ""speed"": 100000, ""asn"": 53181, ""ipaddr4"": ""45.6.52.210"", ""ipaddr6"": ""2001:12f8:0:2::210"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2017-06-06T19:21:09Z"", ""updated"": ""2021-03-03T15:24:30Z"", ""status"": ""ok""}, {""id"": 44890, ""net_id"": 10605, ""ix_id"": 177, ""name"": ""IX.br (PTT.br) Rio de Janeiro: ATM/MPLA"", ""ixlan_id"": 177, ""notes"": """", ""speed"": 100000, ""asn"": 53181, ""ipaddr4"": ""45.6.53.32"", ""ipaddr6"": ""2001:12f8:0:2::53:32"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2018-10-31T18:09:28Z"", ""updated"": ""2020-03-06T02:02:12Z"", ""status"": ""ok""}, {""id"": 67244, ""net_id"": 10605, ""ix_id"": 706, ""name"": ""IX.br (PTT.br) Vit\u00f3ria: ATM/MPLA"", ""ixlan_id"": 706, ""notes"": """", ""speed"": 10000, ""asn"": 53181, ""ipaddr4"": ""187.16.194.72"", ""ipaddr6"": ""2001:12f8:0:17::72"", ""is_rs_peer"": true, ""operational"": true, ""created"": ""2021-05-17T15:16:54Z"", ""updated"": ""2021-05-17T15:17:23Z"", ""status"": ""ok""}], ""meta"": {}}");
            return Empty();
        }

        public Task<JsonDocument> RetrieveAsnIxsAsync(int asNumber)
            => Task.FromResult(RetrieveAsnIxs(asNumber));

        public JsonDocument RetrieveAsnPeers(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnPeersAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnPrefixes(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnPrefixesAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveAsnUpstreams(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnUpstreamsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveIpDetails(string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveIpDetailsAsync(string ipAddress)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrievePrefixDetails(string prefix, byte cidr)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrievePrefixDetailsAsync(string prefix, byte cidr)
        {
            throw new System.NotImplementedException();
        }

        public JsonDocument RetrieveSearchBy(string queryTerm)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveSearchByAsync(string queryTerm)
        {
            throw new System.NotImplementedException();
        }
    }
}