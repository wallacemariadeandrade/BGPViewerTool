using System.Collections.Generic;
using BGPViewerCore.Model;
using BGPViewerCore.Service;
using System.Linq;

namespace Xunit
{
    public class BGPViewerServiceTest
    {
        [Fact]
        public void GetAsnDetais()
        {
            // Mocked from API
            var expected = new AsnDetailsModel {
                ASN = 6762,
                Name = "SEABONE-NET",
                DescriptionShort = "TELECOM ITALIA SPARKLE S.p.A.",
                EmailContacts = new List<string> {
                    "abuse@seabone.net",
                    "peering@seabone.net",
                    "tech@seabone.net",
                    "francesco.chiappini@telecomitalia.it"
                },
                AbuseContacts = new List<string> {
                    "abuse@seabone.net"
                },
                LookingGlassUrl = "https://gambadilegno.noc.seabone.net/lg/",
                CountryCode = "IT"
            };

            var asnDetails = new BGPViewerService().GetAsnDetails(6762);
            
            Assert.Equal(expected.ASN, asnDetails.ASN);
            Assert.Equal(expected.DescriptionShort, asnDetails.DescriptionShort);
            Assert.Equal(expected.Name, asnDetails.Name);
            Assert.Equal(expected.EmailContacts.ElementAt(0), asnDetails.EmailContacts.ElementAt(0));
            Assert.Equal(expected.EmailContacts.ElementAt(1), asnDetails.EmailContacts.ElementAt(1));
            Assert.Equal(expected.EmailContacts.ElementAt(2), asnDetails.EmailContacts.ElementAt(2));
            Assert.Equal(expected.EmailContacts.ElementAt(3), asnDetails.EmailContacts.ElementAt(3));
            Assert.Equal(expected.AbuseContacts.ElementAt(0), asnDetails.AbuseContacts.ElementAt(0));
            Assert.Equal(expected.LookingGlassUrl, asnDetails.LookingGlassUrl);
            Assert.Equal(expected.CountryCode, asnDetails.CountryCode);
        }

        [Fact]
        public void GetAsnDetailsWhenSomePropertiesAreNull()
        {
            // Mock from API
            var expected = new AsnDetailsModel {
                ASN = 53181,
                Name = null,
                DescriptionShort = null,
                EmailContacts = new List<string> (),
                AbuseContacts = new List<string> (),
                LookingGlassUrl = null,
                CountryCode = "BR"
            };

            var asnDetails = new BGPViewerService().GetAsnDetails(53181);
            
            Assert.Equal(expected.ASN, asnDetails.ASN);
            Assert.Equal(expected.DescriptionShort, asnDetails.DescriptionShort);
            Assert.Equal(expected.Name, asnDetails.Name);
            Assert.Equal(expected.EmailContacts.Count(), asnDetails.EmailContacts.Count());
            Assert.Equal(expected.LookingGlassUrl, asnDetails.LookingGlassUrl);
            Assert.Equal(expected.CountryCode, asnDetails.CountryCode);
        }
    }
}