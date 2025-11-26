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
    class Update1958CertificateOfOriginTypeCodeEnumTypeView : ClosedTableGenericService<CertificateOfOriginTypeCodeEnumPM>
    {
        public Update1958CertificateOfOriginTypeCodeEnumTypeView(ICustomContext CustomContext,
          List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
          Func<ICustomContext, ICanUpdateClosedTable<CertificateOfOriginTypeCodeEnumPM>> CreateNewUpdateServiceFunc,
          Func<ICustomContext, ICanGetAllClosedTable<CertificateOfOriginTypeCodeEnumPM>> GetAllDbPMFunc,
          int tenant,
          bool forceUpdateUnChanged)
          : base(CustomContext, mehesTableRows,
          CreateNewUpdateServiceFunc,
          GetAllDbPMFunc,
          tenant,
          forceUpdateUnChanged)
        { }


        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CertificateOfOriginTypeCodeEnumPM curDbPM)
        {
            CertificateOfOriginTypeCodeEnum gov = mehesTableRow.MyCertificateOfOriginTypeCodeEnum ?? new CertificateOfOriginTypeCodeEnum();
            return base.IsEqual(mehesTableRow, curDbPM)
                && gov.IsCustomApprovalRequired == curDbPM.IsCustomApprovalRequired && gov.IsCriterionMandatory == curDbPM.IsCriterionMandatory
                && gov.IsCustomsItemMandatory == curDbPM.IsCustomsItemMandatory && gov.IsZipcodeMandatory == curDbPM.IsZipcodeMandatory;
        }

        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CertificateOfOriginTypeCodeEnumPM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyCertificateOfOriginTypeCodeEnum ?? new CertificateOfOriginTypeCodeEnum();
            curDbPM.IsCustomApprovalRequired = gov.IsCustomApprovalRequired;
            curDbPM.IsCriterionMandatory = gov.IsCriterionMandatory;
            curDbPM.IsCustomsItemMandatory = gov.IsCustomsItemMandatory;
            curDbPM.IsZipcodeMandatory = gov.IsZipcodeMandatory;
        }
    }
}
