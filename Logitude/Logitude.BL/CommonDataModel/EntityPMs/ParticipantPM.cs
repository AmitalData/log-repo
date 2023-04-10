using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class ParticipantPM : ObjectCustomFieldPM
    {
        [Key]
        public string Id { get; set; }        
        public int Tenant { get; set; }
        public bool IsSecured { get; set; }
        public bool IsHybrid { get; set; }
        public int ForwarderTenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TTY { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool Registered { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool RegistrationRequested { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankAddress { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Swift { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IBANNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]        
        public string EnglishName { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Website { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatNumber { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LocalName { get; set; }//card     

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool InActive { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PaymentTermId { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivablesAccountingCard { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayablesAccountingCard { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Notes { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Code { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string InvoiceCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string VatTypeId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ComputedLocalName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PartnerTypeId { get; set; }//card

        public string SearchFields { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrimaryContactId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExistedContactId { get; set; }

        public bool IsFirstContactToAdd { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CityName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryCode { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryName { get; set; }
        public string RegistrationUpdatedBy { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool IsDirect { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FWBNotifyContacts { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FHLNotifyContacts { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FFRNotifyContacts { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        [Include]
        [Association("ParticipantCardPM", "Id", "Id")]
        public virtual CardPM Card { get; set; }

        private List<AddressPM> addresses;
        [Include]
        [Association("ParticipantPMAddressPM", "Id", "CardId")]
        public virtual List<AddressPM> Addresses
        {
            get
            {
                if (addresses == null)
                {
                    addresses = new List<AddressPM>();
                }
                return addresses;
            }
            set { addresses = value; }
        }
        private List<ContactPM> contacts;
        [Include]
        [Association("ParticipantPMContactPM", "Id", "CardId")]
        public virtual List<ContactPM> Contacts
        {
            get
            {
                if (contacts == null)
                {
                    contacts = new List<ContactPM>();
                }
                return contacts;
            }
            set { contacts = value; }
        }
    }
}
