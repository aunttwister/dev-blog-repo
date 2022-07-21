using System.Linq;

namespace dev_blog_api.Services
{
    public class FileService : IFileService
    {
        public List<Models.File> GetFilesInfo(string type)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo($"../dev-blog-frontend/src/assets/{type}/");

            FileInfo[] files = directoryInfo.GetFiles();

            return files.Select<FileInfo, Models.File>(f =>
            {
                var subStrings = f.Name.Split("#");
                return new Models.File
                {
                    PublishDate = DateTime.Parse(subStrings[0]).Date,
                    FileName = subStrings[1].Split(".")[0],
                };
            }).OrderByDescending(f => f.PublishDate).ToList();
        }
    }
}
