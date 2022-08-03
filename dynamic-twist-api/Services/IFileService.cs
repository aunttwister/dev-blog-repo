namespace dynamic_twist_api.Services
{
    public interface IFileService
    {
        public List<Models.File> GetFilesInfo(string type);
    }
}
