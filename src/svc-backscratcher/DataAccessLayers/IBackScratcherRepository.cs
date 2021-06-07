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
        Task<BackScratcherDal> GetBackScratcherAsync(Guid id);
        Task UpdateBackScratcherAsync(BackScratcherDal backScratcher);
        Task DeleteBackScratcherAsync(Guid id);
    }
}
