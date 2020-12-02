using System.Collections.Generic;
using System.Linq;
using BGPViewerCore.Service;
using Xunit;

namespace BGPViewerCore.UnitTests.BGPHeServiceTests
{
    public class BGPHeServiceTest : IClassFixture<BGPHeMockWebDriver>
    {
        private IBGPViewerService Service { get; }

        public BGPHeServiceTest(BGPHeMockWebDriver driver)
        {
            Service = new BGPHeService(driver, 7);
        }

        [Fact]
        public void GetAsnDetails()
        {
            var retrievedAsnDetails = Service.GetAsnDetails(15169);
            Assert.Equal(15169, retrievedAsnDetails.ASN);   
            Assert.Equal("Google LLC", retrievedAsnDetails.Name);   
            Assert.Equal("Google LLC", retrievedAsnDetails.Description);   
            Assert.Equal("US", retrievedAsnDetails.CountryCode);
            Assert.Equal(string.Join(", ", new string[]{"network-abuse@google.com", "arin-contact@google.com", "arin-contact@google.com"}), string.Join(", ", retrievedAsnDetails.EmailContacts));
            Assert.Equal(string.Join(", ", new string[]{"network-abuse@google.com", "arin-contact@google.com", "arin-contact@google.com"}), string.Join(", ", retrievedAsnDetails.AbuseContacts));
            Assert.Null(retrievedAsnDetails.LookingGlassUrl);

            retrievedAsnDetails = Service.GetAsnDetails(268003);
            Assert.Equal(268003, retrievedAsnDetails.ASN);   
            Assert.Equal("CITY NET INTERNET EIRELI ME", retrievedAsnDetails.Name);   
            Assert.Equal("CITY NET INTERNET EIRELI ME", retrievedAsnDetails.Description);   
            Assert.Equal("BR", retrievedAsnDetails.CountryCode);
            Assert.Equal(string.Join(", ", new string[]{"citynet.telecom@hotmail.com"}), string.Join(", ", retrievedAsnDetails.EmailContacts));
            Assert.Equal(string.Join(", ", new string[]{"citynet.telecom@hotmail.com"}), string.Join(", ", retrievedAsnDetails.AbuseContacts));
            Assert.Null(retrievedAsnDetails.LookingGlassUrl);

            retrievedAsnDetails = Service.GetAsnDetails(53181);
            Assert.Equal(53181, retrievedAsnDetails.ASN);   
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", retrievedAsnDetails.Name);   
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", retrievedAsnDetails.Description);   
            Assert.Equal("BR", retrievedAsnDetails.CountryCode);
            Assert.Equal(string.Join(", ", new string[]{"engenharia@k2telecom.com.br"}), string.Join(", ", retrievedAsnDetails.EmailContacts));
            Assert.Equal(string.Join(", ", new string[]{"engenharia@k2telecom.com.br"}), string.Join(", ", retrievedAsnDetails.AbuseContacts));
            Assert.Equal("https://lg.k2telecom.net.br/", retrievedAsnDetails.LookingGlassUrl);
        }

        [Fact]
        public void GetDetailsFromNonExistentAsn()
        {
            Assert.Throws<KeyNotFoundException>(() => Service.GetAsnDetails(-1));
        }

        [Fact]
        public void GetAsnDownstreamsShouldAlwaysReturnEmptyObjects()
        {
            Assert.Empty(Service.GetAsnDownstreams(15169).Item1);
            Assert.Empty(Service.GetAsnDownstreams(15169).Item2);
            Assert.Empty(Service.GetAsnDownstreams(268003).Item1);
            Assert.Empty(Service.GetAsnDownstreams(268003).Item2);
            Assert.Empty(Service.GetAsnDownstreams(53181).Item1);
            Assert.Empty(Service.GetAsnDownstreams(53181).Item2);
        }

