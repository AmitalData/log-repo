 
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
   public partial class ARPaymentChequeRepository:IRepository<ARPaymentCheque>
   {
   
        private IAccountingContext currentContext;
        public ARPaymentChequeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ARPaymentChequeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ARPaymentCheque GetSingle(string id, int tenant)
        {
            return (from a in context.ARPaymentCheques
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ARPaymentCheque> GetAll(int tenant)
        {
            return from a in context.ARPaymentCheques  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ARPaymentCheque GetSingle(EntityKeyFields entityKeys)
        {
            ARPaymentChequeKeys keys = entityKeys as ARPaymentChequeKeys;
            return (from a in context.ARPaymentCheques
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ARPaymentCheque entity)
        {
            onAdd();
            context.ARPaymentCheques.Add(entity);
        }

        public void Remove(ARPaymentCheque entity)
        {
            context.ARPaymentCheques.Attach(entity);
            context.ARPaymentCheques.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ARPaymentCheque entity)
        {
            onUpdate();
            context.ARPaymentCheques.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARPaymentCheque> All()
        {
            return context.ARPaymentCheques.ToList();
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
	 