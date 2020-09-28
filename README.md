# BGPViewerTool

A tool to help network analysts get information about prefixes and ASN's from command line. Built on netstandard2.0 and based on [BGPView API](https://bgpview.docs.apiary.io/#reference).

There are a few ways to use this repo content, so scroll down and have fun! :smile:

:construction: It's under construction :construction:

## .NET Developers
If you're a .NET developer you can use the core library on your application. Download [BGPViewerCore](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/development/BGPViewerCore), add a reference to it on your project and go code! :sunglasses:

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

## Normal Human User
If you're a normal human (maybe not so normal cause you probably work with telecom) and likes Powershell, then you can use BGPViewerPowerTool! It's a bunch of PowerShell scripts that do all the work for you. Download the folder and call the scripts from PowerShell prompt.

### Permit Powershell to Excute Scripts
By default PowerShell don't allow scripts execution. To turn it on you have to run PowerShell as Administrator and paste the command bellow:
```
set-executionpolicy remotesigned

```
### Examples
- Get ASN Details: 
    - ```.\get_asn_details.ps1 3356 ```