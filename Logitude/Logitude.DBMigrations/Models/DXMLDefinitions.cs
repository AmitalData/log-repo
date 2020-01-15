using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DXMLDefinitions
    {
        public List<DXMLTable> DXMLTables { get; set; }

        public List<DXMLView> DXMLViews { get; set; }

        public List<DXMLProcedure> DXMLProcedures { get; set; }
    }
}
