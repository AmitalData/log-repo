using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CardCurrenciesAccountingMapping
    {
        public static void MapEntity(CardCurrenciesAccountingPM entityPM, CardCurrenciesAccounting poco, bool isNewState)
        {
            if (isNewState)
            {
                poco.Tenant = entityPM.Tenant;
            }

            poco.CardId = entityPM.CardId;
            poco.CurrencyId = entityPM.CurrencyId;
            poco.PayableDebitAccount = entityPM.PayableDebitAccount;
            poco.ReceivableCreditAccount = entityPM.ReceivableCreditAccount;
        }
    }
}
