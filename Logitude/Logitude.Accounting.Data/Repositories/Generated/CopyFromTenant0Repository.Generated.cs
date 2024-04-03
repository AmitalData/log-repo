 
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
   public partial class CopyFromTenant0Repository:IRepository<CopyFromTenant0>
   {
   
        private IAccountingContext currentContext;
        public CopyFromTenant0Repository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CopyFromTenant0Repository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CopyFromTenant0 GetSingle(string id, int tenant)
        {
            return (from a in context.CopyFromTenant0
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CopyFromTenant0> GetAll(int tenant)
        {
            return from a in context.CopyFromTenant0  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CopyFromTenant0 GetSingle(EntityKeyFields entityKeys)
        {
            CopyFromTenant0Keys keys = entityKeys as CopyFromTenant0Keys;
            return (from a in context.CopyFromTenant0
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CopyFromTenant0 entity)
        {
            onAdd();
            context.CopyFromTenant0.Add(entity);
        }

        public void Remove(CopyFromTenant0 entity)
        {
            context.CopyFromTenant0.Attach(entity);
            context.CopyFromTenant0.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CopyFromTenant0 entity)
        {
            onUpdate();
            context.CopyFromTenant0.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CopyFromTenant0> All()
        {
            return context.CopyFromTenant0.ToList();
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
	 