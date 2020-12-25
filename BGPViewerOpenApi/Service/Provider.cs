using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGPViewerCore.Service;
using BGPViewerOpenApi.Model;

namespace BGPViewerOpenApi.Service
{
    public class Provider
    {
        private readonly IEnumerable<ApiBase> availableApis;

        public Provider(IEnumerable<ApiBase> availableApis)
        {
            this.availableApis = availableApis;
        }

        internal Task<IEnumerable<ApiBase>> ListAvailableAsync()
        {
            if(ContainsDuplicatedId()) throw new InvalidOperationException("There are one or more APIs with same ID.");

            return Task.FromResult(availableApis);
        }

        private bool ContainsDuplicatedId()
        {
            var ids = availableApis.Select(x => x.Id);
            return ids.Count() != ids.Distinct().Count();
        }
    }
}