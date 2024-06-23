using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using System;

    public class TaskSL
    {
        private string id;
        private DateTime creationTime;
        private DateTime dueDate;
        private string boardName;
        private string title;
        private string description;
        private int columnOrdinal;

       

        public TaskSL (TaskBl taskBl)
        {
            this.id = taskBl.Id;
            this.creationTime = taskBl.CreationTime; 
            this.dueDate= taskBl.DueDate;
            this.boardName = taskBl.BoardName;
            this.title = taskBl.Title;  
            this.description = taskBl.Description;
            this.columnOrdinal = taskBl.ColumnOrdinal;

        }

        
    }

}
