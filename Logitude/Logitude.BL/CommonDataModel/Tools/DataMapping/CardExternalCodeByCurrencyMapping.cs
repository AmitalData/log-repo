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
    public class CardExternalCodeByCurrencyMapping
    {
        public static void MapEntity(CardExternalCodeByCurrencyPM entityPM, CardExternalCodeByCurrency poco, bool isNewState)
        {
            if (isNewState)
            {
              
                poco.Tenant = entityPM.Tenant;
            }

            poco.CardId = entityPM.CardId;
            poco.CurrencyId = entityPM.CurrencyId;
            poco.ExternalRecievableTableId = entityPM.ExternalRecievableTableId;
            poco.ExternalPayableTableId = entityPM.ExternalPayableTableId;


        }
    }
}
