using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BGPViewerCore.Models;

namespace BGPViewerCore.Service
{
    public class PeeringDbService : IBGPViewerService
    {
        private IBGPViewerApi _api;

        public PeeringDbService(IBGPViewerApi api)
        {
            _api = api;
        }

        public void Dispose()
        {
            _api = null;
        }

        private bool ValidateDataElement(JsonElement dataElement) => dataElement.GetArrayLength() > 0;

        public AsnDetailsModel GetAsnDetails(int asNumber)
            => GetAsnDetailsAsync(asNumber).GetAwaiter().GetResult();

        public async Task<AsnDetailsModel> GetAsnDetailsAsync(int asNumber)
        {
            var jsonData = await _api.RetrieveAsnDetailsAsync(asNumber);
            var dataElement = jsonData.RootElement.GetProperty("data");

            if(!ValidateDataElement(dataElement))
            {
                return new AsnDetailsModel 
                {
                    ASN = asNumber,
                    Name = string.Empty,
                    Description = string.Empty,
                    CountryCode = string.Empty,
                    EmailContacts = Enumerable.Empty<string>(),
                    AbuseContacts = Enumerable.Empty<string>(),
                    LookingGlassUrl = string.Empty,
                };
            }

            var firstData = dataElement
                .EnumerateArray()
                .FirstOrDefault();   

            return new AsnDetailsModel 
            {
                ASN = asNumber,
                Name = firstData.GetProperty("name").GetString(),
                Description = firstData.GetProperty("aka").GetString(),
                CountryCode = string.Empty,
                EmailContacts = Enumerable.Empty<string>(),
                AbuseContacts = Enumerable.Empty<string>(),
                LookingGlassUrl = firstData.GetProperty("looking_glass").GetString(),
            };
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnDownstreams(int asNumber)
            => new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(Enumerable.Empty<AsnModel>(), Enumerable.Empty<AsnModel>());

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnDownstreamsAsync(int asNumber)
            => Task.FromResult(GetAsnDownstreams(asNumber));

        public IEnumerable<IxModel> GetAsnIxs(int asNumber)
            => GetAsnIxsAsync(asNumber).GetAwaiter().GetResult();

        public async Task<IEnumerable<IxModel>> GetAsnIxsAsync(int asNumber)
        {
            var jsonData = await _api.RetrieveAsnIxsAsync(asNumber);
            var dataElement = jsonData.RootElement.GetProperty("data");
           
            if(!ValidateDataElement(dataElement)) return Enumerable.Empty<IxModel>();

            return dataElement
                .EnumerateArray()
                .Select(el => new IxModel {
                    Name = el.GetProperty("name").GetString(),
                    FullName = el.GetProperty("name").GetString(),
                    CountryCode = string.Empty,
                    IPv4 = el.GetProperty("ipaddr4").GetString(),
                    IPv6 = el.GetProperty("ipaddr6").GetString(),
                    AsnSpeed = el.GetProperty("speed").GetInt32()
                });
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnPeers(int asNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnPeersAsync(int asNumber)
        {
            throw new NotImplementedException();
        }

        public AsnPrefixesModel GetAsnPrefixes(int asNumber)
        {
            throw new NotImplementedException();
        }

        public Task<AsnPrefixesModel> GetAsnPrefixesAsync(int asNumber)
        {
            throw new NotImplementedException();
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnUpstreams(int asNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnUpstreamsAsync(int asNumber)
        {
            throw new NotImplementedException();
        }

        public IpDetailModel GetIpDetails(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public Task<IpDetailModel> GetIpDetailsAsync(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public PrefixDetailModel GetPrefixDetails(string prefix, byte cidr)
        {
            throw new NotImplementedException();
        }

        public Task<PrefixDetailModel> GetPrefixDetailsAsync(string prefix, byte cidr)
        {
            throw new NotImplementedException();
        }

        public SearchModel SearchBy(string queryTerm)
        {
            throw new NotImplementedException();
        }

        public Task<SearchModel> SearchByAsync(string queryTerm)
        {
            throw new NotImplementedException();
        }
    }
}