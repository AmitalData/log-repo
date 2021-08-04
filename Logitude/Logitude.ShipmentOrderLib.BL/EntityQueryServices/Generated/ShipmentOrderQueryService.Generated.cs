 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.ShipmentOrderLib.Data.EntityPOCOs;
using Logitude.ShipmentOrderLib.BL.EntityPMs;
using Logitude.ShipmentOrderLib.BL.EntityDataMappings;
using Logitude.ShipmentOrderLib.Data.Repositories;
using Logitude.ShipmentOrderLib.Data.EntityKeys;
using Logitude.ShipmentOrderLib.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.ShipmentOrderLib.BL.EntityQueryServices
{ 
   public partial class ShipmentOrderQueryService: EntityQueryService<ShipmentOrder,ShipmentOrderKeys,ShipmentOrderPM,object,ShipmentOrderKeys>
   {
   
        ShipmentOrderRepository repository;
		IShipmentOrderContext  context;
        public ShipmentOrderQueryService(int tenant)
        {
		    context = ShipmentOrderContext.GetContext(tenant);
            MainContext = context;
            repository = new ShipmentOrderRepository(context);
            Repository = repository;
            mapping = new ShipmentOrderDataMapping();
        }

        public ShipmentOrderQueryService(ShipmentOrderRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ShipmentOrderDataMapping();
        }

        public ShipmentOrderQueryService(IShipmentOrderContext context)
        {
            this.repository = new ShipmentOrderRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ShipmentOrderDataMapping();
        }
		 
		public  ShipmentOrderPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ShipmentOrderKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ShipmentOrder entityPOCO)
        {
            ShipmentOrderKeys entityKeys = new ShipmentOrderKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 