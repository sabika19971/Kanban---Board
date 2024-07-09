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
        internal DateTime DueDate { get=>DueDate; 
            
            set
            {
                if (!isPersistent)
                {
                    throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                taskController.UpdateTaskDueDate(Id,BoardId, DueDateColumnName, value);
                DueDate = value; 
            
            
            }
                }
        internal string Title {  get=> Title; 
            set
            {
                if (!isPersistent)
                {
                    throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                taskController.Update(Id,BoardId, TitleColumnName, value);
                Title = value;
            } 
        }
        internal string Description
        {
            get => Description;

            set
            {
                if (!isPersistent)
                {
                    throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                taskController.Update(Id,BoardId, DescriptionColumnName, value); 
                Description = value;
            }
        }
        internal int ColumnOrdinal { get => ColumnOrdinal;
            set 
            {
                if (!isPersistent)
                { 
                    throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                taskController.UpdateColumnOrdinal(Id, ColumnOrdinalColumnName, value);
                ColumnOrdinal = value;

            }
        }
        internal string Assignee 
        {  get=>Assignee;

            set
            {
                if (!isPersistent)
                {
                    throw new InvalidOperationException("cand edit a task that is not in the Db");
                }
                taskController.Update(Id,BoardId, AssigneeColumnName, value);
                Assignee = value;
            }

        }
       


        // ----------- names of columns ----------- // 
        internal string BoardIdColumnName = "BoradId";
        internal string idColumnName = "Id";
        internal string CreationTimeColumnName = "CreationTime";
        internal string DueDateColumnName = "DueDate";
        internal string TitleColumnName = "Title";
        internal string DescriptionColumnName = "Description";
        internal string ColumnOrdinalColumnName = "ColumnOrdinal";
        internal string AssigneeColumnName = "Assignee";


        // ------------ the controller -------------//
        private TaskController taskController;

        //------------ field for insert method --------//
         internal bool isPersistent = false;

        public TaskDAO(int id, int boardId, DateTime creationTime, DateTime dueDate, string title, string description, int columnOrdinal, string assignee)
        {

            taskController = new TaskController();

            
            
            this.Id = id;
            this.BoardId = boardId;
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.ColumnOrdinal = columnOrdinal;
            this.AssigneeColumnName = assignee;


        }





        public void persist()
        {
            taskController.Insert(this);
            isPersistent = true;
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
