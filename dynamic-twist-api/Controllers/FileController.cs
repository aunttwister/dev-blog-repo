using dev_blog_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace dev_blog_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet]
        [Route("info/{type}")]
        public List<Models.File> GetFilesInfo([FromRoute] string type)
        {
            return _fileService.GetFilesInfo(type);
        }
    }
}
