using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;

using System.Net;
using System.Net.Http;
using System.Web.Http;

using Simplog.Server.Infrastructure.Azure;
using Logitude.Server.Tools.StorageService;
using Logitude.Social.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.Data.Repsitories;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityUpdateServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Transactions;
using Logitude.Social.Data.EntityListQueryServices;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.Controllers.SocialModel.Extended
{
    public class PostExtendedController : ApiController
    {
        public HttpResponseMessage PostFilteredPosts(PostFilters postFilter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);
                
                postFilter.Technology = "Angular";
                PostQueryService service = new PostQueryService(socialContext);
                List<PostPM> result = service.GetPostPMsByFilter(postFilter, true, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage PostGetCountPostPMsByFilter(PostFilters postFilter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);

                PostQueryService service = new PostQueryService(socialContext);
                int count = service.GetCountPostPMsByFilter(postFilter, true, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, count);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        

        public HttpResponseMessage GetSinglePostComment(string id, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);
            PostRepository postRepository = new PostRepository(tenant);
            PostQueryService service = new PostQueryService(socialContext);

            PostPM post = service.GetSingle(id, true, false);
            post.NumberOfLikes = postRepository.GetPostNumberOfLikes(id, tenant);
            post.PostComments = service.GetMulti(new Logitude.Social.Data.EntityKeys.PostKeys() { Id = id }, false).Where(p => p.IsCancelled == false).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, post);

        }

        public HttpResponseMessage GetSinglePost(string postid, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);
            PostRepository postRepository = new PostRepository(tenant);
            PostQueryService service = new PostQueryService(socialContext);

            PostPM post = service.GetSingle(postid, true, false);
            post.NumberOfLikes = postRepository.GetPostNumberOfLikes(postid, tenant);
            return Request.CreateResponse(HttpStatusCode.OK, post);
        }

        public HttpResponseMessage GetDeletePostLike(string postId, string userId, int tenant)
        {

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ISocialContext socialContext = SocialContext.GetContext(tenant);
            PostLikeRepository postLikeRepository = new PostLikeRepository(tenant);
            PostLike postLike = postLikeRepository.GetSingle(postId, userId, tenant);

            if (postLike != null)
            {
                postLikeRepository.Remove(postLike);
                postLikeRepository.SubmitChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK, "");
          
        }


        public HttpResponseMessage GetPostSummaryData(string userId,string loggedUserId , int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            ISocialContext socialContext = SocialContext.GetContext(authToken.Tenant);

            FollowerListQueryService followerListQueryService = new FollowerListQueryService(socialContext);
            PostLikeListQueryService postLikeListQueryService = new PostLikeListQueryService(socialContext);
 
            PostSummaryData postSummaryData = new PostSummaryData();
            postSummaryData.CoundFollowerUser = followerListQueryService.UserFllowersCount(userId, tenant);
            postSummaryData.Coundlikesreceived = postLikeListQueryService.Userlikesreceived(userId, tenant);
            postSummaryData.IsFollower = followerListQueryService.CheckIfFllowers(userId, loggedUserId, tenant);



            return Request.CreateResponse(HttpStatusCode.OK, postSummaryData);

        }

        public HttpResponseMessage PostInsertPostLike(PostLikePM likePM)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnEntityTenant("PostLike", likePM.Tenant, authToken.Tenant);

            ISocialContext socialContext = SocialContext.GetContext(likePM.Tenant);
            PostLikeRepository postLikeRepository = new PostLikeRepository(likePM.Tenant);
            PostLike postLike = new PostLike()
            {
                Tenant = likePM.Tenant,
                PostId = likePM.PostId,
                UserId = likePM.UserId,
                
            };

            postLikeRepository.Add(postLike);
            postLikeRepository.SubmitChanges();

            return Request.CreateResponse(HttpStatusCode.OK, likePM);
        }


        public HttpResponseMessage GetSocialContact(string id , int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(tenant);

                SecurityUtility.CheckContactFeature("Contact", "READ", tenant);
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contactPM = contactQuery.GetSingleContactPM(id);

                return Request.CreateResponse(HttpStatusCode.OK, contactPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


    }

    public class PostSummaryData
    {

        public int CoundFollowerUser { get; set; }
        public int Coundlikesreceived { get; set; }
        public bool IsFollower { get; set; }
    }

}