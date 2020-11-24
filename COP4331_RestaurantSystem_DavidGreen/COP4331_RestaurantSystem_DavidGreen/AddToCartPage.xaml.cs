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
    [QueryProperty("ID", "id")]
    [QueryProperty("Name", "name")]
    [QueryProperty("Category", "category")]
    [QueryProperty("Price", "price")]
    public partial class AddToCartPage : ContentPage
    {
        private Models.MenuItem item;

        private List<Tuple<Models.MenuItem, int>> orderItems;

        public AddToCartPage(ref List<Tuple<Models.MenuItem, int>> orderItems, Models.MenuItem item)
        {
            this.orderItems = orderItems;
            this.item = item;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            /*item = new Models.MenuItem(){
                ID = Int32.Parse(ID),
                Name = Name,
                Category = Int32.Parse(Category),
                Price = Decimal.Parse(Price)
            };*/

            itemNameLabel.Text = item.Name;
            itemPriceLabel.Text = item.Price.ToString("F");
        }

        private async void back(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void addToCart(object sender, EventArgs e)
        {
            // Add item to cart here
            orderItems.Add(new Tuple<Models.MenuItem, int>(item, Convert.ToInt32(amountStepper.Value)));


            await Shell.Current.GoToAsync("..");
        }
    }
}