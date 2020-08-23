using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DBMigrations.Models
{
    public class DBConfig
    {
        public string DatabaseType { get; set; }
        public string GlobalConnectionString { get; set; }
        public string MainConnectionString { get; set; }
        public string SystemLogsConnectionString { get; set; }
        public string CargoTrackingConnectionString { get; set; }
    }
}