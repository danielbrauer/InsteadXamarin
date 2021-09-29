using System.Threading.Tasks;
using Instead.Services;
using Microsoft.IdentityModel.Tokens;

namespace Instead.Models
{
    public class LocalUser: User
    {
        Credentials Credentials { get; set; }

        Session Session { get; set; }

        public JsonWebKey PrivateKey { get; protected set; }

        public static LocalUser Current { get; protected set; }

        public static async Task Login(Credentials credentials)
        {
            var user = new LocalUser {
                Credentials = credentials,
                Session = new Session("http://10.0.2.2:3001")
            };

            var keyPair = await user.Session.Login(user.Credentials);

            user.PrivateKey = keyPair.Private;
            user.PublicKey = keyPair.Public;

            Current = user;
        }

        public async Task LogOut()
        {
            await Session.Logout();
            Session = null;
        }
    }
}
