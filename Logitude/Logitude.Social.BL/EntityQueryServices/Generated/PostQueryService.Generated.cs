 
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
   public partial class PostQueryService: EntityQueryService<Post,PostKeys,PostPM,object,PostKeys>
   {
   
        PostRepository repository;
		ISocialContext  context;
        public PostQueryService(int tenant)
        {
		    context = SocialContext.GetContext(tenant);
            MainContext = context;
            repository = new PostRepository(context);
            Repository = repository;
            mapping = new PostDataMapping();
        }

        public PostQueryService(PostRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PostDataMapping();
        }

        public PostQueryService(ISocialContext context)
        {
            this.repository = new PostRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PostDataMapping();
        }
		 
		public  PostPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PostKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Post entityPOCO)
        {
            PostKeys entityKeys = new PostKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 