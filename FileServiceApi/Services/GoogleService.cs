using Models;
using Models.GoogleModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FileServiceApi.Services
{
    public class GoogleService : IService
    {        
        private string BuildAuthLink()
        {
            var baseUrl = @"https://accounts.google.com/o/oauth2/v2/auth";

            return string.Format("{0}?redirect_uri={1}&prompt=consent&response_type=code&client_id={2}&scope={3}",
                baseUrl,
                GoogleDriveAppClient.RedirectUrl,
                GoogleDriveAppClient.ClientId,
                GoogleDriveAppClient.Scope);            
        }

        public string GetAuthLink() => BuildAuthLink();

        public async Task<string> GetToken(AuthorizationModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GoogleDriveAppClient.BaseUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("code", model.Code),
                new KeyValuePair<string, string>("redirect_uri", GoogleDriveAppClient.RedirectUrl),
                new KeyValuePair<string, string>("client_id", GoogleDriveAppClient.ClientId),
                new KeyValuePair<string, string>("client_secret", GoogleDriveAppClient.ClientSecret),
                new KeyValuePair<string, string>("grant_type", GoogleDriveAppClient.GrandType)
                });

                var result = await client.PostAsync("/oauth2/v4/token", content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> GET https://www.googleapis.com/drive/v3/files </summary>
        public async Task<string> GetList(string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GoogleDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var result = await client.GetAsync("/drive/v3/files");
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> GET https://www.googleapis.com/drive/v3/files/fileId </summary>
        public async Task<string> GetFile(string token, string fileId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GoogleDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var result = await client.GetAsync("/drive/v3/files/" + fileId);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> POST https://www.googleapis.com/drive/v3/files/fileId/copy </summary>
        public async Task<string> CopyFile(string token, string fileId, string parentId = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GoogleDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var model = new RequestCopyModel();
                model.parents = new string[] { parentId };

                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var result = await client.PostAsync("/drive/v3/files/" + fileId + "/copy", content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> DELETE https://www.googleapis.com/drive/v3/files/fileId </summary>
        public async Task<string> DeleteFile(string token, string fileId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GoogleDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", token);
                
                var result = await client.DeleteAsync("/drive/v3/files/" + fileId);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> GET https://www.googleapis.com/drive/v2/files/folderId/children </summary>
        public async Task<string> GetList(string token, string folderId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GoogleDriveAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var result = await client.GetAsync("/drive/v2/files/" + folderId + "/children");
                return await result.Content.ReadAsStringAsync();
            }
        }

        internal abstract class GoogleDriveAppClient
        {
            public static string RedirectUrl { get; } = @"http://localhost:46278/swagger";
            public static string ClientId { get; } = "949586784197-ogg773sid1bs5hco4hurjiqi93gm58ku.apps.googleusercontent.com";
            public static string ClientSecret { get; } = "XsP_id5z7UfBNIJGeGfi7zWL";
            public static string GrandType { get; } = "authorization_code";
            public static string Scope { get; } = @"https://www.googleapis.com/auth/drive";
            public static string BaseUrl { get; } = @"https://www.googleapis.com";
        }
    }
}
