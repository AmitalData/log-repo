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

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CardContactQuery
    {
        CardContactRepository repository;

        public CardContactQuery()
        {
            repository = new CardContactRepository();
        }

        public CardContactQuery(int tenant)
        {
            repository = new CardContactRepository(tenant);
        }

        public CardContactQuery(CardContactRepository cardContactRepository)
        {
            repository = cardContactRepository;
        }

        public CardContactPM GetSinglePM(string id, int tenant)
        {
            CardContactPM cardcontact = (from a in repository.context.CardContacts
                                         where a.Id == id && a.Tenant == tenant
                                         select new CardContactPM()
                                         {
                                             CardId = a.CardId,
                                             ContactId = a.ContactId,
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             InternetAccess = a.InternetAccess,
                                             LastLoginDate = a.LastLoginDate,
                                             IsAirExport = a.IsAirExport,
                                             IsAirImport = a.IsAirImport,
                                             IsInlandExport = a.IsInlandExport,
                                             IsInlandImport = a.IsInlandImport,
                                             IsOceanExport = a.IsOceanExport,
                                             IsOceanImport = a.IsOceanImport,
                                             IsAll = a.IsAll,
                                             IsCustomsImport = a.IsCustomsImport,
                                             IsInlandDomestic = a.IsInlandDomestic,
                                         }).FirstOrDefault();

            CardContactProductRepository cardContactProductRepository = new CardContactProductRepository(repository.context);
            CardContactProductQuery cardContactProductQuery = new CardContactProductQuery(cardContactProductRepository);
            cardcontact.CardContactProducts = cardContactProductQuery.GetCardContactProductPMsByCardContactId(cardcontact.Id, cardcontact.Tenant).ToList();

            CardContactAdditionalServiceRepository cardContactAdditionalServiceRepository = new CardContactAdditionalServiceRepository(repository.context);
            CardContactAdditionalServiceQuery cardContactAdditionalServiceQuery = new CardContactAdditionalServiceQuery(cardContactAdditionalServiceRepository);
            cardcontact.CardContactAdditionalServices = cardContactAdditionalServiceQuery.GetCardContactAdditionalServicePMsByCardContactId(cardcontact.Id, cardcontact.Tenant).ToList();

            return cardcontact;
        }

        public IQueryable<CardContactPM> GetCardContactPMsByTenant(int tenant)
        {
            IQueryable<CardContactPM> cardContacts = from a in repository.context.CardContacts
                                                     where a.Tenant == tenant
                                                     select new CardContactPM()
                                                     {
                                                         CardId = a.CardId,
                                                         ContactId = a.ContactId,
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                         InternetAccess = a.InternetAccess,
                                                         LastLoginDate = a.LastLoginDate,
                                                         IsAirExport = a.IsAirExport,
                                                         IsAirImport = a.IsAirImport,
                                                         IsInlandExport = a.IsInlandExport,
                                                         IsInlandImport = a.IsInlandImport,
                                                         IsOceanExport = a.IsOceanExport,
                                                         IsOceanImport = a.IsOceanImport,
                                                         IsAll = a.IsAll,
                                                         IsCustomsImport = a.IsCustomsImport,
                                                         IsInlandDomestic = a.IsInlandDomestic,
                                                     };
            return cardContacts;
        }

        public ContactPM GetContactFromCardContactPMsByCardId(string id)
        {
            ContactPM cardContacts = (from a in repository.context.CardContacts.Include("Contact")
                                      where a.CardId == id
                                      select new ContactPM()
                                      {
                                          Anniversary = a.Contact.Anniversary,
                                          Birthday = a.Contact.Birthday,
                                          BusinessPhone = a.Contact.BusinessPhone,
                                          Email = a.Contact.Email,
                                          EnglishName = a.Contact.EnglishName,
                                          FacebookId = a.Contact.FacebookId,
                                          Fax = a.Contact.Fax,
                                          InActive = a.Contact.InActive,
                                          LocalName = a.Contact.LocalName,
                                          Mobile = a.Contact.Mobile,
                                          Notes = a.Contact.Notes,
                                          Id = a.ContactId,
                                          Tenant = a.Tenant,
                                          Position = a.Contact.Position,


                                      }).FirstOrDefault();
            return cardContacts;
        }

        public CardContactPM GetSinglePMByCardCodeContactExternalId(string contactExternalId, string cardCode, int tenant)
        {
            CardContactPM cardcontact = (from a in repository.context.CardContacts
                                         where a.Contact.ExternalId == contactExternalId && a.Card.Code == cardCode && a.Tenant == tenant
                                         select new CardContactPM()
                                         {
                                             CardId = a.CardId,
                                             ContactId = a.ContactId,
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             InternetAccess = a.InternetAccess,
                                             LastLoginDate = a.LastLoginDate,
                                             IsAirExport = a.IsAirExport,
                                             IsAirImport = a.IsAirImport,
                                             IsInlandExport = a.IsInlandExport,
                                             IsInlandImport = a.IsInlandImport,
                                             IsOceanExport = a.IsOceanExport,
                                             IsOceanImport = a.IsOceanImport,
                                             IsAll = a.IsAll,
                                             IsCustomsImport = a.IsCustomsImport,
                                             IsInlandDomestic = a.IsInlandDomestic,
                                         }).FirstOrDefault();

            CardContactProductRepository cardContactProductRepository = new CardContactProductRepository(repository.context);
            CardContactProductQuery cardContactProductQuery = new CardContactProductQuery(cardContactProductRepository);
            cardcontact.CardContactProducts = cardContactProductQuery.GetCardContactProductPMsByCardContactId(cardcontact.Id, cardcontact.Tenant).ToList();

            CardContactAdditionalServiceRepository cardContactAdditionalServiceRepository = new CardContactAdditionalServiceRepository(repository.context);
            CardContactAdditionalServiceQuery cardContactAdditionalServiceQuery = new CardContactAdditionalServiceQuery(cardContactAdditionalServiceRepository);
            cardcontact.CardContactAdditionalServices = cardContactAdditionalServiceQuery.GetCardContactAdditionalServicePMsByCardContactId(cardcontact.Id, cardcontact.Tenant).ToList();

            return cardcontact;
        }

        public List<CardPM> GetCardsForContact(string contactId, int tenant)
        {
            List<CardPM> myResult = new List<CardPM>();

            AddressRepository AddressRepository = new AddressRepository(this.repository.context);
            PartnerTypeRepository myPartnerTypeRepository = new PartnerTypeRepository(this.repository.context);
            AddressQuery myAddressQuery = new AddressQuery(AddressRepository);

            List<Card> cards = this.repository.GetCardContacts(tenant).Where(d => d.ContactId == contactId).Select(r => r.Card).ToList();
            foreach (Card card in cards)
            {
                CardPM itemPM = new CardPM()
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
                    SalesmanUserId = card.Customer != null ? card.Customer.SalesmanUserId : "",
                    AccountManagerUserId = card.Customer != null ? card.Customer.AccountManagerUserId : "",
                    TeamId = card.Customer != null ? card.Customer.TeamId : "",
                    Website = card.Website,
                    InvoiceCurrencyId = card.InvoiceCurrencyId,
                    VatTypeId = card.VatTypeId,
                    SearchFields = card.SearchFields,
                    BankName = card.BankName,
                    BankAddress = card.BankAddress,
                    AccountNumber = card.AccountNumber,
                    Swift = card.Swift,
                    IBANNumber = card.IBANNumber,
                    InvitationDate = card.InvitationDate,
                    CargoTrackingInvitationDate = card.CargoTrackingInvitationDate,
                    SharedLogisticsInvitationStatusCode = card.SharedLogisticsInvitationStatusCode,
                    SharedLogisticsInvitationStatusName = card.SharedLogisticsInvitationStatus != null ? card.SharedLogisticsInvitationStatus.Name : null,
                    CargoTrackingInvitationStatusCode = card.CargoTrackingInvitationStatusCode,
                    CargoTrackingInvitationStatusName = card.CargoTrackingInvitationStatus != null ? card.CargoTrackingInvitationStatus.Name : null,
                    LastLoginDate = card.LastLoginDate,
                    ContactId = contactId,
                    CityName = card.CityName,
                    ClassifierId = card.ClassifierId,
                    CollectorId = card.CollectorId,
                    IsCustomer = card.IsCustomer,
                    CountryId = card.CountryId,
                    CountryCode = card.CountryCode,
                    CountryName = card.CountryName,
                    CreatedByUserId = card.CreatedByUserId,
                    UpdateDate = card.UpdateDate,
                    ImageDetailId = card.ImageDetailId,
                    EnableConsolidationInvoices = card.EnableConsolidationInvoices,
                    IsActiveForMobile = card.IsActiveForMobile,
                    UpdatedByUserId = card.UpdatedByUserId,
                };

                AddressPM mainAddress = myAddressQuery.GetAddressPMByTypeAndCard(card.Id, "M", card.Tenant);
                if (mainAddress != null)
                {
                    itemPM.CityName = mainAddress.City;
                    itemPM.CountryName = mainAddress.CountryEnglishName;
                }

                if (itemPM.PartnerTypeId != null)
                {
                    PartnerType type = myPartnerTypeRepository.GetSinglePartnerType(itemPM.PartnerTypeId);
                    itemPM.PartnerTypeName = type.Name;
                }

                CardContact cardcontact = this.repository.GetCardContactByContactAndCard(itemPM.Id, contactId, tenant);
                if (cardcontact != null)
                {
                    itemPM.InternetAccess = cardcontact.InternetAccess;
                    itemPM.LastLoginDate = cardcontact.LastLoginDate;
                }

                itemPM.ContactId = this.repository.GetSinglePartnerContactId(card.Id);
                myResult.Add(itemPM);
            }

            return myResult;
        }

        public List<string> GetContactsIdsByCardId(string cardId, int tenant)
        {
            List<string> cardContactsIds = (from a in repository.context.CardContacts
                                      where a.CardId == cardId && a.Tenant == tenant
                                      select a.ContactId).ToList();
            return cardContactsIds;
        }
    }
}
