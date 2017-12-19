using Microsoft.AspNetCore.Mvc;
using FileServiceApi.Services;
using Models;
using Newtonsoft.Json;
using Google.Apis.Drive.v3.Data;
using System.Collections.Generic;

namespace FileServiceApi.Controllers
{
    [Produces("application/json")]
    [Route("api/googledrive")]
    public class googledriveController : Controller
    {
        /// <summary>
        /// GET api/googledrive/authlink
        /// </summary>
        /// <returns>link to login into the google drive service</returns>        
        [HttpGet("authlink")]
        public string GetAuthCode()
        {
            var service = new GoogleService();
            return service.GetAuthLink();            
        }

        /// <summary>
        /// GET api/googledrive/token
        /// </summary>
        /// <param name="model">contains a code to get authorization token</param>
        /// <returns>authorization token</returns>
        [HttpPost("token")]
        public TokenModel GetToken(AuthorizationModel model)
        {
            var service = new GoogleService();
            var json = service.GetToken(model).Result;            

            return JsonConvert.DeserializeObject<TokenModel>(json);
        }

        /// <summary>
        /// GET api/googledrive/list
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <returns>model, that contains files list</returns>
        [HttpGet("list")]
        public Dictionary<string, string> GetList(RequestTokenModel model)
        {
            var service = new GoogleService();
            var json = service.GetList(model.Token).Result;

            var files = JsonConvert.DeserializeObject<FilesListModel>(json).files;

            var list = new Dictionary<string, string>();
            foreach(var file in files)
            {
                list.Add(file.Id, file.Name);
            }

            return list;
        }

        /// <summary>
        /// GET api/googledrive/file/{id}
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to get</param>
        /// <returns>files resource</returns>
        [HttpGet("file/{fileId}")]
        public File GetFile(RequestTokenModel model, string fileId)
        {
            var service = new GoogleService();
            var json = service.GetFile(model.Token, fileId).Result;

            return JsonConvert.DeserializeObject<File>(json);
        }

        /// <summary>
        /// POST api/googledrive/file/{id}/copy
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to copy</param>
        /// <returns>files resource</returns>
        [HttpPost("file/{fileId}/copy")]
        public File CopyFile(RequestTokenModel model, string fileId)
        {
            var service = new GoogleService();
            var json = service.CopyFile(model.Token, fileId).Result;

            return JsonConvert.DeserializeObject<File>(json);
        }

        /// <summary>
        /// POST api/googledrive/file/{id}/copyto/{parentId}
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to copy</param>
        /// <param name="parentId">parent item id</param>
        /// <returns>files resource</returns>
        [HttpPost("file/{fileId}/copyto/{parentId}")]
        public File CopyFileTo(RequestTokenModel model, string fileId, string parentId)
        {
            var service = new GoogleService();
            var json = service.CopyFile(model.Token, fileId, parentId).Result;

            return JsonConvert.DeserializeObject<File>(json);
        }

        /// <summary>
        /// DELETE api/googledrive/file/{fileId}/delete
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to delete</param>
        [HttpDelete("file/{fileId}/delete")]
        public void DeleteFile(RequestTokenModel model, string fileId)
        {
            var service = new GoogleService();
            var json = service.DeleteFile(model.Token, fileId).Result;
        }

        /// <summary>
        /// GET api/googledrive/{folderId}/list
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <returns>model, that contains files list</returns>
        [HttpGet("{folderId}/list")]
        public IList<string> GetList(RequestTokenModel model, string folderId)
        {
            var service = new GoogleService();
            var json = service.GetList(model.Token, folderId).Result;
            
            var items = JsonConvert.DeserializeObject<ChildrenListModel>(json).items;

            var list = new List<string>();
            foreach (var item in items)
            {
                list.Add(item.id);
            }

            return list;
        }
    }
}