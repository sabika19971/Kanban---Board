using System;
using System.Collections;
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
        
        public void resetBoards (string email)
        {          
            boards.Add(email, new List<BoardBl>());           
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
            if (String.IsNullOrEmpty(email)  || String.IsNullOrEmpty(name))
            {
                throw new Exception("Cannot create board with null or blank arguments");
            }
            if ( !(boards.ContainsKey(email) && aut.isOnline(email)) )
            {
                throw new Exception("user email is not registered to the system or is not logged in");
            }
            if (boards[email].Find(board => board.Name.Equals(name)) != null) 
            {
                throw new Exception("board name already exist, cant create two baords with the same name");    
            }
            BoardBl boardToAdd = new BoardBl(name,email);
            boards[email].Add(boardToAdd);
            return boardToAdd;                     
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns> null, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public BoardBl DeleteBoard(string email, string name)
        {
            if (!(boards.ContainsKey(email)))
            {
                throw new Exception("cant delete board from a user that does not exist in the system");
            }
            BoardBl boardToDelete = boards[email].Find(board => board.Name.Equals(name));
            if (boardToDelete == null)
            {
                throw new Exception("there is no such a board");
            }
            if(boardToDelete.Owner != email)
            {
                throw new Exception("only owner can delete the board");
            }
            boards[email].Remove(boardToDelete);
            return null;          
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns> true, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        public bool LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("cant limit column that does not exist");
            }
            if ( !(boards.ContainsKey(email)) )
            {
                throw new Exception("cant limit column for a user that does not exist in the system");
            }
            BoardBl boardColumToLimit = boards[email].Find(board => board.Name == boardName);
            if (boardColumToLimit == null)
            {
                throw new Exception("no such board");
            }
            boardColumToLimit.limitColumn(columnOrdinal, limit);
            return true;                              
        }

        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns> A list of taskBl of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public List<TaskBl> InProgressTasks(string email)
        {
            List<TaskBl> result = new List<TaskBl>();
            if ( !boards.ContainsKey(email) )
            {
                throw new InvalidOperationException("cant show tasks for a user that is not registered in the system");
            }
            if ( !aut.isOnline(email) )
            {
                throw new Exception("user is not logged in");
            }
            List<BoardBl> boardsToCollectTasks = boards[email]; 
            foreach (var boardToCollect in boardsToCollectTasks)
            {
                foreach (var task in boardToCollect.getColumns(1).Tasks())
                {
                    if (task.allowToEditTask(email))
                    {
                        result.Add(task);
                    }
                   
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
        /// <returns> int columLimit, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            if ( !aut.isOnline(email) ) 
            { 
                throw new Exception("user must be logged in"); 
            }
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("cant limit column that does not exist");
            }
            if ( !boards.ContainsKey(email) )
            {
                throw new Exception("cant get limit column for a user that doesnot exist in the system");
            }
            BoardBl boardColumGetLimit = boards[email].Find(board => board.Name == boardName);
            if (boardColumGetLimit == null)
            {
                throw new Exception("no such board");
            }
            return boardColumGetLimit.getColumns(columnOrdinal).MaxTasks;                                                         
        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> string column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            if (!aut.isOnline(email)) 
            { 
                throw new Exception("user must be logged in"); 
            }
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("cant get columnName that does not exist");
            }
            string nameS="";
            if (!boards.ContainsKey(email))
            {
                throw new Exception("No Such User");
            }
            BoardBl wantedboard = boards[email].Find(board => board.Name.Equals(boardName));
            if (wantedboard == null) 
            {
                throw new Exception("no such board with this name");
            }
            if (columnOrdinal == 0) { nameS = "backlog"; }
            if (columnOrdinal == 1) { nameS = "in progress"; }
            if (columnOrdinal == 2) { nameS = "done"; }        
            return nameS;           
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> list of taskBl, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public List<TaskBl> GetColumn(string email, string boardName, int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("cant get column that does not exist");
            }
            if (!aut.isOnline(email))
            {
                throw new Exception("user must be logged in");
            }
            if (!boards.ContainsKey(email))
            {
                throw new InvalidOperationException("cant show tasks for a user that is not registered in the system");
            }
            BoardBl boardToCollectTasks = boards[email].Find(board => board.Name == boardName);
            if (boardToCollectTasks == null)
            {
                throw new Exception("board does not exist");
            }          
            List<TaskBl> result = new List<TaskBl>(boardToCollectTasks.getColumns(columnOrdinal).Tasks());           
            return result;
        }
        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void JoinBoard(string email, int boardID)
        {
            if (!aut.isOnline(email))
            {
                throw new Exception("user must be logged in");
            }
            if (boardID.Equals(null))
            {
                throw new Exception("ilegal board id");
            }

            foreach (KeyValuePair< string, List<BoardBl>> kvp in boards)
            {
                foreach(BoardBl boardBl in kvp.Value)
                {
                    if(boardBl.Id == boardID)
                    {
                        boardBl.Members.Add(email);
                    }
                }
            }
            throw new Exception("there is no such board with this id");
        }


        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            if (!aut.isOnline(currentOwnerEmail))
            {
                throw new Exception("user must be logged in");
            }
            if (!boards.ContainsKey(currentOwnerEmail))
            {
                throw new InvalidOperationException("cant show tasks for a user that is not registered in the system");
            }
            BoardBl boardToChangeOwner = boards[currentOwnerEmail].Find(board => board.Name == boardName);
            if ( boardToChangeOwner== null)
            {
                throw new Exception("user does not have a board in this name");
            }
            boardToChangeOwner.Owner = newOwnerEmail;
           

        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void LeaveBoard(string email, int boardID)
        {

            if (!aut.isOnline(email))
            {
                throw new Exception("user must be logged in");
            }
            if (boardID.Equals(null))
            {
                throw new Exception("ilegal board id");
            }

            foreach (KeyValuePair<string, List<BoardBl>> kvp in boards)
            {
                foreach (BoardBl boardBl in kvp.Value)
                {
                    if (boardBl.Id == boardID)
                    {
                        if(boardBl.Owner == email)
                        {
                            throw new Exception("owner can leave the board only after transfering ownershoip");
                        }
                        boardBl.leaveBoard(email);
                    }
                }
            }
            throw new Exception("there is no such board with this id");
        }
    }
}
