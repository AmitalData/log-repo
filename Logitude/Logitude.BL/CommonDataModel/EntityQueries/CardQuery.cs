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
using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using Logitude.Server.Tools.Counters;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Azure;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;
using System.IO;
using Logitude.BL.DataContracts;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CardQuery
    {
        CardRepository repository;

        public CardQuery()
        {
            repository = new CardRepository();
        }

        public CardQuery(int tenant)
        {
            repository = new CardRepository(tenant);
        }

        public CardQuery(CardRepository cardRepository)
        {
            repository = cardRepository;
        }

        public IQueryable<CardList> GetCardWithAddresses(int tenant)
        {
            IQueryable<CardList> myResult = (from card in repository.context.Cards.Include("Customer").Include("Customer.SalesmanUser").Include("PaymentTerm").Include("PartnerType").Include("SharedLogisticsInvitationStatus").Include("Customer.AccountManagerUser").Include("Customer.AccountManagerUser.Contact")
                                             join c in repository.context.CardContacts on card.Id equals c.CardId into cardCardcontactJoin
                                             from co in cardCardcontactJoin.DefaultIfEmpty()
                                             where card.Tenant == tenant
                                             select new CardList()
                                             {
                                                 Code = card.Code,
                                                 CreateDate = card.CreateDate,
                                                 EnglishName = card.EnglishName,
                                                 LocalName = card.LocalName,
                                                 ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                                 PayablesAccountingCard = card.PayablesAccountingCard,
                                                 AccountingVATSplit = card.AccountingVATSplit,
                                                 InActive = card.InActive,
                                                 Notes = card.Notes,
                                                 Id = card.Id,
                                                 Tenant = card.Tenant,
                                                 VatNumber = card.VatNumber,
                                                 PaymentTermId = card.PaymentTermId,
                                                 PartnerTypeId = card.PartnerTypeId,
                                                 PartnerTypeName = card.PartnerType == null ? null : card.PartnerType.Name,
                                                 PaymentTermName = card.PaymentTerm == null ? null : card.PaymentTerm.EnglishName,
                                                 WebSite = card.Website,
                                                 InvoiceCurrencyId = card.InvoiceCurrencyId,
                                                 VatTypeId = card.VatTypeId,
                                                 SearchFields = card.SearchFields,
                                                 ContactId = co.ContactId,
                                                 AccountNumber = card.AccountNumber,
                                                 BankName = card.BankName,
                                                 BankAddress = card.BankAddress,
                                                 Swift = card.Swift,
                                                 IBANNumber = card.IBANNumber,
                                                 InvitationDate = card.InvitationDate,
                                                 SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                                                 SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                                 PrimaryContactId = card.PrimaryContactId,
                                                 EnableConsolidationInvoices = card.EnableConsolidationInvoices,
                                                 CityName = card.CityName,
                                                 CountryId = card.CountryId,
                                                 CountryCode = card.CountryCode,
                                                 CountryName = card.CountryName,
                                                 SalesmanUserId = card.Customer == null ? null : card.Customer.SalesmanUserId,
                                                 SalesmanBusinessUnitId = card.Customer == null ? null : (card.Customer.SalesmanUser == null ? null : card.Customer.SalesmanUser.BusinessUnitId),
                                                 AccountManagerUserName = card.Customer == null ? null : (card.Customer.AccountManagerUser == null ? null : (card.Customer.AccountManagerUser.Contact.EnglishName)),
                                                 AccountManagerUserId = card.Customer == null ? null : card.Customer.AccountManagerUserId,
                                                 CASSCode = card.Agent == null ? null : card.Agent.CASSCode,
                                                 IATACode = card.Agent == null ? null : card.Agent.IATACode,
                                                 RegulatedAgentCode = card.Agent == null ? null : card.Agent.RegulatedAgentCode,
                                                 IsActiveForMobile = card.IsActiveForMobile,
                                                 KnownConsignor = card.Customer == null ? null : card.Customer.KnownConsignor,
                                                 KCExpirationDate = card.Customer == null ? null : card.Customer.KCExpirationDate,
                                                 IRSPlace = card.IRSPlace,
                                                 IRSNumber = card.IRSNumber,
                                                 SupportNotes = card.SupportNotes,
                                                 GLAccountId = card.GLAccountId,
                                                 ExternalAccountingBusinessArea = card.ExternalAccountingBusinessArea,
                                                 SATPaymentMethodCode = card.SATPaymentMethodCode,
                                                 SATForeignRFC = card.SATForeignRFC,
                                                 MetodoPagoCode = card.MetodoPagoCode,
                                                 UsoCFDICode = card.UsoCFDICode,
                                                 StateName = card.StateName,
                                                 IsInternationalPartner = card.IsInternationalPartner,
                                                 IsAutonomy = card.IsAutonomy,
                                                 CalculatedEnglishName = string.IsNullOrEmpty(card.EnglishName) ? card.LocalName : card.EnglishName,
                                                 CalculatedLocalName = string.IsNullOrEmpty(card.LocalName) ? card.EnglishName : card.LocalName,
                                                 CreatedByPartner = card.CreatedByPartner,
                                             });


            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            myResult = myFilter.RunFilter(myResult);

            return myResult;
        }

        public CardPM GetSinglePMFromCache(string id, int tenant)
        {
            string entityKeyString = $"GetSinglePMFromCache({id},{tenant})";
            var res=CacheManager.GetOrInsertNewObject<CardPM>(entityKeyString, () =>
             {
                 return this.GetSinglePM(id, tenant);
             });
            return res;
        }

        public CardPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "CardPM" + id + tenant;
                CardPM entity;

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressPM myMainAddresss = addressQuery.GetAddressPMByTypeAndCard(id, "M", tenant);
                string myMainAddressId = null;
                if (myMainAddresss != null)
                {
                    myMainAddressId = myMainAddresss.Id;
                }

                AddressPM myBillingAddress = addressQuery.GetAddressPMByTypeAndCard(id, "B", tenant);
                string myBillingAddressId = null;
                if (myBillingAddress != null)
                {
                    myBillingAddressId = myBillingAddress.Id;
                }

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from a in repository.context.Cards.Include("Customer").Include("Customer.SalesmanUser").Include("PartnerType").Include("SharedLogisticsInvitationStatus")
                                  join airline in repository.context.Airlines on a.Id equals airline.Id into airlineJoin
                                  from al in airlineJoin.DefaultIfEmpty()
                                  where a.Id == id

                                  select new CardPM()
                                  {
                                      ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                      AccountingVATSplit = a.AccountingVATSplit,
                                      PayablesAccountingCard = a.PayablesAccountingCard,
                                      EnglishName = a.EnglishName,
                                      Id = a.Id,
                                      InActive = a.InActive,
                                      LocalName = a.LocalName,
                                      Notes = a.Notes,
                                      PartnerTypeId = a.PartnerTypeId,
                                      PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                      PaymentTermId = a.PaymentTermId,
                                      Tenant = a.Tenant,
                                      VatNumber = a.VatNumber,
                                      Code = a.Code,
                                      MainAddressId = myMainAddressId,
                                      BillingAddressId = myBillingAddressId,
                                      CountryId = a.CountryId,
                                      CountryCode = a.CountryCode,
                                      CountryName = a.CountryName,
                                      Website = a.Website,
                                      ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                      InvoiceCurrencyId = a.InvoiceCurrencyId,
                                      VatTypeId = a.VatTypeId,
                                      SearchFields = a.SearchFields,
                                      Prefix = al.Prefix,
                                      ImageDetailId = a.ImageDetailId,
                                      AccountNumber = a.AccountNumber,
                                      BankName = a.BankName,
                                      BankAddress = a.BankAddress,
                                      Swift = a.Swift,
                                      IBANNumber = a.IBANNumber,
                                      InvitationDate = a.InvitationDate,
                                      SharedLogisticsInvitationStatusCode = a.SharedLogisticsInvitationStatusCode,
                                      SharedLogisticsInvitationStatusName = a.SharedLogisticsInvitationStatus != null ? a.SharedLogisticsInvitationStatus.Name : null,
                                      LastLoginDate = a.LastLoginDate,
                                      CollectorId = a.CollectorId,
                                      ClassifierId = a.ClassifierId,
                                      ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                      CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                      CreateDate = a.CreateDate,
                                      UpdateDate = a.UpdateDate,
                                      CreatedByUserId = a.CreatedByUserId,
                                      UpdatedByUserId = a.UpdatedByUserId,
                                      PrimaryContactId = a.PrimaryContactId,
                                      EnableConsolidationInvoices = a.EnableConsolidationInvoices,
                                      SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                      AccountManagerUserId = a.Customer == null ? null : a.Customer.AccountManagerUserId,
                                      SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                      IsActiveForMobile = a.IsActiveForMobile,
                                      IsCustomer = a.IsCustomer,
                                      CityName = a.CityName,
                                      ContactId = a.PrimaryContactId,
                                      IRSPlace = a.IRSPlace,
                                      IRSNumber = a.IRSNumber,
                                      SupportNotes = a.SupportNotes,
                                      GLAccountId = a.GLAccountId,
                                      ExternalAccountingBusinessArea = a.ExternalAccountingBusinessArea,
                                      SATPaymentMethodCode = a.SATPaymentMethodCode,
                                      SATForeignRFC = a.SATForeignRFC,
                                      MetodoPagoCode = a.MetodoPagoCode,
                                      UsoCFDICode = a.UsoCFDICode,
                                      StateName = a.StateName,
                                      CustomerStatusCode = a.Customer != null ? (a.Customer.CustomerStatus != null ? a.Customer.CustomerStatus.Code : null) : null,
                                      IsInternationalPartner = a.IsInternationalPartner,
                                      IsAutonomy = a.IsAutonomy,
                                      CreatedByPartner = a.CreatedByPartner,
                                      StorageFreeDays = a.StorageFreeDays,
                                      RankId = a.Customer != null ? (a.Customer.Rank != null ? a.Customer.Rank.Id : null) : null,
                                      IndustryId = a.Customer != null ? (a.Customer.Industry != null ? a.Customer.Industry.Id : null) : null,
                                  }).FirstOrDefault();


                        if (entity != null)
                        {
                            entity.Addresses = addressQuery.GetAddressesByCardId(id, tenant);

                            string name = "CardPM" + entity.Id + tenant;

                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }

                    else
                    {
                        entity = (CardPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }

                else
                {
                    entity = (from a in repository.context.Cards.Include("Customer").Include("Customer.SalesmanUser").Include("PartnerType").Include("SharedLogisticsInvitationStatus")
                              join airline in repository.context.Airlines on a.Id equals airline.Id into airlineJoin
                              from al in airlineJoin.DefaultIfEmpty()
                              where a.Id == id
                              select new CardPM()
                              {
                                  ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                  AccountingVATSplit = a.AccountingVATSplit,
                                  PayablesAccountingCard = a.PayablesAccountingCard,
                                  EnglishName = a.EnglishName,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  LocalName = a.LocalName,
                                  Notes = a.Notes,
                                  PartnerTypeId = a.PartnerTypeId,
                                  PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                  PaymentTermId = a.PaymentTermId,
                                  Tenant = a.Tenant,
                                  VatNumber = a.VatNumber,
                                  Code = a.Code,
                                  MainAddressId = myMainAddressId,
                                  BillingAddressId = myBillingAddressId,
                                  CountryId = a.CountryId,
                                  CountryCode = a.CountryCode,
                                  CountryName = a.CountryName,
                                  Website = a.Website,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  InvoiceCurrencyId = a.InvoiceCurrencyId,
                                  VatTypeId = a.VatTypeId,
                                  SearchFields = a.SearchFields,
                                  Prefix = al.Prefix,
                                  ImageDetailId = a.ImageDetailId,
                                  AccountNumber = a.AccountNumber,
                                  BankName = a.BankName,
                                  BankAddress = a.BankAddress,
                                  Swift = a.Swift,
                                  IBANNumber = a.IBANNumber,
                                  InvitationDate = a.InvitationDate,
                                  SharedLogisticsInvitationStatusCode = a.SharedLogisticsInvitationStatusCode,
                                  SharedLogisticsInvitationStatusName = a.SharedLogisticsInvitationStatus != null ? a.SharedLogisticsInvitationStatus.Name : null,
                                  LastLoginDate = a.LastLoginDate,
                                  CollectorId = a.CollectorId,
                                  ClassifierId = a.ClassifierId,
                                  ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                  CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                  CreateDate = a.CreateDate,
                                  UpdateDate = a.UpdateDate,
                                  CreatedByUserId = a.CreatedByUserId,
                                  UpdatedByUserId = a.UpdatedByUserId,
                                  PrimaryContactId = a.PrimaryContactId,
                                  EnableConsolidationInvoices = a.EnableConsolidationInvoices,
                                  SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                  AccountManagerUserId = a.Customer == null ? null : a.Customer.AccountManagerUserId,
                                  SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                  IsActiveForMobile = a.IsActiveForMobile,
                                  IRSPlace = a.IRSPlace,
                                  IRSNumber = a.IRSNumber,
                                  SupportNotes = a.SupportNotes,
                                  GLAccountId = a.GLAccountId,
                                  ExternalAccountingBusinessArea = a.ExternalAccountingBusinessArea,
                                  SATPaymentMethodCode = a.SATPaymentMethodCode,
                                  SATForeignRFC = a.SATForeignRFC,
                                  MetodoPagoCode = a.MetodoPagoCode,
                                  UsoCFDICode = a.UsoCFDICode,
                                  StateName = a.StateName,
                                  IsInternationalPartner = a.IsInternationalPartner,
                                  IsAutonomy = a.IsAutonomy,
                                  CreatedByPartner = a.CreatedByPartner,
                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        entity.Addresses = addressQuery.GetAddressesByCardId(id, tenant);
                    }
                }

                return entity;
            }

            return null;
        }

        public List<CardList> GetCardListsByListIds(List<string> cardIds, int tenant)
        {
            List<CardList> cardLists = (from a in repository.context.Cards
                                        where cardIds.Contains(a.Id) && a.Tenant == tenant
                                        select new CardList()
                                        {
                                            Id = a.Id,
                                            Tenant = a.Tenant,
                                            EnglishName = a.EnglishName,

                                        }).ToList();
            return cardLists;
        }

        public CardPM GetSinglePMByCode(string code, int tenant)
        {
            var cardId = repository.GetCardIdByCode(code,tenant);
            return GetSinglePM(cardId, tenant);

        }

        public CardList GetSingleByGLAccount(string glAccountId, int tenant, bool fromCache)
        {

            string entityKeyString= $"GetSingleByGLAccount({glAccountId},{tenant})";
            CardList cardList = CacheManager.GetOrInsertNewObject<CardList>(entityKeyString,
                ()=>
                {
                    return JustGetSingleByGLAccount(glAccountId, tenant);
                }, supressForceInsert:fromCache);
            return cardList;
        }

        private CardList JustGetSingleByGLAccount(string glAccountId, int tenant)
        {
            return (from a in repository.context.Cards
                    where a.GLAccountId == glAccountId && a.Tenant == tenant
                    select new CardList()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        EnglishName = a.EnglishName,
                        SalesmanUserId = a.SalesmanUserId,
                        CollectorId = a.CollectorId,

                    }).FirstOrDefault();
        }

        public List<CardList> GetAllCardsByGLAccount(string glAccountId, int tenant)
        {
            return (from a in repository.context.Cards
                    where a.GLAccountId == glAccountId && a.Tenant == tenant
                    select new CardList()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        EnglishName = a.EnglishName,
                        SalesmanUserId = a.SalesmanUserId,
                        CollectorId = a.CollectorId,

                    }).ToList();
        }

        public IQueryable<CardList> GetCardListsByTenant(int tenant)
        {
            AddressRepository addressRepository = new AddressRepository(tenant);
            AddressQuery addressQuery = new AddressQuery(tenant);
            IQueryable<CardList> cards = from a in repository.context.Cards
                                         where a.Tenant == tenant
                                         select new CardList()
                                         {
                                             Id = a.Id,
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             VatNumber = a.VatNumber,
                                             CountryCode = a.CountryCode,
                                             CountryName = a.CountryName,
                                             CityName = a.CityName,
                                             GLAccountId = a.GLAccountId
                                         };

            return cards;
        }

        public IQueryable<CardList> GetCardPMsByTenant(int tenant)
        {
            AddressRepository addressRepository = new AddressRepository(tenant);
            AddressQuery addressQuery = new AddressQuery(tenant);
            IQueryable<CardList> cards = from a in repository.context.Cards
                                         where a.Tenant == tenant
                                         select new CardList()
                                         {
                                             Id = a.Id,
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             VatNumber = a.VatNumber,
                                             CountryCode = a.CountryCode,
                                             CountryName = a.CountryName,
                                             CityName = a.CityName,
                                             GLAccountId = a.GLAccountId
                                         };

            return cards;
        }


        public List<string> GetCardIdsByTenant(int tenant)
        {

            List<string> cards = (from a in repository.context.Cards
                                  where a.Tenant == tenant
                                  select a.Id).ToList();

            return cards;
        }


        public IQueryable<CardList> GetCustomerCardPMsByTenant(int tenant)
        {
            IQueryable<CardList> cards = (from a in repository.context.Cards.Include("Customer").Include("Customer.SalesmanUser").Include("Agent").Include("PartnerType").Include("SharedLogisticsInvitationStatus")
                                          where a.Tenant == tenant
                                          && a.InActive == false
                                          && (a.PartnerTypeId == "CS" || a.PartnerTypeId == "PO" || a.PartnerTypeId == "AG")
                                          select new CardList()
                                          {
                                              ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                              AccountingVATSplit = a.AccountingVATSplit,
                                              PayablesAccountingCard = a.PayablesAccountingCard,
                                              EnglishName = a.EnglishName,
                                              Id = a.Id,
                                              InActive = a.InActive,
                                              LocalName = a.LocalName,
                                              Notes = a.Notes,
                                              PartnerTypeId = a.PartnerTypeId,
                                              PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                              PaymentTermId = a.PaymentTermId,
                                              Tenant = a.Tenant,
                                              VatNumber = a.VatNumber,
                                              Code = a.Code,
                                              SearchFields = a.SearchFields,
                                              //Website = a.Website,
                                              //// ImageDetailId = a.ImageDetailId,
                                              AccountNumber = a.AccountNumber,
                                              BankName = a.BankName,
                                              BankAddress = a.BankAddress,
                                              Swift = a.Swift,
                                              IBANNumber = a.IBANNumber,
                                              InvitationDate = a.InvitationDate,
                                              SharedLogisticsInvitationStatusCode = a.SharedLogisticsInvitationStatusCode,
                                              SharedLogisticsInvitationStatusName = a.SharedLogisticsInvitationStatus != null ? a.SharedLogisticsInvitationStatus.Name : null,
                                              LastLoginDate = a.LastLoginDate,
                                              // CollectorId = a.CollectorId,
                                              // ClassifierId = a.ClassifierId,
                                              //ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                              //CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                              CreateDate = a.CreateDate,
                                              UpdateDate = a.UpdateDate,
                                              CreatedByUserId = a.CreatedByUserId,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              PrimaryContactId = a.PrimaryContactId,
                                              EnableConsolidationInvoices = a.EnableConsolidationInvoices,
                                              CountryId = a.CountryId,
                                              CountryCode = a.CountryCode,
                                              CountryName = a.CountryName,
                                              IsCustomer = a.IsCustomer,
                                              SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                              SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                              IsActiveForMobile = a.IsActiveForMobile,
                                              IRSPlace = a.IRSPlace,
                                              IRSNumber = a.IRSNumber,
                                              SupportNotes = a.SupportNotes,
                                              GLAccountId = a.GLAccountId,
                                              ExternalAccountingBusinessArea = a.ExternalAccountingBusinessArea,
                                              SATPaymentMethodCode = a.SATPaymentMethodCode,
                                              SATForeignRFC = a.SATForeignRFC,
                                              MetodoPagoCode = a.MetodoPagoCode,
                                              UsoCFDICode = a.UsoCFDICode,
                                              StateName = a.StateName,
                                              IsInternationalPartner = a.IsInternationalPartner,
                                              IsAutonomy = a.IsAutonomy,
                                              CreatedByPartner = a.CreatedByPartner,
                                          });
            return cards;
        }

        public IQueryable<CardList> GetCustomerCardListByTenant(int tenant)
        {
            IQueryable<CardList> cards = (from a in repository.context.Cards.Include("Customer").Include("Customer.SalesmanUser").Include("PartnerType").Include("SharedLogisticsInvitationStatus")
                                          where a.Tenant == tenant
                                          && a.InActive == false
                                          && (a.PartnerTypeId == "CS")
                                          select new CardList()
                                          {
                                              ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                              AccountingVATSplit = a.AccountingVATSplit,
                                              PayablesAccountingCard = a.PayablesAccountingCard,
                                              EnglishName = a.EnglishName,
                                              Id = a.Id,
                                              InActive = a.InActive,
                                              LocalName = a.LocalName,
                                              Notes = a.Notes,
                                              PartnerTypeId = a.PartnerTypeId,
                                              PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                              PaymentTermId = a.PaymentTermId,
                                              Tenant = a.Tenant,
                                              VatNumber = a.VatNumber,
                                              Code = a.Code,
                                              SearchFields = a.SearchFields,
                                              //Website = a.Website,
                                              //// ImageDetailId = a.ImageDetailId,
                                              AccountNumber = a.AccountNumber,
                                              BankName = a.BankName,
                                              BankAddress = a.BankAddress,
                                              Swift = a.Swift,
                                              IBANNumber = a.IBANNumber,
                                              InvitationDate = a.InvitationDate,
                                              SharedLogisticsInvitationStatusCode = a.SharedLogisticsInvitationStatusCode,
                                              SharedLogisticsInvitationStatusName = a.SharedLogisticsInvitationStatus != null ? a.SharedLogisticsInvitationStatus.Name : null,
                                              LastLoginDate = a.LastLoginDate,
                                              // CollectorId = a.CollectorId,
                                              // ClassifierId = a.ClassifierId,
                                              //ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                              //CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                              CreateDate = a.CreateDate,
                                              UpdateDate = a.UpdateDate,
                                              CreatedByUserId = a.CreatedByUserId,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              PrimaryContactId = a.PrimaryContactId,
                                              EnableConsolidationInvoices = a.EnableConsolidationInvoices,
                                              CountryId = a.CountryId,
                                              CountryCode = a.CountryCode,
                                              CountryName = a.CountryName,
                                              IsCustomer = a.IsCustomer,
                                              SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                              SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                              IsActiveForMobile = a.IsActiveForMobile,
                                              IRSPlace = a.IRSPlace,
                                              IRSNumber = a.IRSNumber,
                                              SupportNotes = a.SupportNotes,
                                              GLAccountId = a.GLAccountId,
                                              ExternalAccountingBusinessArea = a.ExternalAccountingBusinessArea,
                                              SATPaymentMethodCode = a.SATPaymentMethodCode,
                                              SATForeignRFC = a.SATForeignRFC,
                                              MetodoPagoCode = a.MetodoPagoCode,
                                              UsoCFDICode = a.UsoCFDICode,
                                              StateName = a.StateName,
                                              IsInternationalPartner = a.IsInternationalPartner,
                                              IsAutonomy = a.IsAutonomy,
                                              CreatedByPartner = a.CreatedByPartner,
                                          });
            return cards;
        }

        public CardPM GetSingleCarrierCard(string id, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(id))
            {
                CardRepository myRepository = new CardRepository(tenant);

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressPM myMainAddresss = addressQuery.GetAddressPMByTypeAndCard(id, "M", tenant);
                string myMainAddressId = null;
                if (myMainAddresss != null)
                {
                    myMainAddressId = myMainAddresss.Id;
                }

                string entityName = "CardPM" + id + tenant;
                CardPM entity;

                if (getFromCache)
                {
                    if (HttpContext.Current != null)
                    {
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            entity = (from a in myRepository.context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus")
                                      where a.Tenant == tenant && a.Id == id
                                      && (a.PartnerTypeId == "AL" || a.PartnerTypeId == "SL" || a.PartnerTypeId == "TR")
                                      select new CardPM()
                                      {
                                          ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                          AccountingVATSplit = a.AccountingVATSplit,
                                          PayablesAccountingCard = a.PayablesAccountingCard,
                                          EnglishName = a.EnglishName,
                                          Id = a.Id,
                                          InActive = a.InActive,
                                          LocalName = a.LocalName,
                                          Notes = a.Notes,
                                          PartnerTypeId = a.PartnerTypeId,
                                          PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                          PaymentTermId = a.PaymentTermId,
                                          Tenant = a.Tenant,
                                          VatNumber = a.VatNumber,
                                          Code = a.Code,
                                          MainAddressId = myMainAddressId,
                                          CountryId = a.CountryId,
                                          CountryCode = a.CountryCode,
                                          CountryName = a.CountryName,
                                          Website = a.Website,
                                          ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                          InvoiceCurrencyId = a.InvoiceCurrencyId,
                                          VatTypeId = a.VatTypeId,
                                          SearchFields = a.SearchFields,
                                          Prefix = a.Airline == null ? null : a.Airline.Prefix,
                                          ImageDetailId = a.ImageDetailId,
                                          AccountNumber = a.AccountNumber,
                                          BankName = a.BankName,
                                          BankAddress = a.BankAddress,
                                          Swift = a.Swift,
                                          IBANNumber = a.IBANNumber,
                                          InvitationDate = a.InvitationDate,
                                          SharedLogisticsInvitationStatusCode = a.SharedLogisticsInvitationStatusCode,
                                          SharedLogisticsInvitationStatusName = a.SharedLogisticsInvitationStatus != null ? a.SharedLogisticsInvitationStatus.Name : null,
                                          LastLoginDate = a.LastLoginDate,
                                          CollectorId = a.CollectorId,
                                          ClassifierId = a.ClassifierId,
                                          ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                          CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                          CreateDate = a.CreateDate,
                                          UpdateDate = a.UpdateDate,
                                          CreatedByUserId = a.CreatedByUserId,
                                          UpdatedByUserId = a.UpdatedByUserId,
                                          PrimaryContactId = a.PrimaryContactId,
                                          EnableConsolidationInvoices = a.EnableConsolidationInvoices,
                                          SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                          AccountManagerUserId = a.Customer == null ? null : a.Customer.AccountManagerUserId,
                                          SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                          IsActiveForMobile = a.IsActiveForMobile,
                                          IsCustomer = a.IsCustomer,
                                          CityName = a.CityName,
                                          ContactId = a.PrimaryContactId,
                                          IRSPlace = a.IRSPlace,
                                          IRSNumber = a.IRSNumber,
                                          SupportNotes = a.SupportNotes,
                                          GLAccountId = a.GLAccountId,
                                          ExternalAccountingBusinessArea = a.ExternalAccountingBusinessArea,
                                          SATPaymentMethodCode = a.SATPaymentMethodCode,
                                          SATForeignRFC = a.SATForeignRFC,
                                          MetodoPagoCode = a.MetodoPagoCode,
                                          UsoCFDICode = a.UsoCFDICode,
                                          StateName = a.StateName,
                                          IsInternationalPartner = a.IsInternationalPartner,
                                          IsAutonomy = a.IsAutonomy,
                                          CreatedByPartner = a.CreatedByPartner,
                                      }).FirstOrDefault();

                            if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }

                        else
                        {
                            entity = (CardPM)CacheManager.CacheWrapper.Get(entityName);
                        }
                    }

                    else
                    {
                        entity = (from a in myRepository.context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus")
                                  where a.Id == id && a.Tenant == tenant
                                  && (a.PartnerTypeId == "AL" || a.PartnerTypeId == "SL" || a.PartnerTypeId == "TR")
                                  select new CardPM()
                                  {
                                      ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                      AccountingVATSplit = a.AccountingVATSplit,
                                      PayablesAccountingCard = a.PayablesAccountingCard,
                                      EnglishName = a.EnglishName,
                                      Id = a.Id,
                                      InActive = a.InActive,
                                      LocalName = a.LocalName,
                                      Notes = a.Notes,
                                      PartnerTypeId = a.PartnerTypeId,
                                      PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                      PaymentTermId = a.PaymentTermId,
                                      Tenant = a.Tenant,
                                      VatNumber = a.VatNumber,
                                      Code = a.Code,
                                      MainAddressId = myMainAddressId,
                                      CountryId = a.CountryId,
                                      CountryCode = a.CountryCode,
                                      CountryName = a.CountryName,
                                      Website = a.Website,
                                      ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                      InvoiceCurrencyId = a.InvoiceCurrencyId,
                                      VatTypeId = a.VatTypeId,
                                      SearchFields = a.SearchFields,
                                      Prefix = a.Airline == null ? null : a.Airline.Prefix,
                                      ImageDetailId = a.ImageDetailId,
                                      AccountNumber = a.AccountNumber,
                                      BankName = a.BankName,
                                      BankAddress = a.BankAddress,
                                      Swift = a.Swift,
                                      IBANNumber = a.IBANNumber,
                                      InvitationDate = a.InvitationDate,
                                      SharedLogisticsInvitationStatusCode = a.SharedLogisticsInvitationStatusCode,
                                      SharedLogisticsInvitationStatusName = a.SharedLogisticsInvitationStatus != null ? a.SharedLogisticsInvitationStatus.Name : null,
                                      LastLoginDate = a.LastLoginDate,
                                      CollectorId = a.CollectorId,
                                      ClassifierId = a.ClassifierId,
                                      ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                      CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                      CreateDate = a.CreateDate,
                                      UpdateDate = a.UpdateDate,
                                      CreatedByUserId = a.CreatedByUserId,
                                      UpdatedByUserId = a.UpdatedByUserId,
                                      PrimaryContactId = a.PrimaryContactId,
                                      EnableConsolidationInvoices = a.EnableConsolidationInvoices,
                                      SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                      AccountManagerUserId = a.Customer == null ? null : a.Customer.AccountManagerUserId,
                                      SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                      IsActiveForMobile = a.IsActiveForMobile,
                                      IsCustomer = a.IsCustomer,
                                      CityName = a.CityName,
                                      ContactId = a.PrimaryContactId,
                                      IRSPlace = a.IRSPlace,
                                      IRSNumber = a.IRSNumber,
                                      SupportNotes = a.SupportNotes,
                                      GLAccountId = a.GLAccountId,
                                      ExternalAccountingBusinessArea = a.ExternalAccountingBusinessArea,
                                      SATPaymentMethodCode = a.SATPaymentMethodCode,
                                      SATForeignRFC = a.SATForeignRFC,
                                      MetodoPagoCode = a.MetodoPagoCode,
                                      UsoCFDICode = a.UsoCFDICode,
                                      StateName = a.StateName,
                                      IsInternationalPartner = a.IsInternationalPartner,
                                      IsAutonomy = a.IsAutonomy,
                                      CreatedByPartner = a.CreatedByPartner,
                                  }).FirstOrDefault();
                    }
                }

                else
                {
                    entity = (from a in myRepository.context.Cards.Include("PartnerType").Include("PaymentTerm").Include("Customer").Include("Customer.SalesmanUser").Include("Airline").Include("SharedLogisticsInvitationStatus")
                              where a.Id == id && a.Tenant == tenant
                              && (a.PartnerTypeId == "AL" || a.PartnerTypeId == "SL" || a.PartnerTypeId == "TR")
                              select new CardPM()
                              {
                                  ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                  AccountingVATSplit = a.AccountingVATSplit,
                                  PayablesAccountingCard = a.PayablesAccountingCard,
                                  EnglishName = a.EnglishName,
                                  Id = a.Id,
                                  InActive = a.InActive,
                                  LocalName = a.LocalName,
                                  Notes = a.Notes,
                                  PartnerTypeId = a.PartnerTypeId,
                                  PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                  PaymentTermId = a.PaymentTermId,
                                  Tenant = a.Tenant,
                                  VatNumber = a.VatNumber,
                                  Code = a.Code,
                                  MainAddressId = myMainAddressId,
                                  CountryId = a.CountryId,
                                  CountryCode = a.CountryCode,
                                  CountryName = a.CountryName,
                                  Website = a.Website,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  InvoiceCurrencyId = a.InvoiceCurrencyId,
                                  VatTypeId = a.VatTypeId,
                                  SearchFields = a.SearchFields,
                                  Prefix = a.Airline == null ? null : a.Airline.Prefix,
                                  ImageDetailId = a.ImageDetailId,
                                  AccountNumber = a.AccountNumber,
                                  BankName = a.BankName,
                                  BankAddress = a.BankAddress,
                                  Swift = a.Swift,
                                  IBANNumber = a.IBANNumber,
                                  InvitationDate = a.InvitationDate,
                                  SharedLogisticsInvitationStatusCode = a.SharedLogisticsInvitationStatusCode,
                                  SharedLogisticsInvitationStatusName = a.SharedLogisticsInvitationStatus != null ? a.SharedLogisticsInvitationStatus.Name : null,
                                  LastLoginDate = a.LastLoginDate,
                                  CollectorId = a.CollectorId,
                                  ClassifierId = a.ClassifierId,
                                  ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                  CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                  CreateDate = a.CreateDate,
                                  UpdateDate = a.UpdateDate,
                                  CreatedByUserId = a.CreatedByUserId,
                                  UpdatedByUserId = a.UpdatedByUserId,
                                  PrimaryContactId = a.PrimaryContactId,
                                  EnableConsolidationInvoices = a.EnableConsolidationInvoices,
                                  SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                  AccountManagerUserId = a.Customer == null ? null : a.Customer.AccountManagerUserId,
                                  SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                  IsActiveForMobile = a.IsActiveForMobile,
                                  IsCustomer = a.IsCustomer,
                                  CityName = a.CityName,
                                  ContactId = a.PrimaryContactId,
                                  IRSPlace = a.IRSPlace,
                                  IRSNumber = a.IRSNumber,
                                  SupportNotes = a.SupportNotes,
                                  GLAccountId = a.GLAccountId,
                                  ExternalAccountingBusinessArea = a.ExternalAccountingBusinessArea,
                                  SATPaymentMethodCode = a.SATPaymentMethodCode,
                                  SATForeignRFC = a.SATForeignRFC,
                                  MetodoPagoCode = a.MetodoPagoCode,
                                  UsoCFDICode = a.UsoCFDICode,
                                  StateName = a.StateName,
                                  IsInternationalPartner = a.IsInternationalPartner,
                                  IsAutonomy = a.IsAutonomy,
                                  CreatedByPartner = a.CreatedByPartner,
                              }).FirstOrDefault();
                }

                return entity;
            }

            return null;
        }

        public CardList GetSingleCardList(Card entityPOCO)
        {
            CardList entityList = null;

            if (entityPOCO != null)
            {
                int tenant = entityPOCO.Tenant;

                entityList = new CardList()
                {
                    Code = entityPOCO.Code,
                    CreateDate = entityPOCO.CreateDate,
                    EnglishName = entityPOCO.EnglishName,
                    LocalName = entityPOCO.LocalName,
                    ReceivablesAccountingCard = entityPOCO.ReceivablesAccountingCard,
                    AccountingVATSplit = entityPOCO.AccountingVATSplit,
                    PayablesAccountingCard = entityPOCO.PayablesAccountingCard,
                    InActive = entityPOCO.InActive,
                    Notes = entityPOCO.Notes,
                    Id = entityPOCO.Id,
                    Tenant = entityPOCO.Tenant,
                    VatNumber = entityPOCO.VatNumber,
                    PaymentTermId = entityPOCO.PaymentTermId,
                    PartnerTypeId = entityPOCO.PartnerTypeId,
                    PartnerTypeName = entityPOCO.PartnerType == null ? null : entityPOCO.PartnerType.Name,
                    PaymentTermName = entityPOCO.PaymentTerm == null ? null : entityPOCO.PaymentTerm.EnglishName,
                    WebSite = entityPOCO.Website,
                    InvoiceCurrencyId = entityPOCO.InvoiceCurrencyId,
                    VatTypeId = entityPOCO.VatTypeId,
                    SearchFields = entityPOCO.SearchFields,
                    BankName = entityPOCO.BankName,
                    BankAddress = entityPOCO.BankAddress,
                    AccountNumber = entityPOCO.AccountNumber,
                    Swift = entityPOCO.Swift,
                    IBANNumber = entityPOCO.IBANNumber,
                    InvitationDate = entityPOCO.InvitationDate,
                    SharedLogisticsInvitationStatusCode = entityPOCO.SharedLogisticsInvitationStatusCode,
                    SharedLogisticsInvitationStatusName = entityPOCO.SharedLogisticsInvitationStatus != null ? entityPOCO.SharedLogisticsInvitationStatus.Name : null,
                    LastLoginDate = entityPOCO.LastLoginDate,
                    PrimaryContactId = entityPOCO.PrimaryContactId,
                    EnableConsolidationInvoices = entityPOCO.EnableConsolidationInvoices,
                    CityName = entityPOCO.CityName,
                    CountryId = entityPOCO.CountryId,
                    CountryCode = entityPOCO.CountryCode,
                    CountryName = entityPOCO.CountryName,
                    SupportNotes = entityPOCO.SupportNotes,
                    GLAccountId = entityPOCO.GLAccountId,
                    ExternalAccountingBusinessArea = entityPOCO.ExternalAccountingBusinessArea,
                    SATPaymentMethodCode = entityPOCO.SATPaymentMethodCode,
                    IsCustomer = entityPOCO.IsCustomer,
                    SATForeignRFC = entityPOCO.SATForeignRFC,
                    MetodoPagoCode = entityPOCO.MetodoPagoCode,
                    UsoCFDICode = entityPOCO.UsoCFDICode,
                    //FirmCode = entityPOCO.Warehouse != null ? entityPOCO.Warehouse.FirmCode : null,
                    StateName = entityPOCO.StateName,
                    IsInternationalPartner = entityPOCO.IsInternationalPartner,
                    IsAutonomy = entityPOCO.IsAutonomy,
                    CalculatedEnglishName = string.IsNullOrEmpty(entityPOCO.EnglishName) ? entityPOCO.LocalName : entityPOCO.EnglishName,
                    CalculatedLocalName = string.IsNullOrEmpty(entityPOCO.LocalName) ? entityPOCO.EnglishName : entityPOCO.LocalName,
                    CreatedByPartner = entityPOCO.CreatedByPartner,
                    StorageFreeDays = entityPOCO.StorageFreeDays,
                };

                if (entityPOCO.Customer != null)
                {
                    #region
                    entityList.KnownConsignor = entityPOCO.Customer.KnownConsignor;
                    entityList.KCExpirationDate = entityPOCO.Customer.KCExpirationDate;
                    entityList.SalesmanUserId = entityPOCO.Customer.SalesmanUserId;
                    entityList.AccountManagerUserId = entityPOCO.Customer.AccountManagerUserId;
                    entityList.IsCreditLimitEnabled = entityPOCO.Customer.IsCreditLimitEnabled;
                    entityList.CreditLimitAmount = entityPOCO.Customer.CreditLimitAmount;
                    entityList.CreditLimitOpenBalance = entityPOCO.Customer.CreditLimitOpenBalance;
                    entityList.CreditLimitWarningPercentage = entityPOCO.Customer.CreditLimitWarningPercentage;
                    entityList.BlockNewInvoiceCreation = entityPOCO.Customer.BlockNewInvoiceCreation;
                    entityList.BlockNewShipmentCreation = entityPOCO.Customer.BlockNewShipmentCreation;

                    if (!string.IsNullOrEmpty(entityPOCO.Customer.SalesmanUserId))
                    {
                        UserRepository userRepository = new UserRepository(tenant);
                        User user = userRepository.GetSingleUser(entityList.SalesmanUserId, tenant, true);
                        if (user != null)
                        {
                            entityList.SalesmanBusinessUnitId = user.BusinessUnitId;
                            entityList.SalesmanUserEnglishName = user.Contact.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.Customer.AccountManagerUserId))
                    {
                        if (entityPOCO.Customer.AccountManagerUser != null)
                        {
                            if (entityPOCO.Customer.AccountManagerUser.Contact != null)
                            {
                                entityList.AccountManagerUserName = entityPOCO.Customer.AccountManagerUser.Contact.EnglishName;
                            }

                            else
                            {
                                Contact contact = ContactRepository.GetSingleContact(entityPOCO.Customer.AccountManagerUserId, tenant, true);
                                if (contact != null)
                                {
                                    entityList.AccountManagerUserName = contact.EnglishName;
                                }
                            }
                        }

                        else
                        {
                            UserRepository userRepository = new UserRepository(tenant);
                            User user = userRepository.GetSingleUser(entityList.AccountManagerUserId, tenant, true);
                            if (user != null)
                            {
                                if (user.Contact != null)
                                {
                                    entityList.AccountManagerUserName = user.Contact.EnglishName;

                                }

                                else
                                {
                                    Contact contact = ContactRepository.GetSingleContact(user.Id, tenant, true);
                                    if (contact != null)
                                    {
                                        entityList.AccountManagerUserName = contact.EnglishName;
                                    }
                                }
                            }
                        }
                    }

                    entityList.OpenShipments = SetCustomerOpenShipments(entityList);
                    #endregion
                }

                else if (entityPOCO.Warehouse != null)
                {
                    entityList.FirmCode = entityPOCO.Warehouse.FirmCode;
                    entityList.WarehouseTypeCode = entityPOCO.Warehouse.TypeCode;
                    entityList.ChargeStorage = entityPOCO.Warehouse.ChargeStorage;
                    entityList.ChargeStorageCurrencyId = entityPOCO.Warehouse.CurrencyId;
                    entityList.AirWeightMeasurementCode = entityPOCO.Warehouse.AirWeightMeasurementCode;
                    entityList.OceanWeightMeasurementCode = entityPOCO.Warehouse.OceanWeightMeasurementCode;
                    entityList.InlandWeightMeasurementCode = entityPOCO.Warehouse.InlandWeightMeasurementCode;
                    entityList.AirWeightRoundingCode = entityPOCO.Warehouse.AirWeightRoundingCode;
                    entityList.OceanWeightRoundingCode = entityPOCO.Warehouse.OceanWeightRoundingCode;
                    entityList.InlandWeightRoundingCode = entityPOCO.Warehouse.InlandWeightRoundingCode;
                }

                if (entityPOCO.PartnerTypeId == "AL")
                {
                    AirlineRepository airlineRepository = new AirlineRepository(tenant);
                    Airline airline = airlineRepository.GetSingleAirline(entityPOCO.Id, tenant);
                    if (airline != null)
                    {
                        entityList.AirlineAccountNumber = airline.AccountNumber;
                    }
                }

                if (entityPOCO.PartnerTypeId == "AG")
                {
                    AgentRepository myRepository = new AgentRepository(tenant);
                    Agent myAgent = myRepository.GetSingleAgent(tenant, entityPOCO.Id);
                    if (myAgent != null)
                    {
                        entityList.CASSCode = myAgent.CASSCode;
                        entityList.IATACode = myAgent.IATACode;
                        entityList.RegulatedAgentCode = myAgent.RegulatedAgentCode;
                        entityList.IsCreditLimitEnabled = myAgent.IsCreditLimitEnabled;
                        entityList.BlockNewInvoiceCreation = myAgent.BlockNewInvoiceCreation;
                        entityList.BlockNewShipmentCreation = myAgent.BlockNewShipmentCreation;
                    }
                }

                ICommonDataContext myCommonDataContext = this.repository.context;
                if (myCommonDataContext == null)
                {
                    myCommonDataContext = CommonDataContext.GetContext(tenant);
                }

                CardContactRepository cardContactRepository = new CardContactRepository(myCommonDataContext);
                entityList.ContactId = cardContactRepository.GetSinglePartnerContactId(entityList.Id);

                AddressRepository addressRepository = new AddressRepository(myCommonDataContext);
                Address mainAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(entityList.Id, "M", tenant);
                Address pickAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(entityList.Id, "P", tenant);
                Address billingAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(entityList.Id, "B", tenant);

                if (mainAddress != null)
                {
                    entityList.MainAddressId = mainAddress.Id;
                }

                if (pickAddress != null)
                {
                    entityList.PickAddressId = pickAddress.Id;
                }

                if (billingAddress != null)
                {
                    entityList.BillingAddressId = billingAddress.Id;
                }

                //GLAccountCurrency
                if (!string.IsNullOrEmpty(entityList.GLAccountId))
                {
                    GLAccountRepository glaccountRepository = new GLAccountRepository(entityList.Tenant);
                    GLAccount glaccount = glaccountRepository.GetSingle(entityList.GLAccountId, entityList.Tenant);
                    if (glaccount != null)
                    {
                        entityList.GLAccountCurrency = glaccount.CurrencyId;
                    }
                }
            }

            return entityList;
        }
        private decimal SetCustomerOpenShipments(CardList card)
        {
            CustomerOpenFilesAmountQuery customerOpenFilesAmountQuery = new CustomerOpenFilesAmountQuery(card.Tenant);
            CustomerOpenFilesAmountPM customerOpenFilesAmount = customerOpenFilesAmountQuery.GetSinglePMByCustomerId(card.Id, card.Tenant);
            if (customerOpenFilesAmount != null)
            {
                return customerOpenFilesAmount.TotalOpenFilesAmount;
            }
            else return 0;

        }

        public IQueryable<CardList> GetIQueryableEntityList(IQueryable<Card> iQueryable)
        {
            IQueryable<CardList> myResult = from card in iQueryable
                                            select new CardList()
                                            {
                                                Code = card.Code,
                                                CreateDate = card.CreateDate,
                                                EnglishName = card.EnglishName,
                                                LocalName = card.LocalName,
                                                ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                                AccountingVATSplit = card.AccountingVATSplit,
                                                PayablesAccountingCard = card.PayablesAccountingCard,
                                                InActive = card.InActive,
                                                Notes = card.Notes,
                                                Id = card.Id,
                                                Tenant = card.Tenant,
                                                VatNumber = card.VatNumber,
                                                PaymentTermId = card.PaymentTermId,
                                                PartnerTypeId = card.PartnerTypeId,
                                                PartnerTypeName = card.PartnerType == null ? null : card.PartnerType.Name,
                                                PaymentTermName = card.PaymentTerm == null ? null : card.PaymentTerm.EnglishName,
                                                WebSite = card.Website,
                                                InvoiceCurrencyId = card.InvoiceCurrencyId,
                                                VatTypeId = card.VatTypeId,
                                                SearchFields = card.SearchFields,
                                                BankName = card.BankName,
                                                BankAddress = card.BankAddress,
                                                AccountNumber = card.AccountNumber,
                                                Swift = card.Swift,
                                                IBANNumber = card.IBANNumber,
                                                InvitationDate = card.InvitationDate,
                                                SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                                                SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                                LastLoginDate = card.LastLoginDate,
                                                SalesmanUserEnglishName = card.Customer != null ? (card.Customer.SalesmanUser != null ? card.Customer.SalesmanUser.Contact.EnglishName : null) : null,
                                                CustomerStatusCode = card.Customer != null ? (card.Customer.CustomerStatus != null ? card.Customer.CustomerStatus.Code : null) : null,
                                                CustomerStatusName = card.Customer != null ? (card.Customer.CustomerStatus != null ? card.Customer.CustomerStatus.Name : null) : null,
                                                PrimaryContactId = card.PrimaryContactId,
                                                EnableConsolidationInvoices = card.EnableConsolidationInvoices,
                                                CityName = card.CityName,
                                                Address1 = card.Address1,
                                                Address2 = card.Address2,
                                                CountryId = card.CountryId,
                                                CountryCode = card.CountryCode,
                                                CountryName = card.CountryName,
                                                KnownConsignor = card.Customer == null ? null : card.Customer.KnownConsignor,
                                                KCExpirationDate = card.Customer == null ? null : card.Customer.KCExpirationDate,
                                                SalesmanUserId = card.Customer == null ? null : card.Customer.SalesmanUserId,
                                                SalesmanBusinessUnitId = card.Customer == null ? null : (card.Customer.SalesmanUser == null ? null : card.Customer.SalesmanUser.BusinessUnitId),
                                                AccountManagerUserName = card.Customer == null ? null : (card.Customer.AccountManagerUser == null ? null : (card.Customer.AccountManagerUser.Contact.EnglishName)),
                                                AccountManagerUserId = card.Customer == null ? null : card.Customer.AccountManagerUserId,
                                                CASSCode = card.Agent == null ? null : card.Agent.CASSCode,
                                                IATACode = card.Agent == null ? null : card.Agent.IATACode,
                                                RegulatedAgentCode = card.Agent == null ? null : card.Agent.RegulatedAgentCode,
                                                SupportNotes = card.SupportNotes,
                                                GLAccountId = card.GLAccountId,
                                                ExternalAccountingBusinessArea = card.ExternalAccountingBusinessArea,
                                                SATPaymentMethodCode = card.SATPaymentMethodCode,
                                                SATForeignRFC = card.SATForeignRFC,
                                                MetodoPagoCode = card.MetodoPagoCode,
                                                UsoCFDICode = card.UsoCFDICode,
                                                StateName = card.StateName,
                                                IsInternationalPartner = card.IsInternationalPartner,
                                                IsAutonomy = card.IsAutonomy,
                                                GLAccountDisplayNumber = card.GLAccountDisplayNumber,
                                                CalculatedEnglishName = string.IsNullOrEmpty(card.EnglishName) ? card.LocalName : card.EnglishName,
                                                CalculatedLocalName = string.IsNullOrEmpty(card.LocalName) ? card.EnglishName : card.LocalName,
                                                CreatedByPartner = card.CreatedByPartner,
                                                RankId = card.Customer != null ? (card.Customer.Rank != null ? card.Customer.Rank.Name : null) : null,
                                                IndustryId = card.Customer != null ? (card.Customer.Industry != null ? card.Customer.Industry.Name : null) : null,
                                                RecordDate = card.UpdateDate != null ? card.UpdateDate : card.CreateDate,
                                            };

            if (myResult.Count() > 0)
            {
                int tenant = myResult.FirstOrDefault().Tenant;

                CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
                myResult = myFilter.RunFilter(myResult);
            }

            return myResult;
        }

        public IQueryable<CardList> GetCustomerCardListByTenantVatNumber(int tenant, string VatNumber)
        {
            IQueryable<CardList> cards = (from a in repository.context.Cards.Include("Customer").Include("Customer.SalesmanUser").Include("PartnerType").Include("SharedLogisticsInvitationStatus")
                                          where a.Tenant == tenant && (a.VatNumber != null && a.VatNumber.StartsWith(VatNumber))
                                          && a.InActive == false
                                          && (a.PartnerTypeId == "CS")
                                          select new CardList()
                                          {
                                              ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                              AccountingVATSplit = a.AccountingVATSplit,
                                              PayablesAccountingCard = a.PayablesAccountingCard,
                                              EnglishName = a.EnglishName,
                                              Id = a.Id,
                                              InActive = a.InActive,
                                              LocalName = a.LocalName,
                                              Notes = a.Notes,
                                              PartnerTypeId = a.PartnerTypeId,
                                              PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                              PaymentTermId = a.PaymentTermId,
                                              Tenant = a.Tenant,
                                              VatNumber = a.VatNumber,
                                              Code = a.Code,
                                              SearchFields = a.SearchFields,
                                              //Website = a.Website,
                                              //// ImageDetailId = a.ImageDetailId,
                                              AccountNumber = a.AccountNumber,
                                              BankName = a.BankName,
                                              BankAddress = a.BankAddress,
                                              Swift = a.Swift,
                                              IBANNumber = a.IBANNumber,
                                              InvitationDate = a.InvitationDate,
                                              SharedLogisticsInvitationStatusCode = a.SharedLogisticsInvitationStatusCode,
                                              SharedLogisticsInvitationStatusName = a.SharedLogisticsInvitationStatus != null ? a.SharedLogisticsInvitationStatus.Name : null,
                                              LastLoginDate = a.LastLoginDate,
                                              // CollectorId = a.CollectorId,
                                              // ClassifierId = a.ClassifierId,
                                              //ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                              //CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                              CreateDate = a.CreateDate,
                                              UpdateDate = a.UpdateDate,
                                              CreatedByUserId = a.CreatedByUserId,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              PrimaryContactId = a.PrimaryContactId,
                                              EnableConsolidationInvoices = a.EnableConsolidationInvoices,
                                              CountryId = a.CountryId,
                                              CountryCode = a.CountryCode,
                                              CountryName = a.CountryName,
                                              IsCustomer = a.IsCustomer,
                                              SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                              SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                              IsActiveForMobile = a.IsActiveForMobile,
                                              IRSPlace = a.IRSPlace,
                                              IRSNumber = a.IRSNumber,
                                              SupportNotes = a.SupportNotes,
                                              GLAccountId = a.GLAccountId,
                                              ExternalAccountingBusinessArea = a.ExternalAccountingBusinessArea,
                                              SATPaymentMethodCode = a.SATPaymentMethodCode,
                                              SATForeignRFC = a.SATForeignRFC,
                                              MetodoPagoCode = a.MetodoPagoCode,
                                              UsoCFDICode = a.UsoCFDICode,
                                              StateName = a.StateName,
                                              IsInternationalPartner = a.IsInternationalPartner,
                                              IsAutonomy = a.IsAutonomy,
                                              CalculatedEnglishName = string.IsNullOrEmpty(a.EnglishName) ? a.LocalName : a.EnglishName,
                                              CalculatedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                              CreatedByPartner = a.CreatedByPartner,
                                          });
            return cards;
        }


        public CardList GetCarrierUpdate(string entityId, int tenant, string ccsTypeCode, string newAirlineActionCode, bool newAirlineActionValue, string notes)
        {

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
                CardRepository CardRepository = new CardRepository(objectContext);
                AirlineRepository airlineRepository = new AirlineRepository(objectContext);
                ShippingLineRepository shippingLineRepository = new ShippingLineRepository(objectContext);
                AddressRepository AddressRepository = new AddressRepository(objectContext);

                Card oldTenantCard = CardRepository.GetSingleCard(entityId, 0, true);
                Card newTenantCard = CardRepository.GetSingleCardByCodeAndType(oldTenantCard.Code, oldTenantCard.PartnerTypeId, tenant, false);
                if (oldTenantCard.UpdateDate == null)
                {
                    oldTenantCard.UpdateDate = oldTenantCard.CreateDate;
                }

                if (newTenantCard.CreateDate != oldTenantCard.UpdateDate)
                {
                    DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newTenantCard.UpdateDate = todayDateTime;
                    newTenantCard.CreateDate = oldTenantCard.UpdateDate;
                    newTenantCard.EnglishName = oldTenantCard.EnglishName;
                    newTenantCard.LocalName = oldTenantCard.LocalName;

                    if (string.IsNullOrEmpty(newTenantCard.ImageDetailId))
                    {
                        newTenantCard.ImageDetailId = oldTenantCard.ImageDetailId;
                    }

                    CardRepository.Update(newTenantCard);
                    CardRepository.SubmitChanges();

                    switch (newTenantCard.PartnerTypeId)
                    {
                        case "AL":
                            {
                                Airline NewAirline = airlineRepository.GetSingleAirline(newTenantCard.Id, newTenantCard.Tenant);
                                Airline oldAirline = airlineRepository.GetSingleAirline(oldTenantCard.Id, oldTenantCard.Tenant);
                                newTenantCard.Website = oldTenantCard.Website;
                                NewAirline.Prefix = oldAirline.Prefix;
                                NewAirline.ICAO = oldAirline.ICAO;
                                NewAirline.CheckDigit = oldAirline.CheckDigit;
                                NewAirline.LimitedLength = oldAirline.LimitedLength;
                                airlineRepository.Update(NewAirline);
                                airlineRepository.SubmitChanges();
                                break;
                            }

                        case "SL":
                            {
                                ShippingLine NewShippingLine = shippingLineRepository.GetSingleShippingLine(newTenantCard.Id, newTenantCard.Tenant);
                                ShippingLine oldShippingLine = shippingLineRepository.GetSingleShippingLine(oldTenantCard.Id, oldTenantCard.Tenant);
                                newTenantCard.Website = oldTenantCard.Website;
                                NewShippingLine.SCACCode = oldShippingLine.SCACCode;
                                shippingLineRepository.Update(NewShippingLine);
                                shippingLineRepository.SubmitChanges();
                                break;
                            }
                    }
                }

                CardList myCardList = new CardList()
                {
                    Code = newTenantCard.Code,
                    EnglishName = newTenantCard.EnglishName,
                    LocalName = newTenantCard.LocalName,
                    ReceivablesAccountingCard = newTenantCard.ReceivablesAccountingCard,
                    AccountingVATSplit = newTenantCard.AccountingVATSplit,
                    PayablesAccountingCard = newTenantCard.PayablesAccountingCard,
                    InActive = newTenantCard.InActive,
                    Notes = newTenantCard.Notes,
                    SupportNotes = newTenantCard.SupportNotes,
                    Id = newTenantCard.Id,
                    Tenant = newTenantCard.Tenant,
                    VatNumber = newTenantCard.VatNumber,
                    CreateDate = newTenantCard.CreateDate,
                    PartnerTypeName = newTenantCard.PartnerType != null ? newTenantCard.PartnerType.Name : null,
                    PaymentTermName = newTenantCard.PaymentTerm != null ? newTenantCard.PaymentTerm.EnglishName : null,
                    PaymentTermId = newTenantCard.PaymentTermId,
                    SalesmanUserId = newTenantCard.Customer != null ? newTenantCard.Customer.SalesmanUserId : "",
                    SearchFields = newTenantCard.SearchFields,
                    PartnerTypeId = newTenantCard.PartnerTypeId,
                    EnableConsolidationInvoices = newTenantCard.EnableConsolidationInvoices,
                    GLAccountId = newTenantCard.GLAccountId,
                    CityName = newTenantCard.CityName,
                    CountryName = newTenantCard.CountryName,
                    StateName = newTenantCard.StateName,

                };

                if (myCardList.CityName == null || myCardList.CountryName == null || myCardList.StateName == null)
                {
                    Address myMainAddress = AddressRepository.GetMainAddressByCardId(myCardList.Id, myCardList.Tenant);
                    if (myMainAddress != null)
                    {
                        myCardList.CityName = myMainAddress.City;
                        if (myMainAddress.Country != null)
                        {
                            myCardList.CountryName = myMainAddress.Country.EnglishName;
                            myCardList.StateName = myMainAddress.State == null ? null : myMainAddress.State.EnglishName;
                        }
                    }
                }
                scope.Complete();

                return myCardList;
            }
        }

        public CardList GetCarrierCopyToCurrentTenant(string entityId, int tenant, string ccsTypeCode, string newAirlineActionCode, bool newAirlineActionValue, string notes)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            CardRepository CardRepository = new CardRepository(objectContext);
            AddressRepository AddressRepository = new AddressRepository(objectContext);
            CountryRepository countryRepository = new CountryRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);
            StateRepository stateRepository = new StateRepository(objectContext);
            CardContactRepository CardContactRepository = new CardContactRepository(objectContext);
            ContactRepository ContactRepository = new ContactRepository(objectContext);
            AirlineRepository airlineRepository = new AirlineRepository(objectContext);
            MAWBStackRepository mAWBStackRepository = new MAWBStackRepository(objectContext);
            ShippingLineRepository shippingLineRepository = new ShippingLineRepository(objectContext);
            TruckerRepository truckerRepository = new TruckerRepository(objectContext);
            AddressQuery addressQuery = new AddressQuery(AddressRepository);
            MAWBStackQuery mawbStackQuery = new MAWBStackQuery(mAWBStackRepository);
            WarehouseRepository warehouseRepository = new WarehouseRepository(objectContext);

            Card oldTenantCard = CardRepository.GetSingleCard(entityId, 0, true);
            Card newTenantCard = CardRepository.GetSingleCardByCodeAndType(oldTenantCard.Code, oldTenantCard.PartnerTypeId, tenant, true);
            if (newTenantCard == null)
            {
                DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

                string tableName = "";
                switch (oldTenantCard.PartnerTypeId)
                {
                    case "AL":
                        {
                            tableName = "Airline";
                            break;
                        }

                    case "SL":
                        {
                            tableName = "ShippingLine";
                            break;
                        }

                    case "TR":
                        {
                            tableName = "Trucker";
                            break;
                        }

                    case "WH":
                        {
                            tableName = "Warehouse";
                            break;
                        }
                }

                #region Card
                newTenantCard = new Card()
                {
                    Tenant = tenant,
                    CreateDate = todayDateTime,
                    Id = IdCounter.GetNumber("Card", tenant).ToString(),
                    Code = oldTenantCard.Code,
                    EnglishName = oldTenantCard.EnglishName,
                    LocalName = oldTenantCard.LocalName,
                    InActive = oldTenantCard.InActive,
                    PartnerTypeId = oldTenantCard.PartnerTypeId,
                    Notes = oldTenantCard.Notes,
                    SupportNotes = oldTenantCard.SupportNotes,
                    VatNumber = oldTenantCard.VatNumber,
                    Website = oldTenantCard.Website,
                    SearchFields = oldTenantCard.SearchFields,
                    CityName = oldTenantCard.CityName,
                    ExternalAccountingBusinessArea = oldTenantCard.ExternalAccountingBusinessArea,
                    SATPaymentMethodCode = oldTenantCard.SATPaymentMethodCode,
                    SATForeignRFC = oldTenantCard.SATForeignRFC,
                    GLAccountId = oldTenantCard.GLAccountId,
                    StateName = oldTenantCard.StateName,
                    IsInternationalPartner = oldTenantCard.IsInternationalPartner,
                    IsAutonomy = oldTenantCard.IsAutonomy,
                };

                #region PaymentTerm
                if (oldTenantCard.PaymentTermId != null)
                {
                    PaymentTermRepository paymentTermsRepository = new PaymentTermRepository(objectContext);
                    PaymentTerm oldPaymentTerm = paymentTermsRepository.GetSinglePaymentTerm(oldTenantCard.PaymentTermId, 0);
                    if (oldPaymentTerm != null)
                    {
                        PaymentTerm newPaymentTem = paymentTermsRepository.GetPaymenTermsByTenant(tenant).Where(p => p.EnglishName == oldPaymentTerm.EnglishName && p.LocalName == oldPaymentTerm.LocalName && p.Days == oldPaymentTerm.Days).FirstOrDefault();
                        if (newPaymentTem == null)
                        {
                            newPaymentTem = new PaymentTerm()
                            {
                                Tenant = tenant,
                                Id = IdCounter.GetNumber("PaymentTerm", tenant).ToString(),
                                Days = oldPaymentTerm.Days,
                                DisplayInLOV = oldPaymentTerm.DisplayInLOV,
                                EnglishName = oldPaymentTerm.EnglishName,
                                InActive = oldPaymentTerm.InActive,
                                LocalName = oldPaymentTerm.LocalName,
                                SearchFields = oldPaymentTerm.SearchFields,
                            };

                            paymentTermsRepository.Add(newPaymentTem);
                            paymentTermsRepository.SubmitChanges();
                        }

                        newTenantCard.PaymentTermId = newPaymentTem.Id;
                    }
                }
                #endregion

                #region Country
                if (oldTenantCard.CountryId != null)
                {
                    Country oldCountry = CountryRepository.GetSingleCountry(oldTenantCard.CountryId, 0, true);
                    if (oldCountry != null)
                    {
                        Country newCountry = countryRepository.GetSingleCountryByCode(oldCountry.Code, tenant, true);
                        if (newCountry == null)
                        {
                            #region
                            GlobalZone newGlobalZone = globalZoneRepository.GetSingleGlobalZoneByCode(oldCountry.GlobalZone.Code, tenant);
                            if (newGlobalZone == null)
                            {
                                GlobalZone oldZone = globalZoneRepository.GetSingleGlobalZone(oldCountry.GlobalZoneId, 0);
                                newGlobalZone = new GlobalZone()
                                {
                                    Id = IdCounter.GetNumber("GlobalZone", tenant).ToString(),
                                    Tenant = tenant,
                                    EnglishName = oldZone.EnglishName,
                                    Code = oldZone.Code,
                                    InActive = oldZone.InActive,
                                    Notes = oldZone.Notes,
                                    LocalName = oldZone.LocalName,
                                    SearchFields = oldZone.SearchFields,
                                };

                                globalZoneRepository.Add(newGlobalZone);
                                globalZoneRepository.SubmitChanges();
                            }

                            newCountry = new Country()
                            {
                                Id = IdCounter.GetNumber("Country", tenant).ToString(),
                                Tenant = tenant,
                                GlobalZoneId = newGlobalZone.Id,
                                EC = oldCountry.EC,
                                EnglishName = oldCountry.EnglishName,
                                Code = oldCountry.Code,
                                InActive = oldCountry.InActive,
                                Notes = oldCountry.Notes,
                                LocalName = oldCountry.LocalName,
                                SearchFields = oldCountry.SearchFields,
                            };

                            countryRepository.Add(newCountry);
                            countryRepository.SubmitChanges();
                            #endregion
                        }

                        newTenantCard.CountryId = newCountry.Id;
                        newTenantCard.CountryCode = newCountry.Code;
                        newTenantCard.CountryName = newCountry.EnglishName;
                    }
                }
                #endregion

                #region Image Detail
                if (oldTenantCard.ImageDetailId != null)
                {
                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
                    ImageDetailRepository imageDetailRepository = new ImageDetailRepository(webFreightContext);
                    ImageDetail oldImageDetail = imageDetailRepository.GetSingleImageDetail(oldTenantCard.ImageDetailId, 0);
                    if (oldImageDetail != null)
                    {
                        string fileName = oldImageDetail.Id + "." + oldImageDetail.Extension;
                        string filePath = "tenant0/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "images");
                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = oldImageDetail.Id,
                            FolderName = "images",
                            Extension = oldImageDetail.Extension,
                            Tenant = 0,
                        };

                        byte[] datainByte = storageservice.Read(fileInfo);

                        ImageDetail newImageDetail = new ImageDetail()
                        {
                            Id = IdCounter.GetNumber("ImageDetail", tenant),
                            Tenant = tenant,
                            Extension = oldImageDetail.Extension,
                            Size = datainByte.Length
                        };

                        imageDetailRepository.Add(newImageDetail);
                        imageDetailRepository.SubmitChanges();
                        newTenantCard.ImageDetailId = newImageDetail.Id;

                        MemoryStream memorystream = new MemoryStream(datainByte);
                        BlobFileInfo fileInfo2 = new BlobFileInfo()
                        {
                            FileName = newImageDetail.Id,
                            FolderName = "images",
                            Extension = newImageDetail.Extension,
                            Tenant = tenant,
                            FileSize = datainByte.Length,
                        };

                        string[] blockIdsList = { Convert.ToBase64String(Guid.NewGuid().ToByteArray()) };
                        storageservice.WriteBlock(datainByte, datainByte.Length, blockIdsList, 0, fileInfo2);
                    }
                }
                #endregion

                CardRepository.Add(newTenantCard);
                CardRepository.SubmitChanges();

                TableLastUpdateClass.UpdateTableHistory(newTenantCard.Tenant, "Card");
                TableLastUpdateClass.UpdateTableHistory(newTenantCard.Tenant, tableName);

                #endregion

                #region Addresses
                List<AddressPM> cardAddresses = addressQuery.GetAddressesByCardId(oldTenantCard.Id, 0);
                foreach (AddressPM address in cardAddresses)
                {
                    Country country = countryRepository.GetSingleCountryByCode(address.CountryCode, tenant, true);

                    if (country == null)
                    {
                        Country oldCountry = CountryRepository.GetSingleCountry(address.CountryId, 0, true);

                        GlobalZone zone = globalZoneRepository.GetSingleGlobalZoneByCode(oldCountry.GlobalZone.Code, tenant);

                        if (zone == null)
                        {
                            GlobalZone oldZone = globalZoneRepository.GetSingleGlobalZone(oldCountry.GlobalZoneId, 0);
                            zone = new GlobalZone()
                            {
                                Id = IdCounter.GetNumber("GlobalZone", tenant).ToString(),
                                Tenant = tenant,
                                EnglishName = oldZone.EnglishName,
                                Code = oldZone.Code,
                                InActive = oldZone.InActive,
                                Notes = oldZone.Notes,
                                LocalName = oldZone.LocalName,
                                SearchFields = oldZone.SearchFields,
                            };

                            globalZoneRepository.Add(zone);
                            globalZoneRepository.SubmitChanges();

                        }

                        country = new Country()
                        {
                            Id = IdCounter.GetNumber("Country", tenant).ToString(),
                            Tenant = tenant,
                            GlobalZoneId = zone.Id,
                            EC = oldCountry.EC,
                            EnglishName = oldCountry.EnglishName,
                            Code = oldCountry.Code,
                            InActive = oldCountry.InActive,
                            Notes = oldCountry.Notes,
                            LocalName = oldCountry.LocalName,
                            SearchFields = oldCountry.SearchFields,
                        };

                        countryRepository.Add(country);
                        countryRepository.SubmitChanges();
                    }

                    State state = null;
                    if (address.StateId != null)
                    {
                        state = stateRepository.GetSingleStateByCode(address.StateCode, tenant);

                        if (state == null)
                        {
                            State oldState = stateRepository.GetSingleState(address.StateId, 0);
                            state = new State()
                            {
                                Id = IdCounter.GetNumber("State", tenant).ToString(),
                                Tenant = tenant,
                                EnglishName = oldState.EnglishName,
                                Code = oldState.Code,
                                InActive = oldState.InActive,
                                Notes = oldState.Notes,
                                LocalName = oldState.LocalName,
                                CountryId = country.Id,
                                SearchFields = oldState.SearchFields,
                            };

                            stateRepository.Add(state);
                            stateRepository.SubmitChanges();
                        }
                    }

                    Address newAddress = new Address()
                    {
                        Id = IdCounter.GetNumber("Address", tenant).ToString(),
                        Tenant = tenant,
                        Address1 = address.Address1,
                        Address2 = address.Address2,
                        AddressTypeId = address.AddressTypeId,
                        ATTN = address.ATTN,
                        CardId = newTenantCard.Id,
                        CountryId = country.Id,
                        City = address.City,
                        Description = address.Description,
                        FaxNumber = address.FaxNumber,
                        InActive = address.InActive,
                        Name = address.Name,
                        PhoneNumber = address.PhoneNumber,
                        ZipCode = address.ZipCode,
                        StateId = state != null ? state.Id : null,
                        SearchFields = address.SearchFields,
                    };

                    AddressRepository.Add(newAddress);
                    AddressRepository.SubmitChanges();
                }
                #endregion

                #region CardContact
                List<CardContact> cardContacts = CardContactRepository.GetCardContactsByCardId(oldTenantCard.Id).ToList();
                foreach (CardContact cardConact in cardContacts)
                {
                    Contact contact = null;
                    if (cardConact.ContactId != null)
                    {
                        contact = ContactRepository.GetSingleContactByEmail(cardConact.Contact.Email, tenant);

                        if (contact == null)
                        {
                            Contact oldContact = ContactRepository.GetSingleContact(cardConact.ContactId, 0);
                            contact = new Contact()
                            {
                                Id = IdCounter.GetNumber("Contact", tenant).ToString(),
                                Tenant = tenant,
                                InActive = oldContact.InActive,
                                Notes = oldContact.Notes,
                                Email = oldContact.Email,
                                Anniversary = oldContact.Anniversary,
                                Birthday = oldContact.Birthday,
                                BusinessPhone = oldContact.BusinessPhone,
                                EnglishName = oldContact.EnglishName,
                                FacebookId = oldContact.FacebookId,
                                Fax = oldContact.Fax,
                                Mobile = oldContact.Mobile,
                                Name = oldContact.Name,
                                LocalName = oldContact.LocalName,
                                Signature = oldContact.Signature,
                                SearchFields = oldContact.SearchFields,
                                DontShowLocalLabels = oldContact.DontShowLocalLabels,
                                Position = oldContact.Position,
                                ComputedKey = (!string.IsNullOrEmpty(oldContact.Email) ? oldContact.Email : oldContact.Id),
                            };

                            ContactRepository.Add(contact);
                            ContactRepository.SubmitChanges();
                        }
                    }

                    CardContact newcardContact = new CardContact()
                    {
                        Id = IdCounter.GetNumber("CardContact", tenant).ToString(),
                        CardId = newTenantCard.Id,
                        Tenant = tenant,
                        ContactId = contact.Id
                    };

                    CardContactRepository.Add(newcardContact);
                    CardContactRepository.SubmitChanges();
                }
                #endregion

                #region Airline
                if (tableName == "Airline")
                {
                    if (airlineRepository.GetSingleAirline(newTenantCard.Id, newTenantCard.Tenant) == null)
                    {
                        Airline oldAirline = airlineRepository.GetSingleAirline(oldTenantCard.Id, oldTenantCard.Tenant);
                        Airline newAirline = new Airline()
                        {
                            Id = newTenantCard.Id,
                            AWBAccount = oldAirline.AWBAccount,
                            Prefix = oldAirline.Prefix,
                            ICAO = oldAirline.ICAO,
                            Tenant = newTenantCard.Tenant,
                            CheckDigit = oldAirline.CheckDigit,
                            LimitedLength = oldAirline.LimitedLength,
                        };

                        if (!string.IsNullOrEmpty(newAirlineActionCode))
                        {
                            switch (newAirlineActionCode)
                            {
                                case "All":
                                    {
                                        newAirline.IsAllowedInAirlinesRestriction = newAirlineActionValue;
                                        break;
                                    }

                                case "Qeq":
                                    {
                                        if (ccsTypeCode == "GLSHK")
                                        {
                                            newAirline.GLSHKRegistrationRequested = true;
                                        }

                                        else
                                        {
                                            newAirline.ChampRegistrationRequested = true;
                                        }

                                        break;
                                    }

                                case "Reg":
                                    {
                                        if (ccsTypeCode == "GLSHK")
                                        {
                                            newAirline.IsGLSHKRegistered = true;
                                        }

                                        else
                                        {
                                            newAirline.IsChampRegistered = true;
                                        }
                                        break;
                                    }

                                case "Dir":
                                    {
                                        this.CheckPartcipant(newAirline, tenant, true, "Dir", null, newTenantCard.Code);

                                        break;
                                    }

                                case "Dec":
                                    {
                                        newAirline.IsDeclined = true;
                                        newAirline.DeclineNotes = notes;
                                        break;
                                    }
                            }
                        }

                        airlineRepository.Add(newAirline);
                        airlineRepository.SubmitChanges();

                        List<MAWBStackPM> stacks = mawbStackQuery.GetMAWBStackPMsByAirlineId(oldAirline.Id, oldAirline.Tenant).ToList();

                        foreach (MAWBStackPM stack in stacks)
                        {
                            MAWBStack newStack = new MAWBStack()
                            {
                                Id = IdCounter.GetNumber("MAWBStack", tenant).ToString(),
                                AirlineId = newAirline.Id,
                                Tenant = newAirline.Tenant,
                                Number = stack.Number,
                                InsertionDate = stack.InsertionDate,
                                Notes = stack.Notes,
                            };

                            mAWBStackRepository.Add(newStack);
                        }

                        mAWBStackRepository.SubmitChanges();
                    }
                }
                #endregion

                #region ShippingLine
                if (tableName == "ShippingLine")
                {
                    if (shippingLineRepository.GetSingleShippingLine(newTenantCard.Id, newTenantCard.Tenant) == null)
                    {
                        ShippingLine oldShippingLine = shippingLineRepository.GetSingleShippingLine(oldTenantCard.Id, oldTenantCard.Tenant);
                        ShippingLine newShippingLine = new ShippingLine()
                        {
                            Id = newTenantCard.Id,
                            OurCreditNumber = oldShippingLine.OurCreditNumber,
                            Tenant = newTenantCard.Tenant,
                            SCACCode = oldShippingLine.SCACCode,
                            IsINTTRARegistered = oldShippingLine.IsINTTRARegistered,
                            INTTRARegistrationNotes = oldShippingLine.INTTRARegistrationNotes,
                            INTTRAUpdatesShipment = oldShippingLine.INTTRAUpdatesShipment,
                        };

                        if (oldShippingLine.ShippingAgentId != null)
                        {
                            CardList newShippingAgent = this.GetCarrierCopyToCurrentTenant(oldShippingLine.ShippingAgentId, tenant, null, null, false, null);
                            if (newShippingAgent != null)
                            {
                                newShippingLine.ShippingAgentId = newShippingAgent.Id;
                            }
                        }

                        shippingLineRepository.Add(newShippingLine);
                        shippingLineRepository.SubmitChanges();
                    }
                }
                #endregion

                #region Trucker
                if (tableName == "Trucker")
                {
                    if (truckerRepository.GetSingleTrucker(newTenantCard.Id, newTenantCard.Tenant) == null)
                    {
                        Trucker oldTrucker = truckerRepository.GetSingleTrucker(oldTenantCard.Id, oldTenantCard.Tenant);
                        Trucker newTrucker = new Trucker()
                        {
                            Id = newTenantCard.Id,
                            Tenant = newTenantCard.Tenant,
                        };

                        truckerRepository.Add(newTrucker);
                        truckerRepository.SubmitChanges();
                    }
                }
                #endregion

                #region Warehouse
                if (tableName == "Warehouse")
                {
                    if (warehouseRepository.GetSingleWarehouse(newTenantCard.Id, newTenantCard.Tenant) == null)
                    {
                        Warehouse oldWarehouse = warehouseRepository.GetSingleWarehouse(oldTenantCard.Id, oldTenantCard.Tenant);
                        Warehouse newWarehouse = new Warehouse()
                        {
                            Id = newTenantCard.Id,
                            FirmCode = oldWarehouse.FirmCode,
                            Tenant = newTenantCard.Tenant,
                        };

                        warehouseRepository.Add(newWarehouse);
                        warehouseRepository.SubmitChanges();
                    }
                }
                #endregion

                TableLastUpdateClass.UpdateTableHistory(tenant, tableName);
                TableLastUpdateClass.UpdateTableHistory(tenant, "Carrier");
                RunStoredProcedureClass.UpdateCardSearcsRecords(newTenantCard.Id, newTenantCard.Tenant);
            }
            #region CardList            
            CardList myCardList = new CardList()
            {
                Code = newTenantCard.Code,
                EnglishName = newTenantCard.EnglishName,
                LocalName = newTenantCard.LocalName,
                ReceivablesAccountingCard = newTenantCard.ReceivablesAccountingCard,
                AccountingVATSplit = newTenantCard.AccountingVATSplit,
                PayablesAccountingCard = newTenantCard.PayablesAccountingCard,
                InActive = newTenantCard.InActive,
                Notes = newTenantCard.Notes,
                SupportNotes = newTenantCard.SupportNotes,
                Id = newTenantCard.Id,
                Tenant = newTenantCard.Tenant,
                VatNumber = newTenantCard.VatNumber,
                CreateDate = newTenantCard.CreateDate,
                PartnerTypeName = newTenantCard.PartnerType != null ? newTenantCard.PartnerType.Name : null,
                PaymentTermName = newTenantCard.PaymentTerm != null ? newTenantCard.PaymentTerm.EnglishName : null,
                PaymentTermId = newTenantCard.PaymentTermId,
                SalesmanUserId = newTenantCard.Customer != null ? newTenantCard.Customer.SalesmanUserId : "",
                SearchFields = newTenantCard.SearchFields,
                PartnerTypeId = newTenantCard.PartnerTypeId,
                EnableConsolidationInvoices = newTenantCard.EnableConsolidationInvoices,
                GLAccountId = newTenantCard.GLAccountId,
                CityName = newTenantCard.CityName,
                CountryName = newTenantCard.CountryName,
                StateName = newTenantCard.StateName,
            };

            if (myCardList.CityName == null || myCardList.CountryName == null || myCardList.StateName == null)
            {
                Address myMainAddress = AddressRepository.GetMainAddressByCardId(myCardList.Id, myCardList.Tenant);
                if (myMainAddress != null)
                {
                    myCardList.CityName = myMainAddress.City;
                    if (myMainAddress.Country != null)
                    {
                        myCardList.CountryName = myMainAddress.Country.EnglishName;
                        myCardList.StateName = myMainAddress.State == null ? null : myMainAddress.State.EnglishName;
                    }
                }
            }
            #endregion

            return myCardList;
        }

        private void CheckPartcipant(Airline myTenantAirline, int tenantManagmentId, bool isProcessed, string processType, string updatedBy, string code)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(myTenantAirline.Tenant);

            TenantManagement airlineTenant = null;
            TenantManagement forwarderTenant = null;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                airlineTenant = tenantManagementRepository.GetTenantManagementByConnectedArline(code);
                forwarderTenant = tenantManagementRepository.GetSingleTenantManagement(tenantManagmentId);

                scope.Complete();
            }

            if (airlineTenant != null)
            {
                ParticipantRepository participantRepository = new ParticipantRepository(airlineTenant.Id);
                Participant participant = participantRepository.GetSingleParticipantByForwarderandAirlineTenant(tenantManagmentId, airlineTenant.Id);

                if (participant != null)
                {
                    if (processType == "Reg")
                    {
                        participant.Registered = isProcessed;
                        participant.RegistrationUpdatedBy = updatedBy;
                    }

                    else if (processType == "Dir")
                    {
                        participant.IsDirect = isProcessed;
                    }

                    else
                    {
                        participant.RegistrationRequested = isProcessed;
                    }

                    participantRepository.Update(participant);
                    participantRepository.SubmitChanges();
                }

                else
                {
                    if (processType != "Reg")
                    {
                        if (forwarderTenant != null)
                        {
                            AddressRepository addressRepository = new AddressRepository(airlineTenant.Id);
                            TenantRepository tenantRepository = new TenantRepository(airlineTenant.Id);

                            Tenant myTenant = tenantRepository.GetSingleByTenant(forwarderTenant.Id);
                            Address myAddress = addressRepository.GetSingleAddress(myTenant.AddressId, forwarderTenant.Id);

                            ParticipantPM entityPM = new ParticipantPM()
                            {
                                Tenant = airlineTenant.Id,
                                EnglishName = forwarderTenant.Name,
                                LocalName = forwarderTenant.Name,
                                TTY = forwarderTenant.TTY,
                                ForwarderTenant = tenantManagmentId,
                                RegistrationRequested = processType == "Req" ? isProcessed : false,
                                IsDirect = processType == "Dir" ? isProcessed : false,
                                PartnerTypeId = "PT",
                            };

                            if (myAddress != null)
                            {
                                entityPM.Addresses.Add(new AddressPM()
                                {
                                    Tenant = airlineTenant.Id,
                                    AddressTypeId = "M",
                                    Description = "Main Address",
                                    Address1 = myAddress.Address1,
                                    Address2 = myAddress.Address2,
                                    ATTN = myAddress.ATTN,
                                    City = myAddress.City,
                                    CountryId = myAddress.CountryId,
                                    PhoneNumber = myAddress.PhoneNumber,
                                    FaxNumber = myAddress.FaxNumber,
                                    Name = myAddress.Name,
                                    StateId = myAddress.StateId,
                                    ZipCode = myAddress.ZipCode,
                                });
                            }

                            ParticipantService service = new ParticipantService(objectContext, entityPM.Tenant);
                            service.Create(entityPM);
                        }
                    }
                }
            }
        }

        public CardList GetSingleByGLAccountId(string glAccountId)
        {
            List<CardList> card = (from a in repository.context.Cards
                                   where a.GLAccountId == glAccountId
                                   select new CardList()
                                   {
                                       ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                       AccountingVATSplit = a.AccountingVATSplit,
                                       PayablesAccountingCard = a.PayablesAccountingCard,
                                       EnglishName = a.EnglishName,
                                       Id = a.Id,
                                       InActive = a.InActive,
                                       LocalName = a.LocalName,
                                       Notes = a.Notes,
                                       Tenant = a.Tenant,
                                       VatNumber = a.VatNumber,
                                       Code = a.Code,
                                       SearchFields = a.SearchFields,
                                       AccountNumber = a.AccountNumber,
                                       BankName = a.BankName,
                                       BankAddress = a.BankAddress,
                                       Swift = a.Swift,
                                       IBANNumber = a.IBANNumber,
                                       LastLoginDate = a.LastLoginDate,
                                       CreateDate = a.CreateDate,
                                       UpdateDate = a.UpdateDate,
                                       CreatedByUserId = a.CreatedByUserId,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       PrimaryContactId = a.PrimaryContactId,
                                       CountryId = a.CountryId,
                                       CountryCode = a.CountryCode,
                                       CountryName = a.CountryName,
                                       IsCustomer = a.IsCustomer,
                                       IsActiveForMobile = a.IsActiveForMobile,
                                       IRSPlace = a.IRSPlace,
                                       IRSNumber = a.IRSNumber,
                                       SupportNotes = a.SupportNotes,
                                       GLAccountId = a.GLAccountId,
                                       ExternalAccountingBusinessArea = a.ExternalAccountingBusinessArea,
                                       SATPaymentMethodCode = a.SATPaymentMethodCode,
                                       SATForeignRFC = a.SATForeignRFC,
                                       MetodoPagoCode = a.MetodoPagoCode,
                                       UsoCFDICode = a.UsoCFDICode,
                                       IsInternationalPartner = a.IsInternationalPartner,
                                       IsAutonomy = a.IsAutonomy,
                                       CreatedByPartner = a.CreatedByPartner,
                                   }).ToList();
            return card.FirstOrDefault();
        }

        public List<CardList> GetCardListToCustomerDWWByCardIds(List<string> CardIds, int tenant)
        {
            List<CardList> Cards = (from a in repository.context.Cards
                                    where CardIds.Contains(a.Id) && a.Tenant == tenant
                                    select new CardList()
                                    {

                                        Id = a.Id,
                                        Tenant = a.Tenant,
                                        EnglishName = a.EnglishName,
                                        Code = a.Code,
                                    }).ToList();
            return Cards;
        }

        public List<CardList> GetCardListsByCardIds(List<string> CardIds, int tenant)
        {
            List<CardList> Cards = (from a in repository.context.Cards.Include("PartnerType")
                                    where CardIds.Contains(a.Id) && a.Tenant == tenant
                                    select new CardList()
                                    {

                                        Id = a.Id,
                                        Tenant = a.Tenant,
                                        EnglishName = a.EnglishName,
                                        Code = a.Code,
                                        PartnerTypeName = a.PartnerType != null ? a.PartnerType.Name : "",
                                        Notes = a.Notes

                                    }).ToList();
            return Cards;
        }

        public CardList GetCardListForWareHouseById(string cardId, int tenant)
        {
            CardList cardList = (from a in repository.context.Cards
                                 where a.Id == cardId && a.Tenant == tenant
                                 select new CardList()
                                 {
                                     Id = a.Id,
                                     Tenant = a.Tenant,
                                     EnglishName = a.EnglishName,
                                     Code = a.Code,
                                     FirmCode = a.Warehouse != null ? a.Warehouse.FirmCode : null,
                                 }).FirstOrDefault();

            return cardList;
        }

        public CardList GetCardListForCustomsShipperById(string cardId, int tenant)
        {
            CardList cardList = (from a in repository.context.Cards
                                 where a.Id == cardId && a.Tenant == tenant
                                 select new CardList()
                                 {
                                     Id = a.Id,
                                     Tenant = a.Tenant,
                                     EnglishName = a.EnglishName,
                                     Code = a.Code,
                                     VatNumber = a.VatNumber,
                                     CountryId = a.CountryId,
                                     CountryCode = a.CountryCode,
                                     CountryName = a.CountryName,
                                     CreatedByUserId = a.CreatedByUserId,
                                     UpdatedByUserId = a.UpdatedByUserId,
                                     CreateDate = a.CreateDate,
                                     UpdateDate = a.UpdateDate,
                                 }).FirstOrDefault();

            return cardList;
        }

        public List<CardList> GetCardPMsByGLAccountId(string glAccountId,int tenant)
        {
            IQueryable<CardList> IQueryable_cards = from a in repository.context.Cards.Include("CreditLimitAmount")
                                         where a.Tenant == tenant && a.GLAccountId == glAccountId
                                         select new CardList()
                                         {
                                             Id = a.Id,
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             LocalName = a.LocalName,
                                             VatNumber = a.VatNumber,
                                             CountryCode = a.CountryCode,
                                             CountryName = a.CountryName,
                                             CreditLimitAmount = a.Customer==null?null: a.Customer.CreditLimitAmount,
                                             Tenant = a.Tenant,
                                             CityName = a.CityName,
                                             GLAccountId = a.GLAccountId,
                                             PartnerTypeId = a.PartnerTypeId,
                                         };


            List<CardList> cards = IQueryable_cards.ToList();
            foreach (CardList card in cards)
            {
                card.OpenShipments = SetCustomerOpenShipments(card);
            }

            return cards;
        }

        public List<CardList> GetCardsByGLAccountIds(List<string> glAccountIds, int tenant)
        {
            List<CardList> cards = (from a in repository.context.Cards
                                                    where a.Tenant == tenant && glAccountIds.Contains( a.GLAccountId)
                                                    select new CardList()
                                                    {
                                                        Id = a.Id,
                                                        Code = a.Code,
                                                        EnglishName = a.EnglishName,
                                                        LocalName = a.LocalName,
                                                        VatNumber = a.VatNumber,
                                                        CountryCode = a.CountryCode,
                                                        CountryName = a.CountryName,
                                                        Tenant = a.Tenant,
                                                        CityName = a.CityName,
                                                        GLAccountId = a.GLAccountId,
                                                        PartnerTypeId = a.PartnerTypeId,
                                                    }).ToList();
            return cards;
        }


        public List<CardList> GetCustomerCardsWithoutGLAccount(int tenant)
        {
            IQueryable<CardList> cards = from a in repository.context.Cards
                                         where a.Tenant == tenant && (a.GLAccountId == null || a.GLAccountId == "")
                                            && (a.PartnerTypeId == "CS" || a.PartnerTypeId == "PO")
                                         select new CardList()
                                         {
                                             Id = a.Id,
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             LocalName = a.LocalName,
                                             PartnerTypeId = a.PartnerTypeId,
                                             PayablesAccountingCard = a.PayablesAccountingCard,
                                             ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                             AccountingVATSplit = a.AccountingVATSplit,
                                             GLAccountId = a.GLAccountId,
                                         };

            return cards.ToList();
        }

        public List<CardList> GetVendorCardsWithoutGLAccount(int tenant)
        {
            IQueryable<CardList> cards = from a in repository.context.Cards
                                         where a.Tenant == tenant && (a.GLAccountId == null || a.GLAccountId == "")
                                            && (a.PartnerTypeId == "VD" || a.PartnerTypeId == "DR" || a.PartnerTypeId == "LL" || a.PartnerTypeId == "WA" || a.PartnerTypeId == "AG")
                                         select new CardList()
                                         {
                                             Id = a.Id,
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             LocalName = a.LocalName,
                                             PartnerTypeId = a.PartnerTypeId,
                                             PayablesAccountingCard = a.PayablesAccountingCard,
                                             ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                             GLAccountId = a.GLAccountId,
                                         };

            return cards.ToList();
        }

        public List<CardList> GetAllOtherCardsWithoutGLAccount(int tenant)
        {
            IQueryable<CardList> cards = from a in repository.context.Cards
                                         where a.Tenant == tenant && (a.GLAccountId == null || a.GLAccountId == "")
                                            && (a.PartnerTypeId != "CS" && a.PartnerTypeId != "PO" && a.PartnerTypeId != "AG")
                                            && (a.PartnerTypeId != "VD" && a.PartnerTypeId != "DR" && a.PartnerTypeId != "LL" && a.PartnerTypeId != "WA")

                                         select new CardList()
                                         {
                                             Id = a.Id,
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             LocalName = a.LocalName,
                                             PartnerTypeId = a.PartnerTypeId,
                                             PayablesAccountingCard = a.PayablesAccountingCard,
                                             ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                             AccountingVATSplit = a.AccountingVATSplit,
                                             GLAccountId = a.GLAccountId,
                                         };

            return cards.ToList();
        }


        public IQueryable<Card> GetAllCards()
        {
            IQueryable<Card> cards = (from a in repository.context.Cards select a);
                                       

            return cards ;
        }

        public IQueryable<CardList> GetCardsByTenant(int tenant)
        {
            IQueryable<CardList> cards = from a in repository.context.Cards
                                         where a.Tenant == tenant
                                         select new CardList()
                                         {
                                             Id = a.Id,
                                             Code = a.Code,
                                             EnglishName = a.EnglishName,
                                             LocalName = a.LocalName,
                                             PartnerTypeId = a.PartnerTypeId,
                                             PayablesAccountingCard = a.PayablesAccountingCard,
                                             ReceivablesAccountingCard = a.ReceivablesAccountingCard,
                                             AccountingVATSplit = a.AccountingVATSplit,
                                             GLAccountId = a.GLAccountId,
                                             PrimaryContactId = a.PrimaryContactId,
                                             BusinessPhone = a.PrimaryContact.BusinessPhone,
                                             //CreditStatus = a.cred,

                                         };

            return cards;
        }

        public List<ShortPartnersDetails> GetConnectedPartnerIdsByGLAccountId(string glAccountId, int tenant)
        {
            List<ShortPartnersDetails> cards = (from a in repository.context.Cards
                                                where a.Tenant == tenant && a.GLAccountId == glAccountId
                                                select new ShortPartnersDetails()
                                                {
                                                    PartnerId = a.Id,
                                                    PartnerName = a.PartnerType.Name,
                                                    InActive = a.InActive
                                                }).ToList();

            return cards;
        }
    }

    public class ShortPartnersDetails
    {
        public string PartnerId { get; set; }
        public string PartnerName { get; set; }
        public bool InActive { get; set; }
    }
}