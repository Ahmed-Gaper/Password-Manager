using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Password_Manager.Database
{
    public class SQLiteDatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public SQLiteDatabaseService(string databasePath)
        {
            _connectionString = $"Data Source={databasePath}";
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand createPasswordsTableCommand = connection.CreateCommand();
                createPasswordsTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Passwords (
                        SiteName TEXT PRIMARY KEY,
                        EncryptedPassword TEXT NOT NULL
                    )";
                createPasswordsTableCommand.ExecuteNonQuery();
            }
        }

        public bool AddPassword(string siteName, string encryptedPassword)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Passwords (SiteName, EncryptedPassword)
                    VALUES ($siteName, $encryptedPassword)";
                command.Parameters.AddWithValue("$siteName", siteName);
                command.Parameters.AddWithValue("$encryptedPassword", encryptedPassword);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // SQLite constraint violation (e.g., duplicate site name)
                {
                    return false;
                }
            }
        }

        public bool UpdatePassword(string siteName, string newEncryptedPassword)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Passwords
                    SET EncryptedPassword = $newEncryptedPassword
                    WHERE SiteName = $siteName";
                command.Parameters.AddWithValue("$newEncryptedPassword", newEncryptedPassword);
                command.Parameters.AddWithValue("$siteName", siteName);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool DeletePassword(string siteName)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Passwords WHERE SiteName = $siteName";
                command.Parameters.AddWithValue("$siteName", siteName);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public List<PasswordEntry> GetAllPasswords()
        {
            List<PasswordEntry> passwords = new List<PasswordEntry>();

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT SiteName, EncryptedPassword FROM Passwords";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PasswordEntry entry = new PasswordEntry
                        {
                            SiteName = reader.GetString(0),
                            EncryptedPassword = reader.GetString(1)
                        };
                        passwords.Add(entry);
                    }
                }
            }

            return passwords;
        }

        public void BackupDatabase(string backupPath)
        {
            string sourcePath = _connectionString.Replace("Data Source=", "");
            File.Copy(sourcePath, backupPath, overwrite: true);
        }

        public void RestoreDatabase(string backupPath)
        {
            string destinationPath = _connectionString.Replace("Data Source=", "");
            File.Copy(backupPath, destinationPath, overwrite: true);
        }
    }
}