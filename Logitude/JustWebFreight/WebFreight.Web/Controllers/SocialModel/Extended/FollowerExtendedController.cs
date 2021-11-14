using Logitude.Server.Tools.Counters;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityLists;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.SocialModel.Extended
{
    public class FollowerExtendedController : ApiController
    {
        public HttpResponseMessage GetAddDeleteFollower(string followeeUserId, string followerUserId,bool isDelete,int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);


            if (!isDelete)
            {
                FollowerPM entityPm = new FollowerPM()
                {
                    FolloweeUserId = followeeUserId,
                    FollowerUserId = followerUserId,
                    Tenant = tenant,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                };

                entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                FollowerUpdateService service = new FollowerUpdateService(socialContext, new Dictionary<string, IContext>(), entityPm.Tenant);
                service.Update(entityPm, true);

            }
            else
            {
                FollowerRepository followerRepository = new FollowerRepository(tenant);
                FollowerKeys followerKeys = new FollowerKeys() { FolloweeUserId = followeeUserId, FollowerUserId = followerUserId };
                Follower follower = followerRepository.GetSingle(followerKeys);

                if (follower != null)
                {
                    followerRepository.Remove(follower);
                    followerRepository.SubmitChanges();
                }

            }

            return Request.CreateResponse(HttpStatusCode.OK, "");

        }

        public HttpResponseMessage GetAddFollowEntity(string entityid, string objecttableid, string followerUserId)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);

            FollowEntityPM followEntitypm = new FollowEntityPM()
            {
                Id = IdCounter.GetNumber("FollowEntity", authToken.Tenant),
                EntityId = entityid,
                ObjectTableId = objecttableid,
                FollowerUserId = followerUserId,
                Tenant = authToken.Tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(authToken.Tenant),
                IsCancelled = false,


            };

       
            followEntitypm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            FollowEntityUpdateService service = new FollowEntityUpdateService(socialContext, new Dictionary<string, IContext>(), followEntitypm.Tenant);
            service.Update(followEntitypm, true);


            return Request.CreateResponse(HttpStatusCode.OK, followEntitypm);

        }


        public HttpResponseMessage GetDeleteFollowEntity(string userid)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);



            FollowEntityRepository followEntityRepository = new FollowEntityRepository(authToken.Tenant);

            List<FollowEntity> list = followEntityRepository.GetFollowEntitiesByUserId(userid, authToken.Tenant).ToList();
            foreach (FollowEntity item in list)
            {
                followEntityRepository.Remove(item);
            }

            followEntityRepository.SubmitChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        public HttpResponseMessage  GetUserFollowEntityLists(string entityid, string objecttableid)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);


            FollowEntityListQueryService listService = new FollowEntityListQueryService(socialContext);
            List<FollowEntityList>followEntityLists=  listService.GetListUserFollowEntity(entityid, objecttableid, authToken.Tenant);


            return Request.CreateResponse(HttpStatusCode.OK, followEntityLists);
        }

    }
}