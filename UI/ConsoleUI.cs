using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password_Manager.Core;

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
                    Console.WriteLine("WRONG CHOICE !  Enter valid number (1,2 or 3)");
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
            PrintCenteredBanner("SinUp");
            string masterEmail,masterPassword,masterPasswordConfirmation;

            bool vaildInput=false;
            do
            {

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
                if(_inputValidator.ValidateMasterEmail(masterEmail) && _inputValidator.ValidateMaterPassword(masterPassword,masterPasswordConfirmation))
                {  
                 vaildInput=true;
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
                Console.Write("You alredy have an account.........!");
                Console.ResetColor();
                //Will update it to allow to add new account (multi users)
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
            PrintCenteredBanner("LogIn");

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
            PrintCenteredBanner("Main Menu");
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
        public void GeneratePassword()
        {
     
        }
        public void AddNewPassword()
        {



            bool vaildInput=false;
            string siteName,email,password,confirmationPassword;

            do
            {
            PrintSidebarTitle("Add New Password ");
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
                   bool wrongChoice=false; 
                   bool running=true;

                   while(running)
                   {
                       
                        if(!wrongChoice)
                        {
 
                        Console.WriteLine("\nWhat would you like to do?");
                        Console.WriteLine("[1] Retry Add Password");
                        Console.WriteLine("[2] Go Back to Main menu");
                
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

                }

            } while (!vaildInput);

             _passwordManager.AddNewPassword(siteName,email,password);

            Console.ForegroundColor=ConsoleColor.Green;
            Console.WriteLine("Added Successfully.........!");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Press any key to show Main menu.....!");
            Console.ReadLine();
            ShowMainMenu();
        }

        public void DeletePassword()
        {
            bool vaildInput=false;
            string siteName;

            PrintSidebarTitle("Delete Password ");
            Console.Write("Site name : ");
            siteName=Console.ReadLine();


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

            Console.WriteLine();
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
            PrintSidebarTitle("Update Password ");
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
                   bool wrongChoice=false; 
                   bool running=true;

                   while(running)
                   {
                       
                        if(!wrongChoice)
                        {
 
                        Console.WriteLine("\nWhat would you like to do?");
                        Console.WriteLine("[1] Retry Update Password");
                        Console.WriteLine("[2] Go Back to Main menu");
                
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

                }

            } while (!vaildInput);

             _passwordManager.UpdatePassword(siteName,newpassword);

            Console.ForegroundColor=ConsoleColor.Green;
            Console.WriteLine("Updated Successfully.........!");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Press any key to show Main menu.....!");
            Console.ReadLine();
            ShowMainMenu();
        }

        public void ShowAllPasswords()
        {
            _passwordManager.ShowAllPasswords();
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



        #region
        
        private void PrintCenteredBanner(string text)
        {
            int width = Console.WindowWidth;
            string border = new string('*', width);
            int padding = (width - text.Length) / 2;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(border);
            Console.WriteLine(new string(' ', padding) + text);
            Console.WriteLine(border);
            Console.ResetColor();
        }
        private void PrintSidebarTitle(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;

            string border = "┌" + new string('─', title.Length + 2) + "┐";
            string textLine = $"│ {title} │";
            string bottomBorder = "└" + new string('─', title.Length + 2) + "┘";

            Console.WriteLine(border);
            Console.WriteLine(textLine);
            Console.WriteLine(bottomBorder);

            Console.ResetColor();
            Console.WriteLine();
        }

        #endregion
    }
}
    