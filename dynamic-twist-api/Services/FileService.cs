using Microsoft.Extensions.FileProviders;
using System.Linq;
using Westwind.AspNetCore.Markdown;

namespace dynamic_twist_api.Services
{
    public class FileService : IFileService
    {
        private IWebHostEnvironment _hostEnvironment;

        public FileService(IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
        }
        public List<Models.File> GetFilesInfo(string type)
        {
            IFileInfo[] files = _hostEnvironment.WebRootFileProvider.GetDirectoryContents("/markdown/blog/").ToArray();

            return files.Select(f =>
            {
                var subStrings = f.Name.Split("#");
                return new Models.File
                {
                    PublishDate = DateTime.Parse(subStrings[0]).Date,
                    FileName = subStrings[1].Split(".")[0],
                    Html = Markdown.ParseFromFile(f)
                };
            }).OrderByDescending(f => f.PublishDate).ToList();
        }
    }
}
