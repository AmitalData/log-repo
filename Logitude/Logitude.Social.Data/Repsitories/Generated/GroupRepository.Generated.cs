 
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
   public partial class GroupRepository:IRepository<Group>
   {
   
        private ISocialContext currentContext;
        public GroupRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public GroupRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  Group GetSingle(string id, int tenant)
        {
            return (from a in context.Groups
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Group> GetAll(int tenant)
        {
            return from a in context.Groups  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Group GetSingle(EntityKeyFields entityKeys)
        {
            GroupKeys keys = entityKeys as GroupKeys;
            return (from a in context.Groups
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Group entity)
        {
            onAdd();
            context.Groups.Add(entity);
        }

        public void Remove(Group entity)
        {
            context.Groups.Attach(entity);
            context.Groups.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Group entity)
        {
            onUpdate();
            context.Groups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Group> All()
        {
            return context.Groups.ToList();
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
	 