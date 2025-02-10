using Password_Manager.Models;
using System.Collections.Generic;

namespace Password_Manager.Database
{
    public interface IDatabaseService
    {
       string ConnectionString { set; }

        void InitializeDatabase (string userEmail);  
        void RegisterUser(string userEmail, string masterHashedPassword);
        void AddPassword(string siteName, string email, string encryptedPassword);
        bool VerifyUserPassword(string userEmail, string enteredHashedPassword) ;
        bool SiteExist(string siteName);
        bool DoesUserExist(string masterEmail);
        bool UpdatePassword(string siteName, string newEncryptedPassword);
        bool DeletePassword(string siteName);
        List<PasswordEntry> GetAllPasswords();
  
    }
}
