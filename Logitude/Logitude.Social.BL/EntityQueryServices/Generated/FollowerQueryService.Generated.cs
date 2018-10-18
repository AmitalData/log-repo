 
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
   public partial class FollowerQueryService: EntityQueryService<Follower,FollowerKeys,FollowerPM,object,FollowerKeys>
   {
   
        FollowerRepository repository;
		ISocialContext  context;
        public FollowerQueryService(int tenant)
        {
		    context = SocialContext.GetContext(tenant);
            MainContext = context;
            repository = new FollowerRepository(context);
            Repository = repository;
            mapping = new FollowerDataMapping();
        }

        public FollowerQueryService(FollowerRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FollowerDataMapping();
        }

        public FollowerQueryService(ISocialContext context)
        {
            this.repository = new FollowerRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FollowerDataMapping();
        }
		 
		public  FollowerPM GetSingle(string followeeuserid, string followeruserid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FollowerKeys(){ FolloweeUserId = followeeuserid, FollowerUserId = followeruserid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Follower entityPOCO)
        {
            FollowerKeys entityKeys = new FollowerKeys() { FolloweeUserId = entityPOCO.FolloweeUserId, FollowerUserId = entityPOCO.FollowerUserId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 