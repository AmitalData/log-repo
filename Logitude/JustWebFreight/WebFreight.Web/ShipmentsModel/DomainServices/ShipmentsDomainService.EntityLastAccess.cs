using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.Security;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.BL.InfrastructureModel;
using Logitude.BL.DataContracts;
namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private EntityLastActivity InsertEntityLastAccess(string entityId, string objectTableName, string userId, int tenant)
        {
            //TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantEntity = TenantRepository.GetSingleTenant(tenant, true);

            EntityLastActivityRepository entityLastAccessRepository = new EntityLastActivityRepository(tenant);
            //ObjectTabelRepository objectTableRep = new ObjectTabelRepository(tenant);
            ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
            EntityLastActivity entityLastAccess = new EntityLastActivity();
            entityLastAccess.Id = IdCounter.GetNumber("EntityLastAccess", tenant).ToString();
            entityLastAccess.EntityId = entityId;
            entityLastAccess.ObjectTableId = objectTable.Id;
            entityLastAccess.UserId = userId;
            entityLastAccess.Tenant = tenant;
            entityLastAccess.ActivityDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            entityLastAccessRepository.Add(entityLastAccess);
            entityLastAccessRepository.SubmitChanges();
            return entityLastAccess;
        }

        public EntityLastAccessPM CheckLastAccessUsers(string entityId, string objectTableName, DateTime currentUserAccessDate, int tenant)
        {
            EntityLastAccessQuery entityLastAccessRep = new EntityLastAccessQuery(tenant);

            //ObjectTabelRepository objectTableRep = new ObjectTabelRepository(tenant);
            ObjectTablePM objectTable = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);

            ContactQuery contactsRepository = new ContactQuery(tenant);
            ContactPM contact = contactsRepository.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);

            //EntityLastAccessPM CurrentUserLastAccess = entityLastAccessRep.GetEntityLastAccessByCurrentUser(objectTable.Id, EntityId,contact.Id, tenant);
            EntityLastAccessPM lastEntityAccess = entityLastAccessRep.GetEntityLastAccessByEntity(objectTable.Id, entityId, tenant);

            if (currentUserAccessDate.Day < lastEntityAccess.AccessDate.Day)
            {
                return lastEntityAccess;
            }
            if (currentUserAccessDate.Hour < lastEntityAccess.AccessDate.Hour)
            {
                return lastEntityAccess;
            }
            if (currentUserAccessDate.Minute < lastEntityAccess.AccessDate.Minute)
            {
                return lastEntityAccess;
            }
            if (currentUserAccessDate.Second < lastEntityAccess.AccessDate.Second)
            {
                return lastEntityAccess;
            }
            return null;
        }

        public EntityLastUpdatedByInfo CheckEntityModifications(string shipmentId, DateTime currentAccessDate, byte[] currentLastModified, int tenant)
        {
            shipmentRepository = new ShipmentRepository(tenant);
            shipmentQuery = new ShipmentQuery(shipmentRepository);
            ContactQuery contactsRepository = new ContactQuery(tenant);
            ContactPM contact = contactsRepository.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);

            byte[] lastModified = shipmentRepository.GetLastModifiedTimeStamp(shipmentId, tenant);

            bool equal = this.CompareTimeStamps(lastModified, currentLastModified);
            EntityLastUpdatedByInfo entityLastModifiedInfo;
            if (!equal)
            {
                entityLastModifiedInfo = shipmentQuery.GetLastUpdatedByInfo(shipmentId, tenant);
                return entityLastModifiedInfo;
            }
            return null;
        }

    }
}