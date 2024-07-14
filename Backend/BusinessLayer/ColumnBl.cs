using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using IntroSE.Kanban.Backend.DataAxcessLayer;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Net;

    internal class ColumnBl
    {
        
        private int id;
        private List<TaskBl> tasks;
        private int maxTasks;
        private int currTask;
        private ColumnDAO columnDAO;
        private int boardId; // new for pk 
        private ColumnController columnController = new ColumnController(); // new
        private TaskController taskController = new TaskController(); //new
        

        internal ColumnBl(int id, int boardId)
        {
            columnDAO = new ColumnDAO(id, boardId,-1,0);
            columnDAO.persist();

            this.id = id;
            this.tasks = new List<TaskBl>();
            currTask = 0;
            maxTasks = -1;
        }

        internal ColumnBl(int id, int boardId, bool indicateLoad) // DIDNT MANAGE TO CHAIN CONSTRUCTORS WITH THE ONE BELOW
        {
            this.columnDAO = columnController.Select(id, boardId);
            columnDAO.isPersistent = true;
            this.id = columnDAO.Id;
            this.boardId = columnDAO.BoardId;
            this.maxTasks = columnDAO.MaxTasks;
            this.currTask = columnDAO.CurrTask;
            this.tasks = new List<TaskBl>();
            List<TaskDAO> taskDAOs = taskController.SelectTasks(this.boardId, this.id);
            foreach (var taskDAO in taskDAOs)
            {
                tasks.Add(new TaskBl(taskDAO));
            }

        }

        internal ColumnBl(ColumnDAO column)
        {
            this.columnDAO = column;
            columnDAO.isPersistent = true;
            this.id = column.Id;
            this.boardId = column.BoardId;
            this.maxTasks = column.MaxTasks;
            this.currTask = column.CurrTask;
            this.tasks = new List<TaskBl>();
            List<TaskDAO> taskDAOs = taskController.SelectTasks(this.boardId,this.id);
            foreach (var taskDAO in taskDAOs)
            {
                tasks.Add(new TaskBl(taskDAO));
            }
        }

        internal int Id
        {
            get { return id; }
        }

        internal int CurrTask
        {
            get { return currTask; }
        }

        internal List<TaskBl> Tasks()
        {  
            return tasks; 
        }

        internal int MaxTasks
        {
            get { return maxTasks; }
            set
            {
                if (value >= currTask)
                {
                    columnDAO.MaxTasks=value;
                    maxTasks = value;
                }
                else
                {                   
                    throw new Exception("cant set max tasks to less then the amount of tasks that already exist");
                }            
            }
        }

        internal bool canAddTask()
        {
            return (maxTasks == -1 || currTask + 1 <= maxTasks);
        }
        
        internal void AddTask(TaskBl task)
        {
            if (maxTasks != -1)
            {
                if(currTask + 1 <= maxTasks)
                {
                    columnDAO.CurrTask=currTask+1;
                    currTask++;
                    tasks.Add(task);
                }            
                else
                {
                    task.ColumnOrdinal--; // reverting it (for failed advance)
                    throw new Exception("you have reached the limit of tasks in the column");
                }
            }
            else
            {
                columnDAO.CurrTask = currTask+1;
                currTask++;
                tasks.Add(task);
            }
        }

        internal bool canAdd(TaskBl taskBl)
        {
            if (tasks.Contains(taskBl))
            {
                return false;
            }
            return true;
        }

        internal void RemoveTask(TaskBl taskToAdvance)
        {
            if (tasks.Contains(taskToAdvance))
            {
                columnDAO.CurrTask = currTask - 1;
                tasks.Remove(taskToAdvance);
                currTask = currTask - 1;
            }
            else
            {
                throw new Exception("there is no task to remove");
            }
        }

        public override string ToString()
        {
            string s = "";
            foreach (var task in tasks)
            {
                s = s + task.Id + ",";

            }
            return s;
        }

        internal void leaveBoard(string email)
        {
            foreach (var task in tasks)
            {
                if(task.Assignee == email)
                {                    
                    task.Assignee = null;
                }
            }
        }

        internal int maxTasksId()
        {
            int maxId = 0;
            foreach (TaskBl task in tasks )
            {
                if (task.Id > maxId)
                {
                    maxId = task.Id;
                }
            }
            return maxId;
        }

        internal void delete()
        {
            foreach (var task in tasks)
            {
                task.delete();
            }
            columnDAO.delete();           
        }
    }

}
