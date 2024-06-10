using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
namespace IntroSE.Kanban.Backend.BusinessLayer;

internal class ServiceFactory
{
    private UserFacade userFacade;
    private BoardFacade boardFacade;
    private TaskFacade taskFacade;
    public ServiceFactory() { 
        userFacade = new UserFacade();
        boardFacade = new BoardFacade();
        taskFacade = new TaskFacade();
    }
}
