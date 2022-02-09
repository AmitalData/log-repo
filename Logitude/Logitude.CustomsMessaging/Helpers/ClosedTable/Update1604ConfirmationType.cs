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
    public class Update1604ConfirmationType : ClosedTableGenericService<ConfirmationTypePM>
    {
        public Update1604ConfirmationType(ICustomContext CustomContext,
            ///string tableId,
            //SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<ConfirmationTypePM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<ConfirmationTypePM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
            : base(CustomContext, mehesTableRows,
            CreateNewUpdateServiceFunc,
            GetAllDbPMFunc,
            tenant,
            forceUpdateUnChanged)
        {

        }

        protected override bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, ConfirmationTypePM curDbPM)
        {
            var gov = mehesTableRow.MyConfirmationType ?? new ConfirmationType();
            return base.IsEqual(mehesTableRow, curDbPM)
                && gov.MalamID == curDbPM.MalamID
                && gov.State == curDbPM.State
                && gov.Exempt_CertificateDocumentCategoryTypeID == curDbPM.Exempt_CertificateDocument
                && gov.IsImport == curDbPM.IsImport
                && gov.IsExemptOtherAuthority == curDbPM.IsExemptOtherAuthority
                && gov.ConfirmationComputerizationLevelTypeID == curDbPM.ConfirmationComputerization
                && gov.IsCEO == curDbPM.IsCEO
                && gov.IsNeedDeclaration == curDbPM.IsNeedDeclaration
                && gov.CertificateDocumentCategoryTypeID == curDbPM.CertificateDocumentCategory
                && gov.AuthorityID == curDbPM.AuthorityID
                && gov.IsQuotaCheckNeeded == curDbPM.IsQuotaCheckNeeded
                && gov.ExternalIDNumPerAuthority == curDbPM.ExternalIDNumPerAuthority
                && gov.IsForCustomsItem == curDbPM.IsForCustomsItem
                && gov.IsPharmacy == curDbPM.IsPharmacy
                && gov.IsVeterinarian == curDbPM.IsVeterinarian
                && gov.IsVehicleStandardization == curDbPM.IsVehicleStandardization
                && gov.IsQuantityMandatory == curDbPM.IsQuantityMandatory
                && gov.IsForCE == curDbPM.IsForCE;
        }
        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, ConfirmationTypePM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyConfirmationType ?? new ConfirmationType();
            curDbPM.MalamID = gov.MalamID;
            curDbPM.State = gov.State;
            curDbPM.Exempt_CertificateDocument = gov.Exempt_CertificateDocumentCategoryTypeID;
            curDbPM.IsImport = gov.IsImport;
            curDbPM.IsExemptOtherAuthority = gov.IsExemptOtherAuthority;
            curDbPM.ConfirmationComputerization = gov.ConfirmationComputerizationLevelTypeID;
            curDbPM.IsCEO = gov.IsCEO;
            curDbPM.IsNeedDeclaration = gov.IsNeedDeclaration;
            curDbPM.CertificateDocumentCategory = gov.CertificateDocumentCategoryTypeID;
            curDbPM.AuthorityID = gov.AuthorityID;
            curDbPM.IsQuotaCheckNeeded = gov.IsQuotaCheckNeeded;
            curDbPM.ExternalIDNumPerAuthority = gov.ExternalIDNumPerAuthority;
            curDbPM.IsForCustomsItem = gov.IsForCustomsItem;
            curDbPM.IsPharmacy = gov.IsPharmacy;
            curDbPM.IsVeterinarian = gov.IsVeterinarian;
            curDbPM.IsVehicleStandardization = gov.IsVehicleStandardization;
            curDbPM.IsQuantityMandatory = gov.IsQuantityMandatory;
            curDbPM.IsForCE = gov.IsForCE;
        }
    }
}

