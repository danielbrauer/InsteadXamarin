using Xamarin.Forms;
using Instead.ViewModels;

namespace Instead.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        LoginViewModel VM => BindingContext as LoginViewModel;

        async void LogIn(System.Object sender, System.EventArgs e)
        {
            VM.ErrorMessage = null;
            var success = await VM.Login(username.Text, password.Text, secretKey.Text);
            if (success)
            {
                await Navigation.PushAsync(new TabRoot());
                Navigation.RemovePage(this);
            }
        }
    }
}
