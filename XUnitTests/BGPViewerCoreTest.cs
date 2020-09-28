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
            // Mocked Data
            var asnDetails = new BGPViewerService(new BGPViewerMockApi())
                .GetAsnDetails(6762);
            
            Assert.Equal(6762, asnDetails.ASN);
            Assert.Equal("TELECOM ITALIA SPARKLE S.p.A.", asnDetails.DescriptionShort);
            Assert.Equal("SEABONE-NET", asnDetails.Name);
            Assert.Equal("abuse@seabone.net", asnDetails.EmailContacts.ElementAt(0));
            Assert.Equal("peering@seabone.net", asnDetails.EmailContacts.ElementAt(1));
            Assert.Equal("tech@seabone.net", asnDetails.EmailContacts.ElementAt(2));
            Assert.Equal("francesco.chiappini@telecomitalia.it", asnDetails.EmailContacts.ElementAt(3));
            Assert.Equal("abuse@seabone.net", asnDetails.AbuseContacts.ElementAt(0));
            Assert.Equal("https://gambadilegno.noc.seabone.net/lg/", asnDetails.LookingGlassUrl);
            Assert.Equal("IT", asnDetails.CountryCode);
        }

        [Fact]
        public void GetAsnDetailsWhenSomePropertiesAreNull()
        {
            // Mocked Data
            var asnDetails = new BGPViewerService(new BGPViewerMockApi())
                .GetAsnDetails(53181);
            
            Assert.Equal(53181, asnDetails.ASN);
            Assert.Equal(null, asnDetails.DescriptionShort);
            Assert.Equal(null, asnDetails.Name);
            Assert.Equal(0, asnDetails.EmailContacts.Count());
            Assert.Equal(null, asnDetails.LookingGlassUrl);
            Assert.Equal("BR", asnDetails.CountryCode);
        }
    }
}