using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Autentication
    {
        private HashSet<UserBl> users;

        public Autentication() { 
            users = new HashSet<UserBl>();
        }
        internal  bool isValidEmail(string email) // can we do it static ? 
        {
            string pat = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
            Regex reg = new Regex(pat);
            return reg.IsMatch(email);
        }

        internal bool isValidPassword(string password)
        {
            string pat = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,20}$";
            Regex reg = new Regex(pat);
            return reg.IsMatch(password);
        }

        internal void Logout(UserBl userBl)
        {
            if (users.Contains(userBl))
            {
                users.Remove(userBl);
            }
            else {
                throw new ArgumentException("user is not logedIn");
            }
        }

        internal void SetOnline(UserBl userBl)
        {

            this.users.Add(userBl);
        }

    }
}
