 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class SupportMailboxRepository:IRepository<SupportMailbox>
   {
   
        private ICRMContext currentContext;
        public SupportMailboxRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public SupportMailboxRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupportMailbox GetSingle(string id, int tenant)
        {
            return (from a in context.SupportMailboxes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SupportMailbox> GetAll(int tenant)
        {
            return from a in context.SupportMailboxes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SupportMailbox GetSingle(EntityKeyFields entityKeys)
        {
            SupportMailboxKeys keys = entityKeys as SupportMailboxKeys;
            return (from a in context.SupportMailboxes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupportMailbox entity)
        {
            onAdd();
            context.SupportMailboxes.Add(entity);
        }

        public void Remove(SupportMailbox entity)
        {
            context.SupportMailboxes.Attach(entity);
            context.SupportMailboxes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupportMailbox entity)
        {
            onUpdate();
            context.SupportMailboxes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupportMailbox> All()
        {
            return context.SupportMailboxes.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 