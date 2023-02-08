 
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
   public partial class TaxReportLineTransmitStatusRepository:IRepository<TaxReportLineTransmitStatus>
   {
   
        private IAccountingContext currentContext;
        public TaxReportLineTransmitStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TaxReportLineTransmitStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaxReportLineTransmitStatus GetSingle(string code)
        {
            return (from a in context.TaxReportLineTransmitStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TaxReportLineTransmitStatus> GetAll()
        {
            return from a in context.TaxReportLineTransmitStatuses  
                   select a;
        }
				 
        public TaxReportLineTransmitStatus GetSingle(EntityKeyFields entityKeys)
        {
            TaxReportLineTransmitStatusKeys keys = entityKeys as TaxReportLineTransmitStatusKeys;
            return (from a in context.TaxReportLineTransmitStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaxReportLineTransmitStatus entity)
        {
            onAdd();
            context.TaxReportLineTransmitStatuses.Add(entity);
        }

        public void Remove(TaxReportLineTransmitStatus entity)
        {
            context.TaxReportLineTransmitStatuses.Attach(entity);
            context.TaxReportLineTransmitStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaxReportLineTransmitStatus entity)
        {
            onUpdate();
            context.TaxReportLineTransmitStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaxReportLineTransmitStatus> All()
        {
            return context.TaxReportLineTransmitStatuses.ToList();
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
	 