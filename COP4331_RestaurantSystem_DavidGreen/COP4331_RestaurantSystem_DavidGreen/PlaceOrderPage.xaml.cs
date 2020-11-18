using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COP4331_RestaurantSystem_DavidGreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceOrderPage : ContentPage
    {
        private List<Tuple<Models.MenuItem, int>> orderItems;

        public PlaceOrderPage(ref List<Tuple<Models.MenuItem, int>> orderItems)
        {
            this.orderItems = orderItems;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            cartListView.ItemsSource = orderItems;
            totalPriceLabel.Text = calculateTotalPrice().ToString("$0.00");
        }

        private void testClearButton_Clicked(object sender, EventArgs e)
        {
            this.orderItems.Clear();
        }

        private decimal calculateTotalPrice()
        {
            decimal totalPrice = 0.00M;
            foreach(Tuple<Models.MenuItem, int> item in this.orderItems)
            {
                totalPrice += item.Item1.Price * item.Item2;
            }

            totalPrice *= (decimal)1.06;

            return totalPrice;
        }

        // Called when the user taps an item in their cart
        private async void orderItemSelected(object sender, ItemTappedEventArgs e)
        {
            // Confirm if the user would like to delete the item from their cart
            var item = e.Item as Tuple<Models.MenuItem, int>;
            bool deleteItem = await DisplayAlert("Delete Item", "Are you sure you would like to delete " + item.Item1.Name + " from your cart?", "Delete", "Cancel");
            if(deleteItem)
            {
                orderItems.Remove(item);
                cartListView.ItemsSource = null;
                cartListView.ItemsSource = orderItems;
                totalPriceLabel.Text = calculateTotalPrice().ToString("$0.00");
            }
        }

        // Called when the user clicks the Place Order button
        private void placeOrder(object sender, EventArgs e)
        {
            // Place an order here
        }
    }
}