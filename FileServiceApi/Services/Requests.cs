using System.Net.Http;

namespace FileServiceApi.Services
{
    public abstract class Requests
    {
        public static HttpRequestMessage GetList()
        {
            return new HttpRequestMessage(HttpMethod.Get, "");
        }
    }
}
