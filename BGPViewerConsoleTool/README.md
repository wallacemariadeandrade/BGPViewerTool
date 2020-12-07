# Windows, Mac or Linux User (.NET Core Solution)
Use can use the .NET Core solution [BGPViewerConsoleTool](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/development/BGPViewerConsoleTool). It's a command line client built on NET Core, a cross-platform version of .NET for building websites, services, and console apps.

To use this tool you'll must have installed NET Core Runtime, which one you can get [here](https://dotnet.microsoft.com/download).


## Use

First, [download and install the .NET Core runtime](https://dotnet.microsoft.com/download). 

Second, download the [app](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/development/BGPViewerConsoleTool/app) folder and place it somewhere on your PC.

And that's it. To use the tool just navigate into the app folder and call BGPViewerConsoleTool.dll. You can also simplify calling the tool using environment variables to set a shortcut for it. 

Some help with environment variables: [Windows]() [Mac]() [Linux]()


Take a look at some examples! :mag:

```
Usage: dotnet BGPViewerConsoleTool.dll [options] <value> [command]
Usage: dotnet BGPViewerConsoleTool.dll -s <search_value>
Usage: dotnet BGPViewerConsoleTool.dll -h

Options:
    -a          AS number       (e.g. -a 15169)
    -p          Prefix          (e.g  -p 8.8.8.0/24)
    -i          IP              (e.g  -i 8.8.8.8)
    -s          Search by       (e.g  -s 8.8.8.8 or -s 6762 or -s "Google")
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
```

## Supported APIs
This command line tool currently supports BGP View API and BGP He API (Selenium approach). You'll be quested about where do you want to get data from when using the tool.