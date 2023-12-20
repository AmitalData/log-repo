using Logitude.Customs.Data;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    class Update2009TradeAgreementTypeView : ClosedTableGenericService<TradeAgreementPM>
    {
        public Update2009TradeAgreementTypeView(ICustomContext CustomContext,
          List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
          Func<ICustomContext, ICanUpdateClosedTable<TradeAgreementPM>> CreateNewUpdateServiceFunc,
          Func<ICustomContext, ICanGetAllClosedTable<TradeAgreementPM>> GetAllDbPMFunc,
          int tenant,
          bool forceUpdateUnChanged)
          : base(CustomContext, mehesTableRows,
          CreateNewUpdateServiceFunc,
          GetAllDbPMFunc,
          tenant,
          forceUpdateUnChanged)
        { }


        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, TradeAgreementPM curDbPM)
        {
            var gov = mehesTableRow.MyTradeAgreement?? new TradeAgreement();
            return base.IsEqual(mehesTableRow, curDbPM)
                && gov.CustomsBookTypeID == curDbPM.CustomsBookTypeID && gov.CountryGroupID == curDbPM.CountryGroupID;


        }

        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, TradeAgreementPM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyTradeAgreement ?? new TradeAgreement();
            curDbPM.CustomsBookTypeID = gov.CustomsBookTypeID;
            curDbPM.CountryGroupID = gov.CountryGroupID;
        }
    }
}
