using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data.Entity.Infrastructure;
using EllipticCurve;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserController                      
    {
        private readonly string _connectionString; 
        private readonly string _tableName;
        private const string TableName = "Users";
        private string dbFileName = "kanban.db";
        private string solutionDirectory = Path.GetFullPath(Directory.GetCurrentDirectory());

        internal UserController() // init and connecting to the DB
        {
            string path = Path.Combine(solutionDirectory, dbFileName);
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        internal bool Insert(UserDAO user) 
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString)) 
            {
                try
                {
                    connection.Open(); 
                    SQLiteCommand command = new SQLiteCommand(null, connection); 
                    string insert = $"INSERT INTO {TableName} ({user.EmailColumnName},{user.PasswordColumnName}) Values (@emailVal,@passwordVal)";                  
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email); 
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password); 
                    command.CommandText = insert; 
                    command.Parameters.Add(emailParam); 
                    command.Parameters.Add(passwordParam); 

                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                   throw new Exception("User insertion to the DB has failed"); 
                }             
                return result > 0; 
            }           
        }

        internal bool Update(string email, string column, string newValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{column}]=@Val where Email=@Email"
                };
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Val", newValue);
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to update user int the DB");
                }
            }
            return res > 0;
        }

        

        internal bool Delete(string email) 
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE from {TableName} where Email=@Email;";
                command.Parameters.AddWithValue("@Email", email);
                try
                {
                    connection.Open(); 
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to delete a user from the DB");
                }             
            }
            return res > 0;
        }

        internal bool DeleteAllUsers()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE from {TableName};";         
                try
                {
                    connection.Open(); 
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to delete users from the DB");
                }
            }
            return res > 0;
        }

        internal UserDAO Select(string email)  
        {
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName} where Email=@Email;";
                command.Parameters.AddWithValue("@Email", email);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open(); 
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read()) 
                    {
                        return ConvertReaderToObject(dataReader);
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load user from the DB");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                }             
            }
        } 

        internal List<UserDAO> SelectAllUsers() 
        {
            List<UserDAO> users = new List<UserDAO>();
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();  
                    dataReader = command.ExecuteReader();

                    while(dataReader.Read())
                    {
                        users.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch(Exception ex) 
                {
                    throw new Exception("Failed to load users from the DB");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                }
               
                return users;
            }
        } 
        

        

        private UserDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new UserDAO(reader.GetString(0), reader.GetString(1));
        }
    }
}
