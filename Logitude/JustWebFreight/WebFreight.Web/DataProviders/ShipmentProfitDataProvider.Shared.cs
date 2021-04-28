using System;
using System.Collections.Generic;

namespace WebFreight.Web.DataProviders
{
    public class ShipmentProfitDataProvider:BaseDataProvider
    {
        public DateTime IssueDate { get; set; }

        public string LocalCurrencyCode { get; set; }
        public string LocalCurrencyName { get; set; }
        public string PayablesInLocalCurrency { get; set; }
        public string ReceivablesInLocalCurrency { get; set; }
        public string ProfitInLocalCurrency { get; set; }
        public string EstimateProfitInLocalCurrency { get; set; }
        public string DifferenceInLocalCurrency { get; set; }
        public string MasterShipmentNumber { get; set; }
        public string ProfitCurrencyCode { get; set; }
        public string ProfitCurrencyName { get; set; }
        public string PayablesInProfitCurrency { get; set; }
        public string ReceivablesInProfitCurrency { get; set; }
        public string ProfitInProfitCurrency { get; set; }
        public string EstimateProfitInProfitCurrency { get; set; }
        public string DifferenceInProfitCurrency { get; set; }

        public List<ProfitDetailsClass> ChargeTypesList { get; set; }

        //****
        public string Notes { get; set; }
        public string ShipmentNumber { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string Direction { get; set; }
        public string TransportMode { get; set; }
        public string Incoterm { get; set; }
        public string CurrentUser { get; set; }
        public string Salesman { get; set; }
        public string ATD { get; set; }
        public string ATA { get; set; }
        public string Orign { get; set; }
        public string Invoices { get; set; }
        public string Containers { get; set; }
        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }
        public string CarrierNumber { get; set; }
        public string CarrierLable { get; set; }
        public string CarrierNumberLabel { get; set; }
        public string POLCode { get; set; }
        public string POLName { get; set; }
        public string POLLable { get; set; }
        public string PODCode { get; set; }
        public string PODName { get; set; }
        public string PODLable { get; set; }
        public string DestinationCode { get; set; }
        public string DestinationName { get; set; }

        public string AgentName { get; set; }
        public string AgentAddress { get; set; }
        public string AgentRef1 { get; set; }
        public string AgentRef2 { get; set; }

        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string ShipperAddress { get; set; }
        public string ConsigneeAddress { get; set; }
        public string VatType { get; set; }

        public string FromLocation { get; set; }
        public string ToLocation { get; set; }

        public string ShipmentField1 { get; set; }
        public string ShipmentField2 { get; set; }
        public string ShipmentField3 { get; set; }
        public string ShipmentField4 { get; set; }
        public string ShipmentField5 { get; set; }
        public string ShipmentField6 { get; set; }
        public string ShipmentField7 { get; set; }
        public string ShipmentField8 { get; set; }
        public string ShipmentField9 { get; set; }
        public string ShipmentField10 { get; set; }
        public string ShipmentField11 { get; set; }
        public string ShipmentField12 { get; set; }
        public string ShipmentField13 { get; set; }
        public string ShipmentField14 { get; set; }
        public string ShipmentField15 { get; set; }
        public string ShipmentField16 { get; set; }
        public string ShipmentField17 { get; set; }
        public string ShipmentField18 { get; set; }
        public string ShipmentField19 { get; set; }
        public string ShipmentField20 { get; set; }
        public string ShipmentField21 { get; set; }
        public string ShipmentField22 { get; set; }
        public string ShipmentField23 { get; set; }
        public string ShipmentField24 { get; set; }
        public string ShipmentField25 { get; set; }
        public string ShipmentField26 { get; set; }
        public string ShipmentField27 { get; set; }
        public string ShipmentField28 { get; set; }
        public string ShipmentField29 { get; set; }
        public string ShipmentField30 { get; set; }
        public string ShipmentField31 { get; set; }
        public string ShipmentField32 { get; set; }
        public string ShipmentField33 { get; set; }
        public string ShipmentField34 { get; set; }
        public string ShipmentField35 { get; set; }
        public string ShipmentField36 { get; set; }
        public string ShipmentField37 { get; set; }
        public string ShipmentField38 { get; set; }
        public string ShipmentField39 { get; set; }
        public string ShipmentField40 { get; set; }

        public string MoveTypeCode { get; set; }
        public string MoveTypeName { get; set; }

        public double? ShipmentVolume { get; set; }
        public string VolumeUnitCode { get; set; }

        public string MasterNumber { get; set; }

        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }

        public DateTime? ATD_DateTime { get; set; }
        public DateTime? ATA_DateTime { get; set; }
        public string BranchName { get; set; }

        public bool IsAccrualsApproved { get; set; }
        public DateTime? AccrualsApprovalDate { get; set; }
    }

    public class ProfitDetailsClass
    {
        public string ChargeTypeId { get; set; }
        public string ChargeTypeCode { get; set; }
        public string ChargeTypeName { get; set; }

        public string VatTypeCode { get; set; }
        public string VatTypeName { get; set; }
        public string VatTypePercentage { get; set; }

        public string PayablesInLocalCurrency { get; set; }
        public string ReceivablesInLocalCurrency { get; set; }
        public string ProfitInLocalCurrency { get; set; }

        public string PayablesInProfitCurrency { get; set; }
        public string ReceivablesInProfitCurrency { get; set; }
        public string ProfitInProfitCurrency { get; set; }

        public double? OpenPayablesInLocal { get; set; }
        public double? ACCTPayablesInLocal { get; set; }
        public double? OpenPayablesInProfit { get; set; }
        public double? ACCTPayablesInProfit { get; set; }

        public string Vendor { get; set; }

        public bool? IsExpenseCharge { get; set; }
    }
}