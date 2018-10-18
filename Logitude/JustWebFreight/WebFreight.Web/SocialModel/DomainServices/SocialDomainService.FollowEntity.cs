using Logitude.Server.Tools.Counters;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityLists;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.SocialModel.DomainServices
{
    public partial class SocialDomainService
    {
        public FollowEntityPM GetSingleFollowEntityPM(string id,  int tenant)
        {
            socialContext = SocialContext.GetContext(tenant);
            followEntityQuery = new FollowEntityQueryService(socialContext);
            FollowEntityPM followEntity = followEntityQuery.GetSingle(id,  false, false);
            return followEntity;
        }

        public FollowEntityList GetSingleFollowEntityList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowEntityListQueryService listService = new FollowEntityListQueryService(socialContext);
            return listService.GetSingle(id);
        }

        public List<FollowEntityList> GetFollowEntityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowEntityListQueryService listService = new FollowEntityListQueryService(socialContext);
            return listService.GetList(tenant);
        }

        public List<FollowEntityList> GetFollowEntitysFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowEntityListQueryService listService = new FollowEntityListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetFollowEntityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowEntityListQueryService queryService = new FollowEntityListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertFollowEntity(FollowEntityPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            FollowEntityUpdateService service = new FollowEntityUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateFollowEntity(FollowEntityPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            FollowEntityUpdateService service = new FollowEntityUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }



        public List<FollowEntityList> GetUserFollowEntityLists( string entityid ,string objecttableid , int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowEntityListQueryService listService = new FollowEntityListQueryService(socialContext);
            return listService.GetListUserFollowEntity(entityid, objecttableid, tenant);
        }
        [Invoke]
        public void AddFollowEntity(string entityid , string objecttableid  ,  string followerUserId, int tenant )
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            FollowEntityPM followEntitypm = new FollowEntityPM()
            {
                Id = IdCounter.GetNumber("FollowEntity", tenant),
                EntityId = entityid,
                ObjectTableId =objecttableid,
                FollowerUserId = followerUserId,
                Tenant = tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                IsCancelled =false,
                
                
            };

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(followEntitypm.Tenant);
            }

            followEntitypm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            FollowEntityUpdateService service = new FollowEntityUpdateService(socialContext, new Dictionary<string, IContext>(), followEntitypm.Tenant);
            service.Update(followEntitypm, true);

        }


        [Invoke]
        public void DeleteFollowEntity(string userid, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            FollowEntityRepository followEntityRepository = new FollowEntityRepository(tenant);

            List<FollowEntity> list = followEntityRepository.GetFollowEntitiesByUserId(userid, tenant).ToList();
            foreach (FollowEntity item in list)
            {
                followEntityRepository.Remove(item);
            }

            followEntityRepository.SubmitChanges();
        }
    }
}