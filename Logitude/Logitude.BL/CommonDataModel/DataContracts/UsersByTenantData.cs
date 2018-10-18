using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.DataContracts
{
    public class UsersByTenantItem
    {
        public int Tenant { get; set; }
        public string TenantName { get; set; }
        public string VATNumber { get; set; }
        public string ActiveTenant { get; set; }
        public string Package{ get; set; }
        public string AddIns { get; set; }

        public int TotalActiveUsers { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ActiveUser { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }


    public class TenantData
    {
        public int Tenant { get; set; }
        public string TenantName { get; set; }
        public string VATNumber { get; set; }
        public string ActiveTenant { get; set; }
        public string PackageName { get; set; }
        public string AddIns { get; set; }
        public string DistributorCode { get; set; }

        public List<string>PackageCodeLists { get; set; }
        public List<string> AddInsPackageCodeLists { get; set; }
    }
}
