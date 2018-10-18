using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class OceanInsightsDataProvider : BaseDataProvider
    {
        [Key] 
        public int Tenant { get; set; }
        public int Total { get; set; }
        public bool Detailed { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<OceanInsightsRecord> OceanInsightsRecordList { get; set; }
        public List<OceanInsightsRecordGrouped> OceanInsightsRecordGroup { get; set; }
    }
    public class OceanInsightsRecord
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ContainerNumber { get; set; }
        public string BLNumber { get; set; }
        public string SCACCode { get; set; }
        public DateTime CreateDate { get; set; }
        public int key { get; set; }
        public int Total { get; set; }
        public string TenantName { get; set; }
    }
    public class OceanInsightsRecordGrouped
    {
        [Key]
        public int key { get; set; }
        public int Total { get; set; }
    }
}