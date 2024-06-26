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
            this.aut = aut;
            Email = email;
            Password = password;         
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
