using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer; 
using IntroSE.Kanban.Backend.BusinessLayer; 

namespace IntroSE.Kanban.Backend.BusinessLayer
{

    public class BoardSL
    {
        private string name;
        private ColumnSL[] columns;


        public BoardSL(BoardBl boardbl)
        {
            Console.WriteLine("Imhere");
            name = boardbl.Name;
            columns[0] = new ColumnSL(boardbl.getColumns(0));
            columns[1] = new ColumnSL(boardbl.getColumns(1));
            columns[2] = new ColumnSL(boardbl.getColumns(2));
        }


    }


}
