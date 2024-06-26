using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    public class UserSL
    {
        public string Email{ get; }
        
        public UserSL(string email)
        {
            this.Email = email;          
        }

        internal UserSL (UserBl userBl)
        {
            this.Email = userBl.Email;         
        }
    }
}
