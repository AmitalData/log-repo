 
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
   public partial class DeficitDecisionRepository:IRepository<DeficitDecision>
   {
   
        private ICustomContext currentContext;
        public DeficitDecisionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeficitDecisionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeficitDecision GetSingle(string deficitid, string declarationid, int tenant)
        {
            return (from a in context.DeficitDecisions
                    where a.DeficitId == deficitid && a.DeclarationId == declarationid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeficitDecision> GetAll(int tenant)
        {
            return from a in context.DeficitDecisions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeficitDecision GetSingle(EntityKeyFields entityKeys)
        {
            DeficitDecisionKeys keys = entityKeys as DeficitDecisionKeys;
            return (from a in context.DeficitDecisions
                    where a.DeficitId == keys.DeficitId && a.DeclarationId == keys.DeclarationId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeficitDecision entity)
        {
            onAdd();
            context.DeficitDecisions.Add(entity);
        }

        public void Remove(DeficitDecision entity)
        {
            context.DeficitDecisions.Attach(entity);
            context.DeficitDecisions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeficitDecision entity)
        {
            onUpdate();
            context.DeficitDecisions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeficitDecision> All()
        {
            return context.DeficitDecisions.ToList();
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
	 