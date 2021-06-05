using svc_backscratcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.DataAccessLayers
{
    public class BackScratcherRepository : IBackScratcherRepository
    {
        public Task CreateBackScratcherAsync(string name, string description, IEnumerable<BackScratcherSize> backScratcherSizes, double price)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBackScratcherAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BackScratcherRest> GetBackScratcherAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BackScratcherRest> SearchBackScratcherAsync(string name, string description, IEnumerable<BackScratcherSize> backScratcherSizes, double price)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBackScratcherAsync(Guid id, string name, string description, IEnumerable<BackScratcherSize> backScratcherSizes, double price)
        {
            throw new NotImplementedException();
        }
    }
}
