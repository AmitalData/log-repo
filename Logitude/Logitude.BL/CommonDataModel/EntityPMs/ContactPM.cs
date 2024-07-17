using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [DataContract]
    public class ContactPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool InActive { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string FacebookId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string EnglishName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ImageDetailId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string LocalName { get; set; }

        [DataMember]
        public string ComputedLocalName { get; set; }

        [MyRegularExpressionAttribute(@"^(([^<>()[\]\\.,;:\s@\""]+"
+ @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
+ @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
+ @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
+ @"[a-zA-Z]{2,}))$", ErrorMessage = "Invalid email format!")]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public bool HasPassword { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Password { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string BusinessPhone { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Mobile { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Fax { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public DateTime? Birthday { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public DateTime? Anniversary { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public string SearchFields { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CardId { get; set; }

        [DataMember]
        public bool IsUser { get; set; }

        [DataMember]
        public bool SignupRole { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public byte[] Signature { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public byte[] SignatureHtml { get; set; }

        




        [DataMember]
        public bool DisplayGettingStarted { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalId { get; set; }

        [DataMember]
        public bool IsHybrid { get; set; }

        [DataMember]
        public bool DontShowLocal { get; set; }

        [DataMember]
        public bool DontShowLocalLabels { get; set; }


        [DataMember]
        public bool MustChangePassword { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int NumberOfRetries { get; set; }

        [DataMember]
        public bool DisconectFromCard { get; set; }

        [DataMember]
        public bool FieldsChanged { get; set; }

        [DataMember]
        public bool SetAsPrimaryForCard { get; set; }

        [Include]
        [Association("ShippingAgentPMContactPM", "CardId", "Id", IsForeignKey = true)]
        public ShippingAgentPM ShippingAgent { get; set; }

        [DataMember]
        public bool BirthdayReminder { get; set; }

        [DataMember]
        public bool AnniversaryReminder { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public DateTime? DoneDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public int? BirthDayOfYear { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string ContactDoneMethodCode { get; set; }

        [DataMember]
        public string ContactDoneMethodName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Position { get; set; }

        [DataMember]
        public bool IsAirExport { get; set; }
        [DataMember]
        public bool IsAirImport { get; set; }
        [DataMember]
        public bool IsOceanExport { get; set; }
        [DataMember]
        public bool IsOceanImport { get; set; }
        [DataMember]
        public bool IsInlandExport { get; set; }
        [DataMember]
        public bool IsInlandImport { get; set; }
        [DataMember]
        public bool IsAll { get; set; }
        [DataMember]
        public bool HasCardContact { get; set; }
        [DataMember]
        public bool IsCustomsImport { get; set; }
        [DataMember]
        public bool IsInlandDomestic { get; set; }

        [DataMember]
        public int? IndexColor { get; set; }
       
        [DataMember]
        public bool IsChangeSignatur { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerId { get; set; }

        [DataMember]
        public bool IsCreatedWithPartner { get; set; }
        [DataMember]
        public bool IsAPIContact { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }

        [DataMember]
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [DataMember]
        public bool IsUserAdditionalPackagesOnly { get; set; }

        [DataMember]
        public bool IsLicencedUser { get; set; }

        private List<CardContactAdditionalServicePM> cardContactAdditionalServices;
        [Include]
        [Association("CardContactAdditionalServiceContact", "Id", "ContactId")]
        [Composition]
        [DataMember]
        public virtual List<CardContactAdditionalServicePM> CardContactAdditionalServices
        {
            get
            {

                if (this.cardContactAdditionalServices == null)
                {
                    cardContactAdditionalServices = new List<CardContactAdditionalServicePM>();
                }
                return this.cardContactAdditionalServices;
            }
            set
            {
                if (value != null)
                {
                    cardContactAdditionalServices = value;
                }
            }
        }

        private List<CardContactProductPM> cardContactProducts;
        [Include]
        [Association("CardContactProductContact", "Id", "ContactId")]
        [Composition]
        [DataMember]
        public virtual List<CardContactProductPM> CardContactProducts
        {
            get
            {

                if (this.cardContactProducts == null)
                {
                    cardContactProducts = new List<CardContactProductPM>();
                }
                return this.cardContactProducts;
            }
            set
            {
                if (value != null)
                {
                    cardContactProducts = value;
                }
            }
        }

        public string OldSimilarInactiveContactId { get; set; }
        [DataMember]
        public string DigitalPortalCardId { get; set; }

        [DataMember]
        public string TimeZone { get; set; }

        [DataMember]
        public string DigitalPortalLanguage { get; set; }

        [DataMember]
        public bool? ContactForAccounting { get; set; }


    }
}
