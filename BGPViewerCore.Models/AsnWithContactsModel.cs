using System.Collections.Generic;

namespace BGPViewerCore.Models
{
    public class AsnWithContactsModel : AsnModel
    {
        public IEnumerable<string> EmailContacts { get; set; }
        public IEnumerable<string> AbuseContacts { get; set; }
    }
}