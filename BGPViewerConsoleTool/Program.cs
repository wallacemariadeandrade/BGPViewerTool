using System;
using System.Collections.Generic;
using System.Linq;
using BGPViewerCore.Service;
using static System.Console;

namespace BGPViewerConsoleTool
{
    class Program
    {
        private const string OPTION_HELP = "-h";
        private static bool isSearching;

        static void Main(string[] args)
        {
            // Validation / Printing help messages
            if(args.Length == 1 && args[0] == OPTION_HELP)
            {
                WriteLine(Help.Get);
                return;
            }
            else if(args.Length == 2 && args[0] == Manager.OPTION_SEARCH)
            {
                isSearching = true; // an turnaround
            }
            else if(args.Length < 3)
            {
                PrintHelpMessage($"{args.Length} args found, are you forgetting something?");
                WriteLine(Help.Get);
                return;
            }

            var service = new BGPViewerService(new BGPViewerWebApi());
            var manager = new Manager(service);
            
            var option = args[0];
            var optionValue = args[1];
            var command = isSearching ? "" : args[2];

            try
            {
                WriteLine(manager.Execute(option, optionValue, command));
            }
            catch (System.Exception ex)
            {
                PrintHelpMessage(ex.Message);
            }
        }

        private static void PrintHelpMessage(string message) => WriteLine($"\n *** {message} *** \n");
    }
}
