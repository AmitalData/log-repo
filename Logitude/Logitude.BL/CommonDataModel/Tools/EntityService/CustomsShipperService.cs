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
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools.CustomFields;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomsShipperService
    {
        bool isNewEntity;
        private int tenant;
        public CustomsShipper entityPOCO { get; set; }
        private Card entityCard;
        private CustomsShipperPM entityPM;
        private Contact loggedContact;
        private CardRepository cardRepository;
        private CustomsShipperRepository entityRepository;
        private AddressRepository addressRepository;
        private ContactRepository contactRepository;
        private CardContactRepository cardContactRepository;
        private ICommonDataContext objectContext;
        private CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository;
        public CustomsShipperService(ICommonDataContext objectContext, int tenant)
        {

            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CustomsShipperRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardContactRepository = new CardContactRepository(objectContext);
            this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(objectContext);
            this.GetLoggedContact();
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

        public void Create(CustomsShipperPM entityPM)
        {




            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.entityPM = entityPM;
                this.isNewEntity = true;
                this.entityPM.Id = IdCounter.GetNumber("Card", tenant).ToString();

                this.entityCard = new Card()
                {
                    Id = entityPM.Id,
                    Tenant = tenant,
                    PartnerTypeId = "SG",
                };

                this.entityPOCO = new CustomsShipper()
                {
                    Id = entityPM.Id,
                    Tenant = tenant,
                };

                this.InitializeComponent();

                foreach (AddressPM itemPM in entityPM.Addresses)
                {
                    this.CreateAddress(itemPM);
                }

                CustomsShipperMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);

                cardRepository.Add(entityCard);
                entityRepository.Add(entityPOCO);
                entityRepository.SubmitChanges();
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "CustomsShipper", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<CustomsShipperPM> { entityPM }.Cast<object>().ToList() }).Update();

                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                if (!LogitudeSettings.IsCostomsDeploy)
                {
                    RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
                }
                scope.Complete();
            }
   
        }

        public void Update(CustomsShipperPM entityPM, bool mapComposition = false)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.entityPM = entityPM;
                this.isNewEntity = false;

                this.entityPOCO = entityRepository.GetSingleCustomsShipper(entityPM.Id, tenant);
                this.entityCard = cardRepository.GetSingleCard(entityPM.Id, entityPM.Tenant);

                this.InitializeComponent();

                //if (CacheManager.CacheWrapper != null)
                //{
                //    string entityName = "Card" + entityPM.Id + entityPM.Tenant;
                //    string entityPmName = "CardPM" + entityPM.Id + entityPM.Tenant;

                //    if (CacheManager.CacheWrapper.Get(entityName) != null)
                //    {
                //        CacheManager.CacheWrapper.Invalidate(entityName);
                //    }

                //    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                //    {
                //        CacheManager.CacheWrapper.Invalidate(entityPmName);
                //    }
                //}

                CustomsShipperMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);

                cardRepository.Update(entityCard);
                entityRepository.Update(entityPOCO);
                entityRepository.SubmitChanges();
                new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "CustomsShipper", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<CustomsShipperPM> { entityPM }.Cast<object>().ToList() }).Update();

                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CustomsShipper");
                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Card");
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                if (!LogitudeSettings.IsCostomsDeploy)
                {
                    RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
                }
                scope.Complete();
            }
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdateDate = entityPM.CreateDate;

                if (loggedContact != null)
                {
                    entityPM.CreatedByUserId = loggedContact.Id;
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }

                entityPM.Code = CodeCounter.GetNumber("CustomsShipper", tenant).ToString();
                
            }

            else
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                if (loggedContact != null)
                {
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }
            }

            this.InitializeCardFields();
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



      
    }
}
