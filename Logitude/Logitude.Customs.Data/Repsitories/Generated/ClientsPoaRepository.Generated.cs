 
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
   public partial class ClientsPoaRepository:IRepository<ClientsPoa>
   {
   
        private ICustomContext currentContext;
        public ClientsPoaRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClientsPoaRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClientsPoa GetSingle(string id, int tenant)
        {
            return (from a in context.ClientsPoas
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClientsPoa> GetAll(int tenant)
        {
            return from a in context.ClientsPoas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClientsPoa GetSingle(EntityKeyFields entityKeys)
        {
            ClientsPoaKeys keys = entityKeys as ClientsPoaKeys;
            return (from a in context.ClientsPoas
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClientsPoa entity)
        {
            onAdd();
            context.ClientsPoas.Add(entity);
        }

        public void Remove(ClientsPoa entity)
        {
            context.ClientsPoas.Attach(entity);
            context.ClientsPoas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClientsPoa entity)
        {
            onUpdate();
            context.ClientsPoas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClientsPoa> All()
        {
            return context.ClientsPoas.ToList();
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
	 