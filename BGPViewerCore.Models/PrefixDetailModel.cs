using System.Collections.Generic;

namespace BGPViewerCore.Models
{
    public class PrefixDetailModel : PrefixModel
    {
        public IEnumerable<AsnModel> ParentAsns { get; set; }
    }
}