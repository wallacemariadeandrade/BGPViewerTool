using BGPViewerCore.Service;
using OpenQA.Selenium.Chrome;

namespace Xunit
{
    public class BGPHeTest : IClassFixture<BGPHeMockWebDriver>
    {
        private IBGPViewerService Service { get; }

        public BGPHeTest(BGPHeMockWebDriver driver)
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
    }
}