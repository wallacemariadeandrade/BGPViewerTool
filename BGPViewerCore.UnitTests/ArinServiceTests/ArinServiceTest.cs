using System.Linq;
using BGPViewerCore.Service;
using Xunit;

namespace BGPViewerCore.UnitTests.ArinServiceTests
{
    public class ArinServiceTest : IClassFixture<ArinMockWebApi>
    {
        public ArinService Service { get; }

        public ArinServiceTest(ArinMockWebApi api)
        {
            Service = new ArinService(api);
        }

        [Fact]
        public void GetAsnDetails()
        {
            var details53181 = Service.GetAsnDetails(53181);
            Assert.Equal(52224, details53181.ASN);
            Assert.Equal("LACNIC-52224", details53181.Name);
            Assert.Equal("This AS is under LACNIC responsibility for further allocations to users in LACNIC region. Please see http://www.lacnic.net/ for further details, or check the WHOIS server located at http://whois.lacnic.net", details53181.Description);
            Assert.Equal(Enumerable.Empty<string>(), details53181.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), details53181.AbuseContacts);
            Assert.Equal(string.Empty, details53181.CountryCode);
            Assert.Equal(string.Empty, details53181.LookingGlassUrl);

            var details3356 = Service.GetAsnDetails(3356);
            Assert.Equal(3356, details3356.ASN);
            Assert.Equal("LEVEL3", details3356.Name);
            Assert.Equal("LEVEL3", details3356.Description);
            Assert.Equal(Enumerable.Empty<string>(), details3356.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), details3356.AbuseContacts);
            Assert.Equal(string.Empty, details3356.CountryCode);
            Assert.Equal(string.Empty, details3356.LookingGlassUrl);
        }

        [Fact]
        public async void GetAsnDetailsAsync()
        {
            var details53181 = await Service.GetAsnDetailsAsync(53181);
            Assert.Equal(52224, details53181.ASN);
            Assert.Equal("LACNIC-52224", details53181.Name);
            Assert.Equal("This AS is under LACNIC responsibility for further allocations to users in LACNIC region. Please see http://www.lacnic.net/ for further details, or check the WHOIS server located at http://whois.lacnic.net", details53181.Description);
            Assert.Equal(Enumerable.Empty<string>(), details53181.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), details53181.AbuseContacts);
            Assert.Equal(string.Empty, details53181.CountryCode);
            Assert.Equal(string.Empty, details53181.LookingGlassUrl);

            var details3356 = await Service.GetAsnDetailsAsync(3356);
            Assert.Equal(3356, details3356.ASN);
            Assert.Equal("LEVEL3", details3356.Name);
            Assert.Equal("LEVEL3", details3356.Description);
            Assert.Equal(Enumerable.Empty<string>(), details3356.EmailContacts);
            Assert.Equal(Enumerable.Empty<string>(), details3356.AbuseContacts);
            Assert.Equal(string.Empty, details3356.CountryCode);
            Assert.Equal(string.Empty, details3356.LookingGlassUrl);
        }

        [Fact]
        public void GetDetailsFromUnregisteredAs()
        {
            var unregisteredDetails = Service.GetAsnDetails(123456);
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
            var downstreams16509 = Service.GetAsnDownstreams(16509);
            Assert.Empty(downstreams16509.Item1);
            Assert.Empty(downstreams16509.Item2);
        }

        [Fact]
        public async void GetAsnDownstreamsAlwaysReturnNullObjectAsync()
        {
            var downstreams16509 = await Service.GetAsnDownstreamsAsync(16509);
            Assert.Empty(downstreams16509.Item1);
            Assert.Empty(downstreams16509.Item2);
        }

        [Fact]
        public void GetAsnIxsAlwaysReturnNullObject()
        {
            var ixs = Service.GetAsnIxs(1234);
            Assert.Empty(ixs);
        }
        
        [Fact]
        public async void GetAsnIxsAlwaysReturnNullObjectAsync()
        {
            var ixs = await Service.GetAsnIxsAsync(1234);
            Assert.Empty(ixs);
        }

        [Fact]
        public void GetAsnPeersAlwaysReturnNullObject()
        {
            var peers1234 = Service.GetAsnPeers(1234);
            Assert.Empty(peers1234.Item1);
            Assert.Empty(peers1234.Item2);
        }

        [Fact]
        public async void GetAsnPeersAlwaysReturnNullObjectAsync()
        {
            var peers1234 = await Service.GetAsnPeersAsync(1234);
            Assert.Empty(peers1234.Item1);
            Assert.Empty(peers1234.Item2);
        }

        [Fact]
        public void GetAsnPrefixesAlwaysReturnNullObject()
        {
            var prefixes = Service.GetAsnPrefixes(1234);
            Assert.Equal(1234, prefixes.ASN);
            Assert.Empty(prefixes.IPv4);
            Assert.Empty(prefixes.IPv6);
        }

        [Fact]
        public async void GetAsnPrefixesAlwaysReturnNullObjectAsync()
        {
            var prefixes = await Service.GetAsnPrefixesAsync(1234);
            Assert.Equal(1234, prefixes.ASN);
            Assert.Empty(prefixes.IPv4);
            Assert.Empty(prefixes.IPv6);
        }

        [Fact]
        public void GetAsnUpstreamsAlwaysReturnNullObject()
        {
            var upstreams1234 = Service.GetAsnUpstreams(1234);
            Assert.Empty(upstreams1234.Item1);
            Assert.Empty(upstreams1234.Item2);
        }

        [Fact]
        public async void GetAsnUpstreamsAlwaysReturnNullObjectAsync()
        {
            var upstreams1234 = await Service.GetAsnUpstreamsAsync(1234);
            Assert.Empty(upstreams1234.Item1);
            Assert.Empty(upstreams1234.Item2);
        }
    }
}