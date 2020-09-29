using System.Collections.Generic;
using BGPViewerCore.Model;
using BGPViewerCore.Service;
using System.Linq;

namespace Xunit
{
    public class BGPViewerServiceTest
    {
        private BGPViewerService _service;
        private BGPViewerService GetService()
        {
            if(_service == null) _service = new BGPViewerService(new BGPViewerMockApi());
            return _service;
        }

        [Fact]
        public void GetAsnDetais()
        {
            // Mocked Data
            var asnDetails = GetService().GetAsnDetails(6762);
            
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
            var asnDetails = GetService().GetAsnDetails(53181);
            
            Assert.Equal(53181, asnDetails.ASN);
            Assert.Equal(null, asnDetails.DescriptionShort);
            Assert.Equal(null, asnDetails.Name);
            Assert.Equal(0, asnDetails.EmailContacts.Count());
            Assert.Equal(null, asnDetails.LookingGlassUrl);
            Assert.Equal("BR", asnDetails.CountryCode);
        }

        [Fact]
        public void CountAsnPrefixes()
        {
            var asn264075Prefixes = GetService().GetAsnPrefixes(264075);
            var asn268374Prefixes = GetService().GetAsnPrefixes(268374);
            var asn131630Prefixes = GetService().GetAsnPrefixes(131630);
            
            Assert.True(asn264075Prefixes.IPv4Prefixes.Count() == 1, $"Error: AS{asn264075Prefixes.ASN} shoud have only one IPv4 prefix");
            Assert.True(asn264075Prefixes.IPv6Prefixes.Count() == 1, $"Error: AS{asn264075Prefixes.ASN} shoud have only one IPv6 prefix");

            Assert.True(asn268374Prefixes.IPv4Prefixes.Count() == 7, $"Error: AS{asn268374Prefixes.ASN} shoud have 7 IPv4 prefixes");
            Assert.True(asn268374Prefixes.IPv6Prefixes.Count() == 1, $"Error: AS{asn268374Prefixes.ASN} shoud have only one IPv6 prefix");

            Assert.True(asn131630Prefixes.IPv4Prefixes.Count() == 3, $"Error: AS{asn131630Prefixes.ASN} shoud have 3 IPv4 prefixes");
            Assert.True(asn131630Prefixes.IPv6Prefixes.Count() == 0, $"Error: AS{asn131630Prefixes.ASN} shoudn't IPv6 prefix");
        }

        [Fact]
        public void VerifyAsnPrefixes()
        {
            var asn264075Prefixes = GetService().GetAsnPrefixes(264075);
            Assert.Equal("143.208.20.0/22", asn264075Prefixes.IPv4Prefixes.ElementAt(0));
            Assert.Equal("2804:2a7c::/32", asn264075Prefixes.IPv6Prefixes.ElementAt(0));
        }
    }
}