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

        private void ValidateData(JsonDocument data)
        {
            if(data.RootElement.TryGetProperty("error_message", out JsonElement message))
            {
                throw new ArgumentException($"{data.RootElement.GetProperty("error_title")}. {message.GetString()}");
            }
        }

        public AsnDetailsModel GetAsnDetails(int asNumber)
            => GetAsnDetailsAsync(asNumber).GetAwaiter().GetResult();

        public async Task<AsnDetailsModel> GetAsnDetailsAsync(int asNumber)
        {
            var data = await api.RetrieveAsnDetailsAsync(asNumber);
            ValidateData(data);
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
            => Enumerable.Empty<IxModel>();

        public Task<IEnumerable<IxModel>> GetAsnIxsAsync(int asNumber)
            => Task.FromResult(GetAsnIxs(asNumber));

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
            => GetIpDetailsAsync(ipAddress).GetAwaiter().GetResult();

        public async Task<IpDetailModel> GetIpDetailsAsync(string ipAddress)
        {
            var data = await api.RetrieveIpDetailsAsync(ipAddress);
            ValidateData(data);
            
            var name = data.RootElement.GetProperty("net")
                .GetProperty("name")
                .GetProperty("$")
                .GetString();

            var netBlock = data.RootElement.GetProperty("net")
                .GetProperty("netBlocks")
                .GetProperty("netBlock");

            var prefix = $"{netBlock.GetProperty("startAddress").GetProperty("$").GetString()}/{netBlock.GetProperty("cidrLength").GetProperty("$").GetString()}";

            return new IpDetailModel {
                IPAddress = ipAddress,
                PtrRecord = string.Empty,
                RIRAllocationPrefix = prefix,
                CountryCode = string.Empty,
                RelatedPrefixes = new PrefixDetailModel[] {
                    new PrefixDetailModel {
                        Name = name,
                        Description = netBlock.GetProperty("description").GetProperty("$").GetString(),
                        Prefix = prefix,
                        ParentAsns = Enumerable.Empty<AsnModel>()
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