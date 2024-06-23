using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer; // get an access to the classes inside BusinessLayer Folder.
using IntroSE.Kanban.Backend.ServiceLayer; 

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class BoardService
    {
        private BoardFacade bf;

        public BoardService(BoardFacade boardfacade) 
        {
            this.bf = boardfacade;
        }

        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string CreateBoard(string email, string name)
        {
            try
            {
                Console.WriteLine("trynig to create board");
                BoardBl boardbl = bf.CreateBoard(email,name);
                //BoardSL boardsl = new BoardSL(boardbl);
                string response = JsonSerializer.Serialize(new Response(null, null));
                return response;
            }
            catch (Exception ex) 
            {
                    Console.WriteLine(ex.Message);
                    string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                    return response;
            }
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteBoard(string email, string name)
        {
            try
            {
                BoardBl boardbl = bf.DeleteBoard(email,name);
                //BoardSL boardsl = new BoardSL(boardbl);
                string response = JsonSerializer.Serialize(new Response(null, null));
                return response;
            }
            catch (Exception ex)
            {
                    Console.WriteLine(ex.Message);
                    string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                    return response;
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try 
            { 
            bool boardbl = bf.LimitColumn(email, boardName, columnOrdinal, limit);
            string response = JsonSerializer.Serialize(new Response(null, null));
            return response;
            }
            catch (Exception ex) 
            {
                
                    string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                    return response;            
            }
        }

       
        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgressTasks(string email)
        {
            try
            {
                List<TaskBl> inProgressTasks = bf.InProgressTasks(email);
                List<TaskSL> inProgressTasksSL = new List<TaskSL>();
                foreach (var task in inProgressTasks)
                {
                    TaskSL tasksl = new TaskSL(task);
                    inProgressTasksSL.Add(tasksl);
                }
                string response = JsonSerializer.Serialize(new Response(inProgressTasksSL , null));
                return response;
            }
            catch (Exception ex)
            {
            string response = JsonSerializer.Serialize(new Response(null, ex.Message));
            return response;
            }
        }

    public string GetColumnLimit(string email, string boardName, int columnOrdinal) 
        {
            try 
            { 
            int columnlimit = bf.GetColumnLimit(email, boardName, columnOrdinal);
            if(columnlimit == -1) 
            {
                    
                string response = JsonSerializer.Serialize(new Response("column have no limit" , null));
                    return response;
            }
            else
                {
                   
                string response = JsonSerializer.Serialize(new Response(columnlimit , null));
                 return response;
               }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                 string response = JsonSerializer.Serialize(new Response(null , ex.Message));
                return response;
            }
        }     





    public string GetColumnName(string email, string boardName, int columnOrdinal) 
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
       // inside BoardFacade

    public string GetColumn(string email, string boardName, int columnOrdinal) 
        {
            try 
            {
                List<TaskBl> tasksbl = bf.GetColumn(email, boardName, columnOrdinal);
                List<TaskSL> tasksSL = new List<TaskSL>();
                foreach(var task in tasksbl) 
                {
                    TaskSL tasksl = new TaskSL(task);
                    tasksSL.Add(tasksl);
                }
                string response = JsonSerializer.Serialize(new Response(tasksSL , null));
                return response;
            }
            catch (Exception ex)
            {
            string response = JsonSerializer.Serialize(new Response(null, ex.Message));
            return response;
            }            
        }


    }
}
