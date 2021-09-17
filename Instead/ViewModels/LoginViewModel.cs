using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Instead.ViewModels
{
    public class LoginViewModel: BaseViewModel
    {

        bool _working;

        public bool Working
        {
            get => _working;
            set
            {
                _working = value;
                OnPropertyChanged();
            }
        }

        public async Task Login(string username, string password, string secretKey)
        {
            Working = true;
            await Services.Client.Login(username, password, secretKey);
            Working = false;
        }
    }
}
