using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ColumnSL
    {
        private int id;
        private List<Task> tasks;

        public ColumnSL(int id)
        {
            this.id = id;
            tasks = new List<Task>();
        }
        public ColumnSL(ColumnBl columnBl) {
            this.id = columnBl.Id;
            tasks = new List<Task>();
        
        }


        
    }

}
