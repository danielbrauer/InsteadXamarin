using RestEase;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Instead
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
        public string publicKey;
        public string mukSalt;
    }

    public interface AuthService
    {

        [Post("auth/startLogin")]
        Task<StartLoginResult> startLogin([Body] StartLoginInput input);

        [Post("auth/finishLogin")]
        Task<FinishLoginResult> finishLogin([Body] FinishLoginInput input);
    }

}
