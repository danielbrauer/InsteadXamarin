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

        string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> Login(string username, string password, string secretKey)
        {
            Working = true;
            try
            {
                await Services.Client.Login(username, password, secretKey);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
            finally
            {
                Working = false;
            }
            return true;
        }
    }
}
