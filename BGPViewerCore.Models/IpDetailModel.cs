using System.Collections.Generic;

namespace BGPViewerCore.Models
{
    public class IpDetailModel
    {
        public string IPAddress { get; set; }
        public string RIRAllocationPrefix { get; set; }
        public string CountryCode { get; set; }
        public string PtrRecord { get; set; }
        public IEnumerable<PrefixDetailModel> RelatedPrefixes { get; set; }
    }
}