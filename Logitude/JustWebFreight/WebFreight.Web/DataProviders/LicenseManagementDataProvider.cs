using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class LicenseManagementDataProvider : BaseDataProvider
    {
        public List<LicenseManagementDataList> LicensedUsers { get; set; }
    }

    public class LicenseManagementDataList
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string Exists { get; set; }

        public string PackageTotal { get; set; }
    }    
}