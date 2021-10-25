 
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
   public partial class ARPaymentBankTranferRepository:IRepository<ARPaymentBankTranfer>
   {
   
        private IAccountingContext currentContext;
        public ARPaymentBankTranferRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ARPaymentBankTranferRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ARPaymentBankTranfer GetSingle(string id, int tenant)
        {
            return (from a in context.ARPaymentBankTranfers
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ARPaymentBankTranfer> GetAll(int tenant)
        {
            return from a in context.ARPaymentBankTranfers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ARPaymentBankTranfer GetSingle(EntityKeyFields entityKeys)
        {
            ARPaymentBankTranferKeys keys = entityKeys as ARPaymentBankTranferKeys;
            return (from a in context.ARPaymentBankTranfers
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ARPaymentBankTranfer entity)
        {
            onAdd();
            context.ARPaymentBankTranfers.Add(entity);
        }

        public void Remove(ARPaymentBankTranfer entity)
        {
            context.ARPaymentBankTranfers.Attach(entity);
            context.ARPaymentBankTranfers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ARPaymentBankTranfer entity)
        {
            onUpdate();
            context.ARPaymentBankTranfers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARPaymentBankTranfer> All()
        {
            return context.ARPaymentBankTranfers.ToList();
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
	 