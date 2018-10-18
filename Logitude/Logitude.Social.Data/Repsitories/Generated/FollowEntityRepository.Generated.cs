 
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
   public partial class FollowEntityRepository:IRepository<FollowEntity>
   {
   
        private ISocialContext currentContext;
        public FollowEntityRepository(int tenant)
        {
            currentContext = SocialContext.GetContext(tenant);
        }

        public FollowEntityRepository(ISocialContext context)
        {
            currentContext = context;
        }

		 
		
		public  FollowEntity GetSingle(string id, int tenant)
        {
            return (from a in context.FollowEntities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<FollowEntity> GetAll(int tenant)
        {
            return from a in context.FollowEntities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public FollowEntity GetSingle(EntityKeyFields entityKeys)
        {
            FollowEntityKeys keys = entityKeys as FollowEntityKeys;
            return (from a in context.FollowEntities
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FollowEntity entity)
        {
            onAdd();
            context.FollowEntities.Add(entity);
        }

        public void Remove(FollowEntity entity)
        {
            context.FollowEntities.Attach(entity);
            context.FollowEntities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FollowEntity entity)
        {
            onUpdate();
            context.FollowEntities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FollowEntity> All()
        {
            return context.FollowEntities.ToList();
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
	 