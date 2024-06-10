using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class User
    {
        private string email;
        private string password;
        private List<Board> boards;

        public User(string email, string password)
        {
            this.email = email;
            this.password = password;
            boards = new List<Board>();
        }

        public string Email
        {
            get { return email; }
        }

        public string Password
        {
            get { return password; }
        }

        public List<Board> Boards
        {
            get { return boards; }
        }

        public bool AddBoard(Board board)
        {
            // isn't implemented yet
        }

        public bool RemoveBoard(string boardName)
        {
            // isn't implemented yet
        }

        public Board GetBoard(string boardName)
        {
            // isn't implemented yet
        }
    }
}
