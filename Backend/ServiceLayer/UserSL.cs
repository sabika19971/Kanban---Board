using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserSL
    {
        private string email;
        

        public UserSL(string email)
        {
            this.email = email;
            
        }

        public UserSL (UserBl userBl)
        {
            this.email = userBl.Email;
           
        }

        public string Email
        {
            get { return email; }
        }

       
    }
}
