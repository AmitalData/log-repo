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
    public partial class ContainerQueryService
    {

        public List<Container> ContainerCustomDataMapping(ShipmentPM MyPM, List<ShipmentPackagePM> MyEntityPMs, int Tenant, string ComputingPartnerName = "")
        {

            if (MyPM.ShipmentTypeId == "FCL" || MyPM.ShipmentTypeId == "FTL" || MyPM.ShipmentTypeId == "FCLD")
            {
                return this.ContainerDataMapping(MyEntityPMs, Tenant,ComputingPartnerName);
            }
            else
            {
                return null;
            }
        }

        public List<ShipmentPackagePM> ContainerCustomDataMappingAndValidatin(House MyHouse, List<Container> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.ContainerDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }

        public List<ShipmentPackagePM> ContainerCustomDataMappingAndValidatin(Direct MyDirect, List<Container> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.ContainerDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }
        public List<ShipmentPackagePM> ContainerCustomDataMappingAndValidatin(Master MyMaster, List<Container> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.ContainerDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }
        public List<ShipmentPackagePM> ContainerCustomDataMappingAndValidatin(Customs MyDirect, List<Container> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.ContainerDataMappingAndValidatin(MyEntities, Tenant, ComputingPartnerName, IsUpdate);
        }

    }
}