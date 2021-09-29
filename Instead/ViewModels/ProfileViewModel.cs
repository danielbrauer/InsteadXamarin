using System;
using Instead.Models;
using Instead.Services;

namespace Instead.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
        }

        internal async void LogOut()
        {
            await LocalUser.Current.LogOut();
        }
    }
}
