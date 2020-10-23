using COP4331_RestaurantSystem_DavidGreen.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        }

        public async Task<List<Order>> GetOrders()
        {

            var orders = new List<Order>();

            Uri uri = new Uri("https://192.168.1.93:44317/RestaurantSystem/GetOrders");
            HttpResponseMessage response = await client.GetAsync(uri);
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<List<Order>>(content);
                return orders;
            }
            return orders;
        }

        public async Task<List<MenuItem>> GetMenuItems()
        {
            var menuItems = new List<MenuItem>();

            Uri uri = new Uri("https://192.168.1.93:44317/RestaurantSystem/GetMenuItems");
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
