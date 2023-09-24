using Mammoth;
using dynamic_twist_api.Application.Core.Helpers;

namespace dynamic_twist_api.Services.WordConvertService
{
    public class WordConvertService : IWordConvertService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public WordConvertService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public async Task ConvertWordToHtml(string type, string fileName, Stream wordFileData)
        {
            DocumentConverter docConverter = new DocumentConverter()
                .ImageConverter(img => {
                    using (var stream = img.GetStream())
                    {
                        var base64 = stream.ConvertToBase64();
                        var src = "data:" + img.ContentType + ";base64," + base64;
                        return new Dictionary<string, string> { { "src", src }, { "class", "img-fluid" } };
                    }
                    })
                .AddStyleMap("p[style-name='Code Block'] => pre:separator('\\n')")
                .AddStyleMap("p[style-name='Subtitle'] => i")
                .AddStyleMap("p[style-name='Heading 1'] => h1:fresh")
                .AddStyleMap("p[style-name='Heading 2'] => h2:fresh");
            var html = docConverter.ConvertToHtml(wordFileData);

            var directoryPath = _hostEnvironment.WebRootPath + $"/html/{type}/";

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            await File.WriteAllTextAsync(directoryPath + fileName + ".html", html.Value);
        }
    }
}
