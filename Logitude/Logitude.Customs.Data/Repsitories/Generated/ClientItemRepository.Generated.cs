 
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
   public partial class ClientItemRepository:IRepository<ClientItem>
   {
   
        private ICustomContext currentContext;
        public ClientItemRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClientItemRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClientItem GetSingle(string itemcode, string clientcode, string id, string itemkey, int tenant)
        {
            return (from a in context.ClientItems
                    where a.ItemCode == itemcode && a.ClientCode == clientcode && a.Id == id && a.ItemKey == itemkey && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClientItem> GetAll(int tenant)
        {
            return from a in context.ClientItems  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClientItem GetSingle(EntityKeyFields entityKeys)
        {
            ClientItemKeys keys = entityKeys as ClientItemKeys;
            return (from a in context.ClientItems
                    where a.ItemCode == keys.ItemCode && a.ClientCode == keys.ClientCode && a.Id == keys.Id && a.ItemKey == keys.ItemKey
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClientItem entity)
        {
            onAdd();
            context.ClientItems.Add(entity);
        }

        public void Remove(ClientItem entity)
        {
            context.ClientItems.Attach(entity);
            context.ClientItems.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClientItem entity)
        {
            onUpdate();
            context.ClientItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClientItem> All()
        {
            return context.ClientItems.ToList();
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
	 