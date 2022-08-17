using System.ComponentModel.DataAnnotations;

namespace dynamic_twist_api.Application.Core.DTO
{
    public class ArticleUploadDto
    {
        [Required]
        public string FileName { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public IFormFile WordFile { get; set; }
    }
}
