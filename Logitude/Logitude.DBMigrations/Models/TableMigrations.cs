using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class TableMigrations
    {
        public TableDefinition Table;
        public TableMigrations(TableDefinition table)
        {
            Table = table;
        }

        public string GetScript()
        {
            return "";
        }

        //public TableDefinition GetTableDefinitionFromDB(string tableName)
        //{

        //}
    }
}
