using System.Threading.Tasks;


namespace dMb.Services
{
    public interface IFileService
    {
        string GetRoothPath();
        string GetDatabasesPath();

        Task<string> CreateFileAsync();
        void DeleteFile(string filePath);

        Task<string[]> GetFilesNamesAsync();

        Task<string> PickFileAsync();
    }
}
