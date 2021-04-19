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
        public TariffSearchSummary()
        {
            MoreLessDetailsLabel = "More Details";
        }

        [Key]
        public string TariffId { get; set; }
        public string TariffNumber { get; set; }
        public string ChargeTypeId { get; set; }
        public string Price { get; set; }
        public string SurchargesPrice { get; set; }
        public decimal? ActualPrice { get; set; }
        public DateTime? EffictiveDate { get; set; }
        public string Remarks { get; set; }
        public string SellerName { get; set; }
        public string ImageId { get; set; }
        public decimal? decimalprice { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySign { get; set; }
        public string CurrencyId { get; set; }
        public string VersionId { get; set; }
        public string TotalSurcharge { get; set; }
        public string WholePrice { get; set; }
        public string WholePriceWithoutAllIn { get; set; }
        public string AllIn { get; set; }
        public string AllInIds { get; set; }
        public string UnitOfMesurmentCode { get; set; }
        public string UnitOfMesurmentId { get; set; }
        public List<SurchargeSummary> SurchargesWithoutAllIn { get; set; }
        public List<SurchargeSummary> AllInSurcharges { get; set; }
        public string SellerId { get;  set; }
        public decimal? MinPrice { get; set; }
        public bool IsMinIconVisible { get; set; }
        public string LineId { get; set; }
        public List<ContainersPrice> ContainersPrices { get; set; }
        public string TransitTime { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? LastUsedDate { get; set; }
        public string ValidityDate { get; set; }
        public int ContainerNumber { get; set; }
        public string NoteMissingContainers { get; set; }
        public string MoreLessDetailsLabel { get; set; }
    }

    public class ContainersPrice
    {
        public string ContainerId { get; set; }
        public string TariffId { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public decimal? Price_WithoutQuantity { get; set; }
    }
}
