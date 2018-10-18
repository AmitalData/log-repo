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
        public FollowerPM GetSingleFollowerPM(string followeeuserid, string followeruserid, int tenant)
        {
            socialContext = SocialContext.GetContext(tenant);
            followerQuery = new FollowerQueryService(socialContext);
            FollowerPM follower = followerQuery.GetSingle(followeeuserid, followeruserid, false, false);
            return follower;
        }

        public FollowerList GetSingleFollowerList(string followeeuserid, string followeruserid, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowerListQueryService listService = new FollowerListQueryService(socialContext);
            return listService.GetSingle(followeeuserid, followeruserid);
        }

        public List<FollowerList> GetFollowerLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowerListQueryService listService = new FollowerListQueryService(socialContext);
            return listService.GetList(tenant);
        }

       [Invoke]
        public int GetFllowerUser(string createdById, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowerListQueryService listService = new FollowerListQueryService(socialContext);
            return listService.UserFllowersCount(createdById, tenant);
        }


        public List<FollowerList> GetFollowersFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowerListQueryService listService = new FollowerListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetFollowerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(tenant);
            }

            FollowerListQueryService queryService = new FollowerListQueryService(socialContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertFollower(FollowerPM entityPm)
        {
            
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            FollowerUpdateService service = new FollowerUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public void UpdateFollower(FollowerPM entityPm)
        {
             
            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            FollowerUpdateService service = new FollowerUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }


       [Invoke]
        public void DeleteFollower(string followeeUserId, string followerUserId, int tenant)
        {
            FollowerRepository followerRepository = new FollowerRepository(tenant);
            FollowerKeys followerKeys = new FollowerKeys() {FolloweeUserId = followeeUserId, FollowerUserId = followerUserId };
            Follower follower = followerRepository.GetSingle(followerKeys);

            if (follower != null)
            {
                followerRepository.Remove(follower);
                followerRepository.SubmitChanges();
            }
        }

       [Invoke]
       public void AddFollower(string followeeUserId, string followerUserId, int tenant)
       {


           FollowerPM entityPm = new FollowerPM()
           {
               FolloweeUserId = followeeUserId,
               FollowerUserId = followerUserId,
               Tenant = tenant,
               CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
           };

           if (socialContext == null)
           {
               socialContext = SocialContext.GetContext(entityPm.Tenant);
           }

           entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
           FollowerUpdateService service = new FollowerUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
           service.Update(entityPm, true);
           
       }

    }
}