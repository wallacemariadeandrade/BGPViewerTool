![alt text](https://github.com/wallacemariadeandrade/BGPViewerTool/blob/master/bgpviewerlogo_480x270.png)

![Core Library Tests](https://github.com/wallacemariadeandrade/BGPViewerTool/workflows/Core%20Library%20Tests/badge.svg)
![Deploy BGPViewerOpenApi to Heroku](https://github.com/wallacemariadeandrade/BGPViewerTool/workflows/Deploy%20BGPViewerOpenApi%20to%20Heroku/badge.svg)

# BGPViewerTool

A tool to help network analysts querying information about prefixes and ASN's. Built on netstandard2.0 and based on [BGPView API](https://bgpview.docs.apiary.io/#reference).

There are a few ways to use this repo content, so scroll down and have fun! :smile:

:construction: It's under construction :construction:

## Developers
If you're a developer I offer you power to build amazing applications :fire:. You can either consume the REST API or use the core library itself (.NET developers, in case).

Take a look bellow for more information.

### BGPViewer OpenAPI
An REST API built on top of [BGPViewerCore](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/master/BGPViewerCore) library.

More information [here](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/master/BGPViewerOpenApi). Check it out! :wink:

### .NET Developers
If you're a .NET developer you can use the core library on your application. Download [BGPViewerCore](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/master/BGPViewerCore), add a reference to it on your project and go code! :sunglasses:

Also, if you're consuming the API instead of using core library directly, you can use core library models to avoid rewriting the wheel. It's a separate library, so just download and add it to your project.

## Windows, Mac or Linux Users (.NET Core Solution)
You can use the .NET Core solution [BGPViewerConsoleTool](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/master/BGPViewerConsoleTool). It's a command line client built on NET Core, a cross-platform version of .NET for building websites, services, and console apps.

To use this tool you'll must have installed NET Core Runtime, which one you can get [here](https://dotnet.microsoft.com/download).


## Windows, Mac or Linux Users (Powershell Solution)
If you're a normal human (maybe not so normal cause you probably work with telecom :laughing::sweat_smile:) and likes Powershell, then you can use [BGPViewerPowerTool](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/master/BGPViewerPowerTool)! It's a bunch of PowerShell scripts that do all the work for you. Download the folder and call the scripts from PowerShell prompt at scripts directory.
