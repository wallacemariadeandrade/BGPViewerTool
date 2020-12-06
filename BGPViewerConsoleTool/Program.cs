using System;
using System.Threading;
using static System.Console;

namespace BGPViewerConsoleTool
{
    class Program
    {
        private const string OPTION_HELP = "-h";
        private static bool isSearching;
        private static bool isLoading = false;
        private static Thread loader;

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

            using(var serviceBuilder = new ServiceBuilder(GetIntInput, WriteLine))
            {
                try
                {
                    serviceBuilder.AskForApi();

                    var manager = new Manager(serviceBuilder.Build());
                    
                    var option = args[0];
                    var optionValue = args[1];
                    var command = isSearching ? "" : args[2];

                    WriteLine("Loading data...");
                    Loading(true);
                    var result = manager.Execute(option, optionValue, command);
                    Loading(false);
                    WriteLine();
                    WriteLine(result);
                }
                catch (System.Exception ex)
                {
                    PrintHelpMessage(ex.Message);
                }
            }
        }

        private static void PrintHelpMessage(string message) => WriteLine($"\n *** {message} *** \n");
        private static int GetIntInput() => int.Parse(Console.ReadLine());
        private static void Loading(bool loading)
        {
            if(loading)
            {
                isLoading = true;

                loader = new Thread(() => {
                    while(isLoading)
                    {
                        Write("#");
                        Thread.Sleep(500);
                    }
                })
                {
                    IsBackground = true,
                    Name = "BGPViewerConsoleTool loader thread"
                };
             
                loader.Start();
            }
            else
            {
                isLoading = false;
            }
        }
    }
}
