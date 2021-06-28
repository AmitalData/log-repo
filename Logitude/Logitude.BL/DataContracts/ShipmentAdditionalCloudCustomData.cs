using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class ShipmentAdditionalCloudCustomData
    {
        [Key]
        public string CustomsFileNo { get; set; }
        public string DeclarationNo { get; set; }
        public string MishgorDescOfGoods1 { get; set; }
        public string GoodsValue { get; set; }
        public string CifValue { get; set; }
        public string TotalTax { get; set; }
        public string MishgorPackageQuantity { get; set; }
        public string MishgorPackageWeight { get; set; }
        public bool IsImporterApprovalRequried { get; set; }
        public string ApprovedByUserName { get; set; }
        public string VersionApproved { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public string VersionId { get; set; }
        public bool IsPaymentRequired { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public RequestPayment RequestPaymentData { get; set; }
        public List<TaxesDetails> TaxesDetails { get; set; }
        public List<GoodsValueDetails> GoodsValueDetails { get; set; }
        public string DenyReason { get; set; }
        public PaymentData PaymentData { get; set; }
        public string ShipmentNumber { get; set; }
        public string DeclarationXmlData { get; set; }
        public string PaymentRequestXML { get; set; }
        public string TargetEnv { get; set; }
        public string DocumentsApprovedByUserName { get; set; }
        public string DeclarationStatus { get; set; }



        //public string VersionId { get; set; }

        /*
         public get Taxtypename() { return this.AdditionalData.Taxtypename }
    public set Taxtypename(newValue: string) { this.AdditionalData.Taxtypename = newValue; }

    public get TaxBasis() { return this.AdditionalData.TaxBasis }
    public set TaxBasis(newValue: string) { this.AdditionalData.TaxBasis = newValue; }

    public get TaxToPay() { return this.AdditionalData.TaxToPay }
    public set TaxToPay(newValue: string) { this.AdditionalData.TaxToPay = newValue; }

    public get TaxAmount() { return this.AdditionalData.TaxAmount }
    public set TaxAmount(newValue: string) { this.AdditionalData.TaxAmount = newValue; }

    public get SupAccount() { return this.AdditionalData.SupAccount }
    public set SupAccount(newValue: string) { this.AdditionalData.SupAccount = newValue; }

    public get IncotermId() { return this.AdditionalData.IncotermId }
    public set IncotermId(newValue: string) { this.AdditionalData.IncotermId = newValue; }

    public get Value() { return this.AdditionalData.Value }
    public set Value(newValue: string) { this.AdditionalData.Value = newValue; }

    public get CurrencyName() { return this.AdditionalData.CurrencyName }
    public set CurrencyName(newValue: string) { this.AdditionalData.CurrencyName = newValue; }

    public get CountryName() { return this.AdditionalData.CountryName }
    public set CountryName(newValue: string) { this.AdditionalData.CountryName = newValue; }

    public get SupplierName() { return this.AdditionalData.SupplierName }
    public set SupplierName(newValue: string) { this.AdditionalData.SupplierName = newValue; }

    public get SupplierFreight() { return this.AdditionalData.SupplierFreight }
    public set SupplierFreight(newValue: string) { this.AdditionalData.SupplierFreight = newValue; }
         */
    }

    public class TaxesDetails
    {
        public string Taxtypename { get; set; }
        public string TaxTypeCode { get; set; }
        public string TaxBasis { get; set; }
        public string TaxToPay { get; set; }
        public string TaxAmount { get; set; }
        public string TaxPostponed { get; set; }
    }

    public class GoodsValueDetails
    {
        public string SupAccount { get; set; }
        public string IncotermId { get; set; }
        public string Value { get; set; }
        public string CurrencyName { get; set; }
        public string CountryName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierFreight { get; set; }
    }

    public class PaymentData
    {
        public string sum { get; set; }
        public string currency { get; set; }
        public string op { get; set; }
        public string DCdisable { get; set; }
        public string DclickTK { get; set; }
        public string thtk { get; set; }
        public string TargetEnv { get; set; }
        public string u71 { get; set; }
    }
}