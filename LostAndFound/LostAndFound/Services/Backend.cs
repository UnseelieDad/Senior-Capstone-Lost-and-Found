using LostAndFound.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Services
{
    public class Backend
    {
        private static readonly HttpClient client;
        private static readonly SmtpClient smtpClient;
        private static readonly string lostItemsUrl = "https://7g66xjlz37.execute-api.us-east-2.amazonaws.com/test/lostitems";
        private static readonly string foundItemsUrl = "https://7g66xjlz37.execute-api.us-east-2.amazonaws.com/test/founditems";
        private static readonly string matchedItemsUrl = "https://7g66xjlz37.execute-api.us-east-2.amazonaws.com/test/matcheditems";
        private static readonly string adminUrl = "https://7g66xjlz37.execute-api.us-east-2.amazonaws.com/test/adminlogin";

        static Backend()
        {
            client = new HttpClient();
            smtpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential
                {
                    UserName = "latechlostandfound@outlook.com",
                    Password = "Geauxbulldogs34"
                }
            };
        }

        public static async Task<T> DoRequest<T>(HttpMethod method, string url, object body = null)
        {
            using (var request = CreateHttpRequest(method, url, body))
            {
                try
                {
                    var response = await client.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            content = content.Replace("0000-00-00", "0001-01-01");
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

        public static async Task<Response> AdminLogin(string pin)
        {
            var obj = new { pin };
            var response = await DoRequest<Response>(HttpMethod.Post, adminUrl, obj);
            return response;
        }

        public static async Task<List<Item>> GetLostItems()
        {
            var response = await DoRequest<List<Item>>(HttpMethod.Get, lostItemsUrl);
            return response;
        }

        public static async Task<List<Item>> GetFoundItems()
        {
            var response = await DoRequest<List<Item>>(HttpMethod.Get, foundItemsUrl);
            return response;
        }

        public static async Task<List<MatchedItem>> GetMatchedItems()
        {
            var response = await DoRequest<List<MatchedItem>>(HttpMethod.Get, matchedItemsUrl);
            return response;
        }

        public static async Task<Response> SubmitLostItem(Item i)
        {
            var response = await DoRequest<Response>(HttpMethod.Post, lostItemsUrl, i);
            return response;
        }

        public static Task SendEmailNotification(MatchedItem item)
        {
            var date = $"{item.DateLost.Month}/{item.DateLost.Day}/{item.DateLost.Year.ToString("D4")}";
            var body = $"Hello,\nAn item you previously reported lost on {date} in {item.LostLocation} has been reported as found.\nPlease see the administration desk in Nethken 132.";
            try
            {
                smtpClient.Send("latechlostandfound@outlook.com", item.LostEmail, "One of your lost items has been found!", body);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Task.CompletedTask;
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
