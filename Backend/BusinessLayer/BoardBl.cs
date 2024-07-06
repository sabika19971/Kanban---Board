using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardBl
    {
        private string name;
        private ColumnBl [] columns = new ColumnBl[3];
        private int sumTask = 0;
        private int id; // האם אני מביא את ה אי י מהטבלה בדאטה בייס
        private List<string> members;
        private string owner;
        


        internal BoardBl(string name ,string email)
        {
            this.name = name;
            columns[0]= new ColumnBl(0);
            columns[1] = new ColumnBl(1);
            columns[2] = new ColumnBl(2);  
            this.id = id;// according to the answer in for the field
            this.members = new List<string>();
            this.owner = email;

        }

       internal List<string> Members
        {
            get { return members; }
        }
        internal int Id { get; }
        internal string Name
        {
            get { return name; }
            set 
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("cant add empty board name");
                }
                name = value; 
            }
        }

        internal ColumnBl getColumns (int i)
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
        internal string Owner
        {
            get { return owner; }
            set { if (isMember(value)){
                    owner = value;
                }
                else
                {
                    throw new Exception(value + " " + " is not a member of this board");
                }
                
            }
        }
        internal bool validTaskId (TaskBl taskBl)
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
            sumTask++;
        }

        internal void AdvanceTask(TaskBl taskToAdvance)
        {
            columns[taskToAdvance.ColumnOrdinal].AddTask(taskToAdvance);
            columns[taskToAdvance.ColumnOrdinal-1].RemoveTask(taskToAdvance);
        }

        internal void limitColumn(int columnOrdinal, int limit)
        {                       
            this.columns[columnOrdinal].MaxTasks = limit;                    
        }

        internal int getNumOfAllTasks()
        {
            return sumTask;
        }

        internal bool JoinBoard(string email)
        {
            throw new NotImplementedException();
        }

        internal void leaveBoard(string email)
        {
            if (!isMember(email))
            {
                throw new Exception("only a member of the board can leave the borad");
            }
           this.getColumns(0).leaveBoard(email);
           this.getColumns(1).leaveBoard(email);
            members.Remove(email);
        }

        internal bool isMember(string email)
        {
            return members.Contains(email);
        }
    }
}
