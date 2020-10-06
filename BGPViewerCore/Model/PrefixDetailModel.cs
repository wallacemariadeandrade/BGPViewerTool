using System.Collections.Generic;

namespace BGPViewerCore.Model
{
    public class PrefixDetailModel
    {
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<AsnModel> ParentAsns { get; set; }
    }
}