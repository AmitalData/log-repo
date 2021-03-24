using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.DataContracts
{
    public class SurchargeSummary
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ChargeTypeId { get; set; }
        public string UnitOfMesurmentCode { get; set; }
        public string UnitOfMesurmentId { get; set; }
        public string TariffId { get; set; }
        public string CurrencyId { get; set; }
        public string TariffNumber { get; set; }
        public string VersionId { get; set; }
        public decimal? Price { get; set; }
        public decimal? ActualPrice { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? ActualMinPrice { get; set; }
        public bool IsMinIconVisible { get; set; }
        public string LineId { get; set; }
        public bool IsAllIn { get; set; }
        public string CurrencySign { get; set; }
        public bool IsDifferentCurrency { get; set; }
        public List<ContainersPrice> ContainersPrices { get; set; }
    }
}
