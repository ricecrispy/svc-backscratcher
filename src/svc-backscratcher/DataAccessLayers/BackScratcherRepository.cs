using svc_backscratcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.DataAccessLayers
{
    public class BackScratcherRepository : IBackScratcherRepository
    {
        public Task<Guid> CreateBackScratcherAsync(BackScratcherDal backScratcher)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBackScratcherAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BackScratcherDal> GetBackScratcherAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BackScratcherDal>> SearchAllBackScraterchersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BackScratcherDal>> SearchBackScratchersAsync(string name, string description, IEnumerable<BackScratcherSize> backScratcherSizes, double price)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBackScratcherAsync(BackScratcherDal backScratcher)
        {
            throw new NotImplementedException();
        }
    }
}
