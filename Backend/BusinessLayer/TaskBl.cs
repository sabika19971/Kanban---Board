using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using System;

    public class Task
    {
        private string id;
        private DateTime creationTime;
        private DateTime dueDate;
        private string title;
        private string description;

        public Task(string id, DateTime dueDate, string title, string description)
        {
            this.id = id;
            creationTime = DateTime.Now;
            this.dueDate = dueDate;
            this.title = title;
            this.description = description;
        }

        public string Id
        {
            get { return id; }
        }

        public DateTime CreationTime
        {
            get { return creationTime; }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
        }

        public string Title
        {
            get { return title; }
        }

        public string Description
        {
            get { return description; }
        }
    }

}
