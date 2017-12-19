using Models;
using System.Threading.Tasks;

namespace FileServiceApi.Services
{
    interface IService
    {
        string GetAuthLink();
        Task<string> GetToken(AuthorizationModel model);
        Task<string> GetList(string token, string optional);
        Task<string> CopyFile(string token, string path, string copyToPath);
    }
}
