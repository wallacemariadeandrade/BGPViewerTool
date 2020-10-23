# NET Core User
Another option for you is to use the [BGPViewerConsoleTool](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/development/BGPViewerConsoleTool). It's a command line client built on NET Core, a cross-platform version of .NET for building websites, services, and console apps.

To use this tool you'll must have installed NET Core Runtime, whitch one you can get [here](https://dotnet.microsoft.com/download).


## Use
Download the [app](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/development/BGPViewerConsoleTool/app) folder and call BGPViewerConsoleTool.dll from it as shown below. Take a look! :mag:

```
Usage: dotnet BGPViewerConsoleTool [options] <value> [command]
Usage: dotnet BGPViewerConsoleTool -s <search_value>
Usage: dotnet BGPViewerConsoleTool -h

Options:
    -a          AS number       (e.g. -a 53181)
    -p          Prefix          (e.g  -p 8.8.8.8/24)
    -i          IP              (e.g  -i 8.8.8.8)
    -s          Search by       (e.g  -s 8.8.8.8 or -s 6762 or -s ""Century Link"")
    -h          Help

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
```