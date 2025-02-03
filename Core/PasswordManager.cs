using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password_Manager.Database;
using Paswword_Manager.Core;

namespace Password_Manager.Core
{
    public class PasswordManager : IPasswordManager
    {
        private readonly IDatabaseService _databaseService;
        private readonly IAuthenticationService  _authenticationService;
        private string _masterEmail,_masterPassword;
        public PasswordManager (IDatabaseService idatabaseService,IAuthenticationService iauthenticationSrevice)
        {
            _databaseService = idatabaseService;
            _authenticationService=iauthenticationSrevice;
        }
        public void AddNewPassword(string siteName,string email,string password)
        {
            
        }

        public void DeletePassword(string siteName)
        {
        }

        public bool LogIn(string email,string password)
        {
            if(string.IsNullOrEmpty(_masterEmail))
                return false;
            if(!_authenticationService.VerifyPassword(password,_masterPassword))
                return false;

            return true;
        }

        public void ShowAllPasswords()
        {
            throw new NotImplementedException();
        }

  

        public bool SignUp(string masterEmail,string masterPassword)
        {
            masterPassword=_authenticationService.HashPassword(masterPassword);
            if(!string.IsNullOrEmpty(_masterEmail))
            {
                return false;
            }

            _masterEmail=masterEmail;
            _masterPassword=masterPassword;
            return true;
            
        }

        public void UpdatePassword(string siteName,string newPassword)
        {
        }

        // Add option of suggest strong passwords 
    }
}
