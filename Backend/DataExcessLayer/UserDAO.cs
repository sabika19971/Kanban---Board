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
        internal string EmailColumnName = "Email";
        internal string PasswordColumnName = "Password";
        private UserController UserController  { get; set; }
        internal bool isPersistent = false;



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

