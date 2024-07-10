using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using IntroSE.Kanban.Backend.DataAxcessLayer;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
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
        private TaskDAO taskDAO;

        internal TaskBl(DateTime dueDate, string title, string description, int id , int boardId)
        {
            taskDAO = new TaskDAO(id,boardId,0,title,description,DateTime.Now,dueDate,null);
            taskDAO.persist();


            this.id = id;
            this.dueDate = dueDate;
            this.creationTime = DateTime.Now;
            this.Title = title;         
            this.Description = description;
            this.columnOrdinal = 0;
            this.assignee = null;
        }

        internal TaskBl(TaskDAO task)
        {
            taskDAO = task;
            taskDAO.isPersistent = true;
            this.id = task.Id;
            this.dueDate = task.DueDate;
            this.creationTime = task.CreationTime;
            this.Title = task.Title;
            this.Description = task.Description;
            this.columnOrdinal = task.ColumnOrdinal;
            this.assignee = task.Assignee;
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
                       
                        taskDAO.Title = value;
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
                        taskDAO.Description = value;
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
            set {
                
                
                taskDAO.Assignee = value;
                assignee = value;
            
            
            
            }
    
        }
        internal DateTime DueDate
        {
            get { return dueDate; }
            set {
                if(dueDate.CompareTo(CreationTime)< 0 )
                {
                    throw new Exception("cant set time to past");
                }  
                else if(value.Equals(null)) 
                {
                    throw new ArgumentException("cant set dueDate to null");
                }
                taskDAO.DueDate = value;
                dueDate = value;
            
            
            }
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
                    taskDAO.ColumnOrdinal = value;
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
                taskDAO.Assignee= emailAssignee;
                Assignee = emailAssignee;
            }
            else if (!Assignee.Equals(email))
            {
                throw new Exception("only assingee can assigne new assingee");
            }
            else 
            {
                taskDAO.Assignee = emailAssignee;
                Assignee = emailAssignee; 
            }
        }
        internal bool allowToEditTask (string email)
        {
            return assignee == email || assignee==null;
        }

        internal void delete()
        {
            taskDAO.delete();
        }
    }

}
