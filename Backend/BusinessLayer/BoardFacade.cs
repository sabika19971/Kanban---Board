using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardFacade
    {
        private Dictionary<string, List<BoardBl>> boards;
        private Autentication aut;

        

        public BoardFacade(Autentication aut)
        {
            boards = new Dictionary<string, List<BoardBl>>();
            this.aut = aut;
        }

        public void resetBoards ( string email)
        {
            Console.WriteLine("im in reset Boards");
            boards.Add(email, new List<BoardBl>());
            Console.WriteLine(boards.Count);
        }
        public List<BoardBl> boardList(string email)
        {
            if (boards.ContainsKey(email))
            {
                return boards[email];
            }
            return null;
        }

        

        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>A BoardBl, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public BoardBl CreateBoard(string email, string name)
        {
            Console.WriteLine(boards.Count);
            if(boards.ContainsKey(email) && aut.isOnline(email)) 
                {
                    if(boards[email].Find(board => board.Name.Equals(name))==null) 
                         {
                            BoardBl boardToAdd = new BoardBl(name);
                            boards[email].Add(boardToAdd);
                            return boardToAdd;
                         }
                     else
                        {
                            throw new Exception("board name already exist");
                        }
                
                }
            else
            {
                throw new Exception("user email is not register to the system or is not logged in ");
            }

        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public BoardBl DeleteBoard(string email, string name)
        {
            if (boards.ContainsKey(email))
            {
                BoardBl boardToDelete = boards[email].Find(board => board.Name.Equals(name));
                if(boardToDelete != null)
                {
                    
                    boards[email].Remove(boardToDelete);
                    return null;
                }
                else
                {
                    throw new Exception("there is no such a board");
                }

            }
            else
            {
                throw new Exception("cant delete board from a user that does not exist in the system");
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
        public bool LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            if (columnOrdinal < 0 || columnOrdinal >= 2)
            {
                throw new Exception("cant limit column that does not exist");

            }
            if (boards.ContainsKey(email))
            {
                
                BoardBl boardColumToLimit = boards[email].Find(board => board.Name == boardName);
                if(boardColumToLimit != null) 
                {
                    
                    boardColumToLimit.limitColumn(columnOrdinal, limit);
                    return true;
                }
                else
                {
                    throw new Exception("no such board");
                }
            }
            else
            {
                throw new Exception("cant limit column for a user that doesnot exist in the system");
            }
        }

        

        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public List<TaskBl> InProgressTasks(string email)
        {
            List<TaskBl> result = new List<TaskBl>();
            if (!boards.ContainsKey(email))
            {
                throw new InvalidOperationException("cant show tasks for a user that is not registered in the system");
            }
            List<BoardBl> boardsToCollectTasks = boards[email];
            foreach (var boardToCollect in boardsToCollectTasks)
            {
                foreach (var task in boardToCollect.getColumns(1).Tasks)
                {
                    result.Add(task);
                }
            }
            return result;
        }
        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
        // inside the BoardFacade
        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= 2)
            {
                throw new Exception("cant limit column that does not exist");
            }
            if (boards.ContainsKey(email))
            {
                BoardBl boardColumGetLimit = boards[email].Find(board => board.Name == boardName);
                if (boardColumGetLimit != null)
                {
                    return boardColumGetLimit.getColumns(columnOrdinal).MaxTasks;
                   
                    
                }
                else
                {
                    throw new Exception("no such board");
                }
            }
            else
            {
                throw new Exception("cant get limit column for a user that doesnot exist in the system");
            }

        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        // inside BoardFacade
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
        public List<TaskBl> GetColumn(string email, string boardName, int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= 2)
            {
                throw new Exception("cant limit column that does not exist");
            }
            List<TaskBl> result = new List<TaskBl>();
            if (!boards.ContainsKey(email))
            {
                throw new InvalidOperationException("cant show tasks for a user that is not registered in the system");
            }
            BoardBl boardToCollectTasks = boards[email].Find(board => board.Name == boardName);
            if (boardToCollectTasks != null)
            {
                foreach (var taskToCollect in boardToCollectTasks.getColumns(columnOrdinal).Tasks)
                {
                    result.Add(taskToCollect);
                }
            }
            else
            {
                throw new Exception("board does not exist");
            }

            return result;

        }
    }
}
