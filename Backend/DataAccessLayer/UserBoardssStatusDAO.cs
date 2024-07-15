using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserBoardssStatusDAO
    {
  
        // ----------- name of columns ----------------//
        internal string EmailColumnName = "Email";
        internal string idColumnName = "Id";
        internal string statusColumnName = "Status";

        // ---------- field for insert method ----------//
        internal bool isPersistent = false;

        // ----------- the controller ------------------// 
        private UserBoardController controller;

        //  --------- getters & setters --------------
        internal int BoardId { get;  set; }
        internal string Email { get; set; } // we dont use the setter
        internal int Status { get;  set; } // 0 - for member, 1 - for owner

        internal UserBoardssStatusDAO(string email,int id, int status)
        {
            controller = new UserBoardController(); // inisialized the controller                                                                
            this.Email = email;
            this.BoardId = id;
            this.Status = status;   
        }

        internal UserBoardssStatusDAO(string email, int id)
        {
             controller = new UserBoardController(); // inisialized the controller

            this.Email = email;
            this.BoardId = id;
        }

        internal void persist()
        {
            controller.Insert(this);
            isPersistent = true;           
        }

        internal void deleteBoard() // used to delete all connections to a board
        {
            if (isPersistent)
            {
                controller.DeleteBoard(BoardId);
            }
        }

        internal void deleteFake() // used for removing a member
        {
            controller.Delete(Email, BoardId);
        }

        internal List<string> LoadMembers() 
        {
            List<UserBoardssStatusDAO> connections = controller.LoadMembers(BoardId);
            List<string> members = new List<string>();
            foreach (var connection in connections)
            {
                if (connection.Email != this.Email)
                {
                    members.Add(connection.Email);
                }
            }
            return members;
        }

        internal void changeOwner(string currentOwnerEmail, string newOwnerEmail)
        {
            controller.UpdateOwnership(BoardId, currentOwnerEmail, newOwnerEmail);        
            /*
            controller.Delete(newOwnerEmail, BoardId);
            controller.Update(BoardId,currentOwnerEmail, EmailColumnName, newOwnerEmail);
            */
            Email = newOwnerEmail;
        }
    }
}
