 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL.EntityPMs;
using Logitude.Social.BL.EntityDataMappings;
using Logitude.Social.Data.Repsitories;
using Logitude.Social.Data.EntityKeys;
using Logitude.Social.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Social.BL.EntityQueryServices
{ 
   public partial class PostLikeQueryService: EntityQueryService<PostLike,PostLikeKeys,PostLikePM,PostPM,PostKeys>
   {
   
        PostLikeRepository repository;
		ISocialContext  context;
        public PostLikeQueryService(int tenant)
        {
		    context = SocialContext.GetContext(tenant);
            MainContext = context;
            repository = new PostLikeRepository(context);
            Repository = repository;
            mapping = new PostLikeDataMapping();
        }

        public PostLikeQueryService(PostLikeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PostLikeDataMapping();
        }

        public PostLikeQueryService(ISocialContext context)
        {
            this.repository = new PostLikeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PostLikeDataMapping();
        }
		 
		public  PostLikePM GetSingle(string postid, string userid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PostLikeKeys(){ PostId = postid, UserId = userid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PostLike entityPOCO)
        {
            PostLikeKeys entityKeys = new PostLikeKeys() { PostId = entityPOCO.PostId, UserId = entityPOCO.UserId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 