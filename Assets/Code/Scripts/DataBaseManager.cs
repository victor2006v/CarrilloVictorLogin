using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance { get; private set; }
    private string dbUri;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        dbUri = "URI=file:" + Application.dataPath + "/UserDataBase.sqlite";
        InitializeDatabase();
    }
    private void InitializeDatabase() {
        using (IDbConnection dbConnection = new SqliteConnection(dbUri))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT UNIQUE NOT NULL,
                        password TEXT NOT NULL,
                        created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";
                dbCommand.ExecuteNonQuery();
            }

            Debug.Log("Database initialized successfully");
        }
    }

    public bool UserExists(string username) {
        using (IDbConnection dbConnection = new SqliteConnection(dbUri))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = "SELECT COUNT(*) FROM Users WHERE username = @username";

                IDbDataParameter param = dbCommand.CreateParameter();
                param.ParameterName = "@username";
                param.Value = username;
                dbCommand.Parameters.Add(param);

                int count = System.Convert.ToInt32(dbCommand.ExecuteScalar());
                return count > 0;
            }
        }
    }

    public bool RegisterUser(string username, string password) {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Username or password cannot be empty");
            return false;
        }

        if (UserExists(username))
        {
            Debug.LogWarning("Username already exists");
            return false;
        }

        try
        {
            using (IDbConnection dbConnection = new SqliteConnection(dbUri))
            {
                dbConnection.Open();

                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    // In production, hash the password using a proper library
                    string hashedPassword = HashPassword(password);

                    dbCommand.CommandText = "INSERT INTO Users (username, password) VALUES (@username, @password)";

                    IDbDataParameter usernameParam = dbCommand.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    dbCommand.Parameters.Add(usernameParam);

                    IDbDataParameter passwordParam = dbCommand.CreateParameter();
                    passwordParam.ParameterName = "@password";
                    passwordParam.Value = hashedPassword;
                    dbCommand.Parameters.Add(passwordParam);

                    dbCommand.ExecuteNonQuery();
                    Debug.Log("User registered successfully: " + username);
                    return true;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error registering user: " + e.Message);
            return false;
        }
    }

    public bool LoginUser(string username, string password) {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Username or password cannot be empty");
            return false;
        }

        try
        {
            using (IDbConnection dbConnection = new SqliteConnection(dbUri))
            {
                dbConnection.Open();

                using (IDbCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "SELECT password FROM Users WHERE username = @username";

                    IDbDataParameter param = dbCommand.CreateParameter();
                    param.ParameterName = "@username";
                    param.Value = username;
                    dbCommand.Parameters.Add(param);

                    using (IDataReader reader = dbCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader.GetString(0);
                            string hashedPassword = HashPassword(password);

                            if (storedPassword == hashedPassword)
                            {
                                Debug.Log("Login successful: " + username);
                                return true;
                            }
                            else
                            {
                                Debug.LogWarning("Incorrect password");
                                return false;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("User does not exist");
                            return false;
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error logging in: " + e.Message);
            return false;
        }
    }

    // Simple hash function - For production use a proper library like BCrypt
    private string HashPassword(string password) {
        using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }





}
