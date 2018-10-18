using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ParentVsChildTenantsDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }

        public List<ParentTenantRecord> ParentTenantRecordList { get; set; }
    }

    public class ParentTenantRecord
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public int? NumberOfUsers { get; set; }
        public int? FreeUsers { get; set; }
        public string Package { get; set; }
        public string ParentTenantName { get; set; }
        public int? ParentTenantId { get; set; }
        public string ParentTenantPackage { get; set; }
        public int? NumberOfInvalidUsers { get; set; }       
    }
}