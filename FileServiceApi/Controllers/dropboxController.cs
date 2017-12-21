using Microsoft.AspNetCore.Mvc;
using FileServiceApi.Services;
using Models;
using Newtonsoft.Json;
using Models.DropboxModels;
using System.Collections.Generic;

namespace FileServiceApi.Controllers
{
    [Produces("application/json")]
    [Route("api/dropbox")]
    public class dropboxController : Controller
    {
        /// <summary>
        /// GET api/dropbox/authlink
        /// </summary>
        /// <returns>link to login into the yandex disc service</returns>        
        [HttpGet("authlink")]
        public string GetAuthCode()
        {
            var service = new DropboxService();
            return service.GetAuthLink();
        }

        /// <summary>
        /// GET api/dropbox/token
        /// </summary>
        /// <param name="model">contains a code to get authorization token</param>
        /// <returns>authorization token</returns>
        [HttpPost("token")]
        public DropboxTokenModel GetToken(AuthorizationModel model)
        {
            var service = new DropboxService();
            var json = service.GetToken(model).Result;

            return JsonConvert.DeserializeObject<DropboxTokenModel>(json);
        }

        /// <summary>
        /// GET api/dropbox/list
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <returns>model, that contains files list</returns>
        [HttpPost("list")]
        public Dictionary<string, string> GetList(RequestTokenModel model)
        {
            var service = new DropboxService();
            var json = service.GetList(model.Token).Result;

            var files = JsonConvert.DeserializeObject<FilesListModel>(json).entries;

            var list = new Dictionary<string, string>();
            foreach (var file in files)
            {
                list.Add(file.id.Replace("id:",""), file.name);
            }

            return list;
        }
    }
}