using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BGPViewerCore.UnitTests.BGPHeServiceTests
{
    public class BGPHeMockWebDriver : IWebDriver
    {
        private readonly IWebDriver innerDriver;
        private readonly MockNavigation innerNavigation;

        public BGPHeMockWebDriver()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            service.EnableVerboseLogging = false;
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            options.PageLoadStrategy = PageLoadStrategy.Normal;
            options.AddArguments("headless");
            options.AddArgument("no-sandbox");
            options.AddArgument("disable-gpu");
            options.AddArgument("disable-crash-reporter");
            options.AddArgument("disable-extensions");
            options.AddArgument("disable-in-process-stack-traces");
            options.AddArgument("disable-logging");
            options.AddArgument("disable-dev-shm-usage");
            options.AddArgument("log-level=3");
            options.AddArgument("output=/dev/null");
            innerDriver = new ChromeDriver(service, options);
            innerNavigation = new MockNavigation(innerDriver);
        }

        public string Url { 
            get => innerNavigation.Url;
            set {
                innerNavigation.Url = value;
            }
        }
        public string Title => innerDriver.Title;
        public string PageSource => innerDriver.PageSource;
        public string CurrentWindowHandle => innerDriver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => innerDriver.WindowHandles;
        public void Close() => innerDriver.Close();
        public void Dispose() => innerDriver.Dispose();
        public IWebElement FindElement(By by) => innerDriver.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => innerDriver.FindElements(by);
        public IOptions Manage() => innerDriver.Manage();
        public INavigation Navigate() => this.innerNavigation;
        public void Quit() => innerDriver.Quit();
        public ITargetLocator SwitchTo() => innerDriver.SwitchTo();

        private class MockNavigation : INavigation
        {
            protected IWebDriver mockDriver;
            public string Url { get; set; }

            private Dictionary<string, string> mocksUrls = new Dictionary<string, string>
            {
                { "https://bgp.he.net/AS15169" , GetPageUrlFor("AS15169MockData.html") },
                { "https://bgp.he.net/AS268003" , GetPageUrlFor("AS268003MockData.html") },
                { "https://bgp.he.net/AS53181" , GetPageUrlFor("AS53181MockData.html") },
                { "https://bgp.he.net/ip/8.8.8.8" , GetPageUrlFor("8.8.8.8_MockData.html") },
                { "https://bgp.he.net/ip/196.100.100.0" , GetPageUrlFor("196.100.100.0_MockData.html") },
                { "https://bgp.he.net/net/8.8.8.0/24" , GetPageUrlFor("8.8.8.0-24_MockData.html") },
                { "https://bgp.he.net/net/196.96.0.0/12" , GetPageUrlFor("196.96.0.0-12_MockData.html") },
                { "https://bgp.he.net/net/2001:4860::/32" , GetPageUrlFor("2001..4860....32_ MockData.html") },
                { "https://bgp.he.net/net/2a02:26f0:128::/48" , GetPageUrlFor("2a02..26f0..128....48_MockData.html") },
                { "https://bgp.he.net/search?search%5Bsearch%5D=ascenty&commit=Search" , GetPageUrlFor("searchby-ascenty_MockData.html") },
                { "default" , GetPageUrlFor("ASNotFoundMockData.html") }
            };

            public MockNavigation(IWebDriver mockDriver)
            {
                this.mockDriver = mockDriver;
            }

            public void Back() => mockDriver.Navigate().Back();
            public void Forward() => mockDriver.Navigate().Forward();

            private static string GetPageUrlFor(string pageName)
            {
                var testsProjectDirectory = Directory.GetCurrentDirectory();
                return Path.Combine("file:///", testsProjectDirectory, "BGPHeServiceMockTestData", pageName);
            }

            public void GoToUrl(string url)
            {
                Url = url; // To simulate real redirection at bgp.he.net
                
                if(mocksUrls.ContainsKey(url))
                {
                    System.Diagnostics.Debug.Print(mocksUrls[url]);
                    Console.WriteLine(mocksUrls[url]);
                    mockDriver.Navigate().GoToUrl(mocksUrls[url]);
                    mockDriver.Url = mocksUrls[url];
                }
                else
                {
                    mockDriver.Navigate().GoToUrl(mocksUrls["default"]);
                    mockDriver.Url = mocksUrls["default"];
                }
            }
            public void GoToUrl(Uri url) => mockDriver.Navigate().GoToUrl(url);
            public void Refresh() => mockDriver.Navigate().Refresh();
        }
    }
}