using System;
using System.Collections.Generic;
using BGPViewerCore.Model;

namespace BGPViewerCore.Service
{
    public interface IBGPViewerService
    {
        AsnDetailsModel GetAsnDetails(int asNumber);
        Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnDownstreams(int asNumber);
        IEnumerable<IxModel> GetAsnIxs(int asNumber);
        Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnPeers(int asNumber);
        AsnPrefixesModel GetAsnPrefixes(int asNumber);
        Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnUpstreams(int asNumber);
        IpDetailModel GetIpDetails(string ipAddress);
        PrefixDetailModel GetPrefixDetails(string prefix, byte cidr);
        SearchModel SearchBy(string queryTerm);
    }
}