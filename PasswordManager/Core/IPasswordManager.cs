using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password_Manager.Models;


namespace Password_Manager.Core
{
    public interface IPasswordManager
    {
         bool SignUp(string masterEmail,string masterPassword);
         bool LogIn(string email,string password);
         void AddNewPassword(string siteName,string email,string password); 
         bool SiteExist(string siteName); 
         void DeletePassword(string siteName); 
         void UpdatePassword(string siteName,string newPassword); 
         List<PasswordEntry> ShowAllPasswords(); 

    }
}
