using System.Text.Json;
using System.Threading.Tasks;
using BGPViewerCore.Service;

namespace BGPViewerCore.UnitTests.PeeringDbServiceTests
{
    public class PeeringDbMockWebApi : IBGPViewerApi
    {
        public JsonDocument RetrieveAsnDetails(int asNumber)
        {
            if(asNumber == 16509)
                return JsonDocument.Parse(@"{""data"": [{""id"": 1418, ""org_id"": 1520, ""name"": ""Amazon.com"", ""aka"": ""Amazon Web Services"", ""name_long"": """", ""website"": ""http://www.amazon.com"", ""asn"": 16509, ""looking_glass"": """", ""route_server"": """", ""irr_as_set"": ""AS-AMAZON"", ""info_type"": ""Enterprise"", ""info_prefixes4"": 6000, ""info_prefixes6"": 2000, ""info_traffic"": """", ""info_ratio"": ""Balanced"", ""info_scope"": ""Global"", ""info_unicast"": true, ""info_multicast"": false, ""info_ipv6"": true, ""info_never_via_route_servers"": false, ""ix_count"": 129, ""fac_count"": 133, ""notes"": ""AWS Peering: https://peering.aws/\n\n\nPeering requests:\n\nWhen submitting a peering request, please address the specific regional contact listed below for the location of your request (i.e. peering requests for London should use peering-emea@amazon.com while peering requests for Singapore should use peering-apac@amazon.com). This will ensure your request is processed and addressed in a timely fashion. Please do not copy contacts not meant for peering policy in the location of your request.\n\n-----\n\nOperational issues:\n\nIf you experience connectivity issues to Amazon, please first visit: http://ec2-reachability.amazonaws.com/.\nWhen contacting our Operational alias below, please include trace/debug information (source and destination address) and details for the affected prefixes. This will reduce troubleshooting time. Please do not contact policy addresses for operational support.\n\n----\n\nInformational:\n\nFor details about our network, routing and interconnection options, please see https://peering.aws.\nA list of our IP ranges with source location information is maintained here: https://ip-ranges.amazonaws.com/ip-ranges.json.\nPlease use the Maintenance alias to notify us regarding maintenance activities on your end and do not copy other aliases into these inquiries.  \n\n----\n\nRPKI:\n\nAWS drops all routes deemed to be RPKI invalid on incoming eBGP sessions."", ""netixlan_updated"": ""2021-12-06T16:51:30.597577Z"", ""netfac_updated"": ""2021-11-23T16:21:38.116605Z"", ""poc_updated"": ""2020-12-01T12:29:55Z"", ""policy_url"": ""https://aws.amazon.com/peering/policy"", ""policy_general"": ""Selective"", ""policy_locations"": ""Preferred"", ""policy_ratio"": false, ""policy_contracts"": ""Not Required"", ""allow_ixp_update"": false, ""created"": ""2007-12-20T16:39:54Z"", ""updated"": ""2021-10-20T12:54:19Z"", ""status"": ""ok""}], ""meta"": {}}");
            if(asNumber == 53181)
                return JsonDocument.Parse(@"{""data"": [{""id"": 10605, ""org_id"": 14463, ""name"": ""K2 Telecom e Multimidia"", ""aka"": ""K2TELECOM"", ""name_long"": ""K2 Telecom e Multimidia LTDA ME"", ""website"": ""https://www.k2telecom.com.br"", ""asn"": 53181, ""looking_glass"": ""https://lg.k2telecom.net.br/"", ""route_server"": ""https://bgp.he.net/AS53181"", ""irr_as_set"": ""AS-K2TELECOM"", ""info_type"": ""NSP"", ""info_prefixes4"": 800, ""info_prefixes6"": 800, ""info_traffic"": ""300-500Gbps"", ""info_ratio"": ""Heavy Inbound"", ""info_scope"": ""Regional"", ""info_unicast"": true, ""info_multicast"": false, ""info_ipv6"": true, ""info_never_via_route_servers"": false, ""ix_count"": 3, ""fac_count"": 8, ""notes"": ""* MANRS for Network Operators *\n\nhttps://www.manrs.org/isps/participants/?gv_search=53181&mode=any"", ""netixlan_updated"": ""2021-08-19T14:21:48.872261Z"", ""netfac_updated"": ""2021-09-10T15:44:55.409125Z"", ""poc_updated"": ""2021-08-19T14:20:38.856675Z"", ""policy_url"": """", ""policy_general"": ""Selective"", ""policy_locations"": ""Not Required"", ""policy_ratio"": false, ""policy_contracts"": ""Not Required"", ""allow_ixp_update"": false, ""created"": ""2016-06-22T05:38:18Z"", ""updated"": ""2021-12-06T15:04:28Z"", ""status"": ""ok""}], ""meta"": {}}");
            return JsonDocument.Parse(@"{""data"": [], ""meta"": {}}");
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
            throw new System.NotImplementedException();
        }

        public Task<JsonDocument> RetrieveAsnIxsAsync(int asNumber)
        {
            throw new System.NotImplementedException();
        }

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