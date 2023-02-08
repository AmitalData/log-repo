 
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
   public partial class InterestReportRepository:IRepository<InterestReport>
   {
   
        private IAccountingContext currentContext;
        public InterestReportRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestReportRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestReport GetSingle(string id, int tenant)
        {
            return (from a in context.InterestReports
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestReport> GetAll(int tenant)
        {
            return from a in context.InterestReports  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public InterestReport GetSingle(EntityKeyFields entityKeys)
        {
            InterestReportKeys keys = entityKeys as InterestReportKeys;
            return (from a in context.InterestReports
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestReport entity)
        {
            onAdd();
            context.InterestReports.Add(entity);
        }

        public void Remove(InterestReport entity)
        {
            context.InterestReports.Attach(entity);
            context.InterestReports.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestReport entity)
        {
            onUpdate();
            context.InterestReports.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestReport> All()
        {
            return context.InterestReports.ToList();
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
	 