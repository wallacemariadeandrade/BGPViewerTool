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
        public void GetAsnPrefixes()
        {
            var prefixes262950 = Service.GetAsnPrefixes(262950);
            Assert.Equal(262950, prefixes262950.ASN);
            Assert.True(prefixes262950.IPv4.Count() == 1);
            Assert.True(prefixes262950.IPv4.Count(x => x == "186.235.120.0/21") == 1);
            Assert.Empty(prefixes262950.IPv6);

            var prefixes53181 = Service.GetAsnPrefixes(53181);
            Assert.Equal(53181, prefixes53181.ASN);
            Assert.True(prefixes53181.IPv4.Count() == 34);
            Assert.True(prefixes53181.IPv4.Count(x => x == "191.241.78.0/23") == 1);
            Assert.True(prefixes53181.IPv4.Count(x => x == "201.33.173.0/24") == 1);
            Assert.True(prefixes53181.IPv4.Count(x => x == "191.241.64.0/20") == 1);
            Assert.True(prefixes53181.IPv6.Count() == 13);
            Assert.True(prefixes53181.IPv6.Count(x => x == "2804:113c:9400::/38") == 1);
            Assert.True(prefixes53181.IPv6.Count(x => x == "2804:113c::/32") == 1);
            Assert.True(prefixes53181.IPv6.Count(x => x == "2804:113c:8c00::/38") == 1);
        }

        [Fact]
        public async void GetAsnPrefixesAsync()
        {
            var prefixes262950 = await Service.GetAsnPrefixesAsync(262950);
            Assert.Equal(262950, prefixes262950.ASN);
            Assert.True(prefixes262950.IPv4.Count() == 1);
            Assert.True(prefixes262950.IPv4.Count(x => x == "186.235.120.0/21") == 1);
            Assert.Empty(prefixes262950.IPv6);

            var prefixes53181 = await Service.GetAsnPrefixesAsync(53181);
            Assert.Equal(53181, prefixes53181.ASN);
            Assert.True(prefixes53181.IPv4.Count() == 34);
            Assert.True(prefixes53181.IPv4.Count(x => x == "191.241.78.0/23") == 1);
            Assert.True(prefixes53181.IPv4.Count(x => x == "201.33.173.0/24") == 1);
            Assert.True(prefixes53181.IPv4.Count(x => x == "191.241.64.0/20") == 1);
            Assert.True(prefixes53181.IPv6.Count() == 13);
            Assert.True(prefixes53181.IPv6.Count(x => x == "2804:113c:9400::/38") == 1);
            Assert.True(prefixes53181.IPv6.Count(x => x == "2804:113c::/32") == 1);
            Assert.True(prefixes53181.IPv6.Count(x => x == "2804:113c:8c00::/38") == 1);
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

        [Fact]
        public async void GetIpDetailsAsync()
        {
            var details191 = await Service.GetIpDetailsAsync("191.241.64.0");
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

            var details2001 = await Service.GetIpDetailsAsync("2001:db8::");
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

        [Fact]
        public void TryGetIpAddressDetailsWithMalformedInput()
        {
            Assert.Throws<ArgumentException>(() => Service.GetIpDetails("192.168"));
            Assert.Throws<ArgumentException>(() => Service.GetIpDetails("177.75.40.256"));
            Assert.Throws<ArgumentException>(() => Service.GetIpDetails("192.168.10.10.1"));
            Assert.Throws<ArgumentException>(() => Service.GetIpDetails("2001::db8::"));
            Assert.Throws<ArgumentException>(() => Service.GetIpDetails("2001:db8:1:"));
        }

        [Fact]
        public async void TryGetIpAddressDetailsWithMalformedInputAsync()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => Service.GetIpDetailsAsync("192.168"));
            await Assert.ThrowsAsync<ArgumentException>(() => Service.GetIpDetailsAsync("177.75.40.256"));
            await Assert.ThrowsAsync<ArgumentException>(() => Service.GetIpDetailsAsync("192.168.10.10.1"));
            await Assert.ThrowsAsync<ArgumentException>(() => Service.GetIpDetailsAsync("2001::db8::"));
            await Assert.ThrowsAsync<ArgumentException>(() => Service.GetIpDetailsAsync("2001:db8:1:"));
        }

        [Fact]
        public void GetPrefixDetails()
        {
            var details191 = Service.GetPrefixDetails("191.241.64.0", 20);
            Assert.Equal("191.241.64.0/20", details191.Prefix);
            Assert.Equal("IANA IPv4 Address Space Registry", details191.Name);
            Assert.Equal("Administered by LACNIC", details191.Description);
            Assert.True(details191.ParentAsns.Count() == 1);
            Assert.Equal(53181, details191.ParentAsns.First().ASN);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", details191.ParentAsns.First().Name);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", details191.ParentAsns.First().Description);
            Assert.Equal(string.Empty, details191.ParentAsns.First().CountryCode);
            
            var details2001 = Service.GetPrefixDetails("2804::113c", 32);
            Assert.Equal("2804::113c/32", details2001.Prefix);
            Assert.Equal("IANA IPv6 Global Unicast Address Assignments", details2001.Name);
            Assert.Equal("Designated to LACNIC on 03 October 2006 (Status: allocated; Note: 2800:0000::/23 was allocated on 2005-11-17. The more recent allocation (2006-10-03) incorporates the \nprevious allocation.)", details2001.Description);
            Assert.True(details2001.ParentAsns.Count() == 1);
            Assert.Equal(2716, details2001.ParentAsns.First().ASN);
            Assert.Equal("Universidade Federal do Rio Grande do Sul", details2001.ParentAsns.First().Name);
            Assert.Equal("Universidade Federal do Rio Grande do Sul", details2001.ParentAsns.First().Description);
            Assert.Equal(string.Empty, details2001.ParentAsns.First().CountryCode);
        }

        [Fact]
        public async void GetPrefixDetailsAsync()
        {
            var details191 = await Service.GetPrefixDetailsAsync("191.241.64.0", 20);
            Assert.Equal("191.241.64.0/20", details191.Prefix);
            Assert.Equal("IANA IPv4 Address Space Registry", details191.Name);
            Assert.Equal("Administered by LACNIC", details191.Description);
            Assert.True(details191.ParentAsns.Count() == 1);
            Assert.Equal(53181, details191.ParentAsns.First().ASN);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", details191.ParentAsns.First().Name);
            Assert.Equal("K2 Telecom e Multimidia LTDA ME", details191.ParentAsns.First().Description);
            Assert.Equal(string.Empty, details191.ParentAsns.First().CountryCode);
            
            var details2001 = await Service.GetPrefixDetailsAsync("2804::113c", 32);
            Assert.Equal("2804::113c/32", details2001.Prefix);
            Assert.Equal("IANA IPv6 Global Unicast Address Assignments", details2001.Name);
            Assert.Equal("Designated to LACNIC on 03 October 2006 (Status: allocated; Note: 2800:0000::/23 was allocated on 2005-11-17. The more recent allocation (2006-10-03) incorporates the \nprevious allocation.)", details2001.Description);
            Assert.True(details2001.ParentAsns.Count() == 1);
            Assert.Equal(2716, details2001.ParentAsns.First().ASN);
            Assert.Equal("Universidade Federal do Rio Grande do Sul", details2001.ParentAsns.First().Name);
            Assert.Equal("Universidade Federal do Rio Grande do Sul", details2001.ParentAsns.First().Description);
            Assert.Equal(string.Empty, details2001.ParentAsns.First().CountryCode);
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