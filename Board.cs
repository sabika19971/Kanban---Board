using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{

    public class Board
    {
        private string name;
        private List<Column> columns;

        public Board(string name)
        {
            this.name = name;
            this.columns = new List<Column>();
            // Initialize default columns
            columns.Add(new Column("backlog"));
            columns.Add(new Column("in progress"));
            columns.Add(new Column("done"));
        }

        public string Name
        {
            get { return name; }
        }

        public List<Column> Columns
        {
            get { return columns; }
        }

        public bool AddColumn(Column column)
        {
            // isn't implemented yet
        }

        public bool RemoveColumn(string columnName)
        {
            // isn't implemented yet
        }

        public Column GetColumn(string columnName)
        {
            // isn't implemented yet
        }
    }
}
