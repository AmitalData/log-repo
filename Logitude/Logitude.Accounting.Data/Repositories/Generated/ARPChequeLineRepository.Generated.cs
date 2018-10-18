 
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
   public partial class ARPChequeLineRepository:IRepository<ARPChequeLine>
   {
   
        private IAccountingContext currentContext;
        public ARPChequeLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ARPChequeLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ARPChequeLine GetSingle(string id, int tenant)
        {
            return (from a in context.ARPChequeLines
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ARPChequeLine> GetAll(int tenant)
        {
            return from a in context.ARPChequeLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ARPChequeLine GetSingle(EntityKeyFields entityKeys)
        {
            ARPChequeLineKeys keys = entityKeys as ARPChequeLineKeys;
            return (from a in context.ARPChequeLines
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 
        public void Add(ARPChequeLine entity)
        {
            context.ARPChequeLines.Add(entity);
        }

        public void Remove(ARPChequeLine entity)
        {
            context.ARPChequeLines.Attach(entity);
            context.ARPChequeLines.Remove(entity);
        }

        public void Update(ARPChequeLine entity)
        {
            context.ARPChequeLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARPChequeLine> All()
        {
            return context.ARPChequeLines.ToList();
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
	 