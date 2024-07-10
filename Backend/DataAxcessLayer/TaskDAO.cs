using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAxcessLayer
{
    internal class TaskDAO
    {
        // ----------- name Of fields -----------//

        internal int Id { get;}
        internal int BoardId { get; }
        internal DateTime CreationTime { get; }

        DateTime dueDate;
        string title;
        string description;
        string assignee;
        int columnOrdinal;
        internal DateTime DueDate 
        { 
            get => dueDate;           
            set
            {
                if (!isPersistent)
                {
                    //throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                else
                {
                    taskController.UpdateTaskDueDate(Id, BoardId, DueDateColumnName, value);
                }
                dueDate = value;
            }
        }
        internal string Title 
        {  
            get=> title; 
            set
            {
                if (!isPersistent)
                {
                    //throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                else
                {
                    taskController.Update(Id, BoardId, TitleColumnName, value);                
                }
                title = value;

            } 
        }

        internal string Description
        {
            get => description;
            set
            {
                if (!isPersistent)
                {
                    //throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                else
                {
                    taskController.Update(Id, BoardId, DescriptionColumnName, value);
                }
                description = value;
            }
        }

        internal int ColumnOrdinal 
        { 
            get => columnOrdinal;
            set 
            {
                if (!isPersistent)
                { 
                    //throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                else
                {
                    taskController.UpdateColumnOrdinal(Id, ColumnOrdinalColumnName, value);
                }
                columnOrdinal = value;
            }
        }

        internal string Assignee 
        {  
            get => assignee;
            set
            {
                if (!isPersistent)
                {
                    //throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                else
                {
                    taskController.Update(Id, BoardId, AssigneeColumnName, value);
                }
                assignee = value;
            }
        }



        // ----------- names of columns ----------- // 
        internal string idColumnName = "Id";
        internal string BoardIdColumnName = "BoardId";
        internal string ColumnOrdinalColumnName = "ColumnOrdinal";
        internal string CreationTimeColumnName = "CreationTime";
        internal string DueDateColumnName = "DueDate";
        internal string TitleColumnName = "Title";
        internal string DescriptionColumnName = "Description";
        internal string AssigneeColumnName = "Assignee";


        // ------------ the controller -------------//
        private TaskController taskController;

        //------------ field for insert method --------//
         internal bool isPersistent = false;

        public TaskDAO(int id, int boardId, int columnOrdinal, string title, string description, DateTime creationTime, DateTime dueDate, string assignee)
        {

            taskController = new TaskController();

            
            
            this.Id = id;
            this.BoardId = boardId;
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.ColumnOrdinal = columnOrdinal;
            this.Assignee = assignee;


        }





        public void persist()
        {
            if (!isPersistent) { 
                taskController.Insert(this);
                isPersistent = true;
            }
        }

        internal void delete()
        {
            if (!isPersistent)
            {
                throw new Exception("cant delet a task that is not in the db");
            }
            taskController.Delete(Id, BoardId);

        }
    }
}
