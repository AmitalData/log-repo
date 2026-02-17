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
    public class CustomerProductMapping
    {
        public static void MapEntity(CustomerProductPM entityPM, CustomerProduct poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.CustomerId = entityPM.CustomerId;
                poco.ProductTypeCode = entityPM.ProductTypeCode;
                poco.Tenant = entityPM.Tenant;
            }

            poco.Notes = entityPM.Notes;
            poco.PotentialChargeableWeight = entityPM.PotentialChargeableWeight;
            poco.CommitmentChargeableWeight = entityPM.CommitmentChargeableWeight;;
            poco.PotentialNumberOfShipments = entityPM.PotentialNumberOfShipments;
            poco.CommitmentNumberOfShipments = entityPM.CommitmentNumberOfShipments;
            poco.PotentialTEU = entityPM.PotentialTEU;
            poco.CommitmentTEU = entityPM.CommitmentTEU;
            poco.PotentialRevenue = entityPM.PotentialRevenue;
            poco.CommitmentRevenue = entityPM.CommitmentRevenue;
            poco.LastShipmentDate = entityPM.LastShipmentDate;
            poco.PrepaidCollectId = entityPM.PrepaidCollectId;
            poco.NotesRightToLeft = entityPM.NotesRightToLeft;
        }
    }
}