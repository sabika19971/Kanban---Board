using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EllipticCurve.Utils;
using IntroSE.Kanban.Backend.BusinessLayer; // get an access to the classes inside BusinessLayer Folder.
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private BoardFacade bf;
        private ILog log;

        internal BoardService(BoardFacade boardfacade, ILog log) 
        {
            this.log = log;
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
                BoardBl boardbl = bf.CreateBoard(email,name);
                string response = JsonSerializer.Serialize(new Response(null, null));
                log.Info(email + " has created a board");
                return response;
            }
            catch (Exception ex) 
            {                
                string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                log.Warn(email + " : " + ex.Message + " when trying to create a board" );
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
                string response = JsonSerializer.Serialize(new Response(null, null));
                log.Info(email + " has deleted a board a board");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                log.Warn(email + " : " + ex.Message + " when trying to delete a board");
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
                log.Info(email + " has limited a column");
                return response;
            }
            catch (Exception ex) 
            {                
                string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                log.Warn(email + " : " + ex.Message + " when trying to limit a column");
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
                string response = JsonSerializer.Serialize(new Response(inProgressTasksSL, null));
                log.Info(email + " got in progress tasks");
                return response;
            }
            catch (Exception ex)
            {
                string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                log.Warn(email + " : " + ex.Message + " when trying to get inprogress tasks");
                return response;
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
        // inside the BoardFacade

        public string GetColumnLimit(string email, string boardName, int columnOrdinal) 
        {
            try 
            { 
                int columnlimit = bf.GetColumnLimit(email, boardName, columnOrdinal);                                        
                string response = JsonSerializer.Serialize(new Response(((object)columnlimit),null));
                log.Info(email + " got column limit");
                return response;           
            }
            catch (Exception ex)
            {              
                string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                log.Warn(email + " : " + ex.Message + " when trying to get colum limit");
                return response;
            }
        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                string columnname = bf.GetColumnName(email, boardName, columnOrdinal);
                string response = JsonSerializer.Serialize(new Response(columnname, null));
                log.Info(email + " got column name");
                return response;
            }
            catch (Exception ex)
            {
                string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                log.Warn(email + " : " + ex.Message + " when trying to get colum name");
                return response;
            }
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
                string response = JsonSerializer.Serialize(new Response(tasksSL, null));
                log.Info(email + " got column");
                return response;
            }
            catch (Exception ex)
            {
                string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                log.Warn(email + " : " + ex.Message + " when trying to get column");
                return response;
            }            
        }


        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string JoinBoard(string email, int boardID)
        {
            try
            {
                bf.JoinBoard(email, boardID);
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
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                bf.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
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
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                bf.LeaveBoard(email, boardID);
                string response = JsonSerializer.Serialize(new Response(null, null));
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
