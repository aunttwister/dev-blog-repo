using dynamic_twist_api.FileResolver;
using dynamic_twist_api.Services.WordConvertService;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace dynamic_twist_api.FileResolver.Models 
{ 
    public class FileResolverBase
    {
        protected List<string> Splitters = new List<string>() {"."};
        protected string BasePath { get; set; }
    }
}
