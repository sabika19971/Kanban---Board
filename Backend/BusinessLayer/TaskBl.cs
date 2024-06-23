using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using System;
    using System.Diagnostics.SymbolStore;
    using System.Reflection;

    public class TaskBl
    {
        
        private string id;
        private DateTime creationTime;
        private DateTime dueDate;
        private string boardName;
        private string title;
        private string description;
        private int columnOrdinal;

        public TaskBl(DateTime dueDate, string title, string description, string boardName)
        {
            this.id = idGenerator();
            this.dueDate = dueDate;
            this.creationTime = DateTime.Now;
            this.Title = title;         // Use property setter to validate
            this.Description = description; // Use property setter to validate
            this.boardName = boardName;
            this.columnOrdinal = 0;
        }

        private string idGenerator()
        {
            return Guid.NewGuid().ToString();
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (IsValidTitle(value))
                {
                    if (legalColumnForEdit(columnOrdinal))
                    {
                        title = value;
                    }
                    else
                    {
                        throw new InvalidOperationException("cant edit task that is done.");
                    }
                   
                }
                else
                {
                    throw new InvalidOperationException("Title is illegal");
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (IsValidDescription(value))
                {
                    if (legalColumnForEdit(this.columnOrdinal))
                    {
                        description = value;
                    }
                    else
                    {
                        throw new Exception("cant edit a done task");
                    }
                    
                }
                else
                {
                    throw new InvalidOperationException("Description is illegal");
                }
            }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        public DateTime CreationTime
        {
            get { return creationTime; }
        }

        public int ColumnOrdinal
        {
            get { return columnOrdinal; }
            set
            {
                if (value > 2 || value<0)
                {
                    throw new Exception("cant progres a done task.");
                }
                else
                {
                    columnOrdinal = value;
                }
            }

        }
        public string Id
        {
            get { return id; }
        }

        public string BoardName
        {
            get { return boardName; }
        }

        public override bool Equals(object obj)
        {
           if((obj is TaskBl))
            {
                TaskBl taskBl = (TaskBl)obj;
                if(taskBl.Id == Id)
                {
                    return true;
                }
                
                
            }
           return false;
        }

        private bool IsValidDescription(string description)
        {
            return !(description == null || description.Length > 300 || description == "");
        }
        private bool legalColumnForEdit(int columnOrdinal)
        {

            return columnOrdinal != 2;
        }

        private bool IsValidTitle(string title)
        {
            return !(title == null || title.Length > 50 || title.Equals(""));
        }
    }

}
