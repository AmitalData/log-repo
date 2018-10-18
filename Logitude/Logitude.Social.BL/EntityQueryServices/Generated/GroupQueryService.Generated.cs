 
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
   public partial class GroupQueryService: EntityQueryService<Group,GroupKeys,GroupPM,object,GroupKeys>
   {
   
        GroupRepository repository;
		ISocialContext  context;
        public GroupQueryService(int tenant)
        {
		    context = SocialContext.GetContext(tenant);
            MainContext = context;
            repository = new GroupRepository(context);
            Repository = repository;
            mapping = new GroupDataMapping();
        }

        public GroupQueryService(GroupRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GroupDataMapping();
        }

        public GroupQueryService(ISocialContext context)
        {
            this.repository = new GroupRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GroupDataMapping();
        }
		 
		public  GroupPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GroupKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Group entityPOCO)
        {
            GroupKeys entityKeys = new GroupKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 