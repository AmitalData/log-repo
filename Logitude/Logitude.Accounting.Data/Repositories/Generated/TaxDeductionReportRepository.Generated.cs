 
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
   public partial class TaxDeductionReportRepository:IRepository<TaxDeductionReport>
   {
   
        private IAccountingContext currentContext;
        public TaxDeductionReportRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TaxDeductionReportRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaxDeductionReport GetSingle(string id, int tenant)
        {
            return (from a in context.TaxDeductionReports
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TaxDeductionReport> GetAll(int tenant)
        {
            return from a in context.TaxDeductionReports  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TaxDeductionReport GetSingle(EntityKeyFields entityKeys)
        {
            TaxDeductionReportKeys keys = entityKeys as TaxDeductionReportKeys;
            return (from a in context.TaxDeductionReports
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaxDeductionReport entity)
        {
            onAdd();
            context.TaxDeductionReports.Add(entity);
        }

        public void Remove(TaxDeductionReport entity)
        {
            context.TaxDeductionReports.Attach(entity);
            context.TaxDeductionReports.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaxDeductionReport entity)
        {
            onUpdate();
            context.TaxDeductionReports.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaxDeductionReport> All()
        {
            return context.TaxDeductionReports.ToList();
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
	 