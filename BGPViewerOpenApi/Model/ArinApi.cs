using System;
using BGPViewerCore.Service;

namespace BGPViewerOpenApi.Model
{
    public class ArinApi : ApiBase
    {
        public override int Id => 4;

        public override string Name => "ARIN API";

        public override string Description => "ARIN’s Whois RESTful web service is the programmatic interface to accessing ARIN’s Whois data. Unlike other solutions to the problem of Whois data access, RESTful web services are well within the mainstream of information exchange protocols, and the re-use of HTTP and web technologies in the simple REST design patterns does not require ARIN to write client software. Available at https://www.arin.net/resources/registry/whois/rws/api/";

        public override Type ApiType => typeof(ArinService);
    }
}