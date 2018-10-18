 
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
   public partial class TicketEscalationRepository:IRepository<TicketEscalation>
   {
   
        private ICRMContext currentContext;
        public TicketEscalationRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TicketEscalationRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  TicketEscalation GetSingle(string id, int tenant)
        {
            return (from a in context.TicketEscalations
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TicketEscalation> GetAll(int tenant)
        {
            return from a in context.TicketEscalations  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TicketEscalation GetSingle(EntityKeyFields entityKeys)
        {
            TicketEscalationKeys keys = entityKeys as TicketEscalationKeys;
            return (from a in context.TicketEscalations
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TicketEscalation entity)
        {
            onAdd();
            context.TicketEscalations.Add(entity);
        }

        public void Remove(TicketEscalation entity)
        {
            context.TicketEscalations.Attach(entity);
            context.TicketEscalations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TicketEscalation entity)
        {
            onUpdate();
            context.TicketEscalations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TicketEscalation> All()
        {
            return context.TicketEscalations.ToList();
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
	 