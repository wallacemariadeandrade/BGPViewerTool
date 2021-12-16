![alt text](https://github.com/wallacemariadeandrade/BGPViewerTool/blob/master/bgpviewerlogo_480x270.png)

![Core Library Tests](https://github.com/wallacemariadeandrade/BGPViewerTool/workflows/Core%20Library%20Tests/badge.svg)
![Deploy BGPViewerOpenApi to Heroku](https://github.com/wallacemariadeandrade/BGPViewerTool/workflows/Deploy%20BGPViewerOpenApi%20to%20Heroku/badge.svg)
![license](https://img.shields.io/github/license/wallacemariadeandrade/BGPViewerTool)

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
If you're a .NET developer you can use the core library on your application. See [BGPViewerCore](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/master/BGPViewerCore) repo to get more information. 

Also, if you're consuming the API instead of using core library directly, you can use core library models to avoid rewriting the wheel. It's a separate library, so just download and add it to your project. It's available [here](https://www.nuget.org/packages/BGPViewerCore.Models/).