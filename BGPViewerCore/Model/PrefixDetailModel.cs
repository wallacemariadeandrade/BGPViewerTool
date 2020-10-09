using System.Collections.Generic;

namespace BGPViewerCore.Model
{
    public class PrefixDetailModel : PrefixModel
    {
        public IEnumerable<AsnModel> ParentAsns { get; set; }
    }
}