using svc_backscratcher.DatabaseAccess;
using svc_backscratcher.FluentMap.Mappings;
using svc_backscratcher.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.DataAccessLayers
{
    public class BackScratcherRepository : IBackScratcherRepository
    {
        private readonly IDatabaseAccess _databaseAccess;

        public BackScratcherRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        static BackScratcherRepository()
        {
            FluentMapInitializer.EnsureMapsInitialized();
        }

        public async Task<Guid> CreateBackScratcherAsync(BackScratcherDal backScratcher)
        {
            
        }

        public async Task DeleteBackScratcherAsync(Guid id)
        {

        }

        public async Task<BackScratcherDal> GetBackScratcherAsync(Guid id)
        {
            
        }

        public async Task<IEnumerable<BackScratcherDal>> SearchAllBackScraterchersAsync()
        {
            
        }

        public async Task<IEnumerable<BackScratcherDal>> SearchBackScratchersAsync(string name, string description, IEnumerable<BackScratcherSize> backScratcherSizes, double price)
        {
            
        }

        public async Task UpdateBackScratcherAsync(BackScratcherDal backScratcher)
        {
            
        }
    }
}
