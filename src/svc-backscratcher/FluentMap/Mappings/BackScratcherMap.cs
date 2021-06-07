using Dapper.FluentMap.Mapping;
using svc_backscratcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.FluentMap.Mappings
{
    internal class BackScratcherMap : EntityMap<BackScratcherDal>
    {
        public BackScratcherMap()
        {
            Map(p => p.Identifier).ToColumn("product_id");
            Map(p => p.Name).ToColumn("product_name");
            Map(p => p.Description).ToColumn("product_description");
            Map(p => p.Size).ToColumn("product_sizes");
            Map(p => p.Price).ToColumn("product_price");
        }
    }
}
