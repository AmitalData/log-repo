using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ShipmentSubTypeService
    {
        private int tenant;
        private bool isNewEntity;
        private Contact loggedContact;
        public ShipmentSubType entityPoco { get; set; }
        private ShipmentSubTypePM entityPM;
        private IShipmentsContext objectContext;
        private ShipmentSubTypeRepository entityRepository;
        public ShipmentSubTypeService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ShipmentSubTypeRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(tenant);
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }

        public void Create(ShipmentSubTypePM entity)
        {
            this.isNewEntity = true;
            this.entityPM = entity;
            this.entityPM.Id = IdCounter.GetNumber("ShipmentSubType", tenant).ToString();
            this.entityPoco = new ShipmentSubType()
            {
                Id = entityPM.Id,
                Tenant = entityPM.Tenant
            };

            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.CreatedByUserId = loggedContact.Id;
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.UpdatedByUserId = loggedContact.Id;
            entityPM.IsManuallyAdded = true;

            this.ValidateCode(entityPM, true);

            ShipmentTracing.Trace(entityPM, entityPoco, isNewEntity);
            ShipmentMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
        }

        public void Update(ShipmentSubTypePM entity)
        {
            this.isNewEntity = false;
            this.entityPM = entity;
            this.entityPoco = entityRepository.GetSingleShipmentSubType(entityPM.Id, entityPM.Tenant);

            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityPM.UpdatedByUserId = loggedContact.Id;

            this.ValidateCode(entityPM, false);

            ShipmentTracing.Trace(entityPM, entityPoco, isNewEntity);
            ShipmentMapping.MapEntity(entityPM, entityPoco, isNewEntity);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }

        private void ValidateCode(ShipmentSubTypePM entityPM, bool isNewEntity)
        {
            bool exist = false;
            if (isNewEntity)
            {
                exist = (from a in objectContext.ShipmentSubTypes
                         where a.Code.ToLower() == entityPM.Code.ToLower() && a.Tenant == entityPM.Tenant
                         select a).Any();
            }

            else
            {
                exist = (from a in objectContext.ShipmentSubTypes
                         where a.Code.ToLower() == entityPM.Code.ToLower()
                         && a.Id != entityPM.Id
                         && a.Tenant == entityPM.Tenant
                         select a).Any();
            }

            if (exist)
            {
                throw new ApplicationException("Code already exists");
            }
        }
    }
}
