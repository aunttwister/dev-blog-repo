namespace dynamic_twist_api.Models
{
    public class File
    {
        public string FileName { get; set; }
        public DateTime PublishDate { get; set; }
        public string FullName => $"{PublishDate:yyyy-MM-dd}#{FileName}.md";
        public string Html { get; set; }
    }
}
