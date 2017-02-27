using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using airmily.Services.Models;

namespace airmily.Interfaces
{
    public interface IAuth
    {
        User GetCurrentUser();
        void setCurrentUser(User setUser);
    }
}
