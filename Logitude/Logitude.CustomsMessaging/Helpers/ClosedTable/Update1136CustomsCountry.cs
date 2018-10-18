using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class Update1136CustomsCountry: ClosedTableGenericService<CustomsCountryPM>
    {
        public Update1136CustomsCountry(ICustomContext CustomContext,
            ///string tableId,
            //SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<CustomsCountryPM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<CustomsCountryPM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
            : base(CustomContext, mehesTableRows,
            CreateNewUpdateServiceFunc,
            GetAllDbPMFunc,
            tenant,
            forceUpdateUnChanged)
        {

        }

        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CustomsCountryPM curDbPM)
        {
            return base.IsEqual(mehesTableRow, curDbPM) && curDbPM.MalamId == mehesTableRow.malamID.ToString();
        }
        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CustomsCountryPM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            curDbPM.MalamId = mehesTableRow.malamID.ToString();

        }
    }
}
