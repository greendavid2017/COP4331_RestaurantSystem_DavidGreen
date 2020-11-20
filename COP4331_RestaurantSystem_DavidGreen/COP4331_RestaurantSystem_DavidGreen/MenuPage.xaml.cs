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
    public partial class MenuPage : ContentPage
    {

        int onAppearingCounter = 0;

        private System.Collections.IEnumerable items;

        private List<Tuple<Models.MenuItem, int>> orderItems;

        public MenuPage()
        {
            this.orderItems = new List<Tuple<Models.MenuItem, int>>();
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            categoryPicker.SelectedIndex = 0;

            // There is a bug in Xamarin forms where OnAppearing runs twice for no reason, so use this counter to only run 1/2 of the time
            onAppearingCounter++;
            if(onAppearingCounter % 2 == 1)
            {
                loadingMenuIndicator.IsRunning = true;
                RestService service = new RestService();
                await service.Initialize();
                var menuItems = await service.GetMenuItems();
                items = menuItems;
                menuListView.ItemsSource = items;
                loadingMenuIndicator.IsRunning = false;
            }
        }

        private async void menuListView_Refreshing(object sender, EventArgs e)
        {
            RestService service = new RestService();
            await service.Initialize();
            var menuItems = await service.GetMenuItems();
            items = menuItems;
            categoryPicker.SelectedIndex = 0;
            menuListView.ItemsSource = items;
            menuListView.IsRefreshing = false;
        }

        private void menuSearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            categoryPicker.SelectedIndex = 0;
            var menuItemsList = items.Cast<Models.MenuItem>().ToList().Where(i => i.Name.ToLowerInvariant().Contains(menuSearchBar.Text.ToLowerInvariant()));
            menuListView.ItemsSource = menuItemsList;
        }

        private void menuSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(e.NewTextValue == String.Empty || e.NewTextValue == null)
            {
                menuListView_Refreshing(sender, e);
            }
        }

        private void categoryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(categoryPicker.SelectedIndex > 0 && items != null && items.Cast<Models.MenuItem>().ToList().Count() > 0)
            {
                try
                {
                    var menuItemsList = items.Cast<Models.MenuItem>().ToList().Where(i => i.Category == categoryPicker.SelectedIndex - 1);
                    menuListView.ItemsSource = menuItemsList;
                }
                catch(Exception ex)
                {

                }
            }
            else if(categoryPicker.SelectedIndex == 0 && items != null && items.Cast<Models.MenuItem>().ToList().Count() > 0)
            {
                menuListView.ItemsSource = items;
            }
            
        }

        private async void menuListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as Models.MenuItem;
            await Navigation.PushModalAsync(new AddToCartPage(ref this.orderItems, selectedItem));
            //await Shell.Current.GoToAsync($"addtocart?id={selectedItem.ID}&name={selectedItem.Name}&price={selectedItem.Price.ToString("F")}&category={selectedItem.Category.ToString()}");
            //var accepted = await DisplayAlert("Confirm Selection", selectedItem.Name + "\n$" + selectedItem.Price.ToString("F"), "Add", "Cancel");
        }

        private async void viewOrderButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PlaceOrderPage(ref this.orderItems));
        }

        private void logOutButton_Clicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("email");
            SecureStorage.Remove("apiToken");
        }
    }
}