namespace BGPViewerConsoleTool
{
    public static class Help
    {
        public static string Get => @"
Usage: dotnet BGPViewerConsoleTool.dll [options] <value> [command]
Usage: dotnet BGPViewerConsoleTool.dll -s <search_value>
Usage: dotnet BGPViewerConsoleTool.dll -h

Options:
    -a          AS number       (e.g. -a 15169)
    -p          Prefix          (e.g  -p 8.8.8.0/24)
    -i          IP              (e.g  -i 8.8.8.8)
    -s          Search by       (e.g  -s 8.8.8.8 or -s 6762 or -s ""Google"")
    -h          Help

Commands: 
    
With -a option:

    -d          AS details      (e.g. -a 15169 -d)
    -px         AS prefixes     (e.g. -a 15169 -px)
    -pr         AS peers        (e.g. -a 15169 -pr)
    -up         AS upstreams    (e.g. -a 15169 -up)
    -dw         AS downstreams  (e.g. -a 15169 -dw)
    -ix         AS IXs          (e.g. -a 15169 -ix)

With -p option:

    -d          Prefix details  (e.g  -p 8.8.8.0/24 -d)

With -i option:

    -d          IP details  (e.g  -i 8.8.8.8 -d)
";
    }
}