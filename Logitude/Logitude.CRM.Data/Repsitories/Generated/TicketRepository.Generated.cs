 
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
   public partial class TicketRepository:IRepository<Ticket>
   {
   
        private ICRMContext currentContext;
        public TicketRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TicketRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  Ticket GetSingle(string id, int tenant)
        {
            return (from a in context.Tickets
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Ticket> GetAll(int tenant)
        {
            return from a in context.Tickets  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Ticket GetSingle(EntityKeyFields entityKeys)
        {
            TicketKeys keys = entityKeys as TicketKeys;
            return (from a in context.Tickets
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Ticket entity)
        {
            onAdd();
            context.Tickets.Add(entity);
        }

        public void Remove(Ticket entity)
        {
            context.Tickets.Attach(entity);
            context.Tickets.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Ticket entity)
        {
            onUpdate();
            context.Tickets.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Ticket> All()
        {
            return context.Tickets.ToList();
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
	 