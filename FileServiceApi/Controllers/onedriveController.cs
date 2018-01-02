using Microsoft.AspNetCore.Mvc;
using FileServiceApi.Services;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Models.OneDriveModels;

namespace FileServiceApi.Controllers
{
    [Produces("application/json")]
    [Route("api/onedrive")]
    public class onedriveController : Controller
    {
        private readonly OneDriveService _service;

        public onedriveController() { _service = new OneDriveService(); }

        /// <summary>
        /// GET api/onedrive/authlink
        /// </summary>
        /// <returns>link to login into the one drive service</returns>        
        [HttpGet("authlink")]
        public string GetAuthCode()
        {
            return _service.GetAuthLink();
        }

        /// <summary>
        /// GET api/onedrive/token
        /// </summary>
        /// <param name="model">contains a code to get authorization token</param>
        /// <returns>authorization token</returns>
        [HttpPost("token")]
        public TokenModel GetToken(AuthorizationModel model)
        {
            var json = _service.GetToken(model).Result;
            return JsonConvert.DeserializeObject<TokenModel>(json);
        }

        /// <summary>
        /// GET api/onedrive/list
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="folderId">parent folder to get child items</param>
        /// <returns>model, that contains files list</returns>
        [HttpGet("list")]
        public Dictionary<string, string> GetList(RequestTokenModel model, string folderId = "root")
        {
            var json = _service.GetList(model.Token, folderId).Result;
            var files = JsonConvert.DeserializeObject<FilesListModel>(json).value as List<OneDriveItem>;

            var list = new Dictionary<string, string>();
            files.ForEach(file => list.Add(file.name, file.id));

            return list;
        }

        /// <summary>
        /// GET api/onedrive/file/{fileId}
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to get</param>
        /// <returns>files resource</returns>
        [HttpGet("file/{fileId}")]
        public OneDriveItem GetFile(RequestTokenModel model, string fileId)
        {
            var json = _service.GetFile(model.Token, fileId).Result;

            return JsonConvert.DeserializeObject<OneDriveItem>(json);
        }

        /// <summary>
        /// POST api/onedrive/file/{id}/copyto/{parentId}
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to copy</param>
        /// <param name="parentId">parent item id</param>        
        [HttpPost("file/{fileId}/copyto/{parentId}")]
        public void CopyFileTo(RequestTokenModel model, string fileId, string parentId)
        {
            var json = _service.CopyFile(model.Token, fileId, parentId).Result;
        }

        /// <summary>
        /// DELETE api/onedrive/file/{fileId}/delete
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to delete</param>
        [HttpDelete("file/{fileId}/delete")]
        public void DeleteFile(RequestTokenModel model, string fileId)
        {
            var json = _service.DeleteFile(model.Token, fileId).Result;
        }

        /// <summary>
        /// GET api/onedrive/file/{fileId}/download
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to download</param>        
        [HttpGet("file/{fileId}/download")]
        public string DownloadFile(RequestTokenModel model, string fileId)
        {
            var json = _service.GetFile(model.Token, fileId).Result;

            return JsonConvert.DeserializeObject<OneDriveItem>(json).DownloadUrl;
        }

        /// <summary>
        /// PUT api/onedrive/file/{path}/upload
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="parentId">file path to upload</param>        
        [HttpPut("file/{parentId}/upload")]
        public void UploadFile(RequestTokenModel model, string parentId)
        {
            var fileContent = @"C:\Test\testDoc.txt";                        
            var res = _service.UploadFile(model.Token, parentId, fileContent);
        }
    }
}