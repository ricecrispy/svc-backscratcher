using Dapper;
using svc_backscratcher.DatabaseAccess;
using svc_backscratcher.FluentMap.Mappings;
using svc_backscratcher.FluentMap.ParameterModels;
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
            var parameters = new CreateProductParameters
            {
                p_product_name = backScratcher.Name,
                p_product_description = backScratcher.Description,
                p_product_sizes = backScratcher.Size.ToArray(),
                p_product_price = backScratcher.Price
            };

            using (var connection = _databaseAccess.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Guid>(
                    "data.create_product",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<BackScratcherDal> GetBackScratcherAsync(Guid id)
        {
            var parameters = new GetProductParameters
            {
                p_product_id = id
            };

            using (var connection = _databaseAccess.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<BackScratcherDal>(
                    "data.get_product",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<BackScratcherDal>> SearchAllBackScraterchersAsync()
        {
            using (var connection = _databaseAccess.GetDatabaseConnection())
            {
                return await connection.QueryAsync<BackScratcherDal>(
                    "data.get_all_products",
                    null,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateBackScratcherAsync(BackScratcherDal backScratcher)
        {
            var parameters = new UpdateProductParameters
            {
                p_product_id = backScratcher.Identifier,
                p_product_name = backScratcher.Name,
                p_product_description = backScratcher.Description,
                p_product_sizes = backScratcher.Size.ToArray(),
                p_product_price = backScratcher.Price
            };

            using (var connection = _databaseAccess.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(
                    "data.update_product",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteBackScratcherAsync(Guid id)
        {
            var parameters = new DeleteProductParameters
            {
                p_product_id = id
            };

            using (var connection = _databaseAccess.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(
                    "data.delete_product",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
