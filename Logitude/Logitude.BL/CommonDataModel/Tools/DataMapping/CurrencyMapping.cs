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
    public class CurrencyMapping
    {
        public static void MapEntity(CurrencyPM entityPM, Currency poco, bool isNewState)
        {
            poco.AddedManually = entityPM.AddedManually;
            poco.Code = entityPM.Code;
            poco.EnglishName = entityPM.EnglishName;
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.Notes = entityPM.Notes;
            poco.Tenant = entityPM.Tenant;
            poco.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
            poco.AccountingExternalCode = entityPM.AccountingExternalCode;
            poco.Sign = entityPM.Sign;
        }
    }
}