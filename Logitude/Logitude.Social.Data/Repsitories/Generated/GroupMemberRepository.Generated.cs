 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Social.Data.Repsitories
{
   public partial class GroupMemberRepository:IRepository<GroupMember>
   {
   
        private ISocialContext currentContext;
        public GroupMemberRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public GroupMemberRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  GroupMember GetSingle(string groupid, string userid, int tenant)
        {
            return (from a in context.GroupMembers
                    where a.GroupId == groupid && a.UserId == userid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GroupMember> GetAll(int tenant)
        {
            return from a in context.GroupMembers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GroupMember GetSingle(EntityKeyFields entityKeys)
        {
            GroupMemberKeys keys = entityKeys as GroupMemberKeys;
            return (from a in context.GroupMembers
                    where a.GroupId == keys.GroupId && a.UserId == keys.UserId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GroupMember entity)
        {
            onAdd();
            context.GroupMembers.Add(entity);
        }

        public void Remove(GroupMember entity)
        {
            context.GroupMembers.Attach(entity);
            context.GroupMembers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GroupMember entity)
        {
            onUpdate();
            context.GroupMembers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GroupMember> All()
        {
            return context.GroupMembers.ToList();
        }

        private ISocialContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 