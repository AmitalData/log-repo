 
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
   public partial class TaxReportLineRepository:IRepository<TaxReportLine>
   {
   
        private IAccountingContext currentContext;
        public TaxReportLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TaxReportLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaxReportLine GetSingle(string taxreportid, int line, int tenant)
        {
            return (from a in context.TaxReportLines
                    where a.TaxReportId == taxreportid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TaxReportLine> GetAll(int tenant)
        {
            return from a in context.TaxReportLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TaxReportLine GetSingle(EntityKeyFields entityKeys)
        {
            TaxReportLineKeys keys = entityKeys as TaxReportLineKeys;
            return (from a in context.TaxReportLines
                    where a.TaxReportId == keys.TaxReportId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaxReportLine entity)
        {
            onAdd();
            context.TaxReportLines.Add(entity);
        }

        public void Remove(TaxReportLine entity)
        {
            context.TaxReportLines.Attach(entity);
            context.TaxReportLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaxReportLine entity)
        {
            onUpdate();
            context.TaxReportLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaxReportLine> All()
        {
            return context.TaxReportLines.ToList();
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
	 