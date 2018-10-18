 
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
   public partial class ClaimsRelatedEntsReasonsExpRepository:IRepository<ClaimsRelatedEntsReasonsExp>
   {
   
        private ICustomContext currentContext;
        public ClaimsRelatedEntsReasonsExpRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimsRelatedEntsReasonsExpRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimsRelatedEntsReasonsExp GetSingle(string claimid, int counterkey, int reasonlineno, int lineno, int tenant)
        {
            return (from a in context.ClaimsRelatedEntsReasonsExps
                    where a.ClaimId == claimid && a.CounterKey == counterkey && a.ReasonLineNo == reasonlineno && a.LineNo == lineno && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimsRelatedEntsReasonsExp> GetAll(int tenant)
        {
            return from a in context.ClaimsRelatedEntsReasonsExps  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimsRelatedEntsReasonsExp GetSingle(EntityKeyFields entityKeys)
        {
            ClaimsRelatedEntsReasonsExpKeys keys = entityKeys as ClaimsRelatedEntsReasonsExpKeys;
            return (from a in context.ClaimsRelatedEntsReasonsExps
                    where a.ClaimId == keys.ClaimId && a.CounterKey == keys.CounterKey && a.ReasonLineNo == keys.ReasonLineNo && a.LineNo == keys.LineNo
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimsRelatedEntsReasonsExp entity)
        {
            onAdd();
            context.ClaimsRelatedEntsReasonsExps.Add(entity);
        }

        public void Remove(ClaimsRelatedEntsReasonsExp entity)
        {
            context.ClaimsRelatedEntsReasonsExps.Attach(entity);
            context.ClaimsRelatedEntsReasonsExps.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimsRelatedEntsReasonsExp entity)
        {
            onUpdate();
            context.ClaimsRelatedEntsReasonsExps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimsRelatedEntsReasonsExp> All()
        {
            return context.ClaimsRelatedEntsReasonsExps.ToList();
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
	 