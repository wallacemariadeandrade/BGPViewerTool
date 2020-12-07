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
                    if(!CheckIfServiceIsAlreadyRunning(option))
                    {
                        messagePrinter.Invoke("Creating the service ...");
                        var service = new BGPHeService(driver.Build(), TIMEOUT);
                        messagePrinter.Invoke("Writing service to cache...");
                        SaveDriverToCache(option);
                        return service;
                    }

                    messagePrinter.Invoke("Loading service from cache...");
                    return new BGPHeService(LoadDriverFromCache(option), TIMEOUT);
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

        private IWebDriver LoadDriverFromCache(int driverIndex)
        {   
            var driver = driverDatabase[driverIndex];

            if(File.Exists(GetCacheFileName(driverIndex)))
            {
                var cacheDriverUrl = File.ReadAllText(GetCacheFileName(driverIndex));
                return new RemoteWebDriver(new Uri(cacheDriverUrl), driver.Options);
            }

            messagePrinter.Invoke("Cache file not found. Creating a new service...");
            var service = driver.Build();
            messagePrinter.Invoke("Writing service to cache...");
            SaveDriverToCache(driverIndex);
            return service;
        }

        private void SaveDriverToCache(int driverIndex) 
            => File.WriteAllText(GetCacheFileName(driverIndex), driverDatabase[driverIndex].ServiceUrl.ToString());
        private string GetCacheFileName(int driverIndex) => $"{driverIndex}_{driverDatabase[driverIndex].ExecutableName}";
        private bool CheckIfServiceIsAlreadyRunning(int driverIndex)
            => System.Diagnostics.Process.GetProcessesByName(driverDatabase[driverIndex].ExecutableName).Length > 0;
    }
}