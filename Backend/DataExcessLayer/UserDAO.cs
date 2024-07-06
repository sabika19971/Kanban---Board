using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataExcessLayer
{
    internal class UserDAO
    {
        private string Email { get; set; }
        private string Password { get; set; }

        internal UserDAO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}

