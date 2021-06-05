using svc_backscratcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.DataAccessLayers
{
    public interface IBackScratcherRepository
    {
        Task<Guid> CreateBackScratcherAsync(string name, string description, IEnumerable<BackScratcherSize> backScratcherSizes, double price);
        Task<IEnumerable<BackScratcherDal>> SearchBackScratcherAsync(string name, string description, IEnumerable<BackScratcherSize> backScratcherSizes, double price);
        Task<BackScratcherDal> GetBackScratcherAsync(Guid id);
        Task UpdateBackScratcherAsync(Guid id, string name, string description, IEnumerable<BackScratcherSize> backScratcherSizes, double price);
        Task DeleteBackScratcherAsync(Guid id);
    }
}
