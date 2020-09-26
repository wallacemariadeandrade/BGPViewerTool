using System.Collections.Generic;

namespace BGPViewerCore.Model
{
    public class AsnDetailsModel
    {   
        public int ASN { get; set; }
        public string Name { get; set; }
        public string DescriptionShort { get; set; }
        public IEnumerable<string> EmailContacts { get; set; }
        public IEnumerable<string> AbuseContacts { get; set; }
        public string LookingGlassUrl { get; set; }
        public string CountryCode { get; set; }
    }
}