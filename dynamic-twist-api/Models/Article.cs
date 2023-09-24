using dynamic_twist_api.FileResolver;

namespace dynamic_twist_api.Models
{
    public class Article
    {
        public Article(IEnumerable<string> articleElements)
        {
            PublishDate = DateTime.Parse(articleElements.ElementAt(0)).Date;
            Name = articleElements.ElementAt(1);
            FileExtension = articleElements.ElementAt(2);
        }
        public string Name { get; set; }
        public string FileExtension { get; set; }
        public DateTime PublishDate { get; set; }
        public string Body { get; set; }
        //Name splitter = "#"
        //Extension splitter "."

    }
}
