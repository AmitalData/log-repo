 
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
   public partial class ARPaymentChequeStatusRepository:IRepository<ARPaymentChequeStatus>
   {
   
        private IAccountingContext currentContext;
        public ARPaymentChequeStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ARPaymentChequeStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ARPaymentChequeStatus GetSingle(string code)
        {
            return (from a in context.ARPaymentChequeStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ARPaymentChequeStatus> GetAll()
        {
            return from a in context.ARPaymentChequeStatuses  
                   select a;
        }
				 
        public ARPaymentChequeStatus GetSingle(EntityKeyFields entityKeys)
        {
            ARPaymentChequeStatusKeys keys = entityKeys as ARPaymentChequeStatusKeys;
            return (from a in context.ARPaymentChequeStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ARPaymentChequeStatus entity)
        {
            onAdd();
            context.ARPaymentChequeStatuses.Add(entity);
        }

        public void Remove(ARPaymentChequeStatus entity)
        {
            context.ARPaymentChequeStatuses.Attach(entity);
            context.ARPaymentChequeStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ARPaymentChequeStatus entity)
        {
            onUpdate();
            context.ARPaymentChequeStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARPaymentChequeStatus> All()
        {
            return context.ARPaymentChequeStatuses.ToList();
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
	 