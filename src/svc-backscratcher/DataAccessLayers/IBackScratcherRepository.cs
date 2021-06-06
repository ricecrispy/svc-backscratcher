using svc_backscratcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.DataAccessLayers
{
    public interface IBackScratcherRepository
    {
        Task<Guid> CreateBackScratcherAsync(BackScratcherDal backScratcher);
        Task<IEnumerable<BackScratcherDal>> SearchAllBackScraterchersAsync();
        Task<IEnumerable<BackScratcherDal>> SearchBackScratchersAsync(string name, string description, IEnumerable<BackScratcherSize> backScratcherSizes, double price);
        Task<BackScratcherDal> GetBackScratcherAsync(Guid id);
        Task UpdateBackScratcherAsync(BackScratcherDal backScratcher);
        Task DeleteBackScratcherAsync(Guid id);
    }
}
