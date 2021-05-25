 
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
   public partial class TicketSeverityRepository:IRepository<TicketSeverity>
   {
   
        private ICRMContext currentContext;
        public TicketSeverityRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TicketSeverityRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  TicketSeverity GetSingle(string id, int tenant)
        {
            return (from a in context.TicketSeverities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public TicketSeverity GetSingleByName(string name, int tenant)
        {
            return (from a in context.TicketSeverities
                    where a.Name == name && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TicketSeverity> GetAll(int tenant)
        {
            return from a in context.TicketSeverities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TicketSeverity GetSingle(EntityKeyFields entityKeys)
        {
            TicketSeverityKeys keys = entityKeys as TicketSeverityKeys;
            return (from a in context.TicketSeverities
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TicketSeverity entity)
        {
            onAdd();
            context.TicketSeverities.Add(entity);
        }

        public void Remove(TicketSeverity entity)
        {
            context.TicketSeverities.Attach(entity);
            context.TicketSeverities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TicketSeverity entity)
        {
            onUpdate();
            context.TicketSeverities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TicketSeverity> All()
        {
            return context.TicketSeverities.ToList();
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
	 