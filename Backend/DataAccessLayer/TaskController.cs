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
    internal class TaskController
    {
        private readonly string _connectionString; 
        private readonly string _tableName;
        private const string TableName = "Tasks";
        private string dbFileName = "kanban.db";
        private string solutionDirectory = Path.GetFullPath(Directory.GetCurrentDirectory());

        internal TaskController() // init and connecting to the DB
        {
            string path = Path.Combine(solutionDirectory, dbFileName);
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        internal bool Insert(TaskDAO task)
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                try
                {
                    connection.Open();
                    string insert = $"INSERT INTO {TableName} ({task.idColumnName}, {task.BoardIdColumnName}, {task.ColumnOrdinalColumnName}, {task.TitleColumnName}, {task.DescriptionColumnName}, {task.CreationTimeColumnName}, {task.DueDateColumnName}, {task.AssigneeColumnName}) VALUES (@idTaskVal, @boardIdVal, @columnOrdinalVal, @titleVal, @descriptionVal, @creationTimeVal, @dueDateVal, @assigneeVal)";
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
                    throw new Exception("Task insertion to the DB has failed");
                }
                return result > 0;
            }
        }



        internal bool Update(int taskId,long boardId, string column, string newValue)
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
                    throw new Exception("Failed to update task in the DB");
                }
            }
            return res > 0;
        }

        internal bool UpdateColumnOrdinal(int taskId, string column, int newValue)
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
                    throw new Exception("Failed to update column ordinal of a task in the DB");
                }
            }
            return res > 0;
        }


        internal bool Delete(int Id,long boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE from {TableName} where Id=@Id AND BoardId=@boardId ;";
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@boardId", boardId);
                try
                {
                    connection.Open(); 
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to delete a task from the DB");
                }
            }
            return res > 0;
        }

        internal bool DeleteAllTasks()
        {
            int res = -1;
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE from {TableName} ;";
                try
                {
                    connection.Open(); 
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to delete tasks from the DB");
                }
            }
            return res > 0;
        }


        internal List<TaskDAO> SelectTasks(long boardId, int columnOrdinal)  
        {
            List<TaskDAO> tasks = new List<TaskDAO>();
            using (var connection = new SQLiteConnection(this._connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TableName} where BoardId=@BoardIdVal AND ColumnOrdinal=@ColumnOrdinalVal;";
                command.Parameters.AddWithValue("@BoardIdVal", boardId);
                command.Parameters.AddWithValue("@ColumnOrdinalVal", columnOrdinal);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();  
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        tasks.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load tasks from the DB");
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


        internal List<TaskDAO> SelectAllTasks() 
        {
            List<TaskDAO> tasks = new List<TaskDAO>();
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
                        tasks.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load tasks from the DB");
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

        internal bool UpdateTaskDueDate(int taskId,long boardId, string column, DateTime newValue )
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{column}]=@Val where Id=@taskId AND BoardId=@BoardId"
                };

                string isoDateTime = newValue.ToString("yyyy-MM-dd HH:mm:ss"); 
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
                    throw new Exception("Failed to update task dueDate");
                }
            }
            return res > 0;
        }
        private TaskDAO ConvertReaderToObject(SQLiteDataReader reader)
        {
            // convert from string in de DB into DateTime Object
            DateTime creationTime = DateTime.Parse(reader.GetString(5));
            DateTime dueDate = DateTime.Parse(reader.GetString(6));
            string assignee = reader.IsDBNull(7) ? null : reader.GetString(7); // Checks if assignee is null or a string

            return new TaskDAO(reader.GetInt32(0), reader.GetInt32(1) , reader.GetInt32(2), reader.GetString(3), reader.GetString(4), creationTime,dueDate,assignee);
        }

       
    }
}

