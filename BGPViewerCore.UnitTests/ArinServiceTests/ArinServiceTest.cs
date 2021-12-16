using System;
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
        public void TryGettingInvalidAsnDetails()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                Service.GetAsnDetails(101010101);
            });
        }

        [Fact]
        public async void TryGettingInvalidAsnDetailsAsync()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => Service.GetAsnDetailsAsync(101010101));
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

        [Fact]
        public void GetIpDetails()
        {
            var ip8888 = Service.GetIpDetails("8.8.8.8");
            Assert.Equal("8.8.8.8", ip8888.IPAddress);
            Assert.Equal(string.Empty, ip8888.PtrRecord);
            Assert.Equal("8.8.8.0/24", ip8888.RIRAllocationPrefix);
            Assert.Equal(string.Empty, ip8888.CountryCode);
            Assert.True(ip8888.RelatedPrefixes.Count() == 1);
            var p8888 = ip8888.RelatedPrefixes.First();
            Assert.Equal("8.8.8.0/24", p8888.Prefix);
            Assert.Equal("LVLT-GOGL-8-8-8", p8888.Name);
            Assert.Equal("Reallocated", p8888.Description);
            Assert.Empty(p8888.ParentAsns);

            var ipv6 = Service.GetIpDetails("2804:2a7c::2a");
            Assert.Equal("2804:2a7c::2a", ipv6.IPAddress);
            Assert.Equal(string.Empty, ipv6.PtrRecord);
            Assert.Equal("2800::/12", ipv6.RIRAllocationPrefix);
            Assert.Equal(string.Empty, ipv6.CountryCode);
            Assert.True(ipv6.RelatedPrefixes.Count() == 1);
            var ipv6Prefix = ipv6.RelatedPrefixes.First();
            Assert.Equal("2800::/12", ipv6Prefix.Prefix);
            Assert.Equal("LACNIC-V6-NET", ipv6Prefix.Name);
            Assert.Equal("Allocated to LACNIC", ipv6Prefix.Description);
            Assert.Empty(ipv6Prefix.ParentAsns);
        }

        [Fact]
        public async void GetIpDetailsAsync()
        {
            var ip8888 = await Service.GetIpDetailsAsync("8.8.8.8");
            Assert.Equal("8.8.8.8", ip8888.IPAddress);
            Assert.Equal(string.Empty, ip8888.PtrRecord);
            Assert.Equal("8.8.8.0/24", ip8888.RIRAllocationPrefix);
            Assert.Equal(string.Empty, ip8888.CountryCode);
            Assert.True(ip8888.RelatedPrefixes.Count() == 1);
            var p8888 = ip8888.RelatedPrefixes.First();
            Assert.Equal("8.8.8.0/24", p8888.Prefix);
            Assert.Equal("LVLT-GOGL-8-8-8", p8888.Name);
            Assert.Equal("Reallocated", p8888.Description);
            Assert.Empty(p8888.ParentAsns);

            var ipv6 = await Service.GetIpDetailsAsync("2804:2a7c::2a");
            Assert.Equal("2804:2a7c::2a", ipv6.IPAddress);
            Assert.Equal(string.Empty, ipv6.PtrRecord);
            Assert.Equal("2800::/12", ipv6.RIRAllocationPrefix);
            Assert.Equal(string.Empty, ipv6.CountryCode);
            Assert.True(ipv6.RelatedPrefixes.Count() == 1);
            var ipv6Prefix = ipv6.RelatedPrefixes.First();
            Assert.Equal("2800::/12", ipv6Prefix.Prefix);
            Assert.Equal("LACNIC-V6-NET", ipv6Prefix.Name);
            Assert.Equal("Allocated to LACNIC", ipv6Prefix.Description);
            Assert.Empty(ipv6Prefix.ParentAsns);
        }

        [Fact]
        public void TryGetIpAddressDetailsWithMalformedInput()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                Service.GetIpDetails("192.168");
            });

            Assert.Throws<ArgumentException>(() => 
            {
                Service.GetIpDetails("177.75.40.256");
            });

            Assert.Throws<ArgumentException>(() => 
            {
                Service.GetIpDetails("192.168.10.10.1");
            });

            Assert.Throws<ArgumentException>(() => 
            {
                Service.GetIpDetails("2001:db8:");
            });
        }

        [Fact]
        public async void TryGetIpAddressDetailsWithMalformedInputAsync()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => Service.GetIpDetailsAsync("192.168"));
            await Assert.ThrowsAsync<ArgumentException>(() => Service.GetIpDetailsAsync("177.75.40.256"));
            await Assert.ThrowsAsync<ArgumentException>(() => Service.GetIpDetailsAsync("192.168.10.10.1"));
        }

        [Fact]
        public void GetPrefixDetailsAlwaysReturnNullObject()
        {
            var details = Service.GetPrefixDetails("8.8.8.0", 24);
            Assert.Equal(string.Empty, details.Name);
            Assert.Equal(string.Empty, details.Description);
            Assert.Equal("8.8.8.0/24", details.Prefix);
            Assert.Empty(details.ParentAsns);
        }

        [Fact]
        public async void GetPrefixDetailsAlwaysReturnNullObjectAsync()
        {
            var details = await Service.GetPrefixDetailsAsync("1.1.1.0", 24);
            Assert.Equal(string.Empty, details.Name);
            Assert.Equal(string.Empty, details.Description);
            Assert.Equal("1.1.1.0/24", details.Prefix);
            Assert.Empty(details.ParentAsns);
        }

        [Fact]
        public void SearchByAlwaysReturnNullObject()
        {
            var result1 = Service.SearchBy("abc");
            Assert.Empty(result1.IPv4);
            Assert.Empty(result1.IPv6);
            Assert.Empty(result1.RelatedAsns);

            var result2 = Service.SearchBy("google");
            Assert.Empty(result2.IPv4);
            Assert.Empty(result2.IPv6);
            Assert.Empty(result2.RelatedAsns);

            var result3 = Service.SearchBy("1.1.1.1");
            Assert.Empty(result3.IPv4);
            Assert.Empty(result3.IPv6);
            Assert.Empty(result3.RelatedAsns);
        }
        
        [Fact]
        public async void SearchByAlwaysReturnNullObjectAsync()
        {
            var result1 = await Service.SearchByAsync("abc");
            Assert.Empty(result1.IPv4);
            Assert.Empty(result1.IPv6);
            Assert.Empty(result1.RelatedAsns);

            var result2 = await Service.SearchByAsync("google");
            Assert.Empty(result2.IPv4);
            Assert.Empty(result2.IPv6);
            Assert.Empty(result2.RelatedAsns);

            var result3 = await Service.SearchByAsync("1.1.1.1");
            Assert.Empty(result3.IPv4);
            Assert.Empty(result3.IPv6);
            Assert.Empty(result3.RelatedAsns);
        }
    }
}