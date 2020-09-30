using System.Collections.Generic;

namespace BGPViewerCore.Model
{
    public class AsnPeersModel
    {
        public int ASN { get; set; }
        public IEnumerable<AsnInfo> IPv4Peers { get; set; }
        public IEnumerable<AsnInfo> IPv6Peers { get; set; }
    }
}