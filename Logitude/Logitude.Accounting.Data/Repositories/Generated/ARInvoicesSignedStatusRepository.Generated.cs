 
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
   public partial class ARInvoicesSignedStatusRepository:IRepository<ARInvoicesSignedStatus>
   {
   
        private IAccountingContext currentContext;
        public ARInvoicesSignedStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ARInvoicesSignedStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ARInvoicesSignedStatus GetSingle(string code)
        {
            return (from a in context.ARInvoicesSignedStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ARInvoicesSignedStatus> GetAll()
        {
            return from a in context.ARInvoicesSignedStatuses  
                   select a;
        }
				 
        public ARInvoicesSignedStatus GetSingle(EntityKeyFields entityKeys)
        {
            ARInvoicesSignedStatusKeys keys = entityKeys as ARInvoicesSignedStatusKeys;
            return (from a in context.ARInvoicesSignedStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ARInvoicesSignedStatus entity)
        {
            onAdd();
            context.ARInvoicesSignedStatuses.Add(entity);
        }

        public void Remove(ARInvoicesSignedStatus entity)
        {
            context.ARInvoicesSignedStatuses.Attach(entity);
            context.ARInvoicesSignedStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ARInvoicesSignedStatus entity)
        {
            onUpdate();
            context.ARInvoicesSignedStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARInvoicesSignedStatus> All()
        {
            return context.ARInvoicesSignedStatuses.ToList();
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
	 