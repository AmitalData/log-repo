namespace WebFreight.Web.DataProviders
{
    public class AWBLabelsDataProvider
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
        public string AirlinePrefix { get; set; }

        public string ModifiedFullMAWB { get; set; }
        public string HouseNumber { get; set; }
        public int? OneDigitPieceNumber { get; set; }
        public string ShipmentNumber { get; set; }

        public string UserName { get; set; }

    }
}