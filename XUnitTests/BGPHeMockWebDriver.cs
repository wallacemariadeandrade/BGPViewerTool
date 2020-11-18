using System;
using System.Collections.ObjectModel;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Xunit
{
    public class BGPHeMockWebDriver : IWebDriver
    {
        private readonly IWebDriver innerDriver;

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
        }

        public string Url { 
            get => innerDriver.Url; 
            set {
                innerDriver.Url = value;
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
        public INavigation Navigate() => new MockNavigation(innerDriver);
        public void Quit() => innerDriver.Quit();
        public ITargetLocator SwitchTo() => innerDriver.SwitchTo();

        private class MockNavigation : INavigation
        {
            protected IWebDriver mockDriver;

            public MockNavigation(IWebDriver mockDriver)
            {
                this.mockDriver = mockDriver;
            }

            public void Back() => mockDriver.Navigate().Back();
            public void Forward() => mockDriver.Navigate().Forward();
            public void GoToUrl(string url)
            {
                var testsProjectDirectory = System.IO.Path.GetFullPath(@"..\..\");
                switch(url)
                {
                    case "https://bgp.he.net/AS15169":
                        mockDriver.Navigate().GoToUrl($"file:///{testsProjectDirectory}/AS15169MockData.html");
                        mockDriver.Url = url;
                        break;
                    
                    case "https://bgp.he.net/AS268003":
                        mockDriver.Navigate().GoToUrl($"file:///{testsProjectDirectory}/AS268003MockData.html");
                        mockDriver.Url = url;
                        break;

                    case "https://bgp.he.net/AS53181":
                        mockDriver.Navigate().GoToUrl($"file:///{testsProjectDirectory}/AS53181MockData.html");
                        mockDriver.Url = url;
                        break;

                    default:
                        throw new Exception($"URL {url} was not mocked for unit testing.");
                }
            }
            public void GoToUrl(Uri url) => mockDriver.Navigate().GoToUrl(url);
            public void Refresh() => mockDriver.Navigate().Refresh();
        }
    }
}