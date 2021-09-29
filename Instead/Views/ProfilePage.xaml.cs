using System;
using System.Collections.Generic;
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

        void LogOut(System.Object sender, System.EventArgs e)
        {
            VM.LogOut();
        }
    }
}
