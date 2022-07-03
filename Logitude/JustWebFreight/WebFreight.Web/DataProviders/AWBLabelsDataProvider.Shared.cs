namespace WebFreight.Web.DataProviders
{
    public class AWBLabelsDataProvider:BaseDataProvider
    {
        public string MainCarriageCarrierCode { get; set; }
        public string MainCarriageFromPortCode { get; set; }
        public string MAWBFull { get; set; }
        public string HAWBFull { get; set; }

        public string MainCarriageCarrierName { get; set; }
        public string MainCarriageToPortCode { get; set; }
        public string TotalQuantity { get; set; }
        public string Transshipment1ToPortCode { get; set; }
        public string Transshipment2ToPortCode { get; set; }
        public string ChargeableWeight { get; set; }
        public string ActualChargeableWeight { get; set; }
        public string BarCode { get; set; }
        public string HouseBarCode { get; set; }
        public string BookingNumber { get; set; }

        //Custom fields
        public string AdditionalInformation { get; set; }
        public string NumberOfLabels { get; set; }
        public string Contents { get; set; }

        public string PieceNumberString { get; set; }
        public string PieceNumber { get; set; }

        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }

        public string ShipperName { get; set; }
        public string ShipperAddress { get; set; }

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
        public string ShipmentField41 { get; set; }
        public string ShipmentField42 { get; set; }
        public string ShipmentField43 { get; set; }
        public string ShipmentField44 { get; set; }
        public string ShipmentField45 { get; set; }
        public string ShipmentField46 { get; set; }
        public string ShipmentField47 { get; set; }
        public string ShipmentField48 { get; set; }
        public string ShipmentField49 { get; set; }
        public string ShipmentField50 { get; set; }
        public string ShipmentField51 { get; set; }
        public string ShipmentField52 { get; set; }
        public string ShipmentField53 { get; set; }
        public string ShipmentField54 { get; set; }
        public string ShipmentField55 { get; set; }
        public string ShipmentField56 { get; set; }
        public string ShipmentField57 { get; set; }
        public string ShipmentField58 { get; set; }
        public string ShipmentField59 { get; set; }
        public string ShipmentField60 { get; set; }
        public string ShipmentField61 { get; set; }
        public string ShipmentField62 { get; set; }
        public string ShipmentField63 { get; set; }
        public string ShipmentField64 { get; set; }
        public string ShipmentField65 { get; set; }
        public string ShipmentField66 { get; set; }
        public string ShipmentField67 { get; set; }
        public string ShipmentField68 { get; set; }
        public string ShipmentField69 { get; set; }
        public string ShipmentField70 { get; set; }

        public string AirlinePrefix { get; set; }

        public string ModifiedFullMAWB { get; set; }
        public string HouseNumber { get; set; }
        public int? OneDigitPieceNumber { get; set; }
        public string ShipmentNumber { get; set; }

        public string UserName { get; set; }
        public string ConsigneePhoneNumber { get; set; }
        public byte[] AirlineLogo { get; set; }
        public string FlightNumber { get; set; }
    }
}