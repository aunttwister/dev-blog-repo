using dynamic_twist_api.Services.WordConvertService;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using Westwind.AspNetCore.Markdown;
namespace dynamic_twist_api.Services.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IWordConvertService _wordConverterService;

        public ArticleService(IWebHostEnvironment hostEnvironment, IWordConvertService wordConverterService)
        {
            _hostEnvironment = hostEnvironment;
            _wordConverterService = wordConverterService;
        }
        public async Task<IEnumerable<Models.Article>> GetArticlesAsync(string type)
        {
            IFileInfo[] files = _hostEnvironment.WebRootFileProvider.GetDirectoryContents($"/html/{type}/")
                                                                    .Where(f => f.Exists)
                                                                    .ToArray();

            var list = files.Distinct().Select(async f =>
            {
                var subStrings = f.Name.Split("#");
                return new Models.Article
                {
                    PublishDate = DateTime.Parse(subStrings[0]).Date,
                    FileName = subStrings[1].Split(".")[0],
                    Body = await File.ReadAllTextAsync(f.PhysicalPath)
                };
            });

            return await Task.WhenAll(list);
        }

        public async Task<Models.Article> GetArticleByFileNameAsync(string type, string fileName)
        {
            IFileInfo file = _hostEnvironment.WebRootFileProvider.GetDirectoryContents($"/html/{type}/").FirstOrDefault(f => f.Name == fileName + ".html");

            var subStrings = file.Name.Split("#");

            return new Models.Article()
            {
                PublishDate = DateTime.Parse(subStrings[0]).Date,
                FileName = subStrings[1].Split(".")[0],
                Body = await File.ReadAllTextAsync(file.PhysicalPath)
            };
        }

        public async Task PostArticleAsync(string type, string fileName, Stream fileData)
        {
            await _wordConverterService.ConvertWordToHtml(type, fileName, fileData);
        }

        public void DeleteArticle(string type, string fileName)
        {
            string fullPath = (_hostEnvironment.WebRootPath + $"/html/{type}/{fileName}.html");
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public bool IsUnique(string type, string fileName)
        {
            IFileInfo file = _hostEnvironment.WebRootFileProvider.GetDirectoryContents($"/html/{type}/").FirstOrDefault(f => f.Name == fileName + ".html");

            return file == null;
        }
    }
}
