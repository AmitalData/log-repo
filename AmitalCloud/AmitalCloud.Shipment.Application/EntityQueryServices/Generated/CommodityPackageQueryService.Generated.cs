 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using AmitalCloud.Shipment.Data.EntityPOCOs;
using AmitalCloud.Shipment.Def.EntityPMs;
using AmitalCloud.Shipment.BL.EntityDataMappings;
using AmitalCloud.Shipment.Data.Repositories;
using AmitalCloud.Shipment.Data.EntityKeys;
using AmitalCloud.Shipment.Data;
using AmitalCloud.Shipment.Data.Context;


namespace AmitalCloud.Shipment.BL.EntityQueryServices
{ 
   public partial class CommodityPackageQueryService: EntityQueryService<CommodityPackage,CommodityPackageKeys,CommodityPackagePM,ShipmentCommodityPM,ShipmentCommodityKeys>
   {
   
        CommodityPackageRepository repository;
		IShipmentContext  context;
        public CommodityPackageQueryService(int tenant)
        {
		    context = ShipmentContext.GetContext(tenant);
            MainContext = context;
            repository = new CommodityPackageRepository(context);
            Repository = repository;
            mapping = new CommodityPackageDataMapping();
        }

        public CommodityPackageQueryService(CommodityPackageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CommodityPackageDataMapping();
        }

        public CommodityPackageQueryService(IShipmentContext context)
        {
            this.repository = new CommodityPackageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CommodityPackageDataMapping();
        }
		 
		public  CommodityPackagePM GetSingle(string packagetypeid, string commodityid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CommodityPackageKeys(){ PackageTypeId = packagetypeid, CommodityId = commodityid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CommodityPackage entityPOCO)
        {
            CommodityPackageKeys entityKeys = new CommodityPackageKeys() { PackageTypeId = entityPOCO.PackageTypeId, CommodityId = entityPOCO.CommodityId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 