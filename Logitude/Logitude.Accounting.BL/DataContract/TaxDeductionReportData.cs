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
     
      
        public string SettingDeductionFileNumber { get; set; }
        public string TenantVatNumber { get; set; }
        public string Phone {get; set;}
        public string TaxYear { get; set; }
        public List<ByMonthList> ByMonthList { get; set; }
        public List<ByVendorList> ByVendorList { get; set; }
        public List<TotalForCompany> TotalForCompany { get; set; }
        public List<DBVendorsList> DBVendorsList { get; set; }
        public double? TotalAmountInLocalCurrency { get; set; }
        public decimal? TotalDeductionInLocalCurrency { get; set; }
        public double? TotalAmountInLocalCurrency08 { get; set; }
        public decimal? TotalTaxDeductionInLocalCurrency08 { get; set; }
        public decimal? TotalEndBalance  { get; set; }
        public int? VendorsCount { get; set; }
        public List<TaxDeductionReportLine> deductionLines { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string TenantAddress1 { get;  set; }
        public string TenantAddress2 { get;  set; }
    }

    public class TotalForCompany
    {
        public string CompanyName { get; set; }
        public string DeductionFileNumber { get; set; }
        public double? TotalPayments { get; set; }
        public decimal? TotalDeductions { get; set; }
    }

    public class DBVendorsList
    {
        public DateTime? RigesterDate { get; set; }
        public string VendorId { get; set; }
        public string GlAccountId { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public decimal? TaxDeductionLocalAmount { get; set; }
        public string DeductionFileTypeCode { get; set; }
        public decimal? EndYearBalance { get; set; }
    }

    public class ByMonthList
    {
        public int Month { get; set; }
        public string ReportMonth { get; set; }
        public double? TotalAmountInLocalCurrency { get; set; }
        public decimal? TotalTaxDeductionLocalAmount { get; set; }
        public int TotalVendors { get; set; }
        public double? TotalPaymentsWithoutDivided { get; set; }
        public decimal? TotalDeductionsWithoutDivided { get; set; }
        public double? TotalDivided { get; set; }
        public decimal? TotalDeductionsFromDivided { get; set; }



    }

    public class ByVendorList
    {
        public string VendorName { get; set; }
        public int Month { get; set; }
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
        public decimal? EndYearBalance { get; set; }
        public string VendorLocalName { get; set; }
        public decimal? TotalAmount { get;  set; }
        public string CardAddress1 { get;  set; }
    }
    

    public class TaxDeductionReportLine
    {
        public string VendorId { get; set; }
        public int MonthOfRegisterDate { get; set; }
        public double? AmountInLocalCurrency { get; set; }
        public decimal? TaxDeductionLocalAmount { get; set; }
        public int? TaxDeductionPercentage { get; set; }
        public string DeductionType { get; set; }
    }
}

