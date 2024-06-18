using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserFacade
        
    {
        private Autentication auto;
        private Dictionary<string, UserBl> userDictionary;

        public UserFacade()
        {
            this.auto = new Autentication();
            this.userDictionary = new Dictionary<string, UserBl>();
        }
        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public UserBl Register(string email, string password) // do we need to implement the exceptions inside the userBi so when he will come to create itself he will throw the exception.
        {
            if (userDictionary.ContainsKey(email))
            {
                throw new ArgumentException("email is already taken");
            }
            try
            {
                UserBl userBl = new UserBl(email, password);
                userDictionary.Add(email, userBl);
                return userBl;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
            
            
        }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public UserBl Login(string email, string password)
        {
            throw new NotImplementedException();
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
