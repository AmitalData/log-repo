using Logitude.Customs.Data.EntityPOCOs;
//using Logitude.Server.Tools.SharedBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.StimulReport
{
    public partial class DeclarationSReport ///: BaseDataProvider
    {
    }
    public partial class acc_supplier
    {
        double? _acc_supplierinsurance;

        public bool isAccumalated { get; set; }

        public double? Acc_supplierinsurance
        {
            get { return _acc_supplierinsurance; }
            set { _acc_supplierinsurance = value; }
        }

        double? _acc_supplierinsurancepercent;

        public double? Acc_supplierinsurancepercent
        {
            get { return _acc_supplierinsurancepercent; }
            set { _acc_supplierinsurancepercent = value; }
        }

        string _acc_supplierinsurancecurr;

        public string Acc_supplierinsurancecurr
        {
            get { return _acc_supplierinsurancecurr; }
            set { _acc_supplierinsurancecurr = value; }
        }

        string _acc_suppliertariff;

        public string Acc_suppliertariff
        {
            get { return _acc_suppliertariff; }
            set { _acc_suppliertariff = value; }
        }

        bool _acc_supplierpreference;

        public bool Acc_supplierpreference
        {
            get { return _acc_supplierpreference; }
            set { _acc_supplierpreference = value; }
        }
        // moran 26.8.15 - task 16086 -->
        double? _acc_supplierfreight;

        public double? Acc_supplierfreight
        {
            get { return _acc_supplierfreight; }
            set { _acc_supplierfreight = value; }
        }

        string _acc_supplierfreightcurr;

        public string Acc_supplierfreightcurr
        {
            get { return _acc_supplierfreightcurr; }
            set { _acc_supplierfreightcurr = value; }
        }

        string _acc_suppliercurrencyrate;

        public string Acc_suppliercurrencyrate
        {
            get { return _acc_suppliercurrencyrate; }
            set { _acc_suppliercurrencyrate = value; }
        }
        // moran 26.8.15 - task 16086 <--
        double? _acc_supplierActualPayedAmount;

        public double? acc_supplierActualPayedAmount
        {
            get { return _acc_supplierActualPayedAmount; }
            set { _acc_supplierActualPayedAmount = value; }
        }

        string _acc_supplierActualPayedCurrencyTypeCode;

        public string acc_supplierActualPayedCurrencyTypeCode
        {
            get { return _acc_supplierActualPayedCurrencyTypeCode; }
            set { _acc_supplierActualPayedCurrencyTypeCode = value; }
        }

        string _acc_supplierActualPayedCurrencyTypeName;

        public string acc_supplierActualPayedCurrencyTypeName
        {
            get { return _acc_supplierActualPayedCurrencyTypeName; }
            set { _acc_supplierActualPayedCurrencyTypeName = value; }
        }

        List<SupplierInvoiceModificationM> acc_suppliermodification;

        public List<SupplierInvoiceModificationM> Acc_suppliermodification
        {
            get { return acc_suppliermodification; }
            set { acc_suppliermodification = value; }
        }

        decimal? _acc_supplierVendorComissionPercentage; // moran 24.12.17 - AMI-62725

        public decimal? Acc_supplierVendorComissionPercentage // moran 24.12.17 - AMI-62725
        {
            get { return _acc_supplierVendorComissionPercentage; }
            set { _acc_supplierVendorComissionPercentage = value; }
        }
    }

    public partial class acc_suppliersup_items // moran 26.8.15 - task 16086
    {

        // moran 4.5.16 - task 20319 -->
        public string SupplierInvoiceItemProcesTypes { get; set; }
        public string CertificateNumber { get; set; }
        public string StatisticQuantity{ get; set; }
        public string StatisticQuantityType { get; set; }
        public string StatisticQuantityTypeName { get; set; }
        public string AddQuantity { get; set; }
        public string AddQuantityType { get; set; }
        public string AddQuantityTypeName { get; set; }
        public string PreferenceDocumentNumber { get; set; }
        public string TaxExemptCode { get; set; }
        public string TaxExemptCodeName { get; set; }
        public string CustomsBookTypeCode { get; set; }
        public string CustomsBookTypeCodeName { get; set; }
        public string DangerousClassificationCode { get; set; }
        public string DangerousClassificationCodeName { get; set; }
        public string NonCustomsItemPrice { get; set; }
        public string NonCustomsItemPriceCurCode { get; set; }
        public string NonCustomsItemPriceCurCodeName { get; set; }
        public string WholeSaleItemPrice { get; set; }
        public string WholeSaleItemPriceCurrencyCode { get; set; }
        public string WholeSaleItemPriceCurrencyCodeName { get; set; }
        public string ManufactureIdentifier { get; set; }
        public string SalesTaxExemptionTypeCode{ get; set; }
        public string SalesTaxExemptionTypeName { get; set; }
        public string OptionalTamaPercentage{ get; set; }
        // moran 4.5.16 - task 20319 <--

        public bool isParent { get; set; }
        public int? parentLineNumber { get; set; }


        List<SupplierInvioceItemsCertificateM> acc_supplieritemscertificate;

        public List<SupplierInvioceItemsCertificateM> Acc_supplieritemscertificate
        {
            get { return acc_supplieritemscertificate; }
            set { acc_supplieritemscertificate = value; }
        }

        // moran 3.5.16 - task 20319 -->
        List<SupplierInvioceItemConDeclarsM> acc_supplieritemsConDeclars;

        public List<SupplierInvioceItemConDeclarsM> Acc_supplieritemsConDeclars
        {
            get { return acc_supplieritemsConDeclars; }
            set { acc_supplieritemsConDeclars = value; }
        }


        List<SupplierInvioceItemModsM> acc_supplieritemsMods;

        public List<SupplierInvioceItemModsM> Acc_supplieritemsMods
        {
            get { return acc_supplieritemsMods; }
            set { acc_supplieritemsMods = value; }
        }


        List<SupplierInvioceItemLeviesM> acc_supplieritemsLevies;

        public List<SupplierInvioceItemLeviesM> Acc_supplieritemsLevies
        {
            get { return acc_supplieritemsLevies; }
            set { acc_supplieritemsLevies = value; }
        }


        List<SupplierInvioceItemProdIdentsM> acc_supplieritemsProdIdents;

        public List<SupplierInvioceItemProdIdentsM> Acc_supplieritemsProdIdents
        {
            get { return acc_supplieritemsProdIdents; }
            set { acc_supplieritemsProdIdents = value; }
        }


        List<SupplierInvioceItemSerialNumsM> acc_supplieritemsSerialNums;

        public List<SupplierInvioceItemSerialNumsM> Acc_supplieritemsSerialNums
        {
            get { return acc_supplieritemsSerialNums; }
            set { acc_supplieritemsSerialNums = value; }
        }


        List<SupplierInvioceItemDescriptsM> acc_supplieritemsDescripts;

        public List<SupplierInvioceItemDescriptsM> Acc_supplieritemsDescripts
        {
            get { return acc_supplieritemsDescripts; }
            set { acc_supplieritemsDescripts = value; }
        }


        List<SupplierInvioceItemVehiclesM> acc_supplieritemsVehicles;

        public List<SupplierInvioceItemVehiclesM> Acc_supplieritemsVehicles
        {
            get { return acc_supplieritemsVehicles; }
            set { acc_supplieritemsVehicles = value; }
        }
        // moran 3.5.16 - task 20319 <--

        // moran 5.5.16 - task 20319 -->
        List<SupplierInvioceItemAddsM> acc_supplieritemAdds;

        public List<SupplierInvioceItemAddsM> Acc_supplieritemAdds
        {
            get { return acc_supplieritemAdds; }
            set { acc_supplieritemAdds = value; }
        }
        // moran 5.5.16 - task 20319 <--

        // moran 8.5.16 - task 20319 -->
        List<SupplierInvioceItemVehicleAndModsM> acc_supplieritemsVehicleAndMods;

        public List<SupplierInvioceItemVehicleAndModsM> Acc_supplieritemsVehicleAndMods
        {
            get { return acc_supplieritemsVehicleAndMods; }
            set { acc_supplieritemsVehicleAndMods = value; }
        }
        // moran 8.5.16 - task 20319 <--
    }

}