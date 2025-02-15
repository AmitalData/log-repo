 
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
   public partial class Aur_TimesheetRepository:IRepository<Aur_Timesheet>
   {
   
        private IAccountingContext currentContext;
        public Aur_TimesheetRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Aur_TimesheetRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Aur_Timesheet GetSingle(int line, string paymentid, int tenant)
        {
            return (from a in context.Aur_Timesheets
                    where a.Line == line && a.PaymentId == paymentid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Aur_Timesheet> GetAll(int tenant)
        {
            return from a in context.Aur_Timesheets  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Aur_Timesheet GetSingle(EntityKeyFields entityKeys)
        {
            Aur_TimesheetKeys keys = entityKeys as Aur_TimesheetKeys;
            return (from a in context.Aur_Timesheets
                    where a.Line == keys.Line && a.PaymentId == keys.PaymentId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Aur_Timesheet entity)
        {
            onAdd();
            context.Aur_Timesheets.Add(entity);
        }

        public void Remove(Aur_Timesheet entity)
        {
            context.Aur_Timesheets.Attach(entity);
            context.Aur_Timesheets.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Aur_Timesheet entity)
        {
            onUpdate();
            context.Aur_Timesheets.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Aur_Timesheet> All()
        {
            return context.Aur_Timesheets.ToList();
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
	 