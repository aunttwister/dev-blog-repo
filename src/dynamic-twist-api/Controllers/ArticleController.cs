using dynamic_twist_api.Application.Core.DTO;
using dynamic_twist_api.Services.ArticleService;
using dynamic_twist_api.Services.WordConvertService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dynamic_twist_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        public ArticleController(
            IArticleService articleService)
        {
            _articleService = articleService;
        }
        [HttpGet]
        [Route("get/{type}")]
        public async Task<IActionResult> GetArticlesAsync([FromRoute] string type)
        {
            var result = await _articleService.GetArticlesAsync(type);

            return Ok(result.Where(r => r != null).OrderByDescending(f => f.PublishDate));
        }
        [HttpGet]
        [Route("get/{type}/{fileName}")]
        public async Task<IActionResult> GetArticleByFileNameAsync([FromRoute] string type, [FromRoute] string fileName)
        {
            var result = await _articleService.GetArticleByFileNameAsync(type, fileName);
            if (result == null)
            {
                return BadRequest("No article found.");
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("post")]
        [Authorize]
        public async Task<IActionResult> PostArticleAsync([FromForm] ArticleUploadDto articleUpload)
        {
            if (!articleUpload.WordFile.FileName.EndsWith(".docx"))
            {
                return BadRequest("Unsupported file format.");
            }

            var wordStream = articleUpload.WordFile.OpenReadStream();
            await _articleService.PostArticleAsync(articleUpload.Type, articleUpload.FileName, wordStream);

            return Ok();
        }

        [HttpDelete]
        [Route("delete/{type}/{fileName}")]
        [Authorize]
        public IActionResult DeleteArticle([FromRoute] string type, [FromRoute] string fileName)
        {
            _articleService.DeleteArticle(type, fileName);
            return NoContent();
        }
    }
}
