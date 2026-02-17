 
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
   public partial class ClientRepository:IRepository<Client>
   {
   
        private ICustomContext currentContext;
        public ClientRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClientRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Client GetSingle(string id, int tenant)
        {
            return (from a in context.Clients
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Client> GetAll(int tenant)
        {
            return from a in context.Clients  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Client GetSingle(EntityKeyFields entityKeys)
        {
            ClientKeys keys = entityKeys as ClientKeys;
            return (from a in context.Clients
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Client entity)
        {
            onAdd();
            context.Clients.Add(entity);
        }

        public void Remove(Client entity)
        {
            context.Clients.Attach(entity);
            context.Clients.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Client entity)
        {
            onUpdate();
            context.Clients.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Client> All()
        {
            return context.Clients.ToList();
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
	 