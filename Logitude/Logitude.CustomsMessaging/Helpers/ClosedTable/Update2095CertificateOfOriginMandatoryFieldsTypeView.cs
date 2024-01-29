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
    class Update2095CertificateOfOriginMandatoryFieldsTypeView : ClosedTableGenericService<CertificateOfOriginMandatoryFieldsPM>
    {
        public Update2095CertificateOfOriginMandatoryFieldsTypeView(ICustomContext CustomContext,
          List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
          Func<ICustomContext, ICanUpdateClosedTable<CertificateOfOriginMandatoryFieldsPM>> CreateNewUpdateServiceFunc,
          Func<ICustomContext, ICanGetAllClosedTable<CertificateOfOriginMandatoryFieldsPM>> GetAllDbPMFunc,
          int tenant,
          bool forceUpdateUnChanged)
          : base(CustomContext, mehesTableRows,
          CreateNewUpdateServiceFunc,
          GetAllDbPMFunc,
          tenant,
          forceUpdateUnChanged)
        { }


        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CertificateOfOriginMandatoryFieldsPM curDbPM)
        {
            var gov = mehesTableRow.MyCertificateOfOriginMandatoryFields?? new CertificateOfOriginMandatoryFields();
            return base.IsEqual(mehesTableRow, curDbPM)
                && gov.MappedCertificatOriginId == curDbPM.MappedCertificatOriginId
                && gov.Location == curDbPM.Location
                && gov.LastUpdatedDate == curDbPM.LastUpdatedDate
                && gov.IsMandatory == curDbPM.IsMandatory;


        }

        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, CertificateOfOriginMandatoryFieldsPM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyCertificateOfOriginMandatoryFields ?? new CertificateOfOriginMandatoryFields();
            curDbPM.MappedCertificatOriginId = gov.MappedCertificatOriginId;
            curDbPM.Location = gov.Location;
            curDbPM.LastUpdatedDate = gov.LastUpdatedDate;
            curDbPM.IsMandatory = gov.IsMandatory;
        }
    }
}
