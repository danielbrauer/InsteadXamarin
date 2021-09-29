using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Instead.Views;

namespace Instead
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var loginPage = new NavigationPage(new LoginPage());
            // Doesn't remove the navigation bar
            NavigationPage.SetHasNavigationBar(loginPage, false);
            MainPage = loginPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
