﻿using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserFacade
    {
        internal Dictionary<string, UserBl> userDictionary;
        internal BoardFacade boardFacade;
        internal UserController userController;
        internal Autentication aut;
        private bool loadUsers = false;
        

        internal UserFacade(BoardFacade board , Autentication aut)
        {
            this.userDictionary = new Dictionary<string, UserBl>();
            userController = new UserController();
            this.boardFacade = board ;
            this.aut = aut;
        }


        internal void LoadUsers()
        {
            if (loadUsers)
            {
                throw new Exception("users DB already loaded");
            }
            List<UserDAO> userDAOs = userController.SelectAllUsers();
            foreach (UserDAO user in userDAOs)
            {
                userDictionary.Add(user.Email,new UserBl(user,aut));
                boardFacade.resetBoards(user.Email);
                Console.WriteLine("loaded " + user.Email + " from the DB");
            }
            loadUsers = true;
        }

        internal void DeleteUsers()
        {
            userController.DeleteAllUsers();
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns> UserBl new user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal UserBl Register(string email, string password) 
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("email or pass cant be null or empty");
            }
            email = email.ToLower();
            if (userDictionary.ContainsKey(email)) 
            {
                throw new ArgumentException("email is already taken");
            }
            UserBl userBl = new UserBl(email, password, aut);
            userDictionary.Add(email, userBl);
            boardFacade.resetBoards(email);
            userBl.Login(password);
            return userBl;
        }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns> UserBl logged in user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        internal UserBl Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant login");
            }
            email = email.ToLower();
            if ( !userDictionary.ContainsKey(email) )
            {             
                throw new ArgumentException($"{email} is not registered.");
            }
            userDictionary[email].Login(password);
            return userDictionary[email];                   
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns> void, unless an error occurs (see <see cref="GradingService"/>)</returns>
        
        internal void Logout(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email is null or empty, cant logout");
            }
            email = email.ToLower();
            if (!userDictionary.ContainsKey(email))
            {             
                throw new ArgumentException("there is not such a user at all");
            }           
            userDictionary[email].Logout();    
        }            
    }
}


