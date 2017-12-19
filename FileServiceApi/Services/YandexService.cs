using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FileServiceApi.Services
{
    public class YandexService : IService
    {
        public YandexService()
        { }

        private string BuildAuthLink()
        {
            return YandexDiscAppClient.BaseUrl + "/authorize?response_type=code&client_id=" + YandexDiscAppClient.ClientId;
        }

        public string GetAuthLink()
        {
            var link = BuildAuthLink();

            return BuildAuthLink();
        }

        public async Task<string> GetToken(AuthorizationModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(YandexDiscAppClient.BaseUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("code", model.Code),                
                new KeyValuePair<string, string>("client_id", YandexDiscAppClient.ClientId),
                new KeyValuePair<string, string>("client_secret", YandexDiscAppClient.ClientSecret),
                new KeyValuePair<string, string>("grant_type", YandexDiscAppClient.GrandType)
                });

                var result = await client.PostAsync("/token", content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        internal abstract class YandexDiscAppClient
        {
            public static string RedirectUrl { get; } = @"http://localhost:46278/swagger";
            public static string ClientId { get; } = "80efe41b160b4dbe89dbba9b9f33e543";
            public static string ClientSecret { get; } = "93fe81cc72344967a039d3969b2ee7d8";
            public static string GrandType { get; } = "authorization_code";
            public static string BaseUrl { get; } = @"https://oauth.yandex.ru";
        }
    }
}
