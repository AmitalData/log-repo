using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
    public class TaxDeductionReportData
    {
        [Key]
        public string Id { get; set; }
        public string DeductionFileNumber { get; set; }
        public string DeductionFileTypeCode { get; set; }
        public string VATNumber { get; set; }
        public string DisplayNumber { get; set; }
        public string GLAccountLocalName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorCity { get; set; }
        public double? SumOfAmountInLocalCurrency { get; set; }
        public decimal? SumOfTaxDeductionLocalAmount { get; set; }
        public int? TaxDeductionPercentage { get; set; }
        public string AssessingOfficerCode { get; set; }
        public string AssessingOfficerName { get; set; }
        public string Occupation { get; set; }
        public string DeductionType { get; set; }
        public string VendorId { get; set; }
        public bool IsAutonomy { get; set; }
        public bool IsInternationlPartner { get; set; }
        public string EnglishName { get; set; }
        public string SettingDeductionFileNumber { get; set; }
        public string TenantVatNumber { get; set; }
        public string Phone {get; set;}

    }


    public class ResultList
    {

    }
}

