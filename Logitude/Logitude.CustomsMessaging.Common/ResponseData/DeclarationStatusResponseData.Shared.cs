using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public  class DeclarationStatusResponseData: ResponseDataBase
    {
        public string ResponseStatusXML { get; set; }
        public string DeclarationID { get; set; }
        public string DeclarationStatusColor { get; set; }
        public string DeclarationVersion { get; set; }
        public string DeclarationStatusCode { get; set; }
        public string DeclarationStatusText { get; set; }
        public string LogisticStatusCode { get; set; }
        public string LogisticStatusText { get; set; }
        public string TaxationDateTime { get; set; }
        public string ReleaseDateTime { get; set; }
        public string DeclarationOfficeID { get; set; }
        public string DeclarationOfficeText { get; set; }
        public string FinancialStatusCode { get; set; }
        public string FinancialStatusText { get; set; }
        public string SubmitDateTime { get; set; }
        public string WarningMessage { get; set; } 
        public string HandeledWroker { get; set; }

        public List<AvailabiltyLogDeclarationCargoQuantities> AvailabiltyQuantitiesList { get; set; }
    }

    public class AvailabiltyLogDeclarationCargoQuantities
    {
        public string CargoIdentifierTypeCode { get; set; }
        public string CargoIdentifierTypeText { get; set; }
        public string CargoIdentifierKey1 { get; set; }
        public string CargoIdentifierKey2 { get; set; }
        public string CargoIdentifierKey3 { get; set; }
        public Boolean IsSecondRound { get; set; }
        public string CargoPackageTypeCode { get; set; }
        public string CargoPackageTypeText { get; set; }
        public string CargoPackageQuantity { get; set; }
        public string CargoPackageWeight { get; set; }
        public string CargoWeightMeasurementUnitCode { get; set; }
        public string CargoWeightMeasurementUnitText { get; set; }
        public string DeclarationPackageTypeCode { get; set; }
        public string DeclarationPackageTypeText { get; set; }
        public string DeclarationPackgeQuantity { get; set; }
        public string DeclarationPackageWeight { get; set; }
        public string DeclarationWeightMeasurementUnitCode { get; set; }
        public string DeclarationWeightMeasurementUnitText { get; set; }
        public string ComparisonResult { get; set; }
    }
}
