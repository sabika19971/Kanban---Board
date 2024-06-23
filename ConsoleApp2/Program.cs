
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Text.Json;

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

            string Register_user0 = g.Register("sabika@gmail.com", "E123456e");
            string create_board_user0 = g.CreateBoard("sabika@gmail.com", "my board");
            string create_board_user12 = g.CreateBoard("sabika@gmail.com", "my board");
            string delete_board = g.DeleteBoard("sabika@gmail.com", "my boardaksdg");
            string GET_COLUMN_LIMIT_NO_limit = g.GetColumnLimit("sabika@gmail.com", "my board", 0);
            string column_so_limit = g.LimitColumn("sabika@gmail.com", "my board", 0, 18);
            string column_to_limit_culumnerorr = g.LimitColumn("sabika@gmail.com", "my board", 123, 18);
            string column_so_limit_negative_value = g.LimitColumn("sabika@gmail.com", "my board", 0, -6655);
            string GET_COLUMN_LIMIT = g.GetColumnLimit("sabika@gmail.com", "my board", 0);
            string GET_COLUMN_LIMIT_NO_board = g.GetColumnLimit("sabika@gmail.com", "my boaasdasdrd", 0);
            string add_task_to_board = g.AddTask("sabika@gmail.com", "my board", "titlebakhs", "kasjdh", new DateTime());

            /*string create_board_user0 = g.CreateBoard("sabika@gmail.com", "my board");
            string login_user0 = g.Login("sabika@gmail.com", "E123456e");
            string create_board_user12 = g.CreateBoard("sabika@gmail.com", "my board");
            string delete_board = g.DeleteBoard("sabika@gmail.com", "my board");*/


            // string create_board_user1_exist = g.CreateBoard("sabika@gmail.com", "my board");







        }
    }

}