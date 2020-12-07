using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace BGPViewerConsoleTool
{
    public class MsEdgeDriverBuilder : DriverBuilder
    {
        private EdgeDriverService service;
        public override Uri ServiceUrl => service.ServiceUrl;
        public override string DriverName => "Microsoft Edge Driver";
        public override string ExecutableName => "msedgedriver";
        public override DriverOptions Options => innerOptions;

        private EdgeOptions innerOptions
        {
            get
            {
                var options = new EdgeOptions();
                options.UseChromium = true;
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

        public MsEdgeDriverBuilder()
        {
            service = EdgeDriverService.CreateChromiumService(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), $"{ExecutableName}.exe");
            service.EnableVerboseLogging = false;
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;
        }

        public override IWebDriver Build() => new EdgeDriver(service, innerOptions);
    }
}