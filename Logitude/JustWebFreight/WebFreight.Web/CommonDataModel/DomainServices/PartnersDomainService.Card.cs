using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.BusinessUnitFilters;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.DataContracts;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Threading.Tasks;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public void UpdatePartnersSearchClass(PartnersSearchClass currentEntity) { }

        public void InsertPartnersSearchClass(PartnersSearchClass entity) { }

        public List<PartnersSearchClass> GetShipmentPartnersSearch(string name, string city, string code, string country, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CardRepository = new CardRepository(tenant);
            List<PartnersSearchClass> result = new List<PartnersSearchClass>();
            List<Card> cards = CardRepository.GetCardsBySearch(name, code, city, country, tenant);
            foreach (Card item in cards)
            {
                PartnersSearchClass newResult = new PartnersSearchClass()
                {
                    Id = item.Id,
                    PartnerType = "Card",
                    PartnerName = item.EnglishName,
                    PartnerId = item.Id,
                    SalesmanUserId = item.Customer == null ? null : item.Customer.SalesmanUserId,
                    PartnerCode = item.Code,
                };

                AddressPM addr1 = this.GetMainAddressByCardId(item.Id, item.Tenant);
                if (addr1 != null)
                {
                    newResult.PartnerCity = addr1.City;
                    newResult.PartnerCountry = addr1.CountryEnglishName;
                }
                result.Add(newResult);
            }
            return result;
        }

        public List<PartnersSearchClass> GetQuotePartnersSearch(string name, string city, string code, string country, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<PartnersSearchClass> result = new List<PartnersSearchClass>();

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardRepository cardRepository = new CardRepository(objectContext);
            List<Card> cards = cardRepository.GetCardsBySearch(name, code, city, country, tenant);
            
            foreach (Card item in cards)
            {
                PartnersSearchClass newResult = new PartnersSearchClass()
                {
                    Id = item.Id,
                    PartnerType = "Card",
                    PartnerName = item.EnglishName,
                    PartnerId = item.Id,
                    SalesmanUserId = item.Customer == null ? null : item.Customer.SalesmanUserId,
                    PartnerCode = item.Code,
                };

                AddressPM addr1 = this.GetMainAddressByCardId(item.Id, item.Tenant);
                if (addr1 != null)
                {
                    newResult.PartnerCity = addr1.City;
                    newResult.PartnerCountry = addr1.CountryEnglishName;
                }
                result.Add(newResult);
            }

            return result;
        }

        public List<PartnersSearchClass> GetQuotePartnersSearchFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            queryOperations.SetFilter("PartnerTypeId", "CS", false, "Equals", null, false);
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(QueryOperations));

            ser.Serialize(memstream, queryOperations);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            List<CardList> cards = this.GetCardCompactFilters(bytearray, tenant).ToList();
            List<PartnersSearchClass> result = new List<PartnersSearchClass>();

            foreach (CardList item in cards)
            {
                PartnersSearchClass newResult = new PartnersSearchClass()
                {
                    Id = item.Id,
                    PartnerType = "Card",
                    PartnerName = item.EnglishName,
                    PartnerId = item.Id,
                    SalesmanUserId = item.SalesmanUserId,
                    PartnerCode = item.Code,
                };

                AddressPM addr1 = this.GetMainAddressByCardId(item.Id, item.Tenant);
                if (addr1 != null)
                {
                    newResult.PartnerCity = addr1.City;
                    newResult.PartnerCountry = addr1.CountryEnglishName;
                }

                result.Add(newResult);
            }

            return result;
        }

        public void UpdateCardList(CardList currentEntity)
        {
        }

        //public Card GetAddedCard(string englishname, string localname, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    CardRepository = new CardRepository(tenant);
        //    return CardRepository.GetCards(tenant).Where(d => d.EnglishName == englishname && d.LocalName == localname && d.Tenant == tenant).FirstOrDefault();
        //}

        public CardPM GetSingleCardPM(string id, int tenant)
        {
         
          
            SecurityUtility.AuthenticationOnTenant(tenant);

            cardQuery = new CardQuery(tenant);
            return cardQuery.GetSinglePM(id, tenant);
        }

        //public List<CardPM> GetFirstCardsByTenantType(string input, int tenant, string typeInput)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    cardQuery = new CardQuery(tenant);
        //    input = input.ToUpper();
        //    string[] types = typeInput.Split(',');
        //    if (types.Length == 1)
        //    {
        //        string type = types[0].ToUpper();
        //        return cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type.ToUpper()).Where(c => c.EnglishName.ToUpper().StartsWith(input)).ToList();
        //    }
        //    else
        //    {
        //        List<CardPM> result = new List<CardPM>();
        //        foreach (string type in types)
        //        {
        //            string type1 = type;
        //            IQueryable<CardPM> cards = cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type1.ToUpper()).Where(c => c.EnglishName.ToUpper().StartsWith(input));
        //            foreach (CardPM card in cards)
        //            {
        //                result.Add(card);
        //            }
        //        }
        //        return result;
        //    }
        //}

        //public List<CardPM> GetFirstCardsByTenantTypeInput(string input, int tenant, string typeInput, bool byCode)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    cardQuery = new CardQuery(tenant);
        //    input = input.ToUpper().Trim();
        //    string[] types = typeInput.Split(',');

        //    if (!byCode)
        //    {
        //        if (types.Length == 1)
        //        {
        //            string type = types[0].ToUpper();
        //            if (input != String.Empty)
        //            {
        //                return cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type.ToUpper()).Where(c => c.EnglishName.ToUpper().StartsWith(input)).ToList();
        //            }
        //            else
        //            {
        //                return cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type.ToUpper()).ToList();
        //            }
        //        }
        //        else
        //        {
        //            List<CardPM> result = new List<CardPM>();
        //            foreach (string type in types)
        //            {
        //                IQueryable<CardPM> cards;
        //                if (input != String.Empty)
        //                {
        //                    string type1 = type;
        //                    cards = cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type1.ToUpper()).Where(c => c.EnglishName.ToUpper().StartsWith(input));
        //                }
        //                else
        //                {
        //                    string type1 = type;
        //                    cards = cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type1.ToUpper());
        //                }

        //                foreach (CardPM card in cards)
        //                {
        //                    result.Add(card);
        //                }

        //            }
        //            return result;
        //        }
        //    }
        //    else
        //    {
        //        if (types.Length == 1)
        //        {
        //            string type = types[0].ToUpper();
        //            if (input != String.Empty)
        //            {
        //                return cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type.ToUpper()).Where(c => c.Code.ToUpper().StartsWith(input.ToUpper())).ToList();
        //            }
        //            else
        //            {
        //                return cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type.ToUpper()).ToList();
        //            }
        //        }
        //        else
        //        {
        //            List<CardPM> result = new List<CardPM>();
        //            foreach (string type in types)
        //            {
        //                IQueryable<CardPM> cards;
        //                if (input != String.Empty)
        //                {
        //                    string type1 = type;
        //                    cards = cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type1.ToUpper()).Where(c => c.Code.ToUpper().StartsWith(input.ToUpper()));
        //                }
        //                else
        //                {
        //                    string type1 = type;
        //                    cards = cardQuery.GetCardPMsByTenant(tenant).Where(c => c.Tenant == tenant && c.PartnerTypeId.ToUpper() == type1.ToUpper());
        //                }

        //                foreach (CardPM card in cards)
        //                {
        //                    result.Add(card);
        //                }
        //            }
        //            return result;
        //        }
        //}
        //}

        //public IQueryable<CardPM> GetCardsByType(int tenant, string type)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    cardQuery = new CardQuery(tenant);
        //    return cardQuery.GetCardPMsByTenant(tenant).Where(d => d.Tenant == tenant && d.PartnerTypeId == type);
        //}

        //public CardPM GetCardById(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    cardQuery = new CardQuery(tenant);
        //    return cardQuery.GetSinglePM(id, tenant);
        //}

        public IQueryable<CardList> GetCardListsByContactId(string contactId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardContactRepository = new CardContactRepository(objectContext);
           
            IQueryable<Card> cards = CardContactRepository.GetCardContacts(tenant).Where(d => d.ContactId == contactId).Select(r => r.Card);
            List<CardList> result = new List<CardList>();

            foreach (Card card in cards)
            {
                CardList cardList = new CardList()
                {
                    Code = card.Code,
                    CreateDate = card.CreateDate,
                    EnglishName = card.EnglishName,
                    LocalName = card.LocalName,
                    ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                    PayablesAccountingCard = card.PayablesAccountingCard,
                    InActive = card.InActive,
                    Notes = card.Notes,
                    SupportNotes = card.SupportNotes,
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
                    PrimaryContactId = card.PrimaryContactId,
                    EnableConsolidationInvoices = card.EnableConsolidationInvoices,
                    CityName = card.CityName,
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
                };

                cardList.ContactId = CardContactRepository.GetSinglePartnerContactId(card.Id);
                result.Add(cardList);
            }

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            result = myFilter.RunFilter(result);

            return result.AsQueryable<CardList>();
        }

        public CardList GetSingleCardList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            CardList entityList = null;

            Card entityPOCO = CardRepository.GetSingleCard(id, tenant, false);

            if (entityPOCO != null)
            {
                CardQuery cardQuery = new CardQuery(tenant);

                entityList = cardQuery.GetSingleCardList(entityPOCO);
            }
        
            return entityList;
        }

        public IQueryable<CardList> GetCardListsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardRepository = new CardRepository(objectContext);
            //AddressRepository = new AddressRepository(objectContext);
            CardContactRepository = new CardContactRepository(objectContext);

            IQueryable<Card> cards = CardRepository.GetCarrierCards(tenant);
            //IQueryable<Address> addresses = AddressRepository.GetMainAddresses(tenant);
            IQueryable<CardContact> cardContacts = CardContactRepository.GetCardContacts(tenant);

            IQueryable<CardList> myList = (from card in cards
                                           //join ad in addresses on card.Id equals ad.CardId into cardAddressJoin
                                           //from j in cardAddressJoin.DefaultIfEmpty()
                                           join c in cardContacts on card.Id equals c.CardId into cardCardcontactJoin
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
                                               InActive = card.InActive,
                                               Notes = card.Notes,
                                               SupportNotes = card.SupportNotes,
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
                                               BankName = card.BankName,
                                               BankAddress = card.BankAddress,
                                               AccountNumber = card.AccountNumber,
                                               Swift = card.Swift,
                                               IBANNumber = card.IBANNumber,
                                               InvitationDate = card.InvitationDate,
                                               SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                                               SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                               LastLoginDate = card.LastLoginDate,
                                               PrimaryContactId = card.PrimaryContactId,
                                               EnableConsolidationInvoices = card.EnableConsolidationInvoices,
                                               CityName = card.CityName,
                                               CountryId = card.CountryId,
                                               CountryCode = card.CountryCode,
                                               CountryName = card.CountryName,
                                               KnownConsignor = card.Customer == null ? null : card.Customer.KnownConsignor,
                                               KCExpirationDate = card.Customer == null ? null : card.Customer.KCExpirationDate,
                                               SalesmanUserId = card.Customer == null ? null : card.Customer.SalesmanUserId,
                                               AccountManagerUserId = card.Customer == null ? null : card.Customer.AccountManagerUserId,
                                               SalesmanBusinessUnitId = card.Customer == null ? null : (card.Customer.SalesmanUser == null ? null : card.Customer.SalesmanUser.BusinessUnitId),
                                               AccountManagerUserName = card.Customer == null ? null : (card.Customer.AccountManagerUser == null ? null : (card.Customer.AccountManagerUser.Contact.EnglishName)),
                                               CASSCode = card.Agent == null ? null : card.Agent.CASSCode,
                                               IATACode = card.Agent == null ? null : card.Agent.IATACode,
                                               RegulatedAgentCode = card.Agent == null ? null : card.Agent.RegulatedAgentCode,
                                           });

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            myList = myFilter.RunFilter(myList);

            return myList;
        }

        [Query(HasSideEffects = true)]
        public List<CardList> GetCardFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardRepository = new CardRepository(objectContext);
            //AddressRepository = new AddressRepository(objectContext);
            CardContactRepository = new CardContactRepository(objectContext);
            PartnerTypeRepository partnersTypeRepository = new PartnerTypeRepository(objectContext);
            FeatureRepository featuresRepository = new FeatureRepository(objectContext);
            
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Card> cards = CardRepository.GetCards(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            CardCustomFilter customfilters = new CardCustomFilter(tenant);

            cards = customfilters.GetFilteredQuery(queryOperations, cards);
            cards = filter.GetFilteredQuery<Card>(nonListQueryOperation, cards);

            int skippedCards = queryOperations.PageIndex;

            IQueryable<CardList> myList = (from card in cards
                                           where card.Tenant == tenant
                                           select new CardList()
                                           {
                                               Code = card.Code,
                                               CreateDate = card.CreateDate,
                                               EnglishName = card.EnglishName,
                                               LocalName = card.LocalName,
                                               ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                               PayablesAccountingCard = card.PayablesAccountingCard,
                                               InActive = card.InActive,
                                               Notes = card.Notes,
                                               SupportNotes = card.SupportNotes,
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
                                               IsCustomer = card.IsCustomer,
                                               PrimaryContactId = card.PrimaryContactId,
                                               EnableConsolidationInvoices = card.EnableConsolidationInvoices,
                                               CityName = card.CityName,
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
                                           });

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            myList = myFilter.RunFilter(myList);

            var query2 = myList;
            query2 = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);
           
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CardList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Card", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderBy(d => d.EnglishName);
            }
            
            query2 = query2.Skip(skippedCards);
            query2 = query2.Take(queryOperations.PageSize);

            List<string> allowedPartnerTypes = new List<string>();
            PartnerTypeRepository partnerTypeRepository = new PartnerTypeRepository(tenant);
            IQueryable<PartnerType> types = partnersTypeRepository.GetPartnerTypes();
            foreach (PartnerType item in types)
            {
                if (item.Id == "PO")
                {
                    allowedPartnerTypes.Add(item.Id);
                }
                else
                {
                    ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(item.Name.Replace(" ", "").ToLower(), tenant);
                if (table != null)
                {
                    if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
                    {
                        allowedPartnerTypes.Add(item.Id);
                    }
                }
            }
            }

            List<CardList> myResult = new List<CardList>();
            if (allowedPartnerTypes.Count > 0)
            {
                myResult = query2.Where(d => allowedPartnerTypes.Contains(d.PartnerTypeId)).ToList();
            }

            //List<CardList> result = new List<CardList>();
            //foreach (CardList list in query2)
            //{
            //    PartnerType type = partnersTypeRepository.GetSinglePartnerType(list.PartnerTypeId);
            //    ObjectTablePM table = ObjectTabelQuery.GetObjectTableByCode(type.Name.Replace(" ", "").ToLower(), tenant);
            //    if (table != null)
            //    {
            //        if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
            //        {
            //            result.Add(list);
            //        }
            //    }
            //}

            return myResult;
        }

        public int GetCardFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardRepository = new CardRepository(objectContext);
            //AddressRepository = new AddressRepository(objectContext);
            CardContactRepository = new CardContactRepository(objectContext);
            PartnerTypeRepository partnersTypeRepository = new PartnerTypeRepository(objectContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();

            IQueryable<Card> cards = CardRepository.GetCards(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            cards = filter.GetFilteredQuery<Card>(nonListQueryOperation, cards);

            IQueryable<CardList> myList = (from card in cards
                                           where card.Tenant == tenant
                                           select new CardList()
                                           {
                                               Code = card.Code,
                                               CreateDate = card.CreateDate,
                                               EnglishName = card.EnglishName,
                                               LocalName = card.LocalName,
                                               ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                               PayablesAccountingCard = card.PayablesAccountingCard,
                                               InActive = card.InActive,
                                               Notes = card.Notes,
                                               SupportNotes = card.SupportNotes,
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
                                               PrimaryContactId = card.PrimaryContactId,
                                               EnableConsolidationInvoices = card.EnableConsolidationInvoices,
                                               CityName = card.CityName,
                                               CountryId = card.CountryId,
                                               CountryCode = card.CountryCode,
                                               CountryName = card.CountryName,
                                               SalesmanUserId = card.Customer == null ? null : card.Customer.SalesmanUserId,
                                               SalesmanBusinessUnitId = card.Customer == null ? null : (card.Customer.SalesmanUser == null ? null : card.Customer.SalesmanUser.BusinessUnitId),
                                               CASSCode = card.Agent == null ? null : card.Agent.CASSCode,
                                               IATACode = card.Agent == null ? null : card.Agent.IATACode,
                                               RegulatedAgentCode = card.Agent == null ? null : card.Agent.RegulatedAgentCode,
                                           });

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            myList = myFilter.RunFilter(myList);

            var query2 = myList;
            query2 = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);
           
            List<string> allowedPartnerTypes = new List<string>();
            PartnerTypeRepository partnerTypeRepository = new PartnerTypeRepository(tenant);
            IQueryable<PartnerType> types = partnersTypeRepository.GetPartnerTypes();
            foreach (PartnerType item in types)
            {
                if (item.Id == "PO")
                {
                    allowedPartnerTypes.Add(item.Id);
                }
                else
                {
                    ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(item.Name.Replace(" ", "").ToLower(), tenant);
                if (table != null)
                {
                    if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
                    {
                        allowedPartnerTypes.Add(item.Id);
                    }
                }
            }
            }

            int count = 0;
            if (allowedPartnerTypes.Count > 0)
            {
                query2 = query2.Where(d => allowedPartnerTypes.Contains(d.PartnerTypeId));
                count = query2.Count();
            }

            //List<CardList> result = new List<CardList>();
            //foreach (CardList list in query2)
            //{
            //    PartnerType type = partnersTypeRepository.GetSinglePartnerType(list.PartnerTypeId);
            //    ObjectTablePM table = ObjectTabelQuery.GetObjectTableByCode(type.Name.Replace(" ", "").ToLower(), tenant);
            //    if (table != null)
            //    {

            //        if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
            //        {
            //            result.Add(list);
            //        }
            //    }
            //}

            //int count = result.Count();
            return count;
        }

        [Query(HasSideEffects = true)]
        public List<CardList> GetCardCompactFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardRepository = new CardRepository(objectContext);
            //AddressRepository = new AddressRepository(objectContext);
            CardContactRepository = new CardContactRepository(objectContext);
            PartnerTypeRepository partnersTypeRepository = new PartnerTypeRepository(objectContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();

            IQueryable<Card> cards = CardRepository.GetCards(tenant);
            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            queryOperations.QueryFilterItems.Remove(item);

            object seachvalue = item != null ? item.FieldValue : null;

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            CardCustomFilter customfilters = new CardCustomFilter(tenant);

            cards = customfilters.GetFilteredQuery(queryOperations, cards);
            cards = filter.GetFilteredQuery<Card>(nonListQueryOperation, cards);

            IQueryable<CardList> myList = (from card in cards
                                           where card.Tenant == tenant
                                           select new CardList()
                                           {
                                               Code = card.Code,
                                               CreateDate = card.CreateDate,
                                               EnglishName = card.EnglishName,
                                               LocalName = card.LocalName,
                                               ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                               PayablesAccountingCard = card.PayablesAccountingCard,
                                               InActive = card.InActive,
                                               Notes = card.Notes,
                                               SupportNotes = card.SupportNotes,
                                               Id = card.Id,
                                               Tenant = card.Tenant,
                                               VatNumber = card.VatNumber,
                                               PaymentTermId = card.PaymentTermId,
                                               PartnerTypeId = card.PartnerTypeId,
                                               PartnerTypeName = card.PartnerType == null ? null : card.PartnerType.Name,
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
                                               PrimaryContactId = card.PrimaryContactId,
                                               EnableConsolidationInvoices = card.EnableConsolidationInvoices,
                                               CityName = card.CityName,
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
                                           });


            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            myList = myFilter.RunFilter(myList);

            var query2 = myList;
            query2 = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);
            List<CardList> resultList;

            if (seachvalue != null)
            {
                listQueryOperation.SetFilter("EnglishName", seachvalue, false, "StartsWith", null, false);
                IQueryable<CardList> nameQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, query2).Take(queryOperations.PageSize);

                queryOperations.SortByColumnName = "EnglishName";
                queryOperations.SortDirectin = "Ascending";

                nameQueryResult = QuerySortClass.GetSortedQuery(queryOperations, nameQueryResult, "Card", tenant);
                resultList = nameQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize)
                {
                    listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("Code", seachvalue, false, "StartsWith", null, false);

                    IQueryable<CardList> coedQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);

                    foreach (CardList card in coedQueryResult)
                    {
                        if (!resultList.Where(p => p.Code == card.Code).Any())
                        {
                            resultList.Add(card);

                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }

                    if (resultList.Count() < queryOperations.PageSize)
                    {
                        listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("SearchFields", seachvalue, false, "Contains", null, false);

                        IQueryable<CardList> searchFieldQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);


                        foreach (CardList card in searchFieldQueryResult)
                        {
                            if (!resultList.Where(p => p.Code == card.Code).Any())
                            {
                                resultList.Add(card);

                            }
                            if (resultList.Count == queryOperations.PageSize)
                            {
                                break;
                            }
                        }
                    }
                }

                query2 = resultList.AsQueryable();
            }

            query2 = query2.Take(queryOperations.PageSize);
            List<CardList> result = new List<CardList>();
            foreach (CardList list in query2)
            {
                string partnerType = list.PartnerTypeId == "PO" ? "CS" : list.PartnerTypeId;
                PartnerType type = partnersTypeRepository.GetSinglePartnerType(partnerType);
                ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(type.Name.Replace(" ", "").ToLower(), tenant);
                if (table != null)
                {
                    if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
                    {
                        result.Add(list);
                    }
                }
            }

            return result;
        }

        //public List<CardPM> GetCardsForContact(string contactId, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    if (objectContext == null)
        //    {
        //        objectContext = CommonDataContext.GetContext(tenant);
        //    }
        //    AddressRepository = new AddressRepository(objectContext);
        //    addressQuery = new AddressQuery(AddressRepository);
        //    CardContactRepository = new CardContactRepository(objectContext);

        //    IQueryable<Card> cards = CardContactRepository.GetCardContacts(tenant).Where(d => d.ContactId == contactId).Select(r => r.Card);
        //    List<CardPM> result = new List<CardPM>();
        //    foreach (Card card in cards)
        //    {
        //        CardPM cardList = new CardPM()
        //        {
        //            Code = card.Code,
        //            CreateDate = card.CreateDate,
        //            EnglishName = card.EnglishName,
        //            LocalName = card.LocalName,
        //            AccountingCard = card.AccountingCard,
        //            InActive = card.InActive,
        //            Notes = card.Notes,
        //            Id = card.Id,
        //            Tenant = card.Tenant,
        //            VatNumber = card.VatNumber,
        //            PaymentTermId = card.PaymentTermId,
        //            PartnerTypeId = card.PartnerTypeId,
        //            PartnerTypeName = card.PartnerType == null ? null : card.PartnerType.Name,
        //            SalesmanUserId = card.Customer != null ? card.Customer.SalesmanUserId : "",
        //            Website = card.Website,
        //            InvoiceCurrencyId = card.InvoiceCurrencyId,
        //            VatTypeId = card.VatTypeId,
        //            SearchFields = card.SearchFields,
        //            BankName = card.BankName,
        //            BankAddress = card.BankAddress,
        //            AccountNumber = card.AccountNumber,
        //            Swift = card.Swift,
        //            IBANNumber = card.IBANNumber,
        //            InvitationDate = card.InvitationDate,
        //            SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
        //            SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
        //            LastLoginDate = card.LastLoginDate,
        //        };

        //        AddressPM mainAddress = addressQuery.GetAddressPMByTypeAndCard(card.Id, "M", card.Tenant);
        //        if (mainAddress != null)
        //        {
        //            cardList.CityName = mainAddress.City;
        //            cardList.CountryName = mainAddress.CountryEnglishName;
        //        }

        //        cardList.ContactId = CardContactRepository.GetSinglePartnerContactId(card.Id);
        //        result.Add(cardList);
        //    }
        //    return result;
        //}

        public void InsertCard(CardPM card)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(card.Tenant);
            }
            CardRepository = new CardRepository(objectContext);
            CardService service = new CardService(objectContext, card);
            service.Create();
            TableLastUpdateClass.UpdateTableHistory(card.Tenant, "Card");
        }

        public void UpdateCard(CardPM currentEntity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }

            CardService service = new CardService(objectContext, currentEntity);
            service.Update();

            TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "Card");
        }

        public void DeleteCard(CardPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            CardRepository = new CardRepository(objectContext);
            Card removedEntity = CardRepository.GetSingleCard(entity.Id, entity.Tenant);
            CardRepository.Remove(removedEntity);
        }

        public IQueryable<CardList> GetCustomerCardsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            cardQuery = new CardQuery(tenant);
            return cardQuery.GetCustomerCardPMsByTenant(tenant);
        }

        public IQueryable<CardList> GetCustomerCardListByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            cardQuery = new CardQuery(tenant);
            return cardQuery.GetCustomerCardListByTenant(tenant);
        }

        public IQueryable<CardList> GetCustomerCardListByTenantVatNumber(int tenant, string VatNumber)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            cardQuery = new CardQuery(tenant);
            return cardQuery.GetCustomerCardListByTenantVatNumber(tenant, VatNumber);
        }
        

        #region Carrier
        //public CardPM GetCarrierById(string id, int tenant)
        //{
        //    cardQuery = new CardQuery(tenant);
        //    return cardQuery.GetSinglePM(id, tenant);
        //}

        public CardList GetSingleCarrierList(string id, int tenant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            CardRepository = new CardRepository(objectContext);
            AddressRepository = new AddressRepository(objectContext);
            addressQuery = new AddressQuery(AddressRepository);
            CardContactRepository = new CardContactRepository(objectContext);

            Card card = CardRepository.GetSingleCard(id, tenant, true);
            CardList cardList = new CardList()
            {
                Code = card.Code,
                CreateDate = card.CreateDate,
                EnglishName = card.EnglishName,
                LocalName = card.LocalName,
                ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                PayablesAccountingCard = card.PayablesAccountingCard,
                InActive = card.InActive,
                Notes = card.Notes,
                SupportNotes = card.SupportNotes,
                Id = card.Id,
                Tenant = card.Tenant,
                VatNumber = card.VatNumber,
                PaymentTermId = card.PaymentTermId,
                PartnerTypeId = card.PartnerTypeId,
                PartnerTypeName = card.PartnerType == null ? null : card.PartnerType.Name,
                PaymentTermName = card.PaymentTerm == null ? null : card.PaymentTerm.EnglishName,
                SalesmanUserId = card.Customer != null ? card.Customer.SalesmanUserId : "",
                WebSite = card.Website,
                SearchFields = card.SearchFields,
                InvitationDate = card.InvitationDate,
                SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                LastLoginDate = card.LastLoginDate,
                PrimaryContactId = card.PrimaryContactId,
                CityName = card.CityName,
                CountryId = card.CountryId,
                CountryCode = card.CountryCode,
                CountryName = card.CountryName,
            };

            if (card.PartnerTypeId == "AL")
            {
                airlineRepository = new AirlineRepository(objectContext);
                Airline airline = airlineRepository.GetSingleAirline(card.Id, tenant);
                if (airline != null)
                {
                    cardList.Prefix = airline.Prefix;
                    cardList.ICAO = airline.ICAO;
                }
            }

            AddressPM mainAddress = addressQuery.GetAddressPMByTypeAndCard(card.Id, "M", card.Tenant);
            if (mainAddress != null)
            {
                cardList.CityName = mainAddress.City;
                cardList.CountryName = mainAddress.CountryEnglishName;
            }

            cardList.ContactId = CardContactRepository.GetSinglePartnerContactId(card.Id);
            return cardList;
        }

        public IQueryable<CardList> GetCarrierListsByTenant(int tenant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            CardRepository = new CardRepository(objectContext);
            //AddressRepository = new AddressRepository(objectContext);
            CardContactRepository = new CardContactRepository(objectContext);
            airlineRepository = new AirlineRepository(objectContext);

            IQueryable<Card> cards = CardRepository.GetCarrierCards(tenant);
            //IQueryable<Address> addresses = AddressRepository.GetMainAddresses(tenant);
            IQueryable<CardContact> cardContacts = CardContactRepository.GetCardContacts(tenant);
            IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);
            IQueryable<CardList> myList = (from card in cards
                                           //join ad in addresses on card.Id equals ad.CardId into cardAddressJoin
                                           //from j in cardAddressJoin.DefaultIfEmpty()
                                           join c in cardContacts on card.Id equals c.CardId into cardCardcontactJoin
                                           from co in cardCardcontactJoin.DefaultIfEmpty()
                                           join a in airlines on card.Id equals a.Id into airlineCardJoin
                                           from al in airlineCardJoin.DefaultIfEmpty()
                                           where card.Tenant == tenant
                                           select new CardList()
                                           {
                                               Code = card.Code,
                                               CreateDate = card.CreateDate,
                                               EnglishName = card.EnglishName,
                                               LocalName = card.LocalName,
                                               ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                               PayablesAccountingCard = card.PayablesAccountingCard,
                                               InActive = card.InActive,
                                               Notes = card.Notes,
                                               SupportNotes = card.SupportNotes,
                                               Id = card.Id,
                                               Tenant = card.Tenant,
                                               VatNumber = card.VatNumber,
                                               PaymentTermId = card.PaymentTermId,
                                               PartnerTypeId = card.PartnerTypeId,
                                               PartnerTypeName = card.PartnerType == null ? null : card.PartnerType.Name,
                                               PaymentTermName = card.PaymentTerm == null ? null : card.PaymentTerm.EnglishName,
                                               SalesmanUserId = card.Customer != null ? card.Customer.SalesmanUserId : "",
                                               WebSite = card.Website,
                                               InvoiceCurrencyId = card.InvoiceCurrencyId,
                                               VatTypeId = card.VatTypeId,
                                               SearchFields = card.SearchFields,
                                               ContactId = co.ContactId,
                                               Prefix = al.Prefix,
                                               ICAO = al.ICAO,
                                               InvitationDate = card.InvitationDate,
                                               SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                                               SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                               LastLoginDate = card.LastLoginDate,
                                               PrimaryContactId = card.PrimaryContactId,
                                               CityName = card.CityName,
                                               CountryId = card.CountryId,
                                               CountryCode = card.CountryCode,
                                               CountryName = card.CountryName,
                                           });
            return myList;
        }

        [Query(HasSideEffects = true)]
        public List<CardList> GetCarrierFilters(byte[] xmlFilters, int tenant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            CardRepository = new CardRepository(objectContext);
            airlineRepository = new AirlineRepository(objectContext);

            PartnerTypeRepository partnersTypeRepository = new PartnerTypeRepository(objectContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Card> cards = CardRepository.GetCarrierCards(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            CardCustomFilter customfilters = new CardCustomFilter(tenant);

            cards = customfilters.GetFilteredQuery(queryOperations, cards);
            cards = filter.GetFilteredQuery<Card>(nonListQueryOperation, cards);

            int skippedCards = queryOperations.PageIndex;

            IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);
            IQueryable<CardList> myList = from card in cards
                                          join a in airlines on card.Id equals a.Id into airlineCardJoin
                                          from al in airlineCardJoin.DefaultIfEmpty()
                                          where card.Tenant == tenant
                                          select new CardList()
                                          {
                                              Code = card.Code,
                                              CreateDate = card.CreateDate,
                                              EnglishName = card.EnglishName,
                                              LocalName = card.LocalName,
                                              ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                              PayablesAccountingCard = card.PayablesAccountingCard,
                                              InActive = card.InActive,
                                              Notes = card.Notes,
                                              SupportNotes = card.SupportNotes,
                                              Id = card.Id,
                                              Tenant = card.Tenant,
                                              VatNumber = card.VatNumber,
                                              PaymentTermId = card.PaymentTermId,
                                              PartnerTypeId = card.PartnerTypeId,
                                              PartnerTypeName = card.PartnerType == null ? null : card.PartnerType.Name,
                                              PaymentTermName = card.PaymentTerm == null ? null : card.PaymentTerm.EnglishName,
                                              SalesmanUserId = card.Customer != null ? card.Customer.SalesmanUserId : "",
                                              WebSite = card.Website,
                                              SearchFields = card.SearchFields,
                                              Prefix = al.Prefix,
                                              ICAO = al.ICAO,
                                              InvitationDate = card.InvitationDate,
                                              SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                                              SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                              LastLoginDate = card.LastLoginDate,
                                              PrimaryContactId = card.PrimaryContactId,
                                              CityName = card.CityName,
                                              CountryId = card.CountryId,
                                              CountryCode = card.CountryCode,
                                              CountryName = card.CountryName,
                                          };

            var query2 = myList;
            query2 = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CardList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Card", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CardList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderBy(d => d.EnglishName);
            }

            query2 = query2.Skip(skippedCards);
            query2 = query2.Take(queryOperations.PageSize);
            List<CardList> result = new List<CardList>();
            List<PartnerType> partnerTypesList = partnersTypeRepository.GetPartnerTypes().ToList();
            List<ObjectTablePM> tables = ObjectTableQuery.GetObjectTablesWithTenantZero(tenant);
            Dictionary<string, bool> accessibleTables = new Dictionary<string, bool>();
            foreach (CardList list in query2)
            {
                PartnerType type = partnerTypesList.FirstOrDefault(p => p.Id == list.PartnerTypeId);
                ObjectTablePM table = tables.FirstOrDefault(t => t.Name.ToLower() == type.Name.Replace(" ", "").ToLower());
                if (table != null)
                {
                    if (accessibleTables.Keys.Any(t =>t == table.Name))
                    {
                        bool isAccessible = accessibleTables[table.Name];
                        if (isAccessible)
                        {
                            result.Add(list);
                        }
                    }
                    else
                    {
                    if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
                    {
                        result.Add(list);
                            accessibleTables.Add(table.Name, true);
                    }
                    else
                        {
                            accessibleTables.Add(table.Name, false);
                }
 
            }
                }
            }
            return result;
        }

        [Query(HasSideEffects = true)]
        public List<CardList> GetCarrierCompactFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
            CardRepository = new CardRepository(objectContext);
            airlineRepository = new AirlineRepository(objectContext);

            PartnerTypeRepository partnersTypeRepository = new PartnerTypeRepository(objectContext);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            IQueryable<Card> cards = CardRepository.GetCards(tenant);

            QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField").FirstOrDefault();
            queryOperations.QueryFilterItems.Remove(item);

            object seachvalue = item != null ? item.FieldValue : null;
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            CardCustomFilter customfilters = new CardCustomFilter(tenant);

            cards = customfilters.GetFilteredQuery(queryOperations, cards);
            cards = filter.GetFilteredQuery<Card>(nonListQueryOperation, cards);

            IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);
            IQueryable<CardList> myList = from card in cards
                                          join a in airlines on card.Id equals a.Id into airlineCardJoin
                                          from al in airlineCardJoin.DefaultIfEmpty()
                                          where card.Tenant == tenant
                                          select new CardList()
                                          {
                                              Code = card.Code,
                                              CreateDate = card.CreateDate,
                                              EnglishName = card.EnglishName,
                                              LocalName = card.LocalName,
                                              ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                              PayablesAccountingCard = card.PayablesAccountingCard,
                                              InActive = card.InActive,
                                              Notes = card.Notes,
                                              SupportNotes = card.SupportNotes,
                                              Id = card.Id,
                                              Tenant = card.Tenant,
                                              VatNumber = card.VatNumber,
                                              PaymentTermId = card.PaymentTermId,
                                              PartnerTypeId = card.PartnerTypeId,
                                              WebSite = card.Website,
                                              SearchFields = card.SearchFields,
                                              Prefix = al.Prefix,
                                              ICAO = al.ICAO,
                                              InvitationDate = card.InvitationDate,
                                              SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                                              SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                              LastLoginDate = card.LastLoginDate,
                                              PrimaryContactId = card.PrimaryContactId,
                                              CityName = card.CityName,
                                              CountryId = card.CountryId,
                                              CountryCode = card.CountryCode,
                                              CountryName = card.CountryName,
                                          };
            var query2 = myList;
            query2 = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);
            List<CardList> resultList;

            if (seachvalue != null)
            {
                listQueryOperation.SetFilter("Code", seachvalue, false, "StartsWith", null, false);
                IQueryable<CardList> codeQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, query2).Take(queryOperations.PageSize);

                queryOperations.SortByColumnName = "Code";
                queryOperations.SortDirectin = "Ascending";

                codeQueryResult = QuerySortClass.GetSortedQuery(queryOperations, codeQueryResult, "Card", tenant);
                resultList = codeQueryResult.ToList();

                if (resultList.Count() < queryOperations.PageSize)
                {
                    listQueryOperation.SetFilter("EnglishName", seachvalue, false, "StartsWith", null, false);
                    listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);

                    IQueryable<CardList> nameQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);

                    foreach (CardList card in nameQueryResult)
                    {
                        if (!resultList.Where(p => p.Code == card.Code).Any())
                        {
                            resultList.Add(card);
                        }
                        if (resultList.Count == queryOperations.PageSize)
                        {
                            break;
                        }
                    }

                    if (resultList.Count() < queryOperations.PageSize)
                    {
                        listQueryOperation.SetFilter("EnglishName", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("Code", null, false, "StartsWith", null, false);
                        listQueryOperation.SetFilter("SearchFields", seachvalue, false, "Contains", null, false);

                        IQueryable<CardList> searchFieldQueryResult = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);


                        foreach (CardList card in searchFieldQueryResult)
                        {
                            if (!resultList.Where(p => p.Code == card.Code).Any())
                            {
                                resultList.Add(card);
                            }
                            if (resultList.Count == queryOperations.PageSize)
                            {
                                break;
                            }
                        }
                    }
                }
                query2 = resultList.AsQueryable();
            }

            query2 = query2.Take(queryOperations.PageSize);
            List<CardList> result = new List<CardList>();
            foreach (CardList list in query2)
            {
                PartnerType type = partnersTypeRepository.GetSinglePartnerType(list.PartnerTypeId);
                ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(type.Name.Replace(" ", "").ToLower(), tenant);
                if (table != null)
                {
                    if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
                    {
                        result.Add(list);
                    }
                }
            }
            return result;
        }

        public int GetCarrierFiltersCount(byte[] xmlFilters, int tenant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardRepository = new CardRepository(objectContext);
            airlineRepository = new AirlineRepository(objectContext);

            PartnerTypeRepository partnersTypeRepository = new PartnerTypeRepository(objectContext);
            FeatureRepository featuresRepository = new FeatureRepository(objectContext);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Card> cards = CardRepository.GetCarrierCards(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            CardCustomFilter customfilters = new CardCustomFilter(tenant);
            cards = customfilters.GetFilteredQuery(queryOperations, cards);
            cards = filter.GetFilteredQuery<Card>(nonListQueryOperation, cards);

            IQueryable<Airline> airlines = airlineRepository.GetAirlines(tenant);
            IQueryable<CardList> myList = from card in cards
                                          join a in airlines on card.Id equals a.Id into airlineCardJoin
                                          from al in airlineCardJoin.DefaultIfEmpty()
                                          where card.Tenant == tenant
                                          select new CardList()
                                          {
                                              Code = card.Code,
                                              CreateDate = card.CreateDate,
                                              EnglishName = card.EnglishName,
                                              LocalName = card.LocalName,
                                              ReceivablesAccountingCard = card.ReceivablesAccountingCard,
                                              PayablesAccountingCard = card.PayablesAccountingCard,
                                              InActive = card.InActive,
                                              Notes = card.Notes,
                                              SupportNotes = card.SupportNotes,
                                              Id = card.Id,
                                              Tenant = card.Tenant,
                                              VatNumber = card.VatNumber,
                                              PaymentTermId = card.PaymentTermId,
                                              PartnerTypeId = card.PartnerTypeId,
                                              PartnerTypeName = card.PartnerType == null ? null : card.PartnerType.Name,
                                              PaymentTermName = card.PaymentTerm == null ? null : card.PaymentTerm.EnglishName,
                                              SalesmanUserId = card.Customer != null ? card.Customer.SalesmanUserId : "",
                                              WebSite = card.Website,
                                              SearchFields = card.SearchFields,
                                              Prefix = al.Prefix,
                                              ICAO = al.ICAO,
                                              InvitationDate = card.InvitationDate,
                                              SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                                              SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                                              LastLoginDate = card.LastLoginDate,
                                              PrimaryContactId = card.PrimaryContactId,
                                              CityName = card.CityName,
                                              CountryId = card.CountryId,
                                              CountryCode = card.CountryCode,
                                              CountryName = card.CountryName,
                                          };

            var query2 = myList;
            query2 = filter.GetFilteredQuery<CardList>(listQueryOperation, query2);

            List<CardList> result = new List<CardList>();
            List<PartnerType> partnerTypesList = partnersTypeRepository.GetPartnerTypes().ToList();
            List<ObjectTablePM> tables = ObjectTableQuery.GetObjectTablesWithTenantZero(tenant);
            Dictionary<string, bool> accessibleTables = new Dictionary<string, bool>();
            foreach (CardList list in query2)
            {
                PartnerType type = partnerTypesList.FirstOrDefault(p => p.Id == list.PartnerTypeId);
                ObjectTablePM table = tables.FirstOrDefault(t => t.Name.ToLower() == type.Name.Replace(" ", "").ToLower());
                if (table != null)
                {
                    if (accessibleTables.Keys.Any(t => t == table.Name))
                    {
                        bool isAccessible = accessibleTables[table.Name];
                        if (isAccessible)
                        {
                            result.Add(list);
                        }
                    }
                    else
                    {
                    if (SecurityUtility.CheckTableContactFeature(table.Name, "READ", tenant))
                    {
                        result.Add(list);
                            accessibleTables.Add(table.Name, true);
                    }
                        else
                        {
                            accessibleTables.Add(table.Name, false);
                }

            }
                }
            }
            int count = result.Count();
            return count;
        }


        public CardList GetCarrierCopyToCurrentTenant(string entityId, int tenant, string ccsTypeCode, string newAirlineActionCode, bool newAirlineActionValue, string notes)
        {
            CardQuery cardQuery = new CardQuery(tenant);
            CardList myResult = cardQuery.GetCarrierCopyToCurrentTenant(entityId, tenant, ccsTypeCode, newAirlineActionCode, newAirlineActionValue, notes);
            return myResult;
        }
        #endregion

        [Invoke]
        public string GetContactCompanyText(string contactId, int tenant)
        {
            string myResult = "";

            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            CardContactRepository = new CardContactRepository(objectContext);
            IQueryable<Card> iQueryable = CardContactRepository.GetCardContacts(tenant).Where(d => d.ContactId == contactId).Select(r => r.Card);

            CustomerBusinessUnitFilter myFilter = new CustomerBusinessUnitFilter(tenant);
            iQueryable = myFilter.RunFilter(iQueryable);

            IQueryable<Card> myCards = iQueryable.Where(d => d.InActive == false);

            List<string> names = myCards.Select(s => s.EnglishName).ToList();

            foreach (string item in names)
            {
                if (string.IsNullOrEmpty(myResult))
                {
                    myResult = item;
                }

                else
                {
                    myResult += ", " + item;
                }
            }

            return myResult;
        }

        public SharedLogisticsStatusStatistics GetSharedLogisticsStatistics(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            SharedLogisticsStatusStatistics dataClass = new SharedLogisticsStatusStatistics() { Id = "0001" };

            cardQuery = new CardQuery(tenant);
            IQueryable<CardList> cards = cardQuery.GetCustomerCardPMsByTenant(tenant);
            IQueryable<CardList> agents = null;

            CustomerRepository = new CustomerRepository(tenant);
            IQueryable<CustomersDataView> customers = CustomerRepository.GetCustomersDataViews(tenant);

            if (customers != null && customers.Count() > 0)
            {
                customers = customers.Where(d => d.CustomerStatusCode == "ACT" && d.IsCustomer && !d.InActive);

                dataClass.InvitedCustomersCount = customers.Where(d => d.SharedLogisticsInvitationStatusCode == 2).Count();
                dataClass.NotInvitedCustomersCount = customers.Where(d => d.SharedLogisticsInvitationStatusCode == 1).Count();
                dataClass.ActivatedCustomersCount = customers.Where(d => d.SharedLogisticsInvitationStatusCode == 3 && !d.IsActiveForMobile).Count();
                dataClass.ActivatedCustomersForMobileCount = customers.Where(d => d.SharedLogisticsInvitationStatusCode == 3 && d.IsActiveForMobile).Count();
            }

            if (cards != null)
            {
                cards = cards.Where(d => d.PartnerTypeId != "PO");

                agents = cards.Where(d => d.PartnerTypeId == "AG" && !d.InActive);
            }

            if (agents != null && agents.Count() > 0)
            {
                dataClass.InvitedAgentsCount = agents.Where(d => d.SharedLogisticsInvitationStatusCode == 2).Count();
                dataClass.NotInvitedAgentsCount = agents.Where(d => d.SharedLogisticsInvitationStatusCode == 1).Count();
                dataClass.ActivatedAgentsCount = agents.Where(d => d.SharedLogisticsInvitationStatusCode == 3).Count();
            }

            return dataClass;
        }
    }
}