using RestEase;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Instead.Services
{

    public struct StartLoginInput
    {
        public string username;
        public string clientEphemeralPublic;
    }

    public struct StartLoginResult
    {
        public string srpSalt;
        public string serverEphemeralPublic;
    }

    public struct FinishLoginInput
    {
        public string clientSessionProof;
    }

    public struct FinishLoginResult
    {
        public int userId;
        public string serverSessionProof;
        public string privateKey;
        public string privateKeyIv;
        public JsonWebKey publicKey;
        public string mukSalt;
    }

    public interface IInsteadApi
    {

        [Post("auth/startLogin")]
        Task<StartLoginResult> StartLogin([Body] StartLoginInput input);

        [Post("auth/finishLogin")]
        Task<FinishLoginResult> FinishLogin([Body] FinishLoginInput input);

        [Put("api/logout")]
        Task Logout();
    }

}
