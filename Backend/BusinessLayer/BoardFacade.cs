﻿using EllipticCurve.Utils;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardFacade
    {
        private Dictionary<string, List<BoardBl>> boards;
        private Autentication aut;
        private long idGen;
        private BoardController boardController;
        private UserBoardController UBC;
        private bool loadBoards = false;

        internal BoardFacade(Autentication aut)
        {
            boards = new Dictionary<string, List<BoardBl>>();
            this.boardController = new BoardController();
            this.aut = aut;
        }

        internal void LoadBoards()
        {
            List<BoardDAO> boardDAOs = boardController.SelectAllBoards();
            foreach (var boardDAO in boardDAOs)
            {
                if ( !this.boards.ContainsKey(boardDAO.Owner))
                {
                    resetBoards(boardDAO.Owner); 
                }
                BoardBl boardToLoad = new BoardBl(boardDAO);
                this.boards[boardDAO.Owner].Add(boardToLoad);
                Console.WriteLine("loaded " + boardDAO.Name + " used by user " + boardDAO.Owner + " from the DB");
            }
            getHighestId();
            loadBoards = true;
        }

        internal void DeleteBoards()
        {
            ColumnController columnController = new ColumnController();
            UserBoardController userBoardController = new UserBoardController();
            columnController.DeleteAllColumns();
            userBoardController.DeleteAllConnections();
            boardController.DeleteAllBoards();
        }
            
        
        internal void resetBoards (string email)
        {          
            boards.Add(email, new List<BoardBl>());           
        }

        internal List<BoardBl> boardList(string email)
        {
            if (boards.ContainsKey(email))
            {
                return boards[email];
            }
            return null;
        }
       
        internal BoardBl findBoard(string name)
        {
            foreach (var kvp in boards)
            {
                foreach (var board in kvp.Value)
                {
                    if (board.Name == name)
                    {
                        return board;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>A BoardBl, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        internal BoardBl CreateBoard(string email, string name)
        {
            if (string.IsNullOrEmpty(email)  || string.IsNullOrEmpty(name))
            {
                throw new Exception("Cannot create board with null or blank arguments");
            }
            email = email.ToLower();
            if ( !(boards.ContainsKey(email) && aut.isOnline(email)) )
            {
                throw new Exception("user email is not registered to the system or is not logged in");
            }
            if (boards[email].Find(board => board.Name.Equals(name)) != null) 
            {
                throw new Exception("board name already exist, cant create two baords with the same name");    
            }
            BoardBl boardToAdd = new BoardBl(getIdGen(),name,email);
            boards[email].Add(boardToAdd);
            return boardToAdd;                     
        }


        private long getIdGen()
        {
            idGen++;
            return idGen;
        }
        internal void getHighestId() // this function is to adjust the board idGen when uploading the boards from DB 
        {
            long maxId = 0;
            foreach (var user in boards.Values)
            {
                foreach (BoardBl boardOfUser in user)
                {
                    if (boardOfUser.getId() > maxId)
                    {
                        maxId = boardOfUser.getId();
                    }
                }

            }
            idGen = maxId;
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns> null, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal BoardBl DeleteBoard(string email, string name)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant delete board");
            }
            email = email.ToLower();
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
            boardToDelete.delete();
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
        
        internal bool LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("cant limit column that doesnt exist");
            }
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant limit column");
            }
            email = email.ToLower();
            if ( !aut.isOnline(email))
            {
                throw new Exception("user must be logged in");
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
        internal List<TaskBl> InProgressTasks(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant get in progress tasks");
            }
            email = email.ToLower();
            List<TaskBl> result = new List<TaskBl>();
            if ( !aut.isOnline(email) )
            {
                throw new Exception("user is not logged in");
            }
            foreach (var kvp in boards)
            {
                foreach (var board in kvp.Value)
                {
                    List<TaskBl> tempList = board.getTasksOf(email);
                    if (tempList != null)
                    {
                        result.AddRange(tempList);    // combines the lists                    
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
        internal int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant get in progress tasks");
            }
            email = email.ToLower();
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

        internal string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant get column name");
            }
            email = email.ToLower();
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

        internal List<TaskBl> GetColumn(string email, string boardName, int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal > 2)
            {
                throw new Exception("cant get column that does not exist");
            }
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant get column");
            }
            email = email.ToLower();
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









        // FROM HERE NEW FUNCTIONS











        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        internal List<long> GetUserBoards(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant get user boards");
            }
            email = email.ToLower();
            if (!aut.isOnline(email))
            {
                throw new Exception("user must be logged in");
            }
            if (!boards.ContainsKey(email))
            {
                throw new Exception("User does not exist");
            }
            List<long> ids = new List<long>();

            foreach (var keyAndValue in boards)
            {
                List<BoardBl> List = keyAndValue.Value;
                foreach (BoardBl board in List)
                {
                    if (board.Members.Contains(email) || board.Owner == email)
                    {
                        ids.Add(board.getId());
                    }
                }
            }
            return ids;
        }




        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void JoinBoard(string email, int boardID)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant join a board");
            }
            email = email.ToLower();
            if (!aut.isOnline(email))
            {
                throw new Exception("user must be logged in");
            }
            if (boardID.Equals(null))
            {
                throw new Exception("illegal board id");
            }

            foreach (KeyValuePair< string, List<BoardBl>> kvp in boards)
            {
                foreach(BoardBl boardBl in kvp.Value)
                {
                    if(boardBl.getId() == boardID)
                    {
                        boardBl.addMemberToBoard(email);
                        return;
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
        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            if (string.IsNullOrEmpty(currentOwnerEmail) || string.IsNullOrEmpty(newOwnerEmail))
            {
                throw new Exception("Email is null or empty, cant transfer ownership");
            }
            currentOwnerEmail = currentOwnerEmail.ToLower();
            newOwnerEmail = newOwnerEmail.ToLower();
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
            boardToChangeOwner.changeOwner(currentOwnerEmail, newOwnerEmail); // updates the DB and updates members / owner on RAM
            boards[currentOwnerEmail].Remove(boardToChangeOwner); // switching the board owner in boards Dict RAM
            boards[newOwnerEmail].Add(boardToChangeOwner); // switching the board owner in boards Dict RAM
        }


        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal string GetBoardName(int boardId)
        {
            if (boardId <= 0)
            {
                throw new ArgumentException("Invalid board ID");
            }
            BoardBl boardOfName = null;
            foreach (var keyAndValue in boards)
            {
                List<BoardBl> List = keyAndValue.Value; 
                foreach (BoardBl board in List)
                {
                    if (board.getId() == boardId)
                    {
                        boardOfName = board;
                    }
                }
            }
            if (boardOfName == null)
            {
                throw new ArgumentException("No board with this ID");
            }
            return boardOfName.Name;          
        }


        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal void LeaveBoard(string email, int boardID)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant leave a board");
            }
            email = email.ToLower();
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
                    if (boardBl.getId() == boardID)
                    {
                        if(boardBl.Owner == email)
                        {
                            throw new Exception("Owner can leave the board only after transfering ownershoip");
                        }
                        boardBl.leaveBoard(email);
                        return;
                    }
                }
            }
            throw new Exception("there is no such board with this id");
        }


    }
}
