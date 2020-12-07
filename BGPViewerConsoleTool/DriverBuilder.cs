using System;
using OpenQA.Selenium;

namespace BGPViewerConsoleTool
{
    public abstract class DriverBuilder
    {
        public DriverBuilder() {}
        public abstract Uri ServiceUrl { get; }
        public abstract string DriverName { get; }
        public abstract string ExecutableName { get; }
        public abstract DriverOptions Options { get; }
        public abstract IWebDriver Build();
    }
}