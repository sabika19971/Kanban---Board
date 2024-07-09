using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataExcessLayer
{
    internal class UserDAO
    {
        //  --------- name of fields ------------------//
        internal string Email { get; set; } // maybe should be public
        private string password;
        internal string Password { 
            get => password;
            set
            {
                if (isPersistent) { 
                    UserController.Update(Email, PasswordColumnName, value); // (id, column, newValue)
                }
                password = value;
            }
        }
        // ----------- name of columns ----------------// 
        internal string EmailColumnName = "Email";
        internal string PasswordColumnName = "Password";

        // ---------- field for insert method ----------//
        internal bool isPersistent = false;
        // ----------- the controller ------------------// 
        private UserController UserController  { get; set; }



        internal UserDAO(string email, string password)
        {
            UserController = new UserController();
            Email = email;
            Password = password;
        }

        public void persist()
        {
            UserController.Insert(this);
            isPersistent = true;  
        }
        //UserController.Delete(Email); delete check
        //UserController.Update(Email,"Password","Kk1!"); update check


    }
}

