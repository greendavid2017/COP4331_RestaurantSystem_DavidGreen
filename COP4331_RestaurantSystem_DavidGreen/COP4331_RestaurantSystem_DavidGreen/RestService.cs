using COP4331_RestaurantSystem_DavidGreen.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace COP4331_RestaurantSystem_DavidGreen
{
    class RestService
    {
        HttpClient client;
        
        public RestService()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://192.168.1.93:44317/");
            var apiToken = SecureStorage.GetAsync("apiToken").Result;
            var email = SecureStorage.GetAsync("email").Result;
            if(apiToken != null && email != null)
            {
                client.DefaultRequestHeaders.Add("apiToken", apiToken);
                client.DefaultRequestHeaders.Add("email", email);
            }
        }

        public async Task<List<Order>> GetOrders()
        {

            var orders = new List<Order>();

            Uri uri = new Uri("RestaurantSystem/GetOrders");
            HttpResponseMessage response = await client.GetAsync(uri);
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<List<Order>>(content);
                return orders;
            }
            return orders;
        }

        public async Task<String> Login(string email, string password)
        {
            var json = JsonConvert.SerializeObject(new { email = email, password = password });
            var response = await client.PostAsync("RestaurantSystem/Login", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                string tokenString = content;
                return tokenString;
            }
            return null;
        }

        public async Task<List<MenuItem>> GetMenuItems()
        {
            var menuItems = new List<MenuItem>();

            Uri uri = new Uri("RestaurantSystem/GetMenuItems");
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                menuItems = JsonConvert.DeserializeObject<List<MenuItem>>(content);
                return menuItems;
            }
            return menuItems;
        }
    }
}
