 
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
   public partial class GroupMemberQueryService: EntityQueryService<GroupMember,GroupMemberKeys,GroupMemberPM,object,GroupMemberKeys>
   {
   
        GroupMemberRepository repository;
		ISocialContext  context;
        public GroupMemberQueryService(int tenant)
        {
		    context = SocialContext.GetContext(tenant);
            MainContext = context;
            repository = new GroupMemberRepository(context);
            Repository = repository;
            mapping = new GroupMemberDataMapping();
        }

        public GroupMemberQueryService(GroupMemberRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GroupMemberDataMapping();
        }

        public GroupMemberQueryService(ISocialContext context)
        {
            this.repository = new GroupMemberRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GroupMemberDataMapping();
        }
		 
		public  GroupMemberPM GetSingle(string groupid, string userid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GroupMemberKeys(){ GroupId = groupid, UserId = userid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GroupMember entityPOCO)
        {
            GroupMemberKeys entityKeys = new GroupMemberKeys() { GroupId = entityPOCO.GroupId, UserId = entityPOCO.UserId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 