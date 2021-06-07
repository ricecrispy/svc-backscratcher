using Dapper.FluentMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace svc_backscratcher.FluentMap.Mappings
{
    internal static class FluentMapInitializer
    {
        static FluentMapInitializer()
        {
            FluentMapper.Initialize(t => t.AddMap(new BackScratcherMap()));
        }

        public static void EnsureMapsInitialized()
        {
        }
    }
}
