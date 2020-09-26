# BGPViewerTool

A tool to help network analysts get information about prefixes and ASN's from command line. Built on netstandard2.0 and based on [BGPView API](https://bgpview.docs.apiary.io/#reference).

## Creating the Service
```c#
var service = new BGPViewerService();
```

## Getting ASN Information
```c#

// Details like
// ASN = 6762,
// Name = "SEABONE-NET",
// DescriptionShort = "TELECOM ITALIA SPARKLE S.p.A.",
// EmailContacts = new List<string> {
//     "abuse@seabone.net",
//     "peering@seabone.net",
//     "tech@seabone.net",
//     "francesco.chiappini@telecomitalia.it"
// },
// AbuseContacts = new List<string> {
//     "abuse@seabone.net"
// },
// LookingGlassUrl = "https://gambadilegno.noc.seabone.net/lg/",
// CountryCode = "IT"
var details = service.GetAsnDetails(6762);

```
