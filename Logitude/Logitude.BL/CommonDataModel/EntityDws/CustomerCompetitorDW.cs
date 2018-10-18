using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityDws
{
   public class CustomerCompetitorDW
    {
     
        public string CustomerId { get; set; }
        public string CompetitorId { get; set; }
        public int Tenant { get; set; }
        public string CustomerName { get; set; }
        public string CompetitorName { get; set; }

    }
}
