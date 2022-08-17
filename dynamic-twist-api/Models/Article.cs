namespace dynamic_twist_api.Models
{
    public class Article
    {
        public string FileName { get; set; }
        public DateTime PublishDate { get; set; }
        public string FullName => $"{PublishDate:yyyy-MM-dd}#{FileName}.html";
        public string Body { get; set; }

    }
}
