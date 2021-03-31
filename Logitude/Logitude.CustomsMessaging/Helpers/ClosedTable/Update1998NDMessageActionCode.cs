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
    public class Update1998NDMessageActionCode : ClosedTableGenericService<NDMessageActionCodePM>
    {
        public Update1998NDMessageActionCode(ICustomContext CustomContext,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<NDMessageActionCodePM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<NDMessageActionCodePM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
            : base(CustomContext, mehesTableRows,
            CreateNewUpdateServiceFunc,
            GetAllDbPMFunc,
            tenant,
            forceUpdateUnChanged)
        {

        }

        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, NDMessageActionCodePM curDbPM)
        {
            var gov = mehesTableRow.MyNDMessageActionCode ?? new NDMessageActionCode();
            return base.IsEqual(mehesTableRow, curDbPM);
        }
        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, NDMessageActionCodePM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyNDMessageActionCode ?? new NDMessageActionCode();
            curDbPM.StartDate = gov.StartDate;
            curDbPM.EndDate = gov.EndDate;

        }
    }
}

