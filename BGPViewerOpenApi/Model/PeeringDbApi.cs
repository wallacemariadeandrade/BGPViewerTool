using System;
using BGPViewerCore.Service;

namespace BGPViewerOpenApi.Model
{
    public class PeeringDbApi : ApiBase
    {
        public override int Id => 3;

        public override string Name => "PeeringDB API";

        public override string Description => "PeeringDB is a freely available, user-maintained, database of networks, and the go-to location for interconnection data. The database facilitates the global interconnection of networks at Internet Exchange Points (IXPs), data centers, and other interconnection facilities, and is the first stop in making interconnection decisions. The database is a non-profit, community-driven initiative run and promoted by volunteers. It is a public tool for the growth and good of the Internet. Join the community and support the continued development of the Internet. Available at https://www.peeringdb.com/apidocs/";

        public override Type ApiType => typeof(PeeringDbService);
    }
}