using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using airmily.Services.Models;


namespace airmily.Interfaces
{
    public class Auth : IAuth
    {
        static private User _currentUser;

        public User getCurrentUser()
        {
            return _currentUser;
        }

        public void setCurrentUser(User setUser)
        {
            _currentUser = setUser;
        }
    }
}
