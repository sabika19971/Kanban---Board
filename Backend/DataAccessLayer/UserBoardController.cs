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
using IntroSE.Kanban.Backend.DataAccessLayer;
namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserBoardController
    {

        private readonly string _connectionString; 
        private readonly string _tableName;
        private const string TableName = "UsersBoardsStatus";
        private string dbFileName = "kanban.db";
        private string solutionDirectory = Path.GetFullPath(Directory.GetCurrentDirectory());

        internal UserBoardController() // init and connecting to the DB
        {
            string path = Path.Combine(solutionDirectory, dbFileName);
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        internal bool Insert(UserBoardssStatusDAO user)
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString)) 
            {
                try
                {
                    connection.Open(); 
                    SQLiteCommand command = new SQLiteCommand(null, connection); 
                    string insert = $"INSERT INTO {TableName} ({user.idColumnName},{user.EmailColumnName},{user.statusColumnName}) Values (@boardIdVal,@emailVal,@statusVal)"; // the @ is a place holders to avoid SQL injection                   
                    SQLiteParameter boardIdPram = new SQLiteParameter(@"boardIdVal", user.BoardId);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email); 
                    SQLiteParameter stasusParam = new SQLiteParameter(@"statusVal", user.Status);
                    command.CommandText = insert; 
                    command.Parameters.Add(boardIdPram);
                    command.Parameters.Add(emailParam); 
                    command.Parameters.Add(stasusParam); 

                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Connection insertion to the DB has failed"); 
                }
                return result > 0; 
            }
        }

        

        internal bool Update(long boardID,string email, string column, string newValue)
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
                    throw new Exception("Failed to update connection");
                }
            }
            return res > 0;
        }

        internal bool UpdateOwnership(long boardID, string currentOwner, string newOwner)
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
                    throw new Exception("Failed to update ownership");
                }
            }
            return res > 0;
        }

        internal bool Delete(string email, long Id)
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
                    connection.Open(); 
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to delete a connection from the DB");
                }
            }
            return res > 0;
        }

        internal bool DeleteBoard(long Id)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE from {TableName} where Id=@Id;";
                command.Parameters.AddWithValue("@Id", Id);
                try
                {
                    connection.Open(); 
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to delete all connections to a board from the DB");
                }
            }
            return res > 0;
        }

        internal bool DeleteAllConnections()
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
                    throw new Exception("Failed to Delete Connections from the DB");
                }
            }
            return res > 0;
        }


        internal List<UserBoardssStatusDAO> LoadMembers(long BoardId)
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
                    connection.Open();  
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        connections.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load members from the DB");
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

        private UserBoardssStatusDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new UserBoardssStatusDAO(reader.GetString(0), reader.GetInt32(1),reader.GetInt32(2));
        }
    }
}

