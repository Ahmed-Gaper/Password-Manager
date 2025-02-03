using Password_Manager.UI;
using Password_Manager.Core;
using Password_Manager.Database;
using Password_Manager.Utils;
using Paswword_Manager.Core;

namespace Password_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //IEncryptionService encryptionService = new AesEncryptionService();
            IDatabaseService databaseService = new SQLiteDatabaseService("password_manager.db");
            IAuthenticationService authenticationService=new SHA_1AuthenticationService();
            IPasswordManager passwordManager = new PasswordManager(databaseService, authenticationService);

            IInputValidator inputValidator = new InputValidator();
            IUserInterface consoleUI = new ConsoleUI(passwordManager, inputValidator);

            consoleUI.Welcome();
            consoleUI.StartTheProgram();
            consoleUI.ShuttingDown();
        }
    }
}