using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnDAO
    {
        //  --------- name of fields ------------------//
        internal int Id { get;} // 0 - "beckLog" 1 -"inProgress" 2- "done"
        internal long BoardId {  get;} 
        private int maxTasks;
        internal int MaxTasks
        {
            get => maxTasks;
            set
            {
                if (isPersistent)
                {
                    columnController.Update(Id, BoardId, maxTasksColumnName, value);
                }
                maxTasks = value;
            }
        } 

        private int currTask;
        internal int CurrTask { get => currTask;
            set
            {
                if (isPersistent)
                {
                    columnController.Update(Id, BoardId, currTaskColumnName, value);
                }
                currTask = value;
            }
        } 

        // ----------- name of columns ----------------// 
        internal string idColumnName = "Id";
        internal string boardIdColumnName = "BoardId";
        internal string maxTasksColumnName = "MaxTasks";
        internal string currTaskColumnName = "CurrTask";


        // ---------- field for insert method ----------//
        internal bool isPersistent = false; 

        // ----------- the controller ------------------// 
        private ColumnController columnController { get; set; }
        internal ColumnDAO(int id, long boardId, int maxTasks, int currTask)
        {
            this.columnController = new ColumnController();
            this.Id = id;
            this.BoardId = boardId; 
            this.MaxTasks = maxTasks;   
            this.CurrTask = currTask;
        
        }


        // Constructor for loading purposes only
        internal ColumnDAO(int id, long boardId)
        {
            this.columnController = new ColumnController();
            ColumnDAO temp = columnController.Select(id, boardId);
            this.Id = temp.Id;
            this.BoardId = temp.BoardId;
            this.MaxTasks = temp.MaxTasks;
            this.CurrTask = temp.CurrTask;
            isPersistent = true;
        }

        /// <summary>
        /// insert the values into the DB
        /// </summary>
        internal void persist()
        {
            if (!isPersistent) { 
                columnController.Insert(this);
                isPersistent = true;
            }
        }

        internal void delete()
        {
            if (!isPersistent) 
            {
                throw new Exception("Cant delete a column that is not in the db");
            }
            columnController.Delete(Id,BoardId);

        }
    }
}
