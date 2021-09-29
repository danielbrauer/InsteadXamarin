using System;
using System.Threading.Tasks;
using Instead.Models;
using Instead.Services;

namespace Instead.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
        }

        internal async Task LogOut()
        {
            await LocalUser.Current.LogOut();
        }
    }
}
