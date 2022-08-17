namespace dynamic_twist_api.Services.WordConvertService
{
    public interface IWordConvertService
    {
        public Task ConvertWordToHtml(string type, string fileName, Stream wordFileData);
    }
}
