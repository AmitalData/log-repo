 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class RevaluationRepository:IRepository<Revaluation>
   {
   
        private IAccountingContext currentContext;
        public RevaluationRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public RevaluationRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Revaluation GetSingle(string id, int tenant)
        {
            return (from a in context.Revaluations
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Revaluation> GetAll(int tenant)
        {
            return from a in context.Revaluations  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Revaluation GetSingle(EntityKeyFields entityKeys)
        {
            RevaluationKeys keys = entityKeys as RevaluationKeys;
            return (from a in context.Revaluations
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Revaluation entity)
        {
            onAdd();
            context.Revaluations.Add(entity);
        }

        public void Remove(Revaluation entity)
        {
            context.Revaluations.Attach(entity);
            context.Revaluations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Revaluation entity)
        {
            onUpdate();
            context.Revaluations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Revaluation> All()
        {
            return context.Revaluations.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 