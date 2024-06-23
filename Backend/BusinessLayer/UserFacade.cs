using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserFacade

    {

        private Dictionary<string, UserBl> userDictionary;
        BoardFacade boardFacade;

        public UserFacade(BoardFacade board)
        {

            this.userDictionary = new Dictionary<string, UserBl>();
            this.boardFacade = board ;
        }
        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>UserBl, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public UserBl Register(string email, string password) // do we need to implement the exceptions inside the userBi so when he will come to create itself he will throw the exception.
        {
            
            if (userDictionary.ContainsKey(email))
            {
                throw new ArgumentException("email is already taken");
                Console.WriteLine("im in the exception :email is already taken ");
            }
            try
            {
                Console.WriteLine("trying to create new user ");
                UserBl userBl = new UserBl(email, password);
                userDictionary.Add(email, userBl);
                boardFacade.resetBoards(email);
                return userBl;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }



        }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>UserBl, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public UserBl Login(string email, string password)
        {
            if (!userDictionary.ContainsKey(email))
            {
                Console.WriteLine("im here");
                throw new ArgumentException($"{email} is not registered.");
            }
            try // to ask about the try catch. 
            {
                userDictionary[email].Login(password);
                return userDictionary[email];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns> void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public void Logout(string email)
        {
            if (!userDictionary.ContainsKey(email))
            {
                Console.WriteLine("im here");
                throw new ArgumentException("there is not such a user at all");
            }
            try
            {
                userDictionary[email].Logout();
                 
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

}
