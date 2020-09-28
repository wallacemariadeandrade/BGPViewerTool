# BGPViewerTool

A tool to help network analysts get information about prefixes and ASN's from command line. Built on netstandard2.0 and based on [BGPView API](https://bgpview.docs.apiary.io/#reference).

There are a few ways to use this repo content, so scroll down and have fun! :smile:

:construction: It's under construction :construction:

## .NET Developers
If you're a .NET developer you can use the core library on your application. Just add a reference to it and go code! :sunglasses:

### Create the Service
```c#
var service = new BGPViewerService(new BGPViewerWebApi());
```

### Get ASN Information
```c#

// Details like
// ASN = 6762,
// Name = "SEABONE-NET",
// DescriptionShort = "TELECOM ITALIA SPARKLE S.p.A.",
// EmailContacts = {
//     "abuse@seabone.net",
//     "peering@seabone.net",
//     "tech@seabone.net",
//     "francesco.chiappini@telecomitalia.it"
// },
// AbuseContacts = {
//     "abuse@seabone.net"
// },
// LookingGlassUrl = "https://gambadilegno.noc.seabone.net/lg/",
// CountryCode = "IT"
var details = service.GetAsnDetails(6762);

```
