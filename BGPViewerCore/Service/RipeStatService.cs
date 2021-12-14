using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BGPViewerCore.Models;

namespace BGPViewerCore.Service
{
    public class RipeStatService : IBGPViewerService
    {
        private RipeStatWebApi api;

        public RipeStatService(RipeStatWebApi api)
        {
            this.api = api;
        }

        public void Dispose() => this.api = null;

        private void ValidateData(JsonDocument jsonData)
        {
            if(jsonData.RootElement.GetProperty("status").GetString() != "ok")
                throw new ArgumentException(
                    string.Join(". ", jsonData.RootElement.GetProperty("messages")
                        .EnumerateArray()
                        .Select(x => string.Join(". ", x.EnumerateArray()
                            .Select(y => y.GetString())))
                ));
        }

        public AsnDetailsModel GetAsnDetails(int asNumber)
            => GetAsnDetailsAsync(asNumber).GetAwaiter().GetResult();

        public async Task<AsnDetailsModel> GetAsnDetailsAsync(int asNumber)
        {
            var jsonData = await api.RetrieveAsnDetailsAsync(asNumber);
            ValidateData(jsonData);
            var dataProperty = jsonData.RootElement.GetProperty("data");

            return new AsnDetailsModel {
                ASN = asNumber,
                Name = dataProperty.GetProperty("holder").GetString(),
                Description = $"{dataProperty.GetProperty("block").GetProperty("desc").GetString()} - {dataProperty.GetProperty("block").GetProperty("name").GetString()}",
                AbuseContacts = Enumerable.Empty<string>(),
                EmailContacts = Enumerable.Empty<string>(),
                CountryCode = string.Empty,
                LookingGlassUrl = string.Empty
            };
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnDownstreams(int asNumber)
            => new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(Enumerable.Empty<AsnModel>(), Enumerable.Empty<AsnModel>());

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnDownstreamsAsync(int asNumber)
            => Task.FromResult(GetAsnDownstreams(asNumber));

        public IEnumerable<IxModel> GetAsnIxs(int asNumber)
            => Enumerable.Empty<IxModel>();

        public Task<IEnumerable<IxModel>> GetAsnIxsAsync(int asNumber)
            => Task.FromResult(GetAsnIxs(asNumber));

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnPeers(int asNumber)
            => new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                Enumerable.Empty<AsnModel>(),
                Enumerable.Empty<AsnModel>()
            );

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
            => new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(
                Enumerable.Empty<AsnModel>(),
                Enumerable.Empty<AsnModel>()
            );

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnUpstreamsAsync(int asNumber)
            => Task.FromResult(GetAsnUpstreams(asNumber));

        public IpDetailModel GetIpDetails(string ipAddress)
            => GetIpDetailsAsync(ipAddress).GetAwaiter().GetResult();

        public async Task<IpDetailModel> GetIpDetailsAsync(string ipAddress)
        {
            var jsonData = await api.RetrieveIpDetailsAsync(ipAddress);
            ValidateData(jsonData);
            var data = jsonData.RootElement.GetProperty("data");
            return new IpDetailModel {
                 IPAddress = ipAddress,
                 PtrRecord = string.Empty,
                 RIRAllocationPrefix = data.GetProperty("prefix").GetString(),
                 CountryCode = string.Empty,
                 RelatedPrefixes = new PrefixDetailModel[] {
                     new PrefixDetailModel {
                         Prefix = data.GetProperty("prefix").GetString(),
                         Name = string.Empty,
                         Description = string.Empty,
                         ParentAsns = data.GetProperty("asns")
                            .EnumerateArray()
                            .Select(x => new AsnModel {
                                ASN = int.Parse(x.GetString()),
                                CountryCode = string.Empty,
                                Name = string.Empty,
                                Description = string.Empty
                            })
                     }
                 }
            };
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