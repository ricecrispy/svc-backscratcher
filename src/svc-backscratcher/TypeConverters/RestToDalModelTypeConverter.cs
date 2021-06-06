using AutoMapper;
using svc_backscratcher.Models;
using System;
using System.Linq;

namespace svc_backscratcher.TypeConverters
{
    public class RestToDalModelTypeConverter :
        ITypeConverter<BackScratcherRest, BackScratcherDal>
    {
        public BackScratcherDal Convert(BackScratcherRest source, BackScratcherDal destination, ResolutionContext context)
        {
            destination = new BackScratcherDal
            {
                Identifier = source.Identifier,
                Name = source.Name,
                Description = source.Description,
                Price = double.Parse(source.Price.Replace("$", "")),
                Size = source.Sizes.Distinct().Select(x =>
                {
                    Enum.TryParse(x.ToUpper().Trim(), out BackScratcherSize size);
                    return size;
                })
            };

            return destination;
        } 
    }

    
}
