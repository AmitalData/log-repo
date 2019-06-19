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
    public class CountryMapping
    {
        public static void MapEntity(CountryPM entityPM, Country poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Tenant = entityPM.Tenant;
                poco.Code = entityPM.Code;
            }

            poco.AddedManually = entityPM.AddedManually;
            poco.EnglishName = entityPM.EnglishName;
            poco.GlobalZoneId = entityPM.GlobalZoneId;
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.Notes = entityPM.Notes;
            poco.EC = entityPM.EC;
            poco.HasStates = entityPM.HasStates;
            poco.IsStateRequired = entityPM.IsStateRequired;
            poco.HasCitiesList = entityPM.HasCitiesList;
            poco.IsNorthAmerica = entityPM.IsNorthAmerica;
            poco.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
        }
    }
}