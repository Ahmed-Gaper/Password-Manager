using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.UI
{
    public interface IInputValidator
    {
        bool ValidateMasterEmail(string masterEmail);
        bool ValidateMaterPassword(string masterPassword,string confirmationPassword);
        bool ValidateStoredPassword(string masterPassword,string confirmationPassword);
    }
}
