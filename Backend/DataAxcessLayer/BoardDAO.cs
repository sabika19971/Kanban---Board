using IntroSE.Kanban.Backend.DataExcessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAxcessLayer
{
    internal class BoardDAO
    {
        //  --------- name of fields ------------------//
        internal string Name { get; set; }
        internal int Id { get; }
        internal string owner;
        internal string Owner
        {
            get => owner;
            set
            {
                if (isPersisted)
                {
                    BoardController.Update(Id, IdColumnName, value);
                }
                owner = value;
            }
        }

        // ----------- name of columns ----------------// 
        internal string BoardColumnName = "Name";
        internal string IdColumnName = "Id";
        internal string OwnerColumnName = "Owner";

        // ---------- field for insert method ----------//
        private bool isPersisted = false;

        // ----------- the controller ------------------// 
        BoardController BoardController;



        public BoardDAO(int id, string name, string email)
        {
            BoardController = new BoardController();
            this.Id = id;
            this.Name = name;
            this.owner= email;
        }

        internal void persist()
        {
            BoardController.Insert(this);
            isPersisted = true;
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
