using System.Linq;
using BGPViewerCore.Service;
using Xunit;

namespace BGPViewerCore.UnitTests.PeeringDbServiceTests
{
    public class PeeringDbServiceTest
    {
        private PeeringDbService _service;
        private PeeringDbService GetService()
        {
            if(_service == null) _service = new PeeringDbService(new PeeringDbMockWebApi());
            return _service;
        }

        [Fact]
        public void GetAsnDetails()
        {
            var details16509 = GetService().GetAsnDetails(16509);
            Assert.Equal(16509, details16509.ASN);
            Assert.Equal("Amazon.com", details16509.Name);
            Assert.Equal("Amazon Web Services", details16509.Description);
            Assert.Equal(Enumerable.Empty<string>(), details16509.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), details16509.AbuseContacts);
            Assert.Equal(string.Empty, details16509.CountryCode);
            Assert.Equal(string.Empty, details16509.LookingGlassUrl);

            var details53181 = GetService().GetAsnDetails(53181);
            Assert.Equal(53181, details53181.ASN);
            Assert.Equal("K2 Telecom e Multimidia", details53181.Name);
            Assert.Equal("K2TELECOM", details53181.Description);
            Assert.Equal(Enumerable.Empty<string>(), details53181.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), details53181.AbuseContacts);
            Assert.Equal(string.Empty, details53181.CountryCode);
            Assert.Equal("https://lg.k2telecom.net.br/", details53181.LookingGlassUrl);
        }

        [Fact]
        public async void GetAsnDetailsAsync()
        {
            var details16509 = await GetService().GetAsnDetailsAsync(16509);
            Assert.Equal(16509, details16509.ASN);
            Assert.Equal("Amazon.com", details16509.Name);
            Assert.Equal("Amazon Web Services", details16509.Description);
            Assert.Equal(Enumerable.Empty<string>(), details16509.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), details16509.AbuseContacts);
            Assert.Equal(string.Empty, details16509.CountryCode);
            Assert.Equal(string.Empty, details16509.LookingGlassUrl);

            var details53181 = await GetService().GetAsnDetailsAsync(53181);
            Assert.Equal(53181, details53181.ASN);
            Assert.Equal("K2 Telecom e Multimidia", details53181.Name);
            Assert.Equal("K2TELECOM", details53181.Description);
            Assert.Equal(Enumerable.Empty<string>(), details53181.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), details53181.AbuseContacts);
            Assert.Equal(string.Empty, details53181.CountryCode);
            Assert.Equal("https://lg.k2telecom.net.br/", details53181.LookingGlassUrl);
        }

        [Fact]
        public void GetDetailsFromUnregisteredAs()
        {
            var unregisteredDetails = GetService().GetAsnDetails(123456);
            Assert.Equal(123456, unregisteredDetails.ASN);
            Assert.Equal(string.Empty, unregisteredDetails.Name);
            Assert.Equal(string.Empty, unregisteredDetails.Description);
            Assert.Equal(string.Empty, unregisteredDetails.CountryCode);
            Assert.Equal(Enumerable.Empty<string>(), unregisteredDetails.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), unregisteredDetails.AbuseContacts);
            Assert.Equal(string.Empty, unregisteredDetails.LookingGlassUrl);
        }

        [Fact]
        public void GetAsnDownstreamsAlwaysReturnNullObject()
        {
            var downstreams16509 = GetService().GetAsnDownstreams(16509);
            Assert.Empty(downstreams16509.Item1);
            Assert.Empty(downstreams16509.Item2);
        }

        [Fact]
        public async void GetAsnDownstreamsAlwaysReturnNullObjectAsync()
        {
            var downstreams16509 = await GetService().GetAsnDownstreamsAsync(16509);
            Assert.Empty(downstreams16509.Item1);
            Assert.Empty(downstreams16509.Item2);
        }

        [Fact]
        public void GetAsnIxs()
        {
            var ixs60068 = GetService().GetAsnIxs(60068);
            Assert.NotEmpty(ixs60068);
            var ix = ixs60068.First();
            Assert.Equal("MSK-IX Moscow: MSK-IX peering network", ix.Name);
            Assert.Equal("MSK-IX Moscow: MSK-IX peering network", ix.FullName);
            Assert.Equal(string.Empty, ix.CountryCode);
            Assert.Equal("195.208.210.117", ix.IPv4);
            Assert.Equal("2001:7f8:20:101::210:117", ix.IPv6);
            Assert.Equal(100000, ix.AsnSpeed);
        }

        [Fact]
        public async void GetAsnIxAsync()
        {
            var ixs53181 = await GetService().GetAsnIxsAsync(53181);
            Assert.NotEmpty(ixs53181);
            var ix = ixs53181.Last();
            Assert.Equal("IX.br (PTT.br) Vitória: ATM/MPLA", ix.Name);
            Assert.Equal("IX.br (PTT.br) Vitória: ATM/MPLA", ix.FullName);
            Assert.Equal(string.Empty, ix.CountryCode);
            Assert.Equal("187.16.194.72", ix.IPv4);
            Assert.Equal("2001:12f8:0:17::72", ix.IPv6);
            Assert.Equal(10000, ix.AsnSpeed);
        }

        [Fact]
        public void GetAsWithoutIx()
        {
            var ixs1234 = GetService().GetAsnIxs(1234);
            Assert.Empty(ixs1234);
        }

        [Fact]
        public void GetAsnPeersAlwaysReturnNullObject()
        {
            var peers1234 = GetService().GetAsnPeers(1234);
            Assert.Empty(peers1234.Item1);
            Assert.Empty(peers1234.Item2);
        }

        [Fact]
        public async void GetAsnPeersAlwaysReturnNullObjectAsync()
        {
            var peers1234 = await GetService().GetAsnPeersAsync(1234);
            Assert.Empty(peers1234.Item1);
            Assert.Empty(peers1234.Item2);
        }

        [Fact]
        public void GetAsnPrefixesAlwaysReturnNullObject()
        {
            var prefixes = GetService().GetAsnPrefixes(1234);
            Assert.Equal(1234, prefixes.ASN);
            Assert.Empty(prefixes.IPv4);
            Assert.Empty(prefixes.IPv6);
        }

        [Fact]
        public async void GetAsnPrefixesAlwaysReturnNullObjectAsync()
        {
            var prefixes = await GetService().GetAsnPrefixesAsync(1234);
            Assert.Equal(1234, prefixes.ASN);
            Assert.Empty(prefixes.IPv4);
            Assert.Empty(prefixes.IPv6);
        }

        [Fact]
        public void GetAsnUpstreamsAlwaysReturnNullObject()
        {
            var upstreams1234 = GetService().GetAsnUpstreams(1234);
            Assert.Empty(upstreams1234.Item1);
            Assert.Empty(upstreams1234.Item2);
        }

        [Fact]
        public async void GetAsnUpstreamsAlwaysReturnNullObjectAsync()
        {
            var upstreams1234 = await GetService().GetAsnUpstreamsAsync(1234);
            Assert.Empty(upstreams1234.Item1);
            Assert.Empty(upstreams1234.Item2);
        }

        [Fact]
        public void GetIpDetailsAlwaysReturnNullObject()
        {
            var details = GetService().GetIpDetails("8.8.8.8");
            Assert.Equal("8.8.8.8", details.IPAddress);
            Assert.Equal(string.Empty, details.PtrRecord);
            Assert.Equal(string.Empty, details.CountryCode);
            Assert.Equal(string.Empty, details.RIRAllocationPrefix);
            Assert.Empty(details.RelatedPrefixes);
        }

        [Fact]
        public async void GetIpDetailsAlwaysReturnNullObjectAsync()
        {
            var details = await GetService().GetIpDetailsAsync("8.8.8.8");
            Assert.Equal("8.8.8.8", details.IPAddress);
            Assert.Equal(string.Empty, details.PtrRecord);
            Assert.Equal(string.Empty, details.CountryCode);
            Assert.Equal(string.Empty, details.RIRAllocationPrefix);
            Assert.Empty(details.RelatedPrefixes);
        }

        [Fact]
        public void GetPrefixDetailsAlwaysReturnNullObject()
        {
            var details = GetService().GetPrefixDetails("8.8.8.0", 24);
            Assert.Equal(string.Empty, details.Name);
            Assert.Equal(string.Empty, details.Description);
            Assert.Equal("8.8.8.0/24", details.Prefix);
            Assert.Empty(details.ParentAsns);
        }

        [Fact]
        public async void GetPrefixDetailsAlwaysReturnNullObjectAsync()
        {
            var details = await GetService().GetPrefixDetailsAsync("1.1.1.0", 24);
            Assert.Equal(string.Empty, details.Name);
            Assert.Equal(string.Empty, details.Description);
            Assert.Equal("1.1.1.0/24", details.Prefix);
            Assert.Empty(details.ParentAsns);
        }

        [Fact]
        public void SearchByAsn()
        {
            var result = GetService().SearchBy("53181");
            Assert.NotNull(result);
            Assert.Empty(result.IPv4);
            Assert.Empty(result.IPv6);
            Assert.Equal(1, result.RelatedAsns.Count());
            var asn = result.RelatedAsns.First();
            Assert.Equal(53181, asn.ASN);
            Assert.Equal("K2 Telecom e Multimidia", asn.Name);
            Assert.Equal("K2TELECOM", asn.Description);
            Assert.Equal(Enumerable.Empty<string>(), asn.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), asn.AbuseContacts);
            Assert.Equal(string.Empty, asn.CountryCode);
        }

        [Fact]
        public async void SearchByAsnAsync()
        {
            var result = await GetService().SearchByAsync("53181");
            Assert.NotNull(result);
            Assert.Empty(result.IPv4);
            Assert.Empty(result.IPv6);
            Assert.Equal(1, result.RelatedAsns.Count());
            var asn = result.RelatedAsns.First();
            Assert.Equal(53181, asn.ASN);
            Assert.Equal("K2 Telecom e Multimidia", asn.Name);
            Assert.Equal("K2TELECOM", asn.Description);
            Assert.Equal(Enumerable.Empty<string>(), asn.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), asn.AbuseContacts);
            Assert.Equal(string.Empty, asn.CountryCode);
        }

        [Fact]
        public void SearchByUnregisteredAsn()
        {
            var result = GetService().SearchBy("123456");
            Assert.NotNull(result);
            Assert.Empty(result.IPv4);
            Assert.Empty(result.IPv6);
            Assert.Equal(1, result.RelatedAsns.Count());
            var asn = result.RelatedAsns.First();
            Assert.Equal(123456, asn.ASN);
            Assert.Equal(string.Empty, asn.Name);
            Assert.Equal(string.Empty, asn.Description);
            Assert.Equal(string.Empty, asn.CountryCode);
            Assert.Equal(Enumerable.Empty<string>(), asn.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), asn.AbuseContacts);
        }

        [Fact]
        public async void SearchByUnregisteredAsnAsync()
        {
            var result = await GetService().SearchByAsync("123456");
            Assert.NotNull(result);
            Assert.Empty(result.IPv4);
            Assert.Empty(result.IPv6);
            Assert.Equal(1, result.RelatedAsns.Count());
            var asn = result.RelatedAsns.First();
            Assert.Equal(123456, asn.ASN);
            Assert.Equal(string.Empty, asn.Name);
            Assert.Equal(string.Empty, asn.Description);
            Assert.Equal(string.Empty, asn.CountryCode);
            Assert.Equal(Enumerable.Empty<string>(), asn.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), asn.AbuseContacts);
        }

        [Fact]
        public void SearchForAnythingExceptAnAsNumberAlwaysReturnNullObject()
        {
            var result1 = GetService().SearchBy("cloudflare");
            Assert.NotNull(result1);
            Assert.Empty(result1.IPv4);
            Assert.Empty(result1.IPv6);
            Assert.Empty(result1.RelatedAsns);

            var result2 = GetService().SearchBy("8.8.8.8");
            Assert.NotNull(result2);
            Assert.Empty(result2.IPv4);
            Assert.Empty(result2.IPv6);
            Assert.Empty(result2.RelatedAsns);
        }

        [Fact]
        public async void SearchForAnythingExceptAnAsNumberAlwaysReturnNullObjectAsync()
        {
            var result1 = await GetService().SearchByAsync("abc");
            Assert.NotNull(result1);
            Assert.Empty(result1.IPv4);
            Assert.Empty(result1.IPv6);
            Assert.Empty(result1.RelatedAsns);

            var result2 = await GetService().SearchByAsync("2001:db8::/32");
            Assert.NotNull(result2);
            Assert.Empty(result2.IPv4);
            Assert.Empty(result2.IPv6);
            Assert.Empty(result2.RelatedAsns);
        }
    }
}