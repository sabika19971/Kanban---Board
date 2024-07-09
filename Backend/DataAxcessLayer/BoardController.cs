using IntroSE.Kanban.Backend.DataExcessLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAxcessLayer
{
    internal class BoardController
    {
        private readonly string _connectionString; // where is the DB
        private readonly string _tableName;
        private const string TableName = "Borads";
        string dbFileName = "KanbanDB.db";
        string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

        public BoardController() // init and connecting to the DB
        {

            string path = Path.GetFullPath(Path.Combine(solutionDirectory, "Backend", dbFileName));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        public bool Insert(BoardDAO board)
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString)) // using the connection for the following scope
            {
                try
                {
                    connection.Open(); // nessecery even though we use "using"
                    SQLiteCommand command = new SQLiteCommand(null, connection); // on which connection the command will run
                    string insert = $"INSERT INTO {TableName} ({board.IdColumnName},{board.BoardColumnName},{board.OwnerColumnName}) Values (@ID,@name,@owner)"; // the @ is a place holders to avoid SQL injection                   
                    SQLiteParameter idParam = new SQLiteParameter(@"ID", board.Id); // inserting parameters to the place holders
                    SQLiteParameter NameParam = new SQLiteParameter(@"name", board.Name); // inserting parameters to the place holders
                    SQLiteParameter ownerParam = new SQLiteParameter(@"owner", board.Owner); // inserting parameters to the place holders
                    command.CommandText = insert; // assigning the command 
                    command.Parameters.Add(idParam); // update inside the command
                    command.Parameters.Add(NameParam); // update inside the command
                    command.Parameters.Add(ownerParam); // update inside the command

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

        public bool Update(int Id, string column, string newValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{column}]=@Val where Id=@Id"
                };
                command.Parameters.AddWithValue("@Email", Id);
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



        public bool Delete(int Id, string name)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
               
                command.CommandText = $"DELETE from {TableName}" +
                                       $" where Id=@Id AND Name=@name;";
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@Name", name);
                try
                {
                    connection.Open(); // nessecery even though we use "using"
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to Delete board from the DB");
                }
            }
            Console.WriteLine(res);
            return res > 0;
        }


        public BoardDAO Select(Dictionary<string, string> filters)
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


        public List<BoardDAO> SelectAllUsers() // will be used for LoadUsers
        {
            List<BoardDAO> boards = new List<BoardDAO>();
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
                        boards.Add(ConvertReaderToObject(dataReader));
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

                return boards;
            }
        }


        private BoardDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new BoardDAO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
        }
    }
}
