 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class ClaimsRelatedEntityRepository:IRepository<ClaimsRelatedEntity>
   {
   
        private ICustomContext currentContext;
        public ClaimsRelatedEntityRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimsRelatedEntityRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimsRelatedEntity GetSingle(string claimid, int entitycounterkey, int tenant)
        {
            return (from a in context.ClaimsRelatedEntities
                    where a.ClaimId == claimid && a.EntityCounterKey == entitycounterkey && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimsRelatedEntity> GetAll(int tenant)
        {
            return from a in context.ClaimsRelatedEntities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimsRelatedEntity GetSingle(EntityKeyFields entityKeys)
        {
            ClaimsRelatedEntityKeys keys = entityKeys as ClaimsRelatedEntityKeys;
            return (from a in context.ClaimsRelatedEntities
                    where a.ClaimId == keys.ClaimId && a.EntityCounterKey == keys.EntityCounterKey
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimsRelatedEntity entity)
        {
            onAdd();
            context.ClaimsRelatedEntities.Add(entity);
        }

        public void Remove(ClaimsRelatedEntity entity)
        {
            context.ClaimsRelatedEntities.Attach(entity);
            context.ClaimsRelatedEntities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimsRelatedEntity entity)
        {
            onUpdate();
            context.ClaimsRelatedEntities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimsRelatedEntity> All()
        {
            return context.ClaimsRelatedEntities.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 