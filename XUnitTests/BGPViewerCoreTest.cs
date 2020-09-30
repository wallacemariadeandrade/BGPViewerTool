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
            
            Assert.Equal(6762, asnDetails.Info.ASN);
            Assert.Equal("TELECOM ITALIA SPARKLE S.p.A.", asnDetails.Info.Description);
            Assert.Equal("SEABONE-NET", asnDetails.Info.Name);
            Assert.Equal("abuse@seabone.net", asnDetails.EmailContacts.ElementAt(0));
            Assert.Equal("peering@seabone.net", asnDetails.EmailContacts.ElementAt(1));
            Assert.Equal("tech@seabone.net", asnDetails.EmailContacts.ElementAt(2));
            Assert.Equal("francesco.chiappini@telecomitalia.it", asnDetails.EmailContacts.ElementAt(3));
            Assert.Equal("abuse@seabone.net", asnDetails.AbuseContacts.ElementAt(0));
            Assert.Equal("https://gambadilegno.noc.seabone.net/lg/", asnDetails.LookingGlassUrl);
            Assert.Equal("IT", asnDetails.Info.CountryCode);
        }

        [Fact]
        public void GetAsnDetailsWhenSomePropertiesAreNull()
        {
            // Mocked Data
            var asnDetails = GetService().GetAsnDetails(53181);
            
            Assert.Equal(53181, asnDetails.Info.ASN);
            Assert.Equal(null, asnDetails.Info.Description);
            Assert.Equal(null, asnDetails.Info.Name);
            Assert.Equal(0, asnDetails.EmailContacts.Count());
            Assert.Equal(null, asnDetails.LookingGlassUrl);
            Assert.Equal("BR", asnDetails.Info.CountryCode);
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
            Assert.Equal("143.208.20.0/22", asn264075Prefixes.IPv4Prefixes.First());
            Assert.Equal("2804:2a7c::/32", asn264075Prefixes.IPv6Prefixes.First());
        }

        [Fact]
        public void CountAsnPeers()
        {
            var asn268374Peers = GetService().GetAsnPeers(268374);
            Assert.True(asn268374Peers.IPv4Peers.Count() == 13);
            Assert.True(asn268374Peers.IPv6Peers.Count() == 8);
        }

        [Fact]
        public void VerifyAsnPeers()
        {
            var asn268374Peers = GetService().GetAsnPeers(268374);

            Assert.Equal(asn268374Peers.IPv4Peers.First().ASN, 53181);
            Assert.Equal(asn268374Peers.IPv4Peers.Last().ASN, 57463);
            Assert.Equal(asn268374Peers.IPv6Peers.First().ASN, 53181);
            Assert.Equal(asn268374Peers.IPv6Peers.Last().ASN, 267613);
        }
    }
}