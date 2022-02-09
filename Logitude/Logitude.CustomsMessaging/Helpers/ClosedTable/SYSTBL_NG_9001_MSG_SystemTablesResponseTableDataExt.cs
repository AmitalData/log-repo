using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt : SYSTBL_NG_9001_MSG_SystemTablesResponseTableData
    {
        public GovernmentProcedureType MyGovernmentProcedureType { get; set; }
        public InternationalSiteP MyInternationalSite { get; set; }
        public ModificationAndDiscountType MyModificationAndDiscountType { get; set; }
        public NDMessageActionCode MyNDMessageActionCode { get; set; }
        public ContainerType MyContainerType { get; set; }
        public CertificateExemptionType MyCertificateExemptionType { get; set; }
        public IncotemrsFileValidation MyIncotemrsFileValidation { get; set; }
        public CargoIdentifireType MyCargoIdentifireType { get; set; }
        public TradeAgreement MyTradeAgreement { get; set; }
        public ConfirmationType MyConfirmationType { get; set; }


        internal static SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt CreateNew(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData item)
        {
            return new SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt()
            {
                extraNumericData = item.extraNumericData,
                extraNumericDataSpecified = item.extraNumericDataSpecified,
                extraStringData = item.extraStringData,
                id = item.id,
                malamID = item.malamID,
                name = item.name,
                state = item.state,
                stateSpecified = item.stateSpecified,
                updateDate = item.updateDate,
                updateDateSpecified = item.updateDateSpecified

            };
        }
    }
    public class GovernmentProcedureType
    {
        public bool IsImport { get; set; }
        public bool IsExport { get; set; }

    }
    public class InternationalSiteP
    {
        public string CountryTypeCode { get; set; }
    }
    public class ModificationAndDiscountType
    {
        public Boolean IsRelevantGoodsItem { get; set; }
        public Boolean IsRelevantInvoice { get; set; }
        public Boolean IsRelevantInvoiceExport { get; set; }
        public Boolean IsRelevantGoodsItemExport { get; set; }
    }
    public class NDMessageActionCode
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class IncotemrsFileValidation
    {
        public String TermsOfSaleTypeID { get; set; }
        public Boolean IsFreightCharge { get; set; }
        public Boolean IsPortIsraelCharge { get; set; }
        public Boolean IsInsurance { get; set; }
        public String CargoIdentifierTypeID { get; set; }
        public String CargoIdentifierTypeName { get; set; }
        public String LeadDocumentTypeID { get; set; }
        public String LeadDocumentTypeName { get; set; }

    }
    public class ContainerType
    {
        public Boolean IsIsoTankContainer { get; set; }
        public Boolean IsNeedSeal { get; set; }
        public Boolean IsAerial { get; set; }

    }
    public class CertificateExemptionType
    {
        public Boolean IsImportDeclaration { get; set; }
        public Boolean IsExportDeclaration { get; set; }


    }
    public class CargoIdentifireType
    {
        public Boolean IsForDeclarationExport { get; set; }
        public Boolean IsForDeclarationImport { get; set; }
        public Boolean IsForManifest { get; set; }
        public Boolean IsKey2Mandatory { get; set; }
        public Boolean IsKey3Mandatory { get; set; }
        public string CargoIdentifierKey1Name { get; set; }
        public string CargoIdentifierKey2Name { get; set; }
        public string CargoIdentifierKey3Name { get; set; }

    }
    public class TradeAgreement
    {
        public int CustomsBookTypeID { get; set; }
    }


    public class ConfirmationType
    {
        public int MalamID { get; set; }
        public int State { get; set; }
        public int Exempt_CertificateDocumentCategoryTypeID { get; set; }
        public bool IsImport { get; set; }
        public bool IsExemptOtherAuthority { get; set; }
        public int ConfirmationComputerizationLevelTypeID { get; set; }
        public bool IsCEO { get; set; }
        public bool IsNeedDeclaration { get; set; }
        public int CertificateDocumentCategoryTypeID { get; set; }
        public int AuthorityID { get; set; }
        public bool IsQuotaCheckNeeded { get; set; }
        public int ExternalIDNumPerAuthority { get; set; }
        public bool IsForCustomsItem { get; set; }
        public bool IsPharmacy { get; set; }
        public bool IsVeterinarian { get; set; }
        public bool IsVehicleStandardization { get; set; }
        public bool IsQuantityMandatory { get; set; }
        public bool IsForCE { get; set; }
    }
}
