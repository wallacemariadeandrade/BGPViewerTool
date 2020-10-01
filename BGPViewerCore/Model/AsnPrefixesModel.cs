using System.Collections.Generic;

namespace BGPViewerCore.Service
{
    public class AsnPrefixesModel
    {
        public int ASN { get; set; }
        public IEnumerable<string> IPv4 { get; set; }
        public IEnumerable<string> IPv6 { get; set; }
    }
}