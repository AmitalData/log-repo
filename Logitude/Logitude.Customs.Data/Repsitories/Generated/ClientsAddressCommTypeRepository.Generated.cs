 
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
   public partial class ClientsAddressCommTypeRepository:IRepository<ClientsAddressCommType>
   {
   
        private ICustomContext currentContext;
        public ClientsAddressCommTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClientsAddressCommTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClientsAddressCommType GetSingle(string clientid, string addressid, int line, int tenant)
        {
            return (from a in context.ClientsAddressCommTypes
                    where a.ClientId == clientid && a.AddressId == addressid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClientsAddressCommType> GetAll(int tenant)
        {
            return from a in context.ClientsAddressCommTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClientsAddressCommType GetSingle(EntityKeyFields entityKeys)
        {
            ClientsAddressCommTypeKeys keys = entityKeys as ClientsAddressCommTypeKeys;
            return (from a in context.ClientsAddressCommTypes
                    where a.ClientId == keys.ClientId && a.AddressId == keys.AddressId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClientsAddressCommType entity)
        {
            onAdd();
            context.ClientsAddressCommTypes.Add(entity);
        }

        public void Remove(ClientsAddressCommType entity)
        {
            context.ClientsAddressCommTypes.Attach(entity);
            context.ClientsAddressCommTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClientsAddressCommType entity)
        {
            onUpdate();
            context.ClientsAddressCommTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClientsAddressCommType> All()
        {
            return context.ClientsAddressCommTypes.ToList();
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
	 