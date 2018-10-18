 
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
   public partial class ClaimsRelatedEntitiesReasonRepository:IRepository<ClaimsRelatedEntitiesReason>
   {
   
        private ICustomContext currentContext;
        public ClaimsRelatedEntitiesReasonRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimsRelatedEntitiesReasonRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimsRelatedEntitiesReason GetSingle(string claimid, int counterkey, int lineno, int tenant)
        {
            return (from a in context.ClaimsRelatedEntitiesReasons
                    where a.ClaimId == claimid && a.CounterKey == counterkey && a.LineNo == lineno && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimsRelatedEntitiesReason> GetAll(int tenant)
        {
            return from a in context.ClaimsRelatedEntitiesReasons  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimsRelatedEntitiesReason GetSingle(EntityKeyFields entityKeys)
        {
            ClaimsRelatedEntitiesReasonKeys keys = entityKeys as ClaimsRelatedEntitiesReasonKeys;
            return (from a in context.ClaimsRelatedEntitiesReasons
                    where a.ClaimId == keys.ClaimId && a.CounterKey == keys.CounterKey && a.LineNo == keys.LineNo
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimsRelatedEntitiesReason entity)
        {
            onAdd();
            context.ClaimsRelatedEntitiesReasons.Add(entity);
        }

        public void Remove(ClaimsRelatedEntitiesReason entity)
        {
            context.ClaimsRelatedEntitiesReasons.Attach(entity);
            context.ClaimsRelatedEntitiesReasons.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimsRelatedEntitiesReason entity)
        {
            onUpdate();
            context.ClaimsRelatedEntitiesReasons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimsRelatedEntitiesReason> All()
        {
            return context.ClaimsRelatedEntitiesReasons.ToList();
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
	 