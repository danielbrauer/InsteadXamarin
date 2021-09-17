using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Instead.ViewModels;

namespace Instead
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        LoginViewModel VM => BindingContext as LoginViewModel;

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await VM.Login(username.Text, password.Text, secretKey.Text);
        }
    }
}
