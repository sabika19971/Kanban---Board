using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ColumnBl
    {
        private int id;
        private List<TaskBl> tasks;
        private int maxTasks;
        private int currTask;

        internal ColumnBl(int id)
        {
            this.id = id;
            this.tasks = new List<TaskBl>();
            currTask = 0;
            maxTasks = -1;
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
                    maxTasks = value;
                }
                else
                {                   
                    throw new Exception("cant set max tasks to less then the amount of tasks that already exist");
                }            
            }
        }

        internal void AddTask(TaskBl task)
        {
            if (maxTasks != -1)
            {
                if(currTask + 1 <= maxTasks)
                {
                    currTask++;
                    tasks.Add(task);
                }            
                else
                {
                    throw new Exception("you have reached the limit of tasks in the column");
                }
            }
            else
            {
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
    }

}
