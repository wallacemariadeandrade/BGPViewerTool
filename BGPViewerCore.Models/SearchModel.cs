using System.Collections.Generic;

namespace BGPViewerCore.Models
{
    public class SearchModel
    {
        public IEnumerable<AsnWithContactsModel> RelatedAsns { get; set; }
        public IEnumerable<PrefixModel> IPv4 { get; set; }
        public IEnumerable<PrefixModel> IPv6 { get; set; }
    }
}