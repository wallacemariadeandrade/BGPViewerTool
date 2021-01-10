using System.Collections.Generic;
using BGPViewerCore.Models;

namespace BGPViewerOpenApi.Model
{
    public class Peers
    {
        public IEnumerable<AsnModel> IPv4 { get; set; }
        public IEnumerable<AsnModel> IPv6 { get; set; }
    }
}