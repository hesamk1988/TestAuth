using Microsoft.AspNetCore.Identity;

namespace TestAuth.Models
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public string Token { get; set; }


        public AuthenticationResponse(IdentityUser user, string token)
        {
            Id = user.Id;
            Username = user.UserName;
            Token = token;
        }
    }
}
