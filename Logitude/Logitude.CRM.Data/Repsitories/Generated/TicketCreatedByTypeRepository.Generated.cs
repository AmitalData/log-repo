 
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
   public partial class TicketCreatedByTypeRepository:IRepository<TicketCreatedByType>
   {
   
        private ICRMContext currentContext;
        public TicketCreatedByTypeRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TicketCreatedByTypeRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  TicketCreatedByType GetSingle(string code)
        {
            return (from a in context.TicketCreatedByTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TicketCreatedByType> GetAll()
        {
            return from a in context.TicketCreatedByTypes  
                   select a;
        }
				 
        public TicketCreatedByType GetSingle(EntityKeyFields entityKeys)
        {
            TicketCreatedByTypeKeys keys = entityKeys as TicketCreatedByTypeKeys;
            return (from a in context.TicketCreatedByTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TicketCreatedByType entity)
        {
            onAdd();
            context.TicketCreatedByTypes.Add(entity);
        }

        public void Remove(TicketCreatedByType entity)
        {
            context.TicketCreatedByTypes.Attach(entity);
            context.TicketCreatedByTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TicketCreatedByType entity)
        {
            onUpdate();
            context.TicketCreatedByTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TicketCreatedByType> All()
        {
            return context.TicketCreatedByTypes.ToList();
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
	 