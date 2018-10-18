using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class APILogsDataList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DiagnosticLog { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public string ExceptionsMessage { get; set; }
    }
}
