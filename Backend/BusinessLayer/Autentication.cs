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
        private HashSet<string> users;

        internal Autentication() { 
            users = new HashSet<string>();
        }

        internal bool isOnline(string email)
        {
            if (users.Contains(email))
            {
                return true;
            }
            return false;
        }

        internal  bool isValidEmail(string email) 
        {
            if (email == null) { return false; }
            string pat = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex reg = new Regex(pat);
            return reg.IsMatch(email);
        }

        internal bool isValidPassword(string password)
        {
            if (password == null) { return false; }

            string pat = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,20}$";
            Regex reg = new Regex(pat);
            return reg.IsMatch(password);
        }

        internal void Logout(UserBl userBl)
        {
            if (users.Contains(userBl.Email))
            {
                users.Remove(userBl.Email);
            }
            else 
            {
                throw new ArgumentException("user is not logedIn");
            }
        }

        internal void SetOnline(UserBl userBl)
        {
            this.users.Add(userBl.Email);
        }


    }
}
