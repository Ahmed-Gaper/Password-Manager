using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.UI
{
    public interface IUserInterface
    {
         void Welcome ();
         void ShuttingDown();

         void StartTheProgram();
         void SignUp();
         void LogIn();
         void GeneratePassword();
         void ShowMainMenu();
         void AddNewPassword(); 
         void DeletePassword(); 
         void UpdatePassword(); 
         void ShowAllPasswords(); 
        void UnactiveCheck();
      

        }
    }
