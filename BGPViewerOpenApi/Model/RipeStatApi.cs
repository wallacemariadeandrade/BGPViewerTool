using System;
using BGPViewerCore.Service;

namespace BGPViewerOpenApi.Model
{
    public class RipeStatApi : ApiBase
    {
        public override int Id => 5;

        public override string Name => "RIPEStat API";

        public override string Description => "RIPEstat is a large-scale information service and the RIPE NCC’s open data platform. You can get essential information on IP address space and Autonomous System Numbers (ASNs) along with related statistics on specific hostnames and countries. The RIPEstat Data API is the public data interface for RIPEstat. It is the only data source for the RIPEstat widgets and the newer RIPEstat UI (https://stat.ripe.net/app/launchpad). Available at https://stat.ripe.net/docs/02.data-api/";

        public override Type ApiType => typeof(RipeStatService);
    }
}