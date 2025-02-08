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
                Console.ForegroundColor=ConsoleColor.Red;
                Console.WriteLine($"Wong Email format !");
                Console.ResetColor();
                Console.WriteLine("Press enter to try again....");
                Console.ReadLine();
                Console.Clear();
                return false;

            }
            return true;

        }

        public bool ValidateMaterPassword(string masterPassword,string confirmationPassword)
        {
            if (masterPassword != confirmationPassword)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: Passwords do not match. Please try again ");
                Console.ResetColor();
                Console.ReadLine();
                Console.Clear();
                return false;
            }
            
            else if(string.IsNullOrEmpty(masterPassword) || masterPassword.Length<8)
            {
                Console.ForegroundColor=ConsoleColor.Red;
                Console.WriteLine($"Password is to short !");
                Console.ResetColor();
                Console.WriteLine("Press enter to try again....");
                Console.ReadLine();
                Console.Clear();
                return false;
            }

 
            return true;
        }
        public bool ValidateStoredPassword(string masterPassword,string confirmationPassword)
        {
            if (masterPassword != confirmationPassword)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: Passwords do not match.");
                Console.ResetColor();
                Console.ReadLine();
                return false;
            }
           
            return true;
        }
        }
    }
