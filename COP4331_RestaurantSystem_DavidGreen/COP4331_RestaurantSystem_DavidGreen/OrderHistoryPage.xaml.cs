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
        private bool isEmployee = false;
        public OrderHistoryPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            loadingOrderHistoryIndicator.IsRunning = true;
            employeeOrderFilterPicker.IsVisible = false;
            employeeOrderFilterPicker.SelectedIndex = 0;
            employeeOrderFilterPicker.IsEnabled = false;
            orderHistoryListView.ItemsSource = null;
            RestService service = new RestService();
            await service.Initialize();
            var email = await SecureStorage.GetAsync("email");
            var user = await service.GetUserOrders(email);
            if (user.IsEmployee)
            {
                this.isEmployee = true;
                employeeOrderFilterPicker.IsVisible = true;
                employeeOrderFilterPicker.SelectedIndex = 0;
                employeeOrderFilterPicker.IsEnabled = true;
            }
            else
            {
                this.isEmployee = false;
                employeeOrderFilterPicker.IsVisible = false;
                employeeOrderFilterPicker.SelectedIndex = 0;
                employeeOrderFilterPicker.IsEnabled = false;
            }
            var orders = user.Orders;
            orders.Sort(delegate (Models.Order o1, Models.Order o2)
            {
                return DateTime.Compare(o1.Submitted, o2.Submitted) * -1;
            });
            orderHistoryListView.ItemsSource = null;
            orderHistoryListView.ItemsSource = orders;
            loadingOrderHistoryIndicator.IsRunning = false;
            base.OnAppearing();
        }

        private async void orderSelected(object sender, ItemTappedEventArgs e)
        {
            var order = e.Item as Models.Order;

            // If the user is not an employee or if they are an employee viewing their personal orders
            if(!isEmployee || (isEmployee && employeeOrderFilterPicker.SelectedIndex == 0) || employeeOrderFilterPicker.SelectedIndex == -1)
            {
                // Get all unique names of menu items in this order
                List<Models.OrderItem> uniqueOrderItems = order.OrderItems.GroupBy(i => i.MenuItem.Name).Select(grp => grp.FirstOrDefault()).ToList();

                // Get the count of each unique menu item
                List<int> uniqueOrderItemsCount = new List<int>();
                foreach (Models.OrderItem item in uniqueOrderItems)
                {
                    uniqueOrderItemsCount.Add(order.OrderItems.Where(i => i.MenuItem.Name == item.MenuItem.Name).Count());
                }

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < uniqueOrderItems.Count; i++)
                {
                    builder.AppendLine(uniqueOrderItems[i].MenuItem.Name + ": " + uniqueOrderItemsCount[i]);
                }

                await DisplayAlert("Order Info", builder.ToString(), "OK");
                builder.Clear();
            }
            else if(isEmployee && employeeOrderFilterPicker.SelectedIndex == 1)
            {
                RestService service = new RestService();
                await service.Initialize();
                string selectedAction = await DisplayActionSheet("Change Order Status", "Cancel", null, "Received Order", "Preparing Order", "Order Done", "Picked Up");
                int selectedIndex = -1;
                switch (selectedAction)
                {
                    case "Received Order":
                        selectedIndex = 0;
                        break;
                    case "Preparing Order":
                        selectedIndex = 1;
                        break;
                    case "Order Done":
                        selectedIndex = 2;
                        break;
                    case "Picked Up":
                        selectedIndex = 3;
                        break;
                    default:
                        selectedIndex = -1;
                        break;
                }
                if(selectedIndex != -1)
                {
                    await service.UpdateOrderStatus(order.ID, selectedIndex);
                    refreshOrders(null, null);
                }
            }
        }

        private async void refreshOrders(object sender, EventArgs e)
        {
            RestService service = new RestService();
            await service.Initialize();
            // Display employee's personal orders
            if (employeeOrderFilterPicker.SelectedIndex == 0 || employeeOrderFilterPicker.SelectedIndex == -1)
            {
                loadingOrderHistoryIndicator.IsRunning = true;
                var email = await SecureStorage.GetAsync("email");
                var user = await service.GetUserOrders(email);
                var orders = user.Orders;
                orders.Sort(delegate (Models.Order o1, Models.Order o2)
                {
                    return DateTime.Compare(o1.Submitted, o2.Submitted) * -1;
                });
                orderHistoryListView.ItemsSource = null;
                orderHistoryListView.ItemsSource = orders;
                loadingOrderHistoryIndicator.IsRunning = false;
            }
            // Display all orders
            else if (employeeOrderFilterPicker.SelectedIndex == 1)
            {
                loadingOrderHistoryIndicator.IsRunning = true;
                var allOrders = await service.GetOrders();
                allOrders.Sort(delegate (Models.Order o1, Models.Order o2)
                {
                    return DateTime.Compare(o1.Submitted, o2.Submitted) * -1;
                });
                orderHistoryListView.ItemsSource = null;
                orderHistoryListView.ItemsSource = allOrders;
                loadingOrderHistoryIndicator.IsRunning = false;
            }
            orderHistoryListView.IsRefreshing = false;
        }

        private async void changeEmployeeOrderFilter(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if(selectedIndex != -1)
            {
                RestService service = new RestService();
                await service.Initialize();
                // Display employee's personal orders
                if (selectedIndex == 0)
                {
                    loadingOrderHistoryIndicator.IsRunning = true;
                    var email = await SecureStorage.GetAsync("email");
                    var user = await service.GetUserOrders(email);
                    var orders = user.Orders;
                    orders.Sort(delegate (Models.Order o1, Models.Order o2)
                    {
                        return DateTime.Compare(o1.Submitted, o2.Submitted) * -1;
                    });
                    orderHistoryListView.ItemsSource = null;
                    orderHistoryListView.ItemsSource = orders;
                    loadingOrderHistoryIndicator.IsRunning = false;
                }
                // Display all orders
                else if (selectedIndex == 1)
                {
                    loadingOrderHistoryIndicator.IsRunning = true;
                    var allOrders = await service.GetOrders();
                    allOrders.Sort(delegate (Models.Order o1, Models.Order o2)
                    {
                        return DateTime.Compare(o1.Submitted, o2.Submitted) * -1;
                    });
                    orderHistoryListView.ItemsSource = null;
                    orderHistoryListView.ItemsSource = allOrders;
                    loadingOrderHistoryIndicator.IsRunning = false;
                }
            }
        }
    }
}