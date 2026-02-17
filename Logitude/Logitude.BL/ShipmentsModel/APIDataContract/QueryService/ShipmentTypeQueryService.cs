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
    public partial class ShipmentTypeQueryService
    {

        public ShipmentType ShipmentTypeCustomDataMapping(string EntityId, int Tenant)
        {
            ShipmentTypePM MyEntityPM = query.GetSinglePM(EntityId, Tenant);
            var temp = new ShipmentType();
            if (MyEntityPM.Id.Contains("LCL"))
            {
                temp.Code = "LCL";
            }
            else if (MyEntityPM.Id.Contains("FCL"))
            {
                temp.Code = "FCL";
            }
            else
            {
                temp.Code = MyEntityPM.Id;
            }

            temp.Name = MyEntityPM.Name;
            return temp;
        }

        public ShipmentTypePM ShipmentTypeCustomDataMappingAndValidatin(ShipmentType MyEntity, int Tenant)
        {
            var temp = new ShipmentTypePM();
            if (!string.IsNullOrEmpty(MyEntity.Code))
            {
                temp = query.GetSinglePM(MyEntity.Code);
                if (temp == null)
                {
                    temp = query.GetSinglePM(MyEntity.Code + "D");
                }
            }
            if (string.IsNullOrEmpty(temp.Id))
            {
                temp.Id = MyEntity.Code;
            }
            temp.Name = MyEntity.Name;
            return temp;

        }

    }
}