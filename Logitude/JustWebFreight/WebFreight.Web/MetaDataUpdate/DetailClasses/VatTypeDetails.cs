using System;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class VatTypeDetails
    {
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string Code { get; set; }

        public double Percentage { get; set; }

        public string EnglishName { get; set; }

        public string LocalName { get; set; }

        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public DateTime FromDate { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }
    }
}