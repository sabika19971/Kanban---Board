using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer; // get an access to the classes inside BusinessLayer Folder.
namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private TaskFacade taskFacade;

        internal TaskService( TaskFacade taskFacade)
        {
            this.taskFacade = taskFacade;
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                TaskBl taskBL= taskFacade.AddTask(email, boardName, title, description, dueDate);
                TaskSL taskSL = new TaskSL(taskBL);
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
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {              
                TaskBl taskBl = taskFacade.UpdateTaskDescription(email,boardName, columnOrdinal, taskId, description);
                TaskSL taskAfterEdit = new TaskSL(taskBl);
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
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                TaskBl taskBl = taskFacade.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
                TaskSL taskAfterEdit = new TaskSL(taskBl);
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
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                TaskBl taskBl = taskFacade.AdvanceTask(email, boardName, columnOrdinal, taskId);
                TaskSL taskAfterEdit = new TaskSL(taskBl);
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
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        // inside TaskFacade
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                TaskBl taskBl = taskFacade.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId , dueDate);
                TaskSL taskAfterEdit = new TaskSL(taskBl);
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
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                TaskBl taskBl = taskFacade.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                TaskSL taskAfterAssigne = new TaskSL(taskBl);
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
