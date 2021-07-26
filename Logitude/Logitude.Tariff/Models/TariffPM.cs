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
        public string NewConcurrencyGUID { get; set; }
        public string ConcurrencyGUID { get; set; }
        public string TypeCode { get; set; }
        public string FreightChargeId { get; set; }
        public string CurrencyId { get; set; }
        public string Name { get; set; }
        public string ContractNumber { get; set; }
        public string SellerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string TariffProductId { get; set; }
        public string Notes { get; set; }
        public List<TariffVersionPM> TariffVersions { get; set; }
    }
}
