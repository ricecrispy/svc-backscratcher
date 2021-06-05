using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.Models
{
    public class BackScratcherDal
    {
        public Guid Identifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<BackScratcherSize> Size { get; set; }
        public double Price { get; set; }
    }
}
