using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    public class TaskSL
    {
        public int Id { get; }
        public DateTime CreationTime { get; }
        public DateTime DueDate { get; }
        public string BoardName { get; }
        public string Title { get; }
        public string Description { get; }
        public int ColumnOrdinal { get; }
        public string Assignee {  get; }

        internal TaskSL (TaskBl taskBl)
        {
            this.Id = taskBl.Id;
            this.CreationTime = taskBl.CreationTime; 
            this.DueDate= taskBl.DueDate;
            this.BoardName = taskBl.BoardName;
            this.Title = taskBl.Title;  
            this.Description = taskBl.Description;
            this.ColumnOrdinal = taskBl.ColumnOrdinal;
            this.Assignee = taskBl.Assignee;
        }      
    }
}
