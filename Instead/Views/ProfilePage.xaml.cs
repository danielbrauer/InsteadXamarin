using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Instead.ViewModels;
using Xamarin.Forms;

namespace Instead.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel();
        }

        ProfileViewModel VM => BindingContext as ProfileViewModel;

        async void LogOut(System.Object sender, System.EventArgs e)
        {
            var wantsLogout = await DisplayAlert(
                "Log Out",
                "Are you sure you want to log out?",
                "Log out",
                "Cancel"
                );
            if (!wantsLogout)
                return;
            await VM.LogOut();
            Navigation.InsertPageBefore(new LoginPage(), Navigation.NavigationStack.Last());
            await Navigation.PopAsync();
        }
    }
}
