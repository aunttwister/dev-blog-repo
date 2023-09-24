using dynamic_twist_api.FileResolver.Exceptions;
using Microsoft.Extensions.FileProviders;
using dynamic_twist_api.FileResolver.Interfaces;
using dynamic_twist_api.FileResolver.Models;

namespace dynamic_twist_api.FileResolver.Services
{
    public class FileResolverService : FileResolverBase, IFileResolverService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public FileResolverService(IWebHostEnvironment hostEnvironment, string basePath)
        {
            _hostEnvironment = hostEnvironment;
            BasePath = basePath;
        }


        public IEnumerable<string> ResolveFileName(IFileInfo file, List<string> Splitters)
        {
            IEnumerable<string> fileNameElements = file.Name.Split(Splitters.ToArray(), StringSplitOptions.None).AsEnumerable();

            if (fileNameElements.Count() != Splitters.Count + 1)
                throw new NameUnresolvedException($"Unable to resolve {file.Name} name.\nDelete this file and repost it.");

            return fileNameElements;
        }

        public IFileInfo[] ResolveFiles(string type)
        {
            string path = BasePath + $"{type}/";
            if (!Directory.Exists(path))
            {
                IDirectoryContents directoryContents = _hostEnvironment.WebRootFileProvider.GetDirectoryContents(BasePath);
                string availableTypes = string.Join(",", directoryContents.Where(f => f.IsDirectory).Select(f => f.Name));

                throw new PathUnresolvableException($"Unable to resolve path {path}. Type {type} might be wrong. Available types are: {availableTypes}.");
            }
            return _hostEnvironment.WebRootFileProvider.GetDirectoryContents($"/html/{type}/")
                                                                    .Where(f => f.Exists)
                                                                    .ToArray();
        }

        public IFileInfo ResolveFile(string type, string fileName)
        {
            return _hostEnvironment.WebRootFileProvider.GetDirectoryContents($"{BasePath}{type}/").FirstOrDefault(f => f.Name == fileName + ".html");
        }

        public string ResolveFilePath(string type, string fileName)
        {
            return (_hostEnvironment.WebRootPath + $"/html/{type}/{fileName}.html");
        }

        public bool FileUnique(string type, string fileName)
        {
            IFileInfo file = ResolveFile(type, fileName);

            return file == null;
        }
    }
}
