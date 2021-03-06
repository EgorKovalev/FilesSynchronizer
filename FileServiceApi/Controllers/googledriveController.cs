﻿using Microsoft.AspNetCore.Mvc;
using FileServiceApi.Services;
using Models;
using Newtonsoft.Json;
using Google.Apis.Drive.v3.Data;
using System.Collections.Generic;
using Models.GoogleModels;

namespace FileServiceApi.Controllers
{
    [Produces("application/json")]
    [Route("api/googledrive")]
    public class googledriveController : Controller
    {
        private readonly GoogleService _service;

        public googledriveController()
        {
            _service = new GoogleService();
        }

        /// <summary>
        /// GET api/googledrive/authlink
        /// </summary>
        /// <returns>link to login into the google drive service</returns>        
        [HttpGet("authlink")]
        public string GetAuthCode()
        {            
            return _service.GetAuthLink();            
        }

        /// <summary>
        /// GET api/googledrive/token
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
        /// GET api/googledrive/list
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <returns>model, that contains files list</returns>
        [HttpGet("list")]
        public Dictionary<string, string> GetList(RequestTokenModel model)
        {            
            var json = _service.GetList(model.Token).Result;
            var files = JsonConvert.DeserializeObject<FilesListModel>(json).files as List<File>;

            var list = new Dictionary<string, string>();
            files.ForEach(file => list.Add(file.Id, file.Name));

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
            var json = _service.GetFile(model.Token, fileId).Result;

            return JsonConvert.DeserializeObject<File>(json);
        }

        /// <summary>
        /// POST api/googledrive/file/{id}/copy
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to copy</param>        
        [HttpPost("file/{fileId}/copy")]
        public void CopyFile(RequestTokenModel model, string fileId)
        {            
            var json = _service.CopyFile(model.Token, fileId).Result;            
        }

        /// <summary>
        /// POST api/googledrive/file/{id}/copyto/{parentId}
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
        /// DELETE api/googledrive/file/{fileId}/delete
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to delete</param>
        [HttpDelete("file/{fileId}/delete")]
        public void DeleteFile(RequestTokenModel model, string fileId)
        {            
            var json = _service.DeleteFile(model.Token, fileId).Result;
        }

        /// <summary>
        /// GET api/googledrive/{folderId}/list
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <returns>model, that contains files list</returns>
        [HttpGet("{folderId}/list")]
        public IList<string> GetList(RequestTokenModel model, string folderId)
        {            
            var json = _service.GetList(model.Token, folderId).Result;
            
            var items = JsonConvert.DeserializeObject<ChildrenListModel>(json).items as List<ChildrenModel>;
            var list = new List<string>();

            items.ForEach(item => list.Add(item.id));

            return list;
        }

        /// <summary>
        /// GET api/googledrive/file/{fileId}/download
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="fileId">file id to download</param>        
        [HttpGet("file/{fileId}/download")]
        public string DownloadFile(RequestTokenModel model, string fileId)
        {
            return _service.GetLinkToDownloadFile(model.Token, fileId).Result;            
        }

        /// <summary>
        /// PUT api/googledrive/file/{path}/upload
        /// </summary>
        /// <param name="model">contains authorization token</param>
        /// <param name="path">file path to upload</param>        
        [HttpPut("file/{path}/upload")]
        public void UploadFile(RequestTokenModel model, string path)
        {
            var fileContent = @"C:\Test\testDoc.txt";

            var id = GetUploadLink(model, path);
            var res = _service.UploadFile(model.Token, id, fileContent);
        }

        private string GetUploadLink(RequestTokenModel model, string path)
        {
            var uploadModel = new UploadInfoRequestModel()
            {
                description = "file description",
                mimeType = "text/plain",
                title = "test file"
            };

            var json = _service.GetUploadModel(model.Token, uploadModel).Result;
            return JsonConvert.DeserializeObject<UploadInfoResponseModel>(json).id;
        }
    }
}