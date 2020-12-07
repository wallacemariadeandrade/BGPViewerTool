using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BGPViewerConsoleTool
{
    public class ChromeDriverBuilder : DriverBuilder
    {
        private ChromeDriverService service;
        public override Uri ServiceUrl => service.ServiceUrl; 
        public override string DriverName => "Chrome Driver";
        public override string ExecutableName => "chromedriver";
        public override DriverOptions Options => innerOptions;

        private ChromeOptions innerOptions
        {
            get 
            {
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
                return options;
            }
        }

        public ChromeDriverBuilder()
        {
            service = ChromeDriverService.CreateDefaultService(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                service.EnableVerboseLogging = false;
                service.SuppressInitialDiagnosticInformation = true;
                service.HideCommandPromptWindow = true;
        }

        public override IWebDriver Build() => new ChromeDriver(service, innerOptions);
    }
}