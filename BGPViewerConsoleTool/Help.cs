namespace BGPViewerConsoleTool
{
    public static class Help
    {
        public static string Get => @"
Usage: dotnet BGPViewerConsoleTool [options] <value> [command]
Usage: dotnet BGPViewerConsoleTool -s <search_value>

Options:
    -a          AS number       (e.g. -a 53181)
    -p          Prefix          (e.g  -p 8.8.8.8/24)
    -i          IP              (e.g  -i 8.8.8.8)
    -s          Search by       (e.g  -s 8.8.8.8 or -s 6762 or -s ""Century Link"")

Commands: 
    
With -a option:

    -d          AS details
    -px         AS prefixes
    -pr         AS peers
    -up         AS upstreams
    -dw         AS downstreams
    -ix         AS IXs

With -p option:

    -d          Prefix details

With -i option:

    -d          IP details
";
    }
}