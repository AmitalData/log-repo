using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Tariff.Models
{
    public class TariffPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string SearchFields { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Name { get; set; }
        public bool InActive { get; set; }
        public string Notes { get; set; }
        public string SellerId { get; set; }
        public string CurrencyId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? LastExpirationDate { get; set; }
        public string PriceSteps { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public DateTime? LastStartDate { get; set; }
        public int LastVersion { get; set; }
        public string ContractNumber { get; set; }
        public string SellerName { get; set; }
        public bool SetAsInActive { get; set; }
        public bool SetAsReActive { get; set; }
        public string TariffNumber { get; set; }
        public List<TariffVersionPM> TariffVersions { get; set; }
        public int TariffLinesAddedNumbers { get; set; }
        public bool TariffLinesAddedFromExcel { get; set; }
        public string ConcurrencyGUID { get; set; }
        public string NewConcurrencyGUID { get; set; }
        public bool IsApprovingDraftVersion { get; set; }
        public bool IsSurchargeUpdate { get; set; }
        public bool IsFromUpdateScreen { get; set; }
        public bool IsFromCopy { get; set; }
        public bool IsUpdatingMissingPorts { get; set; }
        public string TariffProductId { get; set; }
        public string SellerPartnerTypeId { get; set; }
        public bool IsRefreshTranslations { get; set; }
        public string FreightChargeId { get; set; }
        public int ChangeSetOp { get; set; }
        public string Surcharge1Id { get; set; }
        public string Surcharge1UOM { get; set; }
        public string ContainerType1Id { get; set; }
    }
}
