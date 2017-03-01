using airmily.Services.Models;

namespace airmily.Services.Auth
{
    public class Auth : IAuth
    {
	    private static User _currentUser;
	    public User CurrentUser
	    {
			get { return _currentUser; }
		    set
		    {
			    if (_currentUser != value)
					_currentUser = value;
		    }
	    }
    }
}
