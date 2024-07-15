using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardDAO
    {
        //  --------- name of fields ------------------//
        internal long id;
        internal long Id{ get { return id; } set { id = value; } }
        internal string Name { get; set; }
        internal string owner;
        internal string Owner
        {
            get => owner;
            set
            {
                if (isPersisted)
                {
                    BoardController.Update(Id, OwnerColumnName, value);
                }
                owner = value;
            }
        }

        // ----------- name of columns ----------------// 
        internal string IdColumnName = "Id";
        internal string BoardColumnName = "Name";
        internal string OwnerColumnName = "Owner";

        // ---------- field for insert method ----------//
        internal bool isPersisted = false; // maybe provate after load boards

        // ----------- the controller ------------------// 
        private BoardController BoardController;

        internal BoardDAO(long id, string name, string email)
        {
            BoardController = new BoardController();
            this.Id = id;
            this.Name = name;
            this.owner= email;
        }

        internal void persist()
        {
            if (!isPersisted) { // WILL BE REMOVED AFTER LOAD BOARDS 
                BoardController.Insert(this);
                isPersisted = true;
            }
        }

        internal bool delete()
        {
            if (!isPersisted) 
            {
                throw new Exception("cant delete a board that is not in the DB");

            }
            return BoardController.Delete(Id, Name);
        }     
    }
}
