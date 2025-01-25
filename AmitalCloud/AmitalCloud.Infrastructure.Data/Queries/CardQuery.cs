using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Data.Services;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class CardQuery
    {
        CardRepository repository;
        public CardQuery(int tenant) : this(new CardRepository(tenant))
        {
        }
        public CardQuery(CardRepository cardRepository)
        {
            repository = cardRepository;
        }
        public CardPM GetSinglePMFromCache(string id, int tenant)
        {
            string entityKeyString = $"GetSinglePMFromCache({id},{tenant})";
            var res = CacheManager.GetOrInsertNewObject<CardPM>(entityKeyString, () =>
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
                AddressPM myPickupDeliveryAddress = addressQuery.GetAddressPMByTypeAndCard(id, "P", tenant);
                string myPickupDeliveryAddressId = null;
                if (myPickupDeliveryAddress != null)
                {
                    myPickupDeliveryAddressId = myPickupDeliveryAddress.Id;
                }
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = (from a in repository.context.Cards.Include("Customer").Include("Customer.SalesmanUser").Include("PartnerType").Include("SharedLogisticsInvitationStatus")
                                  join airline in repository.context.Airlines on a.Id equals airline.Id into airlineJoin
                                  from al in airlineJoin.DefaultIfEmpty()
                                  where a.Id == id
                                  select new CardPM(a)
                                  {
                                      PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                      MainAddressId = myMainAddressId,
                                      BillingAddressId = myBillingAddressId,
                                      PickupDeliveryAddressId = myPickupDeliveryAddressId,
                                      ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                      ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                      CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                      EnableConsolidationInvoices = a.EnableConsolidationInvoices,
                                      SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                      AccountManagerUserId = a.Customer == null ? null : a.Customer.AccountManagerUserId,
                                      TeamId = a.Customer == null ? null : a.Customer.TeamId,
                                      SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                      CustomerStatusCode = a.Customer != null ? (a.Customer.CustomerStatus != null ? a.Customer.CustomerStatus.Code : null) : null,
                                      RankId = a.Customer != null ? (a.Customer.Rank != null ? a.Customer.Rank.Id : null) : null,
                                      IndustryId = a.Customer != null ? (a.Customer.Industry != null ? a.Customer.Industry.Id : null) : null,
                                      LeadSourceId = a.Customer != null ? (a.Customer.LeadSource != null ? a.Customer.LeadSource.Id : null) : null,
                                      LeadDescription = a.Customer != null ? a.Customer.LeadDescription : null,
                                      StartWorkingDate = a.Customer != null ? a.Customer.StartWorkingDate : null,
                                      ICAO = al != null ? al.ICAO : "",
                                      CustomerSizeId = a.Customer != null ? (a.Customer.CustomerSize != null ? a.Customer.CustomerSize.Id : null) : null,
                                  }).FirstOrDefault();
                        if (entity != null)
                        {
                            entity.Addresses = addressQuery.GetAddressesByCardId(id, tenant);
                            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Card", Tenant = tenant, Type = "PM", Entities = new List<CardPM> { entity }.Cast<object>().ToList() }).Set();

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
                              select new CardPM(a)
                              {
                                  PartnerTypeName = a.PartnerType == null ? null : a.PartnerType.Name,
                                  MainAddressId = myMainAddressId,
                                  BillingAddressId = myBillingAddressId,
                                  PickupDeliveryAddressId = myPickupDeliveryAddressId,
                                  ComputedLocalName = string.IsNullOrEmpty(a.LocalName) ? a.EnglishName : a.LocalName,
                                  CargoTrackingInvitationStatusName = a.CargoTrackingInvitationStatus != null ? a.CargoTrackingInvitationStatus.Name : null,
                                  SharedLogisticsInvitationStatusName = a.SharedLogisticsInvitationStatus != null ? a.SharedLogisticsInvitationStatus.Name : null,
                                  ClassifierName = a.ClassifierUser != null ? a.ClassifierUser.Contact.EnglishName : "",
                                  CollectorName = a.CollectorUser != null ? a.CollectorUser.Contact.EnglishName : "",
                                  SalesmanUserId = a.Customer == null ? null : a.Customer.SalesmanUserId,
                                  AccountManagerUserId = a.Customer == null ? null : a.Customer.AccountManagerUserId,
                                  TeamId = a.Customer == null ? null : a.Customer.TeamId,
                                  SalesmanBusinessUnitId = a.Customer == null ? null : (a.Customer.SalesmanUser == null ? null : a.Customer.SalesmanUser.BusinessUnitId),
                                  CustomerStatusCode = a.Customer != null ? (a.Customer.CustomerStatus != null ? a.Customer.CustomerStatus.Code : null) : null,
                                  RankId = a.Customer != null ? (a.Customer.Rank != null ? a.Customer.Rank.Id : null) : null,
                                  IndustryId = a.Customer != null ? (a.Customer.Industry != null ? a.Customer.Industry.Id : null) : null,
                                  LeadSourceId = a.Customer != null ? (a.Customer.LeadSource != null ? a.Customer.LeadSource.Id : null) : null,
                                  CustomerSizeId = a.Customer != null ? (a.Customer.CustomerSize != null ? a.Customer.CustomerSize.Id : null) : null,
                                  LeadDescription = a.Customer != null ? a.Customer.LeadDescription : null,
                                  StartWorkingDate = a.Customer != null ? a.Customer.StartWorkingDate : null,
                                  ICAO = al != null ? al.ICAO : "",
                              }).FirstOrDefault();

                    if (entity != null)
                    {
                        entity.Addresses = addressQuery.GetAddressesByCardId(id, tenant);
                        new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Card", Tenant = tenant, Type = "PM", Entities = new List<CardPM> { entity }.Cast<object>().ToList() }).Set();
                    }
                }
                entity = Set(entity, tenant);
                return entity;
            }

            return null;
        }
        private CardPM Set(CardPM card, int tenant)
        {
            List<DocumentTypeList> documentTypeLists =
                new DocumentTypeQuery(tenant).GetDocumentTypeListsByObjectTableId(ObjectTableRepository.GetObjectTableByName("ARInvoice"), tenant);

            card.SingleInvoiceTemplateId = (card.SingleInvoiceTemplateId == null) ? GetDefaultDocumentTypeTemplateId("999S", documentTypeLists) : card.SingleInvoiceTemplateId;
            card.CustomsInvoiceTemplateId = (card.CustomsInvoiceTemplateId == null) ? GetDefaultDocumentTypeTemplateId("999CI", documentTypeLists) : card.CustomsInvoiceTemplateId;
            card.ConsolidationInvoiceTemplateId = (card.ConsolidationInvoiceTemplateId == null) ? GetDefaultDocumentTypeTemplateId("999C", documentTypeLists) : card.ConsolidationInvoiceTemplateId;
            card.ManifestInvoiceTemplateId = (card.ManifestInvoiceTemplateId == null) ? GetDefaultDocumentTypeTemplateId("999M", documentTypeLists) : card.ManifestInvoiceTemplateId;
            return card;
        }

        private string GetDefaultDocumentTypeTemplateId(string documentTypeCode, List<DocumentTypeList> documentTypeLists)
        {
            var documentTypeList = documentTypeLists.Where(d => d.Code == documentTypeCode).FirstOrDefault();
            return documentTypeList?.DocumentTypeDefaultReportTemplateId;
        }
    }
}
