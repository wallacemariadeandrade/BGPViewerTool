using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BGPViewerCore.Models;

namespace BGPViewerCore.Service
{
    public class ArinService : IBGPViewerService
    {
        private ArinWebApi api;

        public ArinService(ArinWebApi api)
        {
            this.api = api;
        }

        public void Dispose() => this.api = null;

        private bool ValidateData(JsonDocument data) => data.RootElement.GetRawText() != "{}";
        public AsnDetailsModel GetAsnDetails(int asNumber)
            => GetAsnDetailsAsync(asNumber).GetAwaiter().GetResult();

        public async Task<AsnDetailsModel> GetAsnDetailsAsync(int asNumber)
        {
            var data = await api.RetrieveAsnDetailsAsync(asNumber);
            if(!ValidateData(data))
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
            var asn = data.RootElement.GetProperty("asn");
            
            var details = new AsnDetailsModel
            {
                ASN = int.Parse(asn.GetProperty("startAsNumber").GetProperty("$").GetString()),
                Name = asn.GetProperty("name").GetProperty("$").GetString(),
                Description = asn.GetProperty("name").GetProperty("$").GetString(),
                AbuseContacts = Enumerable.Empty<string>(),
                EmailContacts = Enumerable.Empty<string>(),
                CountryCode = string.Empty,
                LookingGlassUrl = string.Empty
            };

            if(asn.TryGetProperty("comment", out JsonElement comment))
            {
                var lines = comment.GetProperty("line")
                    .EnumerateArray()
                    .Select(x => x.GetProperty("$").GetString());
                details.Description = string.Join(" ", lines);
            }

            return details;
        }

        public Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>> GetAsnDownstreams(int asNumber)
            => new Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>(Enumerable.Empty<AsnModel>(), Enumerable.Empty<AsnModel>());

        public Task<Tuple<IEnumerable<AsnModel>, IEnumerable<AsnModel>>> GetAsnDownstreamsAsync(int asNumber)
            => Task.FromResult(GetAsnDownstreams(asNumber));

        public IEnumerable<IxModel> GetAsnIxs(int asNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IxModel>> GetAsnIxsAsync(int asNumber)
        {
            throw new NotImplementedException();
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