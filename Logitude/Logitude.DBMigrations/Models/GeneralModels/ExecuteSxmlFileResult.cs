using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class ExecuteSxmlFileResult
    {
        public bool ShouldExecute { get; set; }
        public string Action { get; set; }
    }
}