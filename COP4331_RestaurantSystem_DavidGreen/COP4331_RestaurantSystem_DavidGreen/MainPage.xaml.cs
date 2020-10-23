using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace COP4331_RestaurantSystem_DavidGreen
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void TestButton_Clicked(object sender, EventArgs e)
        {
            RestService service = new RestService();
            var items = await service.GetMenuItems();

        }
    }
}
