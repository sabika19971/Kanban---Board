using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserBl
    {

        private Autentication aut;
        private string userEmail;
        private string userPassword;

        public string Email
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

        public string Password
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

        public UserBl(string email, string password)
        {
            try
            {
                aut = new Autentication(); // Initialize aut first
                Email = email;             // Use property setter to validate
                Password = password;
            }
            catch (Exception ex) {
                throw ex;
            }
           
            
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
