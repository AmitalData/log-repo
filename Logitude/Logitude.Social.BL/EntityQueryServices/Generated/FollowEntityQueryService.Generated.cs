 
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
   public partial class FollowEntityQueryService: EntityQueryService<FollowEntity,FollowEntityKeys,FollowEntityPM,object,FollowEntityKeys>
   {
   
        FollowEntityRepository repository;
		ISocialContext  context;
        public FollowEntityQueryService(int tenant)
        {
		    context = SocialContext.GetContext(tenant);
            MainContext = context;
            repository = new FollowEntityRepository(context);
            Repository = repository;
            mapping = new FollowEntityDataMapping();
        }

        public FollowEntityQueryService(FollowEntityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FollowEntityDataMapping();
        }

        public FollowEntityQueryService(ISocialContext context)
        {
            this.repository = new FollowEntityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FollowEntityDataMapping();
        }
		 
		public  FollowEntityPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FollowEntityKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FollowEntity entityPOCO)
        {
            FollowEntityKeys entityKeys = new FollowEntityKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 