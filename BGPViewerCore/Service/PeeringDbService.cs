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
        private PeeringDbWebApi _api;

        public PeeringDbService(PeeringDbWebApi api)
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
            => new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(Enumerable.Empty<AsnModel>(), Enumerable.Empty<AsnModel>());

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnPeersAsync(int asNumber)
            => Task.FromResult(GetAsnPeers(asNumber));

        public AsnPrefixesModel GetAsnPrefixes(int asNumber)
            => new AsnPrefixesModel {
                ASN = asNumber,
                IPv4 = Enumerable.Empty<string>(),
                IPv6 = Enumerable.Empty<string>()
            };

        public Task<AsnPrefixesModel> GetAsnPrefixesAsync(int asNumber)
            => Task.FromResult(GetAsnPrefixes(asNumber));

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnUpstreams(int asNumber)
            => new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(Enumerable.Empty<AsnModel>(), Enumerable.Empty<AsnModel>());

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnUpstreamsAsync(int asNumber)
            => Task.FromResult(GetAsnUpstreams(asNumber));

        public IpDetailModel GetIpDetails(string ipAddress)
            => new IpDetailModel {
                IPAddress = ipAddress,
                RIRAllocationPrefix = string.Empty,
                CountryCode = string.Empty,
                PtrRecord = string.Empty,
                RelatedPrefixes = Enumerable.Empty<PrefixDetailModel>()
            };

        public Task<IpDetailModel> GetIpDetailsAsync(string ipAddress)
            => Task.FromResult(GetIpDetails(ipAddress));

        public PrefixDetailModel GetPrefixDetails(string prefix, byte cidr)
            => new PrefixDetailModel {
                Name = string.Empty,
                Description = string.Empty,
                Prefix = $"{prefix}/{cidr}",
                ParentAsns = Enumerable.Empty<AsnModel>()
            };

        public Task<PrefixDetailModel> GetPrefixDetailsAsync(string prefix, byte cidr)
            => Task.FromResult(GetPrefixDetails(prefix, cidr));

        public SearchModel SearchBy(string queryTerm)
            => SearchByAsync(queryTerm).GetAwaiter().GetResult();

        public async Task<SearchModel> SearchByAsync(string queryTerm)
        {
            if(int.TryParse(queryTerm, out int asNumber))
            {
                var details = await GetAsnDetailsAsync(asNumber);
                return new SearchModel
                {
                    IPv4 = Enumerable.Empty<PrefixDetailModel>(),
                    IPv6 = Enumerable.Empty<PrefixDetailModel>(),
                    RelatedAsns = new AsnWithContactsModel[]{ details }
                };
            }

            return new SearchModel 
            {
                IPv4 = Enumerable.Empty<PrefixDetailModel>(),
                IPv6 = Enumerable.Empty<PrefixDetailModel>(),
                RelatedAsns = Enumerable.Empty<AsnWithContactsModel>()
            };
        }
    }
}