 
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
   public partial class TaxReportRepository:IRepository<TaxReport>
   {
   
        private IAccountingContext currentContext;
        public TaxReportRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TaxReportRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaxReport GetSingle(string id, int tenant)
        {
            return (from a in context.TaxReports
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TaxReport> GetAll(int tenant)
        {
            return from a in context.TaxReports  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TaxReport GetSingle(EntityKeyFields entityKeys)
        {
            TaxReportKeys keys = entityKeys as TaxReportKeys;
            return (from a in context.TaxReports
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaxReport entity)
        {
            onAdd();
            context.TaxReports.Add(entity);
        }

        public void Remove(TaxReport entity)
        {
            context.TaxReports.Attach(entity);
            context.TaxReports.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaxReport entity)
        {
            onUpdate();
            context.TaxReports.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaxReport> All()
        {
            return context.TaxReports.ToList();
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
	 