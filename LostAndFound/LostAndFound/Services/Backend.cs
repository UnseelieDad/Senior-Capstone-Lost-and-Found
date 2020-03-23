using LostAndFound.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Services
{
    public class Backend
    {
        private static readonly HttpClient client;
        private static readonly string lostItemsUrl = "https://7g66xjlz37.execute-api.us-east-2.amazonaws.com/test/lostitems";
        private static readonly string foundItemsUrl = "https://7g66xjlz37.execute-api.us-east-2.amazonaws.com/test/founditems";

        static Backend()
        {
            client = new HttpClient();
        }

        public static async Task<T> DoRequest<T>(HttpMethod method, string url, object body = null)
        {
            using (var request = CreateHttpRequest(HttpMethod.Get, url))
            {
                try
                {
                    var response = await client.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            T obj = JsonConvert.DeserializeObject<T>(content);
                            return obj;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return default;
                        }
                    }
                    else
                    {
                        return default;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return default;
                }
            }
        }

        public static async Task<object> AdminLogin(string pin)
        {
            return null;
        }

        public static async Task<object> ReportLostItem(object item)
        {
            return null;
        }

        public static async Task<List<Item>> GetLostItems()
        {
            var response = await DoRequest<Response>(HttpMethod.Get, lostItemsUrl);
            response.Body = response.Body.Replace("0000-00-00", "0001-01-01");
            var obj = JsonConvert.DeserializeObject<List<Item>>(response.Body, new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            });
            return obj;
        }

        private static HttpRequestMessage CreateHttpRequest(HttpMethod method, string url, object body = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = content;
            }

            return request;
        }

        private static object GetProperty(object o, string property)
        {
            return o.GetType().GetProperty(property)?.GetValue(o, null);
        }
    }
}
