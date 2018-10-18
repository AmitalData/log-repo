using System;
using System.Collections.Generic;
using System.Linq;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.TaxesApprovalModel
{
    public class TaxesApproval : BaseDataProvider
    {
        public A100 A100_Object { get; set; }
        public C100 C100_Object { get; set; }
        public Z900 Z900_Object { get; set; }

        public class A000
        {
            public string RecordCode { get; set; }
            public string FutureUsage { get; set; }
            public string TotalLinesInBKMVDATA { get; set; }
            public string TenantVatNumber { get; set; }
            public string MainId { get; set; }
            public string FixedValue { get; set; }
            public string SystemRegistrationNumber { get; set; }
            public string SystemName { get; set; }
            public string SystemVerion { get; set; }
            public string LogitudeVATNumber { get; set; }
            public string SystemManifacturer { get; set; }
            public string SystemType { get; set; }
            public string FilesSavingDirectory { get; set; }
            public string SystemAcctType { get; set; }
            public string NeededBalanceLevel { get; set; }
            //VAT
            public string Field1016 { get; set; }
            //future usage
            public string TenantName { get; set; }
            public string TenantAddress1 { get; set; }
            public string TenantHouseNumber { get; set; }
            public string TenantCity { get; set; }
            public string TenantZipCode { get; set; }
            public string TaxYear { get; set; }
            public string Startingdate { get; set; }
            public string EndDate { get; set; }
            public string TodayDate { get; set; }
            public string TimeNow { get; set; }
            public string LanguageCode { get; set; }
            public string Encoding { get; set; }
            public string Field1030 { get; set; }
            public string AccountingCurrencyCode { get; set; }
            public string BranchInfo { get; set; }
            //future usage
        }

        public class A100
        {
            public string RecordCode { get; set; }
            public string RecordLineNumber { get; set; }
            public string TenantVatNumber { get; set; }
            public string PrimaryId { get; set; }
            public string SystemConst { get; set; }
            public string FutureUsage { get; set; }
        }

        public class C100
        {
            public string RecordCode { get; set; } //1200
            public string RecordLineNumber { get; set; }
            public string TenantVatNumber { get; set; }
            public string DocumentType { get; set; }
            public string DocumentNumber { get; set; }
            public string DocumentCreateDate { get; set; } //	
            public string DocumentCreateHour { get; set; }	//	
            public string BillToName { get; set; }
            public string BillToAddress_Street { get; set; }
            public string BillToAddress_HouseNumber { get; set; }
            public string BillToAddress_City { get; set; }
            public string BillToAddress_ZipCode { get; set; }
            public string BillToAddress_CountryName { get; set; }
            public string BillToAddress_CountryCode { get; set; }
            public string BillToAddress_Telephone { get; set; }
            public string BillToVAT { get; set; } //
            public string ValueDate { get; set; }	//
            public string TotalAmount { get; set; }	//
            public string CurrencyCode { get; set; }
            public string AmountBeforeDiscount { get; set; } //
            public string Discount { get; set; } //
            public string SubTotal { get; set; } //	
            public string VAT { get; set; }
            public string GrandTotal { get; set; } //
            public string Field1224 { get; set; }
            public string BillToId { get; set; }
            public string Field1226 { get; set; }
            public string Void_CancelledDocument { get; set; }
            public string DocumentDate { get; set; } //
            public string BranchId { get; set; }
            public string UserName { get; set; }
            public string LinkingField { get; set; }
            public string FutureUsage { get; set; }

            public List<D110> D110List { get; set; }
            public D120 D120 { get; set; }
        }

        public class D110
        {
            public string RecordCode { get; set; }
            public string RecordLineNumber { get; set; }
            public string TenantVatNumber { get; set; }
            public string DocumentType { get; set; }
            public string DocumentNumber { get; set; }
            public string DocumentLineNumber { get; set; }
            public string BaseDocumentType { get; set; }
            public string BaseDocumentNumber { get; set; }
            public string ServiceType { get; set; }
            public string Field1259 { get; set; }
            public string DescriptionOfSservice { get; set; }
            public string ManifacturerName { get; set; }
            public string ProductSerialNumber { get; set; }
            public string Field1263 { get; set; }
            public string Quantiy { get; set; }
            public string UnitPrice { get; set; }
            public string LineDiscount { get; set; }
            public string LineAmount { get; set; }
            public string LineVATPercentage { get; set; }
            public string BranchId { get; set; }
            public string EntityDate { get; set; }
            public string Field1273 { get; set; }
            public string Field1274 { get; set; }
            public string FutureUsage { get; set; }
        }

        public class D120
        {
            public string RecordCode { get; set; }
            public string RecordLineNumber { get; set; }
            public string TenantVatNumber { get; set; }
            public string DocumentType { get; set; }
            public string DocumentNumber { get; set; }
            public string EntityLineNumber { get; set; }
            public string PaymentMethod { get; set; }
            public string BankNumber { get; set; }
            public string BranchNumber { get; set; }
            public string AccountBunber { get; set; }
            public string ChequeNumber { get; set; }
            public string PaymentDate { get; set; }
            public string Total { get; set; }
            public string CreditCardCompany { get; set; }
            public string Field1314 { get; set; }
            public string CreditType { get; set; }
            public string Field1320 { get; set; }
            public string DocumentDate { get; set; }
            public string Field1323 { get; set; }
            public string Field1324 { get; set; }
        }

        public class Z900
        {
            public string RecordCode { get; set; }
            public string RecordLineNumber { get; set; }
            public string TenantVatNumber { get; set; }
            public string PrimaryId { get; set; }
            public string SystemConst { get; set; }
            public string TotalRecords { get; set; }
            public string FutureUsage { get; set; }
        }

        public class B110
        {
            public string RecordCode { get; set; }
            public string RecordLineNumber { get; set; }
            public string TenantVatNumber { get; set; }
            public string CardId { get; set; }
            public string CustomerName { get; set; }
            public string EntityTypeCode { get; set; }
            public string EntityTypeName { get; set; }
            public string Address1 { get; set; }
            public string HouseNumber { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }
            public string Field1413 { get; set; }
            public string BalanceOpeningDate { get; set; } //
            public string TotalDebit { get; set; }//
            public string TotalCredit { get; set; }//
            public string Field1417 { get; set; }
            public string Field1418 { get; set; }
            public string CustomerVAT { get; set; }
            public string Field1420 { get; set; }
            public string BranchId { get; set; }
            public string BalanceInForeignCurrency { get; set; }//
            public string ForeignCurrencyCode { get; set; }//
            public string FutureUsage { get; set; }
        }

        public class TaxReport
        {
            public string TodayDate { get; set; }
            public TimeSpan TodayTime { get; set; }
            public string TenantVATNumber { get; set; }
            public string TenantName { get; set; }
            public string FileSavingDirectory { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string B110Count { get; set; }
            public string C100Count { get; set; }
            public string D110Count { get; set; }
            public string D120Count { get; set; }

            public string Count305 { get; set; }
            public double? Total305 { get; set; }
            public string Count310 { get; set; }
            public double? Total310 { get; set; }
            public string Count330 { get; set; }
            public double? Total330 { get; set; }
            public string Count400 { get; set; }
            public double? Total400 { get; set; }
            public string Count700 { get; set; }
            public double? Total700 { get; set; }
        }
    }
}