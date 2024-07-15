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
    internal class UserController                       // FUNCTIONS SHOULD BE INTERNAL IN ALL CONTROLLERS
    {
        private readonly string _connectionString; // where is the DB
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
            using (var connection = new SQLiteConnection(this._connectionString)) // using the connection for the following scope
            {
                try
                {
                    connection.Open(); // nessecery even though we use "using"
                    SQLiteCommand command = new SQLiteCommand(null, connection); // on which connection the command will run
                    string insert = $"INSERT INTO {TableName} ({user.EmailColumnName},{user.PasswordColumnName}) Values (@emailVal,@passwordVal)"; // the @ is a place holders to avoid SQL injection                   
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email); // inserting parameters to the place holders
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password); // inserting parameters to the place holders
                    command.CommandText = insert; // assigning the command 
                    command.Parameters.Add(emailParam); // update inside the command
                    command.Parameters.Add(passwordParam); // update inside the command

                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                   throw new Exception(" Insertion to the DB has failed"); // will be handeled in the service layer
                }
                Console.WriteLine(result);              
                return result > 0; // return true if the command affected 1 or more rows in the DB
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
                    throw new Exception(" failed to update email");
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
                    connection.Open(); // nessecery even though we use "using"
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load user from the DB");
                }             
            }
            Console.WriteLine(res);
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
                    connection.Open(); // nessecery even though we use "using"
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load user from the DB");
                }
            }
            Console.WriteLine(res);
            return res > 0;
        }

        internal UserDAO Select(string email)  // Dictionary<string,string> if we want many filters
        {
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName} where Email=@Email;";
                command.Parameters.AddWithValue("@Email", email);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open(); // nessecery even though we use "using"  
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read()) // indicates if there is a line to read or not
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



        internal List<UserDAO> SelectAllUsers() // will be used for LoadUsers
        {
            List<UserDAO> users = new List<UserDAO>();
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open(); // nessecery even though we use "using"  
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
