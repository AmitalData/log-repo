using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.GlobalModel;
using Logitude.BL.Helpers;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.BL.GlobalModel.Tools.Validating;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.QueueService;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ContactService
    {
        bool isNewEntity;
        private int tenant;
        public Contact Poco { get; set; }
        private ContactPM entityPM;
        private ICommonDataContext objectContext;        
        private RoleRepository roleRepository;
        private ContactRepository entityRepository;
        private CardContactRepository CardContactRepository;
        private ContactTenantRepository contactTenantRepository;
        private ContactTenantRoleRepository contactTenantRoleRepository;        
        private bool isConnectedToCard = false;
        private Contact oldSimilarContact = null;
        private List<CardContact> allCardContact;
        private CardContactAdditionalServiceRepository cardContactAdditionalServiceRepository;
        private CardContactProductRepository cardContactProductRepository;
        public ContactService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.roleRepository = new RoleRepository(objectContext);
            this.entityRepository = new ContactRepository(objectContext);
            this.CardContactRepository = new CardContactRepository(objectContext);
            this.contactTenantRepository = new ContactTenantRepository(objectContext);
            this.contactTenantRoleRepository = new ContactTenantRoleRepository(objectContext);
            this.allCardContact = new List<CardContact>();
            this.cardContactAdditionalServiceRepository = new CardContactAdditionalServiceRepository(objectContext);
            this.cardContactProductRepository = new CardContactProductRepository(objectContext);
        }

        public void Create(ContactPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;

            this.Initialize();
            this.ValidateContactExists();

            if (entityPM.OldSimilarInactiveContactId != null)
            {
                this.entityPM.Id = entityPM.OldSimilarInactiveContactId;
                this.ConnectOldSimilar();
            }

            else
            {
                if (this.oldSimilarContact == null)
                {
                    this.Poco = new Contact()
                    {
                        Id = IdCounter.GetNumber("Contact", entityPM.Tenant).ToString(),
                        UserType = "R",
                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                        UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    };

                    this.entityPM.Id = this.Poco.Id;

                    Random _Random = new Random();
                    Poco.IndexColor = _Random.Next(1, 20);

                    if (!entityPM.IsHybrid)
                    {
                        ContactTracing.Trace(entityPM, Poco, isNewEntity);
                    }

                    this.InitializeCustomerCard();
                    this.InitializeContactTenant();
                    this.InitializeGlobalContact();

                    ContactMapping.MapEntity(entityPM, Poco, isNewEntity);
                    entityRepository.Add(Poco);
                    entityRepository.SubmitChanges();

                    this.InitializePrimaryContact(this.entityPM.Id);
                    this.InitializeNewCardContact(this.entityPM.Id);

                    this.ComputeCompanyName();

                    ContactMapping.MapEntity(entityPM, Poco, isNewEntity);
                    entityRepository.Update(Poco);
                    entityRepository.SubmitChanges();

                    TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Contact");
                }

                else if (isConnectedToCard)
                {
                    this.entityPM.Id = this.oldSimilarContact.Id;
                    this.ConnectOldSimilar();
                }
            }

            AddContactKafkaQueueMessage();
        }

        private void ConnectOldSimilar()
        {
            this.InitializePrimaryContact(this.entityPM.Id);
            this.InitializeNewCardContact(this.entityPM.Id);

            ContactService newContactService = new ContactService(this.objectContext, this.tenant);
            newContactService.Update(entityPM);
        }
        internal string DisableOldContact(string disableOldContactId, int tenant)
        {
            //update contacts  set inactive=1, computedkey  = id  where id='1-10622'
            var oldContact = entityRepository.GetSingleContactForUpdate(disableOldContactId, tenant);
            oldContact.InActive = true;
            oldContact.Email = (disableOldContactId + oldContact.Email) ?? "";
            oldContact.Email = oldContact.Email.Substring(0, Math.Min(70, oldContact.Email.Length));
            //oldContact.ComputedKey = disableOldContactId;
            string SaveExternalId = oldContact.ExternalId;
            oldContact.ExternalId = "-" + oldContact.ExternalId;//UPDATE  CONTACTS SET   externalid =NULL  WHERE   ID IN ('1-10628') AND inactive =1

            entityRepository.Update(oldContact);
            entityRepository.SubmitChanges();
            return SaveExternalId;


        }
        public void Update(ContactPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;

            this.Initialize();
            this.ValidateContactExists();

            this.Poco = entityRepository.GetSingleContact(entityPM.Id, entityPM.Tenant);

            if (!entityPM.IsHybrid)
            {
                ContactTracing.Trace(entityPM, Poco, isNewEntity);
            }

            this.InitializeCacheManager();
            this.InitializeGlobalContact();

            if (this.isConnectedToCard)
            {
                CardContact myCardContact = this.allCardContact.Where(d => d.ContactId == entityPM.Id).FirstOrDefault();

                if (myCardContact != null)
                {
                    if (entityPM.DisconectFromCard)
                    {
                        foreach(CardContactAdditionalServicePM service in entityPM.CardContactAdditionalServices)
                        {
                            this.DeleteAdditionalService(service);
                        }

                        foreach (CardContactProductPM product in entityPM.CardContactProducts)
                        {
                            this.DeleteProduct(product);
                        }

                        AddDisconectFromContactKafkaQueueMessage(myCardContact);
                        CardContactRepository.Remove(myCardContact);
                        CardContactRepository.SubmitChanges();
                    }

                    else
                    {
                        this.UpdateAdditionalServicesCollection(entityPM.CardContactAdditionalServices, myCardContact);
                        this.UpdateProductsCollection(entityPM.CardContactProducts, myCardContact);
                        MapCardContactToContact(myCardContact, entityPM);
                        CardContactRepository.Update(myCardContact);
                    }
                }
            }

            this.ComputeCompanyName();

            if (entityPM.IsUser)
            {
                NumberOfUsersService numberOfUsersService = new NumberOfUsersService(this.entityPM, this.Poco);
                numberOfUsersService.Validate();
            }

            ContactMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Contact");
            AddContactKafkaQueueMessage();
        }

        private void Initialize()
        {
            if (entityPM.Id != null)
            {
                if (entityPM.Id.Contains(","))
                {
                    string[] ids = entityPM.Id.Split(',');
                    entityPM.Id = ids[0];
                }
            }

            if (!string.IsNullOrEmpty(entityPM.Email))
            {
                entityPM.Email = entityPM.Email.ToLower();
            }

            if (entityPM.Birthday != null)
            {
                entityPM.BirthDayOfYear = entityPM.Birthday.Value.DayOfYear;
            }

            if (entityPM.CardId != null && entityPM.CardId != "newCard")
            {
                this.isConnectedToCard = true;
                this.allCardContact = CardContactRepository.GetCardContactsByCardId(entityPM.CardId).ToList();
            }

            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
        }
        private void ValidateContactExists()
        {
            if (!string.IsNullOrEmpty(entityPM.Email))
            {
                List<Contact> allSimilarContacts = (from d in objectContext.Contacts
                                                    where d.Tenant == this.tenant
                                                    && d.UserType == "R"
                                                    && d.InActive == false
                                                    && d.Email.ToLower() == entityPM.Email.ToLower()
                                                    select d).ToList();

                if (allSimilarContacts.Count > 0)
                {
                    if (this.isNewEntity)
                    {
                        this.oldSimilarContact = (from d in allSimilarContacts select d).FirstOrDefault();
                    }

                    else
                    {
                        this.oldSimilarContact = (from d in allSimilarContacts where d.Id != this.entityPM.Id select d).FirstOrDefault();
                    }

                    if (oldSimilarContact != null)
                    {
                        if (!isConnectedToCard)
                        {
                            string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", this.tenant);
                            msg = msg.Replace("%Entity", "Contact");
                            throw new Exception(msg);
                        }

                        else
                        {
                            bool isCardContactExists = (from a in this.allCardContact
                                                        where a.ContactId == oldSimilarContact.Id
                                                        select a).Any();

                            if (isCardContactExists)
                            {
                                string msg = TranslateTextsClass.Translate("Contact.M.ThisContactAlreadyAdded", entityPM.Tenant);
                                throw new Exception(msg);
                            }                            
                        }
                    }
                }
            }
        }
        private void InitializeContactTenant()
        {
            if (this.isNewEntity)
            {
                ContactTenant newContactTenant = new ContactTenant()
                {
                    Id = IdCounter.GetNumber("ContactTenant", entityPM.Tenant).ToString(),
                    TenantId = entityPM.Tenant,
                    ContactId = entityPM.Id,
                };

                contactTenantRepository.Add(newContactTenant);

                if (entityPM.SignupRole)
                {
                    RoleQuery roleQuery = new RoleQuery(roleRepository);
                    RolePM role = roleQuery.GetSinglePMByName("Administrator", 0);

                    ContactTenantRole contactTenantRole = new ContactTenantRole()
                    {
                        ContactTenantId = newContactTenant.Id,
                        Id = IdCounter.GetNumber("ContactTenantRole", entityPM.Tenant).ToString(),
                        RoleId = role.Id,
                        Tenant = entityPM.Tenant,
                    };

                    contactTenantRoleRepository.Add(contactTenantRole);
                }
            }
        }
        private void InitializeCustomerCard()
        {
            if (this.isNewEntity)
            {
                if (!string.IsNullOrEmpty(entityPM.CustomerId))
                {
                    CardContact cardContact = new CardContact()
                    {
                        CardId = entityPM.CustomerId,
                        ContactId = entityPM.Id,
                        Id = IdCounter.GetNumber("CardContact", entityPM.Tenant).ToString(),
                        Tenant = entityPM.Tenant,
                    };

                    CardContactRepository.Add(cardContact);

                    CardRepository myCardRepository = new CardRepository(tenant);
                    Card myCard = myCardRepository.GetSingleCard(entityPM.CustomerId, tenant);

                    if (myCard != null)
                    {
                        UpdateCardSearchFields(myCard);
                        myCardRepository.Update(myCard);
                        myCardRepository.SubmitChanges();
                    }
                }
            }
        }
        private void UpdateCardSearchFields(Card myCard)
        {
            if (!string.IsNullOrEmpty(entityPM.Email))
            {
                if (string.IsNullOrEmpty(myCard.SearchFields))
                {
                    myCard.SearchFields = entityPM.Email;
                }

                else if (!myCard.SearchFields.Contains(entityPM.Email.ToLower()))
                {
                    myCard.SearchFields += "," + entityPM.Email;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.EnglishName))
            {
                if (string.IsNullOrEmpty(myCard.SearchFields))
                {
                    myCard.SearchFields = entityPM.EnglishName;
                }

                else if (!myCard.SearchFields.Contains(entityPM.EnglishName))
                {
                    myCard.SearchFields += "," + entityPM.EnglishName;
                }
            }

            if (myCard.SearchFields.Length > 1000)
            {
                myCard.SearchFields = myCard.SearchFields.Substring(0, 1000);
            }
        }
        private void InitializeGlobalContact()
        {
            if (this.isNewEntity)
            {
                if (!string.IsNullOrEmpty(entityPM.Email))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();

                        GlobalContactRepository globalContactRep = new GlobalContactRepository(globalContext);

                        bool globalContactExists = (from a in globalContactRep.GetGlobalContactByTenant(entityPM.Tenant)
                                                    where a.Email == entityPM.Email
                                                    select a).Any();
                        if (!globalContactExists)
                        {

                            GlobalContact conflictcontact = globalContactRep.GetSingleGlobalContact(entityPM.Id);
                            if (conflictcontact != null)
                            {
                                globalContactRep.Remove(conflictcontact);
                            }

                            GlobalContact gcontact = new GlobalContact() { Email = entityPM.Email, Id = entityPM.Id, GlobalTenantId = entityPM.Tenant, };

                            globalContactRep.Add(gcontact);
                            globalContactRep.SubmitChanges();
                            scope.Complete();
                        }
                    }

                }
            }


            else
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    GlobalContactRepository globalContactRepository = new GlobalContactRepository();
                    GlobalContact globalContact = globalContactRepository.GetSingleGlobalContact(this.entityPM.Id);
                    if (globalContact != null)
                    {
                        if (string.IsNullOrEmpty(this.entityPM.Email) && !string.IsNullOrEmpty(Poco.Email))
                        {
                            //if (globalContact.InternetAccess || globalContact.IsUser)
                            //{
                            string msg = "Sorry you can't update this record email because it's being used as a user or shared contact!";
                            throw new Exception(msg);
                            //}
                            //else
                            //{
                            //}
                        }
                        else
                        {
                            globalContact.Email = this.entityPM.Email;
                            globalContact.InActive = this.entityPM.InActive;
                            globalContactRepository.Update(globalContact);
                            globalContactRepository.SubmitChanges();
                        }

                    }
                    else if (!string.IsNullOrEmpty(entityPM.Email))
                    {
                        globalContact = new GlobalContact() { Email = entityPM.Email, Id = entityPM.Id, GlobalTenantId = entityPM.Tenant, };
                        globalContactRepository.Add(globalContact);
                        globalContactRepository.SubmitChanges();

                    }

                    //string storedEmail = !string.IsNullOrEmpty(this.Poco.Email) ? this.Poco.Email.ToLower() : this.Poco.Email;
                    string pmEmail = !string.IsNullOrEmpty(this.entityPM.Email) ? this.entityPM.Email.ToLower() : this.entityPM.Email;
                    if (Poco.Email != pmEmail)
                    {
                        IGlobalContext globalContext = GlobalContext.GetContext();

                        if (!globalContext.ContactPasswords.Where(c => c.Email == entityPM.Email).Any() && globalContext.ContactPasswords.Where(c => c.Email == Poco.Email.ToLower()).Any())
                        {
                            ContactPassword contactPassword = new ContactPassword()
                            {
                                Email = entityPM.Email,
                                Password = "123",
                                IsLocked = false,
                                NumberOfRetries = 0,
                                MustChangePassword = false,

                            };

                            globalContext.ContactPasswords.Add(contactPassword);
                            globalContext.SaveChanges();
                        }
                    }

                    scope.Complete();
                }
            }
        }
        private void InitializeCacheManager()
        {
            if (!this.isNewEntity)
            {
                if (CacheManager.CacheWrapper != null)
                {
                    string entityName = "User" + entityPM.Id + entityPM.Tenant;
                    string entityPmName = "UserPM" + entityPM.Id + entityPM.Tenant;
                    entityName = entityName.ToLower();
                    entityPmName = entityPmName.ToLower();

                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityName);
                    }
                    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityPmName);
                    }

                    string entityNameContact = "Contact" + entityPM.Id + entityPM.Tenant;
                    string entityPmNameContact = "ContactPM" + entityPM.Id + entityPM.Tenant;

                    entityNameContact = entityNameContact.ToLower();
                    entityPmNameContact = entityPmNameContact.ToLower();
                    if (CacheManager.CacheWrapper.Get(entityNameContact) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityNameContact);
                    }
                    if (CacheManager.CacheWrapper.Get(entityPmNameContact) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityPmNameContact);
                    }

                    entityName = "ContactPM" + entityPM.Email + entityPM.Tenant;
                    entityName = entityName.ToLower();
                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityName);
                    }
                }
            }
        }
        private void InitializePrimaryContact(string myContactId)
        {
            if (this.isConnectedToCard)
            {
                if (this.allCardContact.Count == 0 || entityPM.SetAsPrimaryForCard)
                {
                    CardQuery cardQuery = new CardQuery(entityPM.Tenant);
                    CardPM card = cardQuery.GetSinglePM(entityPM.CardId, entityPM.Tenant);
                    card.PrimaryContactId = myContactId;

                    CardService cardService = new CardService(objectContext, card);
                    cardService.Update();
                }
            }
        }
        private void InitializeNewCardContact(string myContactId)
        {
            if (this.isConnectedToCard)
            {
                CardContact cardContact = new CardContact()
                {
                    CardId = entityPM.CardId,
                    ContactId = myContactId,
                    Id = IdCounter.GetNumber("CardContact", entityPM.Tenant).ToString(),
                    Tenant = entityPM.Tenant,
                    IsAll = entityPM.IsAll,
                    IsAirImport = entityPM.IsAirImport,
                    IsAirExport = entityPM.IsAirExport,
                    IsCustomsImport = entityPM.IsCustomsImport,
                    IsInlandDomestic = entityPM.IsInlandDomestic,
                    IsInlandExport = entityPM.IsInlandExport,
                    IsInlandImport = entityPM.IsInlandImport,
                    IsOceanExport = entityPM.IsOceanExport,
                    IsOceanImport = entityPM.IsOceanImport,
                };

                foreach (CardContactAdditionalServicePM item in entityPM.CardContactAdditionalServices)
                {
                    this.CreateAdditionalService(item, cardContact);
                }
                
                CardContactRepository.Add(cardContact);
                CardContactRepository.SubmitChanges();
            }
        }
        private void ComputeCompanyName()
        {
            string myResult = null;

            List<Card> allCards = (from d in objectContext.CardContacts.Include("Card")
                                   where d.ContactId == this.Poco.Id
                                   select d.Card).ToList();

            if (allCards.Count > 0)
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

                if (myResult != null)
                {
                    if (myResult.Length > 1000)
                    {
                        myResult = myResult.Substring(0, 1000);
                    }
                }
            }

            entityPM.CompanyName = myResult;
        }
        private void MapCardContactToContact(CardContact cardContact, ContactPM contactPM)
        {
            cardContact.IsAll = contactPM.IsAll;
            cardContact.IsAirExport = contactPM.IsAirExport;
            cardContact.IsAirImport = contactPM.IsAirImport;
            cardContact.IsInlandExport = contactPM.IsInlandExport;
            cardContact.IsInlandImport = contactPM.IsInlandImport;
            cardContact.IsOceanExport = contactPM.IsOceanExport;
            cardContact.IsOceanImport = contactPM.IsOceanImport;
            cardContact.IsCustomsImport = contactPM.IsCustomsImport;
            cardContact.IsInlandDomestic = contactPM.IsInlandDomestic;
        }

        private void UpdateAdditionalServicesCollection(List<CardContactAdditionalServicePM> servicePMs, CardContact cardContact)
        {
            if (servicePMs != null)
            {
                foreach (CardContactAdditionalServicePM itemPM in servicePMs)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateAdditionalService(itemPM, cardContact);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateAdditionalService(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteAdditionalService(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void CreateAdditionalService(CardContactAdditionalServicePM itemPM, CardContact cardContact)
        {
            itemPM.Id = IdCounter.GetNumber("CardContactAdditionalService", tenant).ToString();
            itemPM.CardContactId = cardContact.Id;
            itemPM.Tenant = tenant;

            CardContactAdditionalService itemPoco = new CardContactAdditionalService()
            {
                Id = itemPM.Id,
                CardContactId = itemPM.CardContactId,
                AdditionalServiceId = itemPM.AdditionalServiceId,
                Tenant = tenant
            };

            CardContactAdditionalServiceMapping.MapEntity(itemPM, itemPoco, true);
            cardContactAdditionalServiceRepository.Add(itemPoco);
        }
        private void UpdateAdditionalService(CardContactAdditionalServicePM itemPM)
        {
            CardContactAdditionalService itemPoco = cardContactAdditionalServiceRepository.GetSingleCardContactAdditionalService(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                CardContactAdditionalServiceMapping.MapEntity(itemPM, itemPoco, false);
                cardContactAdditionalServiceRepository.Update(itemPoco);
            }
        }
        private void DeleteAdditionalService(CardContactAdditionalServicePM itemPM)
        {
            CardContactAdditionalService itemPoco = cardContactAdditionalServiceRepository.GetSingleCardContactAdditionalService(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                cardContactAdditionalServiceRepository.Remove(itemPoco);
            }
        }

        private void UpdateProductsCollection(List<CardContactProductPM> productPMs, CardContact cardContact)
        {
            if (productPMs != null)
            {
                foreach (CardContactProductPM itemPM in productPMs)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateProduct(itemPM, cardContact);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteProduct(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void CreateProduct(CardContactProductPM itemPM, CardContact cardContact)
        {
            itemPM.Id = IdCounter.GetNumber("CardContactProduct", tenant).ToString();
            itemPM.CardContactId = cardContact.Id;
            itemPM.Tenant = tenant;

            CardContactProduct itemPoco = new CardContactProduct()
            {
                Id = itemPM.Id,
                CardContactId = itemPM.CardContactId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant
            };

            CardContactProductMapping.MapEntity(itemPM, itemPoco, true);
            cardContactProductRepository.Add(itemPoco);
        }
        private void UpdateProduct(CardContactProductPM itemPM)
        {
            CardContactProduct itemPoco = cardContactProductRepository.GetSingleCardContactProduct(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                CardContactProductMapping.MapEntity(itemPM, itemPoco, false);
                cardContactProductRepository.Update(itemPoco);
            }
        }
        private void DeleteProduct(CardContactProductPM itemPM)
        {
            CardContactProduct itemPoco = cardContactProductRepository.GetSingleCardContactProduct(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                cardContactProductRepository.Remove(itemPoco);
            }
        }

        private void AddDisconectFromContactKafkaQueueMessage(CardContact cardContact)
        {
            if (FeatureToggleHelper.HasFeatureToggle("CTL", entityPM.Tenant))
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("CToolLookups", 0);
                var queueMessage = new Dictionary<string, string>() {
                { "Entity", "DisconectFromContact" },
                { "EntityId", "{" + "\"ContactId\":" + "\"" + cardContact.ContactId + "\"," + "\"CardId\":" + "\"" + cardContact.CardId + "\"," + "\"Tenant\":" + tenant + "}" },
                { "Tenant", tenant.ToString()}};
                queueservice.Send(queueMessage, tenant);
            }
        }

        private void AddContactKafkaQueueMessage()
        {
            if (!FeatureToggleHelper.HasFeatureToggle("CTL", entityPM.Tenant))
            {
                return;
            }
            AddKafkaQueueMessage();
        }

        private void AddKafkaQueueMessage()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("CToolLookups", 0);
            var queueMessage = new Dictionary<string, string>() {
                { "Entity", "Contact" },
                { "EntityId", entityPM.Id },
                { "Tenant", tenant.ToString()}};
            queueservice.Send(queueMessage, tenant);
        }
    }
}
