using System;
using System.IO;
using System.Text;
using BGPViewerCore.Service;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;

namespace BGPViewerConsoleTool
{
    public class ServiceManager
    {
        private const int DEFAULT_DRIVER_INDEX = 0;
        private const int AVAILABLE_DRIVER_START_INDEX = 1;
        private const int TIMEOUT = 7;
        private int option;
        private Func<int> optionGetter;
        private Action<string> messagePrinter;

        private Dictionary<int, DriverBuilder> driverDatabase;

        public ServiceManager(Func<int> optionGetter, Action<string> messagePrinter, IEnumerable<DriverBuilder> availableDrivers)
        {
            this.optionGetter = optionGetter;
            this.messagePrinter = messagePrinter;

            driverDatabase = new Dictionary<int, DriverBuilder>();
            var index = AVAILABLE_DRIVER_START_INDEX;
            foreach(var driver in availableDrivers)
            {
                driverDatabase.Add(index, driver);
                index++;
            }
        }

        public IBGPViewerService Build()
        {
            if(option != DEFAULT_DRIVER_INDEX)
            {
                if(driverDatabase.ContainsKey(option))
                {
                    var driver = driverDatabase[option];
                    messagePrinter.Invoke("Creating the service ...");
                    var service = new BGPHeService(driver.Build(), TIMEOUT);
                    messagePrinter.Invoke("Service is ready.");
                    return service;
                }
                else
                    throw new ArgumentException("This option doesn't exist. Try again.", nameof(option));
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
                .AppendLine($"{DEFAULT_DRIVER_INDEX} / BGP View API");
                
            foreach(var driver in driverDatabase) message.AppendLine($"{driver.Key} / BGP He with {driver.Value.DriverName}");

            message.AppendLine();

            messagePrinter.Invoke(message.ToString());
            option = optionGetter.Invoke();
        }
    }
}