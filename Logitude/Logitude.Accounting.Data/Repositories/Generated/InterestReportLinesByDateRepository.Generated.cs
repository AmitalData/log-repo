 
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
   public partial class InterestReportLinesByDateRepository:IRepository<InterestReportLinesByDate>
   {
   
        private IAccountingContext currentContext;
        public InterestReportLinesByDateRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestReportLinesByDateRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestReportLinesByDate GetSingle(string id, int tenant)
        {
            return (from a in context.InterestReportLinesByDates
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestReportLinesByDate> GetAll(int tenant)
        {
            return from a in context.InterestReportLinesByDates  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public InterestReportLinesByDate GetSingle(EntityKeyFields entityKeys)
        {
            InterestReportLinesByDateKeys keys = entityKeys as InterestReportLinesByDateKeys;
            return (from a in context.InterestReportLinesByDates
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestReportLinesByDate entity)
        {
            onAdd();
            context.InterestReportLinesByDates.Add(entity);
        }

        public void Remove(InterestReportLinesByDate entity)
        {
            context.InterestReportLinesByDates.Attach(entity);
            context.InterestReportLinesByDates.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestReportLinesByDate entity)
        {
            onUpdate();
            context.InterestReportLinesByDates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestReportLinesByDate> All()
        {
            return context.InterestReportLinesByDates.ToList();
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
	 