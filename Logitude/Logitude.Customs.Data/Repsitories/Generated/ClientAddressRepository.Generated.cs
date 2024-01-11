 
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
   public partial class ClientAddressRepository:IRepository<ClientAddress>
   {
   
        private ICustomContext currentContext;
        public ClientAddressRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClientAddressRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClientAddress GetSingle(string clientid, string addressid, int tenant)
        {
            return (from a in context.ClientAddresses
                    where a.ClientId == clientid && a.AddressId == addressid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClientAddress> GetAll(int tenant)
        {
            return from a in context.ClientAddresses  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClientAddress GetSingle(EntityKeyFields entityKeys)
        {
            ClientAddressKeys keys = entityKeys as ClientAddressKeys;
            return (from a in context.ClientAddresses
                    where a.ClientId == keys.ClientId && a.AddressId == keys.AddressId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClientAddress entity)
        {
            onAdd();
            context.ClientAddresses.Add(entity);
        }

        public void Remove(ClientAddress entity)
        {
            context.ClientAddresses.Attach(entity);
            context.ClientAddresses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClientAddress entity)
        {
            onUpdate();
            context.ClientAddresses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClientAddress> All()
        {
            return context.ClientAddresses.ToList();
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
	 