using IntroSE.Kanban.Backend.ServiceLayer;

namespace TestProject1 { 
 



    public class Tests
    {
        private GradingService g;
        [SetUp]
        public void Setup()
        {
             g = new GradingService();
           
        }

        [Test]
        public void Test1()
        {
            string s = g.Register("sabika@gmail.com", "123456");
            Console.WriteLine(s);
        }
    }
}