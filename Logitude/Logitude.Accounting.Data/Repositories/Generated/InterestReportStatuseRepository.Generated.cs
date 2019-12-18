 
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
   public partial class InterestReportStatuseRepository:IRepository<InterestReportStatuse>
   {
   
        private IAccountingContext currentContext;
        public InterestReportStatuseRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestReportStatuseRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestReportStatuse GetSingle(string code)
        {
            return (from a in context.InterestReportStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestReportStatuse> GetAll()
        {
            return from a in context.InterestReportStatuses  
                   select a;
        }
				 
        public InterestReportStatuse GetSingle(EntityKeyFields entityKeys)
        {
            InterestReportStatuseKeys keys = entityKeys as InterestReportStatuseKeys;
            return (from a in context.InterestReportStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestReportStatuse entity)
        {
            onAdd();
            context.InterestReportStatuses.Add(entity);
        }

        public void Remove(InterestReportStatuse entity)
        {
            context.InterestReportStatuses.Attach(entity);
            context.InterestReportStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestReportStatuse entity)
        {
            onUpdate();
            context.InterestReportStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestReportStatuse> All()
        {
            return context.InterestReportStatuses.ToList();
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
	 