using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
   public class DWSubQueryList
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DWFactTableCode { get; set; }
        public string DWQueryId { get; set; }
        public string SQLString { get; set; }
        public string FiltersXML { get; set; }
        public string ColumnsXML { get; set; }

    }
}
