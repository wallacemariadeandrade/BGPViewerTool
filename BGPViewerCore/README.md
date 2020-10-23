# .NET Developers
If you're a .NET developer you can use the core library on your application. Download [BGPViewerCore](https://github.com/wallacemariadeandrade/BGPViewerTool/tree/development/BGPViewerCore), add a reference to it on your project and go code! :sunglasses:

## Creating the Service
```c#
var service = new BGPViewerService(new BGPViewerWebApi());
```

## Getting ASN Information
```c#

// AS Number, Name, Description, Email Contacts, Looking Glass URL and Country Code
var details = service.GetAsnDetails(6762);

```

## Getting ASN Prefixes
```c#

// AS Number, IPv4 Prefixes, IPv6 Prefixes
var prefixes = service.GetAsnPrefixes(131630);

```

## Getting ASN Peers, Upstreams and Downstreams
```c#

// AS Number, Name, Description and Country Code for IPv4 and IPv6 peers
var prefixes = service.GetAsnPeers(131630);
var upstreams = service.GetAsnUpstreams(131630);
var downstreams = service.GetAsnDownstreams(131630);

```

## Getting IXs where given ASN is present
```c#

// IX Name, Country Code, IPv4 and IPv6 Address, Participant Speed
var ixs = service.GetAsnIxs(131630);

```

## Getting IP address details
```c#

// Allocation Prefix, PTR Record, Name, Description, Related ASNs
var ip = service.GetIpDetails("10.100.100.20");

```

## Getting prefix details
```c#

// Prefix, Name, Description, Parent ASNs
var ip = service.GetPrefixDetails("10.100.100.20", 20);

```

## Searching by ASN, prefix, IP, name, description
```c#

// IPv4 and IPv6 Prefixes, Related ASNs
var searchResult = service.SearchBy("Google");

```