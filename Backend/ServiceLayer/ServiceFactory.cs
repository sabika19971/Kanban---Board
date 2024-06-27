﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using log4net.Config;

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
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public ServiceFactory() {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        aut = new Autentication();
        bf = new BoardFacade(aut);
        uf = new UserFacade(bf,aut);
        tf = new TaskFacade(aut,bf,uf);

        US = new UserService(uf, Log);
        BS = new BoardService(bf, Log);
        TS = new TaskService(tf, Log);
    }
  
    public UserService UserService => US;
    public BoardService BoardService => BS;
    public TaskService TaskService => TS;
}
