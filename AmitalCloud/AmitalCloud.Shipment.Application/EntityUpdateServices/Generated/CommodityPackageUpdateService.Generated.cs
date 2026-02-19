 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using POCO = AmitalCloud.Shipment.Data.EntityPOCOs;
using AmitalCloud.Shipment.Def.EntityPMs;
using AmitalCloud.Shipment.BL.EntityDataMappings;
using AmitalCloud.Shipment.Data.Repositories;
using AmitalCloud.Shipment.Data.EntityKeys;
using AmitalCloud.Shipment.Data;
using AmitalCloud.Shipment.Data.Context;

namespace AmitalCloud.Shipment.BL.EntityUpdateServices
{ 
   public partial class CommodityPackageUpdateService:EntityUpdateService<POCO.CommodityPackage,CommodityPackagePM,ShipmentCommodityPM>
   {
   
        CommodityPackageRepository entityRepository;
        public CommodityPackageUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IShipmentContext  context = mainContext as ShipmentContext;
            context = context ??mainContext as IShipmentContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CommodityPackageDataMapping();
            Repository = new CommodityPackageRepository(context);
        }

       
        private IShipmentContext currentContext;
        public CommodityPackageUpdateService(int tenant)
        {
            currentContext = ShipmentContext.GetContext(tenant);
        }

        public CommodityPackageUpdateService(IShipmentContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CommodityPackagePM entityPM)
        {
            CommodityPackageKeys entityKeys = new CommodityPackageKeys() { PackageTypeId = entityPM.PackageTypeId, CommodityId = entityPM.CommodityId };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(CommodityPackagePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(CommodityPackagePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 