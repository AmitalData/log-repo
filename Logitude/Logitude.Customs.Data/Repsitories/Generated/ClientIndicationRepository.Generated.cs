 
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
   public partial class ClientIndicationRepository:IRepository<ClientIndication>
   {
   
        private ICustomContext currentContext;
        public ClientIndicationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClientIndicationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClientIndication GetSingle(string indicationid, string clientid, int tenant)
        {
            return (from a in context.ClientIndications
                    where a.IndicationId == indicationid && a.ClientId == clientid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClientIndication> GetAll(int tenant)
        {
            return from a in context.ClientIndications  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClientIndication GetSingle(EntityKeyFields entityKeys)
        {
            ClientIndicationKeys keys = entityKeys as ClientIndicationKeys;
            return (from a in context.ClientIndications
                    where a.IndicationId == keys.IndicationId && a.ClientId == keys.ClientId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClientIndication entity)
        {
            onAdd();
            context.ClientIndications.Add(entity);
        }

        public void Remove(ClientIndication entity)
        {
            context.ClientIndications.Attach(entity);
            context.ClientIndications.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClientIndication entity)
        {
            onUpdate();
            context.ClientIndications.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClientIndication> All()
        {
            return context.ClientIndications.ToList();
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
	 