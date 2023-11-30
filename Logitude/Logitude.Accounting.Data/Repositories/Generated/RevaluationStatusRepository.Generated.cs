 
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
   public partial class RevaluationStatusRepository:IRepository<RevaluationStatus>
   {
   
        private IAccountingContext currentContext;
        public RevaluationStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public RevaluationStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  RevaluationStatus GetSingle(string code)
        {
            return (from a in context.RevaluationStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RevaluationStatus> GetAll()
        {
            return from a in context.RevaluationStatuses  
                   select a;
        }
				 
        public RevaluationStatus GetSingle(EntityKeyFields entityKeys)
        {
            RevaluationStatusKeys keys = entityKeys as RevaluationStatusKeys;
            return (from a in context.RevaluationStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RevaluationStatus entity)
        {
            onAdd();
            context.RevaluationStatuses.Add(entity);
        }

        public void Remove(RevaluationStatus entity)
        {
            context.RevaluationStatuses.Attach(entity);
            context.RevaluationStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RevaluationStatus entity)
        {
            onUpdate();
            context.RevaluationStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RevaluationStatus> All()
        {
            return context.RevaluationStatuses.ToList();
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
	 