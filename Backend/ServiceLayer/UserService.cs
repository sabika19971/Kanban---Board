using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer; // get an access to the classes inside BusinessLayer Folder.


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    
    internal class UserService
    {

        
        private UserFacade Uf;

        public UserService()
        {
            this.Uf=new UserFacade();
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
                UserBl userBl= Uf.Register(email, password);
                UserSL usl = new UserSL(userBl);
                string response = JsonConvert.SerializeObject(new Response(usl,null));
                return response; 
            }
            catch (Exception ex) {
            {
                    string response = JsonConvert.SerializeObject(new Response(null, ex.Message));
                    /* can't register because of : 
                     * email already exist
                     * invalid email
                     * invalid password
                     * */
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
                    UserBl userBl = Uf.Login(email, password);
                    UserSL usl = new UserSL(userBl);
                    string response = JsonConvert.SerializeObject(new Response(usl, null));
                    return response;
                }
                catch (Exception ex)
                {

                    string response = JsonConvert.SerializeObject(new Response(null, "wrong username or password"));
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
            throw new NotImplementedException();
        }







    }
}
