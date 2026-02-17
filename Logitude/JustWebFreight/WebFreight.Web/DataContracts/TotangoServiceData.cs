using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.DataContracts
{
    public class TotangoActivityInfo
    {

        public string OrganizationId { get; set; }
        public string OrgDisplayName { get; set; }
        public string UserName { get; set; }
        public string Module { get; set; }
        public string Activity { get; set; }
        public string ContactId { get; set; }
        public int Tenant { get; set; }
        public bool IsSharedLogisticsContact { get; set; }
        public string CardId { get; set; }
        public string PartnerTypeId { get; set; }


    }
}
