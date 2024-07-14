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
    internal class ColumnController
    {
        private readonly string _connectionString; // where is the DB
        private readonly string _tableName;
        private const string TableName = "Columns";
        string dbFileName = "kanban.db";
        string solutionDirectory = Path.GetFullPath(Directory.GetCurrentDirectory());

        public ColumnController() // init and connecting to the DB
        {
            string path = Path.Combine(solutionDirectory, dbFileName);
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        public bool Insert(ColumnDAO col)
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString)) // using the connection for the following scope
            {
                try
                {
                    connection.Open(); // nessecery even though we use "using"
                    SQLiteCommand command = new SQLiteCommand(null, connection); // on which connection the command will run
                    string insert = $"INSERT INTO {TableName} ({col.idColumnName},{col.boardIdColumnName},{col.maxTasksColumnName},{col.currTaskColumnName}) Values (@id,@boardId,@maxTasks,@currTask)"; // the @ is a place holders to avoid SQL injection                   
                    SQLiteParameter id = new SQLiteParameter(@"id", col.Id); // inserting parameters to the place holders
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", col.BoardId); // inserting parameters to the place holders
                    SQLiteParameter maxTasksParam = new SQLiteParameter(@"maxTasks", col.MaxTasks); // inserting parameters to the place holders
                    SQLiteParameter currTaskParam = new SQLiteParameter(@"currTask", col.CurrTask); // inserting parameters to the place holders
                    command.CommandText = insert; // assigning the command 
                    command.Parameters.Add(id); // update inside the command
                    command.Parameters.Add(boardIdParam); // update inside the command
                    command.Parameters.Add(maxTasksParam); // update inside the command
                    command.Parameters.Add(currTaskParam); // update inside the command

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

        public bool Update(int id,int boradId, string column, int newValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{column}]=@Val where BoardId=@BoardId AND Id=@Id;"
                };
                command.Parameters.AddWithValue("@BoardId", boradId);
                command.Parameters.AddWithValue("@Val", newValue);
                command.Parameters.AddWithValue("@Id", id);
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(" Failed to update a Cloumn field");
                }
            }
            return res > 0;
        }



        public bool Delete(int Id,int boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE from {TableName} where Id=@Id AND BoardId=@BoardId;";
                command.Parameters.AddWithValue("@BoardId", boardId);
                command.Parameters.AddWithValue("@Id", Id);
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

        public bool DeleteAllColumns()
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


        public ColumnDAO Select(int id, int boardId)  
        {
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName} where Id=@IdVal AND BoardId=@BoardIdVal;";
                command.Parameters.AddWithValue("@IdVal", id);
                command.Parameters.AddWithValue("@BoardIdVal", boardId);
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
                    throw new Exception("Failed to load column from the DB");
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

        public ColumnDAO SelectFeatures(Dictionary<string, string> filters)
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



        public List<ColumnDAO> SelectAllColumns() // will be used for LoadColumns
        {
            List<ColumnDAO> columns = new List<ColumnDAO>();
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
                        columns.Add(ConvertReaderToObject(dataReader));
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

                return columns;
            }
        }


        private ColumnDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new ColumnDAO(reader.GetInt32(0), reader.GetInt32(1),reader.GetInt32(2), reader.GetInt32(3));
        }
    }
}

