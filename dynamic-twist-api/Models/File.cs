namespace dev_blog_api.Models
{
    public class File
    {
        public string FileName { get; set; }
        public DateTime PublishDate { get; set; }
        public string FullName => $"{PublishDate:yyyy-MM-dd}#{FileName}.md";
    }
}
