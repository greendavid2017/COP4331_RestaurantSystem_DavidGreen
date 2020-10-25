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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

        private async void registerButton_Clicked(object sender, EventArgs e)
        {
            RestService service = new RestService();
            service.InitializeLogin();

            var token = await service.Register(emailEntry.Text, passwordEntry.Text);
            if (token != null)
            {
                try
                {
                    await SecureStorage.SetAsync("apiToken", token);
                    await SecureStorage.SetAsync("email", emailEntry.Text);
                    await Shell.Current.GoToAsync("//menu");
                }
                catch (Exception ex)
                {
                    // If device does not support secure storage may get an exception
                    await DisplayAlert("Device Incompatible", "Your device does not support Secure Storage and is therefore not compatible with this application.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Registration Failed", "A user with this email already exists. Please login instead.", "OK");
            }
        }

        private async void cancelButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}