using Microsoft.AspNetCore.Mvc;
using Models;
using FileServiceApi.Services;
using Newtonsoft.Json;

namespace FileServiceApi.Controllers
{
    [Produces("application/json")]
    [Route("api/yandexdisc")]
    public class yandexdiscController : Controller
    {
        /// <summary>
        /// GET api/yandexdisc/authlink
        /// </summary>
        /// <returns>link to login into the yandex disc service</returns>        
        [HttpGet("authlink")]
        public string GetAuthCode()
        {
            var service = new YandexService();
            return service.Login();
        }

        /// <summary>
        /// GET api/yandexdisc/token
        /// </summary>
        /// <param name="model">contains a code to get authorization token</param>
        /// <returns>authorization token</returns>
        [HttpPost("token")]
        public TokenModel GetToken(AuthorizationModel model)
        {
            var service = new YandexService();
            var json = service.GetToken(model).Result;

            return JsonConvert.DeserializeObject<TokenModel>(json);
        }
    }
}