        [Fact]
        public void GetAsnIxs()
        {   
            var ixs = Service.GetAsnIxs(268003);
            Assert.Empty(ixs);

            ixs = Service.GetAsnIxs(15169);
            var firstIx = ixs.First();
            Assert.Equal("AMS-IX", firstIx.Name);
            Assert.Equal("AMS-IX", firstIx.FullName);
            Assert.Equal("NL", firstIx.CountryCode);
            Assert.Equal("80.249.208.247", firstIx.IPv4);
            Assert.Equal("2001:7f8:1::a501:5169:1", firstIx.IPv6);

            var lastIx = ixs.Last();
            Assert.Equal("YYCIX", lastIx.Name);
            Assert.Equal("YYCIX", lastIx.FullName);
            Assert.Equal("CA", lastIx.CountryCode);
            Assert.Equal("206.126.225.128", lastIx.IPv4);
            Assert.Equal("2001:504:2f::1:5169:1", lastIx.IPv6);
        }

        [Fact]
        public void AsnIxSpeedShouldAlwaysBeZeroWhenApiDoesNotProvideThisInformation()
        {
            var ixs = Service.GetAsnIxs(15169);
            Assert.True(ixs.Count(ix => ix.AsnSpeed == 0) == ixs.Count());
            ixs = Service.GetAsnIxs(268003);
            Assert.True(ixs.Count(ix => ix.AsnSpeed == 0) == ixs.Count());
            ixs = Service.GetAsnIxs(53181);
            Assert.True(ixs.Count(ix => ix.AsnSpeed == 0) == ixs.Count());
        }

        [Fact]
        public void GetAsnPeers()
        {
            var peers = Service.GetAsnPeers(15169);
            var firstPeerIpv4 = peers.Item1.First();
            Assert.Equal(12956, firstPeerIpv4.ASN);
            Assert.Equal("Telefonica International Wholesale Services II, S.L.U.", firstPeerIpv4.Name);
            Assert.Equal("Telefonica International Wholesale Services II, S.L.U.", firstPeerIpv4.Description);
            Assert.Null(firstPeerIpv4.CountryCode);

            var firstPeerIpv6 = peers.Item2.First();
            Assert.Equal(12956, firstPeerIpv6.ASN);
            Assert.Equal("Telefonica International Wholesale Services II, S.L.U.", firstPeerIpv6.Name);
            Assert.Equal("Telefonica International Wholesale Services II, S.L.U.", firstPeerIpv6.Description);
            Assert.Null(firstPeerIpv6.CountryCode);

            peers = Service.GetAsnPeers(268003);
            var lastPeerIpv4 = peers.Item1.Last();
            Assert.Equal(53181, lastPeerIpv4.ASN);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", lastPeerIpv4.Name);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", lastPeerIpv4.Description);
            Assert.Null(lastPeerIpv4.CountryCode);

