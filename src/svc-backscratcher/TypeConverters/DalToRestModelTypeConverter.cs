using AutoMapper;
using svc_backscratcher.Models;
using System.Linq;

namespace svc_backscratcher.TypeConverters
{
    public class DalToRestModelTypeConverter :
        ITypeConverter<BackScratcherDal, BackScratcherRest>
    {
        public BackScratcherRest Convert(BackScratcherDal source, BackScratcherRest destination, ResolutionContext context)
        {
            destination = new BackScratcherRest
            {
                Identifier = source.Identifier,
                Name = source.Name,
                Description = source.Description,
                Price = $"${source.Price:F2}",
                Sizes = source.Size.Select(x => x.ToString())
            };

            return destination;
        }
    }
}
