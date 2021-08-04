 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.ShipmentOrderModule.Data.Repositories
{
   public partial class ShipmentOrderRepository:IRepository<ShipmentOrder>
   {
   
        private IShipmentOrderContext currentContext;
        public ShipmentOrderRepository(int tenant)
        {
            currentContext = ShipmentOrderContext.GetContext(tenant);
        }

        public ShipmentOrderRepository(IShipmentOrderContext context)
        {
            currentContext = context;
        }

		 
		
		public  ShipmentOrder GetSingle(string id, int tenant)
        {
            return (from a in context.ShipmentOrders
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ShipmentOrder> GetAll(int tenant)
        {
            return from a in context.ShipmentOrders  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ShipmentOrder GetSingle(EntityKeyFields entityKeys)
        {
            ShipmentOrderKeys keys = entityKeys as ShipmentOrderKeys;
            return (from a in context.ShipmentOrders
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ShipmentOrder entity)
        {
            onAdd();
            context.ShipmentOrders.Add(entity);
        }

        public void Remove(ShipmentOrder entity)
        {
            context.ShipmentOrders.Attach(entity);
            context.ShipmentOrders.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ShipmentOrder entity)
        {
            onUpdate();
            context.ShipmentOrders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShipmentOrder> All()
        {
            return context.ShipmentOrders.ToList();
        }

        private IShipmentOrderContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 