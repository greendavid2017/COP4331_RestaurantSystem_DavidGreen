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
    }
}