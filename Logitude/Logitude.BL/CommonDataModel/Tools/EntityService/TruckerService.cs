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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Helpers;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools.CustomFields;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class TruckerService
    {
        bool isNewEntity;
        private int tenant;
        public Trucker entityPOCO { get; set; }
        private Card entityCard;
        private TruckerPM entityPM;
        private Contact loggedContact;
        private CardRepository cardRepository;
        private TruckerRepository entityRepository;
        private AddressRepository addressRepository;
        private ContactRepository contactRepository;
        private CardContactRepository cardContactRepository;
        private ICommonDataContext objectContext;
        private CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository;

        public TruckerService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new TruckerRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardContactRepository = new CardContactRepository(objectContext);
            this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(objectContext);
            this.GetLoggedContact();
        }

        public TruckerService(ICommonDataContext objectContext, TruckerPM entityPM, string loggedContactId)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.entityRepository = new TruckerRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardContactRepository = new CardContactRepository(objectContext);
            this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(objectContext);
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

        public void Create(TruckerPM entityPM)
        {
            this.entityPM = entityPM;
            this.isNewEntity = true;
            this.entityPM.Id = string.IsNullOrEmpty(this.entityPM.Id) || this.entityPM.IsHybrid ? IdCounter.GetNumber("Card", tenant).ToString() : this.entityPM.Id;

            this.entityCard = new Card()
            {
                Id = entityPM.Id,
                Tenant = tenant,
                PartnerTypeId = "TR",
                UploadingUniqueKey = entityPM.UploadingUniqueKey,
            };

            this.entityPOCO = new Trucker()
            {
                Id = entityPM.Id,
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
                TruckerTracing.Trace(entityPM, entityPOCO, isNewEntity);
            }

            TruckerMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
            TruckerValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);

            cardRepository.Add(entityCard);
            entityRepository.Add(entityPOCO);
            entityRepository.SubmitChanges();
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Trucker", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<TruckerPM> { entityPM }.Cast<object>().ToList() }).Update();

            //TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Trucker");
            //TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Carrier");

            foreach (ContactPM itemPM in entityPM.Contacts)
            {
                this.UpdateContactSearchField(itemPM);
            }
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            {
                RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
            }
        }

        public void Update(TruckerPM entityPM, bool mapComposition = false)
        {
            this.entityPM = entityPM;
            this.isNewEntity = false;

            this.entityPOCO = entityRepository.GetSingleTrucker(entityPM.Id, tenant);
            this.entityCard = cardRepository.GetSingleCard(entityPM.Id, entityPM.Tenant);

            this.InitializeComponent();

            if (mapComposition)
            {
                this.SetChangeSet(this.entityPM.CardExternalCodeByCurrencies);
            }
            if (CacheManager.CacheWrapper != null)
            {
                string entityName = "Card" + entityPOCO.Id + entityPOCO.Tenant;
                string entityPmName = "CardPM" + entityPOCO.Id + entityPOCO.Tenant;
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
                TruckerTracing.Trace(entityPM, entityPOCO, isNewEntity);
            }

            TruckerMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
            TruckerValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);

            cardRepository.Update(entityCard);
            entityRepository.Update(entityPOCO);
            entityRepository.SubmitChanges();
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Trucker", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<TruckerPM> { entityPM }.Cast<object>().ToList() }).Update();

            //TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Trucker");
            //TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Carrier");
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            {
                RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
            }
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdateDate = entityPM.CreateDate;
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;
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
            entityCard.EmailForSendingSingArinvoice = entityPM.Card?.EmailForSendingSingArinvoice;
            entityCard.SendingInterestReport = entityPM.Card!=null?entityPM.Card.SendingInterestReport : entityCard.SendingInterestReport;
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
            itemContactPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

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
                else
                {
                    Contact newContact = contactRepository.GetSingleContact(itemContactPM.Id, itemContactPM.Tenant);

                    ContactMapping.MapEntity(itemContactPM, newContact, isNewEntity);
                    contactRepository.Update(newContact);
                }

                if (isNewEntity)
                {
                    if (itemContactPM.SetAsPrimaryForCard)
                    {
                        entityPM.PrimaryContactId = itemContactPM.Id;
                        entityPM.PrimaryContactPhone = itemContactPM.BusinessPhone;
                        entityPM.PrimaryContactName = itemContactPM.EnglishName;
                    }
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

        public void Submit()
        {
            cardExternalCodeByCurrencyRepository.SubmitChanges();
        }
    }
}
