namespace dynamic_twist_api.Services.ArticleService
{
    public interface IArticleService
    {
        public Task<IEnumerable<Models.Article>> GetArticlesAsync(string type);
        public Task PostArticleAsync(string type, string fileName, Stream fileData);
        public Task<Models.Article> GetArticleByFileNameAsync(string type, string fileName);
        public bool IsUnique(string type, string fileName);
        public void DeleteArticle(string type, string fileName);
    }
}
