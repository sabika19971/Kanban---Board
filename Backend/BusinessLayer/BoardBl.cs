using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    public class BoardBl
    {
        private string name;
        private ColumnBl [] columns = new ColumnBl[3];
        
        public BoardBl(string name)
        {
            this.name = name;
            columns[0]= new ColumnBl(0);
            columns[1] = new ColumnBl(1);
            columns[2] = new ColumnBl(2);
        }

        public string Name
        {
            get { return name; }
            set 
            { 
                if(value == null || value.Equals(""))
                {
                    throw new ArgumentException("cant add empty board name");
                }
                name = value; 
            }
        }

        public ColumnBl getColumns (int i)
        {
            if (indexIsValid(i))
            {
                return columns[i];
            }
            return null;
        }

       

        private bool indexIsValid(int i)
        {
            if (i <0 || i >2)
            {
                return false;
            }
            return true;
        }

        public bool validTaskId (TaskBl taskBl)
        {
            if (columns[0].canAdd(taskBl) & columns[1].canAdd(taskBl) & columns[2].canAdd(taskBl))
            {
                return true;
            }
            return false;
        }

        internal void AddTask(TaskBl taskToAdd)
        {
            columns[0].AddTask(taskToAdd);
        }

        internal void AdvanceTask(TaskBl taskToAdvance)
        {
            columns[taskToAdvance.ColumnOrdinal].AddTask(taskToAdvance);
            columns[taskToAdvance.ColumnOrdinal-1].RemoveTask(taskToAdvance);
        }

        internal void limitColumn(int columnOrdinal, int limit)
        {
            try
            {
                
                this.columns[columnOrdinal].MaxTasks = limit;
            }
            catch(Exception ex) 
            {
                Console.WriteLine (ex.ToString ());
                throw ex;
            }
           
        }
    }
}
