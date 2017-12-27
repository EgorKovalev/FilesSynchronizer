using Models;
using System.Threading.Tasks;

namespace FileServiceApi.Services
{
    interface IService
    {
        string GetAuthLink();
        Task<string> GetToken(AuthorizationModel model);
        Task<string> GetList(string token, string path);
        Task<string> CopyFile(string token, string path, string copyTo);
        Task<string> DeleteFile(string token, string path);
        Task<string> GetLinkToDownloadFile(string token, string path);
        Task<string> UploadFile(string token, string url, string fileContent);
    }
}
