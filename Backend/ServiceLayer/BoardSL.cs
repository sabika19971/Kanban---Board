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
        public string Name{ get; }
        private ColumnSL[] columns;
        private int id; 

        internal BoardSL(BoardBl boardbl, int id)
        {
            Name = boardbl.Name;
            columns[0] = new ColumnSL(boardbl.getColumns(0));
            columns[1] = new ColumnSL(boardbl.getColumns(1));
            columns[2] = new ColumnSL(boardbl.getColumns(2));
            this.id = id; 
        }

        public ColumnSL getColumns(int i)
        {
            if (indexIsValid(i))
            {
                return columns[i];
            }
            return null;
        }

        public int Id {  get ; } 
        private bool indexIsValid(int i)
        {
            if (i < 0 || i > 2)
            {
                return false;
            }
            return true;
        }
    }
}
