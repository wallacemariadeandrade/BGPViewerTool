using System;
using BGPViewerCore.Service;

namespace BGPViewerOpenApi.Model
{
    public class BGPViewApi : ApiBase
    {
        public override int Id => 1;
        public override string Name => "BGP View API";
        public override string Description => "BGPView is a simple API allowing consumers to view all sort of analytics data about the current state and structure of the internet. Available at https://bgpview.docs.apiary.io/#";
        public override Type ApiType => typeof(BGPViewerService);
    }
}