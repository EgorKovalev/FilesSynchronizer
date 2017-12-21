using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
                client.BaseAddress = new Uri(DropboxAppClient.BaseAuthUrl);
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

        public Task<string> GetList(string token, string path)
        {
            throw new NotImplementedException();
        }

        public Task<string> CopyFile(string token, string path, string copyTo)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteFile(string token, string path)
        {
            throw new NotImplementedException();
        }
        internal abstract class DropboxAppClient
        {
            public static string RedirectUrl { get; } = @"http://localhost:46278/swagger";
            public static string ClientId { get; } = "x6vr1brv2shxh9v";
            public static string ClientSecret { get; } = "mjqcsoxxqwbm6fy";
            public static string GrantType { get; } = "authorization_code";
            public static string BaseAuthUrl { get; } = @"https://api.dropboxapi.com";
            public static string BaseUrl { get; } = @"https://www.dropbox.com";
        }
    }
}
