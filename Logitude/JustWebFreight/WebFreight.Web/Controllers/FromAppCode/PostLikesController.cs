using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class PostLikesMobileController : ApiController
    {
        public string PostInsertPostLike(PostLikePM likePM)
        {

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnEntityTenant("PostLike", likePM.Tenant, authToken.Tenant);


            ISocialContext socialContext = SocialContext.GetContext(likePM.Tenant);
            PostLikeUpdateService postLikeUpdateService = new PostLikeUpdateService(socialContext);
            PostUpdateService service = new PostUpdateService(socialContext, new Dictionary<string, IContext>(), likePM.Tenant);
            //postlike.ChangeSetOp = ChangeSetOperation.Insert;


            PostQueryService postQueryService = new PostQueryService(likePM.Tenant);
            PostPM entityPM = postQueryService.GetSingle(likePM.PostId, false, false);
            entityPM.ChangeSetOp = ChangeSetOperation.Update;
            likePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            entityPM.PostLikes.Add(likePM);
            service.Update(entityPM, true);
            socialContext.SaveChanges();
            //PostUpdateService service = new PostUpdateService(socialContext, new Dictionary<string, IContext>(), Postpm.Tenant);
            //Postpm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            //foreach (PostLikePM postcomm in Postpm.PostLikes)
            //{
            //    postcomm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            //}
            //service.Update(Postpm, true);
            return null;
        }

        public string DeletePostLike(string PostId, string UserId, int tenant)
            
        {

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);


            ISocialContext socialContext = SocialContext.GetContext(tenant);
            PostUpdateService service = new PostUpdateService(socialContext, new Dictionary<string, IContext>(), tenant);
            PostQueryService postQueryService = new PostQueryService(socialContext);

            PostPM entityPM = postQueryService.GetSingle(PostId, true, false);
            entityPM.ChangeSetOp = ChangeSetOperation.Update;
            PostLikeQueryService query = new PostLikeQueryService(socialContext);
            PostLikePM likepm = query.GetSingle(PostId, UserId, false, false);
            likepm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            entityPM.DeletedPostLikes.Add(likepm);
            likepm.ChangeSetOp = ChangeSetOperation.Delete;
            service.Update(entityPM, true);
            socialContext.SaveChanges();

            return null;
        }
     
    }
}