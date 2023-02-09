 
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
   public partial class InterestReportLineRepository:IRepository<InterestReportLine>
   {
   
        private IAccountingContext currentContext;
        public InterestReportLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestReportLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestReportLine GetSingle(string interestreportid, string interesttransactionid, int tenant)
        {
            return (from a in context.InterestReportLines
                    where a.InterestReportId == interestreportid && a.InterestTransactionId == interesttransactionid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestReportLine> GetAll(int tenant)
        {
            return from a in context.InterestReportLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public InterestReportLine GetSingle(EntityKeyFields entityKeys)
        {
            InterestReportLineKeys keys = entityKeys as InterestReportLineKeys;
            return (from a in context.InterestReportLines
                    where a.InterestReportId == keys.InterestReportId && a.InterestTransactionId == keys.InterestTransactionId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestReportLine entity)
        {
            onAdd();
            context.InterestReportLines.Add(entity);
        }

        public void Remove(InterestReportLine entity)
        {
            context.InterestReportLines.Attach(entity);
            context.InterestReportLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestReportLine entity)
        {
            onUpdate();
            context.InterestReportLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestReportLine> All()
        {
            return context.InterestReportLines.ToList();
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
	 