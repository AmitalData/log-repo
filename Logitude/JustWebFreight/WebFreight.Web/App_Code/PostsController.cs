using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityQueryServices;
using Logitude.Social.BL.EntityUpdateServices;
using Logitude.Social.Data;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.Repsitories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class PostsMobileController : ApiController
    {

        public List<PostPM> PostFilteredPosts(int tenant,  PostFilters filters) 
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);

           

          //  QueryOperations queryOperations = new QueryOperations();
           // queryOperations.SetFilter("QueryName", filters.QueryName, false, "Equals", null, false);
 

           // ICRMContext crmContext = CRMContext.GetContext(tenant);
            ISocialContext socialContext = SocialContext.GetContext(tenant);

            PostQueryService service = new PostQueryService(socialContext);
            return service.GetPostPMsByFilter(filters, false,tenant);
        }

        public PostPM GetSinglePostComment(string id,int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);



            //  QueryOperations queryOperations = new QueryOperations();
            // queryOperations.SetFilter("QueryName", filters.QueryName, false, "Equals", null, false);


            // ICRMContext crmContext = CRMContext.GetContext(tenant);
            ISocialContext socialContext = SocialContext.GetContext(tenant);
            PostRepository postRepository = new PostRepository(tenant);
            PostQueryService service = new PostQueryService(socialContext);
            PostPM post = service.GetSingle(id, true, false);
            post.NumberOfLikes = postRepository.GetPostNumberOfLikes(id, tenant);
            post.PostComments = service.GetMulti(new Logitude.Social.Data.EntityKeys.PostKeys() { Id = id }, false).Where(p => p.IsCancelled == false).ToList();

            return post;
        }

        public PostPM GetSinglePost(string postid, int tenant)
        {
            PostRepository postRepository = new PostRepository(tenant);
            ISocialContext socialContext = SocialContext.GetContext(tenant);

            PostQueryService service = new PostQueryService(socialContext);
            PostPM post = service.GetSingle(postid, true, false);
            post.NumberOfLikes = postRepository.GetPostNumberOfLikes(postid, tenant);
            return post;
        }

        public string PostInsertPost(PostPM Postpm)
        {

            ISocialContext socialContext = SocialContext.GetContext(Postpm.Tenant);
            //SecurityUtility.CheckContactFeature("Post", "NEW", Postpm.Tenant);

            if (socialContext == null)
            {
                socialContext = SocialContext.GetContext(Postpm.Tenant);
            }

            PostUpdateService service = new PostUpdateService(socialContext, new Dictionary<string, IContext>(), Postpm.Tenant);
            Postpm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (PostLikePM postcomm in Postpm.PostLikes)
            {
                postcomm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            service.Update(Postpm, true);


            
            return null;
        }


        //public void Put(PostLikePM postlike)
        //{
        //    ISocialContext socialContext = SocialContext.GetContext(postlike.Tenant);
        //    PostLikeUpdateService postLikeUpdateService = new PostLikeUpdateService(socialContext);
        //    postlike.ChangeSetOp = ChangeSetOperation.Insert;
        //    postLikeUpdateService.Update(postlike, true);
        //    var data = from item in db.TruckInfoes
        //               where item.TruckId == id
        //               select item;
        //    TruckInfo oldRecord = data.SingleOrDefault();
        //    oldRecord.Reg = obj.Reg;
        //    oldRecord.Description = obj.Description;
        //    oldRecord.Condition = obj.Condition;
        //    db.SaveChanges();
        //}

        public string DeletePostLike(string PostId, string UserId, int tenant)
        {
            ISocialContext socialContext = SocialContext.GetContext(tenant);
            PostUpdateService service = new PostUpdateService(socialContext, new Dictionary<string, IContext>(), tenant);
            PostQueryService postQueryService = new PostQueryService(socialContext);

            PostPM entityPM = postQueryService.GetSingle(PostId, true, false);
            entityPM.IsCancelled = true;

            entityPM.ChangeSetOp = ChangeSetOperation.Update;

            //PostLikeQueryService query = new PostLikeQueryService(socialContext);
            //PostLikePM likepm = query.GetSingle(PostId, UserId, false, false);
            //likepm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
            //entityPM.DeletedPostLikes.Add(likepm);
            //likepm.ChangeSetOp = ChangeSetOperation.Delete;

            service.Update(entityPM, true);
            socialContext.SaveChanges();

            return null;
        }
    }
  
}