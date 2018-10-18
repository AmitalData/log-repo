 
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
   public partial class TaxReportLineStatusRepository:IRepository<TaxReportLineStatus>
   {
   
        private IAccountingContext currentContext;
        public TaxReportLineStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TaxReportLineStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaxReportLineStatus GetSingle(string code)
        {
            return (from a in context.TaxReportLineStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TaxReportLineStatus> GetAll()
        {
            return from a in context.TaxReportLineStatuses  
                   select a;
        }
				 
        public TaxReportLineStatus GetSingle(EntityKeyFields entityKeys)
        {
            TaxReportLineStatusKeys keys = entityKeys as TaxReportLineStatusKeys;
            return (from a in context.TaxReportLineStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaxReportLineStatus entity)
        {
            onAdd();
            context.TaxReportLineStatuses.Add(entity);
        }

        public void Remove(TaxReportLineStatus entity)
        {
            context.TaxReportLineStatuses.Attach(entity);
            context.TaxReportLineStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaxReportLineStatus entity)
        {
            onUpdate();
            context.TaxReportLineStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaxReportLineStatus> All()
        {
            return context.TaxReportLineStatuses.ToList();
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
	 