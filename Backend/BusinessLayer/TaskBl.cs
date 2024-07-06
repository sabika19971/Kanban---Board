using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics.SymbolStore;
    using System.Reflection;

    internal class TaskBl
    {
        
        private int id;
        private DateTime creationTime;
        private DateTime dueDate;
        private string title;
        private string description;
        private int columnOrdinal;
        private string assignee;

        internal TaskBl(DateTime dueDate, string title, string description, int id)
        {
            this.id = id;
            this.dueDate = dueDate;
            this.creationTime = DateTime.Now;
            this.Title = title;         
            this.Description = description;
            this.columnOrdinal = 0;
            this.assignee = null;
        }

        internal string Title
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

        internal string Description
        {
            get { return description; }
            set
            {
                if (IsValidDescription(value))
                {                   
                    if (legalColumnForEdit(columnOrdinal))
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

        internal string Assignee
        {
            get { return assignee; }
            set { assignee = value; }
    
        }
        internal DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        internal DateTime CreationTime
        {
            get { return creationTime; }
        }

        internal int ColumnOrdinal
        {
            get { return columnOrdinal; }
            set
            {
                if (value > 2 || value < 0)
                {
                    throw new Exception("cant progres a done task.");
                }
                else
                {
                    columnOrdinal = value;
                }
            }

        }
        internal int Id
        {
            get { return id; }
        }

        private bool IsValidDescription(string description)
        {
            return !(String.IsNullOrWhiteSpace(description) || description.Length > 300);
        }

        private bool legalColumnForEdit(int columnOrdinal)
        {
            if(columnOrdinal == 0 || columnOrdinal == 1)
            {
                return true;
            }
            return false;
        }

        private bool IsValidTitle(string title)
        {
            return !(String.IsNullOrWhiteSpace(title) || title.Length > 50);
        }

        internal void AssignTask(string emailAssignee, string email)
        {
            if(assignee == null)
            {
                Assignee = emailAssignee;
            }
            else if (!Assignee.Equals(email))
            {
                throw new Exception("only assingee can assigne new assingee");
            }
            else { Assignee = emailAssignee; }
        }
        internal bool allowToEditTask (string email)
        {
            return assignee == email || assignee==null;
        }
    }

}
