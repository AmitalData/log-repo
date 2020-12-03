 
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
   public partial class CalculatedChartsOfAccountsLineRepository:IRepository<CalculatedChartsOfAccountsLine>
   {
   
        private IAccountingContext currentContext;
        public CalculatedChartsOfAccountsLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CalculatedChartsOfAccountsLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CalculatedChartsOfAccountsLine GetSingle(string id, int tenant)
        {
            return (from a in context.CalculatedChartsOfAccountLines
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CalculatedChartsOfAccountsLine> GetAll(int tenant)
        {
            return from a in context.CalculatedChartsOfAccountLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CalculatedChartsOfAccountsLine GetSingle(EntityKeyFields entityKeys)
        {
            CalculatedChartsOfAccountsLineKeys keys = entityKeys as CalculatedChartsOfAccountsLineKeys;
            return (from a in context.CalculatedChartsOfAccountLines
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CalculatedChartsOfAccountsLine entity)
        {
            onAdd();
            context.CalculatedChartsOfAccountLines.Add(entity);
        }

        public void Remove(CalculatedChartsOfAccountsLine entity)
        {
            context.CalculatedChartsOfAccountLines.Attach(entity);
            context.CalculatedChartsOfAccountLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CalculatedChartsOfAccountsLine entity)
        {
            onUpdate();
            context.CalculatedChartsOfAccountLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CalculatedChartsOfAccountsLine> All()
        {
            return context.CalculatedChartsOfAccountLines.ToList();
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
	 