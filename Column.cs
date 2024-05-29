using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Column
    {
        private string name;
        private List<Task> tasks;

        public Column(string name)
        {
            this.name = name;
            this.tasks = new List<Task>();
        }

        public string Name
        {
            get { return name; }
        }

        public List<Task> Tasks
        {
            get { return tasks; }
        }

        public bool AddTask(Task task)
        {
            // isn't implemented yet
        }

        public Task GetTaskById(string taskId)
        {
            // isn't implemented yet
        }

        public bool MoveTask(string taskId, Column targetColumn)
        {
            // isn't implemented yet
        }
    }

}
