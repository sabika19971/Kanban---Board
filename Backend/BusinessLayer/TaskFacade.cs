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


        public TaskFacade() {


           
            aut = new Autentication();
            boardFacade = new BoardFacade();

        }

        internal TaskBl UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            if ((columnOrdinal < 0 || columnOrdinal >= 2))
            {
                throw new Exception("cant edit done task");
            }

            List<BoardBl> boards = boardFacade.boardList(email);
            if (boards.Any())
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsEdit = boards.Find(board => board.Name.Equals(boardName));
            if (boardTaskIsEdit != null)
            {
                TaskBl taskToEdit = boardTaskIsEdit.getColumns(columnOrdinal).Tasks.Find(task => task.Id.Equals(taskId));
                if (taskToEdit != null)
                {
                    taskToEdit.Description = description;
                    return taskToEdit;
                }
                else
                {
                    throw new Exception("task does not exist ");
                }   
                
            }
            else
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }

        }

        internal TaskBl AddTask(string email, string boardName, string title, string description, DateTime dueDate) // to ask about the id generator.
        {

            if (aut.isOnline(email))
            {

                List<BoardBl> boards = boardFacade.boardList(email);
                if(boards.Any())
                {
                    throw new Exception("user have no boards");
                }
                BoardBl boardBeAdded = boards.Find(board => board.Name.Equals(boardName));
                if (boardBeAdded != null)
                {
                    TaskBl taskToAdd = new TaskBl(dueDate, title, description, boardName);
                   while (boardBeAdded.validTaskId(taskToAdd))
                    {
                        taskToAdd = new TaskBl(dueDate, title, description, boardName);
                    }

                    boardBeAdded.AddTask(taskToAdd);
                    return taskToAdd;
                }
                else
                {
                    throw new Exception("no board in this name, you must create a board first");
                }
                
            }
            else
            {
                throw new Exception("user must be logged in in order to add task");
            }


        }



        internal TaskBl UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            if ((columnOrdinal<0||columnOrdinal>=2))
            {
                throw new Exception("cant edit done task");
            }


            List<BoardBl> boards = boardFacade.boardList(email);
            if (boards.Any())
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsEdit = boards.Find(board => board.Name.Equals(boardName));
            if (boardTaskIsEdit != null)
            {
                TaskBl taskToEdit = boardTaskIsEdit.getColumns(columnOrdinal).Tasks.Find(task => task.Id.Equals(taskId));
                if (taskToEdit != null)
                {
                    taskToEdit.Title = title;
                    return taskToEdit;
                }
                else
                {
                    throw new Exception("task does not exist ");
                }

            }
            else
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }
        }

       internal TaskBl AdvanceTask(string email, string boardName, int columnOrdinal, int taskId) // do i get here the current column? 
        {
            if (columnOrdinal < 0 || columnOrdinal >= 2)
            {
                throw new Exception("cant advance done task or ilegal columnOrdinal ");
            }

            List<BoardBl> boards = boardFacade.boardList(email);
            if (boards.Any())
            {
                throw new ArgumentException("user dont have any boards");
            }
            BoardBl boardTaskBeAdvance = boards.Find(board => board.Name.Equals(boardName));
            if (boardTaskBeAdvance != null)
            {
                TaskBl taskToAdvance = boardTaskBeAdvance.getColumns(columnOrdinal).Tasks.Find(task => task.Id.Equals(taskId));
                if (taskToAdvance != null)
                {
                    taskToAdvance.ColumnOrdinal = columnOrdinal + 1;
                    boardTaskBeAdvance.AdvanceTask(taskToAdvance);
                    return taskToAdvance;
                }
                else
                {
                    throw new Exception("no such task");
                }
            }
            else
            {
                throw new Exception($" board '{boardName} does not exist'");
            }
                
          
        }

        internal TaskBl UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if ((columnOrdinal < 0 || columnOrdinal >= 2))
            {
                throw new Exception("cant edit done task");
            }


            List<BoardBl> boards = boardFacade.boardList(email);
            if (boards.Any())
            {
                throw new Exception("user have no boards");
            }
            BoardBl boardTaskIsEdit = boards.Find(board => board.Name.Equals(boardName));
            if (boardTaskIsEdit != null)
            {
                TaskBl taskToEdit = boardTaskIsEdit.getColumns(columnOrdinal).Tasks.Find(task => task.Id.Equals(taskId));
                if (taskToEdit != null)
                {
                    taskToEdit.DueDate = dueDate;
                    return taskToEdit;
                }
                else
                {
                    throw new Exception("task does not exist ");
                }

            }
            else
            {
                throw new Exception($"board '{boardName}' does not exist ");
            }
        }
    }
}
