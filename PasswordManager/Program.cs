using Password_Manager.UI;
using Password_Manager.Core;
using Password_Manager.Database;
using PasswordManager.Encryption;

namespace Password_Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEncryptionService encryptionService = new XorEncryptionService();
            IDatabaseService databaseService = new SQLiteDatabaseService();
            IAuthenticationService authenticationService=new SHA_1AuthenticationService();
            IPasswordManager passwordManager = new PasswordManagerService(databaseService, authenticationService,encryptionService);

            IInputValidator inputValidator = new InputValidator();
            IUserInterface consoleUI = new ConsoleUI(passwordManager, inputValidator);

            consoleUI.Welcome();
            consoleUI.StartTheProgram();
            consoleUI.ShuttingDown();
        }
    }
}