 
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
   public partial class TicketSourceRepository:IRepository<TicketSource>
   {
   
        private ICRMContext currentContext;
        public TicketSourceRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TicketSourceRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  TicketSource GetSingle(string code)
        {
            return (from a in context.TicketSources
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TicketSource> GetAll()
        {
            return from a in context.TicketSources  
                   select a;
        }
				 
        public TicketSource GetSingle(EntityKeyFields entityKeys)
        {
            TicketSourceKeys keys = entityKeys as TicketSourceKeys;
            return (from a in context.TicketSources
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TicketSource entity)
        {
            onAdd();
            context.TicketSources.Add(entity);
        }

        public void Remove(TicketSource entity)
        {
            context.TicketSources.Attach(entity);
            context.TicketSources.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TicketSource entity)
        {
            onUpdate();
            context.TicketSources.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TicketSource> All()
        {
            return context.TicketSources.ToList();
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
	 