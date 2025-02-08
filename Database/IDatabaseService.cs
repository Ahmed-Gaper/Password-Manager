using System.Collections.Generic;

namespace Password_Manager.Database
{
    public interface IDatabaseService
    {
        void InitializeDatabase();

        bool AddPassword(string siteName, string encryptedPassword);
        bool UpdatePassword(string siteName, string newEncryptedPassword);
        bool DeletePassword(string siteName);
        List<PasswordEntry> GetAllPasswords();

        void BackupDatabase(string backupPath);
        void RestoreDatabase(string backupPath);
    }

    public class PasswordEntry
    {
        public string SiteName { get; set; }
        public string EncryptedPassword { get; set; }
    }
}