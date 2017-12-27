using Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace FileServiceApi.Services
{
    public class YandexService : IService
    {
        private const int _maxItemsOnPage = 10000;

        private string BuildAuthLink()
        {
            return YandexDiscAppClient.BaseAuthUrl + "/authorize?response_type=code&client_id=" + YandexDiscAppClient.ClientId;
        }

        public string GetAuthLink() => BuildAuthLink();

        public async Task<string> GetToken(AuthorizationModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(YandexDiscAppClient.BaseAuthUrl);
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

        /// <summary> GET https://cloud-api.yandex.net/v1/disk/resources?path={path}&limit={_maxItemsOnPage} </summary>
        public async Task<string> GetList(string token, string path = "/")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(YandexDiscAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", token);

                var result = await client.GetAsync(string.Format("/v1/disk/resources?path={0}&limit={1}", path, _maxItemsOnPage));
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> POST https://cloud-api.yandex.net/v1/disk/resources/copy </summary>
        public async Task<string> CopyFile(string token, string path, string copyToPath = null)
        {
            if (copyToPath is null) copyToPath = path + "-copy";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(YandexDiscAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", token);

                var result = await client.PostAsync(string.Format("/v1/disk/resources/copy?from={0}&path={1}", path, copyToPath), null);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> DELETE https://cloud-api.yandex.net/v1/disk/resources </summary>
        public async Task<string> DeleteFile(string token, string path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(YandexDiscAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", token);

                var result = await client.DeleteAsync("/v1/disk/resources?path=" + path);
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> GET https://cloud-api.yandex.net/v1/disk/resources/download </summary>
        public async Task<string> DownloadFile(string token, string path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(YandexDiscAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", token);                

                var result = await client.GetAsync(string.Format("/v1/disk/resources/download?path={0}", path));
                return await result.Content.ReadAsStringAsync();
            }
        }

        /// <summary> PUT <generated_link> </summary>
        public async Task<string> UploadFile(string token, string url, string fileContent)
        {
            byte[] array = File.ReadAllBytes(fileContent);

            using (var client = new HttpClient())
            {
                var content = new ByteArrayContent(array);

                var result = await client.PutAsync(url, content);
                return await result.Content.ReadAsStringAsync();
            }            
        }

        /// <summary> GET https://cloud-api.yandex.net/v1/disk/resources/upload </summary>
        public async Task<string> GetUploadModel(string token, string path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(YandexDiscAppClient.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth", token);

                var result = await client.GetAsync(string.Format("/v1/disk/resources/upload?path={0}", path));
                return await result.Content.ReadAsStringAsync();
            }
        }

        internal abstract class YandexDiscAppClient
        {
            public static string RedirectUrl { get; } = @"http://localhost:46278/swagger";
            public static string ClientId { get; } = "80efe41b160b4dbe89dbba9b9f33e543";
            public static string ClientSecret { get; } = "93fe81cc72344967a039d3969b2ee7d8";
            public static string GrandType { get; } = "authorization_code";
            public static string BaseAuthUrl { get; } = @"https://oauth.yandex.ru";
            public static string BaseUrl { get; } = @"https://cloud-api.yandex.net";
        }
    }
}
