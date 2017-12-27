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
        private readonly DropboxService _service;

        public dropboxController() { _service = new DropboxService(); }

        /// <summary>
        /// GET api/dropbox/authlink
        /// </summary>
        /// <returns>link to login into the yandex disc service</returns>        
        [HttpGet("authlink")]
        public string GetAuthCode()
        {            
            return _service.GetAuthLink();
        }

        /// <summary>
        /// GET api/dropbox/token
        /// </summary>
        /// <param name="model">contains a code to get authorization token</param>
        /// <returns>authorization token</returns>
        [HttpPost("token")]
        public DropboxTokenModel GetToken(AuthorizationModel model)
        {            
            var json = _service.GetToken(model).Result;
            return JsonConvert.DeserializeObject<DropboxTokenModel>(json);
        }

        /// <summary>
        /// POST api/dropbox/list
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to get</param>
        /// <returns>model, that contains files list</returns>
        [HttpPost("list")]
        public Dictionary<string, string> GetList(RequestTokenModel model, string path = "")
        {
            var newPath = path.Replace(">", "/"); //Temporary solution. Only for a swagger's bug                       

            var json = _service.GetList(model.Token, newPath).Result;
            var files = JsonConvert.DeserializeObject<FilesListModel>(json).entries as List<DropboxItem>;

            var list = new Dictionary<string, string>();
            files.ForEach(file => list.Add(file.path_lower, file.name));

            return list;
        }

        /// <summary>
        /// POST api/dropbox/file/{path}
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to get</param>
        /// <returns>files resource</returns>
        [HttpPost("file/{path}")]
        public DropboxItem GetFile(RequestTokenModel model, string path)
        {
            var newPath = path.Replace(">", "/"); //Temporary solution. Only for a swagger's bug
                        
            var json = _service.GetFile(model.Token, newPath).Result;
            return JsonConvert.DeserializeObject<DropboxItem>(json);
        }
        
        /// <summary>
        /// POST api/dropbox/file/{path}/copyto/{pathTo}
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to copy from</param>
        /// <param name="pathTo">file path to copy to</param>        
        /// (!) please use '>' instead of '/' because of https://github.com/OAI/OpenAPI-Specification/issues/892 (!)
        [HttpPost("file/{path}/copyto/{pathTo}")]
        public void CopyFileTo(RequestTokenModel model, string path, string pathTo)
        {
            var newPath = path.Replace(">", "/"); //Temporary solution. Only for a swagger's bug
            var newPathTo = pathTo.Replace(">", "/"); //Temporary solution. Only for a swagger's bug
                        
            var json = _service.CopyFile(model.Token, newPath, newPathTo).Result;
        }

        /// <summary>
        /// POST api/dropbox/file/{path}/delete
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to delete</param>
        /// (!) please use '>' instead of '/' because of https://github.com/OAI/OpenAPI-Specification/issues/892 (!)
        [HttpPost("file/{path}/delete")]
        public void DeleteFile(RequestTokenModel model, string path)
        {
            var newPath = path.Replace(">", "/"); //Temporary solution. Only for swagger bug
            
            var json = _service.DeleteFile(model.Token, newPath).Result;
        }

        /// <summary>
        /// POST api/dropbox/file/{path}/download
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to download</param>
        /// (!) please use '>' instead of '/' because of https://github.com/OAI/OpenAPI-Specification/issues/892 (!)
        [HttpPost("file/{path}/download")]
        public string DownloadFile(RequestTokenModel model, string path)
        {
            var newPath = path.Replace(">", "/"); //Temporary solution. Only for swagger bug
                        
            var json = _service.GetLinkToDownloadFile(model.Token, newPath).Result;
            return JsonConvert.DeserializeObject<LinkToDownloadModel>(json).link;
        }

        /// <summary>
        /// POST api/dropbox/file/{path}/upload
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to upload</param>
        /// (!) please use '>' instead of '/' because of https://github.com/OAI/OpenAPI-Specification/issues/892 (!)
        [HttpPost("file/{path}/upload")]
        public void UploadFile(RequestTokenModel model, string path)
        {
            var fileContent = @"C:\Test\testDoc.txt";
            var newPath = path.Replace(">", "/"); //Temporary solution. Only for swagger bug
                        
            var json = _service.UploadFile(model.Token, newPath, fileContent);
        }
    }
}