using Models;
using System.Threading.Tasks;

namespace FileServiceApi.Services
{
    interface IService
    {
        string GetAuthLink();
        Task<string> GetToken(AuthorizationModel model);
        Task<string> GetList(string token, string optional);
    }
}
