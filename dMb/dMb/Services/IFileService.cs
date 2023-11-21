using System.Threading.Tasks;


namespace dMb.Services
{
    public interface IFileService
    {
        string GetRootPath();

        void DeleteFile(string filePath);

        Task<string[]> GetFilesNamesAsync();

        Task<string> PickFileAsync();
    }
}
