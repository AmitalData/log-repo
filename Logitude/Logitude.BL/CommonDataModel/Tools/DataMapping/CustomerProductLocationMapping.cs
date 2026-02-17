using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CustomerProductLocationMapping
    {
        public static void MapEntity(CustomerProductLocationPM entityPM, CustomerProductLocation poco, bool isNewEntity)
        {
            poco.CustomerId = entityPM.CustomerId;
            poco.ProductTypeCode = entityPM.ProductTypeCode;
            poco.CountryId = entityPM.CountryId;
            poco.Tenant = entityPM.Tenant;
            poco.PotentialChargeableWeight = entityPM.PotentialChargeableWeight;
            poco.CommitmentChargeableWeight = entityPM.CommitmentChargeableWeight; ;
            poco.PotentialNumberOfShipments = entityPM.PotentialNumberOfShipments;
            poco.CommitmentNumberOfShipments = entityPM.CommitmentNumberOfShipments;
            poco.PotentialTEU = entityPM.PotentialTEU;
            poco.CommitmentTEU = entityPM.CommitmentTEU;
            poco.PotentialRevenue = entityPM.PotentialRevenue;
            poco.CommitmentRevenue = entityPM.CommitmentRevenue;
        }
    }
}