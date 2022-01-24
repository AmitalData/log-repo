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
            var gov = mehesTableRow.MyIncotemrsFileValidation ?? new IncotemrsFileValidation();
            return base.IsEqual(mehesTableRow, curDbPM) && curDbPM.IsFreightCharge == gov.IsFreightCharge
                && gov.TermsOfSaleTypeID == curDbPM.TermsOfSaleTypeID
                 && gov.IsPortIsraelCharge == curDbPM.IsPortIsraelCharge
                  && gov.IsInsurance == curDbPM.IsInsurance
                   && gov.CargoIdentifierTypeID == curDbPM.CargoIdentifierTypeID
                    && gov.CargoIdentifierTypeName == curDbPM.CargoIdentifierTypeName
                     && gov.LeadDocumentTypeID == curDbPM.LeadDocumentTypeID
                      && gov.LeadDocumentTypeName == curDbPM.LeadDocumentTypeName; 
        }
        protected override void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, IncotemrsFileValidationPM curDbPM)
        {
            base.SetOtherFields(mehesTableRow, curDbPM);
            var gov = mehesTableRow.MyIncotemrsFileValidation ?? new IncotemrsFileValidation();
            curDbPM.TermsOfSaleTypeID = gov.TermsOfSaleTypeID;
            curDbPM.IsFreightCharge = gov.IsFreightCharge;
            curDbPM.IsPortIsraelCharge = gov.IsPortIsraelCharge;
            curDbPM.IsInsurance = gov.IsInsurance;
            curDbPM.CargoIdentifierTypeID = gov.CargoIdentifierTypeID;
            curDbPM.CargoIdentifierTypeName = gov.CargoIdentifierTypeName;
            curDbPM.LeadDocumentTypeID = gov.LeadDocumentTypeID;
            curDbPM.LeadDocumentTypeName = gov.LeadDocumentTypeName;
        }
    }
}