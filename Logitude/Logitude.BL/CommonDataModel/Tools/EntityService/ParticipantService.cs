using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.CustomFields;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ParticipantService
    {
        bool isNewEntity;
        private int tenant;
        public Participant entityPOCO { get; set; }
        private Card entityCard;
        private ParticipantPM entityPM;
        private Contact loggedContact;
        private CardRepository cardRepository;
        private ParticipantRepository entityRepository;
        private AddressRepository addressRepository;
        private ContactRepository contactRepository;
        private CardContactRepository cardContactRepository;
        private ICommonDataContext objectContext;

        public ParticipantService(ICommonDataContext objectContext, int tenant)
        {
            
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ParticipantRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardContactRepository = new CardContactRepository(objectContext);
            this.GetLoggedContact();
        }

        public ParticipantService(ICommonDataContext objectContext, ParticipantPM entityPM, string loggedContactId)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ParticipantRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardContactRepository = new CardContactRepository(objectContext);
            this.loggedContact = contactRepository.GetSingleContact(loggedContactId, tenant);
        }

        private void GetLoggedContact()
        {
            string email = HttpContext.Current.User.Identity.Name;
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }

        public void Create(ParticipantPM entityPM)
        {
            this.entityPM = entityPM;
            this.isNewEntity = true;

            bool exist = this.IsEntityExists();

            if (exist)
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                msg = msg.Replace("%Entity", "Participant");
                //throw new ApplicationException(msg);
            }

            else
            {
                this.entityPM.Id = IdCounter.GetNumber("Card", entityPM.Tenant).ToString();
                this.entityCard = new Card()
                {
                    Id = entityPM.Id,
                    Tenant = tenant,
                    PartnerTypeId = "PT",
                };

                this.entityPOCO = new Participant()
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

                if (!entityPM.IsHybrid)
                {
                    ParticipantTracing.Trace(entityPM, entityPOCO, isNewEntity);
                }

                ParticipantMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
                ParticipantValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);

                cardRepository.Add(entityCard);
                entityRepository.Add(entityPOCO);
                entityRepository.SubmitChanges();
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Participant", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<ParticipantPM> { entityPM }.Cast<object>().ToList() }).Update();

                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                if (!LogitudeSettings.IsCostomsDeploy)
                {
                    RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
                }
            }
        }

        public void Update(ParticipantPM entityPM)
        {
            this.entityPM = entityPM;
            this.isNewEntity = false;

             bool exist = this.IsEntityExists();

             if (exist)
             {
                 string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", tenant);
                 msg = msg.Replace("%Entity", "Country");
                 throw new ApplicationException(msg);
             }

             else
             {
                 this.entityPOCO = entityRepository.GetSingleParticipant(entityPM.Id, entityPM.Tenant);
                 this.entityCard = cardRepository.GetSingleCard(entityPM.Id, entityPM.Tenant);

                 this.InitializeComponent();


                if (entityPM.Registered != entityPOCO.Registered)
                 {
                     this.UpdateAirline();

                     entityPM.RegistrationUpdatedBy = loggedContact.EnglishName;
                 }

                 if (entityPM.Registered && !entityPOCO.Registered)
                 {
                     entityPM.RegistrationDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                 }

                 else
                 {
                     entityPM.RegistrationDate = null;
                 }

                 if (entityPM.TTY != entityPOCO.TTY)
                 {
                     this.UpdateForworderTenant();
                 }

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

                 if (!entityPM.IsHybrid)
                 {
                     ParticipantTracing.Trace(entityPM, entityPOCO, isNewEntity);
                 }

                 ParticipantMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);
                ParticipantValidating.Validate(entityPM, this.entityCard, objectContext, isNewEntity);

                cardRepository.Update(entityCard);
                 entityRepository.Update(entityPOCO);
                 entityRepository.SubmitChanges();
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "Participant", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<ParticipantPM> { entityPM }.Cast<object>().ToList() }).Update();

                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                if (!LogitudeSettings.IsCostomsDeploy)
                {
                    RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
                }
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

                if (!entityPM.IsHybrid)
                {
                    entityPM.Code = CodeCounter.GetNumber("Participant", entityPM.Tenant).ToString();
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

        private void CreateAddress(AddressPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("Address", tenant).ToString();
            itemPM.CardId = this.entityCard.Id;

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

            itemContactPM.CardId = this.entityCard.Id;

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

        private bool IsEntityExists()
        {
            bool myResult = false;

            if (isNewEntity)
            {
                myResult = (from a in entityRepository.GetParticipants(entityPM.Tenant)
                            where a.ForwarderTenant == entityPM.ForwarderTenant && a.Tenant == entityPM.Tenant
                            select a).Any();
            }

            else
            {
                myResult = (from a in entityRepository.GetParticipants(entityPM.Tenant)
                            where a.ForwarderTenant == entityPM.ForwarderTenant && a.Tenant == entityPM.Tenant && a.Id != entityPM.Id
                            select a).Any();
            }

            return myResult;
        }

        private void UpdateAirline()
        {
            TenantManagement airlineTenant = null;
            TenantManagement forwarderTenant = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                airlineTenant = tenantManagementRepository.GetSingleTenantManagement(tenant);
                forwarderTenant = tenantManagementRepository.GetSingleTenantManagement(entityPM.ForwarderTenant);

                scope.Complete();
            }

            if (airlineTenant != null)
            {
                AirlineRepository airlineRepository = new AirlineRepository(entityPM.ForwarderTenant);
                Airline airline = airlineRepository.GetSingleAirlineByCode(airlineTenant.TenantConnectedToAirlineCode, entityPM.ForwarderTenant);

                if (airline != null)
                {
                    airline.TTY = entityPM.TTY;

                    if (entityPM.Registered != entityPOCO.Registered)
                    {
                        airline.RegistrationUpdatedBy = loggedContact.EnglishName;
                    }

                    if (forwarderTenant.AWBMessagesCCSTypeCode == "GLSHK")
                    {
                        airline.IsGLSHKRegistered = entityPM.Registered;
                    }
                    else
                    {
                        airline.IsChampRegistered = entityPM.Registered;
                    }

                    airlineRepository.Update(airline);
                    airlineRepository.SubmitChanges();
                }
            }
        }

        private void UpdateForworderTenant()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement forwarderTenant = tenantManagementRepository.GetSingleTenantManagement(entityPM.ForwarderTenant);

                if (forwarderTenant != null)
                {
                    forwarderTenant.TTY = entityPM.TTY;

                    tenantManagementRepository.Update(forwarderTenant);
                    tenantManagementRepository.SubmitChanges();
                }

                scope.Complete();
            }
        }
    }
}
