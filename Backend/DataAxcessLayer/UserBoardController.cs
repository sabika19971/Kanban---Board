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
using IntroSE.Kanban.Backend.DataExcessLayer;
namespace IntroSE.Kanban.Backend.DataAxcessLayer
{
    internal class UserBoardController
    {

        private readonly string _connectionString; // where is the DB
        private readonly string _tableName;
        private const string TableName = "UsersBoardsStatus";
        string dbFileName = "kanban.db";
        string solutionDirectory = Path.GetFullPath(Directory.GetCurrentDirectory());

        public UserBoardController() // init and connecting to the DB
        {
            string path = Path.Combine(solutionDirectory, dbFileName);
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        public bool Insert(UserBoardssStatusDAO user)
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString)) // using the connection for the following scope
            {
                try
                {
                    connection.Open(); // nessecery even though we use "using"
                    SQLiteCommand command = new SQLiteCommand(null, connection); // on which connection the command will run
                    string insert = $"INSERT INTO {TableName} ({user.idColumnName},{user.EmailColumnName},{user.statusColumnName}) Values (@boardIdVal,@emailVal,@statusVal)"; // the @ is a place holders to avoid SQL injection                   
                    SQLiteParameter boardIdPram = new SQLiteParameter(@"boardIdVal", user.BoardId);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email); // inserting parameters to the place holders
                    SQLiteParameter stasusParam = new SQLiteParameter(@"statusVal", user.Status); // inserting parameters to the place holders
                    command.CommandText = insert; // assigning the command 
                    command.Parameters.Add(boardIdPram);
                    command.Parameters.Add(emailParam); // update inside the command
                    command.Parameters.Add(stasusParam); // update inside the command

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

        

        public bool Update(int boardID,string email, string column, string newValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{column}]=@Val where Email=@Email AND BoardId=@BoardId"
                };
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@BoardId", boardID);
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

        public bool UpdateOwnership(int boardID, string currentOwner, string newOwner)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command1 = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [Status]=@Val where Email=@Email AND Id=@BoardId"
                };
                SQLiteCommand command2 = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [Status]=@Val where Email=@Email AND Id=@BoardId"
                };
                command1.Parameters.AddWithValue("@BoardId", boardID);
                command2.Parameters.AddWithValue("@BoardId", boardID);
                
                command1.Parameters.AddWithValue("@Email", currentOwner);                
                command1.Parameters.AddWithValue("@Val", 0);
                
                command2.Parameters.AddWithValue("@Email", newOwner);
                command2.Parameters.AddWithValue("@Val", 1);
                try
                {
                    connection.Open();
                    res = command1.ExecuteNonQuery() + command2.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(" failed to update email");
                }
            }
            return res > 0;
        }

        public bool Delete(string email, int Id)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE from {TableName} where Email=@Email AND Id=@Id;";
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Id", Id);
                try
                {
                    connection.Open(); // nessecery even though we use "using"
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("failed to delete a connection to a board");
                }
            }
            Console.WriteLine(res);
            return res > 0;
        }

        public bool DeleteBoard(int Id)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE from {TableName} where Id=@Id;";
                command.Parameters.AddWithValue("@Id", Id);
                try
                {
                    connection.Open(); // nessecery even though we use "using"
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("failed to delete all connections to a board");
                }
            }
            Console.WriteLine(res);
            return res > 0;
        }

        public bool DeleteAllConnections()
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
                    throw new Exception("Failed to Delete Connections from the DB");
                }
            }
            Console.WriteLine(res);
            return res > 0;
        }

        public UserBoardssStatusDAO Select(Dictionary<string, string> filters)
        {
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);

                // Constructing the WHERE clause dynamically based on the provided filters
                StringBuilder queryBuilder = new StringBuilder($"SELECT * FROM {TableName} WHERE ");

                // Add each filter condition to the WHERE clause
                int index = 0;
                foreach (var filter in filters)
                {
                    string paramName = $"@Param{index}";
                    queryBuilder.Append($"{filter.Key} = {paramName} AND ");
                    command.Parameters.AddWithValue(paramName, filter.Value);
                    index++;
                }

                // Remove the last " AND " from the query
                queryBuilder.Remove(queryBuilder.Length - 5, 5); // Remove the last " AND "

                command.CommandText = queryBuilder.ToString();

                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open(); // Open the connection
                    dataReader = command.ExecuteReader();

                    if (dataReader.Read()) // Check if there is a row to read
                    {
                        return ConvertReaderToObject(dataReader); // Convert dataReader to UserDAO object
                    }
                    else
                    {
                        return null; // No matching user found
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load user from the DB", ex);
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close(); // Close the data reader
                    }
                }
            }
        }

        internal List<UserBoardssStatusDAO> LoadMembers(int BoardId)
        {
            List<UserBoardssStatusDAO> connections = new List<UserBoardssStatusDAO>();
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName} where Id=@id;";
                command.Parameters.AddWithValue("@id", BoardId);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open(); // nessecery even though we use "using"  
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        connections.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch (Exception ex)
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
                return connections;
            }
        }

        /*
        public List<UserBoardssStatusDAO> SelectAllConnections() 
        {
            List<UserBoardssStatusDAO> connections = new List<UserBoardssStatusDAO>();
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open(); // nessecery even though we use "using"  
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        connections.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch (Exception ex)
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

                return connections;
            }
        }
        */


        private UserBoardssStatusDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new UserBoardssStatusDAO(reader.GetString(0), reader.GetInt32(1),reader.GetInt32(2));
        }
    }
}

