using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.UI
{
    internal class InputValidator : IInputValidator
    {
        public bool ValidateMasterEmail(string masterEmail)
        {
            if( string.IsNullOrEmpty(masterEmail) || masterEmail.Length<3 )
            {
         
                return false;

            }
            return true;

        }

        public bool ValidateStoredPassword(string masterPassword,string confirmationPassword)
        {
            if (masterPassword != confirmationPassword)
            {
  
                return false;
            }
           
            return true;
        }

        }
    }
