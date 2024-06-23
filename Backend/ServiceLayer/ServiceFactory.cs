using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer;

internal class ServiceFactory
{
    private UserFacade uf;
    private BoardFacade bf;
    private TaskFacade tf;
    private Autentication aut;

    private UserService US;
    private BoardService BS;
    private TaskService TS;


    public ServiceFactory() { 

        aut = new Autentication();
        bf = new BoardFacade(aut);
        uf = new UserFacade(bf,aut);
        tf = new TaskFacade(aut,bf,uf);

        US = new UserService(uf);
        BS = new BoardService(bf);
        TS = new TaskService(tf);
    }
  
    public UserService UserService => US;

    public BoardService BoardService => BS;

    public TaskService TaskService => TS;


}
