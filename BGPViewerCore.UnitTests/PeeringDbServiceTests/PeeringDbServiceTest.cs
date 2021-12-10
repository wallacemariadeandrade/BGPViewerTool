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
        public void GetAsnDownstreams()
        {
            var downstreams16509 = GetService().GetAsnDownstreams(16509);
            Assert.Empty(downstreams16509.Item1);
            Assert.Empty(downstreams16509.Item2);
        }

        [Fact]
        public async void GetAsnDownstreamsAsync()
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
    }
}