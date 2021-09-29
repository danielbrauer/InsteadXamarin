using System;
using Instead.Services;
using Microsoft.IdentityModel.Tokens;

namespace Instead.Models
{
    public class User
    {
        public int Id { get; protected set; }
        public JsonWebKey PublicKey { get; protected set; }
        public string DisplayName { get; protected set; }
    }
}
