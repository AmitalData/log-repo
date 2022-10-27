 
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
   public partial class TicketTypeRepository:IRepository<TicketType>
   {
   
        private ICRMContext currentContext;
        public TicketTypeRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TicketTypeRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  TicketType GetSingle(string id, int tenant)
        {
            return (from a in context.TicketTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TicketType> GetAll(int tenant)
        {
            return from a in context.TicketTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TicketType GetSingle(EntityKeyFields entityKeys)
        {
            TicketTypeKeys keys = entityKeys as TicketTypeKeys;
            return (from a in context.TicketTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TicketType entity)
        {
            onAdd();
            context.TicketTypes.Add(entity);
        }

        public void Remove(TicketType entity)
        {
            context.TicketTypes.Attach(entity);
            context.TicketTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TicketType entity)
        {
            onUpdate();
            context.TicketTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TicketType> All()
        {
            return context.TicketTypes.ToList();
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
	 