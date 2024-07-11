using IntroSE.Kanban.Backend.DataAxcessLayer;
using IntroSE.Kanban.Backend.DataExcessLayer;
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
        private int sumTask; //inisialized with the function getHighestSumMax in cunstracutor 
        private int id; 
        private List<string> members;
        private string owner;
        private BoardDAO boardDAO;
        private UserBoardssStatusDAO userBoardssStatusDAO;
        //private ColumnController ColumnController = new ColumnController();



        internal BoardBl(int id, string name ,string email)
        {
            boardDAO = new BoardDAO(id,name,email);
            //boardDAO.isPersisted = true; 
            boardDAO.persist();
            //userBoardssStatusDAO = new UserBoardssStatusDAO(email,id,1);
            //userBoardssStatusDAO.persist();
            
            columns[0]= new ColumnBl(0,id);
            columns[1] = new ColumnBl(1,id);
            columns[2] = new ColumnBl(2,id);

            this.name = name;
            this.id = id; 
            this.members = new List<string>();
            this.owner = email;
            getHighestSumMax();
        }

        internal BoardBl(BoardDAO boardDAO)
        {
            this.boardDAO = boardDAO;
            this.boardDAO.isPersisted = true;
            this.id = boardDAO.Id;
            this.name = boardDAO.Name;
            this.owner = boardDAO.Owner;
            this.members = new List<string>(); // NEEDS TO BE LOADED after dealing with the connecting table
            //columns[0] = new ColumnBl(ColumnController.Select(0,this.id));      
            //columns[1] = new ColumnBl(ColumnController.Select(1, this.id));
            //columns[2] = new ColumnBl(ColumnController.Select(2, this.id));
            columns[0] = new ColumnBl(0, this.id,true);         // to reduce coupling
            columns[1] = new ColumnBl(1, this.id,true);         // to reduce coupling
            columns[2] = new ColumnBl(2, this.id,true);         // to reduce coupling

            getHighestSumMax();
            //userBoardssStatusDAO = new UserBoardssStatusDAO(email, id, 1);         WILL BE ADDED AFTER WE WILL DEAL WITH THE CONNECTING TABLE
            //userBoardssStatusDAO.persist();
        }

        internal List<string> Members
        {
            get { return members; }
        }
        internal int getId() 
        {
            return id;
        }
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

        internal bool canAddTask()
        {
            return columns[0].canAddTask();
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
            UserBoardssStatusDAO userToLeaveBoard = new UserBoardssStatusDAO(email,this.id);
            userToLeaveBoard.delete();
            members.Remove(email);
        }

        internal bool isMember(string email)
        {
            return members.Contains(email);
        }

        private void getHighestSumMax()
        {
            int maxSumMax = 0;
            for (int i = 0; i < columns.Length; i++)
            {
                if(columns[i].maxTasksId()> maxSumMax)
                {
                    maxSumMax = columns[i].maxTasksId();
                }  

            }
            sumTask = maxSumMax;
        }

        internal void delete()
        {
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i].delete();
            }
            userBoardssStatusDAO.delete();
            boardDAO.delete();
        }

        internal void addMemberToBoard(string email)
        {
            foreach(var member in members)
            {
                if (email == member)
                {
                    throw new ArgumentException("member already exist in the board");
                }
            }
            UserBoardssStatusDAO userToAddAsMember = new UserBoardssStatusDAO(email,getId(),0);
            userToAddAsMember.persist();
            
           Members.Add(email);
        }

        internal void changeOwner(string currentOwnerEmail, string newOwnerEmail)
        {
           
            userBoardssStatusDAO.changeOwner(currentOwnerEmail, newOwnerEmail);
        }
    }
}
