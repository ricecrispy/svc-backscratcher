using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.Models
{
    public class BackScratcherRest
    {
        public Guid Identifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Size { get; set; }
        public string PriceString { get; set; }
    }
}
