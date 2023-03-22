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
   public partial class OceanOrInlandPackageQueryService
   {
		public List<OceanOrInlandPackage> OceanOrInlandPackageCustomDataMapping(ShipmentPM MyPM, List<ShipmentPackagePM> MyEntityPMs, int Tenant,string ComputingPartnerName="")
        {
            if (!string.IsNullOrEmpty(MyPM.ShipmentTypeId) && (MyPM.ShipmentTypeId.Contains("LCL") || MyPM.ShipmentTypeId.Contains("LTL")))
            {
                return this.OceanOrInlandPackageDataMapping(MyEntityPMs, Tenant, ComputingPartnerName);
            }
            else
            {
                return null;
            }
        } 

		public List<ShipmentPackagePM> OceanOrInlandPackageCustomDataMappingAndValidatin(House myHouse,List<OceanOrInlandPackage> MyEntities,int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.OceanOrInlandPackageDataMappingAndValidatin(MyEntities,Tenant, ComputingPartnerName, IsUpdate);   
        }

        public List<ShipmentPackagePM> OceanOrInlandPackageCustomDataMappingAndValidatin(Direct myDirect, List<OceanOrInlandPackage> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.OceanOrInlandPackageDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }
        public List<ShipmentPackagePM> OceanOrInlandPackageCustomDataMappingAndValidatin(Master myMaster, List<OceanOrInlandPackage> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.OceanOrInlandPackageDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }

        public List<ShipmentPackagePM> OceanOrInlandPackageCustomDataMappingAndValidatin(Customs myDirect, List<OceanOrInlandPackage> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.OceanOrInlandPackageDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }

    }
}