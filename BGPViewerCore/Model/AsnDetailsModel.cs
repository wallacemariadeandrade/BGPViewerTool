using System.Collections.Generic;

namespace BGPViewerCore.Model
{
    public class AsnDetailsModel : AsnModel
    {   
        public IEnumerable<string> EmailContacts { get; set; }
        public IEnumerable<string> AbuseContacts { get; set; }
        public string LookingGlassUrl { get; set; }
    }
}