using airmily.Services.Models;

namespace airmily.Services.Auth
{
    public interface IAuth
    {
        User CurrentUser { get; set; }
    }
}
