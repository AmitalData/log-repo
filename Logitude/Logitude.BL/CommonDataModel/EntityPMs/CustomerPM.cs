using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [DataContract]
    public class CustomerPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public bool IsSecured { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CompetitorFields { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BankAddress { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Swift { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountNumber { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IBANNumber { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public DateTime? StartWorkingDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Notes { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string SupportNotes { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Code { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string EnglishName { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string LocalName { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string AccountManagerUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string SalesmanUserId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SalesmanUserEnglishName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SalesmanBusinessUnitId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Website { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool InActive { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string BillToId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BillToName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string VatNumber { get; set; }//card

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ReceivablesAccountingCard { get; set; }//card

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EORInumber { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PayablesAccountingCard { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string InvoiceCurrencyId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string PaymentTermId { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string RankId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string TeamId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember] 
        public string VatTypeId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ComputedLocalName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool FieldsChanged { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PartnerTypeId { get; set; }//card

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string CreatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string UpdatedByUserId { get; set; }

        [DataMember]
        public string SearchFields { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CardPMId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExistedContactId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool StartWorkingManuallySet { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CityName { get; set; }

        public string CityCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CountryName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ATTN { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string AccountManagerUserEnglishName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RankName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string TeamName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RankCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PhoneNumber { get; set; }// main address phone

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FaxNumber { get; set; }//main address fax

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CityWithCountry { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ImageDetailId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field1 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field2 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field3 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field4 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field5 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field6 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field7 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field8 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field9 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public CustomFieldClass Field10 { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SharedLogisticsInvitationStatusName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsActiveForMobile { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastLoginDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? InvitationDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsHybrid { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreelancerId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ForwarderId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomsAgentId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MediatorId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string FreelancerName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ForwarderName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomsAgentName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string MediatorName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string GoogleAddressString { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsExternal { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsCustomer { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerStatusCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BeforeDeactiveStatusCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ActivationDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? InactiveDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ActivationRequestDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ActivatedByUserId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SetAsInactiveByUserId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ActivationRequestedByUserId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool SetReActivated { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool SetInActive { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool SetReady { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool SetActivated { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventNote { get; set; }

        //------------Address Fields------------

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Address1_Potential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Address2_Potential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string City_Potential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string CountryId_Potential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string ZipCode_Potential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string StateId_Potential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string PhoneNumber_Potential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string FaxNumber_Potential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string ATTN_Potential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool IsLocalLanguage { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CodeMyCustomer { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrimaryContactName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrimaryContactEmail { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string PrimaryContactPhone { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string CustomerStatusName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PrimaryContactId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Phone { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? ReadyForActivationDate { get; set; }

         [DataMember]
         [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsFirstContactToAdd { get; set; }

         [DataMember]
         public bool SavedForActivation { get; set; }

        [DataMember]
        public Guid QueueMessageLockToken { get; set; }
        [DataMember]
        public string QuestionnaireAnsewrsHtmlString { get; set; }

        [DataMember]
        public bool SetAsPotential { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string UpdatedByUserCode { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IRSPlace { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IRSNumber { get; set; }

        [DataMember]
        public string RequestedAirlines { get; set; }
        [DataMember]
        public string RegisteredAirlines { get; set; }
        [DataMember]
        public string PendingAirlines { get; set; }

        [DataMember]
        public bool IsCustomerAllowed { get; set; }

        [Include]
        [Association("CustomerCardPM", "Id", "Id", IsForeignKey = true)]
        [DataMember]
        public CardPM Card { get; set; }

        private List<AddressPM> addresses;
        [Include]
        [Association("CustomerPMAddressPM", "Id", "CardId")]
        [DataMember]
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
        [Association("CustomerPMContactPM", "Id", "CardId")]
        [DataMember]
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

        private List<SharedLogisticContactPM> sharedLogisticContacts;
        [Include]
        [Association("CustomerPMSharedLogisticContactPM", "Id", "CardId")]
        [DataMember]
        public virtual List<SharedLogisticContactPM> SharedLogisticContacts
        {
            get
            {
                if (sharedLogisticContacts == null)
                {
                    sharedLogisticContacts = new List<SharedLogisticContactPM>();
                }
                return sharedLogisticContacts;
            }

            set { sharedLogisticContacts = value; }
        }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IndustryId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LeadSourceId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CollectorId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ClassifierId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string IndustryName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LeadSourceName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CollectorName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ClassifierName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public double? CreditLimit { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LeadDescription { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RegionId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string RegionName { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string CustomerSizeId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastCallDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastMeetingDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastOpportunityDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastOpportunitySubject { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string LastOpportunityStatus { get; set; }


        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstInvoiceDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? FirstShipmentDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastShipmentDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastQuoteDate { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? LastInteractionDate { get; set; }

        [DataMember]
        public bool EnableConsolidationInvoices { get; set; }

        [DataMember]
        public bool ActivityWatch { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string KnownConsignor { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? KCExpirationDate { get; set; }
       
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool LogBoxActivated { get; set; }

        private List<CustomerProductPM> customerProducts;
        [Include]
        [Association("CustomerProductCustomer", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<CustomerProductPM> CustomerProducts
        {
            get
            {

                if (this.customerProducts == null)
                {
                    customerProducts = new List<CustomerProductPM>();
                }
                return this.customerProducts;
            }
            set
            {
                if (value != null)
                {
                    customerProducts = value;
                }
            }
        }

        private List<CustomerProductActualDataPM> customerProductActualDatas;
        [Include]
        [Association("CustomerProductActualDataCustomer", "Id", "CustomerId")]
        [Composition]
        public virtual List<CustomerProductActualDataPM> CustomerProductActualDatas
        {
            get
            {

                if (this.customerProductActualDatas == null)
                {
                    customerProductActualDatas = new List<CustomerProductActualDataPM>();
                }
                return this.customerProductActualDatas;
            }
            set
            {
                if (value != null)
                {
                    customerProductActualDatas = value;
                }
            }
        }

        private List<CustomerCompetitorPM> customerCompetitors;
        [Include]
        [Association("CustomerCompetitorCustomer", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<CustomerCompetitorPM> CustomerCompetitors
        {
            get
            {
                if (this.customerCompetitors == null)
                {
                    customerCompetitors = new List<CustomerCompetitorPM>();
                }
                return this.customerCompetitors;
            }
            set
            {
                if (value != null)
                {
                    customerCompetitors = value;
                }
            }
        }

        private List<CustomerAdditionalServicePM> customerAdditionalServices;
        [Include]
        [Association("CustomerAdditionalServiceCustomer", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<CustomerAdditionalServicePM> CustomerAdditionalServices
        {
            get
            {
                if (this.customerAdditionalServices == null)
                {
                    customerAdditionalServices = new List<CustomerAdditionalServicePM>();
                }
                return this.customerAdditionalServices;
            }
            set
            {
                if (value != null)
                {
                    customerAdditionalServices = value;
                }
            }
        }

        private List<CustomerSalesNotePM> salesNotes;
        [Include]
        [Association("CustomerPMCustomerSalesNotePM", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<CustomerSalesNotePM> SalesNotes
        {
            get
            {
                if (salesNotes == null)
                {
                    salesNotes = new List<CustomerSalesNotePM>();
                }

                return salesNotes;
            }

            set
            {
                salesNotes = value;
            }
        }

        private List<CustomerSalesmanByProductPM> customerSalesmanByProduct;
        [Include]
        [Association("CustomerPMCustomerSalesmanByProductPM", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<CustomerSalesmanByProductPM> CustomerSalesmanByProducts
        {
            get
            {
                if (customerSalesmanByProduct == null)
                {
                    customerSalesmanByProduct = new List<CustomerSalesmanByProductPM>();
                }

                return customerSalesmanByProduct;
            }

            set
            {
                customerSalesmanByProduct = value;
            }
        }



        private List<CustomerAccountManagerByProductPM> customerAccountManagerByProducts;
        [Include]
        [Association("CustomerAccountManagerByProductPM", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<CustomerAccountManagerByProductPM> CustomerAccountManagerByProducts
        {
            get
            {
                if (customerAccountManagerByProducts == null)
                {
                    customerAccountManagerByProducts = new List<CustomerAccountManagerByProductPM>();
                }

                return customerAccountManagerByProducts;
            }

            set
            {
                customerAccountManagerByProducts = value;
            }
        }

        private List<CustomerCustomsAgentByProductPM> customerCustomsAgentByProducts;
        [Include]
        [Association("CustomerCustomsAgentByProductPM", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<CustomerCustomsAgentByProductPM> CustomerCustomsAgentByProducts
        {
            get
            {
                if (customerCustomsAgentByProducts == null)
                {
                    customerCustomsAgentByProducts = new List<CustomerCustomsAgentByProductPM>();
                }

                return customerCustomsAgentByProducts;
            }

            set
            {
                customerCustomsAgentByProducts = value;
            }
        }




        private List<CustomerForwarderByProductPM> customerForwarderByProducts;
        [Include]
        [Association("CustomerForwarderByProductPM", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<CustomerForwarderByProductPM> CustomerForwarderByProducts
        {
            get
            {
                if (customerForwarderByProducts == null)
                {
                    customerForwarderByProducts = new List<CustomerForwarderByProductPM>();
                }

                return customerForwarderByProducts;
            }

            set
            {
                customerForwarderByProducts = value;
            }
        }

        private List<CustomerMediatorByProductPM> customerMediatorByProducts;
        [Include]
        [Association("CustomerMediatorByProductPM", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<CustomerMediatorByProductPM> CustomerMediatorByProducts
        {
            get
            {
                if (customerMediatorByProducts == null)
                {
                    customerMediatorByProducts = new List<CustomerMediatorByProductPM>();
                }

                return customerMediatorByProducts;
            }

            set
            {
                customerMediatorByProducts = value;
            }
        }


         

        private List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrencies;
        [Include]
        [Association("CardExternalCodeByCurrencyPM", "Id", "CardId")]
        [Composition]
        [DataMember]
        public virtual List<CardExternalCodeByCurrencyPM> CardExternalCodeByCurrencies
        {
            get
            {
                if (cardExternalCodeByCurrencies == null)
                {
                    cardExternalCodeByCurrencies = new List<CardExternalCodeByCurrencyPM>();
                }

                return cardExternalCodeByCurrencies;
            }

            set
            {
                cardExternalCodeByCurrencies = value;
            }
        }

        private List<ProductItemPM> customerProductItems;
        [Include]
        [Association("CustomerProductItemCustomer", "Id", "CustomerId")]
        [Composition]
        [DataMember]
        public virtual List<ProductItemPM> CustomerProductItems
        {
            get
            {
                if (customerProductItems == null)
                {
                    customerProductItems = new List<ProductItemPM>();
                }

                return customerProductItems;
            }

            set
            {
                customerProductItems = value;
            }
        }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsLogBox { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool IsPrivateLabelCustomer { get; set; }

        [DataMember]
        public bool IsCreditLimitEnabled { get; set; }

        [DataMember]
        public double? CreditLimitAmount { get; set; }

        [DataMember]
        public double? InsuredcreditLimit { get; set; }

        [DataMember]
        public double? CreditLimitOpenBalance { get; set; }

        [DataMember]
        public int? CreditLimitWarningPercentage { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string ExternalAccountingBusinessArea { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string PaymentMethodCode { get; set; }

        [DataMember]
        public bool BlockNewInvoiceCreation { get; set; }

        [DataMember]
        public bool BlockNewShipmentCreation { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string ExternalId2 { get; set; }



        [DataMember]
        public string SATForeignRFC { get; set; }

        [DataMember]
        public string MetodoPagoCode { get; set; }

        [DataMember]
        public string UsoCFDICode { get; set; }
        [DataMember]
        public string RegimenFiscalCode { get; set; }

        [DataMember]
        public int CustomerTenant { get; set; }

        public string MainAddressId { get; set; }
        public string BillingAddressId { get; set; }
        public string GLAccountId { get; set; }
        public string CreatedByPartner { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public int? StorageFreeDays { get; set; }

      
        [DataMember]
        public bool AccountingVATSplit { get; set; }

        [DataMember]
        public string UploadingUniqueKey { get; set; }

        [DataMember]
        public string GLAccountNumber { get; set; }

        [DataMember]
        public bool IsAutonomy { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public bool AddLogboxCustomerQueue { get; set; }

    }
}
