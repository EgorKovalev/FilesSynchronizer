using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileServiceApi.Services
{
    public class GoogleService
    {        
        public GoogleService()
        { }
                       
        private string GetAuthRequestLink()
        {
            var baseUrl = @"https://accounts.google.com/o/oauth2/v2/auth";

            return string.Format("{0}?redirect_uri={1}&prompt=consent&response_type=code&client_id={2}&scope={3}",
                baseUrl,
                GoogleDriveAppClient.RedirectUrl,
                GoogleDriveAppClient.ClientId,
                GoogleDriveAppClient.Scope);            
        }

        public string Login()
        {
            var link = GetAuthRequestLink();           

            return GetAuthRequestLink();
        }

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
