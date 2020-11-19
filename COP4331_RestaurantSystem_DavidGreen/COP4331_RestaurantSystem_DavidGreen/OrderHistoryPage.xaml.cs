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
                //loadingMenuIndicator.IsRunning = true;
                RestService service = new RestService();
                await service.Initialize();
                var email = await SecureStorage.GetAsync("email");
                var orders = await service.GetUserOrders(email);
                orderHistoryListView.ItemsSource = null;
                orderHistoryListView.ItemsSource = orders;
                //loadingMenuIndicator.IsRunning = false;
            }
        }

        private void orderHistoryListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}