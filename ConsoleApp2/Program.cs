
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Text.Json;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Reflection;
using log4net;
using log4net.Config;
using System.IO;

namespace ConsoleApp2
{

    internal class Program
    {

      
        static void Main (string[] args)
        {
            GradingService g = new GradingService();



            //UserService us = new UserService(new UserFacade()); 
            /*string notValidePassword=g.Register("sabika@gmail.com", "E123456");
            string noValidEmail = g.Register("@kajdhg", "12345667");
            string register_user = g.Register("sabihghka@gmail.com", "E123456e");
            string validMailUserName2 = g.Register("sabihghka@gmail.com", "E123456e");
            string noteddedTothelistofusers = g.Register("sabika@gmail.com", "E12345612@e");

            string login_user = g.Login("sabika@gmail.com", "E12345612@e");
            string login_user2 = g.Login("sabihghka@gmail.com", "E123456e");
            string login_userWrong_password = g.Login("sabihghka@gmail.com", "kajgdfask");
            string login_not_registered = g.Login("asjkhdg", "lakshgd");
            string logout_not_registered = g.Logout("asjkhdg");
            string logout_user2 = g.Logout("sabihghka@gmail.com");

            string login_user0 = g.Login("sabika@gmail.com", "E123456e");
            string creat_board_for_user = g.CreateBoard("sabika@gmail.com", "MyToDo list");*/

            //string Register_user0 = g.Register("sabika@gmail.com", null);
            //string Register_user01 = g.Register("sab@ika@gmail.com", "17635127517651735537812");
            /*
            string Register_user02 = g.Register("sabika@gmail.com", "@1235askjhdWg");
            string Login_user02 = g.Login("sabika@gmail.com", "@1235askjhdWg");
            string CreateBoard_user_02 = g.CreateBoard("sabika@gmail.com", "wedding");
            string Add_task_user_02 = g.AddTask("sabika@gmail.com", "wedding", "title", "fuku", new DateTime());
            string Add_task_user_022 = g.AddTask("sabika@gmail.com", "wedding", "tisdftle", "fuku", new DateTime());
            string Add_task_user_0222 = g.AddTask("sabika@gmail.com", "wedding", "tisdftle", "fuku", new DateTime());
            string Add_task_user_02222 = g.AddTask("sabika@gmail.com", "wedding", "tisdftle", "fuku", new DateTime());
            String gET_CULUMN_user02 = g.GetColumn("sabika@gmail.com", "wedding", 0);
            string updet_due_date = g.UpdateTaskDueDate("sabika@gmail.com", "wedding", 0, 3, new DateTime((long)DayOfWeek.Saturday));
            */

            /*
            string register = g.Register("kfirmiz@gmail.com", "Kfir1%");
            string login = g.Login("kfirmiz@gmail.com", "Kfir1%");
            string createBoard = g.CreateBoard("kfirmiz@gmail.com", "TODO");
            //string createBoard1 = g.CreateBoard("kfirmiz@gmail.com", "");
            //string limit = g.LimitColumn("kfirmiz@gmail.com","TODO",0,2);
            string limit = g.LimitColumn("kfirmiz@gmail.com", "TODO", 0, 2);
            string addTask1 = g.AddTask("kfirmiz@gmail.com", "TODO", "wadafikfuk1", "wtf", new DateTime());
            string addTask2 = g.AddTask("kfirmiz@gmail.com", "TODO", "wadafikfuk2", "wtf", new DateTime());
            string addTask3 = g.AddTask("kfirmiz@gmail.com", "TODO", "wadafikfuk3", "wtf", new DateTime());
            //string addTask4 = g.AddTask("kfirmiz@gmail.com", "", "wadafikfuk3", "wtf", new DateTime());
            string getColumn = g.GetColumn("kfirmiz@gmail.com", "TODO", 0);
            string advanceTask = g.AdvanceTask("kfirmiz@gmail.com", "TODO", 0, 2);
            string getColumn1 = g.GetColumn("kfirmiz@gmail.com", "TODO", 0);
            //string getColumn2 = g.GetColumn("kfirmiz@gmail.co", "TODO", 0);
            */

            //Console.WriteLine(getColumn);

            //Console.WriteLine(String.IsNullOrWhiteSpace("   "));
            //string checking = g.UpdateTaskDueDate("kfirmiz", "something", 0, 1, new DateTime((long)DayOfWeek.Saturday));
            //TaskBl task = new Task(new DateTime((long)DayOfWeek.Saturday), "wow", "so wow", string boardName, int id);


            // string advance_taske()
            //string Register_user03 = g.Register(null, "17635127517651735537812");



            /*string Register_null = g.Register(null,null);
            string Login_user0 = g.Login("sabika@gmail.com", "E123456easdsa");*/


            /*string create_board_user0 = g.CreateBoard("sabika@gmail.com", "my board");
            string create_board_user12 = g.CreateBoard("sabika@gmail.com", "my board");
            string delete_board = g.DeleteBoard("sabika@gmail.com", "my boardaksdg");
            string GET_COLUMN_LIMIT_NO_limit = g.GetColumnLimit("sabika@gmail.com", "my board", 0);
            string column_so_limit = g.LimitColumn("sabika@gmail.com", "my board", 0, 18);
            string column_to_limit_culumnerorr = g.LimitColumn("sabika@gmail.com", "my board", 123, 18);
            string column_so_limit_negative_value = g.LimitColumn("sabika@gmail.com", "my board", 0, -6655);
            string GET_COLUMN_LIMIT = g.GetColumnLimit("sabika@gmail.com", "my board", 0);
            string GET_COLUMN_LIMIT_NO_board = g.GetColumnLimit("sabika@gmail.com", "my boaasdasdrd", 0);
            string add_task_to_board = g.AddTask("sabika@gmail.com", "my board", "titlebakhs", "kasjdh", new DateTime());*/

            /*string create_board_user0 = g.CreateBoard("sabika@gmail.com", "my board");
            string login_user0 = g.Login("sabika@gmail.com", "E123456e");
            string create_board_user12 = g.CreateBoard("sabika@gmail.com", "my board");
            string delete_board = g.DeleteBoard("sabika@gmail.com", "my board");*/


            // string create_board_user1_exist = g.CreateBoard("sabika@gmail.com", "my board");






            // ------------------------------ DATABASE TESTS ----------------------------
        
            //string register = g.Register("kfirmiz@gmail.com", "Kfir1%");
            //string register = g.Register("second@gmail.com", "Kfir1%");
            string loadData = g.LoadData();
            //string deleteData = g.DeleteData();

            string login = g.Login("kfirmiz@gmail.com", "Kfir1%");
            //string login1 = g.Login("second@gmail.com", "Kfir1%");
            //string createBoard = g.CreateBoard("second@gmail.com", "TODO1");
            string createBoard = g.CreateBoard("kfirmiz@gmail.com", "TODO2");
            string addtask = g.AddTask("kfirmiz@gmail.com", "TODO2", "first", "It should work this time", new DateTime(2024, 8, 9));
            //string addtask1 = g.AddTask("second@gmail.com", "TODO1", "first", "It should work this time", new DateTime(2024, 8, 9));

            //string limit = g.LimitColumn("kfirmiz@gmail.com", "TODO1", 0, 2);
            //string limit1 = g.LimitColumn("second@gmail.com", "TODO1", 1, 4);

            //string createBoard1 = g.CreateBoard("kfirmiz@gmail.com", "second");
            //string createBoard2 = g.CreateBoard("kfirmiz@gmail.com", "third");
            //string getIds = g.GetUserBoards("kfirmiz@gmail.com");

            // --------------------------------------------------------------------------

        }
    }
}