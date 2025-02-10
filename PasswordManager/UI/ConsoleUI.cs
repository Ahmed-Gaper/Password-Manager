using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Password_Manager.Core;
using Password_Manager.Models;

namespace Password_Manager.UI
{
    public class ConsoleUI : IUserInterface
    {
        private readonly IPasswordManager _passwordManager;
        private readonly IInputValidator  _inputValidator;
        public ConsoleUI (IPasswordManager ipasswordManager,IInputValidator iinputValidator)
        {
            _passwordManager=ipasswordManager;
            _inputValidator=iinputValidator;
        }
        public void StartTheProgram()
        {
            bool runnig=true,wrongChoice=false;
            string choice;

            while(runnig)
            {

                if(!wrongChoice)
                {
                Console.Clear();
                Console.WriteLine("[1] Sign up");
                Console.WriteLine("[2] Log in");
                Console.WriteLine("[3] Exit the App");
                }

                Console.ForegroundColor=ConsoleColor.Green;
                Console.Write("\nEnter your choice : ");
                Console.ResetColor();

                    

                choice=Console.ReadLine();

                switch (choice)
                {
                    case "1" : 

                    SignUp();

                    break;
                     
                    case "2":
                    LogIn();
                    break;

                    case "3" :
                    runnig=false;
                        return;
                    

                    default:

                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine("WRONG CHOICE !  Enter valid number");
                    Console.ResetColor();
                    wrongChoice=true;

                    break;

                  
               }
            }

        }
        public void Welcome()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;

            string message = "W E L C O M E   T O   Y O U R   P A S S W O R D   M A N A G E R";
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            int leftPadding = (consoleWidth - message.Length) / 2;
            int topPadding = consoleHeight / 2;

            Console.SetCursorPosition(leftPadding, topPadding);
            Console.WriteLine(message);

