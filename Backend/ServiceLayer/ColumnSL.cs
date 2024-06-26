using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
   
    public class ColumnSL
    {
        public int Id { get; }
        public List<TaskSL> Tasks { get; }

        public ColumnSL(int id)
        {
            this.Id = id;
            Tasks = new List<TaskSL>();
        }
        internal ColumnSL(ColumnBl columnBl) {
            this.Id = columnBl.Id;
            Tasks = new List<TaskSL>();
            foreach (var taskbl in columnBl.Tasks())
            {
                Tasks.Add(new TaskSL(taskbl));
            }       
        }       
    }
}
