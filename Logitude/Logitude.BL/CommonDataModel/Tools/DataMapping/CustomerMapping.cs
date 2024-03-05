using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CustomerMapping
    {
        public static void MapEntity(CustomerPM entityPM, Customer entityPOCO, bool isNewState, Card entityCard)
        {
            if (isNewState)
            {
                entityCard.Code = entityPM.Code;
                entityCard.Id = entityPM.Id = entityPM.Id;
                entityCard.Tenant = entityPOCO.Tenant = entityPM.Tenant;
                entityCard.CreateDate = entityPM.CreateDate;
                entityCard.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entityPOCO.LogBoxActivated = entityPM.LogBoxActivated;
            entityPOCO.IsPrivateLabelCustomer = entityPM.IsPrivateLabelCustomer;
            entityPOCO.RankId = entityPM.RankId;
            entityPOCO.TeamId = entityPM.TeamId;
            entityPOCO.AccountManagerUserId = entityPM.AccountManagerUserId;
            entityPOCO.SalesmanUserId = entityPM.SalesmanUserId;
            entityPOCO.CollectorId = entityPM.CollectorId;
            entityPOCO.ClassifierId = entityPM.ClassifierId;
            entityPOCO.RegionId = entityPM.RegionId;
            entityPOCO.StartWorkingDate = entityPM.StartWorkingDate;
            entityPOCO.Field1 = entityPM.Field1 != null ? entityPM.Field1.Value : null;
            entityPOCO.Field2 = entityPM.Field2 != null ? entityPM.Field2.Value : null;
            entityPOCO.Field3 = entityPM.Field3 != null ? entityPM.Field3.Value : null;
            entityPOCO.Field4 = entityPM.Field4 != null ? entityPM.Field4.Value : null;
            entityPOCO.Field5 = entityPM.Field5 != null ? entityPM.Field5.Value : null;
            entityPOCO.Field6 = entityPM.Field6 != null ? entityPM.Field6.Value : null;
            entityPOCO.Field7 = entityPM.Field7 != null ? entityPM.Field7.Value : null;
            entityPOCO.Field8 = entityPM.Field8 != null ? entityPM.Field8.Value : null;
            entityPOCO.Field9 = entityPM.Field9 != null ? entityPM.Field9.Value : null;
            entityPOCO.Field10 = entityPM.Field10 != null ? entityPM.Field10.Value : null;
            entityPOCO.LeadSourceId = entityPM.LeadSourceId;
            entityPOCO.IndustryId = entityPM.IndustryId;
            entityPOCO.CreditLimit = entityPM.CreditLimit;
            entityPOCO.LeadDescription = entityPM.LeadDescription;
            entityPOCO.IsCustomer = entityPM.IsCustomer;
            entityPOCO.CustomerStatusCode = entityPM.CustomerStatusCode;
            entityPOCO.FreelancerId = entityPM.FreelancerId;
            entityPOCO.ForwarderId = entityPM.ForwarderId;
            entityPOCO.MediatorId = entityPM.MediatorId;
            entityPOCO.CustomsAgentId = entityPM.CustomsAgentId;
            entityPOCO.BeforeDeactiveStatusCode = entityPM.BeforeDeactiveStatusCode;
            entityPOCO.ReadyForActivationDate = entityPM.ReadyForActivationDate;
            entityPOCO.CustomerSizeId = entityPM.CustomerSizeId;
            entityPOCO.LastCallDate = entityPM.LastCallDate;
            entityPOCO.LastMeetingDate = entityPM.LastMeetingDate;
            entityPOCO.LastOpportunityDate = entityPM.LastOpportunityDate;
            entityPOCO.LastOpportunitySubject = entityPM.LastOpportunitySubject;
            entityPOCO.LastOpportunityStatus = entityPM.LastOpportunityStatus;
            entityPOCO.LastQuoteDate = entityPM.LastQuoteDate;
            entityPOCO.LastInteractionDate = entityPM.LastInteractionDate;
            entityPOCO.ActivityWatch = entityPM.ActivityWatch;
            entityPOCO.KnownConsignor = entityPM.KnownConsignor;
            entityPOCO.KCExpirationDate = entityPM.KCExpirationDate;
            entityPOCO.IsCreditLimitEnabled = entityPM.IsCreditLimitEnabled;
            entityPOCO.CreditLimitAmount = entityPM.CreditLimitAmount;
            entityPOCO.InsuredcreditLimit = entityPM.InsuredcreditLimit;
            entityPOCO.CreditLimitOpenBalance = entityPM.CreditLimitOpenBalance;
            entityPOCO.CreditLimitWarningPercentage = entityPM.CreditLimitWarningPercentage;
            entityPOCO.BlockNewInvoiceCreation = entityPM.BlockNewInvoiceCreation;
            entityPOCO.BlockNewShipmentCreation = entityPM.BlockNewShipmentCreation;
            entityPOCO.PrimaryContactName = entityPM.PrimaryContactName;
            entityPOCO.PrimaryContactEmail = entityPM.PrimaryContactEmail;
            entityPOCO.PrimaryContactPhone = entityPM.PrimaryContactPhone;
            entityPOCO.EmailForSendingSingArinvoice = entityPM.EmailForSendingSingArinvoice;
            entityPOCO.SendingInterestReport = entityPM.SendingInterestReport;
            entityPOCO.ActivatedByUserId = entityPM.ActivatedByUserId;
            entityPOCO.ActivationRequestedByUserId = entityPM.ActivationRequestedByUserId;
            entityPOCO.SetAsInactiveByUserId = entityPM.SetAsInactiveByUserId;
            entityPOCO.ActivationDate = entityPM.ActivationDate;
            entityPOCO.ActivationRequestDate = entityPM.ActivationRequestDate;
            entityPOCO.InactiveDate = entityPM.InactiveDate;
          
            if (!entityPM.IsHybrid || isNewState) //islam: if hybrid and not a new call dont map the field
            {
                entityPOCO.FirstShipmentDate = entityPM.FirstShipmentDate;
                entityPOCO.FirstInvoiceDate = entityPM.FirstInvoiceDate;
            }

            if (entityPOCO.StartWorkingDate != entityPM.StartWorkingDate)
            {
                entityPOCO.StartWorkingManuallySet = true;
                entityPM.StartWorkingManuallySet = true;
            }

            else
            {
                entityPOCO.StartWorkingManuallySet = entityPM.StartWorkingManuallySet;
            }

            entityCard.UpdateDate = entityPM.UpdateDate;
            entityCard.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityCard.EnableConsolidationInvoices = entityPM.EnableConsolidationInvoices;
            entityCard.Website = entityPM.Website;
            entityCard.ReceivablesAccountingCard = entityPM.ReceivablesAccountingCard;
            entityCard.PayablesAccountingCard = entityPM.PayablesAccountingCard;
            entityCard.AccountingVATSplit = entityPM.AccountingVATSplit;
            entityCard.EnglishName = entityPM.EnglishName;
            entityCard.InActive = entityPM.InActive;
            entityCard.LocalName = entityPM.LocalName;
            entityCard.Notes = entityPM.Notes;
            entityCard.SupportNotes = entityPM.SupportNotes;
            entityCard.PartnerTypeId = entityPM.PartnerTypeId;
            entityCard.PaymentTermId = entityPM.PaymentTermId;
            entityCard.VatNumber = entityPM.VatNumber;
            entityCard.InvoiceCurrencyId = entityPM.InvoiceCurrencyId;
            entityCard.VatTypeId = entityPM.VatTypeId;
            entityCard.BankName = entityPM.BankName;
            entityCard.BankAddress = entityPM.BankAddress;
            entityCard.IBANNumber = entityPM.IBANNumber;
            entityCard.Swift = entityPM.Swift;
            entityCard.AccountNumber = entityPM.AccountNumber;
            entityCard.IsCustomer = entityPM.IsCustomer;
            entityCard.SalesmanUserId = entityPM.SalesmanUserId;
            entityCard.IRSNumber = entityPM.IRSNumber;
            entityCard.IRSPlace = entityPM.IRSPlace;
            entityCard.ExternalAccountingBusinessArea = entityPM.ExternalAccountingBusinessArea;
            entityCard.SATPaymentMethodCode = entityPM.PaymentMethodCode;
            entityCard.ExternalId2 = entityPM.ExternalId2;
            entityCard.SATForeignRFC = entityPM.SATForeignRFC;
            entityCard.ClassifierId = entityPM.ClassifierId;
            entityCard.CollectorId = entityPM.CollectorId;
            entityCard.StorageFreeDays = entityPM.StorageFreeDays;
            entityCard.BillToId = entityPM.BillToId;
            entityCard.MetodoPagoCode = entityPM.MetodoPagoCode;
            entityCard.UsoCFDICode = entityPM.UsoCFDICode;
            entityCard.RegimenFiscalCode = entityPM.RegimenFiscalCode;
            entityCard.IsAutonomy = entityPM.IsAutonomy;
            entityCard.SATCustomerName = entityPM.SATCustomerName;
            entityCard.ExportLocalCustomerGroupId = entityPM.ExportLocalCustomerGroupId;
            entityCard.ImportLocalCustomerGroupId = entityPM.ImportLocalCustomerGroupId;
            entityCard.EORInumber = entityPM.EORInumber;
            entityCard.SingleInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.SingleInvoiceTemplateId : null;
            entityCard.CustomsInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.CustomsInvoiceTemplateId : null;
            entityCard.ConsolidationInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.ConsolidationInvoiceTemplateId : null;
            entityCard.ManifestInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.ManifestInvoiceTemplateId : null;

            if (!entityPM.IsFirstContactToAdd)
            {
                entityCard.PrimaryContactId = entityPM.PrimaryContactId;
            }

            else
            {
                entityPM.PrimaryContactId = entityCard.PrimaryContactId;
            }

            entityPM.SetReActivated = false;
            entityPM.SetInActive = false;
            entityPM.SetReady = false;
            entityPM.SetActivated = false;
            entityPM.SavedForActivation = false;
            entityPM.SetAsPotential = false;
            entityPM.CodeMyCustomer = entityPM.IsCustomer ? entityCard.Code + " (Customer)" : entityCard.Code;
            entityCard.CreatedByPartner = entityPM.CreatedByPartner;
            BuildSearchFields(entityPM, entityCard, isNewState);
            BuildCompetitorFields(entityPM, entityPOCO);
        }

        private static void BuildCompetitorFields(CustomerPM entityPM, Customer entityCard)
        {
            #region Competitors Search Field 
            string myCompetitorFields = "";
            var CompetitorIds = entityPM.CustomerCompetitors.Where(p => p.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).Select(p => p.CompetitorId);
            foreach (string item in CompetitorIds)
            {
                MethodHelper.AddToSearchFields(ref myCompetitorFields, item);
            }

            if (myCompetitorFields.Length > 1000)
            {
                myCompetitorFields = myCompetitorFields.Substring(0, 1000);
            }
            entityCard.CompetitorFields = myCompetitorFields;
            entityPM.CompetitorFields = myCompetitorFields;

            #endregion
        }

        private static void BuildSearchFields(CustomerPM entityPM, Card entityCard, bool isNewEntity)
        {
            string mySearchFields = "";

            int tenant = entityPM.Tenant;

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.VatNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReceivablesAccountingCard);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PayablesAccountingCard);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CityName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CountryName);

            #region Contacts
            if (isNewEntity)
            {
                foreach (ContactPM item in entityPM.Contacts)
                {
                    if (item.Email != null)
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, item.Email.ToLower());
                    }

                    MethodHelper.AddToSearchFields(ref mySearchFields, item.EnglishName);
                }
            }

            else
            {
                CardContactRepository cardContactRepository = new CardContactRepository(tenant);
                List<Contact> myContacts = cardContactRepository.GetContactsByCardId(entityPM.Id).ToList();
                foreach (Contact item in myContacts)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.Email);
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.EnglishName);

                    item.CompanyName = entityPM.EnglishName;
                    BuildContactSearchFields(item);
                }

                cardContactRepository.SubmitChanges();
            }
            #endregion

            #region Custom Fields
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Customer", tenant).Where(o => o.DataTypeCode == "Text" || o.DataTypeCode == "nText").ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            foreach (ObjectField field in customFields)
            {
                object value = customFieldResolver.GetFieldValue(entityPM, field, tenant);
                if (value != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, value.ToString());
                }
            }
            #endregion

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityCard.SearchFields = mySearchFields;
        }

        private static void BuildContactSearchFields(Contact entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPOCO.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPOCO.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPOCO.Email);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPOCO.BusinessPhone);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPOCO.Mobile);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPOCO.Fax);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPOCO.CompanyName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPOCO.SearchFields = mySearchFields;
        }

        public static CustomerPM GetMappedPMFromPoco(Customer a)
        {
            CustomerPM pm = new CustomerPM()
            {
                BillToId = a.Card.BillToId,
                BillToName = a.Card.EnglishName,
                Id = a.Id,
                RankId = a.RankId,
                TeamId = a.TeamId,
                AccountManagerUserId = a.AccountManagerUserId,
                SalesmanUserId = a.SalesmanUserId,
                SalesmanUserEnglishName = a.SalesmanUser == null ? null : (a.SalesmanUser.Contact == null ? null : a.SalesmanUser.Contact.EnglishName),
                SalesmanBusinessUnitId = a.SalesmanUser == null ? null : a.SalesmanUser.BusinessUnitId,
                Tenant = a.Tenant,
                Website = a.Card.Website,
                Code = a.Card.Code,
                LocalName = a.Card.LocalName,
                EnglishName = a.Card.EnglishName,
                CardPMId = a.Id,
                ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                PayablesAccountingCard = a.Card.PayablesAccountingCard,
                AccountingVATSplit = a.Card.AccountingVATSplit,
                CreateDate = a.Card.CreateDate,
                UpdateDate = a.Card.UpdateDate,
                CreatedByUserId = a.Card.CreatedByUserId,
                UpdatedByUserId = a.Card.UpdatedByUserId,
                InActive = a.Card.InActive,
                Notes = a.Card.Notes,
                SupportNotes = a.Card.SupportNotes,
                PartnerTypeId = a.Card.PartnerTypeId,
                PaymentTermId = a.Card.PaymentTermId,
                VatNumber = a.Card.VatNumber,
                InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                ComputedLocalName = string.IsNullOrEmpty(a.Card.LocalName) ? a.Card.EnglishName : a.Card.LocalName,
                AccountManagerUserEnglishName = a.AccountManagerUser != null ? a.AccountManagerUser.Contact.EnglishName : null,
                CityName = a.Card.CityName,
                RankCode = a.Rank != null ? a.Rank.Code : null,
                RankName = a.Rank != null ? a.Rank.Name : null,
                VatTypeId = a.Card.VatTypeId,
                ImageDetailId = a.Card.ImageDetailId,
                BankName = a.Card.BankName,
                BankAddress = a.Card.BankAddress,
                IBANNumber = a.Card.IBANNumber,
                Swift = a.Card.Swift,
                AccountNumber = a.Card.AccountNumber,
                SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                CargoTrackingInvitationStatusName = a.Card.CargoTrackingInvitationStatus != null ? a.Card.CargoTrackingInvitationStatus.Name : null,
                IsActiveForMobile = a.Card.IsActiveForMobile,
                LastLoginDate = a.Card.LastLoginDate,
                InvitationDate = a.Card.InvitationDate,
                CargoTrackingInvitationDate = a.Card.CargoTrackingInvitationDate,
                LeadSourceId = a.LeadSourceId,
                IndustryId = a.IndustryId,
                ClassifierId = a.ClassifierId,
                CollectorId = a.CollectorId,
                LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                IndustryName = a.Industry != null ? a.Industry.Name : null,
                ClassifierName = a.Classifier != null ? a.Classifier.Contact.EnglishName : null,
                CollectorName = a.Collector != null ? a.Collector.Contact.EnglishName : null,
                CreditLimit = a.CreditLimit,
                LeadDescription = a.LeadDescription,
                IsCustomer = a.IsCustomer,
                FreelancerId = a.FreelancerId,
                FreelancerName = a.Freelancer != null ? a.Freelancer.Contact.EnglishName : null,
                CustomerStatusCode = a.CustomerStatusCode,
                ForwarderId = a.ForwarderId,
                ForwarderName = a.Forwarder != null ? a.Forwarder.EnglishName : null,
                CustomsAgentId = a.CustomsAgentId,
                CustomsAgentName = a.CustomsAgent != null ? a.CustomsAgent.EnglishName : null,
                MediatorId = a.MediatorId,
                MediatorName = a.Mediator != null ? a.Mediator.EnglishName : null,
                BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
                PrimaryContactName = a.PrimaryContactName,
                PrimaryContactEmail = a.PrimaryContactEmail,
                EmailForSendingSingArinvoice = a.EmailForSendingSingArinvoice,
                SendingInterestReport = a.SendingInterestReport,
                PrimaryContactPhone = a.PrimaryContactPhone,
                CustomerStatusName = a.CustomerStatus != null ? a.CustomerStatus.Name : null,
                PrimaryContactId = a.Card.PrimaryContactId,
                ReadyForActivationDate = a.ReadyForActivationDate,
                RegionId = a.RegionId,
                RegionName = a.Region != null ? a.Region.Name : null,
                CustomerSizeId = a.CustomerSizeId,
                LastCallDate = a.LastCallDate,
                LastMeetingDate = a.LastMeetingDate,
                LastOpportunityDate = a.LastOpportunityDate,
                LastOpportunityStatus = a.LastOpportunityStatus,
                LastOpportunitySubject = a.LastOpportunitySubject,
                FirstInvoiceDate = a.FirstInvoiceDate,
                FirstShipmentDate = a.FirstShipmentDate,
                LastShipmentDate = a.LastShipmentDate,
                StartWorkingDate = a.StartWorkingDate,
                StartWorkingManuallySet = a.StartWorkingManuallySet,
                LastQuoteDate = a.LastQuoteDate,
                LastInteractionDate = a.LastInteractionDate,
                EnableConsolidationInvoices = a.Card.EnableConsolidationInvoices,
                ActivityWatch = a.ActivityWatch,
                KnownConsignor = a.KnownConsignor,
                KCExpirationDate = a.KCExpirationDate,
                LogBoxActivated = a.LogBoxActivated,
                IRSNumber = a.Card.IRSNumber,
                IRSPlace = a.Card.IRSPlace,
                IsPrivateLabelCustomer = a.IsPrivateLabelCustomer,
                Card = new CardPM()
                {
                    Id = a.Id,
                    Tenant = a.Tenant,
                    EnglishName = a.Card.EnglishName,
                    CityName = a.Card.CityName,
                    CountryId = a.Card.CountryId,
                    CountryName = a.Card.CountryName,
                    PrimaryContactId = a.Card.PrimaryContactId,
                    ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                    PayablesAccountingCard = a.Card.PayablesAccountingCard,
                    AccountingVATSplit = a.Card.AccountingVATSplit,
                },
            };
            return pm;

        }
    }
}
