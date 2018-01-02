using System.Threading.Tasks;
using Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using Models.OneDriveModels;
using Newtonsoft.Json;
using System.Text;
using System.IO;

namespace FileServiceApi.Services
{
    public class OneDriveService : IService
    {
        private string BuildAuthLink()
        {
            return string.Format("{0}?client_id={1}&scope={2}&response_type=code&redirect_uri={3}",
                OneDriveAppClient.LoginUrl,
                OneDriveAppClient.ClientId,
                OneDriveAppClient.Scope,
                OneDriveAppClient.RedirectUrl);
        }

        public string GetAuthLink() => BuildAuthLink();
        
        public async Task<string> GetToken(AuthorizationModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(OneDriveAppClient.RedeemTokenUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("code", model.Code),
                new KeyValuePair<string, string>("client_id", OneDriveAppClient.ClientId),
                new KeyValuePair<string, string>("redirect_uri", OneDriveAppClient.RedirectUrl),
                new KeyValuePair<string, string>("client_secret", OneDriveAppClient.ClientSecret),
                new KeyValuePair<string, string>("grant_type", OneDriveAppClient.GrandType)
                });

                var result = await client.PostAsync("/oauth20_token.srf", content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> GET https://graph.microsoft.com/v1.0/me/drive/{itemId}/children </summary>
        public async Task<string> GetList(string token, string itemId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(OneDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var result = await client.GetAsync(string.Format("/v1.0/me/drive/items/{0}/children", itemId));
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> GET https://graph.microsoft.com/v1.0/me/drive/items/{fileId} </summary>
        public async Task<string> GetFile(string token, string fileId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(OneDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var result = await client.GetAsync("/v1.0/me/drive/items/" + fileId);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> POST https://graph.microsoft.com/v1.0/me/drive/items/{fileId}/copy </summary>
        public async Task<string> CopyFile(string token, string fileId, string copyToFolderId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(OneDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var model = new CopyFileModel()
                {
                    name = "copy.txt",
                    parentReference = new ParentReference() { id = copyToFolderId }
                };                

                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var result = await client.PostAsync(string.Format("/v1.0/me/drive/items/{0}/copy", fileId), content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> DELETE https://graph.microsoft.com/v1.0/me/drive/items/{fileId} </summary>
        public async Task<string> DeleteFile(string token, string fileId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(OneDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var result = await client.DeleteAsync("/v1.0/me/drive/items/" + fileId);
                return await result.Content.ReadAsStringAsync();
            }
        }

        public Task<string> GetLinkToDownloadFile(string token, string path)
        {
            //use GetFile() instead
            throw new System.NotImplementedException();
        }

        /// <summary> PUT https://graph.microsoft.com/v1.0/me/drive/items/{parentId}:/{file_name}:/content </summary>
        public async Task<string> UploadFile(string token, string parentId, string fileContent)
        {
            byte[] array = File.ReadAllBytes(fileContent);
            var fileName = "uploaded_file.txt";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(OneDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var content = new ByteArrayContent(array);

                var result = await client.PutAsync(string.Format("/v1.0/me/drive/items/{0}:/{1}:/content", parentId, fileName), content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        internal abstract class OneDriveAppClient
        {
            public static string Scope { get; } = "files.readwrite.all";
            public static string LoginUrl { get; } = @"https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
            public static string RedeemTokenUrl { get; } = @"https://login.live.com";
            public static string ClientId { get; } = "6dc047d6-c080-41ed-b314-d5067a98343a";
            public static string ClientSecret { get; } = "mtyLFPA2117}~$wmyrHII5[";
            public static string RedirectUrl { get; } = @"http://localhost:46278/swagger";
            public static string GrandType { get; } = "authorization_code";
            public static string BaseUrl { get; } = @"https://graph.microsoft.com";
        }
    }
}
