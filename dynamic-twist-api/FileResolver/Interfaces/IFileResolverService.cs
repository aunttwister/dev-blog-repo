using Microsoft.Extensions.FileProviders;

namespace dynamic_twist_api.FileResolver.Interfaces
{
    public interface IFileResolverService
    {
        public static string BasePath;
        IEnumerable<string> ResolveFileName(IFileInfo file, List<string> Splitters);
        IFileInfo ResolveFile(string type, string fileName);
        string ResolveFilePath(string type, string fileName);
        bool FileUnique(string type, string fileName);
    }
}
