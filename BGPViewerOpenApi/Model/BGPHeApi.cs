using System;
using BGPViewerCore.Service;

namespace BGPViewerOpenApi.Model
{
    public class BGPHeApi : ApiBase
    {
        public override int Id => 2;
        public override string Name => "BGP He API";
        public override string Description => "A tool from Hurricane Eletric to query information about internet. Available at https://bgp.he.net/";
        public override Type ApiType => typeof(BGPHeService);
    }
}