using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DXMLProcedure
    {
        public string DXMLFileName { get; set; }

        public ProcedureDefinition ProcedureDefinition { get; set; }
    }
}
