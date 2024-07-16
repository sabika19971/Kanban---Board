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
    internal class ColumnController
    {
        private readonly string _connectionString; 
        private readonly string _tableName;
        private const string TableName = "Columns";
        private string dbFileName = "kanban.db";
        private string solutionDirectory = Path.GetFullPath(Directory.GetCurrentDirectory());

        internal ColumnController() // init and connecting to the DB
        {
            string path = Path.Combine(solutionDirectory, dbFileName);
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TableName;
        }

        internal bool Insert(ColumnDAO col)
        {
            int result = -1;
            using (var connection = new SQLiteConnection(this._connectionString)) 
            {
                try
                {
                    connection.Open(); 
                    SQLiteCommand command = new SQLiteCommand(null, connection); 
                    string insert = $"INSERT INTO {TableName} ({col.idColumnName},{col.boardIdColumnName},{col.maxTasksColumnName},{col.currTaskColumnName}) Values (@id,@boardId,@maxTasks,@currTask)"; // the @ is a place holders to avoid SQL injection                   
                    SQLiteParameter id = new SQLiteParameter(@"id", col.Id); 
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardId", col.BoardId); 
                    SQLiteParameter maxTasksParam = new SQLiteParameter(@"maxTasks", col.MaxTasks); 
                    SQLiteParameter currTaskParam = new SQLiteParameter(@"currTask", col.CurrTask); 
                    command.CommandText = insert;  
                    command.Parameters.Add(id); 
                    command.Parameters.Add(boardIdParam); 
                    command.Parameters.Add(maxTasksParam); 
                    command.Parameters.Add(currTaskParam); 

                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Column insertion to the DB has failed"); 
                }
                return result > 0; 
            }
        }

        internal bool Update(int id,long boradId, string column, int newValue)
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
                    throw new Exception("Failed to update a Cloumn int he DB");
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
                command.CommandText = $"DELETE from {TableName} where Id=@Id AND BoardId=@BoardId;";
                command.Parameters.AddWithValue("@BoardId", boardId);
                command.Parameters.AddWithValue("@Id", Id);
                try
                {
                    connection.Open(); 
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to delete a column from the DB");
                }
            }
            return res > 0;
        }

        internal bool DeleteAllColumns()
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
                    throw new Exception("Failed to delete columns from the DB");
                }
            }
            return res > 0;
        }


        internal ColumnDAO Select(int id, long boardId)  
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
                    throw new Exception("Failed to load a column from the DB");
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

        internal List<ColumnDAO> SelectAllColumns() 
        {
            List<ColumnDAO> columns = new List<ColumnDAO>();
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
                        columns.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load columns from the DB");
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

