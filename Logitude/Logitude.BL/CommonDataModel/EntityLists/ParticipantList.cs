using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ParticipantList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int ForwarderTenant { get; set; }
        public string TTY { get; set; }
        public bool Registered { get; set; }
        public bool RegistrationRequested { get; set; }
        public string AccountNumber { get; set; }
        public string EnglishName { get; set; }
        public string Website { get; set; }
        public string VatNumber { get; set; }
        public string LocalName { get; set; }    
        public bool InActive { get; set; }
        public string PaymentTermId { get; set; }
        public string ReceivablesAccountingCard { get; set; }
        public string PayablesAccountingCard { get; set; }
        public string Notes { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string Code { get; set; }
        public string InvoiceCurrencyId { get; set; }
        public string VatTypeId { get; set; }
        public string ComputedLocalName { get; set; }
        public string PartnerTypeId { get; set; }
        public string SearchFields { get; set; }
        public string PrimaryContactId { get; set; }
        public string CityName { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegistrationUpdatedBy { get; set; }
        public string ForwarderTenantName { get; set; }
        public bool IsDirect { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
    }
}
