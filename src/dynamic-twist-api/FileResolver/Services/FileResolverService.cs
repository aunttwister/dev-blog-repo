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
                return null;

            return fileNameElements;
        }

        public IFileInfo[] ResolveFiles(string type)
        {
            string path = BasePath + $"{type}";
            IFileInfo[] files = _hostEnvironment.WebRootFileProvider.GetDirectoryContents(path)
                                                                        .Where(f => f.Exists)
                                                                        .ToArray();
            if (files == null)
            {
                IDirectoryContents directoryContents = _hostEnvironment.WebRootFileProvider.GetDirectoryContents(BasePath);
                string availableTypes = string.Join(", ", directoryContents.Where(f => f.IsDirectory).Select(f => f.Name));

                throw new PathUnresolvableException($"Unable to resolve path {path}. Type {type} might be wrong. Available types are: {availableTypes}.");
            }
            return files;
        }

        public IFileInfo ResolveFile(string type, string fileName)
        {
            return _hostEnvironment.WebRootFileProvider.GetDirectoryContents($"{BasePath}{type}/").FirstOrDefault(f => f.Name == fileName + ".html");
        }

        public string ResolveFilePath(string type, string fileName)
        {
            if (fileName.Contains(".html"))
                return (_hostEnvironment.WebRootPath + $".\\html\\{type}\\{fileName}");
            else
                return (_hostEnvironment.WebRootPath + $".\\html\\{type}\\{fileName}.html");
        }

        public bool FileUnique(string type, string fileName)
        {
            IFileInfo file = ResolveFile(type, fileName);

            return file == null;
        }
    }
}
