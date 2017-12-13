using Microsoft.AspNetCore.Mvc;
using FileServiceApi.Services;
using Models;
using Newtonsoft.Json;

namespace FileServiceApi.Controllers
{
    [Produces("application/json")]
    [Route("api/googledrive")]
    public class googledriveController : Controller
    {
        // GET api/googledrive/authcode
        [HttpGet("authcode")]
        public string GetAuthCode()
        {
            var service = new GoogleService();
            return service.GetAuthRequestLink();            
        }

        // GET api/googledrive/token
        [HttpPost("token")]
        public TokenModel GetToken(AuthorizationModel model)
        {
            var service = new GoogleService();
            var json = service.GetToken(model).Result;            

            return JsonConvert.DeserializeObject<TokenModel>(json);
        }        
    }
}