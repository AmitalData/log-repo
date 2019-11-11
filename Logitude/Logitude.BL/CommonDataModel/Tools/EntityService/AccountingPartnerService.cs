using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AccountingPartnerService
    {
        bool isNewEntity;
        private int tenant;
        public AccountingPartner entityPOCO { get; set; }
        private Card entityCard;
        private AccountingPartnerPM entityPM;
        private Contact loggedContact;
        private CardRepository cardRepository;
        private AccountingPartnerRepository entityRepository;
        private AddressRepository addressRepository;
        private ContactRepository contactRepository;
        private CardContactRepository cardContactRepository;
        private ICommonDataContext objectContext;
        private CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository;
        private ContactService contactService;
        public AccountingPartnerService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AccountingPartnerRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardContactRepository = new CardContactRepository(objectContext);
            this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(objectContext);
            this.GetLoggedContact();
        }

        public AccountingPartnerService(ICommonDataContext objectContext, AccountingPartnerPM entityPM, string loggedContactId)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AccountingPartnerRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardContactRepository = new CardContactRepository(objectContext);
            this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(objectContext);
            this.contactService = new ContactService(objectContext, tenant);
            this.loggedContact = contactRepository.GetSingleContact(loggedContactId, tenant);
        }

        private void GetLoggedContact()
        {
            string email = HttpContext.Current.User.Identity.Name;
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }

        private List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrencyChangeSet;
        public void SetChangeSet(List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrencyChangeSet)
        {
            this.cardExternalCodeByCurrencyChangeSet = cardExternalCodeByCurrencyChangeSet;
        }

        public void Create(AccountingPartnerPM entityPM)
        {
            this.entityPM = entityPM;
            this.isNewEntity = true;


            this.entityCard = cardRepository.GetSingleCardByCode(entityPM.Code, entityPM.Tenant, false);
            bool IsNewCard = true;
            if (this.entityCard != null)
            {
                this.entityPM.Id = this.entityCard.Id;
                this.entityCard.PartnerTypeId = "AC";
                IsNewCard = false;
            }
            else
            {
                this.entityPM.Id = IdCounter.GetNumber("Card", tenant).ToString();
                this.entityCard = new Card()
                {
                    Id = entityPM.Id,
                    Tenant = tenant,
                    PartnerTypeId = "AC",
                };
            }


            this.entityPOCO = new AccountingPartner()
            {
                Id = this.entityCard.Id,
                Tenant = tenant,
            };

            this.InitializeComponent();


            foreach (AddressPM itemPM in entityPM.Addresses)
            {
                this.CreateAddress(itemPM);
            }

            foreach (ContactPM itemPM in entityPM.Contacts)
            {
                this.CreateContact(itemPM);
            }

            foreach (CardExternalCodeByCurrencyPM itemPM in entityPM.CardExternalCodeByCurrencies)
            {
                this.CreateCardExternalCodeByCurrency(itemPM);
            }

            if (!entityPM.IsHybrid)
            {
                AccountingPartnerTracing.Trace(entityPM, entityPOCO, isNewEntity);
            }

            AccountingPartnerMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
            AccountingPartnerValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);
            if (IsNewCard)
            {
                cardRepository.Add(entityCard);
            }
            else
            {
                cardRepository.Update(entityCard);
            }
            entityRepository.Add(entityPOCO);
            entityRepository.SubmitChanges();

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "AccountingPartner");
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Card");

            foreach (ContactPM itemPM in entityPM.Contacts)
            {
                this.UpdateContactSearchField(itemPM);
            }
        }

        public void Update(AccountingPartnerPM entityPM, bool mapComposition = false)
        {
            this.entityPM = entityPM;
            this.isNewEntity = false;

            this.entityPOCO = entityRepository.GetSingleAccountingPartner(tenant, entityPM.Id);
            this.entityCard = cardRepository.GetSingleCard(entityPM.Id, entityPM.Tenant);
            this.entityCard.PartnerTypeId = "AC";
            if (mapComposition)
            {
                this.cardExternalCodeByCurrencyChangeSet = this.entityPM.CardExternalCodeByCurrencies;
            }
            this.InitializeComponent();

            //this.UpdateGLAccount(this.entityPM, this.entityPOCO);


            if (CacheManager.CacheWrapper != null)
            {
                string entityName = "Card" + entityPM.Id + entityPM.Tenant;
                string entityPmName = "CardPM" + entityPM.Id + entityPM.Tenant;

                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }

                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }

                entityName = "AccountingPartner" + entityPM.Id + entityPM.Tenant;
                entityPmName = "AccountingPartnerPM" + entityPM.Id + entityPM.Tenant;

                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }

                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }
            }

            this.UpdateCardExternalCodeByCurrencyCollection();

            if (!entityPM.IsHybrid)
            {
                AccountingPartnerTracing.Trace(entityPM, entityPOCO, isNewEntity);
            }

            AccountingPartnerMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
            AccountingPartnerValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);

            cardRepository.Update(entityCard);
            entityRepository.Update(entityPOCO);
            entityRepository.SubmitChanges();

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "AccountingPartner");
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Card");
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdateDate = entityPM.CreateDate;
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;

                if (!entityPM.IsHybrid && (string.IsNullOrEmpty(entityPM.Code) || entityPM.Code == "new"))
                {
                    entityPM.Code = CodeCounter.GetNumber("AccountingPartner", tenant).ToString();
                }

                if (!string.IsNullOrEmpty(entityPM.TenantAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPM.TenantAddressId, tenant);
                    if (address != null)
                    {
                        address.CardId = entityPM.Id;
                        addressRepository.Update(address);
                    }
                }
            }

            else
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            this.InitializeCardFields();
            this.ComputeContactFields();
        }

        private void InitializeCardFields()
        {
            if (isNewEntity)
            {
                // On update, will be updated from Address Service

                if (entityPM.Addresses != null)
                {
                    AddressPM myAddress = entityPM.Addresses.Where(a => a.AddressTypeId == "M").FirstOrDefault();

                    if (myAddress != null)
                    {
                        entityPM.CityName = myAddress.City;
                        entityPM.CountryId = myAddress.CountryId;

                        if (!string.IsNullOrEmpty(myAddress.CountryId))
                        {
                            Country country = CountryRepository.GetSingleCountry(myAddress.CountryId, entityPM.Tenant, true);
                            if (country != null)
                            {
                                entityPM.CountryCode = country.Code;
                                entityPM.CountryName = country.EnglishName;
                            }
                        }

                        if (!string.IsNullOrEmpty(myAddress.StateId))
                        {
                            StateRepository stateRepository = new StateRepository(objectContext);
                            State state = stateRepository.GetSingleState(myAddress.StateId, entityPM.Tenant);
                            if (state != null)
                            {
                                entityCard.StateName = state.EnglishName;
                            }
                        }
                    }

                    entityCard.CityName = entityPM.CityName;
                    entityCard.CountryId = entityPM.CountryId;
                    entityCard.CountryCode = entityPM.CountryCode;
                    entityCard.CountryName = entityPM.CountryName;
                }

                else
                {
                    entityPM.CityName = entityCard.CityName;
                    entityPM.CountryId = entityCard.CountryId;
                    entityPM.CountryCode = entityCard.CountryCode;
                    entityPM.CountryName = entityCard.CountryName;
                }
            }
        }

        private void ComputeContactFields()
        {
            if (!string.IsNullOrEmpty(entityPM.PrimaryContactId))
            {
                ContactRepository contactRepository = new ContactRepository(objectContext);
                Contact contact = contactRepository.GetSingleContact(entityPM.PrimaryContactId, entityPM.Tenant);
                if (contact != null)
                {
                    entityPM.PrimaryContactName = contact.EnglishName;
                    entityPM.PrimaryContactEmail = contact.Email;
                    entityPM.PrimaryContactPhone = contact.BusinessPhone;
                }
            }
        }

        private void UpdateCardExternalCodeByCurrencyCollection()
        {
            if (cardExternalCodeByCurrencyChangeSet != null)
            {
                foreach (CardExternalCodeByCurrencyPM itemPM in cardExternalCodeByCurrencyChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCardExternalCodeByCurrency(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCardExternalCodeByCurrency(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCardExternalCodeByCurrency(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void CreateCardExternalCodeByCurrency(CardExternalCodeByCurrencyPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("CardExternalCodeByCurrency", tenant).ToString();
            itemPM.CardId = this.entityPM.Id;
            itemPM.Tenant = tenant;
            itemPM.CurrencyId = itemPM.CurrencyId;
            itemPM.ExternalRecievableTableId = itemPM.ExternalRecievableTableId;
            itemPM.ExternalPayableTableId = itemPM.ExternalPayableTableId;
            CardExternalCodeByCurrency itemPoco = new CardExternalCodeByCurrency()
            {
                Id = itemPM.Id,
                CardId = itemPM.CardId,
                CurrencyId = itemPM.CurrencyId,
                ExternalRecievableTableId = itemPM.ExternalRecievableTableId,
                ExternalPayableTableId = itemPM.ExternalPayableTableId,
                Tenant = tenant,
            };

            CardExternalCodeByCurrencyMapping.MapEntity(itemPM, itemPoco, true);
            cardExternalCodeByCurrencyRepository.Add(itemPoco);
        }
        private void UpdateCardExternalCodeByCurrency(CardExternalCodeByCurrencyPM itemPM)
        {
            CardExternalCodeByCurrency itemPoco = cardExternalCodeByCurrencyRepository.GetSingleCardExternalCodeByCurrency(itemPM.Id, tenant);
            CardExternalCodeByCurrencyMapping.MapEntity(itemPM, itemPoco, false);

            cardExternalCodeByCurrencyRepository.Update(itemPoco);
        }
        private void DeleteCardExternalCodeByCurrency(CardExternalCodeByCurrencyPM itemPM)
        {
            CardExternalCodeByCurrency itemPoco = cardExternalCodeByCurrencyRepository.GetSingleCardExternalCodeByCurrency(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                cardExternalCodeByCurrencyRepository.Remove(itemPoco);
            }
        }

        private void CreateAddress(AddressPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("Address", tenant).ToString();
            itemPM.CardId = this.entityPOCO.Id;

            Address newAddress = new Address()
            {
                Id = itemPM.Id,
                CardId = itemPM.CardId,
                Tenant = tenant
            };

            AddressMapping.MapEntity(itemPM, newAddress, isNewEntity);
            addressRepository.Add(newAddress);
        }
        private void CreateContact(ContactPM itemContactPM)
        {
            if (itemContactPM.Email != null)
            {
                itemContactPM.Email = itemContactPM.Email.ToLower();
            }

            itemContactPM.CardId = this.entityPM.Id;
            itemContactPM.CompanyName = this.entityPM.EnglishName;

            if (itemContactPM.IsCreatedWithPartner)
            {
                if (!string.IsNullOrEmpty(itemContactPM.Email))
                {
                    itemContactPM.Id = entityPM.ExistedContactId;
                }

                if (string.IsNullOrEmpty(itemContactPM.Id))
                {
                    #region
                    itemContactPM.Id = IdCounter.GetNumber("Contact", tenant).ToString();

                    Contact newContact = new Contact()
                    {
                        Id = itemContactPM.Id,
                        Tenant = tenant,
                        UserType = "R",
                    };

                    Random rnd = new Random();
                    newContact.IndexColor = rnd.Next(1, 20);

                    ContactMapping.MapEntity(itemContactPM, newContact, isNewEntity);
                    contactRepository.Add(newContact);

                    ContactTenant newContactTenant = new ContactTenant()
                    {
                        Id = IdCounter.GetNumber("ContactTenant", tenant).ToString(),
                        TenantId = entityPM.Tenant,
                        ContactId = itemContactPM.Id,
                    };

                    ContactTenantRepository contactTenantRepository = new ContactTenantRepository(objectContext);
                    contactTenantRepository.Add(newContactTenant);


                    if (!entityPM.IsHybrid)
                    {
                        ContactTracing.Trace(itemContactPM, newContact, isNewEntity);
                    }

                    #region global db region
                    if (!string.IsNullOrEmpty(itemContactPM.Email))
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IGlobalContext globalContext = GlobalContext.GetContext();

                            GlobalContactRepository globalContactRep = new GlobalContactRepository(globalContext);

                            bool globalContactExists = (from a in globalContactRep.GetGlobalContactByTenant(tenant)
                                                        where a.Email == itemContactPM.Email
                                                        select a).Any();
                            if (!globalContactExists)
                            {

                                GlobalContact conflictcontact = globalContactRep.GetSingleGlobalContact(entityPM.Id);
                                if (conflictcontact != null)
                                {
                                    globalContactRep.Remove(conflictcontact);
                                }

                                GlobalContact gcontact = new GlobalContact() { Email = itemContactPM.Email, Id = itemContactPM.Id, GlobalTenantId = tenant, };

                                globalContactRep.Add(gcontact);
                                globalContactRep.SubmitChanges();
                                scope.Complete();
                            }
                        }
                    }
                    #endregion

                    #region if has customer id
                    if (!string.IsNullOrEmpty(itemContactPM.CustomerId))
                    {
                        CardContact cardContact = new CardContact()
                        {
                            CardId = itemContactPM.CustomerId,
                            ContactId = itemContactPM.Id,
                            Id = IdCounter.GetNumber("CardContact", entityPM.Tenant).ToString(),
                            Tenant = tenant,
                        };

                        cardContactRepository.Add(cardContact);
                    }
                    #endregion

                    #endregion
                }

                CardContact newCardContact = new CardContact()
                {
                    CardId = entityPM.Id,
                    ContactId = itemContactPM.Id,
                    Id = IdCounter.GetNumber("CardContact", tenant).ToString(),
                    Tenant = tenant
                };

                cardContactRepository.Add(newCardContact);
            }
        }
        private void UpdateContactSearchField(ContactPM itemContactPM)
        {
            if (itemContactPM != null)
            {
                if (itemContactPM.Id != null)
                {
                    string myResult = null;

                    IQueryable<Card> allCards = objectContext.CardContacts.Where(d => d.ContactId == itemContactPM.Id).Select(s => s.Card);

                    if (allCards.Count() > 0)
                    {
                        List<string> names = (from d in allCards where d.EnglishName != null group d by d.EnglishName into g select g.Key).ToList();
                        foreach (string name in names)
                        {
                            if (myResult == null)
                            {
                                myResult = name;
                            }

                            else
                            {
                                myResult += "," + name;
                            }
                        }
                    }

                    if (myResult != null)
                    {
                        if (myResult.Length > 1000)
                        {
                            myResult = myResult.Substring(0, 1000);
                        }
                    }


                    Contact myContact = contactRepository.GetSingleContact(itemContactPM.Id, itemContactPM.Tenant);
                    if (myContact != null)
                    {
                        myContact.CompanyName = myResult;
                        itemContactPM.CompanyName = myResult;

                        ContactMapping.BuildSearchFields(itemContactPM, myContact);

                        contactRepository.Update(myContact);
                        contactRepository.SubmitChanges();
                    }
                }
            }
        }

        private void UpdateGLAccount(AccountingPartnerPM entityPM, AccountingPartner entityPOCO)
        {
            if ((entityPOCO.Card != null && entityPOCO.Card.EnglishName != entityPM.EnglishName) || (entityPOCO.Card != null && entityPOCO.Card.LocalName != entityPM.LocalName))
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
                if (tenantPOCO != null && tenantPOCO.AccountingActivated)
                {
                    FullAccountingHelper fullAccountingHelper = new FullAccountingHelper();
                    GLAccountData data = new GLAccountData();
                    data.Tenant = entityPM.Tenant;
                    data.EnglishName = entityPM.EnglishName;
                    data.LocalName = entityPM.LocalName;
                    data.EntityId = entityPM.Id;
                    fullAccountingHelper.UpdateGLAccount(data);
                }
            }
        }
        public void Submit()
        {
            cardExternalCodeByCurrencyRepository.SubmitChanges();
        }
    }
}
