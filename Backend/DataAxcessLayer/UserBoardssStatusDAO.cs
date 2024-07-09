using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAxcessLayer
{
    internal class UserBoardssStatusDAO
    {

        //  --------- name of fields ------------------//
        private string email;
        private int id;
        private int status; // 0 - for member, 1 - for owner

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
        internal string Email 
        { get => Email;
            set
            {
                if (!isPersistent)
                {
                    throw new InvalidOperationException("cant change email for boardUser that is not in the db");
                }
                Email = value;

            }
           
                
                
        }
        internal int Status { get;  set; }

        public UserBoardssStatusDAO(string email,int id, int status)
        {
             controller = new UserBoardController(); // inisialized the controller
                                                                   
            this.Email = email;
            this.BoardId = id;
            this.Status = status;   


        }

        public UserBoardssStatusDAO(string email, int id)
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

        internal void delete()
        {
            if (!isPersistent)
            {
                throw new Exception("cant delete an object that is not on the data base");
            }
            controller.Delete(email, id);
        }

        

        internal void changeOwner(string currentOwnerEmail, string newOwnerEmail)
        {
            controller.Delete(newOwnerEmail, BoardId);
            controller.Update(BoardId,currentOwnerEmail, EmailColumnName, newOwnerEmail);
            Email = newOwnerEmail;

        }
    }
}
