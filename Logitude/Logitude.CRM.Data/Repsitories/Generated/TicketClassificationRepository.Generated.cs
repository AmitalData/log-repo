 
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
   public partial class TicketClassificationRepository:IRepository<TicketClassification>
   {
   
        private ICRMContext currentContext;
        public TicketClassificationRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TicketClassificationRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  TicketClassification GetSingle(string id, int tenant)
        {
            return (from a in context.TicketClassifications
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TicketClassification> GetAll(int tenant)
        {
            return from a in context.TicketClassifications  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TicketClassification GetSingle(EntityKeyFields entityKeys)
        {
            TicketClassificationKeys keys = entityKeys as TicketClassificationKeys;
            return (from a in context.TicketClassifications
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TicketClassification entity)
        {
            onAdd();
            context.TicketClassifications.Add(entity);
        }

        public void Remove(TicketClassification entity)
        {
            context.TicketClassifications.Attach(entity);
            context.TicketClassifications.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TicketClassification entity)
        {
            onUpdate();
            context.TicketClassifications.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TicketClassification> All()
        {
            return context.TicketClassifications.ToList();
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
	 