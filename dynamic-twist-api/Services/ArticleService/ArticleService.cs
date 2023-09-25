using dynamic_twist_api.Application.Core.Exceptions;
using dynamic_twist_api.FileResolver.Services;
using dynamic_twist_api.Models;
using dynamic_twist_api.Services.WordConvertService;
using Microsoft.Extensions.FileProviders;

namespace dynamic_twist_api.Services.ArticleService
{
    public class ArticleService : FileResolverService, IArticleService
    {
        private readonly IWordConvertService _wordConverterService;
        private readonly static string basePath = ".\\html\\";

        public ArticleService(IWebHostEnvironment hostEnvironment, IWordConvertService wordConverterService) : base(hostEnvironment, basePath)
        {
            _wordConverterService = wordConverterService;
            Splitters = new List<string> { "#", "." };
        }
        public async Task<IEnumerable<Article>> GetArticlesAsync(string type)
        {
            IFileInfo[] files = ResolveFiles(type);

            IEnumerable<Task<Article>> articles = files.Distinct().Select(async file =>
            {
                Article article = await ResolveArticle(file);
                if (article != null)
                    return article;
                else
                    return null;
            });

            return await Task.WhenAll(articles);
        }

        public async Task<Article> GetArticleByFileNameAsync(string type, string fileName)
        {
            IFileInfo file = ResolveFile(type, fileName);

            return await ResolveArticle(file);
        }

        public async Task PostArticleAsync(string type, string fileName, Stream fileData)
        {
            if (!fileName.Contains(Splitters[0]) && !fileName.Contains(Splitters[1]))
                throw new NameUnresolveableException("File name doesn't contain necessary characters for name resolving.");
            await _wordConverterService.ConvertWordToHtml(type, fileName, fileData);
        }

        public void DeleteArticle(string type, string fileName)
        {
            string fullPath = ResolveFilePath(type, fileName);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        private async Task<Article> ResolveArticle(IFileInfo file)
        {
            IEnumerable<string> articleElements = ResolveFileName(file, Splitters);
            if (articleElements == null)
                return null;

            if (DateTime.TryParse(articleElements.ElementAt(0), out DateTime date) == false)
                return null;
            Article article = new Article(articleElements);

            article.Body = await File.ReadAllTextAsync(file.PhysicalPath);

            return article;
        }
    }
}
