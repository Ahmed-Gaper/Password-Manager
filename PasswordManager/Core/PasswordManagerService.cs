using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password_Manager.Database;
using Password_Manager.Core;
using Password_Manager.Models;
using PasswordManager.Encryption;

namespace Password_Manager.Core
{
    public class PasswordManagerService : IPasswordManager
    {
        private readonly IDatabaseService _databaseService;
        private readonly IAuthenticationService  _authenticationService;
        private readonly  IEncryptionService    _encryptionService;
        private string _masterEmail,_masterPassword;
        public PasswordManagerService (IDatabaseService idatabaseService,IAuthenticationService iauthenticationSrevice,IEncryptionService iencryptionService)
        {
            _databaseService = idatabaseService;
            _authenticationService=iauthenticationSrevice;
            _encryptionService = iencryptionService;
        }
        public void AddNewPassword(string siteName,string email,string password)
        {
            _databaseService.AddPassword(siteName,email,_encryptionService.Encrypt(password,"515"));

            
        }

        public bool SiteExist(string siteName)
        {
            if(_databaseService.SiteExist(siteName))
                return true;
            return false;
        }

        public void DeletePassword(string siteName)
        {
            _databaseService.DeletePassword(siteName);

        }

        public bool LogIn(string masterEmail,string masterPassword)
        {
            masterPassword=_authenticationService.HashPassword(masterPassword);

            if(!_databaseService.DoesUserExist(masterEmail))
                return false;

            _databaseService.ConnectionString=masterEmail;

             if(!_databaseService.VerifyUserPassword(masterEmail,masterPassword))
                return false;
            return true;

   
        }

        public List<PasswordEntry> ShowAllPasswords()
        {
            List<PasswordEntry> passwordList = _databaseService.GetAllPasswords();
    
            foreach (var password in passwordList)
            {
                password.Password = _encryptionService.Decrypt(password.Password,"515");
            }

                return passwordList;
        }


  

        public bool SignUp(string masterEmail,string masterPassword)
        {
            masterPassword=_authenticationService.HashPassword(masterPassword);
            if(_databaseService.DoesUserExist(masterEmail))
                return false;
            _databaseService.InitializeDatabase(masterEmail);
            _databaseService.RegisterUser(masterEmail,masterPassword);
            return true;
        }

        public void UpdatePassword(string siteName,string newPassword)
        {
            _databaseService.UpdatePassword(siteName,newPassword);
        }

        // Add option of suggest strong passwords 
    }
}
