using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.SystemLogsModel.EntityPMs
{
   public class ErrorLogPM
    {
       [Key]
       public string Id { get; set; }
       public int Tenant { get; set; }
       public string UserName { get; set; }
       public DateTime LogDate { get; set; }
       public DateTime ClientDate { get; set; }
       public string Tier { get; set; }
       public string Exception { get; set; }
       public string StackTrace { get; set; }
       public string SearchFields { get; set; }
       public bool IsSecured { get; set; }
       public string IP { get; set; }
    }
}
