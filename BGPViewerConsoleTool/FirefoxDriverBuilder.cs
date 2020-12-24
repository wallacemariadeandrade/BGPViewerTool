using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace BGPViewerConsoleTool
{
    public class FirefoxDriverBuilder : DriverBuilder
    {
        private FirefoxDriverService service;
        public override Uri ServiceUrl => service.ServiceUrl;
        public override string DriverName => "Firefox Driver";
        public override string ExecutableName => "geckodriver";
        public override DriverOptions Options => innerOptions;

        private FirefoxOptions innerOptions
        {
            get 
            {
                var options = new FirefoxOptions();
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

        public FirefoxDriverBuilder()
        {
            service = FirefoxDriverService.CreateDefaultService(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), $"{ExecutableName}.exe");
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;
        }

        public override IWebDriver Build()
        {
            try
            {
                return new FirefoxDriver(service, innerOptions);
            }
            catch(InvalidOperationException innerException)
            {
                System.Diagnostics.Process.GetProcessById(service.ProcessId).Kill();
                throw new InvalidOperationException("An error occurred while creating Firefox Driver. Check if your Firefox Browser is working and try again. (do you have it installed?)", innerException);
            }
        }
    }
}