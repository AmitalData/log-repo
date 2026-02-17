 
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
   public partial class ClaimsRelatedEntitiesAmountRepository:IRepository<ClaimsRelatedEntitiesAmount>
   {
   
        private ICustomContext currentContext;
        public ClaimsRelatedEntitiesAmountRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimsRelatedEntitiesAmountRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimsRelatedEntitiesAmount GetSingle(string claimid, int counterkey, int lineno, int tenant)
        {
            return (from a in context.ClaimsRelatedEntitiesAmounts
                    where a.ClaimId == claimid && a.CounterKey == counterkey && a.LineNo == lineno && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimsRelatedEntitiesAmount> GetAll(int tenant)
        {
            return from a in context.ClaimsRelatedEntitiesAmounts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimsRelatedEntitiesAmount GetSingle(EntityKeyFields entityKeys)
        {
            ClaimsRelatedEntitiesAmountKeys keys = entityKeys as ClaimsRelatedEntitiesAmountKeys;
            return (from a in context.ClaimsRelatedEntitiesAmounts
                    where a.ClaimId == keys.ClaimId && a.CounterKey == keys.CounterKey && a.LineNo == keys.LineNo
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimsRelatedEntitiesAmount entity)
        {
            onAdd();
            context.ClaimsRelatedEntitiesAmounts.Add(entity);
        }

        public void Remove(ClaimsRelatedEntitiesAmount entity)
        {
            context.ClaimsRelatedEntitiesAmounts.Attach(entity);
            context.ClaimsRelatedEntitiesAmounts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimsRelatedEntitiesAmount entity)
        {
            onUpdate();
            context.ClaimsRelatedEntitiesAmounts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimsRelatedEntitiesAmount> All()
        {
            return context.ClaimsRelatedEntitiesAmounts.ToList();
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
	 