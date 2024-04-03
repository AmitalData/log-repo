 
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
   public partial class ClaimsRelatedEntitiesRefundRepository:IRepository<ClaimsRelatedEntitiesRefund>
   {
   
        private ICustomContext currentContext;
        public ClaimsRelatedEntitiesRefundRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimsRelatedEntitiesRefundRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimsRelatedEntitiesRefund GetSingle(string claimid, int counterkey, int refundquntitylineno, int tenant)
        {
            return (from a in context.ClaimsRelatedEntitiesRefunds
                    where a.ClaimId == claimid && a.CounterKey == counterkey && a.RefundQuntityLineNo == refundquntitylineno && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimsRelatedEntitiesRefund> GetAll(int tenant)
        {
            return from a in context.ClaimsRelatedEntitiesRefunds  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimsRelatedEntitiesRefund GetSingle(EntityKeyFields entityKeys)
        {
            ClaimsRelatedEntitiesRefundKeys keys = entityKeys as ClaimsRelatedEntitiesRefundKeys;
            return (from a in context.ClaimsRelatedEntitiesRefunds
                    where a.ClaimId == keys.ClaimId && a.CounterKey == keys.CounterKey && a.RefundQuntityLineNo == keys.RefundQuntityLineNo
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimsRelatedEntitiesRefund entity)
        {
            onAdd();
            context.ClaimsRelatedEntitiesRefunds.Add(entity);
        }

        public void Remove(ClaimsRelatedEntitiesRefund entity)
        {
            context.ClaimsRelatedEntitiesRefunds.Attach(entity);
            context.ClaimsRelatedEntitiesRefunds.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimsRelatedEntitiesRefund entity)
        {
            onUpdate();
            context.ClaimsRelatedEntitiesRefunds.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimsRelatedEntitiesRefund> All()
        {
            return context.ClaimsRelatedEntitiesRefunds.ToList();
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
	 