using System;
using System.Collections.Generic;
using BGPViewerCore.Service;

namespace BGPViewerConsoleTool
{
    public class Manager : IDisposable
    {
        public const string OPTION_ASN = "-a";
        public const string OPTION_IP = "-i";
        public const string OPTION_PREFIX = "-p";
        public const string OPTION_SEARCH = "-s";

        private readonly IDictionary<string, Func<int, string>> asnCommands;
        private readonly IDictionary<string, Func<string, string>> ipCommands;
        private readonly IDictionary<string, Func<(string, byte), string>> prefixCommands;
        private readonly Func<string, string> searchCommand;
        private readonly IBGPViewerService service;
        public Manager(IBGPViewerService service)
        {
            this.service = service;
            asnCommands = BuildAsnCommands(service);
            ipCommands = BuildIpCommands(service);
            prefixCommands = BuildPrefixCommands(service);
            searchCommand = (queryTerm) => Printer.PrintSearch(service.SearchBy(queryTerm));
        }

        // Façade
        public string Execute(string option, string optionValue, string command)
        {
            if(option.Equals(OPTION_IP))
            {
                if(ipCommands.ContainsKey(command))
                    return ipCommands[command].Invoke(optionValue);
                throw CommandNotFound(option, command);
            }
            else if(option.Equals(OPTION_PREFIX))
            {
                if(prefixCommands.ContainsKey(command))
                {
                    var splittedPrefix = optionValue.Split("/"); // splits 8.8.8.8/24 into 8.8.8.8 and 24 separately
                    if(splittedPrefix.Length == 2 && byte.TryParse(splittedPrefix[1], out byte cidr))
                        return prefixCommands[command].Invoke((splittedPrefix[0], cidr));
                    throw new ArgumentException($"{optionValue} is not at prefix/cidr format or cidr is not an integer!");
                }
                throw CommandNotFound(option, command);
            }
            else if(option.Equals(OPTION_ASN))
            {
                if(asnCommands.ContainsKey(command))
                {
                    if(int.TryParse(optionValue, out int asn))
                        return asnCommands[command].Invoke(asn);
                    throw new ArgumentException($"{optionValue} is not an integer!");
                }
                throw CommandNotFound(option, command);
            }
            else if(option.Equals(OPTION_SEARCH))
            {
                return searchCommand.Invoke(optionValue);
            }
            else
                throw new ArgumentOutOfRangeException(option, "Option provided not found!");
        }

        private Exception CommandNotFound(string option, string command) => new ArgumentOutOfRangeException(option, $"{option} option doesn't contains command {command}!");

        private IDictionary<string, Func<int, string>> BuildAsnCommands(IBGPViewerService service) => new Dictionary<string, Func<int, string>>
        {
            {
                "-d", 
                (asn) => Printer.PrintAsnDetails(service.GetAsnDetails(asn))
            },
            {
                "-px",
                (asn) => Printer.PrintAsnPrefixes(service.GetAsnPrefixes(asn))
            },
            {
                "-pr",
                (asn) => {
                    var peers = service.GetAsnPeers(asn);
                    return "====== IPv4 =======\n"+
                        Printer.PrintAsnsSet(peers.Item1)+
                        "\n====== IPv6 =======\n"+
                        Printer.PrintAsnsSet(peers.Item2);
                }
            },
            {
                "-up",
                (asn) => {
                    var upstreams = service.GetAsnUpstreams(asn);
                    return "====== IPv4 =======\n"+
                        Printer.PrintAsnsSet(upstreams.Item1)+
                        "\n====== IPv6 =======\n"+
                        Printer.PrintAsnsSet(upstreams.Item2);
                }
            },
            {
                "-dw",
                (asn) => {
                    var downstreams = service.GetAsnDownstreams(asn);
                    return "====== IPv4 =======\n"+
                        Printer.PrintAsnsSet(downstreams.Item1)+
                        "\n====== IPv6 =======\n"+
                        Printer.PrintAsnsSet(downstreams.Item2);
                }
            },
            {
                "-ix",
                (asn) => Printer.PrintIxSet(service.GetAsnIxs(asn))
            }
        };

        private IDictionary<string, Func<string, string>> BuildIpCommands(IBGPViewerService service) => new Dictionary<string, Func<string, string>>
        {
            {
                "-d", 
                (ipAddress) => { 
                    return Printer.PrintIPDetails(service.GetIpDetails(ipAddress));
                } 
            }
        };

        private IDictionary<string, Func<(string, byte), string>> BuildPrefixCommands(IBGPViewerService service) => new Dictionary<string, Func<(string, byte), string>>
        {
            {
                "-d",
                (t) => {
                    return Printer.PrintPrefixDetails(service.GetPrefixDetails(t.Item1, t.Item2));
                }
            }
        };

        public void Dispose() => service.Dispose();
    }
}