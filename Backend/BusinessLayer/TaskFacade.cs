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

        public TaskFacade(Autentication aut, BoardFacade boardFacade, UserFacade userFacade) {
            this.aut = aut;
            this.boardFacade = boardFacade;
            this.uf = userFacade;

        }

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
            List<BoardBl> boards = boardFacade.boardList(email);
            if ( !boards.Any() )
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsEdit = boards.Find(board => board.Name.Equals(boardName));
            if (boardTaskIsEdit == null)
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }
            TaskBl taskToEdit = boardTaskIsEdit.getColumns(columnOrdinal).Tasks().Find(task => task.Id.Equals(taskId));
            if (taskToEdit == null)
            {
                throw new Exception("task does not exist ");
            }
            taskToEdit.Description = description;
            return taskToEdit;                     
        }

        internal TaskBl AddTask(string email, string boardName, string title, string description, DateTime dueDate) // to ask about the id generator.
        {

            if ( !aut.isOnline(email) )
            {
                throw new Exception("user must be logged in in order to add task");
            }
            List<BoardBl> boards = boardFacade.boardList(email);
            if( !boards.Any() )
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardToBeAdded = boards.Find(board => board.Name.Equals(boardName));
            if (boardToBeAdded == null)
            {
                throw new Exception("no board with this name, a board must be created first");
            }
            TaskBl taskToAdd = new TaskBl(dueDate, title, description, boardName, boardToBeAdded.getNumOfAllTasks()+1);
            boardToBeAdded.AddTask(taskToAdd);
            return taskToAdd;                                     
        }

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
            List<BoardBl> boards = boardFacade.boardList(email);
            if ( !boards.Any() )
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsEdit = boards.Find(board => board.Name.Equals(boardName));
            if (boardTaskIsEdit == null)
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }
            TaskBl taskToEdit = boardTaskIsEdit.getColumns(columnOrdinal).Tasks().Find(task => task.Id.Equals(taskId));
            if (taskToEdit == null)
            {
                throw new Exception("task does not exist ");
            }
            taskToEdit.Title = title;
            return taskToEdit;           
        }

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
            List<BoardBl> boards = boardFacade.boardList(email);
            if ( !boards.Any() )
            {
                throw new ArgumentException("user dont have any boards");
            }
            BoardBl boardTaskBeAdvance = boards.Find(board => board.Name.Equals(boardName));
            if (boardTaskBeAdvance == null)
            {
                throw new Exception($" board '{boardName} does not exist'");
            }
            TaskBl taskToAdvance = boardTaskBeAdvance.getColumns(columnOrdinal).Tasks().Find(task => task.Id.Equals(taskId));
            if (taskToAdvance == null)
            {
                throw new Exception("no such task");
            }
            taskToAdvance.ColumnOrdinal = columnOrdinal + 1;
            boardTaskBeAdvance.AdvanceTask(taskToAdvance);
            return taskToAdvance;                                               
        }

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
            List<BoardBl> boards = boardFacade.boardList(email);
            if ( !boards.Any() )
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsEdit = boards.Find(board => board.Name.Equals(boardName));
            if (boardTaskIsEdit == null)
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }
            TaskBl taskToEdit = boardTaskIsEdit.getColumns(columnOrdinal).Tasks().Find(task => task.Id.Equals(taskId));
            if (taskToEdit == null)
            {
                throw new Exception("task does not exist ");
            }
            taskToEdit.DueDate = dueDate;
            return taskToEdit;                 
        }
    }
}
