using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using System.Data.Entity.Core.Objects;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.Server.Tools;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.IO;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.CustomFilters;
using System.Reflection;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerQuery
    {
        CustomerRepository repository;
        public CustomerQuery()
        {
            repository = new CustomerRepository();
        }
        public CustomerQuery(int tenant)
        {
            repository = new CustomerRepository(tenant);
        }
        public CustomerQuery(CustomerRepository repository)
        {
            this.repository = repository;
        }

        public CustomerPM GetSinglePM(string id, int tenant)
        {
            bool getFromCache = false;
            string entityName = "CustomerPM" + id + tenant;
            CustomerPM entity;

            if (HttpContext.Current != null && getFromCache)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Rank").Include("Card.SharedLogisticsInvitationStatus").Include("Collector.Contact").Include("Classifier.Contact").Include("Freelancer.Contact").Include("Forwarder").Include("CustomsAgent").Include("Mediator").Include("LeadSource").Include("CustomerStatus").Include("ActivatedByUser.Contact").Include("SetAsInactiveByUser.Contact").Include("ActivationRequestedByUser.Contact")
                              where a.Tenant == tenant && a.Id == id
                              select new CustomerPM()
                              {
                                  BillToId = a.BillToId,
                                  BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                  Id = a.Id,
                                  RankId = a.RankId,
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
                                  IsActiveForMobile = a.Card.IsActiveForMobile,
                                  LastLoginDate = a.Card.LastLoginDate,
                                  InvitationDate = a.Card.InvitationDate,
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
                                  IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                  CreditLimitAmount = a.CreditLimitAmount,
                                  CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                                  CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                                  ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                  PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                  BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                  BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                                  ExternalId2 = a.Card.ExternalId2,
                                  SATForeignRFC = a.Card.SATForeignRFC,
                                  MetodoPagoCode = a.Card.MetodoPagoCode,
                                  UsoCFDICode = a.Card.UsoCFDICode,
                                  CompetitorFields = a.CompetitorFields,
                                  ActivationDate = a.ActivationDate,
                                  InactiveDate = a.InactiveDate,
                                  ActivationRequestDate = a.ActivationRequestDate,
                                  ActivatedByUserId = a.ActivatedByUserId,
                                  SetAsInactiveByUserId = a.SetAsInactiveByUserId,
                                  ActivationRequestedByUserId = a.ActivationRequestedByUserId,
                                  GLAccountId = a.Card.GLAccountId,
                                  CreatedByPartner = a.Card.CreatedByPartner,
                                  StorageFreeDays = a.Card.StorageFreeDays,
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
                                      GLAccountId = a.Card.GLAccountId,
                                  },

                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        this.SetCustomerAddressData(entity);

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }

                else
                {
                    entity = (CustomerPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            else
            {
                entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Rank").Include("Card.SharedLogisticsInvitationStatus").Include("Collector.Contact").Include("Classifier.Contact").Include("Freelancer.Contact").Include("Forwarder").Include("CustomsAgent").Include("Mediator").Include("LeadSource").Include("CustomerStatus").Include("ActivatedByUser.Contact").Include("SetAsInactiveByUser.Contact").Include("ActivationRequestedByUser.Contact")
                          where a.Id == id && a.Tenant == tenant
                          select new CustomerPM()
                          {
                              BillToId = a.BillToId,
                              BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                              Id = a.Id,
                              RankId = a.RankId,
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
                              ImageDetailId = a.Card.ImageDetailId,
                              VatTypeId = a.Card.VatTypeId,
                              BankName = a.Card.BankName,
                              BankAddress = a.Card.BankAddress,
                              IBANNumber = a.Card.IBANNumber,
                              Swift = a.Card.Swift,
                              AccountNumber = a.Card.AccountNumber,
                              SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                              IsActiveForMobile = a.Card.IsActiveForMobile,
                              LastLoginDate = a.Card.LastLoginDate,
                              InvitationDate = a.Card.InvitationDate,
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
                              IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                              CreditLimitAmount = a.CreditLimitAmount,
                              CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                              CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                              ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                              PaymentMethodCode = a.Card.SATPaymentMethodCode,
                              BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                              BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                              ExternalId2 = a.Card.ExternalId2,
                              SATForeignRFC = a.Card.SATForeignRFC,
                              MetodoPagoCode = a.Card.MetodoPagoCode,
                              UsoCFDICode = a.Card.UsoCFDICode,
                              CompetitorFields = a.CompetitorFields,
                              ActivationDate = a.ActivationDate,
                              InactiveDate = a.InactiveDate,
                              ActivationRequestDate = a.ActivationRequestDate,
                              ActivatedByUserId = a.ActivatedByUserId,
                              SetAsInactiveByUserId = a.SetAsInactiveByUserId,
                              ActivationRequestedByUserId = a.ActivationRequestedByUserId,
                              GLAccountId = a.Card.GLAccountId,
                              CreatedByPartner = a.Card.CreatedByPartner,
                              StorageFreeDays = a.Card.StorageFreeDays,
                              Card = new CardPM
                              {
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                                  EnglishName = a.Card.EnglishName,
                                  PrimaryContactId = a.Card.PrimaryContactId,
                                  ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                  PayablesAccountingCard = a.Card.PayablesAccountingCard,
                                  GLAccountId = a.Card.GLAccountId,
                              },

                          }).FirstOrDefault();

                if (entity != null)
                {
                    this.SetCustomerAddressData(entity);
                }
            }

            if (entity != null)
            {
                CustomerProductRepository customerProductRepository = new CustomerProductRepository(repository.context);
                CustomerCompetitorRepository customerCompetitorRepository = new CustomerCompetitorRepository(repository.context);
                CustomerAdditionalServiceRepository customerAdditionalServiceRepository = new CustomerAdditionalServiceRepository(repository.context);
                CustomerSalesmanByProductRepository customerSalesmanByProductRepository = new CustomerSalesmanByProductRepository(repository.context);
                CustomerAccountManagerByProductRepository customerAccountManagerByProductRepository = new CustomerAccountManagerByProductRepository(repository.context);
                CustomerForwarderByProductRepository customerForwarderByProductRepository = new CustomerForwarderByProductRepository(repository.context);
                CustomerCustomsAgentByProductRepository customerCustomsAgentByProductRepository = new CustomerCustomsAgentByProductRepository(repository.context);
                CustomerMediatorByProductRepository customerMediatorByProductRepository = new CustomerMediatorByProductRepository(repository.context);
                CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);

                CustomerProductQuery customerProductQuery = new CustomerProductQuery(customerProductRepository);
                CustomerCompetitorQuery customerCompetitorQuery = new CustomerCompetitorQuery(customerCompetitorRepository);
                CustomerAdditionalServiceQuery customerAdditionalServiceQuery = new CustomerAdditionalServiceQuery(customerAdditionalServiceRepository);
                CustomerSalesmanByProductQuery customerSalesmanByProductQuery = new CustomerSalesmanByProductQuery(customerSalesmanByProductRepository);
                CustomerAccountManagerByProductQuery customerAccountManagerByProductQuery = new CustomerAccountManagerByProductQuery(customerAccountManagerByProductRepository);
                CustomerForwarderByProductQuery customerForwarderByProductQuery = new CustomerForwarderByProductQuery(customerForwarderByProductRepository);
                CustomerCustomsAgentByProductQuery customerCustomsAgentByProductQuery = new CustomerCustomsAgentByProductQuery(customerCustomsAgentByProductRepository);
                CustomerMediatorByProductQuery customerMediatorByProductQuery = new CustomerMediatorByProductQuery(customerMediatorByProductRepository);
                CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);

                entity.CustomerProducts = customerProductQuery.GetCustomerProductPMsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerCompetitors = customerCompetitorQuery.GetCustomerCompetitorsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerAdditionalServices = customerAdditionalServiceQuery.GetCustomerAdditionalServicesByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerSalesmanByProducts = customerSalesmanByProductQuery.GetCustomerSalesmanByProductPMs(entity.Tenant, entity.Id);

                entity.CustomerAccountManagerByProducts = customerAccountManagerByProductQuery.GetCustomerAccountManagerByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerForwarderByProducts = customerForwarderByProductQuery.GetCustomerForwarderByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerCustomsAgentByProducts = customerCustomsAgentByProductQuery.GetCustomerCustomsAgentByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerMediatorByProducts = customerMediatorByProductQuery.GetCustomerMediatorByProductPMs(entity.Tenant, entity.Id);
                entity.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(entity.Id, entity.Tenant);

                CustomerSalesNoteRepository salesNoteRepository = new CustomerSalesNoteRepository(repository.context);
                CustomerSalesNoteQuery salesNoteQuery = new CustomerSalesNoteQuery(salesNoteRepository);
                entity.SalesNotes = salesNoteQuery.GetSalesNotesByCustomerId(entity.Id, entity.Tenant).ToList();

                entity.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        entity.IsExternal = true;
                    }
                }

                if (!string.IsNullOrEmpty(entity.ReceivablesAccountingCard))
                {
                    int accountingCard = 0;
                    bool isParsed = Int32.TryParse(entity.ReceivablesAccountingCard, out accountingCard);

                    if (isParsed)
                    {
                        TenantManagement tenantManagement = null;

                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                            tenantManagement = tenantManagementRepository.GetSingleTenantManagement(accountingCard);

                            scope.Complete();
                        }

                        if (tenantManagement != null)
                        {
                            AirlineRepository airlineRepository = new AirlineRepository(repository.context);
                            List<Airline> tenantZeroAirlines = airlineRepository.GetAirlines(0).ToList();

                            if (tenantZeroAirlines.Count > 0)
                            {
                                if (tenantManagement.AWBMessagesCCSTypeCode == "GLSHK")
                                {
                                    tenantZeroAirlines = tenantZeroAirlines.Where(d => d.GLSHKPIMA != null).ToList();
                                }

                                else
                                {
                                    tenantZeroAirlines = tenantZeroAirlines.Where(d => d.TTY != null).ToList();
                                }

                                string requested = "";
                                string registered = "";
                                string pending = "";

                                List<Airline> currenctTenantAilines = airlineRepository.GetAirlines(tenantManagement.Id).ToList();
                                foreach (Airline tenantZeroItem in tenantZeroAirlines)
                                {
                                    Airline myTenantItem = currenctTenantAilines.Where(d => d.Card.Code == tenantZeroItem.Card.Code).FirstOrDefault();

                                    if (myTenantItem != null)
                                    {
                                        if (tenantManagement.AWBMessagesCCSTypeCode == "GLSHK")
                                        {
                                            if (myTenantItem.GLSHKRegistrationRequested)
                                            {
                                                if (string.IsNullOrEmpty(requested))
                                                {
                                                    requested = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    requested = requested + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.IsGLSHKRegistered)
                                            {
                                                if (string.IsNullOrEmpty(registered))
                                                {
                                                    registered = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    registered = registered + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.GLSHKRegistrationRequested && !myTenantItem.IsGLSHKRegistered)
                                            {
                                                if (string.IsNullOrEmpty(pending))
                                                {
                                                    pending = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    pending = pending + ", " + myTenantItem.Card.Code;
                                                }
                                            }
                                        }

                                        else
                                        {
                                            if (myTenantItem.ChampRegistrationRequested)
                                            {
                                                if (string.IsNullOrEmpty(requested))
                                                {
                                                    requested = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    requested = requested + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.IsChampRegistered)
                                            {
                                                if (string.IsNullOrEmpty(registered))
                                                {
                                                    registered = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    registered = registered + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.ChampRegistrationRequested && !myTenantItem.IsChampRegistered)
                                            {
                                                if (string.IsNullOrEmpty(pending))
                                                {
                                                    pending = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    pending = pending + ", " + myTenantItem.Card.Code;
                                                }
                                            }
                                        }
                                    }
                                }

                                entity.RequestedAirlines = requested;
                                entity.RegisteredAirlines = registered;
                                entity.PendingAirlines = pending;
                            }
                        }
                    }
                }

                CustomerPM securedPm = new CustomerPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Customer", tenant);

                if (securedPm != null && entity != null)
                {
                    Customer entityPOC = (from s in repository.context.Customers where s.Id == securedPm.Id select s).FirstOrDefault();

                    securedPm.Field1 = new CustomFieldClass("Field1", "Customer", entityPOC.Field1);
                    securedPm.Field2 = new CustomFieldClass("Field2", "Customer", entityPOC.Field2);
                    securedPm.Field3 = new CustomFieldClass("Field3", "Customer", entityPOC.Field3);
                    securedPm.Field4 = new CustomFieldClass("Field4", "Customer", entityPOC.Field4);
                    securedPm.Field5 = new CustomFieldClass("Field5", "Customer", entityPOC.Field5);
                    securedPm.Field6 = new CustomFieldClass("Field6", "Customer", entityPOC.Field6);
                    securedPm.Field7 = new CustomFieldClass("Field7", "Customer", entityPOC.Field7);
                    securedPm.Field8 = new CustomFieldClass("Field8", "Customer", entityPOC.Field8);
                    securedPm.Field9 = new CustomFieldClass("Field9", "Customer", entityPOC.Field9);
                    securedPm.Field10 = new CustomFieldClass("Field10", "Customer", entityPOC.Field10);
                }
                if (securedPm != null)
                {
                    CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
                    securedPm.IsCustomerAllowed = myFilter.IsCustomerAllowed(securedPm);
                }
                return securedPm;
            }

            else
            {
                return null;
            }
        }


        public CustomerPM GetSinglePMForHybrid(string id, int tenant)
        {
            bool getFromCache = false;
            string entityName = "HybridCustomerPM" + id + tenant;
            CustomerPM entity;

            if (HttpContext.Current != null && getFromCache)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("Card.SharedLogisticsInvitationStatus").Include("LeadSource").Include("CustomerStatus")
                              where a.Tenant == tenant && a.Id == id
                              select new CustomerPM()
                              {
                                  BillToId = a.BillToId,
                                  BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                  Id = a.Id,
                                  RankId = a.RankId,
                                  AccountManagerUserId = a.AccountManagerUserId,
                                  SalesmanUserId = a.SalesmanUserId,
                                  Tenant = a.Tenant,
                                  Website = a.Card.Website,
                                  Code = a.Card.Code,
                                  LocalName = a.Card.LocalName,
                                  EnglishName = a.Card.EnglishName,
                                  CardPMId = a.Id,
                                  ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                  PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                                  IsActiveForMobile = a.Card.IsActiveForMobile,
                                  LastLoginDate = a.Card.LastLoginDate,
                                  InvitationDate = a.Card.InvitationDate,
                                  LeadSourceId = a.LeadSourceId,
                                  IndustryId = a.IndustryId,
                                  ClassifierId = a.ClassifierId,
                                  CollectorId = a.CollectorId,
                                  LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                                  IndustryName = a.Industry != null ? a.Industry.Name : null,
                                  CreditLimit = a.CreditLimit,
                                  LeadDescription = a.LeadDescription,
                                  IsCustomer = a.IsCustomer,
                                  FreelancerId = a.FreelancerId,
                                  CustomerStatusCode = a.CustomerStatusCode,
                                  ForwarderId = a.ForwarderId,
                                  CustomsAgentId = a.CustomsAgentId,
                                  MediatorId = a.MediatorId,
                                  BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                                  CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
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
                                  IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                  CreditLimitAmount = a.CreditLimitAmount,
                                  CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                                  CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                                  ExternalId2 = a.Card.ExternalId2,
                                  SATForeignRFC = a.Card.SATForeignRFC,
                                  MetodoPagoCode = a.Card.MetodoPagoCode,
                                  UsoCFDICode = a.Card.UsoCFDICode,
                                  CompetitorFields = a.CompetitorFields,
                                  CreatedByPartner = a.Card.CreatedByPartner,
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
                                  },

                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        this.SetCustomerAddressData(entity);

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }

                else
                {
                    entity = (CustomerPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            else
            {
                entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("Card.SharedLogisticsInvitationStatus").Include("LeadSource").Include("CustomerStatus")
                          where a.Tenant == tenant && a.Id == id
                          select new CustomerPM()
                          {
                              BillToId = a.BillToId,
                              BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                              Id = a.Id,
                              RankId = a.RankId,
                              AccountManagerUserId = a.AccountManagerUserId,
                              SalesmanUserId = a.SalesmanUserId,
                              Tenant = a.Tenant,
                              Website = a.Card.Website,
                              Code = a.Card.Code,
                              LocalName = a.Card.LocalName,
                              EnglishName = a.Card.EnglishName,
                              CardPMId = a.Id,
                              ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                              PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                              IsActiveForMobile = a.Card.IsActiveForMobile,
                              LastLoginDate = a.Card.LastLoginDate,
                              InvitationDate = a.Card.InvitationDate,
                              LeadSourceId = a.LeadSourceId,
                              IndustryId = a.IndustryId,
                              ClassifierId = a.ClassifierId,
                              CollectorId = a.CollectorId,
                              LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                              IndustryName = a.Industry != null ? a.Industry.Name : null,
                              CreditLimit = a.CreditLimit,
                              LeadDescription = a.LeadDescription,
                              IsCustomer = a.IsCustomer,
                              FreelancerId = a.FreelancerId,
                              CustomerStatusCode = a.CustomerStatusCode,
                              ForwarderId = a.ForwarderId,
                              CustomsAgentId = a.CustomsAgentId,
                              MediatorId = a.MediatorId,
                              BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                              CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
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
                              IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                              CreditLimitAmount = a.CreditLimitAmount,
                              CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                              CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                              ExternalId2 = a.Card.ExternalId2,
                              SATForeignRFC = a.Card.SATForeignRFC,
                              MetodoPagoCode = a.Card.MetodoPagoCode,
                              UsoCFDICode = a.Card.UsoCFDICode,
                              CompetitorFields = a.CompetitorFields,
                              CreatedByPartner = a.Card.CreatedByPartner,
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
                              },

                          }).FirstOrDefault();

                if (entity != null)
                {
                    this.SetCustomerAddressData(entity);
                }
            }

            if (entity != null)
            {
                CustomerProductRepository customerProductRepository = new CustomerProductRepository(repository.context);
                CustomerCompetitorRepository customerCompetitorRepository = new CustomerCompetitorRepository(repository.context);
                CustomerAdditionalServiceRepository customerAdditionalServiceRepository = new CustomerAdditionalServiceRepository(repository.context);
                CustomerSalesmanByProductRepository customerSalesmanByProductRepository = new CustomerSalesmanByProductRepository(repository.context);
                CustomerAccountManagerByProductRepository customerAccountManagerByProductRepository = new CustomerAccountManagerByProductRepository(repository.context);
                CustomerForwarderByProductRepository customerForwarderByProductRepository = new CustomerForwarderByProductRepository(repository.context);
                CustomerCustomsAgentByProductRepository customerCustomsAgentByProductRepository = new CustomerCustomsAgentByProductRepository(repository.context);
                CustomerMediatorByProductRepository customerMediatorByProductRepository = new CustomerMediatorByProductRepository(repository.context);
                CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);

                CustomerProductQuery customerProductQuery = new CustomerProductQuery(customerProductRepository);
                CustomerCompetitorQuery customerCompetitorQuery = new CustomerCompetitorQuery(customerCompetitorRepository);
                CustomerAdditionalServiceQuery customerAdditionalServiceQuery = new CustomerAdditionalServiceQuery(customerAdditionalServiceRepository);
                CustomerSalesmanByProductQuery customerSalesmanByProductQuery = new CustomerSalesmanByProductQuery(customerSalesmanByProductRepository);
                CustomerAccountManagerByProductQuery customerAccountManagerByProductQuery = new CustomerAccountManagerByProductQuery(customerAccountManagerByProductRepository);
                CustomerForwarderByProductQuery customerForwarderByProductQuery = new CustomerForwarderByProductQuery(customerForwarderByProductRepository);
                CustomerCustomsAgentByProductQuery customerCustomsAgentByProductQuery = new CustomerCustomsAgentByProductQuery(customerCustomsAgentByProductRepository);
                CustomerMediatorByProductQuery customerMediatorByProductQuery = new CustomerMediatorByProductQuery(customerMediatorByProductRepository);
                CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);

                entity.CustomerProducts = customerProductQuery.GetCustomerProductPMsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerCompetitors = customerCompetitorQuery.GetCustomerCompetitorsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerAdditionalServices = customerAdditionalServiceQuery.GetCustomerAdditionalServicesByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerSalesmanByProducts = customerSalesmanByProductQuery.GetCustomerSalesmanByProductPMs(entity.Tenant, entity.Id);

                entity.CustomerAccountManagerByProducts = customerAccountManagerByProductQuery.GetCustomerAccountManagerByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerForwarderByProducts = customerForwarderByProductQuery.GetCustomerForwarderByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerCustomsAgentByProducts = customerCustomsAgentByProductQuery.GetCustomerCustomsAgentByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerMediatorByProducts = customerMediatorByProductQuery.GetCustomerMediatorByProductPMs(entity.Tenant, entity.Id);
                entity.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(entity.Id, entity.Tenant);

                CustomerSalesNoteRepository salesNoteRepository = new CustomerSalesNoteRepository(repository.context);
                CustomerSalesNoteQuery salesNoteQuery = new CustomerSalesNoteQuery(salesNoteRepository);
                entity.SalesNotes = salesNoteQuery.GetSalesNotesByCustomerId(entity.Id, entity.Tenant).ToList();

                entity.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        entity.IsExternal = true;
                    }
                }

                if (!string.IsNullOrEmpty(entity.ReceivablesAccountingCard))
                {
                    int accountingCard = 0;
                    bool isParsed = Int32.TryParse(entity.ReceivablesAccountingCard, out accountingCard);

                    if (isParsed)
                    {
                        TenantManagement tenantManagement = null;

                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                            tenantManagement = tenantManagementRepository.GetSingleTenantManagement(accountingCard);

                            scope.Complete();
                        }

                        if (tenantManagement != null)
                        {
                            AirlineRepository airlineRepository = new AirlineRepository(repository.context);
                            List<Airline> tenantZeroAirlines = airlineRepository.GetAirlines(0).ToList();

                            if (tenantZeroAirlines.Count > 0)
                            {
                                if (tenantManagement.AWBMessagesCCSTypeCode == "GLSHK")
                                {
                                    tenantZeroAirlines = tenantZeroAirlines.Where(d => d.GLSHKPIMA != null).ToList();
                                }

                                else
                                {
                                    tenantZeroAirlines = tenantZeroAirlines.Where(d => d.TTY != null).ToList();
                                }

                                string requested = "";
                                string registered = "";
                                string pending = "";

                                List<Airline> currenctTenantAilines = airlineRepository.GetAirlines(tenantManagement.Id).ToList();
                                foreach (Airline tenantZeroItem in tenantZeroAirlines)
                                {
                                    Airline myTenantItem = currenctTenantAilines.Where(d => d.Card.Code == tenantZeroItem.Card.Code).FirstOrDefault();

                                    if (myTenantItem != null)
                                    {
                                        if (tenantManagement.AWBMessagesCCSTypeCode == "GLSHK")
                                        {
                                            if (myTenantItem.GLSHKRegistrationRequested)
                                            {
                                                if (string.IsNullOrEmpty(requested))
                                                {
                                                    requested = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    requested = requested + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.IsGLSHKRegistered)
                                            {
                                                if (string.IsNullOrEmpty(registered))
                                                {
                                                    registered = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    registered = registered + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.GLSHKRegistrationRequested && !myTenantItem.IsGLSHKRegistered)
                                            {
                                                if (string.IsNullOrEmpty(pending))
                                                {
                                                    pending = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    pending = pending + ", " + myTenantItem.Card.Code;
                                                }
                                            }
                                        }

                                        else
                                        {
                                            if (myTenantItem.ChampRegistrationRequested)
                                            {
                                                if (string.IsNullOrEmpty(requested))
                                                {
                                                    requested = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    requested = requested + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.IsChampRegistered)
                                            {
                                                if (string.IsNullOrEmpty(registered))
                                                {
                                                    registered = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    registered = registered + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.ChampRegistrationRequested && !myTenantItem.IsChampRegistered)
                                            {
                                                if (string.IsNullOrEmpty(pending))
                                                {
                                                    pending = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    pending = pending + ", " + myTenantItem.Card.Code;
                                                }
                                            }
                                        }
                                    }
                                }

                                entity.RequestedAirlines = requested;
                                entity.RegisteredAirlines = registered;
                                entity.PendingAirlines = pending;
                            }
                        }
                    }
                }

                CustomerPM securedPm = new CustomerPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Customer", tenant);

                if (securedPm != null && entity != null)
                {
                    Customer entityPOC = (from s in repository.context.Customers where s.Id == securedPm.Id select s).FirstOrDefault();

                    securedPm.Field1 = new CustomFieldClass("Field1", "Customer", entityPOC.Field1);
                    securedPm.Field2 = new CustomFieldClass("Field2", "Customer", entityPOC.Field2);
                    securedPm.Field3 = new CustomFieldClass("Field3", "Customer", entityPOC.Field3);
                    securedPm.Field4 = new CustomFieldClass("Field4", "Customer", entityPOC.Field4);
                    securedPm.Field5 = new CustomFieldClass("Field5", "Customer", entityPOC.Field5);
                    securedPm.Field6 = new CustomFieldClass("Field6", "Customer", entityPOC.Field6);
                    securedPm.Field7 = new CustomFieldClass("Field7", "Customer", entityPOC.Field7);
                    securedPm.Field8 = new CustomFieldClass("Field8", "Customer", entityPOC.Field8);
                    securedPm.Field9 = new CustomFieldClass("Field9", "Customer", entityPOC.Field9);
                    securedPm.Field10 = new CustomFieldClass("Field10", "Customer", entityPOC.Field10);
                }
                if (securedPm != null)
                {
                    CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
                    securedPm.IsCustomerAllowed = myFilter.IsCustomerAllowed(securedPm);
                }
                return securedPm;
            }

            else
            {
                return null;
            }
        }

        public CustomerPM GetSinglePMByCodeForHybrid(string code, int tenant, bool getFromCache)
        {
            string entityName = "HybridCustomerPM" + code + tenant;
            CustomerPM entity;
            if (getFromCache)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("Card.SharedLogisticsInvitationStatus").Include("LeadSource").Include("CustomerStatus")
                                  where a.Tenant == tenant && a.Card.Code == code
                                  select new CustomerPM()
                                  {
                                      BillToId = a.BillToId,
                                      BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                      Id = a.Id,
                                      RankId = a.RankId,
                                      AccountManagerUserId = a.AccountManagerUserId,
                                      SalesmanUserId = a.SalesmanUserId,
                                      Tenant = a.Tenant,
                                      Website = a.Card.Website,
                                      Code = a.Card.Code,
                                      LocalName = a.Card.LocalName,
                                      EnglishName = a.Card.EnglishName,
                                      CardPMId = a.Id,
                                      ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                      PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                                      IsActiveForMobile = a.Card.IsActiveForMobile,
                                      LastLoginDate = a.Card.LastLoginDate,
                                      InvitationDate = a.Card.InvitationDate,
                                      LeadSourceId = a.LeadSourceId,
                                      IndustryId = a.IndustryId,
                                      ClassifierId = a.ClassifierId,
                                      CollectorId = a.CollectorId,
                                      LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                                      IndustryName = a.Industry != null ? a.Industry.Name : null,
                                      CreditLimit = a.CreditLimit,
                                      LeadDescription = a.LeadDescription,
                                      IsCustomer = a.IsCustomer,
                                      FreelancerId = a.FreelancerId,
                                      CustomerStatusCode = a.CustomerStatusCode,
                                      ForwarderId = a.ForwarderId,
                                      CustomsAgentId = a.CustomsAgentId,
                                      MediatorId = a.MediatorId,
                                      BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                                      CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
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
                                      IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                      CreditLimitAmount = a.CreditLimitAmount,
                                      CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                                      CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                                      ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                      PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                      BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                      BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                                      ExternalId2 = a.Card.ExternalId2,
                                      SATForeignRFC = a.Card.SATForeignRFC,
                                      MetodoPagoCode = a.Card.MetodoPagoCode,
                                      UsoCFDICode = a.Card.UsoCFDICode,
                                      CompetitorFields = a.CompetitorFields,
                                      CreatedByPartner = a.Card.CreatedByPartner,
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
                                      },
                                  }).FirstOrDefault();

                        if (entity != null)
                        {
                            this.SetCustomerAddressData(entity);

                            if (CacheManager.CacheWrapper.Get(entityName) == null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                    else
                    {
                        entity = (CustomerPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("Card.SharedLogisticsInvitationStatus").Include("LeadSource").Include("CustomerStatus")
                              where a.Card.Code == code && a.Tenant == tenant
                              select new CustomerPM()
                              {
                                  BillToId = a.BillToId,
                                  BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                  Id = a.Id,
                                  RankId = a.RankId,
                                  AccountManagerUserId = a.AccountManagerUserId,
                                  SalesmanUserId = a.SalesmanUserId,
                                  Tenant = a.Tenant,
                                  Website = a.Card.Website,
                                  Code = a.Card.Code,
                                  LocalName = a.Card.LocalName,
                                  EnglishName = a.Card.EnglishName,
                                  CardPMId = a.Id,
                                  ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                  PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                                  IsActiveForMobile = a.Card.IsActiveForMobile,
                                  LastLoginDate = a.Card.LastLoginDate,
                                  InvitationDate = a.Card.InvitationDate,
                                  LeadSourceId = a.LeadSourceId,
                                  IndustryId = a.IndustryId,
                                  ClassifierId = a.ClassifierId,
                                  CollectorId = a.CollectorId,
                                  LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                                  IndustryName = a.Industry != null ? a.Industry.Name : null,
                                  CreditLimit = a.CreditLimit,
                                  LeadDescription = a.LeadDescription,
                                  IsCustomer = a.IsCustomer,
                                  FreelancerId = a.FreelancerId,
                                  CustomerStatusCode = a.CustomerStatusCode,
                                  ForwarderId = a.ForwarderId,
                                  CustomsAgentId = a.CustomsAgentId,
                                  MediatorId = a.MediatorId,
                                  BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                                  CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
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
                                  IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                  CreditLimitAmount = a.CreditLimitAmount,
                                  CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                                  CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                                  ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                  PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                  BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                  BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                                  ExternalId2 = a.Card.ExternalId2,
                                  SATForeignRFC = a.Card.SATForeignRFC,
                                  MetodoPagoCode = a.Card.MetodoPagoCode,
                                  UsoCFDICode = a.Card.UsoCFDICode,
                                  CompetitorFields = a.CompetitorFields,
                                  CreatedByPartner = a.Card.CreatedByPartner,
                                  Card = new CardPM
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
                                  },

                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        this.SetCustomerAddressData(entity);
                    }
                }
            }
            else
            {
                entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("Card.SharedLogisticsInvitationStatus").Include("LeadSource").Include("CustomerStatus")
                          where a.Card.Code == code && a.Tenant == tenant
                          select new CustomerPM()
                          {
                              BillToId = a.BillToId,
                              BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                              Id = a.Id,
                              RankId = a.RankId,
                              AccountManagerUserId = a.AccountManagerUserId,
                              SalesmanUserId = a.SalesmanUserId,
                              Tenant = a.Tenant,
                              Website = a.Card.Website,
                              Code = a.Card.Code,
                              LocalName = a.Card.LocalName,
                              EnglishName = a.Card.EnglishName,
                              CardPMId = a.Id,
                              ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                              PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                              IsActiveForMobile = a.Card.IsActiveForMobile,
                              LastLoginDate = a.Card.LastLoginDate,
                              InvitationDate = a.Card.InvitationDate,
                              LeadSourceId = a.LeadSourceId,
                              IndustryId = a.IndustryId,
                              ClassifierId = a.ClassifierId,
                              CollectorId = a.CollectorId,
                              LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                              IndustryName = a.Industry != null ? a.Industry.Name : null,
                              CreditLimit = a.CreditLimit,
                              LeadDescription = a.LeadDescription,
                              IsCustomer = a.IsCustomer,
                              FreelancerId = a.FreelancerId,
                              CustomerStatusCode = a.CustomerStatusCode,
                              ForwarderId = a.ForwarderId,
                              CustomsAgentId = a.CustomsAgentId,
                              MediatorId = a.MediatorId,
                              BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                              CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
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
                              IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                              CreditLimitAmount = a.CreditLimitAmount,
                              CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                              CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                              ExternalId2 = a.Card.ExternalId2,
                              SATForeignRFC = a.Card.SATForeignRFC,
                              MetodoPagoCode = a.Card.MetodoPagoCode,
                              UsoCFDICode = a.Card.UsoCFDICode,
                              CreatedByPartner = a.Card.CreatedByPartner,
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
                              },

                          }).FirstOrDefault();

                if (entity != null)
                {
                    this.SetCustomerAddressData(entity);
                }
            }

            if (entity != null)
            {
                CustomerProductRepository customerProductRepository = new CustomerProductRepository(repository.context);
                CustomerCompetitorRepository customerCompetitorRepository = new CustomerCompetitorRepository(repository.context);
                CustomerAdditionalServiceRepository customerAdditionalServiceRepository = new CustomerAdditionalServiceRepository(repository.context);
                CustomerSalesmanByProductRepository customerSalesmanByProductRepository = new CustomerSalesmanByProductRepository(repository.context);
                CustomerAccountManagerByProductRepository customerAccountManagerByProductRepository = new CustomerAccountManagerByProductRepository(repository.context);
                CustomerForwarderByProductRepository customerForwarderByProductRepository = new CustomerForwarderByProductRepository(repository.context);
                CustomerCustomsAgentByProductRepository customerCustomsAgentByProductRepository = new CustomerCustomsAgentByProductRepository(repository.context);
                CustomerMediatorByProductRepository customerMediatorByProductRepository = new CustomerMediatorByProductRepository(repository.context);

                CustomerProductQuery customerProductQuery = new CustomerProductQuery(customerProductRepository);
                CustomerCompetitorQuery customerCompetitorQuery = new CustomerCompetitorQuery(customerCompetitorRepository);
                CustomerAdditionalServiceQuery customerAdditionalServiceQuery = new CustomerAdditionalServiceQuery(customerAdditionalServiceRepository);
                CustomerSalesmanByProductQuery customerSalesmanByProductQuery = new CustomerSalesmanByProductQuery(customerSalesmanByProductRepository);
                CustomerAccountManagerByProductQuery customerAccountManagerByProductQuery = new CustomerAccountManagerByProductQuery(customerAccountManagerByProductRepository);
                CustomerForwarderByProductQuery customerForwarderByProductQuery = new CustomerForwarderByProductQuery(customerForwarderByProductRepository);
                CustomerCustomsAgentByProductQuery customerCustomsAgentByProductQuery = new CustomerCustomsAgentByProductQuery(customerCustomsAgentByProductRepository);
                CustomerMediatorByProductQuery customerMediatorByProductQuery = new CustomerMediatorByProductQuery(customerMediatorByProductRepository);

                entity.CustomerProducts = customerProductQuery.GetCustomerProductPMsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerCompetitors = customerCompetitorQuery.GetCustomerCompetitorsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerAdditionalServices = customerAdditionalServiceQuery.GetCustomerAdditionalServicesByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerSalesmanByProducts = customerSalesmanByProductQuery.GetCustomerSalesmanByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerAccountManagerByProducts = customerAccountManagerByProductQuery.GetCustomerAccountManagerByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerForwarderByProducts = customerForwarderByProductQuery.GetCustomerForwarderByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerCustomsAgentByProducts = customerCustomsAgentByProductQuery.GetCustomerCustomsAgentByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerMediatorByProducts = customerMediatorByProductQuery.GetCustomerMediatorByProductPMs(entity.Tenant, entity.Id);

                CustomerSalesNoteRepository salesNoteRepository = new CustomerSalesNoteRepository(repository.context);
                CustomerSalesNoteQuery salesNoteQuery = new CustomerSalesNoteQuery(salesNoteRepository);
                entity.SalesNotes = salesNoteQuery.GetSalesNotesByCustomerId(entity.Id, entity.Tenant).ToList();
                if (entity != null)
                {
                    entity.IsExternal = false;

                    AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                    AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                    if (accountingSystem != null)
                    {
                        if (accountingSystem.IsExternalCodesFromTable)
                        {
                            entity.IsExternal = true;
                        }
                    }
                }

                CustomerPM securedPm = new CustomerPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Customer", tenant);

                if (securedPm != null && entity != null)
                {
                    Customer entityPOC = (from s in repository.context.Customers
                                          where s.Id == securedPm.Id
                                          select s).FirstOrDefault();

                    securedPm.Field1 = new CustomFieldClass("Field1", "Customer", entityPOC.Field1);
                    securedPm.Field2 = new CustomFieldClass("Field2", "Customer", entityPOC.Field2);
                    securedPm.Field3 = new CustomFieldClass("Field3", "Customer", entityPOC.Field3);
                    securedPm.Field4 = new CustomFieldClass("Field4", "Customer", entityPOC.Field4);
                    securedPm.Field5 = new CustomFieldClass("Field5", "Customer", entityPOC.Field5);
                    securedPm.Field6 = new CustomFieldClass("Field6", "Customer", entityPOC.Field6);
                    securedPm.Field7 = new CustomFieldClass("Field7", "Customer", entityPOC.Field7);
                    securedPm.Field8 = new CustomFieldClass("Field8", "Customer", entityPOC.Field8);
                    securedPm.Field9 = new CustomFieldClass("Field9", "Customer", entityPOC.Field9);
                    securedPm.Field10 = new CustomFieldClass("Field10", "Customer", entityPOC.Field10);
                }

                return securedPm;
            }

            return entity;
        }

        public CustomerPM GetSinglePMByVatNumberForHybrid(string vatNumber, int tenant, bool getFromCache)
        {
            string entityName = "HybridCustomerPM" + vatNumber + tenant;
            CustomerPM entity;
            if (getFromCache)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("Card.SharedLogisticsInvitationStatus").Include("LeadSource").Include("CustomerStatus")
                                  where a.Tenant == tenant && a.Card.VatNumber == vatNumber
                                  select new CustomerPM()
                                  {
                                      BillToId = a.BillToId,
                                      BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                      Id = a.Id,
                                      RankId = a.RankId,
                                      AccountManagerUserId = a.AccountManagerUserId,
                                      SalesmanUserId = a.SalesmanUserId,
                                      Tenant = a.Tenant,
                                      Website = a.Card.Website,
                                      Code = a.Card.Code,
                                      LocalName = a.Card.LocalName,
                                      EnglishName = a.Card.EnglishName,
                                      CardPMId = a.Id,
                                      ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                      PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                                      IsActiveForMobile = a.Card.IsActiveForMobile,
                                      LastLoginDate = a.Card.LastLoginDate,
                                      InvitationDate = a.Card.InvitationDate,
                                      LeadSourceId = a.LeadSourceId,
                                      IndustryId = a.IndustryId,
                                      ClassifierId = a.ClassifierId,
                                      CollectorId = a.CollectorId,
                                      LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                                      IndustryName = a.Industry != null ? a.Industry.Name : null,
                                      CreditLimit = a.CreditLimit,
                                      LeadDescription = a.LeadDescription,
                                      IsCustomer = a.IsCustomer,
                                      FreelancerId = a.FreelancerId,
                                      CustomerStatusCode = a.CustomerStatusCode,
                                      ForwarderId = a.ForwarderId,
                                      CustomsAgentId = a.CustomsAgentId,
                                      MediatorId = a.MediatorId,
                                      BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                                      CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
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
                                      IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                      CreditLimitAmount = a.CreditLimitAmount,
                                      CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                                      CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                                      ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                      PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                      BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                      BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                                      ExternalId2 = a.Card.ExternalId2,
                                      SATForeignRFC = a.Card.SATForeignRFC,
                                      MetodoPagoCode = a.Card.MetodoPagoCode,
                                      UsoCFDICode = a.Card.UsoCFDICode,
                                      CompetitorFields = a.CompetitorFields,
                                      CreatedByPartner = a.Card.CreatedByPartner,
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
                                      },
                                  }).FirstOrDefault();

                        if (entity != null)
                        {
                            this.SetCustomerAddressData(entity);

                            if (CacheManager.CacheWrapper.Get(entityName) == null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                    else
                    {
                        entity = (CustomerPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("Card.SharedLogisticsInvitationStatus").Include("LeadSource").Include("CustomerStatus")
                              where a.Card.VatNumber == vatNumber && a.Tenant == tenant
                              select new CustomerPM()
                              {
                                  BillToId = a.BillToId,
                                  BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                  Id = a.Id,
                                  RankId = a.RankId,
                                  AccountManagerUserId = a.AccountManagerUserId,
                                  SalesmanUserId = a.SalesmanUserId,
                                  Tenant = a.Tenant,
                                  Website = a.Card.Website,
                                  Code = a.Card.Code,
                                  LocalName = a.Card.LocalName,
                                  EnglishName = a.Card.EnglishName,
                                  CardPMId = a.Id,
                                  ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                  PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                                  IsActiveForMobile = a.Card.IsActiveForMobile,
                                  LastLoginDate = a.Card.LastLoginDate,
                                  InvitationDate = a.Card.InvitationDate,
                                  LeadSourceId = a.LeadSourceId,
                                  IndustryId = a.IndustryId,
                                  ClassifierId = a.ClassifierId,
                                  CollectorId = a.CollectorId,
                                  LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                                  IndustryName = a.Industry != null ? a.Industry.Name : null,
                                  CreditLimit = a.CreditLimit,
                                  LeadDescription = a.LeadDescription,
                                  IsCustomer = a.IsCustomer,
                                  FreelancerId = a.FreelancerId,
                                  CustomerStatusCode = a.CustomerStatusCode,
                                  ForwarderId = a.ForwarderId,
                                  CustomsAgentId = a.CustomsAgentId,
                                  MediatorId = a.MediatorId,
                                  BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                                  CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
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
                                  IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                  CreditLimitAmount = a.CreditLimitAmount,
                                  CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                                  CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                                  ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                  PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                  BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                  BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                                  ExternalId2 = a.Card.ExternalId2,
                                  SATForeignRFC = a.Card.SATForeignRFC,
                                  MetodoPagoCode = a.Card.MetodoPagoCode,
                                  UsoCFDICode = a.Card.UsoCFDICode,
                                  CompetitorFields = a.CompetitorFields,
                                  CreatedByPartner = a.Card.CreatedByPartner,
                                  Card = new CardPM
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
                                  },

                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        this.SetCustomerAddressData(entity);
                    }
                }
            }
            else
            {
                entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("Card.SharedLogisticsInvitationStatus").Include("LeadSource").Include("CustomerStatus")
                          where a.Card.VatNumber == vatNumber && a.Tenant == tenant
                          select new CustomerPM()
                          {
                              BillToId = a.BillToId,
                              BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                              Id = a.Id,
                              RankId = a.RankId,
                              AccountManagerUserId = a.AccountManagerUserId,
                              SalesmanUserId = a.SalesmanUserId,
                              Tenant = a.Tenant,
                              Website = a.Card.Website,
                              Code = a.Card.Code,
                              LocalName = a.Card.LocalName,
                              EnglishName = a.Card.EnglishName,
                              CardPMId = a.Id,
                              ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                              PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                              IsActiveForMobile = a.Card.IsActiveForMobile,
                              LastLoginDate = a.Card.LastLoginDate,
                              InvitationDate = a.Card.InvitationDate,
                              LeadSourceId = a.LeadSourceId,
                              IndustryId = a.IndustryId,
                              ClassifierId = a.ClassifierId,
                              CollectorId = a.CollectorId,
                              LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                              IndustryName = a.Industry != null ? a.Industry.Name : null,
                              CreditLimit = a.CreditLimit,
                              LeadDescription = a.LeadDescription,
                              IsCustomer = a.IsCustomer,
                              FreelancerId = a.FreelancerId,
                              CustomerStatusCode = a.CustomerStatusCode,
                              ForwarderId = a.ForwarderId,
                              CustomsAgentId = a.CustomsAgentId,
                              MediatorId = a.MediatorId,
                              BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                              CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
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
                              IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                              CreditLimitAmount = a.CreditLimitAmount,
                              CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                              CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                              ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                              PaymentMethodCode = a.Card.SATPaymentMethodCode,
                              BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                              BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                              ExternalId2 = a.Card.ExternalId2,
                              SATForeignRFC = a.Card.SATForeignRFC,
                              MetodoPagoCode = a.Card.MetodoPagoCode,
                              UsoCFDICode = a.Card.UsoCFDICode,
                              CompetitorFields = a.CompetitorFields,
                              CreatedByPartner = a.Card.CreatedByPartner,
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
                              },
                          }).FirstOrDefault();

                if (entity != null)
                {
                    this.SetCustomerAddressData(entity);
                }
            }

            if (entity != null)
            {
                CustomerProductRepository customerProductRepository = new CustomerProductRepository(repository.context);
                CustomerCompetitorRepository customerCompetitorRepository = new CustomerCompetitorRepository(repository.context);
                CustomerAdditionalServiceRepository customerAdditionalServiceRepository = new CustomerAdditionalServiceRepository(repository.context);
                CustomerSalesmanByProductRepository customerSalesmanByProductRepository = new CustomerSalesmanByProductRepository(repository.context);
                CustomerAccountManagerByProductRepository customerAccountManagerByProductRepository = new CustomerAccountManagerByProductRepository(repository.context);
                CustomerForwarderByProductRepository customerForwarderByProductRepository = new CustomerForwarderByProductRepository(repository.context);
                CustomerCustomsAgentByProductRepository customerCustomsAgentByProductRepository = new CustomerCustomsAgentByProductRepository(repository.context);
                CustomerMediatorByProductRepository customerMediatorByProductRepository = new CustomerMediatorByProductRepository(repository.context);

                CustomerProductQuery customerProductQuery = new CustomerProductQuery(customerProductRepository);
                CustomerCompetitorQuery customerCompetitorQuery = new CustomerCompetitorQuery(customerCompetitorRepository);
                CustomerAdditionalServiceQuery customerAdditionalServiceQuery = new CustomerAdditionalServiceQuery(customerAdditionalServiceRepository);
                CustomerSalesmanByProductQuery customerSalesmanByProductQuery = new CustomerSalesmanByProductQuery(customerSalesmanByProductRepository);
                CustomerAccountManagerByProductQuery customerAccountManagerByProductQuery = new CustomerAccountManagerByProductQuery(customerAccountManagerByProductRepository);
                CustomerForwarderByProductQuery customerForwarderByProductQuery = new CustomerForwarderByProductQuery(customerForwarderByProductRepository);
                CustomerCustomsAgentByProductQuery customerCustomsAgentByProductQuery = new CustomerCustomsAgentByProductQuery(customerCustomsAgentByProductRepository);
                CustomerMediatorByProductQuery customerMediatorByProductQuery = new CustomerMediatorByProductQuery(customerMediatorByProductRepository);

                entity.CustomerProducts = customerProductQuery.GetCustomerProductPMsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerCompetitors = customerCompetitorQuery.GetCustomerCompetitorsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerAdditionalServices = customerAdditionalServiceQuery.GetCustomerAdditionalServicesByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerSalesmanByProducts = customerSalesmanByProductQuery.GetCustomerSalesmanByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerAccountManagerByProducts = customerAccountManagerByProductQuery.GetCustomerAccountManagerByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerForwarderByProducts = customerForwarderByProductQuery.GetCustomerForwarderByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerCustomsAgentByProducts = customerCustomsAgentByProductQuery.GetCustomerCustomsAgentByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerMediatorByProducts = customerMediatorByProductQuery.GetCustomerMediatorByProductPMs(entity.Tenant, entity.Id);

                CustomerSalesNoteRepository salesNoteRepository = new CustomerSalesNoteRepository(repository.context);
                CustomerSalesNoteQuery salesNoteQuery = new CustomerSalesNoteQuery(salesNoteRepository);
                entity.SalesNotes = salesNoteQuery.GetSalesNotesByCustomerId(entity.Id, entity.Tenant).ToList();

                if (entity != null)
                {
                    entity.IsExternal = false;

                    AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                    AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                    if (accountingSystem != null)
                    {
                        if (accountingSystem.IsExternalCodesFromTable)
                        {
                            entity.IsExternal = true;
                        }
                    }

                }

                CustomerPM securedPm = new CustomerPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Customer", tenant);

                if (securedPm != null && entity != null)
                {
                    Customer entityPOC = (from s in repository.context.Customers
                                          where s.Id == securedPm.Id
                                          select s).FirstOrDefault();

                    securedPm.Field1 = new CustomFieldClass("Field1", "Customer", entityPOC.Field1);
                    securedPm.Field2 = new CustomFieldClass("Field2", "Customer", entityPOC.Field2);
                    securedPm.Field3 = new CustomFieldClass("Field3", "Customer", entityPOC.Field3);
                    securedPm.Field4 = new CustomFieldClass("Field4", "Customer", entityPOC.Field4);
                    securedPm.Field5 = new CustomFieldClass("Field5", "Customer", entityPOC.Field5);
                    securedPm.Field6 = new CustomFieldClass("Field6", "Customer", entityPOC.Field6);
                    securedPm.Field7 = new CustomFieldClass("Field7", "Customer", entityPOC.Field7);
                    securedPm.Field8 = new CustomFieldClass("Field8", "Customer", entityPOC.Field8);
                    securedPm.Field9 = new CustomFieldClass("Field9", "Customer", entityPOC.Field9);
                    securedPm.Field10 = new CustomFieldClass("Field10", "Customer", entityPOC.Field10);
                }

                return securedPm;
            }
            else
            {
                return null;
            }
        }

        public IQueryable<CustomerPM> GetCustomerPMsByTenant(int tenant)
        {
            IQueryable<CustomerPM> customers = from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Card.SharedLogisticsInvitationStatus").Include("Rank").Include("Collector.Contact").Include("Classifier.Contact").Include("Freelancer.Contact").Include("Forwarder").Include("CustomsAgent").Include("Mediator").Include("LeadSource")
                                               where a.Tenant == tenant
                                               select new CustomerPM()
                                               {
                                                   BillToId = a.BillToId,
                                                   BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                                   Id = a.Id,
                                                   RankId = a.RankId,
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
                                                   ImageDetailId = a.Card.ImageDetailId,
                                                   BankName = a.Card.BankName,
                                                   BankAddress = a.Card.BankAddress,
                                                   IBANNumber = a.Card.IBANNumber,
                                                   Swift = a.Card.Swift,
                                                   AccountNumber = a.Card.AccountNumber,
                                                   SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                                                   IsActiveForMobile = a.Card.IsActiveForMobile,
                                                   LastLoginDate = a.Card.LastLoginDate,
                                                   InvitationDate = a.Card.InvitationDate,
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
                                                   CustomerStatusName = a.CustomerStatus != null ? a.CustomerStatus.Name : null,
                                                   ForwarderId = a.ForwarderId,
                                                   ForwarderName = a.Forwarder != null ? a.Forwarder.EnglishName : null,
                                                   CustomsAgentId = a.CustomsAgentId,
                                                   CustomsAgentName = a.CustomsAgent != null ? a.CustomsAgent.EnglishName : null,
                                                   MediatorId = a.MediatorId,
                                                   MediatorName = a.Mediator != null ? a.Mediator.EnglishName : null,
                                                   BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
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
                                                   IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                                   CreditLimitAmount = a.CreditLimitAmount,
                                                   CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                                                   CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                                                   ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                                   PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                                   BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                                   BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                                                   ExternalId2 = a.Card.ExternalId2,
                                                   SATForeignRFC = a.Card.SATForeignRFC,
                                                   MetodoPagoCode = a.Card.MetodoPagoCode,
                                                   UsoCFDICode = a.Card.UsoCFDICode,
                                                   CreatedByPartner = a.Card.CreatedByPartner,
                                                   Card = new CardPM()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       EnglishName = a.Card.EnglishName,
                                                       PrimaryContactId = a.Card.PrimaryContactId,
                                                   },
                                               };

            return customers;
        }

        public IQueryable<CustomerPM> GetCustomersByNameOrCode(string code, string name, int tenant)
        {
            string codeNew = "";
            codeNew = code;

            string nameNew = "";
            nameNew = name;

            var query = from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Card.SharedLogisticsInvitationStatus").Include("Rank").Include("Collector.Contact").Include("Classifier.Contact").Include("Freelancer.Contact").Include("Forwarder").Include("CustomsAgent").Include("Mediator")
                        where a.Tenant == tenant
                        select new CustomerPM()
                        {
                            BillToId = a.BillToId,
                            BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                            Id = a.Id,
                            RankId = a.RankId,
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
                            ImageDetailId = a.Card.ImageDetailId,
                            VatTypeId = a.Card.VatTypeId,
                            BankName = a.Card.BankName,
                            BankAddress = a.Card.BankAddress,
                            IBANNumber = a.Card.IBANNumber,
                            Swift = a.Card.Swift,
                            AccountNumber = a.Card.AccountNumber,
                            SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                            IsActiveForMobile = a.Card.IsActiveForMobile,
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
                            CustomerStatusName = a.CustomerStatus != null ? a.CustomerStatus.Name : null,
                            ForwarderId = a.ForwarderId,
                            ForwarderName = a.Forwarder != null ? a.Forwarder.EnglishName : null,
                            CustomsAgentId = a.CustomsAgentId,
                            CustomsAgentName = a.CustomsAgent != null ? a.CustomsAgent.EnglishName : null,
                            MediatorId = a.MediatorId,
                            MediatorName = a.Mediator != null ? a.Mediator.EnglishName : null,
                            BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
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
                            LastLoginDate = a.Card.LastLoginDate,
                            IsPrivateLabelCustomer = a.IsPrivateLabelCustomer,
                            IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                            CreditLimitAmount = a.CreditLimitAmount,
                            CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                            CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                            ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                            PaymentMethodCode = a.Card.SATPaymentMethodCode,
                            BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                            BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                            ExternalId2 = a.Card.ExternalId2,
                            SATForeignRFC = a.Card.SATForeignRFC,
                            MetodoPagoCode = a.Card.MetodoPagoCode,
                            UsoCFDICode = a.Card.UsoCFDICode,
                            CreatedByPartner = a.Card.CreatedByPartner,
                            Card = new CardPM()
                            {
                                Id = a.Id,
                                Tenant = a.Tenant,
                                EnglishName = a.Card.EnglishName,
                                PrimaryContactId = a.Card.PrimaryContactId,
                            },
                        };

            IQueryable<CustomerPM> query2 = null;
            if (!string.IsNullOrEmpty(code))
            {
                query2 = query.Where(d => d.Code.ToUpper().StartsWith(code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (query2 != null)
                {
                    if (query2.Count() == 0)
                    {
                        query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                    }
                }
                else
                {
                    query2 = query.Where(d => d.EnglishName.ToUpper().StartsWith(name.ToUpper()));
                }
            }
            if (query2 != null)
            {
                return query2;
            }
            else
                return query;
        }

        private void SetCustomerAddressData(CustomerPM entity)
        {
            AddressRepository addressRep = new AddressRepository(repository.context);
            AddressQuery addressQuery = new AddressQuery(addressRep);
            AddressPM entityAddress = addressQuery.GetAddressPMByTypeAndCard(entity.Id, "M", entity.Tenant);

            if (entityAddress != null)
            {
                entity.PhoneNumber = entityAddress.PhoneNumber;
                entity.FaxNumber = entityAddress.FaxNumber;
                entity.GoogleAddressString = entityAddress.Address1 + " " + entityAddress.City + " " + (entityAddress.StateEnglishName != null ? (entityAddress.StateEnglishName + " ") : "") + entityAddress.CountryEnglishName;
                entity.CountryId = entityAddress.CountryId;
                entity.Address1_Potential = entityAddress.Address1;
                entity.Address2_Potential = entityAddress.Address2;
                entity.ZipCode_Potential = entityAddress.ZipCode;
                entity.City_Potential = entityAddress.City;
                entity.CountryId_Potential = entityAddress.CountryId;
                entity.StateId_Potential = entityAddress.StateId;
                entity.PhoneNumber_Potential = entityAddress.PhoneNumber;
                entity.FaxNumber_Potential = entityAddress.FaxNumber;
                entity.ATTN_Potential = entityAddress.ATTN;

                Country country = CountryRepository.GetSingleCountry(entityAddress.CountryId, entity.Tenant, true);
                if (country != null)
                {
                    entity.CountryCode = country.Code;
                    entity.CountryName = country.EnglishName;

                    if (!string.IsNullOrEmpty(country.Code) && !string.IsNullOrEmpty(entity.CityName))
                    {
                        entity.CityWithCountry = entity.CityName + "(" + country.Code + ")";
                    }
                }
                else
                {
                    entity.CityWithCountry = entity.CityName;
                }
            }
        }

        public IQueryable<CustomerList> GetIQueryableEntityList(IQueryable<CustomersDataView> iQueryable)
        {
            IQueryable<CustomerList> result = from customer in iQueryable
                                              select new CustomerList()
                                              {
                                                  Code = customer.Code,
                                                  EnglishName = customer.EnglishName,
                                                  LocalName = customer.LocalName,
                                                  ReceivablesAccountingCard = customer.ReceivablesAccountingCard,
                                                  PayablesAccountingCard = customer.PayablesAccountingCard,
                                                  InActive = customer.InActive,
                                                  Notes = customer.Notes,
                                                  SupportNotes = customer.SupportNotes,
                                                  BillToId = customer.BillToId,
                                                  Website = customer.Website,
                                                  SalesmanUserId = customer.SalesmanUserId,
                                                  SalesmanBusinessUnitId = customer.SalesmanBusinessUnitId,
                                                  Id = customer.Id,
                                                  PaymentTermId = customer.PaymentTermId,
                                                  CreateDate = customer.CreateDate,
                                                  UpdateDate = customer.UpdateDate,
                                                  CreatedByUserId = customer.CreatedByUserId,
                                                  UpdatedByUserId = customer.UpdatedByUserId,
                                                  Tenant = customer.Tenant,
                                                  VatNumber = customer.VatNumber,
                                                  SearchFields = customer.SearchFields,
                                                  PaymentTermEnglishName = customer.PaymentTermEnglishName,
                                                  InvoiceCurrencyId = customer.InvoiceCurrencyId,
                                                  LastShipmentDate = customer.LastShipmentDate,
                                                  StartWorkingDate = customer.StartWorkingDate,
                                                  StartWorkingManuallySet = customer.StartWorkingManuallySet,
                                                  AccountManagerUserEnglishName = customer.AccountManagerUserEnglishName,
                                                  SalesmanUserEnglishName = customer.SalesmanUserEnglishName,
                                                  CollectorName = customer.CollectorName,
                                                  ClassifierName = customer.ClassifierName,
                                                  VatTypeId = customer.VatTypeId,
                                                  BillToName = customer.BillToName,
                                                  Field1 = customer.Field1,
                                                  Field2 = customer.Field2,
                                                  Field3 = customer.Field3,
                                                  Field4 = customer.Field4,
                                                  Field5 = customer.Field5,
                                                  Field6 = customer.Field6,
                                                  Field7 = customer.Field7,
                                                  Field8 = customer.Field8,
                                                  Field9 = customer.Field9,
                                                  Field10 = customer.Field10,
                                                  RankCode = customer.RankCode,
                                                  RankName = customer.RankName,
                                                  SharedLogisticsInvitationStatusName = customer.SharedLogisticsInvitationStatusName,
                                                  SharedLogisticsInvitationStatusCode = customer.SharedLogisticsInvitationStatusCode,
                                                  IsActiveForMobile = customer.IsActiveForMobile,
                                                  LastLoginDate = customer.LastLoginDate,
                                                  InvitationDate = customer.InvitationDate,
                                                  IndustryName = customer.IndustryName,
                                                  LeadDescription = customer.LeadDescription,
                                                  ClassifierId = customer.ClassifierId,
                                                  IsCustomer = customer.IsCustomer,
                                                  CollectorId = customer.CollectorId,
                                                  FreelancerId = customer.FreelancerId,
                                                  FreelancerName = customer.FreelancerName,
                                                  ForwarderId = customer.ForwarderId,
                                                  ForwarderName = customer.ForwarderName,
                                                  CustomsAgentId = customer.CustomsAgentId,
                                                  CustomsAgentName = customer.CustomsAgentName,
                                                  MediatorId = customer.MediatorId,
                                                  MediatorName = customer.MediatorName,
                                                  BeforeDeactiveStatusCode = customer.BeforeDeactiveStatusCode,
                                                  ReadyForActivationDate = customer.ReadyForActivationDate,
                                                  CreatedByUserName = customer.CreatedByUserName,
                                                  UpdatedByUserName = customer.UpdatedByUserName,
                                                  PrimaryContactName = customer.PrimaryContactName,
                                                  PrimaryContactEmail = customer.PrimaryContactEmail,
                                                  PrimaryContactId = customer.PrimaryContactId,
                                                  RegionId = customer.RegionId,
                                                  RegionName = customer.RegionName,
                                                  CustomerStatusCode = customer.CustomerStatusCode,
                                                  CustomerStatusName = customer.CustomerStatusName,
                                                  CustomerStatusTemplateCode = customer.CustomerStatusCode == "POT" ? "P" : null,
                                                  LastCallDate = customer.LastCallDate,
                                                  LastMeetingDate = customer.LastMeetingDate,
                                                  LastOpportunityDate = customer.LastOpportunityDate,
                                                  LastOpportunityStatus = customer.LastOpportunityStatus,
                                                  LastOpportunitySubject = customer.LastOpportunitySubject,
                                                  FirstInvoiceDate = customer.FirstInvoiceDate,
                                                  FirstShipmentDate = customer.FirstShipmentDate,
                                                  LastQuoteDate = customer.LastQuoteDate,
                                                  LastInteractionDate = customer.LastInteractionDate,
                                                  EnableConsolidationInvoices = customer.EnableConsolidationInvoices,
                                                  CityName = customer.CityName,
                                                  CountryId = customer.CountryId,
                                                  CountryCode = customer.CountryCode,
                                                  CountryName = customer.CountryName,
                                                  ActivityWatch = customer.ActivityWatch,
                                                  RankId = customer.RankId,
                                                  IndustryId = customer.IndustryId,
                                                  LeadSourceId = customer.LeadSourceId,
                                                  LeadSourceName = customer.LeadSourceName,
                                                  InvoiceCurrencyCode = customer.InvoiceCurrencyCode,
                                                  KnownConsignor = customer.KnownConsignor,
                                                  KCExpirationDate = customer.KCExpirationDate,
                                                  PartnerTypeId = customer.PartnerTypeId,
                                                  CustomerSizeId = customer.CustomerSizeId,
                                                  CustomerSizeName = customer.CustomerSizeName,
                                                  IsCreditLimitEnabled = customer.IsCreditLimitEnabled,
                                                  CreditLimitAmount = customer.CreditLimitAmount,
                                                  CreditLimitOpenBalance = customer.CreditLimitOpenBalance,
                                                  CreditLimitWarningPercentage = customer.CreditLimitWarningPercentage,
                                                  ExternalAccountingBusinessArea = customer.ExternalAccountingBusinessArea,
                                                  PaymentMethodCode = customer.SATPaymentMethodCode,
                                                  BlockNewInvoiceCreation = customer.BlockNewInvoiceCreation,
                                                  BlockNewShipmentCreation = customer.BlockNewShipmentCreation,
                                                  ExternalId2 = customer.ExternalId2,
                                                  SATForeignRFC = customer.SATForeignRFC,
                                                  MetodoPagoCode = customer.MetodoPagoCode,
                                                  UsoCFDICode = customer.UsoCFDICode,
                                                  Address1 = customer.Address1,
                                                  Address2 = customer.Address2,
                                                  Phone = customer.Phone,
                                                  ZipCode = customer.ZipCode,
                                                  CompetitorFields = customer.CompetitorFields,
                                                  ActivationDate = customer.ActivationDate,
                                                  InactiveDate = customer.InactiveDate,
                                                  ActivationRequestDate = customer.ActivationRequestDate,
                                                  ActivatedByUserId = customer.ActivatedByUserId,
                                                  SetAsInactiveByUserId = customer.SetAsInactiveByUserId,
                                                  ActivationRequestedByUserId = customer.ActivationRequestedByUserId,
                                                  ActivatedByUserName = customer.ActivatedByUserName,
                                                  SetAsInactiveByName = customer.SetAsInactiveByName,
                                                  ActivationRequestedByUserName = customer.ActivationRequestedByUserName,
                                                  CreatedByPartner = customer.CreatedByPartner,
                                                  StateName = customer.StateName,
                                              };

            return result;
        }

        public CustomerList GetSingleCustomerList(string id, int tenant)
        {
            CustomerList customerList = (from customer in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Card.SharedLogisticsInvitationStatus").Include("Rank").Include("Collector.Contact").Include("Classifier.Contact").Include("Freelancer.Contact").Include("Forwarder").Include("CustomsAgent").Include("Mediator").Include("Card.CreatedByUser.Contact").Include("Card.UpdatedByUser.Contact").Include("Card.PrimaryContact").Include("Region").Include("CustomerSize").Include("ActivatedByUser.Contact").Include("SetAsInactiveByUser.Contact").Include("ActivationRequestedByUser.Contact")
                                         where customer.Tenant == tenant && customer.Id == id
                                         select new CustomerList()
                                         {
                                             Code = customer.Card.Code,
                                             EnglishName = customer.Card.EnglishName,
                                             LocalName = customer.Card.LocalName,
                                             ReceivablesAccountingCard = customer.Card.ReceivablesAccountingCard,
                                             PayablesAccountingCard = customer.Card.PayablesAccountingCard,
                                             InActive = customer.Card.InActive,
                                             Notes = customer.Card.Notes,
                                             SupportNotes = customer.Card.SupportNotes,
                                             BillToId = customer.BillToId,
                                             BillToName = customer.BillToCard == null ? "" : customer.BillToCard.EnglishName,
                                             Website = customer.Card.Website,
                                             SalesmanUserId = customer.SalesmanUserId,
                                             SalesmanUserEnglishName = customer.SalesmanUser == null ? null : (customer.SalesmanUser.Contact == null ? null : customer.SalesmanUser.Contact.EnglishName),
                                             SalesmanBusinessUnitId = customer.SalesmanUser == null ? null : customer.SalesmanUser.BusinessUnitId,
                                             Id = customer.Id,
                                             PaymentTermId = customer.Card.PaymentTermId,
                                             CreateDate = customer.Card.CreateDate,
                                             UpdateDate = customer.Card.UpdateDate,
                                             CreatedByUserId = customer.Card.CreatedByUserId,
                                             UpdatedByUserId = customer.Card.UpdatedByUserId,
                                             Tenant = customer.Tenant,
                                             VatNumber = customer.Card.VatNumber,
                                             SearchFields = customer.Card.SearchFields,
                                             PaymentTermEnglishName = customer.Card.PaymentTerm != null ? customer.Card.PaymentTerm.EnglishName : null,
                                             InvoiceCurrencyId = customer.Card.InvoiceCurrencyId,
                                             AccountManagerUserEnglishName = customer.AccountManagerUser == null ? null : (customer.AccountManagerUser.Contact == null ? null : customer.AccountManagerUser.Contact.EnglishName),
                                             VatTypeId = customer.Card.VatTypeId,
                                             Field1 = customer.Field1,
                                             Field2 = customer.Field2,
                                             Field3 = customer.Field3,
                                             Field4 = customer.Field4,
                                             Field5 = customer.Field5,
                                             Field6 = customer.Field6,
                                             Field7 = customer.Field7,
                                             Field8 = customer.Field8,
                                             Field9 = customer.Field9,
                                             Field10 = customer.Field10,
                                             RankCode = customer.Rank.Code,
                                             RankName = customer.Rank.Name,
                                             SharedLogisticsInvitationStatusName = customer.Card.SharedLogisticsInvitationStatus != null ? customer.Card.SharedLogisticsInvitationStatus.Name : null,
                                             IsActiveForMobile = customer.Card.IsActiveForMobile,
                                             LastLoginDate = customer.Card.LastLoginDate,
                                             InvitationDate = customer.Card.InvitationDate,
                                             IndustryName = customer.Industry != null ? customer.Industry.Name : null,
                                             LeadDescription = customer.LeadDescription,
                                             ClassifierId = customer.ClassifierId,
                                             CollectorId = customer.CollectorId,
                                             ClassifierName = customer.Classifier != null ? customer.Classifier.Contact.EnglishName : null,
                                             CollectorName = customer.Collector != null ? customer.Collector.Contact.EnglishName : null,
                                             IsCustomer = customer.IsCustomer,
                                             CustomerStatusCode = customer.CustomerStatusCode,
                                             CustomerStatusName = customer.CustomerStatus != null ? customer.CustomerStatus.Name : null,
                                             FreelancerId = customer.FreelancerId,
                                             FreelancerName = customer.Freelancer != null ? customer.Freelancer.Contact.EnglishName : null,
                                             ForwarderId = customer.ForwarderId,
                                             ForwarderName = customer.Forwarder != null ? customer.Forwarder.EnglishName : null,
                                             CustomsAgentId = customer.CustomsAgentId,
                                             CustomsAgentName = customer.CustomsAgent != null ? customer.CustomsAgent.EnglishName : null,
                                             MediatorId = customer.MediatorId,
                                             MediatorName = customer.Mediator != null ? customer.Mediator.EnglishName : null,
                                             BeforeDeactiveStatusCode = customer.BeforeDeactiveStatusCode,
                                             ReadyForActivationDate = customer.ReadyForActivationDate,
                                             CreatedByUserName = customer.Card.CreatedByUser != null ? customer.Card.CreatedByUser.Contact.EnglishName : null,
                                             UpdatedByUserName = customer.Card.UpdatedByUser != null ? customer.Card.UpdatedByUser.Contact.EnglishName : null,
                                             PrimaryContactName = customer.PrimaryContactName,
                                             PrimaryContactEmail = customer.PrimaryContactEmail,
                                             PrimaryContactPhone = customer.PrimaryContactPhone,
                                             PrimaryContactId = customer.Card.PrimaryContactId,
                                             RegionId = customer.RegionId,
                                             RegionName = customer.Region != null ? customer.Region.Name : null,
                                             CustomerStatusTemplateCode = customer.CustomerStatusCode == "POT" ? "P" : null,
                                             LastCallDate = customer.LastCallDate,
                                             LastMeetingDate = customer.LastMeetingDate,
                                             LastOpportunityDate = customer.LastOpportunityDate,
                                             LastOpportunityStatus = customer.LastOpportunityStatus,
                                             LastOpportunitySubject = customer.LastOpportunitySubject,
                                             FirstInvoiceDate = customer.FirstInvoiceDate,
                                             FirstShipmentDate = customer.FirstShipmentDate,
                                             LastShipmentDate = customer.LastShipmentDate,
                                             StartWorkingDate = customer.StartWorkingDate,
                                             StartWorkingManuallySet = customer.StartWorkingManuallySet,
                                             LastQuoteDate = customer.LastQuoteDate,
                                             LastInteractionDate = customer.LastInteractionDate,
                                             EnableConsolidationInvoices = customer.Card.EnableConsolidationInvoices,
                                             CityName = customer.Card.CityName,
                                             CountryId = customer.Card.CountryId,
                                             CountryCode = customer.Card.CountryCode,
                                             CountryName = customer.Card.CountryName,
                                             ActivityWatch = customer.ActivityWatch,
                                             RankId = customer.RankId,
                                             IndustryId = customer.IndustryId,
                                             LeadSourceId = customer.LeadSourceId,
                                             LeadSourceName = customer.LeadSource != null ? customer.LeadSource.Name : null,
                                             InvoiceCurrencyCode = customer.Card.InvoiceCurrency == null ? null : customer.Card.InvoiceCurrency.Code,
                                             KnownConsignor = customer.KnownConsignor,
                                             KCExpirationDate = customer.KCExpirationDate,
                                             PartnerTypeId = customer.Card.PartnerTypeId,
                                             LogBoxActivated = customer.LogBoxActivated,
                                             IsPrivateLabelCustomer = customer.IsPrivateLabelCustomer,
                                             CustomerSizeId = customer.CustomerSizeId,
                                             CustomerSizeName = customer.CustomerSize == null ? null : customer.CustomerSize.Name,
                                             IsCreditLimitEnabled = customer.IsCreditLimitEnabled,
                                             CreditLimitAmount = customer.CreditLimitAmount,
                                             CreditLimitOpenBalance = customer.CreditLimitOpenBalance,
                                             CreditLimitWarningPercentage = customer.CreditLimitWarningPercentage,
                                             ExternalAccountingBusinessArea = customer.Card.ExternalAccountingBusinessArea,
                                             PaymentMethodCode = customer.Card.SATPaymentMethodCode,
                                             BlockNewInvoiceCreation = customer.BlockNewInvoiceCreation,
                                             BlockNewShipmentCreation = customer.BlockNewShipmentCreation,
                                             ExternalId2 = customer.Card.ExternalId2,
                                             SATForeignRFC = customer.Card.SATForeignRFC,
                                             MetodoPagoCode = customer.Card.MetodoPagoCode,
                                             UsoCFDICode = customer.Card.UsoCFDICode,
                                             Address1 = customer.Card != null ? customer.Card.Address1 : null,
                                             Address2 = customer.Card != null ? customer.Card.Address2 : null,
                                             ActivationDate = customer.ActivationDate,
                                             ActivationRequestDate = customer.ActivationRequestDate,
                                             InactiveDate = customer.InactiveDate,
                                             ActivatedByUserId = customer.ActivatedByUserId,
                                             SetAsInactiveByUserId = customer.SetAsInactiveByUserId,
                                             ActivationRequestedByUserId = customer.ActivationRequestedByUserId,
                                             ActivatedByUserName = customer.ActivatedByUser == null ? null : (customer.ActivatedByUser.Contact == null ? null : customer.ActivatedByUser.Contact.EnglishName),
                                             SetAsInactiveByName = customer.SetAsInactiveByUser == null ? null : (customer.SetAsInactiveByUser.Contact == null ? null : customer.SetAsInactiveByUser.Contact.EnglishName),
                                             ActivationRequestedByUserName = customer.ActivationRequestedByUser == null ? null : (customer.ActivationRequestedByUser.Contact == null ? null : customer.ActivationRequestedByUser.Contact.EnglishName),
                                             CreatedByPartner = customer.Card.CreatedByPartner,
                                         }).FirstOrDefault();

            return customerList;
        }

        public CustomerList GetSingleCustomerListById(string id, int tenant)
        {
            CustomerRepository CustomerRepository = new CustomerRepository(tenant);
            CustomerList customerList = null;
            Customer customer = CustomerRepository.GetSingleCustomer(id, tenant, false);
            customerList = GetSingleCustomerList(id, tenant);

            if (customer != null && customer.Rank != null)
            {
                customerList.RankCode = customer.Rank.Code;
                customerList.RankName = customer.Rank.Name;
            }

            if (customer != null && customer.AccountManagerUser != null)
            {
                customerList.AccountManagerUserEnglishName = customer.AccountManagerUser.Contact.EnglishName;
            }

            if (customer != null && customer.SalesmanUser != null)
            {
                customerList.SalesmanUserEnglishName = customer.SalesmanUser.Contact.EnglishName;
            }

            if (customer != null && customer.Collector != null)
            {
                customerList.CollectorName = customer.Collector.Contact.EnglishName;
            }

            if (customer != null && customer.Classifier != null)
            {
                customerList.ClassifierName = customer.Classifier.Contact.EnglishName;
            }

            if (customer != null && customer.Card != null)
            {
                customerList.ReceivablesAccountingCard = customer.Card.ReceivablesAccountingCard;
                customerList.PayablesAccountingCard = customer.Card.PayablesAccountingCard;
            }

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Customer", tenant, new List<CustomerList> { customerList }.Cast<object>().ToList());

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            myFilter.SetBlockedBusinessUnit(customerList);

            return customerList;
        }

        public CustomerList GetSingleCustomerListByCode(string code, int tenant)
        {
            CustomerList customerList = (from customer in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Card.SharedLogisticsInvitationStatus").Include("Rank").Include("Forwarder").Include("CustomsAgent").Include("Mediator").Include("Card.CreatedByUser.Contact").Include("Card.UpdatedByUser.Contact").Include("Card.PrimaryContact").Include("Card.InvoiceCurrency").Include("CustomerSize")
                                         where customer.Tenant == tenant && customer.Card.Code == code
                                         select new CustomerList()
                                         {
                                             Code = customer.Card.Code,
                                             EnglishName = customer.Card.EnglishName,
                                             LocalName = customer.Card.LocalName,
                                             ReceivablesAccountingCard = customer.Card.ReceivablesAccountingCard,
                                             PayablesAccountingCard = customer.Card.PayablesAccountingCard,
                                             InActive = customer.Card.InActive,
                                             Notes = customer.Card.Notes,
                                             SupportNotes = customer.Card.SupportNotes,
                                             BillToId = customer.BillToId,
                                             BillToName = customer.BillToCard == null ? "" : customer.BillToCard.EnglishName,
                                             Website = customer.Card.Website,
                                             SalesmanUserId = customer.SalesmanUserId,
                                             SalesmanUserEnglishName = customer.SalesmanUser == null ? null : (customer.SalesmanUser.Contact == null ? null : customer.SalesmanUser.Contact.EnglishName),
                                             SalesmanBusinessUnitId = customer.SalesmanUser == null ? null : customer.SalesmanUser.BusinessUnitId,
                                             Id = customer.Id,
                                             PaymentTermId = customer.Card.PaymentTermId,
                                             CreateDate = customer.Card.CreateDate,
                                             UpdateDate = customer.Card.UpdateDate,
                                             CreatedByUserId = customer.Card.CreatedByUserId,
                                             UpdatedByUserId = customer.Card.UpdatedByUserId,
                                             Tenant = customer.Tenant,
                                             VatNumber = customer.Card.VatNumber,
                                             SearchFields = customer.Card.SearchFields,
                                             PaymentTermEnglishName = customer.Card.PaymentTerm != null ? customer.Card.PaymentTerm.EnglishName : null,
                                             InvoiceCurrencyId = customer.Card.InvoiceCurrencyId,
                                             AccountManagerUserEnglishName = customer.AccountManagerUser == null ? null : (customer.AccountManagerUser.Contact == null ? null : customer.AccountManagerUser.Contact.EnglishName),
                                             VatTypeId = customer.Card.VatTypeId,
                                             Field1 = customer.Field1,
                                             Field2 = customer.Field2,
                                             Field3 = customer.Field3,
                                             Field4 = customer.Field4,
                                             Field5 = customer.Field5,
                                             Field6 = customer.Field6,
                                             Field7 = customer.Field7,
                                             Field8 = customer.Field8,
                                             Field9 = customer.Field9,
                                             Field10 = customer.Field10,
                                             RankCode = customer.Rank.Code,
                                             RankName = customer.Rank.Name,
                                             SharedLogisticsInvitationStatusName = customer.Card.SharedLogisticsInvitationStatus != null ? customer.Card.SharedLogisticsInvitationStatus.Name : null,
                                             IsActiveForMobile = customer.Card.IsActiveForMobile,
                                             LastLoginDate = customer.Card.LastLoginDate,
                                             InvitationDate = customer.Card.InvitationDate,
                                             IndustryName = customer.Industry != null ? customer.Industry.Name : null,
                                             LeadDescription = customer.LeadDescription,
                                             ClassifierId = customer.ClassifierId,
                                             ClassifierName = customer.Classifier != null ? customer.Classifier.Contact.EnglishName : null,
                                             CollectorId = customer.CollectorId,
                                             CollectorName = customer.Collector != null ? customer.Collector.Contact.EnglishName : null,
                                             IsCustomer = customer.IsCustomer,
                                             CustomerStatusCode = customer.CustomerStatusCode,
                                             CustomerStatusName = customer.CustomerStatus != null ? customer.CustomerStatus.Name : null,
                                             ForwarderId = customer.ForwarderId,
                                             ForwarderName = customer.Forwarder != null ? customer.Forwarder.EnglishName : null,
                                             CustomsAgentId = customer.CustomsAgentId,
                                             CustomsAgentName = customer.CustomsAgent != null ? customer.CustomsAgent.EnglishName : null,
                                             MediatorId = customer.MediatorId,
                                             MediatorName = customer.Mediator != null ? customer.Mediator.EnglishName : null,
                                             BeforeDeactiveStatusCode = customer.BeforeDeactiveStatusCode,
                                             ReadyForActivationDate = customer.ReadyForActivationDate,
                                             CreatedByUserName = customer.Card.CreatedByUser != null ? customer.Card.CreatedByUser.Contact.EnglishName : null,
                                             UpdatedByUserName = customer.Card.UpdatedByUser != null ? customer.Card.UpdatedByUser.Contact.EnglishName : null,
                                             PrimaryContactName = customer.PrimaryContactName,
                                             PrimaryContactEmail = customer.PrimaryContactEmail,
                                             PrimaryContactPhone = customer.PrimaryContactPhone,
                                             PrimaryContactId = customer.Card.PrimaryContactId,
                                             RegionId = customer.RegionId,
                                             RegionName = customer.Region != null ? customer.Region.Name : null,
                                             CustomerStatusTemplateCode = customer.CustomerStatusCode == "POT" ? "P" : null,
                                             LastCallDate = customer.LastCallDate,
                                             LastMeetingDate = customer.LastMeetingDate,
                                             LastOpportunityDate = customer.LastOpportunityDate,
                                             LastOpportunityStatus = customer.LastOpportunityStatus,
                                             LastOpportunitySubject = customer.LastOpportunitySubject,
                                             FirstInvoiceDate = customer.FirstInvoiceDate,
                                             FirstShipmentDate = customer.FirstShipmentDate,
                                             LastShipmentDate = customer.LastShipmentDate,
                                             StartWorkingDate = customer.StartWorkingDate,
                                             StartWorkingManuallySet = customer.StartWorkingManuallySet,
                                             LastQuoteDate = customer.LastQuoteDate,
                                             LastInteractionDate = customer.LastInteractionDate,
                                             EnableConsolidationInvoices = customer.Card.EnableConsolidationInvoices,
                                             CityName = customer.Card.CityName,
                                             CountryId = customer.Card.CountryId,
                                             CountryCode = customer.Card.CountryCode,
                                             CountryName = customer.Card.CountryName,
                                             ActivityWatch = customer.ActivityWatch,
                                             RankId = customer.RankId,
                                             IndustryId = customer.IndustryId,
                                             LeadSourceId = customer.LeadSourceId,
                                             InvoiceCurrencyCode = customer.Card.InvoiceCurrency == null ? null : customer.Card.InvoiceCurrency.Code,
                                             KnownConsignor = customer.KnownConsignor,
                                             KCExpirationDate = customer.KCExpirationDate,
                                             PartnerTypeId = customer.Card.PartnerTypeId,
                                             LogBoxActivated = customer.LogBoxActivated,
                                             CustomerSizeId = customer.CustomerSizeId,
                                             CustomerSizeName = customer.CustomerSize == null ? null : customer.CustomerSize.Name,
                                             IsPrivateLabelCustomer = customer.IsPrivateLabelCustomer,
                                             IsCreditLimitEnabled = customer.IsCreditLimitEnabled,
                                             CreditLimitAmount = customer.CreditLimitAmount,
                                             CreditLimitOpenBalance = customer.CreditLimitOpenBalance,
                                             CreditLimitWarningPercentage = customer.CreditLimitWarningPercentage,
                                             ExternalAccountingBusinessArea = customer.Card.ExternalAccountingBusinessArea,
                                             PaymentMethodCode = customer.Card.SATPaymentMethodCode,
                                             BlockNewInvoiceCreation = customer.BlockNewInvoiceCreation,
                                             BlockNewShipmentCreation = customer.BlockNewShipmentCreation,
                                             ExternalId2 = customer.Card.ExternalId2,
                                             SATForeignRFC = customer.Card.SATForeignRFC,
                                             MetodoPagoCode = customer.Card.MetodoPagoCode,
                                             UsoCFDICode = customer.Card.UsoCFDICode,
                                             CreatedByPartner = customer.Card.CreatedByPartner,
                                         }).FirstOrDefault();

            return customerList;
        }

        public CustomerList GetSingleCustomerListByVatNumber(string vatnumber, int tenant)
        {
            CustomerList customerList = (from customer in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Card.SharedLogisticsInvitationStatus").Include("Rank").Include("Forwarder").Include("CustomsAgent").Include("Mediator").Include("Card.CreatedByUser.Contact").Include("Card.UpdatedByUser.Contact").Include("Card.PrimaryContact").Include("Card.InvoiceCurrency").Include("CustomerSize")
                                         where customer.Tenant == tenant && customer.Card.VatNumber == vatnumber
                                         select new CustomerList()
                                         {
                                             Code = customer.Card.Code,
                                             EnglishName = customer.Card.EnglishName,
                                             LocalName = customer.Card.LocalName,
                                             ReceivablesAccountingCard = customer.Card.ReceivablesAccountingCard,
                                             PayablesAccountingCard = customer.Card.PayablesAccountingCard,
                                             InActive = customer.Card.InActive,
                                             Notes = customer.Card.Notes,
                                             SupportNotes = customer.Card.SupportNotes,
                                             BillToId = customer.BillToId,
                                             BillToName = customer.BillToCard == null ? "" : customer.BillToCard.EnglishName,
                                             Website = customer.Card.Website,
                                             SalesmanUserId = customer.SalesmanUserId,
                                             SalesmanUserEnglishName = customer.SalesmanUser == null ? null : (customer.SalesmanUser.Contact == null ? null : customer.SalesmanUser.Contact.EnglishName),
                                             SalesmanBusinessUnitId = customer.SalesmanUser == null ? null : customer.SalesmanUser.BusinessUnitId,
                                             Id = customer.Id,
                                             PaymentTermId = customer.Card.PaymentTermId,
                                             CreateDate = customer.Card.CreateDate,
                                             UpdateDate = customer.Card.UpdateDate,
                                             CreatedByUserId = customer.Card.CreatedByUserId,
                                             UpdatedByUserId = customer.Card.UpdatedByUserId,
                                             Tenant = customer.Tenant,
                                             VatNumber = customer.Card.VatNumber,
                                             SearchFields = customer.Card.SearchFields,
                                             PaymentTermEnglishName = customer.Card.PaymentTerm != null ? customer.Card.PaymentTerm.EnglishName : null,
                                             InvoiceCurrencyId = customer.Card.InvoiceCurrencyId,
                                             AccountManagerUserEnglishName = customer.AccountManagerUser == null ? null : (customer.AccountManagerUser.Contact == null ? null : customer.AccountManagerUser.Contact.EnglishName),
                                             VatTypeId = customer.Card.VatTypeId,
                                             Field1 = customer.Field1,
                                             Field2 = customer.Field2,
                                             Field3 = customer.Field3,
                                             Field4 = customer.Field4,
                                             Field5 = customer.Field5,
                                             Field6 = customer.Field6,
                                             Field7 = customer.Field7,
                                             Field8 = customer.Field8,
                                             Field9 = customer.Field9,
                                             Field10 = customer.Field10,
                                             RankCode = customer.Rank.Code,
                                             RankName = customer.Rank.Name,
                                             SharedLogisticsInvitationStatusName = customer.Card.SharedLogisticsInvitationStatus != null ? customer.Card.SharedLogisticsInvitationStatus.Name : null,
                                             IsActiveForMobile = customer.Card.IsActiveForMobile,
                                             LastLoginDate = customer.Card.LastLoginDate,
                                             InvitationDate = customer.Card.InvitationDate,
                                             IndustryName = customer.Industry != null ? customer.Industry.Name : null,
                                             LeadDescription = customer.LeadDescription,
                                             ClassifierId = customer.ClassifierId,
                                             ClassifierName = customer.Classifier != null ? customer.Classifier.Contact.EnglishName : null,
                                             CollectorId = customer.CollectorId,
                                             CollectorName = customer.Collector != null ? customer.Collector.Contact.EnglishName : null,
                                             IsCustomer = customer.IsCustomer,
                                             CustomerStatusCode = customer.CustomerStatusCode,
                                             CustomerStatusName = customer.CustomerStatus != null ? customer.CustomerStatus.Name : null,
                                             ForwarderId = customer.ForwarderId,
                                             ForwarderName = customer.Forwarder != null ? customer.Forwarder.EnglishName : null,
                                             CustomsAgentId = customer.CustomsAgentId,
                                             CustomsAgentName = customer.CustomsAgent != null ? customer.CustomsAgent.EnglishName : null,
                                             MediatorId = customer.MediatorId,
                                             MediatorName = customer.Mediator != null ? customer.Mediator.EnglishName : null,
                                             BeforeDeactiveStatusCode = customer.BeforeDeactiveStatusCode,
                                             ReadyForActivationDate = customer.ReadyForActivationDate,
                                             CreatedByUserName = customer.Card.CreatedByUser != null ? customer.Card.CreatedByUser.Contact.EnglishName : null,
                                             UpdatedByUserName = customer.Card.UpdatedByUser != null ? customer.Card.UpdatedByUser.Contact.EnglishName : null,
                                             PrimaryContactName = customer.PrimaryContactName,
                                             PrimaryContactEmail = customer.PrimaryContactEmail,
                                             PrimaryContactPhone = customer.PrimaryContactPhone,
                                             PrimaryContactId = customer.Card.PrimaryContactId,
                                             RegionId = customer.RegionId,
                                             RegionName = customer.Region != null ? customer.Region.Name : null,
                                             CustomerStatusTemplateCode = customer.CustomerStatusCode == "POT" ? "P" : null,
                                             LastCallDate = customer.LastCallDate,
                                             LastMeetingDate = customer.LastMeetingDate,
                                             LastOpportunityDate = customer.LastOpportunityDate,
                                             LastOpportunityStatus = customer.LastOpportunityStatus,
                                             LastOpportunitySubject = customer.LastOpportunitySubject,
                                             FirstInvoiceDate = customer.FirstInvoiceDate,
                                             FirstShipmentDate = customer.FirstShipmentDate,
                                             LastShipmentDate = customer.LastShipmentDate,
                                             StartWorkingDate = customer.StartWorkingDate,
                                             StartWorkingManuallySet = customer.StartWorkingManuallySet,
                                             LastQuoteDate = customer.LastQuoteDate,
                                             LastInteractionDate = customer.LastInteractionDate,
                                             EnableConsolidationInvoices = customer.Card.EnableConsolidationInvoices,
                                             CityName = customer.Card.CityName,
                                             CountryId = customer.Card.CountryId,
                                             CountryCode = customer.Card.CountryCode,
                                             CountryName = customer.Card.CountryName,
                                             ActivityWatch = customer.ActivityWatch,
                                             RankId = customer.RankId,
                                             IndustryId = customer.IndustryId,
                                             LeadSourceId = customer.LeadSourceId,
                                             InvoiceCurrencyCode = customer.Card.InvoiceCurrency == null ? null : customer.Card.InvoiceCurrency.Code,
                                             KnownConsignor = customer.KnownConsignor,
                                             KCExpirationDate = customer.KCExpirationDate,
                                             PartnerTypeId = customer.Card.PartnerTypeId,
                                             LogBoxActivated = customer.LogBoxActivated,
                                             CustomerSizeId = customer.CustomerSizeId,
                                             CustomerSizeName = customer.CustomerSize == null ? null : customer.CustomerSize.Name,
                                             IsPrivateLabelCustomer = customer.IsPrivateLabelCustomer,
                                             IsCreditLimitEnabled = customer.IsCreditLimitEnabled,
                                             CreditLimitAmount = customer.CreditLimitAmount,
                                             CreditLimitOpenBalance = customer.CreditLimitOpenBalance,
                                             CreditLimitWarningPercentage = customer.CreditLimitWarningPercentage,
                                             ExternalAccountingBusinessArea = customer.Card.ExternalAccountingBusinessArea,
                                             PaymentMethodCode = customer.Card.SATPaymentMethodCode,
                                             BlockNewInvoiceCreation = customer.BlockNewInvoiceCreation,
                                             BlockNewShipmentCreation = customer.BlockNewShipmentCreation,
                                             ExternalId2 = customer.Card.ExternalId2,
                                             SATForeignRFC = customer.Card.SATForeignRFC,
                                             MetodoPagoCode = customer.Card.MetodoPagoCode,
                                             UsoCFDICode = customer.Card.UsoCFDICode,
                                             CreatedByPartner = customer.Card.CreatedByPartner,

                                         }).FirstOrDefault();

            return customerList;
        }

        public List<CustomerList> GetRecentEntityLists(int tenant, string userId, string objectTableId)
        {
            List<CustomerList> entityList = new List<CustomerList>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            repository = new CustomerRepository(tenant);

            IQueryable<Customer> entities = null;
            entities = repository.GetCustomers(tenant);

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                Customer a = (from d in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Rank").Include("Card.SharedLogisticsInvitationStatus").Include("Industry").Include("Collector.Contact").Include("Classifier.Contact").Include("Freelancer.Contact").Include("Forwarder").Include("CustomsAgent").Include("Mediator").Include("Card.CreatedByUser.Contact").Include("Card.UpdatedByUser.Contact").Include("Card.PrimaryContact").Include("Card.InvoiceCurrency").Include("CustomerStatus")
                              where d.Id == lastActivity.EntityId
                              select d).FirstOrDefault();

                if (a != null)
                {
                    CustomerList list = new CustomerList()
                    {
                        BillToId = a.BillToId,
                        BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                        Id = a.Id,
                        SalesmanUserId = a.SalesmanUserId,
                        SalesmanUserEnglishName = a.SalesmanUser == null ? null : (a.SalesmanUser.Contact == null ? null : a.SalesmanUser.Contact.EnglishName),
                        SalesmanBusinessUnitId = a.SalesmanUser == null ? null : a.SalesmanUser.BusinessUnitId,
                        Tenant = a.Tenant,
                        Website = a.Card.Website,
                        Code = a.Card.Code,
                        LocalName = a.Card.LocalName,
                        EnglishName = a.Card.EnglishName,
                        ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                        PayablesAccountingCard = a.Card.PayablesAccountingCard,
                        CreateDate = a.Card.CreateDate,
                        UpdateDate = a.Card.UpdateDate,
                        CreatedByUserId = a.Card.CreatedByUserId,
                        UpdatedByUserId = a.Card.UpdatedByUserId,
                        InActive = a.Card.InActive,
                        Notes = a.Card.Notes,
                        SupportNotes = a.Card.SupportNotes,
                        PaymentTermId = a.Card.PaymentTermId,
                        VatNumber = a.Card.VatNumber,
                        InvoiceCurrencyId = a.Card.InvoiceCurrencyId,
                        AccountManagerUserEnglishName = a.AccountManagerUser != null ? a.AccountManagerUser.Contact.EnglishName : null,
                        RankName = a.Rank != null ? a.Rank.Name : null,
                        RankCode = a.Rank != null ? a.Rank.Code : null,
                        VatTypeId = a.Card.VatTypeId,
                        SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                        IsActiveForMobile = a.Card.IsActiveForMobile,
                        LastLoginDate = a.Card.LastLoginDate,
                        InvitationDate = a.Card.InvitationDate,
                        IndustryName = a.Industry != null ? a.Industry.Name : null,
                        ClassifierName = a.Classifier != null ? a.Classifier.Contact.EnglishName : null,
                        CollectorName = a.Collector != null ? a.Collector.Contact.EnglishName : null,
                        LeadDescription = a.LeadDescription,
                        LastActivityDate = lastActivity.ActivityDate,
                        LastActivityTypeName = lastActivity.ActivityType.Name,
                        LastActivityByUserName = lastActivity.User.Contact.EnglishName,
                        ClassifierId = a.ClassifierId,
                        IsCustomer = a.IsCustomer,
                        CustomerStatusCode = a.CustomerStatusCode,
                        CustomerStatusName = a.CustomerStatus != null ? a.CustomerStatus.Name : null,
                        CollectorId = a.CollectorId,
                        FreelancerId = a.FreelancerId,
                        FreelancerName = a.Freelancer != null ? a.Freelancer.Contact.EnglishName : null,
                        ForwarderId = a.ForwarderId,
                        ForwarderName = a.Forwarder != null ? a.Forwarder.EnglishName : null,
                        CustomsAgentId = a.CustomsAgentId,
                        CustomsAgentName = a.CustomsAgent != null ? a.CustomsAgent.EnglishName : null,
                        MediatorId = a.MediatorId,
                        MediatorName = a.Mediator != null ? a.Mediator.EnglishName : null,
                        BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                        ReadyForActivationDate = a.ReadyForActivationDate,
                        CreatedByUserName = a.Card.CreatedByUser != null ? a.Card.CreatedByUser.Contact.EnglishName : null,
                        UpdatedByUserName = a.Card.UpdatedByUser != null ? a.Card.UpdatedByUser.Contact.EnglishName : null,
                        PrimaryContactName = a.PrimaryContactName,
                        PrimaryContactEmail = a.PrimaryContactEmail,
                        PrimaryContactPhone = a.PrimaryContactPhone,
                        PrimaryContactId = a.Card.PrimaryContactId,
                        RegionId = a.RegionId,
                        RegionName = a.Region != null ? a.Region.Name : null,
                        CustomerStatusTemplateCode = a.CustomerStatusCode == "POT" ? "P" : null,
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
                        CityName = a.Card.CityName,
                        CountryId = a.Card.CountryId,
                        CountryCode = a.Card.CountryCode,
                        CountryName = a.Card.CountryName,
                        ActivityWatch = a.ActivityWatch,
                        RankId = a.RankId,
                        IndustryId = a.IndustryId,
                        LeadSourceId = a.LeadSourceId,
                        InvoiceCurrencyCode = a.Card.InvoiceCurrency == null ? null : a.Card.InvoiceCurrency.Code,
                        KnownConsignor = a.KnownConsignor,
                        KCExpirationDate = a.KCExpirationDate,
                        PartnerTypeId = a.Card.PartnerTypeId,
                        LogBoxActivated = a.LogBoxActivated,
                        IsPrivateLabelCustomer = a.IsPrivateLabelCustomer,
                        IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                        CreditLimitAmount = a.CreditLimitAmount,
                        CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                        CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                        ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                        PaymentMethodCode = a.Card.SATPaymentMethodCode,
                        BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                        BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                        ExternalId2 = a.Card.ExternalId2,
                        SATForeignRFC = a.Card.SATForeignRFC,
                        MetodoPagoCode = a.Card.MetodoPagoCode,
                        UsoCFDICode = a.Card.UsoCFDICode,
                        CreatedByPartner = a.Card.CreatedByPartner,
                    };

                    entityList.Add(list);
                }
            }

            return entityList;
        }

        public List<CustomerCurrencyCode> GetCurrencyCodeForCustomer(string customerId, int tenant)
        {
            ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(tenant);
            List<ARInvoicePM> invoices = invoiceQuery.GetInvoicesByCustomer(customerId, tenant).ToList();
            List<CustomerCurrencyCode> result = (from a in invoices
                                                 group a by new { a.InvoiceCurrencyId } into gr
                                                 select new CustomerCurrencyCode()
                                                 {
                                                     InvoiceCurrencyId = gr.Key.InvoiceCurrencyId,
                                                 }).ToList();
            return result;
        }

        public CardExternalCodeByCurrencyPM GetSingleCardExternalCodeByCurrencyPM(string cardId, string currencyId, int tenant)
        {
            return (from a in repository.context.CardExternalCodeByCurrencies
                    where a.CardId == cardId && a.CurrencyId == currencyId && a.Tenant == tenant
                    select new CardExternalCodeByCurrencyPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CardId = a.CardId,
                        CurrencyId = a.CurrencyId,
                        CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                        //  ExternalTableId = a.ExternalTableId,
                        //  ExternalTableName = a.ExternalTable != null ? a.ExternalTable.Name : null,
                        // ExternalTableCode = a.ExternalTable != null ? a.ExternalTable.Code : null,

                    }).FirstOrDefault();
        }

        public List<ChartingDataClass> GetCustomersGroupBySalesman(int days, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();
            IQueryable<CustomersDataView> dataViews = repository.GetCustomersDataViews(tenant);
            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date1 = todayDate.Value.AddDays(days).Date;
            DateTime? date2 = todayDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59);

            IQueryable<CustomersDataView> dataSourceQuery =
                (from d in dataViews
                 where d.Tenant == tenant
                 && d.SalesmanUserId != null
                 && !d.InActive
                 select d);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            dataSourceQuery = myFilter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanBusinessUnitId == businessUnitId);
            }

            if (fieldCode == "C")
            {
                if (days >= 0)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) == todayDate);
                }

                else if (days == -1)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) == date1);
                }

                else
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= date2);
                }
            }

            else if (fieldCode == "P")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.CustomerStatusCode == "POT");
            }

            if (dataSourceQuery != null)
            {
                if (isTopTen)
                {
                    myResult =
                        (from d in dataSourceQuery
                         group d by new { d.SalesmanUserId, d.SalesmanUserEnglishName } into g
                         select new ChartingDataClass()
                         {
                             Id = g.Key.SalesmanUserId,
                             StringProperty = g.Key.SalesmanUserEnglishName,
                             IntegerProperty = g.Count(),
                             Day = days,
                             OwnerId = g.Key.SalesmanUserId,
                             BusinessUnitId = businessUnitId
                         })
                         .OrderByDescending(o => o.IntegerProperty)
                         .Take(10)
                         .ToList();
                }

                else
                {
                    myResult =
                        (from d in dataSourceQuery
                         group d by new { d.SalesmanUserId, d.SalesmanUserEnglishName } into g
                         select new ChartingDataClass()
                         {
                             Id = g.Key.SalesmanUserId,
                             StringProperty = g.Key.SalesmanUserEnglishName,
                             IntegerProperty = g.Count(),
                             Day = days,
                             OwnerId = g.Key.SalesmanUserId,
                             BusinessUnitId = businessUnitId
                         })
                         .ToList();
                }
            }

            return myResult;
        }
        public List<ChartingDataClass> GetCustomersGroupBySalesmanCustom(DateTime? FromDate, DateTime? ToDate, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            List<ChartingDataClass> myResult = new List<ChartingDataClass>();
            IQueryable<CustomersDataView> dataViews = repository.GetCustomersDataViews(tenant);


            IQueryable<CustomersDataView> dataSourceQuery =
                (from d in dataViews
                 where d.Tenant == tenant
                 && d.SalesmanUserId != null
                 && !d.InActive
                 select d);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            dataSourceQuery = myFilter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanBusinessUnitId == businessUnitId);
            }

            if (fieldCode == "C")
            {

                dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= ToDate);

            }

            else if (fieldCode == "P")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.CustomerStatusCode == "POT");
            }

            if (dataSourceQuery != null)
            {
                if (isTopTen)
                {
                    myResult =
                        (from d in dataSourceQuery
                         group d by new { d.SalesmanUserId, d.SalesmanUserEnglishName } into g
                         select new ChartingDataClass()
                         {
                             Id = g.Key.SalesmanUserId,
                             StringProperty = g.Key.SalesmanUserEnglishName,
                             IntegerProperty = g.Count(),
                             OwnerId = g.Key.SalesmanUserId,
                             BusinessUnitId = businessUnitId
                         })
                         .OrderByDescending(o => o.IntegerProperty)
                         .Take(10)
                         .ToList();
                }

                else
                {
                    myResult =
                        (from d in dataSourceQuery
                         group d by new { d.SalesmanUserId, d.SalesmanUserEnglishName } into g
                         select new ChartingDataClass()
                         {
                             Id = g.Key.SalesmanUserId,
                             StringProperty = g.Key.SalesmanUserEnglishName,
                             IntegerProperty = g.Count(),
                             OwnerId = g.Key.SalesmanUserId,
                             BusinessUnitId = businessUnitId
                         })
                         .ToList();
                }
            }

            return myResult;
        }

        public List<CustomerDW> GetCustomersDWLists(int tenant, int skip, int take)
        {

            List<CustomerDW> CustomerDWLists = (from a in repository.context.Customers.Include("CustomerStatus").Include("Region").Include("CustomerSize").Include("Rank")
                                                join card in repository.context.Cards on a.Id equals card.Id
                                                where a.Tenant == tenant && (card.PartnerTypeId == "CS" || card.PartnerTypeId == "PO")
                                                select new CustomerDW()
                                                {
                                                    Name = card.EnglishName,
                                                    Code = card.Code,
                                                    CustomerStatusName = a.CustomerStatus != null ? a.CustomerStatus.Name : "",
                                                    CardId = card.Id,
                                                    City = card.CityName,
                                                    CountryID = card.CountryId,
                                                    CountryName = card.CountryName,
                                                    CountryCode = card.CountryCode,
                                                    RegionName = a.Region != null ? a.Region.Name : "",
                                                    ForwarderId = a.ForwarderId,
                                                    CustomsAgentId = a.CustomsAgentId,
                                                    RankId = a.RankId,
                                                    RankName = a.Rank != null ? a.Rank.Name : "",
                                                    SalesmanUserId = a.SalesmanUserId,
                                                    CreateDate = card.CreateDate,
                                                    UpdateDate = card.UpdateDate,
                                                    StartWorkingDate = a.StartWorkingDate,
                                                    ClientID = card.Id,
                                                    CustomerSizeID = a.CustomerSizeId,
                                                    ClientNameHEB = card.EnglishName,
                                                    CustomerSizeName = a.CustomerSize != null ? a.CustomerSize.Name : "",
                                                    Id = a.Id,
                                                    ActivityWatch = a.ActivityWatch,
                                                    PrimaryContactId = card != null ? card.PrimaryContactId : "",
                                                    TenantNumber = card != null ? card.ReceivablesAccountingCard : "",
                                                    Reseller = a.Field2,
                                                    CardType = card.PartnerType != null ? card.PartnerType.Name : "",
                                                    IsCustomer = card.IsCustomer,
                                                }).OrderBy(d => d.CreateDate).Skip(skip).Take(take).ToList();

            return SetOtherPropInCustomerDwLists(CustomerDWLists, tenant);
        }

        public List<CustomerDW> GetCustomersDWByListsUpdateDate(int tenant, DateTime updateDate, int skip, int take)
        {
            List<CustomerDW> CustomerDWLists = (from a in repository.context.Customers.Include("CustomerStatus").Include("Region").Include("CustomerSize").Include("Rank")
                                                join card in repository.context.Cards on a.Id equals card.Id
                                                where a.Tenant == tenant && card.UpdateDate > updateDate && (card.PartnerTypeId == "CS" || card.PartnerTypeId == "PO")
                                                select new CustomerDW()
                                                {
                                                    Name = card.EnglishName,
                                                    Code = card.Code,
                                                    CustomerStatusName = a.CustomerStatus != null ? a.CustomerStatus.Name : "",
                                                    CardId = card.Id,
                                                    City = card.CityName,
                                                    CountryID = card.CountryId,
                                                    CountryName = card.CountryName,
                                                    CountryCode = card.CountryCode,
                                                    RegionName = a.Region != null ? a.Region.Name : "",
                                                    ForwarderId = a.ForwarderId,
                                                    CustomsAgentId = a.CustomsAgentId,
                                                    RankId = a.RankId,
                                                    RankName = a.Rank != null ? a.Rank.Name : "",
                                                    SalesmanUserId = a.SalesmanUserId,
                                                    CreateDate = card.CreateDate,
                                                    UpdateDate = card.UpdateDate,
                                                    StartWorkingDate = a.StartWorkingDate,
                                                    ClientID = card.Id,
                                                    CustomerSizeID = a.CustomerSizeId,
                                                    ClientNameHEB = card.EnglishName,
                                                    CustomerSizeName = a.CustomerSize != null ? a.CustomerSize.Name : "",
                                                    Id = a.Id,
                                                    ActivityWatch = a.ActivityWatch,
                                                    PrimaryContactId = card.PrimaryContactId,
                                                    TenantNumber = card != null ? card.ReceivablesAccountingCard : "",
                                                    Reseller = a.Field2,
                                                    CardType = card.PartnerType != null ? card.PartnerType.Name : "",
                                                    IsCustomer = card.IsCustomer,
                                                }).OrderBy(d => d.CreateDate).Skip(skip).Take(take).ToList();


            return SetOtherPropInCustomerDwLists(CustomerDWLists, tenant);
        }

        private List<CustomerDW> SetOtherPropInCustomerDwLists(List<CustomerDW> customerDWLists, int tenant)
        {
            CustomerCompetitorQuery customerCompetitorQuery = new CustomerCompetitorQuery(tenant);
            CustomerAdditionalServiceQuery customerAdditionalServiceQuery = new CustomerAdditionalServiceQuery(tenant);
            AddressRepository addressRep = new AddressRepository(repository.context);
            AddressQuery addressQuery = new AddressQuery(addressRep);

            List<string> contactIds = new List<string>();
            List<string> customerIds = new List<string>();
            List<string> cardAddressIds = new List<string>();
            List<string> cardIds = new List<string>();


            foreach (CustomerDW item in customerDWLists)
            {
                customerIds.Add(item.Id);


                if (!string.IsNullOrEmpty(item.CardId))
                {
                    if (!cardAddressIds.Contains(item.CardId)) cardAddressIds.Add(item.CardId);
                }

                //ResellerId
                if (!string.IsNullOrEmpty(item.Reseller))
                {
                    if (!cardIds.Contains(item.Reseller)) cardIds.Add(item.Reseller);

                }

                //ForwarderId
                if (!string.IsNullOrEmpty(item.ForwarderId))
                {
                    if (!cardIds.Contains(item.ForwarderId)) cardIds.Add(item.ForwarderId);


                }
                //CustomsAgentId
                if (!string.IsNullOrEmpty(item.CustomsAgentId))
                {
                    if (!cardIds.Contains(item.CustomsAgentId)) cardIds.Add(item.CustomsAgentId);

                }
                //PrimaryContactId
                if (!string.IsNullOrEmpty(item.PrimaryContactId))
                {
                    if (!contactIds.Contains(item.PrimaryContactId)) contactIds.Add(item.PrimaryContactId);

                }
                //SalesmanUserId
                if (!string.IsNullOrEmpty(item.SalesmanUserId))
                {
                    if (!contactIds.Contains(item.SalesmanUserId)) contactIds.Add(item.SalesmanUserId);
                }

                if (string.IsNullOrEmpty(item.ForwarderName)) item.ForwarderName = null;

            }

            List<CustomerAdditionalServicePM> customerAdditionalServicePMLists = customerAdditionalServiceQuery.GetCustomerAdditionalServicesByListsCustomerIds(customerIds, tenant);
            List<CustomerCompetitorPM> customerCompetitorsLists = customerCompetitorQuery.GetCustomerCompetitorsByListsCustomerIds(customerIds, tenant);
            List<AddressList> addressLists = addressQuery.GetAddressListsByCardIds(cardAddressIds, "M", tenant);


            #region cards
            List<CardList> cardLists = null;

            CardQuery cardQuery = new CardQuery(tenant);

            if (cardIds.Count > 0) cardLists = cardQuery.GetCardListToCustomerDWWByCardIds(cardIds, tenant);

            #endregion

            #region user
            List<ContactList> contactLists = null;
            if (contactIds.Count > 0)
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                contactLists = contactQuery.GetContactListsByListIds(contactIds, tenant).ToList(); 
            }

            #endregion

            foreach (CustomerDW item in customerDWLists)
            {
                item.CustomerCompetitors = customerCompetitorsLists.Where(d => d.CustomerId == item.Id).ToList();
                item.OtherServices = customerAdditionalServicePMLists.Where(d => d.CustomerId == item.Id).ToList();

                // ZipCode
                AddressList addressList = addressLists.Where(d => d.CardId == item.CardId).FirstOrDefault();
                if (addressList != null) item.ZipCode = addressList.ZipCode;


                #region Cards

                // ResellerName
                if (!string.IsNullOrEmpty(item.Reseller) && cardLists != null)
                {
                    CardList cardList = cardLists.Where(d => d.Id == item.Reseller).FirstOrDefault();
                    if (cardList != null) item.Reseller = cardList.EnglishName;

                }

                // ForwarderName
                if (!string.IsNullOrEmpty(item.ForwarderId) && cardLists != null)
                {
                    CardList cardList = cardLists.Where(d => d.Id == item.ForwarderId).FirstOrDefault();
                    if (cardList != null) item.ForwarderName = cardList.EnglishName;

                }


                // CustomsAgentName
                if (!string.IsNullOrEmpty(item.CustomsAgentId) && cardLists != null)
                {
                    CardList cardList = cardLists.Where(d => d.Id == item.CustomsAgentId).FirstOrDefault();
                    if (cardList != null) item.CustomsAgentName = cardList.EnglishName;

                }
                #endregion


                #region Contacts
                //SalesmanUserName
                if (!string.IsNullOrEmpty(item.SalesmanUserId) && contactLists != null)
                {
                    ContactList contactList = contactLists.Where(d => d.Id == item.SalesmanUserId).FirstOrDefault();
                    if (contactList != null) item.SalesmanUserName = contactList.EnglishName;

                }

                //PrimaryContact data
                if (!string.IsNullOrEmpty(item.PrimaryContactId) && contactLists != null)
                {
                    ContactList contactList = contactLists.Where(d => d.Id == item.PrimaryContactId).FirstOrDefault();
                    if (contactList != null)
                    {
                        item.EnglishName = contactList.EnglishName;
                        item.LocalName = contactList.LocalName;
                        item.BusinessPhone = contactList.BusinessPhone;
                        item.Mobile = contactList.Mobile;
                        item.Fax = contactList.Fax;
                    }


                }
                #endregion

            }

            return customerDWLists;
        }

        public int GetCustomersDWCount(int tenant)
        {
            return repository.context.Customers.Where(a => a.Tenant == tenant && a.Card != null && (a.Card.PartnerTypeId == "CS" || a.Card.PartnerTypeId == "PO")).Count();
        }

        public int GetCustomersDWCountByUpdateDate(int tenant, DateTime updateDate)
        {
            return repository.context.Customers.Where(a => a.Tenant == tenant && a.Card != null && a.Card.UpdateDate > updateDate && (a.Card.PartnerTypeId == "CS" || a.Card.PartnerTypeId == "PO")).Count();
        }

        public List<CustomerList> GetCustomerFilters(byte[] xmlFilters, int tenant)
        {
            CustomerRepository CustomerRepository = new CustomerRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<CustomersDataView> customers = CustomerRepository.GetCustomersDataViews(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CustomerCustomFilter customfilters = new CustomerCustomFilter(tenant);
            customers = customfilters.GetFilteredQuery(queryOperations, customers);
            customers = customfilters.GetFreelancerCustomers(customers, tenant); // customs - freelancers

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            customers = myFilter.RunFilter(customers);

            customers = filter.GetFilteredQuery<CustomersDataView>(nonListQueryOperation, customers);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<CustomerList> query2 = GetIQueryableEntityList(customers);
            query2 = filter.GetFilteredQuery<CustomerList>(listQueryOperation, query2);

            IQueryable<CustomerList> bigQuery = query2;

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CustomerList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customer", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        bigQuery = sortClass.GetSorterQuery<CustomerList, string>(queryOperations, bigQuery);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    bigQuery = sortClass.GetSorterQuery<CustomerList, string>(queryOperations, bigQuery);
                                    break;
                                }
                            case "double":
                                {
                                    bigQuery = sortClass.GetSorterQuery<CustomerList, double>(queryOperations, bigQuery);
                                    break;
                                }
                            case "datetime":
                                {
                                    bigQuery = sortClass.GetSorterQuery<CustomerList, DateTime>(queryOperations, bigQuery);
                                    break;
                                }
                            case "integer":
                                {
                                    bigQuery = sortClass.GetSorterQuery<CustomerList, int>(queryOperations, bigQuery);
                                    break;
                                }
                            case "lookup":
                                {
                                    bigQuery = sortClass.GetSorterQuery<CustomerList, string>(queryOperations, bigQuery);
                                    break;
                                }
                            case "boolean":
                                {
                                    bigQuery = sortClass.GetSorterQuery<CustomerList, bool>(queryOperations, bigQuery);
                                    break;
                                }
                            default:
                                {
                                    bigQuery = bigQuery.OrderByDescending(d => d.Code);
                                    break;
                                }
                        }
                    }
                }
            }

            else
            {
                bigQuery = bigQuery.OrderByDescending(d => d.Code);
            }

            bigQuery = bigQuery.Skip(skippedPorts);
            bigQuery = bigQuery.Take(queryOperations.PageSize);


            List<CustomerList> listQuery = bigQuery.ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Customer", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public CustomerPM GetSinglePMForLogBox(string id, int tenant)
        {
            bool getFromCache = false;
            string entityName = "CustomerPM" + id + tenant;
            CustomerPM entity;

            if (HttpContext.Current != null && getFromCache)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Rank").Include("Card.SharedLogisticsInvitationStatus").Include("Collector.Contact").Include("Classifier.Contact").Include("Freelancer.Contact").Include("Forwarder").Include("CustomsAgent").Include("Mediator").Include("LeadSource").Include("CustomerStatus")
                              where a.Tenant == tenant && a.Id == id
                              select new CustomerPM()
                              {
                                  BillToId = a.BillToId,
                                  BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                  Id = a.Id,
                                  RankId = a.RankId,
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
                                  IsActiveForMobile = a.Card.IsActiveForMobile,
                                  LastLoginDate = a.Card.LastLoginDate,
                                  InvitationDate = a.Card.InvitationDate,
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
                                  PrimaryContactName = a.Card.PrimaryContact != null ? a.Card.PrimaryContact.EnglishName : null,
                                  PrimaryContactEmail = a.Card.PrimaryContact != null ? a.Card.PrimaryContact.Email : null,
                                  PrimaryContactPhone = a.Card.PrimaryContact != null ? (a.Card.PrimaryContact.BusinessPhone != null ? a.Card.PrimaryContact.BusinessPhone : null) : null,
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
                                  IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                  CreditLimitAmount = a.CreditLimitAmount,
                                  CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                                  CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                                  ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                                  PaymentMethodCode = a.Card.SATPaymentMethodCode,
                                  BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                                  BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                                  ExternalId2 = a.Card.ExternalId2,
                                  SATForeignRFC = a.Card.SATForeignRFC,
                                  MetodoPagoCode = a.Card.MetodoPagoCode,
                                  UsoCFDICode = a.Card.UsoCFDICode,
                                  CreatedByPartner = a.Card.CreatedByPartner,
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
                                  },

                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        this.SetCustomerAddressData(entity);

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }

                else
                {
                    entity = (CustomerPM)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            else
            {
                entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("AccountManagerUser.Contact").Include("Rank").Include("Card.SharedLogisticsInvitationStatus").Include("Collector.Contact").Include("Classifier.Contact").Include("Freelancer.Contact").Include("Forwarder").Include("CustomsAgent").Include("Mediator").Include("Card.PrimaryContact").Include("LeadSource").Include("CustomerStatus")
                          where a.Id == id && a.Tenant == tenant
                          select new CustomerPM()
                          {
                              BillToId = a.BillToId,
                              BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                              Id = a.Id,
                              RankId = a.RankId,
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
                              ImageDetailId = a.Card.ImageDetailId,
                              VatTypeId = a.Card.VatTypeId,
                              BankName = a.Card.BankName,
                              BankAddress = a.Card.BankAddress,
                              IBANNumber = a.Card.IBANNumber,
                              Swift = a.Card.Swift,
                              AccountNumber = a.Card.AccountNumber,
                              SharedLogisticsInvitationStatusName = a.Card.SharedLogisticsInvitationStatus != null ? a.Card.SharedLogisticsInvitationStatus.Name : null,
                              IsActiveForMobile = a.Card.IsActiveForMobile,
                              LastLoginDate = a.Card.LastLoginDate,
                              InvitationDate = a.Card.InvitationDate,
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
                              PrimaryContactName = a.Card.PrimaryContact != null ? a.Card.PrimaryContact.EnglishName : null,
                              PrimaryContactEmail = a.Card.PrimaryContact != null ? a.Card.PrimaryContact.Email : null,
                              PrimaryContactPhone = a.Card.PrimaryContact != null ? (a.Card.PrimaryContact.BusinessPhone != null ? a.Card.PrimaryContact.BusinessPhone : null) : null,
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
                              IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                              CreditLimitAmount = a.CreditLimitAmount,
                              CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                              CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                              ExternalAccountingBusinessArea = a.Card.ExternalAccountingBusinessArea,
                              PaymentMethodCode = a.Card.SATPaymentMethodCode,
                              BlockNewInvoiceCreation = a.BlockNewInvoiceCreation,
                              BlockNewShipmentCreation = a.BlockNewShipmentCreation,
                              ExternalId2 = a.Card.ExternalId2,
                              SATForeignRFC = a.Card.SATForeignRFC,
                              MetodoPagoCode = a.Card.MetodoPagoCode,
                              UsoCFDICode = a.Card.UsoCFDICode,
                              CreatedByPartner = a.Card.CreatedByPartner,
                              Card = new CardPM
                              {
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                                  EnglishName = a.Card.EnglishName,
                                  PrimaryContactId = a.Card.PrimaryContactId,
                                  ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                  PayablesAccountingCard = a.Card.PayablesAccountingCard,
                              },

                          }).FirstOrDefault();

                if (entity != null)
                {
                    this.SetCustomerAddressData(entity);
                }
            }

            if (entity != null)
            {
                CustomerProductRepository customerProductRepository = new CustomerProductRepository(repository.context);
                CustomerCompetitorRepository customerCompetitorRepository = new CustomerCompetitorRepository(repository.context);
                CustomerAdditionalServiceRepository customerAdditionalServiceRepository = new CustomerAdditionalServiceRepository(repository.context);
                CustomerSalesmanByProductRepository customerSalesmanByProductRepository = new CustomerSalesmanByProductRepository(repository.context);
                CustomerAccountManagerByProductRepository customerAccountManagerByProductRepository = new CustomerAccountManagerByProductRepository(repository.context);
                CustomerForwarderByProductRepository customerForwarderByProductRepository = new CustomerForwarderByProductRepository(repository.context);
                CustomerCustomsAgentByProductRepository customerCustomsAgentByProductRepository = new CustomerCustomsAgentByProductRepository(repository.context);
                CustomerMediatorByProductRepository customerMediatorByProductRepository = new CustomerMediatorByProductRepository(repository.context);
                CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(repository.context);

                CustomerProductQuery customerProductQuery = new CustomerProductQuery(customerProductRepository);
                CustomerCompetitorQuery customerCompetitorQuery = new CustomerCompetitorQuery(customerCompetitorRepository);
                CustomerAdditionalServiceQuery customerAdditionalServiceQuery = new CustomerAdditionalServiceQuery(customerAdditionalServiceRepository);
                CustomerSalesmanByProductQuery customerSalesmanByProductQuery = new CustomerSalesmanByProductQuery(customerSalesmanByProductRepository);
                CustomerAccountManagerByProductQuery customerAccountManagerByProductQuery = new CustomerAccountManagerByProductQuery(customerAccountManagerByProductRepository);
                CustomerForwarderByProductQuery customerForwarderByProductQuery = new CustomerForwarderByProductQuery(customerForwarderByProductRepository);
                CustomerCustomsAgentByProductQuery customerCustomsAgentByProductQuery = new CustomerCustomsAgentByProductQuery(customerCustomsAgentByProductRepository);
                CustomerMediatorByProductQuery customerMediatorByProductQuery = new CustomerMediatorByProductQuery(customerMediatorByProductRepository);
                CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery = new CardExternalCodeByCurrencyQuery(cardExternalCodeByCurrencyRepository);

                entity.CustomerProducts = customerProductQuery.GetCustomerProductPMsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerCompetitors = customerCompetitorQuery.GetCustomerCompetitorsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerAdditionalServices = customerAdditionalServiceQuery.GetCustomerAdditionalServicesByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerSalesmanByProducts = customerSalesmanByProductQuery.GetCustomerSalesmanByProductPMs(entity.Tenant, entity.Id);

                entity.CustomerAccountManagerByProducts = customerAccountManagerByProductQuery.GetCustomerAccountManagerByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerForwarderByProducts = customerForwarderByProductQuery.GetCustomerForwarderByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerCustomsAgentByProducts = customerCustomsAgentByProductQuery.GetCustomerCustomsAgentByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerMediatorByProducts = customerMediatorByProductQuery.GetCustomerMediatorByProductPMs(entity.Tenant, entity.Id);
                entity.CardExternalCodeByCurrencies = cardExternalCodeByCurrencyQuery.GetCardExternalCodeByCurrencyPMsForCustomer(entity.Id, entity.Tenant);

                CustomerSalesNoteRepository salesNoteRepository = new CustomerSalesNoteRepository(repository.context);
                CustomerSalesNoteQuery salesNoteQuery = new CustomerSalesNoteQuery(salesNoteRepository);
                entity.SalesNotes = salesNoteQuery.GetSalesNotesByCustomerId(entity.Id, entity.Tenant).ToList();

                entity.IsExternal = false;

                AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                if (accountingSystem != null)
                {
                    if (accountingSystem.IsExternalCodesFromTable)
                    {
                        entity.IsExternal = true;
                    }
                }

                if (!string.IsNullOrEmpty(entity.ReceivablesAccountingCard))
                {
                    int accountingCard = 0;
                    bool isParsed = Int32.TryParse(entity.ReceivablesAccountingCard, out accountingCard);

                    if (isParsed)
                    {
                        TenantManagement tenantManagement = null;

                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                            tenantManagement = tenantManagementRepository.GetSingleTenantManagement(accountingCard);

                            scope.Complete();
                        }

                        if (tenantManagement != null)
                        {
                            AirlineRepository airlineRepository = new AirlineRepository(repository.context);
                            List<Airline> tenantZeroAirlines = airlineRepository.GetAirlines(0).ToList();

                            if (tenantZeroAirlines.Count > 0)
                            {
                                if (tenantManagement.AWBMessagesCCSTypeCode == "GLSHK")
                                {
                                    tenantZeroAirlines = tenantZeroAirlines.Where(d => d.GLSHKPIMA != null).ToList();
                                }

                                else
                                {
                                    tenantZeroAirlines = tenantZeroAirlines.Where(d => d.TTY != null).ToList();
                                }

                                string requested = "";
                                string registered = "";
                                string pending = "";

                                List<Airline> currenctTenantAilines = airlineRepository.GetAirlines(tenantManagement.Id).ToList();
                                foreach (Airline tenantZeroItem in tenantZeroAirlines)
                                {
                                    Airline myTenantItem = currenctTenantAilines.Where(d => d.Card.Code == tenantZeroItem.Card.Code).FirstOrDefault();

                                    if (myTenantItem != null)
                                    {
                                        if (tenantManagement.AWBMessagesCCSTypeCode == "GLSHK")
                                        {
                                            if (myTenantItem.GLSHKRegistrationRequested)
                                            {
                                                if (string.IsNullOrEmpty(requested))
                                                {
                                                    requested = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    requested = requested + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.IsGLSHKRegistered)
                                            {
                                                if (string.IsNullOrEmpty(registered))
                                                {
                                                    registered = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    registered = registered + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.GLSHKRegistrationRequested && !myTenantItem.IsGLSHKRegistered)
                                            {
                                                if (string.IsNullOrEmpty(pending))
                                                {
                                                    pending = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    pending = pending + ", " + myTenantItem.Card.Code;
                                                }
                                            }
                                        }

                                        else
                                        {
                                            if (myTenantItem.ChampRegistrationRequested)
                                            {
                                                if (string.IsNullOrEmpty(requested))
                                                {
                                                    requested = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    requested = requested + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.IsChampRegistered)
                                            {
                                                if (string.IsNullOrEmpty(registered))
                                                {
                                                    registered = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    registered = registered + ", " + myTenantItem.Card.Code;
                                                }
                                            }

                                            if (myTenantItem.ChampRegistrationRequested && !myTenantItem.IsChampRegistered)
                                            {
                                                if (string.IsNullOrEmpty(pending))
                                                {
                                                    pending = myTenantItem.Card.Code;
                                                }

                                                else
                                                {
                                                    pending = pending + ", " + myTenantItem.Card.Code;
                                                }
                                            }
                                        }
                                    }
                                }

                                entity.RequestedAirlines = requested;
                                entity.RegisteredAirlines = registered;
                                entity.PendingAirlines = pending;
                            }
                        }
                    }
                }

                CustomerPM securedPm = new CustomerPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Customer", tenant);

                if (securedPm != null && entity != null)
                {
                    Customer entityPOC = (from s in repository.context.Customers where s.Id == securedPm.Id select s).FirstOrDefault();

                    securedPm.Field1 = new CustomFieldClass("Field1", "Customer", entityPOC.Field1);
                    securedPm.Field2 = new CustomFieldClass("Field2", "Customer", entityPOC.Field2);
                    securedPm.Field3 = new CustomFieldClass("Field3", "Customer", entityPOC.Field3);
                    securedPm.Field4 = new CustomFieldClass("Field4", "Customer", entityPOC.Field4);
                    securedPm.Field5 = new CustomFieldClass("Field5", "Customer", entityPOC.Field5);
                    securedPm.Field6 = new CustomFieldClass("Field6", "Customer", entityPOC.Field6);
                    securedPm.Field7 = new CustomFieldClass("Field7", "Customer", entityPOC.Field7);
                    securedPm.Field8 = new CustomFieldClass("Field8", "Customer", entityPOC.Field8);
                    securedPm.Field9 = new CustomFieldClass("Field9", "Customer", entityPOC.Field9);
                    securedPm.Field10 = new CustomFieldClass("Field10", "Customer", entityPOC.Field10);
                }
                //if (securedPm != null)
                //{
                //    CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
                //    securedPm.IsCustomerAllowed = myFilter.IsCustomerAllowed(securedPm);
                //}
                return securedPm;
            }

            else
            {
                return null;
            }
        }

        public CustomerPM GetSingleCustomerPMByCode(string code, int tenant)
        {
            CustomerPM entity = (from a in repository.context.Customers.Include("Card").Include("BillToCard").Include("Card.SharedLogisticsInvitationStatus").Include("LeadSource").Include("CustomerStatus")
                                 where a.Card.Code == code && a.Tenant == tenant
                                 select new CustomerPM()
                                 {
                                     BillToId = a.BillToId,
                                     BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                     Id = a.Id,
                                     RankId = a.RankId,
                                     AccountManagerUserId = a.AccountManagerUserId,
                                     SalesmanUserId = a.SalesmanUserId,
                                     Tenant = a.Tenant,
                                     Website = a.Card.Website,
                                     Code = a.Card.Code,
                                     LocalName = a.Card.LocalName,
                                     EnglishName = a.Card.EnglishName,
                                     CardPMId = a.Id,
                                     ReceivablesAccountingCard = a.Card.ReceivablesAccountingCard,
                                     PayablesAccountingCard = a.Card.PayablesAccountingCard,
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
                                     IsActiveForMobile = a.Card.IsActiveForMobile,
                                     LastLoginDate = a.Card.LastLoginDate,
                                     InvitationDate = a.Card.InvitationDate,
                                     LeadSourceId = a.LeadSourceId,
                                     IndustryId = a.IndustryId,
                                     ClassifierId = a.ClassifierId,
                                     CollectorId = a.CollectorId,
                                     LeadSourceName = a.LeadSource != null ? a.LeadSource.Name : null,
                                     IndustryName = a.Industry != null ? a.Industry.Name : null,
                                     CreditLimit = a.CreditLimit,
                                     LeadDescription = a.LeadDescription,
                                     IsCustomer = a.IsCustomer,
                                     FreelancerId = a.FreelancerId,
                                     CustomerStatusCode = a.CustomerStatusCode,
                                     ForwarderId = a.ForwarderId,
                                     CustomsAgentId = a.CustomsAgentId,
                                     MediatorId = a.MediatorId,
                                     BeforeDeactiveStatusCode = a.BeforeDeactiveStatusCode,
                                     CodeMyCustomer = a.IsCustomer ? a.Card.Code + " (Customer)" : a.Card.Code,
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
                                     IsCreditLimitEnabled = a.IsCreditLimitEnabled,
                                     CreditLimitAmount = a.CreditLimitAmount,
                                     CreditLimitOpenBalance = a.CreditLimitOpenBalance,
                                     CreditLimitWarningPercentage = a.CreditLimitWarningPercentage,
                                     ExternalId2 = a.Card.ExternalId2,
                                     SATForeignRFC = a.Card.SATForeignRFC,
                                     MetodoPagoCode = a.Card.MetodoPagoCode,
                                     UsoCFDICode = a.Card.UsoCFDICode,
                                     CreatedByPartner = a.Card.CreatedByPartner,
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
                                     },

                                 }).FirstOrDefault();

            if (entity != null)
            {
                this.SetCustomerAddressData(entity);
            }

            if (entity != null)
            {
                CustomerProductRepository customerProductRepository = new CustomerProductRepository(repository.context);
                CustomerCompetitorRepository customerCompetitorRepository = new CustomerCompetitorRepository(repository.context);
                CustomerAdditionalServiceRepository customerAdditionalServiceRepository = new CustomerAdditionalServiceRepository(repository.context);
                CustomerSalesmanByProductRepository customerSalesmanByProductRepository = new CustomerSalesmanByProductRepository(repository.context);
                CustomerAccountManagerByProductRepository customerAccountManagerByProductRepository = new CustomerAccountManagerByProductRepository(repository.context);
                CustomerForwarderByProductRepository customerForwarderByProductRepository = new CustomerForwarderByProductRepository(repository.context);
                CustomerCustomsAgentByProductRepository customerCustomsAgentByProductRepository = new CustomerCustomsAgentByProductRepository(repository.context);
                CustomerMediatorByProductRepository customerMediatorByProductRepository = new CustomerMediatorByProductRepository(repository.context);

                CustomerProductQuery customerProductQuery = new CustomerProductQuery(customerProductRepository);
                CustomerCompetitorQuery customerCompetitorQuery = new CustomerCompetitorQuery(customerCompetitorRepository);
                CustomerAdditionalServiceQuery customerAdditionalServiceQuery = new CustomerAdditionalServiceQuery(customerAdditionalServiceRepository);
                CustomerSalesmanByProductQuery customerSalesmanByProductQuery = new CustomerSalesmanByProductQuery(customerSalesmanByProductRepository);
                CustomerAccountManagerByProductQuery customerAccountManagerByProductQuery = new CustomerAccountManagerByProductQuery(customerAccountManagerByProductRepository);
                CustomerForwarderByProductQuery customerForwarderByProductQuery = new CustomerForwarderByProductQuery(customerForwarderByProductRepository);
                CustomerCustomsAgentByProductQuery customerCustomsAgentByProductQuery = new CustomerCustomsAgentByProductQuery(customerCustomsAgentByProductRepository);
                CustomerMediatorByProductQuery customerMediatorByProductQuery = new CustomerMediatorByProductQuery(customerMediatorByProductRepository);

                entity.CustomerProducts = customerProductQuery.GetCustomerProductPMsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerCompetitors = customerCompetitorQuery.GetCustomerCompetitorsByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerAdditionalServices = customerAdditionalServiceQuery.GetCustomerAdditionalServicesByCustomerId(entity.Id, entity.Tenant).ToList();
                entity.CustomerSalesmanByProducts = customerSalesmanByProductQuery.GetCustomerSalesmanByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerAccountManagerByProducts = customerAccountManagerByProductQuery.GetCustomerAccountManagerByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerForwarderByProducts = customerForwarderByProductQuery.GetCustomerForwarderByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerCustomsAgentByProducts = customerCustomsAgentByProductQuery.GetCustomerCustomsAgentByProductPMs(entity.Tenant, entity.Id);
                entity.CustomerMediatorByProducts = customerMediatorByProductQuery.GetCustomerMediatorByProductPMs(entity.Tenant, entity.Id);

                CustomerSalesNoteRepository salesNoteRepository = new CustomerSalesNoteRepository(repository.context);
                CustomerSalesNoteQuery salesNoteQuery = new CustomerSalesNoteQuery(salesNoteRepository);
                entity.SalesNotes = salesNoteQuery.GetSalesNotesByCustomerId(entity.Id, entity.Tenant).ToList();
                if (entity != null)
                {
                    entity.IsExternal = false;

                    AccountingSystemHelper accountingSystemHelper = new AccountingSystemHelper();
                    AccountingSystemPM accountingSystem = accountingSystemHelper.GetAccountingSystem(tenant);
                    if (accountingSystem != null)
                    {
                        if (accountingSystem.IsExternalCodesFromTable)
                        {
                            entity.IsExternal = true;
                        }
                    }
                }

                CustomerPM securedPm = new CustomerPM();
                SecuredMapping.GetMappedPM(entity, securedPm, "Customer", tenant);

                if (securedPm != null && entity != null)
                {
                    Customer entityPOC = (from s in repository.context.Customers
                                          where s.Id == securedPm.Id
                                          select s).FirstOrDefault();

                    securedPm.Field1 = new CustomFieldClass("Field1", "Customer", entityPOC.Field1);
                    securedPm.Field2 = new CustomFieldClass("Field2", "Customer", entityPOC.Field2);
                    securedPm.Field3 = new CustomFieldClass("Field3", "Customer", entityPOC.Field3);
                    securedPm.Field4 = new CustomFieldClass("Field4", "Customer", entityPOC.Field4);
                    securedPm.Field5 = new CustomFieldClass("Field5", "Customer", entityPOC.Field5);
                    securedPm.Field6 = new CustomFieldClass("Field6", "Customer", entityPOC.Field6);
                    securedPm.Field7 = new CustomFieldClass("Field7", "Customer", entityPOC.Field7);
                    securedPm.Field8 = new CustomFieldClass("Field8", "Customer", entityPOC.Field8);
                    securedPm.Field9 = new CustomFieldClass("Field9", "Customer", entityPOC.Field9);
                    securedPm.Field10 = new CustomFieldClass("Field10", "Customer", entityPOC.Field10);
                }

                return securedPm;
            }

            return entity;
        }
    }
}