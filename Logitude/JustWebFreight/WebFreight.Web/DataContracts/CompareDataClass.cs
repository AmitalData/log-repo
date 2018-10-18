using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class CompareDataClass
    {
        [Key]
        public string Id { get; set; }
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string RankCode { get; set; }
        public string RankName { get; set; }

        public string SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }

        public decimal? TEU { get; set; }
        public decimal? TEU_Old { get; set; }
        public decimal? TEU_New { get; set; }
        public decimal? TEU_Actual { get; set; }
        public decimal? TEU_Potential { get; set; }

        public decimal? Revenue { get; set; }
        public decimal? Revenue_Old { get; set; }
        public decimal? Revenue_New { get; set; }
        public decimal? Revenue_Actual { get; set; }
        public decimal? Revenue_Potential { get; set; }

        public decimal? NumberOfShipments { get; set; }
        public decimal? NumberOfShipments_Old { get; set; }
        public decimal? NumberOfShipments_New { get; set; }
        public decimal? NumberOfShipments_Actual { get; set; }
        public decimal? NumberOfShipments_Potential { get; set; }
       
        public decimal? ChargeableWeight { get; set; }
        public decimal? ChargeableWeight_Old { get; set; }
        public decimal? ChargeableWeight_New { get; set; }
        public decimal? ChargeableWeight_Actual { get; set; }
        public decimal? ChargeableWeight_Potential { get; set; }
        
        public int LocationsCount_Potential { get; set; }
        public int LocationsCount_Actual { get; set; }
    }
}