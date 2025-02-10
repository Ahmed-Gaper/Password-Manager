using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.Core
{
    public interface IAuthenticationService
    {
        string HashPassword(string masterPassword);

        bool VerifyPassword(string enterdPassword,string storedPassword);

    }
}
