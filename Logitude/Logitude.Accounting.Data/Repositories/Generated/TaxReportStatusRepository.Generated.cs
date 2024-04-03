 
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
   public partial class TaxReportStatusRepository:IRepository<TaxReportStatus>
   {
   
        private IAccountingContext currentContext;
        public TaxReportStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TaxReportStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaxReportStatus GetSingle(string code)
        {
            return (from a in context.TaxReportStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TaxReportStatus> GetAll()
        {
            return from a in context.TaxReportStatuses  
                   select a;
        }
				 
        public TaxReportStatus GetSingle(EntityKeyFields entityKeys)
        {
            TaxReportStatusKeys keys = entityKeys as TaxReportStatusKeys;
            return (from a in context.TaxReportStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaxReportStatus entity)
        {
            onAdd();
            context.TaxReportStatuses.Add(entity);
        }

        public void Remove(TaxReportStatus entity)
        {
            context.TaxReportStatuses.Attach(entity);
            context.TaxReportStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaxReportStatus entity)
        {
            onUpdate();
            context.TaxReportStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaxReportStatus> All()
        {
            return context.TaxReportStatuses.ToList();
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
	 