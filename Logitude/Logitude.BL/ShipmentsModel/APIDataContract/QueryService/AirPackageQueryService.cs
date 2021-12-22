using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel;

 namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{ 
   public partial class AirPackageQueryService
    {
   		
		public List<AirPackage> AirPackageCustomDataMapping(ShipmentPM EntityPm, List<ShipmentPackagePM> MyEntityPMs,int Tenant, string ComputingPartnerName = "")
        {
            if (EntityPm.TransportModeId == "A")
            {
                return this.AirPackageDataMapping(MyEntityPMs, Tenant, ComputingPartnerName);
            }
            else
            {
                return null;
            }
        } 

		public List<ShipmentPackagePM> AirPackageCustomDataMappingAndValidatin(House MainEntity, List<AirPackage> MyEntities,int Tenant,string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.AirPackageDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }

        public List<ShipmentPackagePM> AirPackageCustomDataMappingAndValidatin(Direct MainEntity, List<AirPackage> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.AirPackageDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }

        public List<ShipmentPackagePM> AirPackageCustomDataMappingAndValidatin(Customs MainEntity, List<AirPackage> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.AirPackageDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }

    }
}