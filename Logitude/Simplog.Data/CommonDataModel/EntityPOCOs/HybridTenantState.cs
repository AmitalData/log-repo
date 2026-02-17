using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class HybridTenantState
    {

        [Key]
        public int Tenant { get; set; }
        public int FailedQueue { get; set; }
        public int WaitingQueue { get; set; }
        public DateTime LastUpdateDateTime { get; set; }


    }
}
