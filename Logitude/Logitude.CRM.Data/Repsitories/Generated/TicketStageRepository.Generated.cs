 
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
   public partial class TicketStageRepository:IRepository<TicketStage>
   {
   
        private ICRMContext currentContext;
        public TicketStageRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TicketStageRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  TicketStage GetSingle(string id, int tenant)
        {
            return (from a in context.TicketStages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TicketStage> GetAll(int tenant)
        {
            return from a in context.TicketStages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TicketStage GetSingle(EntityKeyFields entityKeys)
        {
            TicketStageKeys keys = entityKeys as TicketStageKeys;
            return (from a in context.TicketStages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TicketStage entity)
        {
            onAdd();
            context.TicketStages.Add(entity);
        }

        public void Remove(TicketStage entity)
        {
            context.TicketStages.Attach(entity);
            context.TicketStages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TicketStage entity)
        {
            onUpdate();
            context.TicketStages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TicketStage> All()
        {
            return context.TicketStages.ToList();
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
	 