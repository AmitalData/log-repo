 
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
   public partial class ClaimsRelatedEntitiesSeizureRepository:IRepository<ClaimsRelatedEntitiesSeizure>
   {
   
        private ICustomContext currentContext;
        public ClaimsRelatedEntitiesSeizureRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimsRelatedEntitiesSeizureRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimsRelatedEntitiesSeizure GetSingle(string claimid, int counterkey, int seizurelinono, int tenant)
        {
            return (from a in context.ClaimsRelatedEntitiesSeizures
                    where a.ClaimId == claimid && a.CounterKey == counterkey && a.SeizureLinoNo == seizurelinono && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimsRelatedEntitiesSeizure> GetAll(int tenant)
        {
            return from a in context.ClaimsRelatedEntitiesSeizures  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimsRelatedEntitiesSeizure GetSingle(EntityKeyFields entityKeys)
        {
            ClaimsRelatedEntitiesSeizureKeys keys = entityKeys as ClaimsRelatedEntitiesSeizureKeys;
            return (from a in context.ClaimsRelatedEntitiesSeizures
                    where a.ClaimId == keys.ClaimId && a.CounterKey == keys.CounterKey && a.SeizureLinoNo == keys.SeizureLinoNo
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimsRelatedEntitiesSeizure entity)
        {
            onAdd();
            context.ClaimsRelatedEntitiesSeizures.Add(entity);
        }

        public void Remove(ClaimsRelatedEntitiesSeizure entity)
        {
            context.ClaimsRelatedEntitiesSeizures.Attach(entity);
            context.ClaimsRelatedEntitiesSeizures.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimsRelatedEntitiesSeizure entity)
        {
            onUpdate();
            context.ClaimsRelatedEntitiesSeizures.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimsRelatedEntitiesSeizure> All()
        {
            return context.ClaimsRelatedEntitiesSeizures.ToList();
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
	 