            var lastPeerIpv6 = peers.Item2.Last();
            Assert.Equal(53181, lastPeerIpv6.ASN);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", lastPeerIpv6.Name);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", lastPeerIpv6.Description);
            Assert.Null(lastPeerIpv6.CountryCode);
        }

        [Fact]
        public void GetAsnPrefixes()
        {
            var prefixes = Service.GetAsnPrefixes(268003);
            Assert.Equal(268003, prefixes.ASN);
            Assert.Equal("45.167.100.0/24", prefixes.IPv4.First());
            Assert.Equal("45.167.103.0/24", prefixes.IPv4.Last());
            Assert.Equal("2804:567c::/32", prefixes.IPv6.First());
        }

        [Fact]
        public void GetAsnUpstreams()
        {
            var upstreams = Service.GetAsnUpstreams(53181);
            Assert.True(upstreams.Item1.Count() == 5);
            Assert.True(upstreams.Item2.Count() == 6);
            var firstIpv4Upstream = upstreams.Item1.First();
            Assert.Equal(6762, firstIpv4Upstream.ASN);
            Assert.Equal("TELECOM ITALIA SPARKLE S.p.A.", firstIpv4Upstream.Name);
            Assert.Equal("TELECOM ITALIA SPARKLE S.p.A.", firstIpv4Upstream.Description);
            Assert.Null(firstIpv4Upstream.CountryCode);

            var firstIpv6Upstream = upstreams.Item2.First();
            Assert.Equal(4230,firstIpv6Upstream.ASN);
            Assert.Equal("CLARO S.A.", firstIpv6Upstream.Name);
            Assert.Equal("CLARO S.A.", firstIpv6Upstream.Description);
            Assert.Null(firstIpv6Upstream.CountryCode);
        }

        [Fact]
        public void GetIpDetails()
        {
            var ip = Service.GetIpDetails("8.8.8.8");
            Assert.Null(ip.CountryCode);
            Assert.Equal("8.8.8.8", ip.IPAddress);
            Assert.Equal("dns.google", ip.PtrRecord);
            Assert.Equal("8.0.0.0/9", ip.RIRAllocationPrefix);
            Assert.True(ip.RelatedPrefixes.Count() == 3);

            var firstRelatedPrefix = ip.RelatedPrefixes.First();
            Assert.Equal("8.0.0.0/9", firstRelatedPrefix.Prefix);
            Assert.Equal("Level 3 Parent, LLC", firstRelatedPrefix.Name);
            Assert.Equal("Level 3 Parent, LLC", firstRelatedPrefix.Description);
            
            Assert.True(firstRelatedPrefix.ParentAsns.Count() == 2);
            
            var firstPrefixParentAsn = firstRelatedPrefix.ParentAsns.First();
            Assert.Equal(3356, firstPrefixParentAsn.ASN);
            Assert.Equal("Level 3 Parent, LLC", firstPrefixParentAsn.Name);
            Assert.Equal("Level 3 Parent, LLC", firstPrefixParentAsn.Description);
            Assert.Null(firstPrefixParentAsn.CountryCode);
        }

        [Fact]
        public void GetIpDetailsWhenPtrRecordDoesNotExist()
        {
            var ip = Service.GetIpDetails("196.100.100.0");
            Assert.Null(ip.CountryCode);
            Assert.Equal("196.100.100.0", ip.IPAddress);
            Assert.Null(ip.PtrRecord);
            Assert.Equal("196.96.0.0/12", ip.RIRAllocationPrefix);
            Assert.True(ip.RelatedPrefixes.Count() == 2);

            var firstRelatedPrefix = ip.RelatedPrefixes.First();
            Assert.Equal("196.96.0.0/12", firstRelatedPrefix.Prefix);
            Assert.Equal("Safaricom Limited", firstRelatedPrefix.Name);
            Assert.Equal("Safaricom Limited", firstRelatedPrefix.Description);
            
            Assert.True(firstRelatedPrefix.ParentAsns.Count() == 1);

            var lastRelatedPrefix = ip.RelatedPrefixes.Last();
            Assert.Equal("196.96.0.0/13", lastRelatedPrefix.Prefix);
            Assert.Equal("Used by safaricom 2G/3G/4G  subscribers.", lastRelatedPrefix.Name);
            Assert.Equal("Used by safaricom 2G/3G/4G  subscribers.", lastRelatedPrefix.Description);
            
            Assert.True(lastRelatedPrefix.ParentAsns.Count() == 1);
        }

        [Fact]
        public void GetPrefixDetails()
        {
            var prefix = Service.GetPrefixDetails("8.8.8.0", 24);
            Assert.Equal("8.8.8.0/24", prefix.Prefix);
            Assert.Equal("Google LLC", prefix.Name);
            Assert.Equal("Google LLC", prefix.Description);
            
            Assert.True(prefix.ParentAsns.Count() == 3, $"It was expected 3 and got {prefix.ParentAsns.Count()}");

            var firstParentAsn = prefix.ParentAsns.First();
            Assert.Equal(15169, firstParentAsn.ASN);
            Assert.Equal("Google LLC", firstParentAsn.Name);
            Assert.Equal("Google LLC", firstParentAsn.Description);
            Assert.Equal("US", firstParentAsn.CountryCode);

            var lastParentAsn = prefix.ParentAsns.Last();
            Assert.Equal(3549, lastParentAsn.ASN);
            Assert.Equal("Level 3 Parent, LLC", lastParentAsn.Name);
            Assert.Equal("Level 3 Parent, LLC", lastParentAsn.Description);
            Assert.Null(lastParentAsn.CountryCode);

            prefix = Service.GetPrefixDetails("196.96.0.0", 12);
            Assert.Equal("196.96.0.0/12", prefix.Prefix);
            Assert.Equal("Safaricom Limited", prefix.Name);
            Assert.Equal("Safaricom Limited", prefix.Description);

            Assert.True(prefix.ParentAsns.Count() == 1, $"It was expected 1 and got {prefix.ParentAsns.Count()}");
            
            firstParentAsn = prefix.ParentAsns.First();
            Assert.Equal(33771, firstParentAsn.ASN);
            Assert.Equal("Safaricom Limited", firstParentAsn.Name);
            Assert.Equal("Safaricom Limited", firstParentAsn.Description);
            Assert.Equal("KE", firstParentAsn.CountryCode);
        }

        [Fact]
        public void GetV6PrefixDetails()
        {
            var prefix = Service.GetPrefixDetails("2001:4860::", 32);
            Assert.Equal("2001:4860::/32", prefix.Prefix);
            Assert.Equal("Google LLC", prefix.Name);
            Assert.Equal("Google LLC", prefix.Description);
            
            Assert.True(prefix.ParentAsns.Count() == 1, $"It was expected 1 and got {prefix.ParentAsns.Count()}");

            var firstParentAsn = prefix.ParentAsns.First();
            Assert.Equal(15169, firstParentAsn.ASN);
            Assert.Equal("Google LLC", firstParentAsn.Name);
            Assert.Equal("Google LLC", firstParentAsn.Description);
            Assert.Equal("US", firstParentAsn.CountryCode);


            prefix = Service.GetPrefixDetails("2a02:26f0:128::", 48);
            Assert.Equal("2a02:26f0:128::/48", prefix.Prefix);
            Assert.Equal("Akamai International B.V.", prefix.Name);
            Assert.Equal("Akamai International B.V.", prefix.Description);

            Assert.True(prefix.ParentAsns.Count() == 3, $"It was expected 3 and got {prefix.ParentAsns.Count()}");
            
            firstParentAsn = prefix.ParentAsns.First();
            Assert.Equal(6762, firstParentAsn.ASN);
            Assert.Equal("Akamai International B.V.", firstParentAsn.Name);
            Assert.Equal("Akamai International B.V.", firstParentAsn.Description);
            Assert.Equal("EU", firstParentAsn.CountryCode);

            var lastParentAsn = prefix.ParentAsns.Last();
            Assert.Equal(20940, lastParentAsn.ASN);
            Assert.Equal("Akamai International B.V.", lastParentAsn.Name);
            Assert.Equal("Akamai International B.V.", lastParentAsn.Description);
            Assert.Null(lastParentAsn.CountryCode);
        }

        [Fact]
        public void SearchByAsnWithLittleData()
        {
            var searchResultForAsn53181 = Service.SearchBy("53181");
            
            Assert.Equal(1, searchResultForAsn53181.RelatedAsns.Count());
            Assert.Equal(53181, searchResultForAsn53181.RelatedAsns.First().ASN);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", searchResultForAsn53181.RelatedAsns.First().Name);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", searchResultForAsn53181.RelatedAsns.First().Description);
            Assert.Equal("BR", searchResultForAsn53181.RelatedAsns.First().CountryCode);
            
            Assert.True(searchResultForAsn53181.RelatedAsns.First().EmailContacts.Count() == 1, $"Should have only one contact email and actually has {searchResultForAsn53181.RelatedAsns.First().AbuseContacts.Count()}.");
            Assert.True(searchResultForAsn53181.RelatedAsns.First().AbuseContacts.Count() == 1, $"Should have only one abuse email and actually has {searchResultForAsn53181.RelatedAsns.First().AbuseContacts.Count()}.");
            
            Assert.Equal("engenharia@k2telecom.com.br", searchResultForAsn53181.RelatedAsns.First().EmailContacts.First());
            Assert.Equal("engenharia@k2telecom.com.br", searchResultForAsn53181.RelatedAsns.First().AbuseContacts.First());
            
            var firstIpv4 = searchResultForAsn53181.IPv4.First();
            Assert.Equal("191.241.64.0/20", firstIpv4.Prefix);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", firstIpv4.Name);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", firstIpv4.Description);

            var lastIpv6 = searchResultForAsn53181.IPv6.Last();
            Assert.Equal("2804:113c:fc00::/38", lastIpv6.Prefix);
            Assert.Equal(string.Empty, lastIpv6.Name);
            Assert.Equal(string.Empty, lastIpv6.Description);
        }
    }
}