using svc_backscratcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.FluentMap.ParameterModels
{
    internal class GetProductsByAttributesParameters
    {
        public string p_product_name { get; set; }
        public string p_product_description { get; set; }
        public IEnumerable<BackScratcherSize> p_product_sizes { get; set; }
        public double p_product_price { get; set; }
    }
}
