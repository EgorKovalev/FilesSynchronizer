using Microsoft.AspNetCore.Mvc;
using Models;
using FileServiceApi.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using Models.YandexModels;

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
            return service.GetAuthLink();
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

        /// <summary>
        /// GET api/yandexdisc/list
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <returns>model, that contains files list</returns>
        [HttpGet("list")]
        public Dictionary<string, string> GetList(RequestTokenModel model)
        {
            var service = new YandexService();
            var json = service.GetList(model.Token).Result;

            var files = JsonConvert.DeserializeObject<FilesListModel>(json)._embedded.items;

            var list = new Dictionary<string, string>();
            foreach (var file in files)
            {
                list.Add(file.name, file.path.Replace("disk:",""));
            }

            return list;
        }

        /// <summary>
        /// GET api/yandexdisc/file/{path}
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to get</param>
        /// <returns>files resource</returns>
        [HttpGet("file/{path}")]
        public FilesListModel GetFile(RequestTokenModel model, string path)
        {
            var service = new YandexService();
            var json = service.GetList(model.Token, path).Result;

            return JsonConvert.DeserializeObject<FilesListModel>(json);
        }

        /// <summary>
        /// POST api/yandexdisc/file/{path}/copy
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to copy from</param>        
        [HttpPost("file/{path}/copy")]
        public void CopyFile(RequestTokenModel model, string path)
        {
            var service = new YandexService();
            var json = service.CopyFile(model.Token, path).Result;            
        }

        /// <summary>
        /// POST api/yandexdisc/file/{path}/copyto/{pathTo}
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to copy from</param>
        /// <param name="pathTo">file path to copy to</param>        
        /// (!) please use '>' instead of '/' because of https://github.com/OAI/OpenAPI-Specification/issues/892 (!)
        [HttpPost("file/{path}/copyto/{pathTo}")]
        public void CopyFileTo(RequestTokenModel model, string path, string pathTo)
        {
            var newPath = pathTo.Replace(">", "/"); //Temporary solution. Only for a swagger's bug
            var service = new YandexService();
            var json = service.CopyFile(model.Token, path, newPath).Result;
        }

        /// <summary>
        /// DELETE api/yandexdisc/file/{path}/delete
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to delete</param>
        [HttpDelete("file/{path}/delete")]
        public void DeleteFile(RequestTokenModel model, string path)
        {
            var newPath = path.Replace(">", "/"); //Temporary solution. Only for swagger bug
            var service = new YandexService();
            var json = service.DeleteFile(model.Token, newPath).Result;
        }
    }
}