using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class AccountingPartner
    {
        [Key, ForeignKey("Card")] 
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public double? CreditLimit { get; set; }
        public double? InsuredCreditlimit { get; set; }
        public virtual Card Card { get; set; }
    }
}