 
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
   public partial class TaxDeductionReportStatusRepository:IRepository<TaxDeductionReportStatus>
   {
   
        private IAccountingContext currentContext;
        public TaxDeductionReportStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TaxDeductionReportStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaxDeductionReportStatus GetSingle(string code)
        {
            return (from a in context.TaxDeductionReportStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TaxDeductionReportStatus> GetAll()
        {
            return from a in context.TaxDeductionReportStatuses  
                   select a;
        }
				 
        public TaxDeductionReportStatus GetSingle(EntityKeyFields entityKeys)
        {
            TaxDeductionReportStatusKeys keys = entityKeys as TaxDeductionReportStatusKeys;
            return (from a in context.TaxDeductionReportStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaxDeductionReportStatus entity)
        {
            onAdd();
            context.TaxDeductionReportStatuses.Add(entity);
        }

        public void Remove(TaxDeductionReportStatus entity)
        {
            context.TaxDeductionReportStatuses.Attach(entity);
            context.TaxDeductionReportStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaxDeductionReportStatus entity)
        {
            onUpdate();
            context.TaxDeductionReportStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaxDeductionReportStatus> All()
        {
            return context.TaxDeductionReportStatuses.ToList();
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
	 