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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        private async void loginButton_Clicked(object sender, EventArgs e)
        {
            RestService service = new RestService();
            service.InitializeLogin();

            var token = await service.Login(emailEntry.Text, passwordEntry.Text);
            if(token != null)
            {
                try
                {
                    await SecureStorage.SetAsync("apiToken", token);
                    await SecureStorage.SetAsync("email", emailEntry.Text);
                    await Shell.Current.GoToAsync("//menu");
                }
                catch(Exception ex)
                {
                    // If device does not support secure storage may get an exception
                }

                //await DisplayAlert("Hooray!", "Hooray!", "Hooray!");
            }
            else
            {
                await DisplayAlert("Login Failed", "Your username or password is incorrect. Please try again.", "OK");
            }
        }

        private async void registerButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("register");
        }
    }
}