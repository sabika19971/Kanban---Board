using IntroSE.Kanban.Backend.DataExcessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserBl
    {
        private Autentication aut;
        private string userEmail;
        private string userPassword;
        private UserDAO userDAO;

        internal string Email
        {
            get { return userEmail; }
            set
            {
                if (aut.isValidEmail(value))
                {
                    userEmail = value;
                }
                else
                {                   
                    throw new ArgumentException("Email is not legal");
                }
            }
        }

        internal string Password
        {         
            set
            {
                if (aut.isValidPassword(value))
                {
                    userDAO.Password = value; // updating the DB
                    userPassword = value;
                }
                else
                {
                    throw new ArgumentException("Password is not legal");
                }
            }
        }

        internal UserBl(string email, string password, Autentication aut)
        {     
            userDAO = new UserDAO(email, password);
            userDAO.persist(); 

            this.aut = aut;
            Email = email;
            Password = password;         
        }

        internal UserBl(UserDAO user,Autentication aut)
        {
            userDAO = user;
            user.isPersistent = true;
            
            this.aut = aut;
            Email = user.Email;
            Password = user.Password;
        }

        internal void Login(string password)
        {
            if (userPassword == password)
            {
                aut.SetOnline(this);
            }
            else
            {
                throw new ArgumentException("Incorrect password");
            }
        }

        internal void Logout()
        {
            aut.Logout(this);
        }
    }
        
}
