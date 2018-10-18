using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityDws
{
  public  class CustomerAdditionalServiceDW
    {
   
        public string CustomerId { get; set; }
        public string AdditionalServiceId { get; set; }

        public int Tenant { get; set; }
        public bool Potential { get; set; }

        public string AdditionalServiceName { get; set; }
        public string AdditionalServiceCode { get; set; }
        public string CustomerName { get; set; }

        // dummy for customer additional service report
        public string Salesman { get; set; }
        public string PrimaryContact { get; set; }
        public string SalesmanUserId { get; set; }
        public string BusinessUnitId { get; set; }
    }
}
