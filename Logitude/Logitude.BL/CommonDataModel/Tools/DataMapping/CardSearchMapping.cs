using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CardSearchMapping
    {
        public static void MapEntity(CardSearchPM entityPM, CardSearch poco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                poco.Id = entityPM.Id;
                poco.Tenant = entityPM.Tenant;

            }

            poco.RecordDate = entityPM.RecordDate;
            poco.Weight = entityPM.Weight;
            poco.Keyword = entityPM.Keyword;
            poco.CardId = entityPM.CardId;
            poco.PartnerTypeId = entityPM.PartnerTypeId;
            poco.InActive = entityPM.InActive;
            poco.IsCustomer = entityPM.IsCustomer;

        }
    }
}