            Console.ResetColor();
            Console.WriteLine("Press any kay to continue..........");
            Console.ReadLine();
        }
        public void ShuttingDown()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Your Password Manager shutting down...........");
            Console.ResetColor();
            Console.ReadKey();
            Environment.Exit(0);
        }

        public void SignUp()
        {
            Console.Clear();
            string masterEmail,masterPassword,masterPasswordConfirmation;

            bool vaildInput=false;
            do
            {
            ConsoleHelper.PrintCenteredBanner("SinUp");

            Console.Write("Enter you master Email : ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(Minmum length is 3)");
            Console.ResetColor();
            masterEmail=Console.ReadLine();

            Console.WriteLine();

            Console.Write("Enter you master Password : ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(Minmum length is 8)");
            Console.ResetColor();
            masterPassword = Console.ReadLine();

            Console.WriteLine();

            Console.Write("Rewrite your master Password : ");
            masterPasswordConfirmation = Console.ReadLine();

            Console.WriteLine();    
                if(_inputValidator.ValidateMasterEmail(masterEmail) && _inputValidator.ValidateStoredPassword(masterPassword,masterPasswordConfirmation))
                {  
                 vaildInput=true;
                }
                else
                {
                    ConsoleHelper.PrintErrorMessage("Wrong email format");
                    if(ConsoleHelper.TakeValidChoice("Try again","Back to startup menu")==2)
                    {
                        StartTheProgram();
                        return;
                    }


                }

            }while(!vaildInput);

            bool successfulSignUp=_passwordManager.SignUp(masterEmail,masterPassword);

            if(successfulSignUp)
            {
                Console.ForegroundColor=ConsoleColor.Green;
                Console.WriteLine("SignUp Successful.........!");
                Console.ResetColor();

            }
            else
            {
                Console.ForegroundColor=ConsoleColor.Red;
                Console.WriteLine("You alredy have an account.........!");
                Console.ResetColor();
            }
                Console.WriteLine("Press any key to LogIn.....!");
                Console.ReadLine();
                LogIn();

        }

        public void LogIn()
        {
            int attempts=0;
            int maxAttempts=3;
            while(attempts < maxAttempts)
            {
             attempts++;
            Console.Clear();
            ConsoleHelper.PrintCenteredBanner("LogIn");

            string _email,_password;

            Console.Write("Mail : ");
            _email=Console.ReadLine();

            Console.WriteLine();

            Console.Write("Password : ");
            _password=Console.ReadLine();
            Console.WriteLine();

            bool _isAuthenticated =_passwordManager.LogIn(_email,_password);
            if(_isAuthenticated)
            {
                Console.ForegroundColor=ConsoleColor.Green;
                Console.WriteLine("Login Successful.........!");
                Console.ResetColor();
                Console.WriteLine("Press any key to Continue.....!");
                    Console.ReadKey();
                
                ShowMainMenu();
            }
            else
            {

               bool wrongChoice=false; 
               bool running=true;

               while(running)
               {
                    if(maxAttempts-attempts==0)
                            break;
                    if(!wrongChoice)
                    {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine($"\nWrong Email or Password , {maxAttempts-attempts} Attempts remaining !");
                    Console.ResetColor();

                    Console.WriteLine("\nWhat would you like to do?");
                    Console.WriteLine("[1] Retry Login");
                    Console.WriteLine("[2] Go Back to Sign Up");

                
                    }
                    string choice = Console.ReadLine();
                    Console.WriteLine();
                
                    switch (choice)
                    {
                        case "1":
                                running=false;
                            break;
                        case "2":
                            SignUp();
                            return; 
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                                wrongChoice=true;
                            break;

                    }
               }
            }

            }
           //you can upgrade it to lock the account
            Console.ForegroundColor=ConsoleColor.Red;
            Console.WriteLine($"\nMaximum login attempts reached. Please try again later.");
            Console.ResetColor();
            Console.ReadLine();
            ShuttingDown();

           
        }

        public void ShowMainMenu()
        {
            Console.Clear();
            ConsoleHelper.PrintCenteredBanner("Main Menu");
            bool runnig=true,wrongChoice=false;
            string choice;

            while(runnig)
            {

                if(!wrongChoice)
                {
                Console.WriteLine("[1] Generate password");
                Console.WriteLine("[2] Add new password");
                Console.WriteLine("[3] Delete password");
                Console.WriteLine("[4] Update password");
                Console.WriteLine("[5] Show all your passwords");
                Console.WriteLine("[6] Close your password manager");
                }

                Console.ForegroundColor=ConsoleColor.Green;
                Console.Write("\nEnter your choice : ");
                Console.ResetColor();

                    

                choice=Console.ReadLine();

                switch (choice)
                {
                    case "1" : 
                    GeneratePassword();
                    break;
                     
                    case "2":
                    AddNewPassword();
                    break;

                    case "3":
                    DeletePassword();
                    break;

                    case "4":
                    UpdatePassword();
                    break;

                    case "5":
                    ShowAllPasswords();
                    break;

                    case "6" :
                    ShuttingDown();
                        return;
                    

                    default:

                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine("WRONG CHOICE !  Enter valid number (1,2 or 3)");
                    Console.ResetColor();
                    wrongChoice=true;

                    break;

                  
               }
            }            
        }

        //To do
   
    public  void GeneratePassword()
    {
           const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
           const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
           const string Digits = "0123456789";
           const string SpecialChars = "!@#$%^&*()-_=+<>?";
           int length;
            Console.ForegroundColor=ConsoleColor.Green;
            Console.Write("Enter the required length : ");
            length=int.Parse(Console.ReadLine());
            Console.ResetColor();

        string allChars = Lowercase + Uppercase + Digits + SpecialChars;
        char[] password = new char[length];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[length];

            rng.GetBytes(randomBytes);

            for (int i = 0; i < length; i++)
            {
                int index = randomBytes[i] % allChars.Length; 
                password[i] = allChars[index];
            }
        }
            Console.ForegroundColor=ConsoleColor.Green;
            Console.WriteLine(new string (password));
            Console.ResetColor();
            Console.WriteLine("Press any key to show Main menu.....!");
            Console.ReadLine();
            ShowMainMenu();

    }
        public void AddNewPassword()
        {



            bool vaildInput=false;
            string siteName,email,password,confirmationPassword;

            do
            {
            ConsoleHelper.PrintSidebarTitle("Add New Password ");
            Console.Write("Site name : ");
            siteName=Console.ReadLine();

            Console.Write("Email : ");
            email=Console.ReadLine();

            Console.Write("Password : ");
            password=Console.ReadLine();

            Console.Write("Password Confirmation : ");
            confirmationPassword=Console.ReadLine();


                if (_inputValidator.ValidateStoredPassword(password, confirmationPassword))
                {
                    vaildInput = true;

                }
                else
                {
                    ConsoleHelper.PrintErrorMessage("Passwords do not match. Please try again ");
                  if(ConsoleHelper.TakeValidChoice("Retry Add Password","Go Back to Main menu")==2)
                  {
                    ShowMainMenu();
                    return;
                  }
                }

            } while (!vaildInput);

             
            if(_passwordManager.SiteExist(siteName))
            {
               Console.ForegroundColor=ConsoleColor.Red;
               Console.WriteLine("Site is already exist");
               Console.ResetColor();

                if(ConsoleHelper.TakeValidChoice("Update Site","Go Back to Main menu")==1)
                {
                    UpdatePassword();
                    return;
                }
                else
                {
                    ShowMainMenu();
                    return;
                }

            }
            else
            {
             _passwordManager.AddNewPassword(siteName,email,password);

            Console.ForegroundColor=ConsoleColor.Green;
            Console.WriteLine("Added Successfully.........!");
            Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to show Main menu.....!");
            Console.ReadLine();
            ShowMainMenu();
        }

        public void DeletePassword()
        {
            bool vaildInput=false;
            string siteName;

            ConsoleHelper.PrintSidebarTitle("Delete Password ");
            Console.Write("Site name : ");
            siteName=Console.ReadLine();

            if(!_passwordManager.SiteExist(siteName))
            {
                Console.ForegroundColor=ConsoleColor.Red;
                Console.WriteLine("Site isn't exist");
                Console.ResetColor();
            }
            else
            {

                bool wrongChoice=false; 
                bool running=true;

                while(running)
                {
                       
                    if(!wrongChoice)
                    {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine($"\nAre you Sure you want to delete {siteName} from your Password Manager ?");
                    Console.ResetColor();

                    Console.WriteLine("[1] Confirm ");
                    Console.WriteLine("[2] Cancel");
                
                    }
                    string choice = Console.ReadLine();
                    Console.WriteLine();
                
                    switch (choice)
                    {
                        case "1":
                                running=false;
                            break;
                        case "2":
                            ShowMainMenu();
                            return; 
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                                wrongChoice=true;
                            break;

                    }
                }

                _passwordManager.DeletePassword(siteName);

                Console.ForegroundColor=ConsoleColor.Green;
                Console.WriteLine("Deleted Successfully.........!");
                Console.ResetColor();

            }

            Console.WriteLine("Press any key to show Main menu.....!");
            Console.ReadLine();
            ShowMainMenu();
        }

        public void UpdatePassword()
        {
            bool vaildInput=false;
            string siteName,newpassword,confirmationPassword;

            do
            {
            ConsoleHelper.PrintSidebarTitle("Update Password ");
            Console.Write("Site name : ");
            siteName=Console.ReadLine();

            Console.Write("New Password : ");
            newpassword=Console.ReadLine();

            Console.Write("Password Confirmation : ");
            confirmationPassword=Console.ReadLine();


                if (_inputValidator.ValidateStoredPassword(newpassword, confirmationPassword))
                {
                    vaildInput = true;

                }
                else
                {
                    ConsoleHelper.PrintErrorMessage("Passwords do not match. Please try again");
                    if(ConsoleHelper.TakeValidChoice("Retry Update Password","Go Back to Main menu")==2)
                    {
                        ShowMainMenu();
                    }
            
                }

            } while (!vaildInput);

            if(_passwordManager.SiteExist(siteName))
            {
              _passwordManager.UpdatePassword(siteName,newpassword);

                Console.ForegroundColor=ConsoleColor.Green;
                Console.WriteLine("Updated Successfully.........!");
                Console.ResetColor();
            }
            else
            { 
                Console.ForegroundColor=ConsoleColor.Red;
                Console.WriteLine("Site isn't exist");
                Console.ResetColor();
   

            }
            Console.WriteLine();
            Console.WriteLine("Press any key to show Main menu.....!");
            Console.ReadLine();
            ShowMainMenu();
        }

        public void ShowAllPasswords()
        {
            List<PasswordEntry> passwordList = _passwordManager.ShowAllPasswords();

            if (passwordList.Count == 0)
            {
                Console.WriteLine("No passwords stored.");
            }
            else
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("| Site Name        | Email               | Password |");
                Console.WriteLine("---------------------------------------------------------------");

                foreach (var site in passwordList)
                {
                    Console.WriteLine($"| {site.SiteName,-15} | {site.Email,-20} | {site.Password,-18} |");
                }

    Console.WriteLine("---------------------------------------------------------------");
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to show Main menu.....!");
            Console.ReadLine();
            ShowMainMenu();
        }

        //To Do       
        public void UnactiveCheck()
        {
            throw new NotImplementedException();
        }



    
    }
}
    