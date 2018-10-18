 
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
   public partial class GLAccountWithholdingTaxRepository:IRepository<GLAccountWithholdingTax>
   {
   
        private IAccountingContext currentContext;
        public GLAccountWithholdingTaxRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountWithholdingTaxRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountWithholdingTax GetSingle(string id, int tenant)
        {
            return (from a in context.GLAccountWithholdingTax
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountWithholdingTax> GetAll(int tenant)
        {
            return from a in context.GLAccountWithholdingTax  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountWithholdingTax GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountWithholdingTaxKeys keys = entityKeys as GLAccountWithholdingTaxKeys;
            return (from a in context.GLAccountWithholdingTax
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountWithholdingTax entity)
        {
            onAdd();
            context.GLAccountWithholdingTax.Add(entity);
        }

        public void Remove(GLAccountWithholdingTax entity)
        {
            context.GLAccountWithholdingTax.Attach(entity);
            context.GLAccountWithholdingTax.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountWithholdingTax entity)
        {
            onUpdate();
            context.GLAccountWithholdingTax.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountWithholdingTax> All()
        {
            return context.GLAccountWithholdingTax.ToList();
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
	 