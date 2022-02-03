using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using System.Collections.Generic;
using System;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CardService
    {
        bool isNewEntity;
        private int tenant;
        public Card Poco { get; set; }
        private Contact loggedContact;
        private AddressRepository addressRepository;
        private CardPM entityPM;
        private ICommonDataContext objectContext;
        private CardRepository entityRepository;
        private ContactRepository contactRepository;
        GLAccountCardDataService gLAccountCardDataService;
        public CardService(ICommonDataContext objectContext, CardPM entityPM)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);

            this.GetLoggedContact();
        }
        public CardService(ICommonDataContext objectContext,int tenant)
        {

            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                this.loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);

            }
        }

        public void Create(CardPM entityPM)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.Create();
            RunStoredProcedures();

        }
        public void Create()
        {
            this.isNewEntity = true;
            
            if (string.IsNullOrEmpty(entityPM.Id))
            {
                this.entityPM.Id = IdCounter.GetNumber("Card", tenant).ToString();
            }

            entityPM.Code = CodeCounter.GetNumber("Card", entityPM.Tenant).ToString();

            this.Initialize();

            this.Poco = new Card();
            this.Poco.Id = this.entityPM.Id;
            this.Poco.Code = entityPM.Code;

            CardValidating.Validate(entityPM);
            CardTracing.Trace(entityPM, Poco, isNewEntity);
            CardMapping.MapEntity(entityPM, Poco, isNewEntity);

            this.GetCountryName();

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            if (entityPM.Addresses != null)
            {
                foreach (AddressPM addressPM in entityPM.Addresses)
                {
                    addressPM.Id = IdCounter.GetNumber("Address", entityPM.Tenant).ToString();
                    addressPM.CardId = this.Poco.Id;

                    Address newAddress = new Address()
                    {
                        Id = addressPM.Id,
                        CardId = addressPM.CardId,
                        Tenant = tenant
                    };

                    AddressMapping.MapEntity(addressPM, newAddress, isNewEntity);
                    addressRepository.Add(newAddress);
                }
            }
            AddCardKafkaQueueMessage();
        }
        public void Update(CardPM entityPM)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.Update();
            RunStoredProcedures();
        }

        public void Update()
        {
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleCard(entityPM.Id , tenant);
          
            this.Initialize();

            CardValidating.Validate(entityPM);

            this.CacheEntity();
            this.GetCountryName();

            CardTracing.Trace(entityPM, Poco, isNewEntity);
            CardMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            //UpdateGLaccountCardsDara();
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Card");

            if (entityPM.DisconectFromContact)
            {
                CardContactRepository CardContactRepository = new Simplog.Data.CommonDataModel.Repositories.CardContactRepository(objectContext);
                CardContact cardContact = CardContactRepository.GetCardContactByContactAndCard(entityPM.Id, entityPM.ContactId, entityPM.Tenant);
                AddDisconectFromContactKafkaQueueMessage(cardContact);
                CardContactRepository.Remove(cardContact);
                CardContactRepository.SubmitChanges();
            }
            if((entityPM.PartnerTypeId== PartnerTypes.Customer || entityPM.PartnerTypeId == PartnerTypes.Vendor || entityPM.PartnerTypeId == PartnerTypes.AccountingPartner) && entityPM.GLAccountId !=null)
            {
                HandleGLAccountCardData(entityPM.Id,entityPM.GLAccountId,entityPM.Tenant);              
            }

            AddCardKafkaQueueMessage();

        }

        public void HandleGLAccountCardData(string cardId,string glaccountId, int tenant)
        {
            if (glaccountId != null)
            {
                gLAccountCardDataService = new GLAccountCardDataService(cardId, glaccountId, tenant);
                if (gLAccountCardDataService.cardGLaccount != null)
                {
                    bool GlAccountCardDataExists = CheckIfGlAccountCardDataExists();
                    if (GlAccountCardDataExists)
                    {
                        gLAccountCardDataService.UpdateGLaccountCardsData();
                    }
                    else { gLAccountCardDataService.CreateGLaccountCardsDara(); }
                }
            }
        }
        private bool CheckIfGlAccountCardDataExists()
        {
                if (gLAccountCardDataService.gLAccountCardsDataPM == null)
                {
                    return false;
                }
                else return true;            
        }
        private void RunStoredProcedures()
        {

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            {
                RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
            }
        }



        private void Initialize()
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
        }

        private void CacheEntity()
        {
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
            }
        }
        private void GetCountryName()
        {
            if (entityPM.Addresses != null)
            {
                AddressPM add = entityPM.Addresses.Where(a => a.AddressTypeId == "M").FirstOrDefault();
                if (add != null)
                {
                    this.Poco.CityName = add.City;
                    this.Poco.Address1 = add.Address1;
                    this.Poco.Address2 = add.Address2;
                    this.Poco.Phone = add.PhoneNumber;
                    this.Poco.ZipCode = add.ZipCode;

                    if (add.CountryId != null)
                    {
                        Country country = CountryRepository.GetSingleCountry(add.CountryId, entityPM.Tenant, true);
                        if (country != null)
                        {
                            this.Poco.CountryName = country.EnglishName;
                        }
                    }

                    if (add.StateId != null)
                    {
                        StateRepository stateRepository = new StateRepository(objectContext);
                        State state = stateRepository.GetSingleState(add.StateId, entityPM.Tenant);
                        if (state != null)
                        {
                            this.Poco.StateName = state.EnglishName;
                        }
                    }
                }
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
                { "EntityId", "{" + "\"ContactId\":" + "\"" + cardContact.ContactId + "\"," + "\"CardId\":" + "\"" + cardContact.CardId + "\"," + "\"Tenant\":" + 100 + "}" },
                { "Tenant", tenant.ToString()}};
                queueservice.Send(queueMessage, tenant);
            }
        }

        private void AddCardKafkaQueueMessage()
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
                { "Entity", "Card" },
                { "EntityId", entityPM.Id },
                { "Tenant", tenant.ToString()}};
            queueservice.Send(queueMessage, tenant);
        }
    }

    public static class PartnerTypes
    {

        public static string Customer = "CS";
        public static string Vendor = "VD";
        public static string AccountingPartner = "AC";

    }
}

