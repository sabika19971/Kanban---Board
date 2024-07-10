using IntroSE.Kanban.Backend.DataExcessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAxcessLayer
{
    internal class ColumnDAO
    {
        //  --------- name of fields ------------------//
        internal int Id { get;} // 0 - "beckLog" 1 -"inProgress" 2- "done"
        internal int BoardId {  get;} // FK, PK.
        private int maxTasks;
        internal int MaxTasks
        {
            get => maxTasks;
            set
            {
                if (!isPersistent)
                {
                    Console.WriteLine("column isnt in the DB"); // will be changed after LoadColumns  //throw new InvalidOperationException("cant edit a column that is not in the database");
                }
                else
                {
                    columnController.Update(Id, BoardId, maxTasksColumnName, value);
                }
                maxTasks = value;
            }
        } 
        // the num of tasks can be added to a column.

        private int currTask;
        internal int CurrTask { get => currTask;
            set
            {
                if (!isPersistent)
                {
                    Console.WriteLine("column isnt in the DB"); // will be changed after LoadColumns //throw new InvalidOperationException("cant edit a column that is not in the database");
                }
                else
                {
                    columnController.Update(Id, BoardId, currTaskColumnName, value);
                }
                currTask = value;
            }
        } 


        // curr task in the column. ---- maybe need to delete
        // ----------- name of columns ----------------// 
        internal string idColumnName = "Id";
        internal string boardIdColumnName = "BoardId";
        internal string maxTasksColumnName = "MaxTasks";
        internal string currTaskColumnName = "CurrTask";


        // ---------- field for insert method ----------//
        internal bool isPersistent = false; 

        // ----------- the controller ------------------// 
        private ColumnController columnController { get; set; }
        public ColumnDAO(int id, int boardId, int maxTasks, int currTask)
        {
            // setting the controller 
            this.columnController = new ColumnController();

            this.Id = id;
            this.BoardId = boardId; 
            this.MaxTasks = maxTasks;   
            this.CurrTask = currTask;
        
        }

        /// <summary>
        /// insert the values into the DB
        /// </summary>
        public void persist()
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
                throw new Exception("cant delete an argument that is not in the db");
            }
            columnController.Delete(Id,BoardId);

        }
    }
}
