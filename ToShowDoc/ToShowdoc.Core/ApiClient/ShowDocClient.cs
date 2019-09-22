using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToShowDoc.Core.Entity;

namespace ToShowDoc.Core.ApiClient
{
    public class ShowDocClient
    {
        private static readonly HttpClient HttpClient;

        static ShowDocClient()
        {
            HttpClient = new HttpClient();
        }

        public static async Task<ShowDocResponse> UpdateByApi(ShowDocEntity entity, ShowDocRequest request)
        {
           
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("api_key",entity.AppKey),
                new KeyValuePair<string, string>("api_token",entity.AppToken),
                new KeyValuePair<string, string>("cat_name",request.CatName),
                new KeyValuePair<string, string>("page_title",request.PageTitle),
                new KeyValuePair<string, string>("page_content",request.PageContent),
                new KeyValuePair<string, string>("s_number",request.SNumber.ToString()),
            });

            var response = await HttpClient.PostAsync(entity.ShowDocUrl, content);

            return JsonConvert.DeserializeObject<ShowDocResponse>(await response.Content.ReadAsStringAsync());
        }

    }
}
