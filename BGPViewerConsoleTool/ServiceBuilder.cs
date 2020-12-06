using System;
using System.IO;
using System.Text;
using BGPViewerCore.Service;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;

namespace BGPViewerConsoleTool
{
    public class ServiceBuilder : IDisposable
    {
        private const int OPTION_BGP_VIEW = 1;
        private const int OPTION_BGP_HE_CHROME = 2;
        private const int OPTION_BGP_HE_FIREFOX = 3;
        private const int OPTION_BGP_HE_EDGE = 4;
        private const int TIMEOUT = 7;
        private IWebDriver driver;
    
        private Func<int> optionGetter;
        private Action<string> messagePrinter;
        private int option = 0;

        public ServiceBuilder(Func<int> optionGetter, Action<string> messagePrinter)
        {
            this.optionGetter = optionGetter;
            this.messagePrinter = messagePrinter;
        }

        public IBGPViewerService Build()
        {
            if(option > 1)
            {
                if(option == OPTION_BGP_HE_CHROME)
                {
                    var chromeOptions = new ChromeOptions();

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

                    messagePrinter.Invoke("Creating the service...");
                    driver = new ChromeDriver(service, options);
                    return new BGPHeService(driver, TIMEOUT);
                }
                else if(option == OPTION_BGP_HE_FIREFOX)
                {
                    var service = FirefoxDriverService.CreateDefaultService(
                        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
                    service.SuppressInitialDiagnosticInformation = true;
                    service.HideCommandPromptWindow = true;

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
                    
                    messagePrinter.Invoke("Creating the service...");
                    driver = new FirefoxDriver(service, options);
                    return new BGPHeService(driver, TIMEOUT);
                }
                else
                {
                    var service = EdgeDriverService.CreateChromiumService(
                        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "msedgedriver.exe");
                    service.EnableVerboseLogging = false;
                    service.SuppressInitialDiagnosticInformation = true;
                    service.HideCommandPromptWindow = true;

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

                    messagePrinter.Invoke("Creating the service...");
                    driver = new EdgeDriver(service, options);
                    return new BGPHeService(driver, TIMEOUT);
                }
            }
            else
            {
                return new BGPViewerService(new BGPViewerWebApi());
            }
        }

        public void AskForApi()
        {
            var message = new StringBuilder()
                .AppendLine()
                .AppendLine("Select API to retrieve data from:")
                .AppendLine($"{OPTION_BGP_VIEW} / BGP View API")
                .AppendLine($"{OPTION_BGP_HE_CHROME} / BGP He API (needs Google Chrome installed)")
                .AppendLine($"{OPTION_BGP_HE_FIREFOX} / BGP He API (needs Mozilla Firefox installed)")
                .AppendLine($"{OPTION_BGP_HE_EDGE} / BGP He API (needs Microsoft Edge installed)")
                .AppendLine();

            messagePrinter.Invoke(message.ToString());
            option = optionGetter.Invoke();
        }

        public void Dispose() => this.driver?.Dispose();
    }
}