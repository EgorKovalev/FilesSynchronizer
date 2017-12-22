using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FileServiceApi.Services
{
    public class DropboxService : IService
    {
        private string BuildAuthLink()
        {
            return string.Format(DropboxAppClient.BaseUrl + "/oauth2/authorize?response_type=code&client_id={0}&redirect_uri={1}",                
                DropboxAppClient.ClientId,
                DropboxAppClient.RedirectUrl);            
        }

        public string GetAuthLink() => BuildAuthLink();

        public async Task<string> GetToken(AuthorizationModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DropboxAppClient.BaseApiUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("code", model.Code),
                new KeyValuePair<string, string>("client_id", DropboxAppClient.ClientId),
                new KeyValuePair<string, string>("client_secret", DropboxAppClient.ClientSecret),
                new KeyValuePair<string, string>("grant_type", DropboxAppClient.GrantType),
                new KeyValuePair<string, string>("redirect_uri", DropboxAppClient.RedirectUrl)
                });

                var result = await client.PostAsync("/oauth2/token", content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> POST https://api.dropboxapi.com/2/files/list_folder </summary>
        public async Task<string> GetList(string token, string path = "")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DropboxAppClient.BaseApiUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var model = new Dictionary<string, string>() { { "path", path } };
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var result = await client.PostAsync("/2/files/list_folder", content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> POST https://api.dropboxapi.com/2/files/get_metadata </summary>
        public async Task<string> GetFile(string token, string path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DropboxAppClient.BaseApiUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var model = new Dictionary<string, string>() { { "path", path } };
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var result = await client.PostAsync("/2/files/get_metadata", content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> POST https://api.dropboxapi.com/2/files/copy </summary>
        public async Task<string> CopyFile(string token, string path, string copyTo)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DropboxAppClient.BaseApiUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var model = new Dictionary<string, string>() { { "from_path", path }, { "to_path", copyTo } };
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");                

                var result = await client.PostAsync("/2/files/copy", content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> POST https://api.dropboxapi.com/2/files/delete </summary>
        public async Task<string> DeleteFile(string token, string path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DropboxAppClient.BaseApiUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var model = new Dictionary<string, string>() { { "path", path } };
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                var result = await client.PostAsync("/2/files/delete", content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> POST https://content.dropboxapi.com/2/files/download </summary>
        public async Task<string> DownloadFile(string token, string path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(DropboxAppClient.BaseContentUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);                
                client.DefaultRequestHeaders.Add("Dropbox-API-Arg", "{\"path\":\"" + path + "\"}");

                var result = await client.PostAsync("/2/files/download", null);
                return await result.Content.ReadAsStringAsync();
            }
        }

        internal abstract class DropboxAppClient
        {
            public static string RedirectUrl { get; } = @"http://localhost:46278/swagger";
            public static string ClientId { get; } = "x6vr1brv2shxh9v";
            public static string ClientSecret { get; } = "mjqcsoxxqwbm6fy";
            public static string GrantType { get; } = "authorization_code";
            public static string BaseApiUrl { get; } = @"https://api.dropboxapi.com";
            public static string BaseUrl { get; } = @"https://www.dropbox.com";
            public static string BaseContentUrl { get; } = @"https://content.dropboxapi.com";
        }
    }
}
