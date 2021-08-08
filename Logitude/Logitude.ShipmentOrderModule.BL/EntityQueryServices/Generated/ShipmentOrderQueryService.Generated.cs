 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using Logitude.ShipmentOrderModule.BL.EntityDataMappings;
using Logitude.ShipmentOrderModule.Data.Repositories;
using Logitude.ShipmentOrderModule.Data.EntityKeys;
using Logitude.ShipmentOrderModule.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.ShipmentOrderModule.BL.EntityQueryServices
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
	 