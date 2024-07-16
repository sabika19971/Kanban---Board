using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardController
    {
        private readonly string _connectionString; 
        private readonly string _tableName;
        private const string TableName = "Boards";
        private string dbFileName = "kanban.db";
        private string solutionDirectory = Path.GetFullPath(Directory.GetCurrentDirectory());

        internal BoardController() // init and connecting to the DB
        {

            string path = Path.Combine(solutionDirectory, dbFileName);
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        internal bool Insert(BoardDAO board)
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString)) 
            {
                try
                {
                    connection.Open(); 
                    SQLiteCommand command = new SQLiteCommand(null, connection); 
                    string insert = $"INSERT INTO {TableName} ({board.IdColumnName},{board.BoardColumnName},{board.OwnerColumnName}) Values (@ID,@name,@owner)"; // the @ is a place holders to avoid SQL injection                   
                    SQLiteParameter idParam = new SQLiteParameter(@"ID", board.Id);
                    SQLiteParameter NameParam = new SQLiteParameter(@"name", board.Name); 
                    SQLiteParameter ownerParam = new SQLiteParameter(@"owner", board.Owner); 
                    command.CommandText = insert; 
                    command.Parameters.Add(idParam); 
                    command.Parameters.Add(NameParam); 
                    command.Parameters.Add(ownerParam); 

                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Board insertion to the DB has failed"); 
                }
                return result > 0; 
            }
        }

        internal bool Update(long Id, string column, string newValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{column}]=@Val where Id=@Id"
                };
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Val", newValue);
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to update a board in the DB");
                }
            }
            return res > 0;
        }


        internal bool Delete(long Id, string name)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
               
                command.CommandText = $"DELETE from {TableName}" +
                                       $" where Id=@Id AND Name=@name;";
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@name", name);
                try
                {
                    connection.Open(); 
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to Delete a board from the DB");
                }
            }
            return res > 0;
        }


        internal bool DeleteAllBoards()
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
                    throw new Exception("Failed to Delete boards from the DB");
                }
            }
            return res > 0;
        }


        internal BoardDAO Select(Dictionary<string, string> filters)
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
                    throw new Exception("Failed to load a board from the DB");
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


        internal List<BoardDAO> SelectAllBoards() 
        {
            List<BoardDAO> boards = new List<BoardDAO>();
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();  
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        boards.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load boards from the DB");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }
                }

                return boards;
            }
        }

        private BoardDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new BoardDAO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
        }
    }
}
