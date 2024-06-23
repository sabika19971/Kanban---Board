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

    public class ColumnBl
    {
        private int id;
        private List<TaskBl> tasks;
        private int maxTasks;
        private int currTask;

        public ColumnBl(int id)
        {
            this.id = id;
            this.tasks = new List<TaskBl>();
            currTask = 0;
            maxTasks = -1;
        }

        public int Id
        {
            get { return id; }
        }

        public List<TaskBl> Tasks
        {
            get { return tasks; }
        }

        public int MaxTasks
        {

            get { return maxTasks; }

            set
            {
                if (value > 0 || value < currTask)
                {
                    maxTasks = value;
                }
                else
                {
                    throw new Exception("cant set max tasks to be negative num or your limit of ");
                }
               
            }
        }
        
        public void AddTask(TaskBl task)
        {
            if (maxTasks != -1)
            {
                if(currTask+1<= maxTasks)
                {
                    currTask++;
                    tasks.Add(task);
                }
                else
                {
                    throw new Exception("you have reached the limit of tasks in the column");
                }
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
            if (currTask > 0)
            {
                tasks.Remove(taskToAdvance);
                currTask =currTask-1;
            }
            else
            {
                throw new Exception("there is no task to remove");
            }
        }
    }

}
