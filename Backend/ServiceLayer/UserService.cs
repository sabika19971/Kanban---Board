using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer; // get an access to the classes inside BusinessLayer Folder.


namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class UserService
    {
        private UserFacade uf;

        internal UserService(UserFacade uf)
        {
            this.uf = uf;
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        public string Register(string email, string password)
        {
            try
            {               
                UserBl userBl = uf.Register(email, password);
                UserSL usl = new UserSL(userBl);
                string response = JsonSerializer.Serialize(new Response(null,null));
                return response;
            }
            catch (Exception ex)
            {
                string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                return response;               
            }
        }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        public string Login(string email, string password)
        {               
            try
            {               
                UserBl userBl = uf.Login(email, password);
                UserSL usl = new UserSL(userBl);               
                string response = JsonSerializer.Serialize(new Response(usl.Email, null));
                return response;
            }
            catch (Exception ex)
            {              
                string response = JsonSerializer.Serialize(new Response(null, ex.Message));
                return response;
            }
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        public string Logout(string email)
        {
            try
            {             
                uf.Logout(email);
                string response = JsonSerializer.Serialize(new Response(null, null));
                return response;                              
            }
            catch (Exception ex)
            {
                string response = JsonSerializer.Serialize(new Response(null, "logout failed"));
                return response;
            }
        }
    }
}