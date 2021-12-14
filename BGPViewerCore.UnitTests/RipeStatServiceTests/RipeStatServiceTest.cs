using System;
using System.Linq;
using BGPViewerCore.Service;
using Xunit;

namespace BGPViewerCore.UnitTests.RipeStatServiceTests
{
    public class RipeStatServiceTests : IClassFixture<RipeStatMockWebApi>
    {
        public RipeStatService Service { get; }

        public RipeStatServiceTests(RipeStatMockWebApi api)
        {
            Service = new RipeStatService(api);
        }

        [Fact]
        public void GetAsnDetails()
        {
            var details1299 = Service.GetAsnDetails(1299);
            Assert.Equal(1299, details1299.ASN);
            Assert.Equal("Assigned by ARIN - IANA 16-bit Autonomous System (AS) Numbers Registry", details1299.Description);
            Assert.Equal("TWELVE99 - Telia Company AB", details1299.Name);
            Assert.Empty(details1299.AbuseContacts);
            Assert.Empty(details1299.EmailContacts);
            Assert.Equal(string.Empty, details1299.LookingGlassUrl);
            Assert.Equal(string.Empty, details1299.CountryCode);
            
            var details264075 = Service.GetAsnDetails(264075);
            Assert.Equal(264075, details264075.ASN);
            Assert.Equal("Assigned by LACNIC - IANA 32-bit Autonomous System (AS) Numbers Registry", details264075.Description);
            Assert.Equal("K1 Telecom e Multimidia LTDA", details264075.Name);
            Assert.Empty(details264075.AbuseContacts);
            Assert.Empty(details264075.EmailContacts);
            Assert.Equal(string.Empty, details264075.LookingGlassUrl);
            Assert.Equal(string.Empty, details264075.CountryCode);
        }

        [Fact]
        public async void GetAsnDetailsAsync()
        {
            var details1299 = await Service.GetAsnDetailsAsync(1299);
            Assert.Equal(1299, details1299.ASN);
            Assert.Equal("Assigned by ARIN - IANA 16-bit Autonomous System (AS) Numbers Registry", details1299.Description);
            Assert.Equal("TWELVE99 - Telia Company AB", details1299.Name);
            Assert.Empty(details1299.AbuseContacts);
            Assert.Empty(details1299.EmailContacts);
            Assert.Equal(string.Empty, details1299.LookingGlassUrl);
            Assert.Equal(string.Empty, details1299.CountryCode);
            
            var details264075 = await Service.GetAsnDetailsAsync(264075);
            Assert.Equal(264075, details264075.ASN);
            Assert.Equal("Assigned by LACNIC - IANA 32-bit Autonomous System (AS) Numbers Registry", details264075.Description);
            Assert.Equal("K1 Telecom e Multimidia LTDA", details264075.Name);
            Assert.Empty(details264075.AbuseContacts);
            Assert.Empty(details264075.EmailContacts);
            Assert.Equal(string.Empty, details264075.LookingGlassUrl);
            Assert.Equal(string.Empty, details264075.CountryCode);
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
            await Assert.ThrowsAsync<ArgumentException>(() => 
                Service.GetAsnDetailsAsync(101010101)
            );
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
            var details191 = Service.GetIpDetails("191.241.64.0");
            Assert.Equal("191.241.64.0", details191.IPAddress);
            Assert.Equal(string.Empty, details191.PtrRecord);
            Assert.Equal("191.241.64.0/24", details191.RIRAllocationPrefix);
            Assert.Equal(string.Empty, details191.CountryCode);
            Assert.True(details191.RelatedPrefixes.Count() == 1);
            var prefix191 = details191.RelatedPrefixes.First();
            Assert.Equal(string.Empty, prefix191.Description);
            Assert.Equal(string.Empty, prefix191.Name);
            Assert.Equal("191.241.64.0/24", prefix191.Prefix);
            Assert.True(prefix191.ParentAsns.Count() == 1);
            var asn191 = prefix191.ParentAsns.First();
            Assert.Equal(53181, asn191.ASN);
            Assert.Equal(string.Empty, asn191.Name);
            Assert.Equal(string.Empty, asn191.Description);
            Assert.Equal(string.Empty, asn191.CountryCode);

            var details2001 = Service.GetIpDetails("2001:db8::");
            Assert.Equal("2001:db8::", details2001.IPAddress);
            Assert.Equal(string.Empty, details2001.PtrRecord);
            Assert.Equal("2000::/3", details2001.RIRAllocationPrefix);
            Assert.Equal(string.Empty, details2001.CountryCode);
            Assert.True(details2001.RelatedPrefixes.Count() == 1);
            var prefix2001 = details2001.RelatedPrefixes.First();
            Assert.Equal(string.Empty, prefix2001.Description);
            Assert.Equal(string.Empty, prefix2001.Name);
            Assert.Equal("2000::/3", prefix2001.Prefix);
            Assert.True(prefix2001.ParentAsns.Count() == 1);
            var asn2001 = prefix2001.ParentAsns.First();
            Assert.Equal(131111, asn2001.ASN);
            Assert.Equal(string.Empty, asn2001.Name);
            Assert.Equal(string.Empty, asn2001.Description);
            Assert.Equal(string.Empty, asn2001.CountryCode);
        }

        // ip details async
        // ip malformed
    }
}