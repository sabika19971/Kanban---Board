using IntroSE.Kanban.Backend.DataAxcessLayer;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class TaskFacade
    {      
        private Autentication aut;
        private BoardFacade boardFacade;
        private UserFacade uf;
        private TaskController taskController;
        private bool loadTasks = false;

        public TaskFacade(Autentication aut, BoardFacade boardFacade, UserFacade userFacade) {
            this.aut = aut;
            this.boardFacade = boardFacade;
            this.uf = userFacade;
            this.taskController = new TaskController();
            
        }

        internal void DeleteTasks()
        {
            taskController.DeleteAllTasks();
        }


        /* TOO EXPENSIVE
        private void LoadTasks()
        {
            List<TaskDAO> taskDAOs = taskController.SelectAllTasks();
            List<TaskBl> taskBls = new List<TaskBl>();
            foreach (var taskDAO in taskDAOs)
            {
                taskBls.Add(new TaskBl(taskDAO));
            }
            boardFacade.LoadTasks(taskBls);
        }
        */


      

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns> TaskBl updatedTask, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        internal TaskBl UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            if (columnOrdinal < 0 || columnOrdinal >= 2)
            {
                throw new Exception("cant edit done task");
            }
            if ( !aut.isOnline(email) )
            {
                throw new Exception("user is not logged in");
            }
            BoardBl boardTaskIsEdit = boardFacade.findBoard(boardName); // new, to allow members access
            /*
            List<BoardBl> boards = boardFacade.boardList(email);
            if ( !boards.Any() )
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsEdit = boards.Find(board => board.Name.Equals(boardName));
            */
            if (boardTaskIsEdit == null)
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }
            TaskBl taskToEdit = boardTaskIsEdit.getColumns(columnOrdinal).Tasks().Find(task => task.Id.Equals(taskId));
            if (taskToEdit == null)
            {
                throw new Exception("task does not exist ");
            }
            if (!taskToEdit.allowToEditTask(email))
            {
                throw new Exception("only assingee can edit task");
            }
            taskToEdit.Description = description;
            return taskToEdit;                     
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns> taskBl added task, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        internal TaskBl AddTask(string email, string boardName, string title, string description, DateTime dueDate) 
        {

            if ( !aut.isOnline(email) )
            {
                throw new Exception("user must be logged in in order to add task");
            }
            BoardBl boardToBeAdded = boardFacade.findBoard(boardName); // new, to allow members access
            //List<BoardBl> boards = boardFacade.boardList(email);
            /* DOESNT ALLOW MEMBERS TO ADD TASK MAYBE NOT NESSECERY
            if( !boards.Any() )
            {
                throw new Exception("user have no boards");
            }
            */
            //BoardBl boardToBeAdded = boards.Find(board => board.Name.Equals(boardName));
            if (boardToBeAdded == null)
            {
                throw new Exception("no board with this name, a board must be created first");
            }
            if ( !(boardToBeAdded.isMember(email) || boardToBeAdded.Owner  == email) )
            {
                throw new Exception("must be a member in the board to add a task");
            }
            if ( !boardToBeAdded.canAddTask() ) // make sure we dont add to DB if RAM insertion isnt possible
            {
                throw new Exception("reached maximum of tasks in the first column");
            }
            TaskBl taskToAdd = new TaskBl(dueDate, title, description, boardToBeAdded.getNumOfAllTasks()+1 , boardToBeAdded.getId());
            boardToBeAdded.AddTask(taskToAdd);
            return taskToAdd;                                     
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns> TaskBl updatedTask, unless an error occurs (see <see cref="GradingService"/>)</returns>

        internal TaskBl UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            if (columnOrdinal<0 || columnOrdinal>=2)
            {
                throw new Exception("cant edit done task");
            }
            if ( !aut.isOnline(email) )
            {
                throw new Exception("user is not logged in");
            }
            BoardBl boardTaskIsEdit = boardFacade.findBoard(boardName); // new, to allow members access
            /*
            List<BoardBl> boards = boardFacade.boardList(email);
            if ( !boards.Any() )
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsEdit = boards.Find(board => board.Name.Equals(boardName));
            */
            if (boardTaskIsEdit == null)
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }
            TaskBl taskToEdit = boardTaskIsEdit.getColumns(columnOrdinal).Tasks().Find(task => task.Id.Equals(taskId));
            if (taskToEdit == null)
            {
                throw new Exception("task does not exist ");
            }
            if (!taskToEdit.allowToEditTask(email))
            {
                throw new Exception("only assingee can edit task");
            }
            taskToEdit.Title = title;
            return taskToEdit;           
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns> taskBl advancedTask, unless an error occurs (see <see cref="GradingService"/>)</returns>
    
        internal TaskBl AdvanceTask(string email, string boardName, int columnOrdinal, int taskId) 
        {
            if (columnOrdinal < 0 || columnOrdinal >= 2)
            {
                throw new Exception("cant advance done task or ilegal columnOrdinal");
            }
            if ( !aut.isOnline(email) )
            {
                throw new Exception("user is not logged in ");
            }
            BoardBl boardTaskBeAdvance = boardFacade.findBoard(boardName); // new, to allow members access
            /*
            List<BoardBl> boards = boardFacade.boardList(email);
            if ( !boards.Any() )
            {
                throw new ArgumentException("user dont have any boards");
            }
            BoardBl boardTaskBeAdvance = boards.Find(board => board.Name.Equals(boardName));
            */
            if (boardTaskBeAdvance == null)
            {
                throw new Exception($" board '{boardName} does not exist'");
            }
            
            TaskBl taskToAdvance = boardTaskBeAdvance.getColumns(columnOrdinal).Tasks().Find(task => task.Id.Equals(taskId));
            if (taskToAdvance == null)
            {
                throw new Exception("no such task");
            }
            if (!taskToAdvance.allowToEditTask(email))
            {
                throw new Exception("only assingee can advance task");
            }              
            taskToAdvance.ColumnOrdinal = columnOrdinal + 1;
            boardTaskBeAdvance.AdvanceTask(taskToAdvance);
            return taskToAdvance;                                               
        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns> TaskBl updatedTask, unless an error occurs (see <see cref="GradingService"/>)</returns>

        internal TaskBl UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (columnOrdinal < 0 || columnOrdinal >= 2)
            {
                throw new Exception("cant edit done task");
            }
            if ( !aut.isOnline(email) )
            {
                throw new Exception("user is not logged in");
            }
            BoardBl boardTaskIsEdit = boardFacade.findBoard(boardName); // new, to allow members access
            /*
            List<BoardBl> boards = boardFacade.boardList(email);
            if ( !boards.Any() )
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsEdit = boards.Find(board => board.Name.Equals(boardName));
            */
            if (boardTaskIsEdit == null)
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }
            TaskBl taskToEdit = boardTaskIsEdit.getColumns(columnOrdinal).Tasks().Find(task => task.Id.Equals(taskId));
            if (taskToEdit == null )
            {
                throw new Exception("task does not exist ");
            }
            if (!taskToEdit.allowToEditTask(email))
            {
                throw new Exception("only assingee can edit task");
            }
            taskToEdit.DueDate = dueDate;
            return taskToEdit;                 
        }

        internal TaskBl AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            if (!aut.isOnline(email))
            {
                throw new Exception("user is not logged in");
            }
            if (String.IsNullOrEmpty(emailAssignee))
            {
                throw new ArgumentException("cant assign a task to null or empty email");
            }
            BoardBl boardTaskIsAssingee = boardFacade.findBoard(boardName); // new, to allow members access
            /*
            List<BoardBl> boards = boardFacade.boardList(email);
            if (!boards.Any())
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsAssingee = boards.Find(board => board.Name.Equals(boardName));
            */
            if (boardTaskIsAssingee == null)
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }
            if ( (!boardTaskIsAssingee.isMember(emailAssignee) && boardTaskIsAssingee.Owner != emailAssignee) || (!boardTaskIsAssingee.isMember(email) && boardTaskIsAssingee.Owner != email) )
            {
                throw new Exception("cant assingee a user that is not a member of the board ");
            }
            TaskBl taskToAssisngee = boardTaskIsAssingee.getColumns(columnOrdinal).Tasks().Find(task => task.Id.Equals(taskID));
            if (taskToAssisngee == null)
            {
                throw new Exception("task does not exist ");
            }
            taskToAssisngee.AssignTask(emailAssignee, email);
            return taskToAssisngee;
        }
    }
}
