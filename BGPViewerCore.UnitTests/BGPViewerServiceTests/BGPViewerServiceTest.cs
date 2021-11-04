using BGPViewerCore.Service;
using System;
using System.Linq;
using Xunit;

namespace BGPViewerCore.UnitTests.BGPViewerServiceTests
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
            Assert.Equal("TELECOM ITALIA SPARKLE S.p.A.", asnDetails.Description);
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
            Assert.Null(asnDetails.Description);
            Assert.Null(asnDetails.Name);
            Assert.True(asnDetails.EmailContacts.Count() == 0);
            Assert.Null(asnDetails.LookingGlassUrl);
            Assert.Equal("BR", asnDetails.CountryCode);
        }

        [Fact]
        public void TryGettingInvalidAsnDetails()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                GetService().GetAsnDetails(101010101);
            });
        }

        [Fact]
        public void CountAsnPrefixes()
        {
            var asn264075Prefixes = GetService().GetAsnPrefixes(264075);
            var asn268374Prefixes = GetService().GetAsnPrefixes(268374);
            var asn131630Prefixes = GetService().GetAsnPrefixes(131630);
            
            Assert.True(asn264075Prefixes.IPv4.Count() == 1, $"Error: AS{asn264075Prefixes.ASN} shoud have only one IPv4 prefix");
            Assert.True(asn264075Prefixes.IPv6.Count() == 1, $"Error: AS{asn264075Prefixes.ASN} shoud have only one IPv6 prefix");

            Assert.True(asn268374Prefixes.IPv4.Count() == 7, $"Error: AS{asn268374Prefixes.ASN} shoud have 7 IPv4 prefixes");
            Assert.True(asn268374Prefixes.IPv6.Count() == 1, $"Error: AS{asn268374Prefixes.ASN} shoud have only one IPv6 prefix");

            Assert.True(asn131630Prefixes.IPv4.Count() == 3, $"Error: AS{asn131630Prefixes.ASN} shoud have 3 IPv4 prefixes");
            Assert.True(asn131630Prefixes.IPv6.Count() == 0, $"Error: AS{asn131630Prefixes.ASN} shoudn't IPv6 prefix");
        }

        [Fact]
        public void VerifyAsnPrefixes()
        {
            var asn264075Prefixes = GetService().GetAsnPrefixes(264075);
          
            Assert.Equal("143.208.20.0/22", asn264075Prefixes.IPv4.First());
            Assert.Equal("2804:2a7c::/32", asn264075Prefixes.IPv6.First());
        }

        [Fact]
        public void CountAsnPeers()
        {
            var asn268374Peers = GetService().GetAsnPeers(268374);
          
            Assert.True(asn268374Peers.Item1.Count() == 13);
            Assert.True(asn268374Peers.Item2.Count() == 8);
        }

        [Fact]
        public void VerifyAsnPeers()
        {
            var asn268374Peers = GetService().GetAsnPeers(268374);
          
            Assert.Equal(53181, asn268374Peers.Item1.First().ASN);
            Assert.Equal(57463, asn268374Peers.Item1.Last().ASN);
            Assert.Equal(53181, asn268374Peers.Item2.First().ASN);
            Assert.Equal(267613, asn268374Peers.Item2.Last().ASN);
        }

        [Fact]
        public void GettingAsnUpstreams()
        {
            var asn52575Upstreams = GetService().GetAsnUpstreams(52575);
          
            Assert.True(asn52575Upstreams.Item1.Count() == 1);
            Assert.True(asn52575Upstreams.Item2.Count() == 0);
            Assert.Equal(265185, asn52575Upstreams.Item1.First().ASN);
        }

        [Fact]
        public void GettingAsnDownstreams()
        {
            var asn52908Downstreams = GetService().GetAsnDownstreams(52908);
          
            Assert.True(asn52908Downstreams.Item1.Count() == 2);
            Assert.True(asn52908Downstreams.Item2.Count() == 0);
            Assert.Equal(267360, asn52908Downstreams.Item1.First().ASN);
            Assert.Equal(268699, asn52908Downstreams.Item1.Last().ASN);
        }

        [Fact]
        public void GettingAnsIxs()
        {
            var asn53181Ixs = GetService().GetAsnIxs(53181);
            var ixSp = asn53181Ixs.Where(ix => ix.Name.Contains("São Paulo")).Count();
            var ixRj = asn53181Ixs.Where(ix => ix.Name.Contains("Rio de Janeiro")).Count();
            
            Assert.True(asn53181Ixs.Count() == 4, $"Error: expected 4, got {asn53181Ixs.Count()}");
            Assert.True(ixSp == 1, $"Error: expected 1 for São Paulo, got {ixSp}");
            Assert.True(ixRj == 3, $"Error: expected 3 for Rio de Janeiro, got {ixRj}");
        }

        [Fact]
        public void GettingAsnWithoutIxs()
        {
            var asn268003Ixs = GetService().GetAsnIxs(268003);
            Assert.Empty(asn268003Ixs);
        }

        [Fact]
        public void GettingIpAddressDetails()
        {
            var ipDetails = GetService().GetIpDetails("143.208.20.0");
          
            Assert.Equal("BR", ipDetails.CountryCode);
            Assert.Equal("143.208.20.0/22", ipDetails.RIRAllocationPrefix);
            Assert.Equal("143-208-20-0.k1fibra.net.br", ipDetails.PtrRecord);
            Assert.Equal(264075, ipDetails.RelatedPrefixes.First().ParentAsns.First().ASN);
        }

        [Fact]
        public void GettingIpAddressDetailsWithSomeNullProperties()
        {
            var ipDetails = GetService().GetIpDetails("170.245.37.10");
            
            Assert.Null(ipDetails.PtrRecord);
            Assert.Null(ipDetails.RelatedPrefixes.First().ParentAsns.First().CountryCode);
            Assert.Null(ipDetails.RelatedPrefixes.First().Name);
            Assert.Null(ipDetails.RelatedPrefixes.First().Description);
            Assert.Null(ipDetails.RelatedPrefixes.Last().ParentAsns.First().CountryCode);
        }
        
        [Fact]
        public void TryGetIpAddressDetailsWithMalformedInput()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                GetService().GetIpDetails("192.168");
            });

            Assert.Throws<ArgumentException>(() => 
            {
                GetService().GetIpDetails("177.75.40.256");
            });

            Assert.Throws<ArgumentException>(() => 
            {
                GetService().GetIpDetails("192.168.10.10.1");
            });
        }

        [Fact]
        public void GetPrefixDetails()
        {
            var prefixDetails = GetService().GetPrefixDetails("143.208.20.0", 22);
            
            Assert.Equal(264075, prefixDetails.ParentAsns.First().ASN);
            Assert.Equal("K1 Telecom e Multimidia LTDA", prefixDetails.Name);
        }

        [Fact]
        public void GetPrefixDetailsWithSomeNullProperties()
        {
            var prefixDetails = GetService().GetPrefixDetails("177.75.40.0", 21);
            
            Assert.Equal(53040, prefixDetails.ParentAsns.First().ASN);
            Assert.Null(prefixDetails.Name);
            Assert.Null(prefixDetails.Description);
            Assert.Null(prefixDetails.ParentAsns.First().Description);
            Assert.Null(prefixDetails.ParentAsns.First().CountryCode);
        }

        [Fact]
        public void TryGetPrefixDetailsWithWrongCIDR()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                GetService().GetPrefixDetails("143.208.20.0", 20); // Should be 143.208.20.0/22 or lesser
            });
        }

        [Fact]
        public void SearchByAsnWithLittleData()
        {
            var searchResultForAsn53181 = GetService().SearchBy("53181");
            
            Assert.True(searchResultForAsn53181.RelatedAsns.Count() == 1);
            Assert.Equal(53181, searchResultForAsn53181.RelatedAsns.First().ASN);
            Assert.Null(searchResultForAsn53181.RelatedAsns.First().Name);
            Assert.Null(searchResultForAsn53181.RelatedAsns.First().Description);
            Assert.Null(searchResultForAsn53181.RelatedAsns.First().CountryCode);
            Assert.Empty(searchResultForAsn53181.RelatedAsns.First().EmailContacts);
            Assert.Empty(searchResultForAsn53181.RelatedAsns.First().AbuseContacts);
            Assert.Empty(searchResultForAsn53181.IPv4);
            Assert.Empty(searchResultForAsn53181.IPv6);
        }

        [Fact]
        public void SearchByAsnWithSomeData()
        {
            var searchResultForAsn3356 = GetService().SearchBy("3356");
            
            Assert.True(searchResultForAsn3356.RelatedAsns.Count() == 1);
            Assert.Equal(3356, searchResultForAsn3356.RelatedAsns.First().ASN);
            Assert.Equal("LEVEL3", searchResultForAsn3356.RelatedAsns.First().Name);
            Assert.Equal("Level 3 Parent, LLC", searchResultForAsn3356.RelatedAsns.First().Description);
            Assert.Equal("US", searchResultForAsn3356.RelatedAsns.First().CountryCode);
            Assert.True(searchResultForAsn3356.RelatedAsns.First().EmailContacts.Count() == 1);
            Assert.True(searchResultForAsn3356.RelatedAsns.First().AbuseContacts.Count() == 1);
            Assert.True(searchResultForAsn3356.IPv4.Count() == 2);
            Assert.Equal("12.130.205.0/24", searchResultForAsn3356.IPv4.First().Prefix);
            Assert.Equal("65.51.86.0/24", searchResultForAsn3356.IPv4.Last().Prefix);
            Assert.Empty(searchResultForAsn3356.IPv6);
        }

        [Fact]
        public void SearchByInexistantThing()
        {
            var result = GetService().SearchBy("empty");

            Assert.Empty(result.RelatedAsns);
            Assert.Empty(result.IPv4);
            Assert.Empty(result.IPv6);
        }

        [Fact]
        public async void GetAsnDetaisAsync()
        {
            // Mocked Data
            var asnDetailsAsync = await GetService().GetAsnDetailsAsync(6762);
            
            Assert.Equal(6762, asnDetailsAsync.ASN);
            Assert.Equal("TELECOM ITALIA SPARKLE S.p.A.", asnDetailsAsync.Description);
            Assert.Equal("SEABONE-NET", asnDetailsAsync.Name);
            Assert.Equal("abuse@seabone.net", asnDetailsAsync.EmailContacts.ElementAt(0));
            Assert.Equal("peering@seabone.net", asnDetailsAsync.EmailContacts.ElementAt(1));
            Assert.Equal("tech@seabone.net", asnDetailsAsync.EmailContacts.ElementAt(2));
            Assert.Equal("francesco.chiappini@telecomitalia.it", asnDetailsAsync.EmailContacts.ElementAt(3));
            Assert.Equal("abuse@seabone.net", asnDetailsAsync.AbuseContacts.ElementAt(0));
            Assert.Equal("https://gambadilegno.noc.seabone.net/lg/", asnDetailsAsync.LookingGlassUrl);
            Assert.Equal("IT", asnDetailsAsync.CountryCode);
        }

        [Fact]
        public async void GetAsnDetailsWhenSomePropertiesAreNullAsync()
        {
            // Mocked Data
            var asnDetails = await GetService().GetAsnDetailsAsync(53181);
            
            Assert.Equal(53181, asnDetails.ASN);
            Assert.Null(asnDetails.Description);
            Assert.Null(asnDetails.Name);
            Assert.True(asnDetails.EmailContacts.Count() == 0);
            Assert.Null(asnDetails.LookingGlassUrl);
            Assert.Equal("BR", asnDetails.CountryCode);
        }

        [Fact]
        public async void TryGettingInvalidAsnDetailsAsync()
        {
            await Assert.ThrowsAsync<ArgumentException>(
                () => GetService().GetAsnDetailsAsync(101010101));
        }

        [Fact]
        public async void CountAsnPrefixesAsync()
        {
            var asn264075Prefixes = await GetService().GetAsnPrefixesAsync(264075);
            var asn268374Prefixes = await GetService().GetAsnPrefixesAsync(268374);
            var asn131630Prefixes = await GetService().GetAsnPrefixesAsync(131630);
            
            Assert.True(asn264075Prefixes.IPv4.Count() == 1, $"Error: AS{asn264075Prefixes.ASN} shoud have only one IPv4 prefix");
            Assert.True(asn264075Prefixes.IPv6.Count() == 1, $"Error: AS{asn264075Prefixes.ASN} shoud have only one IPv6 prefix");

            Assert.True(asn268374Prefixes.IPv4.Count() == 7, $"Error: AS{asn268374Prefixes.ASN} shoud have 7 IPv4 prefixes");
            Assert.True(asn268374Prefixes.IPv6.Count() == 1, $"Error: AS{asn268374Prefixes.ASN} shoud have only one IPv6 prefix");

            Assert.True(asn131630Prefixes.IPv4.Count() == 3, $"Error: AS{asn131630Prefixes.ASN} shoud have 3 IPv4 prefixes");
            Assert.True(asn131630Prefixes.IPv6.Count() == 0, $"Error: AS{asn131630Prefixes.ASN} shoudn't IPv6 prefix");
        }

        [Fact]
        public async void VerifyAsnPrefixesAsync()
        {
            var asn264075Prefixes = await GetService().GetAsnPrefixesAsync(264075);
          
            Assert.Equal("143.208.20.0/22", asn264075Prefixes.IPv4.First());
            Assert.Equal("2804:2a7c::/32", asn264075Prefixes.IPv6.First());
        }

        [Fact]
        public async void CountAsnPeersAsync()
        {
            var asn268374Peers = await GetService().GetAsnPeersAsync(268374);
          
            Assert.True(asn268374Peers.Item1.Count() == 13);
            Assert.True(asn268374Peers.Item2.Count() == 8);
        }

        [Fact]
        public async void VerifyAsnPeersAsync()
        {
            var asn268374Peers = await GetService().GetAsnPeersAsync(268374);
          
            Assert.Equal(53181, asn268374Peers.Item1.First().ASN);
            Assert.Equal(57463, asn268374Peers.Item1.Last().ASN);
            Assert.Equal(53181, asn268374Peers.Item2.First().ASN);
            Assert.Equal(267613, asn268374Peers.Item2.Last().ASN);
        }

        [Fact]
        public async void GettingAsnUpstreamsAsync()
        {
            var asn52575Upstreams = await GetService().GetAsnUpstreamsAsync(52575);
          
            Assert.True(asn52575Upstreams.Item1.Count() == 1);
            Assert.True(asn52575Upstreams.Item2.Count() == 0);
            Assert.Equal(265185, asn52575Upstreams.Item1.First().ASN);
        }

        [Fact]
        public async void GettingAsnDownstreamsAsync()
        {
            var asn52908Downstreams = await GetService().GetAsnDownstreamsAsync(52908);
          
            Assert.True(asn52908Downstreams.Item1.Count() == 2);
            Assert.True(asn52908Downstreams.Item2.Count() == 0);
            Assert.Equal(267360, asn52908Downstreams.Item1.First().ASN);
            Assert.Equal(268699, asn52908Downstreams.Item1.Last().ASN);
        }
    }
}