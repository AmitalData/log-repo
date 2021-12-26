using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;
namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class Update23928IncotemrsFileValidation : ClosedTableGenericService<IncotemrsFileValidationPM>
    {
        public Update23928IncotemrsFileValidation(ICustomContext CustomContext,
            ///string tableId,
            //SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<IncotemrsFileValidationPM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<IncotemrsFileValidationPM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
            : base(CustomContext, mehesTableRows,
            CreateNewUpdateServiceFunc,
            GetAllDbPMFunc,
            tenant,
            forceUpdateUnChanged)
        {

        }

        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, IncotemrsFileValidationPM curDbPM)
        {
            var gov = mehesTableRow.MyCertificateExemptionType ?? new CertificateExemptionType();
            return base.IsEqual(mehesTableRow, curDbPM);//&& curDbPM.IsImportDeclaration == gov.IsImportDeclaration && gov.IsExportDeclaration == curDbPM.IsExportDeclaration;
        }
        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, IncotemrsFileValidationPM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyIncotemrsFileValidation ?? new IncotemrsFileValidation();
          //  curDbPM.IsImportDeclaration = gov.IsImportDeclaration;
            //curDbPM.IsExportDeclaration = gov.IsExportDeclaration;
        }
    }
}

