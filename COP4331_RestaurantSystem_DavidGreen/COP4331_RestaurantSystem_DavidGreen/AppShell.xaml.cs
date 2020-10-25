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
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("register", typeof(RegisterPage));
            Routing.RegisterRoute("addtocart", typeof(AddToCartPage));
        }

    }
}