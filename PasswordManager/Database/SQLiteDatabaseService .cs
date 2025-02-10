using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Password_Manager.Models;

namespace Password_Manager.Database
{
    public class SQLiteDatabaseService : IDatabaseService
    {
        private string _connectionString;

        public string ConnectionString
        {
            set { string sanitizedEmail = value.Replace("@", "_").Replace(".", "_");
            string databasePath = $"password_manager_{sanitizedEmail}.db";
            _connectionString = $"Data Source={databasePath}"; }
        }
   
        public void InitializeDatabase(string userEmail)
        {
            string sanitizedEmail = userEmail.Replace("@", "_").Replace(".", "_");
            string databasePath = $"password_manager_{sanitizedEmail}.db";
            _connectionString = $"Data Source={databasePath}";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                // Create the Passwords table
                SqliteCommand createPasswordsTable = connection.CreateCommand();
                createPasswordsTable.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Passwords (
                        SiteName TEXT PRIMARY KEY,
                        Email TEXT NOT NULL,
                        EncryptedPassword TEXT NOT NULL
                    )";
                createPasswordsTable.ExecuteNonQuery();

                // Create the UserCredentials table to store the user's master password
                SqliteCommand createUserTable = connection.CreateCommand();
                createUserTable.CommandText = @"
                    CREATE TABLE IF NOT EXISTS UserCredentials (
                        Email TEXT PRIMARY KEY,
                        HashedPassword TEXT NOT NULL
                    )";
                createUserTable.ExecuteNonQuery();
            }
        }

       
        public bool DoesUserExist(string userEmail)
        {
            string sanitizedEmail = userEmail.Replace("@", "_").Replace(".", "_");
            string databasePath = $"password_manager_{sanitizedEmail}.db";
            return File.Exists(databasePath);
        }


        public void RegisterUser(string userEmail, string masterHashedPassword)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT OR REPLACE INTO UserCredentials (Email, HashedPassword)
                    VALUES ($email, $hashedPassword)";
                command.Parameters.AddWithValue("$email", userEmail);
                command.Parameters.AddWithValue("$hashedPassword", masterHashedPassword);
                command.ExecuteNonQuery();
            }
        }

  
        public bool VerifyUserPassword(string userEmail, string enteredHashedPassword)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT HashedPassword 
                    FROM UserCredentials 
                    WHERE Email = $email";
                command.Parameters.AddWithValue("$email", userEmail);
                Console.WriteLine(_connectionString);
                object result = command.ExecuteScalar();
                if (result == null)
                {
                    return false;
                }
                string storedHashedPassword = result.ToString();
                return storedHashedPassword == enteredHashedPassword;
            }
        }



        public void AddPassword(string siteName, string email, string encryptedPassword)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Passwords (SiteName, Email, EncryptedPassword)
                    VALUES ($siteName, $email, $encryptedPassword)";
                command.Parameters.AddWithValue("$siteName", siteName);
                command.Parameters.AddWithValue("$email", email);
                command.Parameters.AddWithValue("$encryptedPassword", encryptedPassword);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
                {
                    Console.WriteLine("This site already exists in the database.");
                }
            }
        }

        public bool SiteExist(string siteName)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Passwords WHERE SiteName = $siteName";
                command.Parameters.AddWithValue("$siteName", siteName);

                long count = (long)command.ExecuteScalar();
                return count > 0;
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
                command.CommandText = "SELECT SiteName, Email, EncryptedPassword FROM Passwords";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PasswordEntry entry = new PasswordEntry
                        {
                            SiteName = reader.GetString(0),
                            Email = reader.GetString(1),
                            Password = reader.GetString(2)
                        };
                        passwords.Add(entry);
                    }
                }
            }

            return passwords;
        }


    
    }
}
