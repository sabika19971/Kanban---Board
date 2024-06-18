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
        public UserBl(string email,string password)
        {
            this.aut = new Autentication();
        }
        private string email;

        public string Email
        {
            get { return email; }
            set
            {
                if (aut.isValidEmail(email))

                {
                    this.email = value;
                }
                else
                {
                    throw new ArgumentException("email is not legal");
                }

            }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                if (aut.isValidPassword(password))
                {
                    password = value;

                }
                else
                {
                    throw new ArgumentException("password is not legal");
                }
            }
        }

        internal void Login(string password)
        {
            if(this.password == password)
            {
                aut.SetOnline(this);
            }
            else
            {
                throw new ArgumentException("incorrect password"); 
            }
        }

        internal void Logout(string email)
        {
            aut.Logout(this);
        }
    }
        
}
