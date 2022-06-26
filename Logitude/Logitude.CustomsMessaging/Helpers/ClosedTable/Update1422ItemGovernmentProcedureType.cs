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
    class Update1422ItemGovernmentProcedureType : ClosedTableGenericService<ItemGovernmentProcedureTypePM>
    {
        public Update1422ItemGovernmentProcedureType(ICustomContext CustomContext,
          List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
          Func<ICustomContext, ICanUpdateClosedTable<ItemGovernmentProcedureTypePM>> CreateNewUpdateServiceFunc,
          Func<ICustomContext, ICanGetAllClosedTable<ItemGovernmentProcedureTypePM>> GetAllDbPMFunc,
          int tenant,
          bool forceUpdateUnChanged)
          : base(CustomContext, mehesTableRows,
          CreateNewUpdateServiceFunc,
          GetAllDbPMFunc,
          tenant,
          forceUpdateUnChanged)
        { }


        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, ItemGovernmentProcedureTypePM curDbPM)
        {
            var gov = mehesTableRow.MyItemGovernmentProcedureType ?? new ItemGovernmentProcedureType();
            return base.IsEqual(mehesTableRow, curDbPM)
                && gov.LeadDocumentTypeID == curDbPM.LeadDocumentTypeID;


        }

        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, ItemGovernmentProcedureTypePM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyItemGovernmentProcedureType ?? new ItemGovernmentProcedureType();
            curDbPM.LeadDocumentTypeID = gov.LeadDocumentTypeID;
        }
    }
}
