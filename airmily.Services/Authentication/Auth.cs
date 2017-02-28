using airmily.Services.Models;

namespace airmily.Services.Auth
{
    public class Auth : IAuth
    {
        public User CurrentUser { get; set; }
    }
}
