using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.DataContracts
{
   public class TariffSearchSummary
    {
        [Key]
        public string TariffId { get; set; }
        public string TariffNumber { get; set; }
        public string ChargeTypeId { get; set; }
        public string Price { get; set; }
        public decimal? ActualPrice { get; set; }
        public DateTime? EffictiveDate { get; set; }
        public string Remarks { get; set; }
        public string SellerName { get; set; }
        public string ImageId { get; set; }
        public decimal? decimalprice { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyId { get; set; }
        public string VersionId { get; set; }
        public string TotalSurcharge { get; set; }
        public string WholePrice { get; set; }
        public string AllIn { get; set; }
        public string UnitOfMesurmentCode { get; set; }
        public string UnitOfMesurmentId { get; set; }
        public List<SurchargeSummary> Surcharges { get; set; }
        public string SellerId { get;  set; }
        public decimal? MinPrice { get; set; }
        public bool IsMinIconVisible { get; set; }
    }
}
