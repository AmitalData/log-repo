using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomsShipperService
    {
        bool isNewEntity;
        private int tenant;
        private CustomsShipperPM entityPM;
        public CustomsShipper entityPOCO { get; set; }
        private Card entityCard;
        private Contact loggedContact;
        private CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository;
        private ICommonDataContext objectContext;
        private CustomsShipperRepository entityRepository;
        private CardRepository cardRepository;
        private CardQuery cardQuery;
        private ContactRepository contactRepository;
        public CustomsShipperService(ICommonDataContext objectContext, int tenant)
        {

            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CustomsShipperRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardQuery = new CardQuery(cardRepository);
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
            this.entityPM = entityPM;
      
                this.isNewEntity = true;

                entityPM.Id = IdCounter.GetNumber("Card", entityPM.Tenant).ToString();
                entityPM.Tenant = tenant;

                this.entityCard = new Card()
                {
                    Id = entityPM.Id,
                    Tenant = tenant,
                    PartnerTypeId = "CH",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CreatedByUserId = loggedContact.Id,
                    UpdatedByUserId = loggedContact.Id,
                };

                this.entityPOCO = new CustomsShipper()
                {
                    Id = entityPM.Id,
                    Tenant = tenant,
                };

                CustomsShipperMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);


                cardRepository.Add(entityCard);
                entityRepository.Add(entityPOCO);
                entityRepository.SubmitChanges();
                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CustomsShipper");
  
         
        }

        public void Update(CustomsShipperPM entityPM, bool mapComposition = false)
        {
            this.entityPM = entityPM;
            CardPM c = cardQuery.GetSingleCarrierCard(entityPM.Id, entityPM.Tenant, false);
            
                this.isNewEntity = false;

                this.entityPOCO = entityRepository.GetSingleCustomsShipper(entityPM.Id, tenant);
                this.entityCard = cardRepository.GetSingleCard(entityPM.Id, entityPM.Tenant);

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


                entityCard.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityCard.UpdatedByUserId = loggedContact.Id;

                CustomsShipperMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);

                cardRepository.Update(entityCard);
                entityRepository.Update(entityPOCO);
                entityRepository.SubmitChanges();
                cardRepository.SubmitChanges();
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "CustomsShipper");

            

        }


    

     
    }
}
