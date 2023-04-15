 
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
   public partial class EventRemarkRepository:IRepository<EventRemark>
   {
   
        private IAccountingContext currentContext;
        public EventRemarkRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public EventRemarkRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  EventRemark GetSingle(string id, int tenant)
        {
            return (from a in context.EventRemarks
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<EventRemark> GetAll(int tenant)
        {
            return from a in context.EventRemarks  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public EventRemark GetSingle(EntityKeyFields entityKeys)
        {
            EventRemarkKeys keys = entityKeys as EventRemarkKeys;
            return (from a in context.EventRemarks
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(EventRemark entity)
        {
            onAdd();
            context.EventRemarks.Add(entity);
        }

        public void Remove(EventRemark entity)
        {
            context.EventRemarks.Attach(entity);
            context.EventRemarks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(EventRemark entity)
        {
            onUpdate();
            context.EventRemarks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EventRemark> All()
        {
            return context.EventRemarks.ToList();
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
	 