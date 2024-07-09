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
    internal class TaskController
    {
        private readonly string _connectionString; // where is the DB
        private readonly string _tableName;
        private const string TableName = "Tasks";
        string dbFileName = "KanbanDB.db";
        string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

        public TaskController() // init and connecting to the DB
        {

            string path = Path.GetFullPath(Path.Combine(solutionDirectory, "Backend", dbFileName));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }
        
        /*public bool Insert(TaskDAO task) 
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString)) // using the connection for the following scope
            {
                try
                {
                    connection.Open(); // nessecery even though we use "using"
                    SQLiteCommand command = new SQLiteCommand(null, connection); // on which connection the command will run
                    string insert = $"INSERT INTO {TableName} ({task.BoardIdColumnName},{task.idColumnName},{task.CreationTimeColumnName},{task.DueDateColumnName}" +
                        $"{task.TitleColumnName},{task.DescriptionColumnName},{task.ColumnOrdinalColumnName}" +
                        $"{task.AssigneeColumnName})" +
                        $" Values (@boardIdVal,@idTaskVal,@creationTimeVal,@dueDateVal,@titleVal,@descriptionVal,columnOrdinalVal,@asseingeeVal)"; // the @ is a place holders to avoid SQL injection                   
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", task.BoardId); // inserting parameters to the place holders
                    SQLiteParameter idTaskParam = new SQLiteParameter(@"idTaskVal", task.Id); // inserting parameters to the place holders
                    SQLiteParameter CreationTimeParam = new SQLiteParameter(@"creationTimeVal", task.CreationTime); // inserting parameters to the place holders
                    SQLiteParameter DueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate); // inserting parameters to the place holders
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title); // inserting parameters to the place holders
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description); // inserting parameters to the place holders
                    SQLiteParameter ColumnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", task.ColumnOrdinal); // inserting parameters to the place holders
                    SQLiteParameter AssigneeParam = new SQLiteParameter(@"asseingeeVal", task.Assignee); // inserting parameters to the place holders
                    command.CommandText = insert; // assigning the command 
                    command.Parameters.Add(boardIdParam); // update inside the command
                    command.Parameters.Add(idTaskParam); // update inside the command
                    command.Parameters.Add(CreationTimeParam); // update inside the command
                    command.Parameters.Add(DueDateParam); // update inside the command
                    command.Parameters.Add(titleParam); // update inside the command
                    command.Parameters.Add(descriptionParam); // update inside the command
                    command.Parameters.Add(ColumnOrdinalParam); // update inside the command
                    command.Parameters.Add(AssigneeParam); // update inside the command

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
*/

        public bool Insert(TaskDAO task)
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                try
                {
                    connection.Open();
                    string insert = $"INSERT INTO {TableName} ({task.BoardIdColumnName}, {task.idColumnName}, {task.CreationTimeColumnName}, {task.DueDateColumnName}, " +
                                    $"{task.TitleColumnName}, {task.DescriptionColumnName}, {task.ColumnOrdinalColumnName}, {task.AssigneeColumnName}) " +
                                    $"VALUES (@boardIdVal, @idTaskVal, @creationTimeVal, @dueDateVal, @titleVal, @descriptionVal, @columnOrdinalVal, @assigneeVal)";

                    using (var command = new SQLiteCommand(insert, connection))
                    {
                        command.Parameters.AddWithValue("@boardIdVal", task.BoardId);
                        command.Parameters.AddWithValue("@idTaskVal", task.Id);
                        command.Parameters.AddWithValue("@titleVal", task.Title);
                        command.Parameters.AddWithValue("@descriptionVal", task.Description);
                        command.Parameters.AddWithValue("@columnOrdinalVal", task.ColumnOrdinal);
                        command.Parameters.AddWithValue("@assigneeVal", task.Assignee);

                        // Convert DateTime to string in the expected format for SQLite
                        command.Parameters.AddWithValue("@creationTimeVal", task.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@dueDateVal", task.DueDate.ToString("yyyy-MM-dd HH:mm:ss"));


                        result = command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Insertion to the DB has failed", ex);
                }
                return result > 0;
            }
        }


        // update for description or title
        public bool Update(int taskId,int boardId, string column, string newValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{column}]=@Val where Id=@taskId And BoardId=@boardId"
                };
                command.Parameters.AddWithValue("@taskId", taskId);
                command.Parameters.AddWithValue("@Val", newValue);
                command.Parameters.AddWithValue("@boardId", boardId);
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

        public bool UpdateColumnOrdinal(int taskId, string column, int newValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{column}]=@Val where Id=@taskId"
                };
                command.Parameters.AddWithValue("@taskId", taskId);
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


        public bool Delete(int Id,int boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE from {TableName} where Id=@Id AND BoradId=@boardId ;";
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@boardId", boardId);
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


        public TaskDAO Select(Dictionary<string, string> filters)
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



        public List<TaskDAO> SelectAllTasks() // will be used for LoadUsers
        {
            List<TaskDAO> tasks = new List<TaskDAO>();
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
                        tasks.Add(ConvertReaderToObject(dataReader));
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

                return tasks;
            }
        }

        internal bool UpdateTaskDueDate(int taskId,int boardId, string column, DateTime newValue )
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{column}]=@Val where Id=@taskId AND BoardId=@BoardId"
                };

                string isoDateTime = newValue.ToString("yyyy-MM-dd HH:mm:ss"); // converting an object to primitive value in irder ti enter it ti the database 
                command.Parameters.AddWithValue("@taskId", taskId);
                command.Parameters.AddWithValue("@Val", isoDateTime);
                command.Parameters.AddWithValue("@BoardId", boardId);
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
        private TaskDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            // convert from string in de DB into DateTime Object
            DateTime creationTime = DateTime.Parse(reader.GetString(2));
            DateTime dueDate = DateTime.Parse(reader.GetString(3));

            return new TaskDAO(reader.GetInt32(0), reader.GetInt32(1)
                ,creationTime,dueDate,reader.GetString(4),reader.GetString(5),reader.GetInt32(6),reader.GetString(7));
        }

       
    }
}

