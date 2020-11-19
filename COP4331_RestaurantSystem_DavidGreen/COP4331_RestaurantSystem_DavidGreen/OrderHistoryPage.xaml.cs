using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COP4331_RestaurantSystem_DavidGreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderHistoryPage : ContentPage
    {
        private int onAppearingCounter = 0;
        public OrderHistoryPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            // There is a bug in Xamarin forms where OnAppearing runs twice for no reason, so use this counter to only run 1/2 of the time
            onAppearingCounter++;
            if (onAppearingCounter % 2 == 1)
            {
                loadingOrderHistoryIndicator.IsRunning = true;
                RestService service = new RestService();
                await service.Initialize();
                var email = await SecureStorage.GetAsync("email");
                var orders = await service.GetUserOrders(email);
                orderHistoryListView.ItemsSource = null;
                orders.Sort(delegate (Models.Order o1, Models.Order o2)
                {
                    return DateTime.Compare(o1.Submitted, o2.Submitted) * -1;
                });
                orderHistoryListView.ItemsSource = null;
                orderHistoryListView.ItemsSource = orders;
                loadingOrderHistoryIndicator.IsRunning = false;
            }
        }

        private async void orderHistoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var order = e.Item as Models.Order;

            // Get all unique names of menu items in this order
            List<Models.OrderItem> uniqueOrderItems = order.OrderItems.GroupBy(i => i.MenuItem.Name).Select(grp => grp.FirstOrDefault()).ToList();

            // Get the count of each unique menu item
            List<int> uniqueOrderItemsCount = new List<int>();
            foreach(Models.OrderItem item in uniqueOrderItems)
            {
                uniqueOrderItemsCount.Add(order.OrderItems.Where(i => i.MenuItem.Name == item.MenuItem.Name).Count());
            }

            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < uniqueOrderItems.Count; i++)
            {
                builder.AppendLine(uniqueOrderItems[i].MenuItem.Name + ": " + uniqueOrderItemsCount[i]);
            }

            await DisplayAlert("Order Info", builder.ToString(), "OK");
            builder.Clear();
        }

        private async void orderHistoryListView_Refreshing(object sender, EventArgs e)
        {
            RestService service = new RestService();
            await service.Initialize();
            var email = await SecureStorage.GetAsync("email");
            var orders = await service.GetUserOrders(email);
            orderHistoryListView.ItemsSource = null;
            orders.Sort(delegate (Models.Order o1, Models.Order o2)
            {
                return DateTime.Compare(o1.Submitted, o2.Submitted) * -1;
            });
            orderHistoryListView.ItemsSource = null;
            orderHistoryListView.ItemsSource = orders;
            orderHistoryListView.IsRefreshing = false;
        }
    }
}