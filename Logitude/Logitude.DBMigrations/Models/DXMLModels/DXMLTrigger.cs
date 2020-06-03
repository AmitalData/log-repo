using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DXMLTrigger
    {
        public string DXMLFileName { get; set; }

        public TriggerDefinition TriggerDefinition { get; set; }
    }
}