using COP4331_RestaurantSystem_DavidGreen.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace COP4331_RestaurantSystem_DavidGreen
{
    class RestService
    {
        HttpClient client;
        
        public RestService()
        {
            
        }

        public async Task Initialize()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://192.168.1.93:44317/");
            var apiToken = await SecureStorage.GetAsync("apiToken");
            var email = await SecureStorage.GetAsync("email");

            if (apiToken != null && email != null)
            {
                client.DefaultRequestHeaders.Add("apiToken", apiToken);
                client.DefaultRequestHeaders.Add("email", email);

                // If your current token is not valid we need to make the user re-login
                if (await (CheckToken()) == false)
                {
                    await Shell.Current.GoToAsync("login");
                }

                return;
            }
            await Shell.Current.GoToAsync("login");
        }

        public void InitializeLogin()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://192.168.1.93:44317/");
        }

        public async Task<bool> CheckToken()
        {
            HttpResponseMessage response = await client.GetAsync("RestaurantSystem/CheckToken");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Models.Order>> GetOrders()
        {

            var orders = new List<Order>();

            HttpResponseMessage response = await client.GetAsync("RestaurantSystem/GetOrders");
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<List<Models.Order>>(content);
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

        public async Task<String> Register(string email, string password)
        {
            var json = JsonConvert.SerializeObject(new { email = email, password = password });
            var response = await client.PostAsync("RestaurantSystem/Register", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                string tokenString = content;
                return tokenString;
            }
            return null;
        }

        public async Task<bool> UpdateOrderStatus(int orderId, int orderStatus)
        {
            var json = JsonConvert.SerializeObject(new { orderId = orderId, orderStatus = orderStatus });
            var response = await client.PostAsync("RestaurantSystem/UpdateOrderStatus", new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Models.MenuItem>> GetMenuItems()
        {
            var menuItems = new List<Models.MenuItem>();
            HttpResponseMessage response = await client.GetAsync("RestaurantSystem/GetMenuItems");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                menuItems = JsonConvert.DeserializeObject<List<Models.MenuItem>>(content);
                return menuItems;
            }
            return menuItems;
        }

        public async Task<Models.User> GetUserOrders(String email)
        {
            HttpResponseMessage response = await client.GetAsync($"RestaurantSystem/GetUserOrders?email={email}");
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Models.User>(content);
                return user;
            }
            return null;
        }

        public async Task<bool> CreateOrder(String email, List<Models.MenuItem> orderItems)
        {
            var json = JsonConvert.SerializeObject(new {email = email, orderItems = orderItems});
            HttpResponseMessage response = await client.PostAsync("RestaurantSystem/CreateOrder", new StringContent(json, Encoding.UTF8, "application/json"));
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

    }
}
