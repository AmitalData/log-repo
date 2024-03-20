using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    class Update1977OriginCriterionTypeView : ClosedTableGenericService<OriginCriterionPM>
    {
        public Update1977OriginCriterionTypeView(ICustomContext CustomContext,
          List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
          Func<ICustomContext, ICanUpdateClosedTable<OriginCriterionPM>> CreateNewUpdateServiceFunc,
          Func<ICustomContext, ICanGetAllClosedTable<OriginCriterionPM>> GetAllDbPMFunc,
          int tenant,
          bool forceUpdateUnChanged)
          : base(CustomContext, mehesTableRows,
          CreateNewUpdateServiceFunc,
          GetAllDbPMFunc,
          tenant,
          forceUpdateUnChanged)
        { }


        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, OriginCriterionPM curDbPM)
        {
            var gov = mehesTableRow.MyOriginCriterion ?? new OriginCriterion();
            return base.IsEqual(mehesTableRow, curDbPM)
                && gov.CertificateOfOriginTypeCodeID == curDbPM.CertificateOfOriginTypeCodeID && gov.OriginCriterionCode == curDbPM.OriginCriterionCode;


        }

        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, OriginCriterionPM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyOriginCriterion ?? new OriginCriterion();
            curDbPM.CertificateOfOriginTypeCodeID = gov.CertificateOfOriginTypeCodeID;
            curDbPM.OriginCriterionCode = gov.OriginCriterionCode;
        }
    }
}
