# BGPViewerTool

A tool to help network analysts get information about prefixes and ASN's from command line. Built on netstandard2.0 and based on [BGPView API](https://bgpview.docs.apiary.io/#reference).

There are a few ways to use this repo content, so scroll down and have fun! :smile:

:construction: It's under construction :construction:

## .NET Developers
If you're a .NET developer you can use the core library on your application. Download [BGPViewerCore](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/development/BGPViewerCore), add a reference to it on your project and go code! :sunglasses:

### Creating the Service
```c#
var service = new BGPViewerService(new BGPViewerWebApi());
```

### Getting ASN Information
```c#

// AS Number, Name, Description, Email Contacts, Looking Glass URL and Country Code
var details = service.GetAsnDetails(6762);

```

### Getting ASN Prefixes
```c#

// AS Number, IPv4 Prefixes, IPv6 Prefixes
var prefixes = service.GetAsnPrefixes(131630);

```

### Getting ASN Peers, Upstreams and Downstreams
```c#

// AS Number, Name, Description and Country Code for IPv4 and IPv6 peers
var prefixes = service.GetAsnPeers(131630);
var upstreams = service.GetAsnUpstreams(131630);
var downstreams = service.GetAsnDownstreams(131630);

```

### Getting IXs where given ASN is present
```c#

// IX Name, Country Code, IPv4 and IPv6 Address, Participant Speed
var ixs = service.GetAsnIxs(131630);

```

### Getting IP address details
```c#

// Allocation Prefix, PTR Record, Name, Description, Related ASNs
var ip = service.GetIpDetails("10.100.100.20");

```

### Getting prefix details
```c#

// Prefix, Name, Description, Parent ASNs
var ip = service.GetPrefixDetails("10.100.100.20", 20);

```


## Normal Human User
If you're a normal human (maybe not so normal cause you probably work with telecom :laughing::sweat_smile:) and likes Powershell, then you can use [BGPViewerPowerTool](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/development/BGPViewerPowerTool)! It's a bunch of PowerShell scripts that do all the work for you. Download the folder and call the scripts from PowerShell prompt at scripts directory.

> [PowerShell](https://docs.microsoft.com/pt-br/powershell/scripting/overview?view=powershell-7) is a cross-platform task automation and configuration management framework, consisting of a command-line shell and scripting language. Unlike most shells, which accept and return text, PowerShell is built on top of the .NET Common Language Runtime (CLR), and accepts and returns .NET objects. This fundamental change brings entirely new tools and methods for automation.

Download PowerShell [here](https://docs.microsoft.com/pt-br/powershell/scripting/install/installing-powershell?view=powershell-7).

### Permit Powershell to Excute Scripts
By default PowerShell doesn't allow scripts execution. To turn it on you have to run PowerShell as Administrator and paste the command bellow:
```
set-executionpolicy remotesigned

```
### Examples
- Get ASN Details: 
    - ```.\get_asn_details.ps1 3356 ```
- Get ASN Prefixes:
    - ```.\get_asn_prefixes.ps1 3356 ```
- Get ASN Peers:
    - ```.\get_asn_peers.ps1 3356 ```
- Get ASN Upstreams:
    - ```.\get_asn_upstreams.ps1 3356 ```
- Get ASN Downstreams:
    - ```.\get_asn_downstreams.ps1 3356 ```
- Get ASN IXs:
    - ```.\get_asn_ixs.ps1 3356 ```
- Get Prefix Details:
    - ```.\get_prefix_details.ps1 4.55.0.0/16 or .\get_prefix_details.ps1 4.55.0.0 16 ```
- Get IP Address Details:
    - ```.\get_ip_details.ps1 4.55.0.0 ```
- Search by ASN, Prefix, IP Address, Name
    - ```.\search.ps1 "Century Link" or .\search.ps1 CenturyLink or .\search.ps1 3356 ```