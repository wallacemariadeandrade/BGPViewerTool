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
    }
}