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
    class Update1957CertificateOfOriginStatusCodeEnumTypeView : ClosedTableGenericService<CertificateOfOriginStatusCodeEnumPM>
    {
        public Update1957CertificateOfOriginStatusCodeEnumTypeView(ICustomContext CustomContext,
          List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
          Func<ICustomContext, ICanUpdateClosedTable<CertificateOfOriginStatusCodeEnumPM>> CreateNewUpdateServiceFunc,
          Func<ICustomContext, ICanGetAllClosedTable<CertificateOfOriginStatusCodeEnumPM>> GetAllDbPMFunc,
          int tenant,
          bool forceUpdateUnChanged)
          : base(CustomContext, mehesTableRows,
          CreateNewUpdateServiceFunc,
          GetAllDbPMFunc,
          tenant,
          forceUpdateUnChanged)
        { }


        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CertificateOfOriginStatusCodeEnumPM curDbPM)
        {
            var gov = mehesTableRow.MyCertificateOfOriginStatusCodeEnum ?? new CertificateOfOriginStatusCodeEnum();
            return base.IsEqual(mehesTableRow, curDbPM)
                && gov.RecordEditable == curDbPM.RecordEditable;


        }

        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CertificateOfOriginStatusCodeEnumPM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyCertificateOfOriginStatusCodeEnum ?? new CertificateOfOriginStatusCodeEnum();
            curDbPM.RecordEditable = gov.RecordEditable;
        }
    }
}
