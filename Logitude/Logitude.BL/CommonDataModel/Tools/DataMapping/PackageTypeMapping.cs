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
    public class PackageTypeMapping
    {
        public static void MapEntity(PackageTypePM entityPM, PackageType poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Tenant = entityPM.Tenant;
            }

            poco.Code = entityPM.Code;
            poco.EnglishName = entityPM.EnglishName;
            poco.AddedManually = entityPM.AddedManually;
            poco.IsAir = entityPM.IsAir;
            poco.IsContainer = entityPM.IsContainer;
            poco.PrintAs = entityPM.PrintAs;
            poco.IsInland = entityPM.IsInland;
            poco.IsOcean = entityPM.IsOcean;
            poco.LocalName = entityPM.LocalName;
            poco.Notes = entityPM.Notes;
            poco.Tenant = entityPM.Tenant;
            poco.ContainerSize = entityPM.ContainerSize;
            poco.TEU = entityPM.TEU;
            poco.Volume = entityPM.Volume;
            poco.InActive = entityPM.InActive;
            poco.MeasurementId = entityPM.MeasurementId;
            poco.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
            poco.IsRefrigerated = entityPM.IsRefrigerated;
            poco.IsVehicle = entityPM.IsVehicle;

            if (poco.IsContainer)
            {
                poco.IsVehicle = false;
            }

            else
            {
                poco.IsRefrigerated = false;
            }
        }
    }
}
