using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.DataContracts
{
   public class EntityExternalUpdate
    {
        public string Entity { get; set; }
        public string ShipmentNumber { get; set; }
        public string Master { get; set; }
        public string House { get; set; }

        public int Tenant { get; set; }
        public List<Status> Statuses{ get; set; }

    }


    public class Status
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? Date { get; set; }
        public string PortCode { get; set; }
        public string PortCountry { get; set; }

    }
}
