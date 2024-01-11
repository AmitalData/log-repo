 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsDocumentsTicketRepository:IRepository<CustomsDocumentsTicket>
   {
   
        private ICustomContext currentContext;
        public CustomsDocumentsTicketRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsDocumentsTicketRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsDocumentsTicket GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsDocumentsTickets
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsDocumentsTicket> GetAll(int tenant)
        {
            return from a in context.CustomsDocumentsTickets  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsDocumentsTicket GetSingle(EntityKeyFields entityKeys)
        {
            CustomsDocumentsTicketKeys keys = entityKeys as CustomsDocumentsTicketKeys;
            return (from a in context.CustomsDocumentsTickets
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsDocumentsTicket entity)
        {
            onAdd();
            context.CustomsDocumentsTickets.Add(entity);
        }

        public void Remove(CustomsDocumentsTicket entity)
        {
            context.CustomsDocumentsTickets.Attach(entity);
            context.CustomsDocumentsTickets.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsDocumentsTicket entity)
        {
            onUpdate();
            context.CustomsDocumentsTickets.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsDocumentsTicket> All()
        {
            return context.CustomsDocumentsTickets.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 