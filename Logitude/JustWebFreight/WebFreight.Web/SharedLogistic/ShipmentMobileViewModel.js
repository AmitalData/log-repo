
var ListItemClass = function (label, value) {
    this.Label = label
    this.Value = value;
    this.Image1SRC = "";
    this.Image2SRC = "";
    this.IconDisplay = "none";
};

var RoutingItemClass = function () {
    this.LegHeader = "";
    this.FromFlagSRC = "";
    this.FromPortCode = "";
    this.FromPortName = "";
    this.FromDate = "";
    this.FromTime = "";
    this.FromDateTimeIsActual = false;
    this.FromDateTimeVisibility = "collapse";
    this.ToFlagSRC = "";
    this.ToPortCode = "";
    this.ToPortName = "";
    this.ToDate = "";
    this.ToTime = "";
    this.ToDateTimeIsActual = false;
    this.ToDateTimeVisibility = "collapse";
};

var PackageItemClass = function () {
    this.ImageSRC = "";
    this.MainTitle = "";
    this.SubTitle = "";
    this.Volume = "";
    this.GrossWeight = "";
};

function IsLCLShipment(shipment) {

    var Result = false;

    if (shipment.TransportModeId == "A") {
        Result = true;
    }

    else if (shipment.TransportModeId == "O" && shipment.ShipmentTypeId == "LCLD") {
        Result = true;
    }

    else if (shipment.TransportModeId == "I" && shipment.ShipmentTypeId == "LTL") {
        Result = true;
    }

    return Result;
}

(function (jQuery) {

    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;
    jQuery.CurrentCardType = null;
    jQuery.CurrentEntityId = null;
    jQuery.CurrentEntityPM = null;
    jQuery.CurrentEntityKey = null;
    jQuery.CurrentEmail = null;
    jQuery.IsExternalURL = true;

    jQuery.IsTabSelected_GEN = false;
    jQuery.IsTabSelected_PAR = false;
    jQuery.IsTabSelected_CAR = false;

    jQuery.BuildGeneralTabMainData = (function (shipment) {

        var myRef = "";
        var RefLabel = "";
        var RefValue = "";
        var DirectionSRC = "";
        var TransportSRC = "";

        if (shipment.DirectionId == "I") {
            myRef = shipment.ShipperReference1;
            if ($.trim(shipment.ShipperReference2) != "") {
                myRef = ($.trim(myRef) == "") ? shipment.ShipperReference2 : myRef + ", " + shipment.ShipperReference2;
            }
        }

        else {
            myRef = shipment.ConsigneeReference1;
            if ($.trim(shipment.ConsigneeReference2) != "") {
                myRef = ($.trim(myRef) == "") ? shipment.ConsigneeReference2 : myRef + ", " + shipment.ConsigneeReference2;
            }
        }

        if (shipment.ShipmentLevelCode == "H") {

            RefValue = $.trim(shipment.House);
            RefLabel = "House";
        }

        else {

            if (shipment.TransportModeId == "A") {
                RefValue = $.trim(shipment.LongMaster);
            }

            else {

                if ($.trim(shipment.MainCarriageCarrierCode) != "") {

                    RefValue = $.trim(shipment.MainCarriageCarrierCode);

                    if ($.trim(shipment.LongMaster) != "") {
                        RefValue = RefValue + "-" + $.trim(shipment.LongMaster);
                    }
                }

                else {

                    RefValue = $.trim(shipment.LongMaster);
                }
            }

            RefLabel = "Master";
        }

        var status = $.trim(shipment.StatusName);

        if ($.trim(shipment.StatusDate) != "") {
            status = status + "," + $.Convert.ToShortDate(shipment.StatusDate);
        }

        switch (shipment.DirectionId) {

            case "E": {
                DirectionSRC = "Images/Icons/Export.png";
                break;
            }

            case "I": {
                DirectionSRC = "Images/Icons/Import.png";
                break;
            }

            case "D": {
                DirectionSRC = "Images/Icons/Domestic.png";
                break;
            }
        }

        switch (shipment.TransportModeId) {

            case "A": {
                TransportSRC = "Images/Icons/Air.png";
                break;
            }

            case "I": {
                TransportSRC = "Images/Icons/Inland.png";
                break;
            }

            case "O": {
                TransportSRC = "Images/Icons/Ocean.png";
                break;
            }
        }

        var GeneralMainList = [];

        var tracking = new ListItemClass("Tracking Ref", $.trim(shipment.ShipmentNumber));
        tracking.Image1SRC = DirectionSRC;
        tracking.Image2SRC = TransportSRC;
        tracking.IconDisplay = "normal";

        GeneralMainList.push(new ListItemClass("Status", status));
        GeneralMainList.push(tracking);
        GeneralMainList.push(new ListItemClass("My Ref", $.trim(myRef)));
        GeneralMainList.push(new ListItemClass(RefLabel, $.trim(RefValue)));
        //GeneralMainList.push(new ListItemClass("Routing", $.trim(shipment.Routing)));

        $("#GeneralMainListBox").kendoMobileListView(
        {
            dataSource: { data: GeneralMainList },
            template: kendo.template($("#MainItemDataTemplate").html())
        });
    });
    jQuery.BuildGeneralTabRoutingData = (function (shipment) {

        var DataList = [];

        if (shipment.ShipmentPickUps.length > 0) {

            $.each(shipment.ShipmentPickUps, function (index, value) {

                var fullNumber = value.PickUpDeliveryNumber.split('/');
                var number = fullNumber[1];

                var leg = new RoutingItemClass();
                leg.LegHeader = "Pick Up: " + number;
                leg.FromFlagSRC = "../images/Flags/" + ($.trim(value.FromPortCountryCode) != '' ? $.trim(value.FromPortCountryCode) : $.trim(value.FromAddressCountryCode)) + ".png";
                leg.FromPortCode = $.trim(value.FromPortCode) != '' ? $.trim(value.FromPortCode) : $.trim(value.FromAddressCity);
                leg.FromPortName = $.trim(value.FromPortName) != '' ? $.trim(value.FromPortName) : $.trim(value.FromAddressCountryName);
                leg.FromDate = $.trim(value.ATD) != "" ? $.Convert.ToShortDate(value.ATD) : $.Convert.ToShortDate(value.ETD);
                leg.FromTime = $.trim(value.ATD) != "" ? $.Convert.ToShortTime(value.ATD) : $.Convert.ToShortTime(value.ETD);
                leg.FromDateTimeIsActual = $.trim(value.ATD) != "";
                leg.FromDateTimeVisibility = ($.trim(value.ETD) != "" || $.trim(value.ATD) != "") ? "visible" : "collapse";
                leg.ToFlagSRC = "../images/Flags/" + ($.trim(value.ToPortCountryCode) != '' ? $.trim(value.ToPortCountryCode) : $.trim(value.ToAddressCountryCode)) + ".png";
                leg.ToPortCode = $.trim(value.ToPortCode) != '' ? $.trim(value.ToPortCode) : $.trim(value.ToAddressCity);
                leg.ToPortName = $.trim(value.ToPortName) != '' ? $.trim(value.ToPortName) : $.trim(value.ToAddressCountryName);
                leg.ToDate = $.trim(value.ATA) != "" ? $.Convert.ToShortDate(value.ATA) : $.Convert.ToShortDate(value.ETA);
                leg.ToTime = $.trim(value.ATA) != "" ? $.Convert.ToShortTime(value.ATA) : $.Convert.ToShortTime(value.ETA);
                leg.ToDateTimeIsActual = $.trim(value.ATA) != "";
                leg.ToDateTimeVisibility = ($.trim(value.ETA) != "" || $.trim(value.ATA) != "") ? "visible" : "collapse";

                if ($.trim(leg.FromDate) == "") {
                    leg.FromDate = "No Date";
                }

                if ($.trim(leg.ToDate) == "") {
                    leg.ToDate = "No Date";
                }

                DataList.push(leg);
            });
        }

        /* Pre Carriage */
        if ($.trim(shipment.PreCarriageFromPortId) != '' && $.trim(shipment.PreCarriageToPortId) != '') {

            var leg = new RoutingItemClass();
            leg.LegHeader = "Pre Carriage";
            leg.FromFlagSRC = "../images/Flags/" + shipment.PreCarriageFromPortCountryCode + ".png";
            leg.FromPortCode = $.trim(shipment.PreCarriageFromPortCode);
            leg.FromPortName = $.trim(shipment.PreCarriageFromPortName);
            leg.FromDate = $.trim(shipment.PreCarriageATD) != "" ? $.Convert.ToShortDate(shipment.PreCarriageATD) : $.Convert.ToShortDate(shipment.PreCarriageETD);
            leg.FromTime = $.trim(shipment.PreCarriageATD) != "" ? $.Convert.ToShortTime(shipment.PreCarriageATD) : $.Convert.ToShortTime(shipment.PreCarriageETD);
            leg.FromDateTimeIsActual = $.trim(shipment.PreCarriageATD) != "";
            leg.FromDateTimeVisibility = ($.trim(shipment.PreCarriageETD) != "" || $.trim(shipment.PreCarriageATD) != "") ? "visible" : "collapse";
            leg.ToFlagSRC = "../images/Flags/" + shipment.PreCarriageToPortCountryCode + ".png";
            leg.ToPortCode = $.trim(shipment.PreCarriageToPortCode);
            leg.ToPortName = $.trim(shipment.PreCarriageToPortName);
            leg.ToDate = $.trim(shipment.PreCarriageATA) != "" ? $.Convert.ToShortDate(shipment.PreCarriageATA) : $.Convert.ToShortDate(shipment.PreCarriageETA);
            leg.ToTime = $.trim(shipment.PreCarriageATA) != "" ? $.Convert.ToShortTime(shipment.PreCarriageATA) : $.Convert.ToShortTime(shipment.PreCarriageETA);
            leg.ToDateTimeIsActual = $.trim(shipment.PreCarriageATA) != "";
            leg.ToDateTimeVisibility = ($.trim(shipment.PreCarriageETA) != "" || $.trim(shipment.PreCarriageATA) != "") ? "visible" : "collapse";

            if ($.trim(leg.FromDate) == "") {
                leg.FromDate = "No Date";
            }

            if ($.trim(leg.ToDate) == "") {
                leg.ToDate = "No Date";
            }

            DataList.push(leg);
        }

        /* Main Carriage */
        var leg = new RoutingItemClass();
        leg.LegHeader = "Main Carriage Leg 1";
        leg.FromFlagSRC = "../images/Flags/" + shipment.MainCarriageFromPortCountryCode + ".png";
        leg.FromPortCode = $.trim(shipment.MainCarriageFromPortCode);
        leg.FromPortName = $.trim(shipment.MainCarriageFromPortName);
        leg.FromDate = $.trim(shipment.MainCarriageATD) != "" ? $.Convert.ToShortDate(shipment.MainCarriageATD) : $.Convert.ToShortDate(shipment.MainCarriageETD);
        leg.FromTime = $.trim(shipment.MainCarriageATD) != "" ? $.Convert.ToShortTime(shipment.MainCarriageATD) : $.Convert.ToShortTime(shipment.MainCarriageETD);
        leg.FromDateTimeIsActual = $.trim(shipment.MainCarriageATD) != "";
        leg.FromDateTimeVisibility = ($.trim(shipment.MainCarriageETD) != "" || $.trim(shipment.MainCarriageATD) != "") ? "visible" : "collapse";
        leg.ToFlagSRC = "../images/Flags/" + shipment.MainCarriageToPortCountryCode + ".png";
        leg.ToPortCode = $.trim(shipment.MainCarriageToPortCode);
        leg.ToPortName = $.trim(shipment.MainCarriageToPortName);
        leg.ToDate = $.trim(shipment.MainCarriageATA) != "" ? $.Convert.ToShortDate(shipment.MainCarriageATA) : $.Convert.ToShortDate(shipment.MainCarriageETA);
        leg.ToTime = $.trim(shipment.MainCarriageATA) != "" ? $.Convert.ToShortTime(shipment.MainCarriageATA) : $.Convert.ToShortTime(shipment.MainCarriageETA);
        leg.ToDateTimeIsActual = $.trim(shipment.MainCarriageATA) != "";
        leg.ToDateTimeVisibility = ($.trim(shipment.MainCarriageETA) != "" || $.trim(shipment.MainCarriageATA) != "") ? "visible" : "collapse";

        if ($.trim(leg.FromDate) == "") {
            leg.FromDate = "No Date";
        }

        if ($.trim(leg.ToDate) == "") {
            leg.ToDate = "No Date";
        }

        DataList.push(leg);

        /* Transshipment1 */
        if ($.trim(shipment.Transshipment1FromPortId) != '' && $.trim(shipment.Transshipment1ToPortId) != '') {

            var leg = new RoutingItemClass();
            leg.LegHeader = "Main Carriage Leg 2";
            leg.FromFlagSRC = "../images/Flags/" + shipment.Transshipment1FromPortCountryCode + ".png";
            leg.FromPortCode = $.trim(shipment.Transshipment1FromPortCode);
            leg.FromPortName = $.trim(shipment.Transshipment1FromPortName);
            leg.FromDate = $.trim(shipment.Transshipment1ATD) != "" ? $.Convert.ToShortDate(shipment.Transshipment1ATD) : $.Convert.ToShortDate(shipment.Transshipment1ETD);
            leg.FromTime = $.trim(shipment.Transshipment1ATD) != "" ? $.Convert.ToShortTime(shipment.Transshipment1ATD) : $.Convert.ToShortTime(shipment.Transshipment1ETD);
            leg.FromDateTimeIsActual = $.trim(shipment.Transshipment1ATD) != "";
            leg.FromDateTimeVisibility = ($.trim(shipment.Transshipment1ETD) != "" || $.trim(shipment.Transshipment1ATD) != "") ? "visible" : "collapse";
            leg.ToFlagSRC = "../images/Flags/" + shipment.Transshipment1ToPortCountryCode + ".png";
            leg.ToPortCode = $.trim(shipment.Transshipment1ToPortCode);
            leg.ToPortName = $.trim(shipment.Transshipment1ToPortName);
            leg.ToDate = $.trim(shipment.Transshipment1ATA) != "" ? $.Convert.ToShortDate(shipment.Transshipment1ATA) : $.Convert.ToShortDate(shipment.Transshipment1ETA);
            leg.ToTime = $.trim(shipment.Transshipment1ATA) != "" ? $.Convert.ToShortTime(shipment.Transshipment1ATA) : $.Convert.ToShortTime(shipment.Transshipment1ETA);
            leg.ToDateTimeIsActual = $.trim(shipment.Transshipment1ATA) != "";
            leg.ToDateTimeVisibility = ($.trim(shipment.Transshipment1ETA) != "" || $.trim(shipment.Transshipment1ATA) != "") ? "visible" : "collapse";

            if ($.trim(leg.FromDate) == "") {
                leg.FromDate = "No Date";
            }

            if ($.trim(leg.ToDate) == "") {
                leg.ToDate = "No Date";
            }

            DataList.push(leg);
        }


        /* Transshipment2 */
        if ($.trim(shipment.Transshipment2FromPortId) != '' && $.trim(shipment.Transshipment2ToPortId) != '') {

            var leg = new RoutingItemClass();
            leg.LegHeader = "Main Carriage Leg 3";
            leg.FromFlagSRC = "../images/Flags/" + shipment.Transshipment2FromPortCountryCode + ".png";
            leg.FromPortCode = $.trim(shipment.Transshipment2FromPortCode);
            leg.FromPortName = $.trim(shipment.Transshipment2FromPortName);
            leg.FromDate = $.trim(shipment.Transshipment2ATD) != "" ? $.Convert.ToShortDate(shipment.Transshipment2ATD) : $.Convert.ToShortDate(shipment.Transshipment2ETD);
            leg.FromTime = $.trim(shipment.Transshipment2ATD) != "" ? $.Convert.ToShortTime(shipment.Transshipment2ATD) : $.Convert.ToShortTime(shipment.Transshipment2ETD);
            leg.FromDateTimeIsActual = $.trim(shipment.Transshipment2ATD) != "";
            leg.FromDateTimeVisibility = ($.trim(shipment.Transshipment2ETD) != "" || $.trim(shipment.Transshipment2ATD) != "") ? "visible" : "collapse";
            leg.ToFlagSRC = "../images/Flags/" + shipment.Transshipment2ToPortCountryCode + ".png";
            leg.ToPortCode = $.trim(shipment.Transshipment2ToPortCode);
            leg.ToPortName = $.trim(shipment.Transshipment2ToPortName);
            leg.ToDate = $.trim(shipment.Transshipment2ATA) != "" ? $.Convert.ToShortDate(shipment.Transshipment2ATA) : $.Convert.ToShortDate(shipment.Transshipment2ETA);
            leg.ToTime = $.trim(shipment.Transshipment2ATA) != "" ? $.Convert.ToShortTime(shipment.Transshipment2ATA) : $.Convert.ToShortTime(shipment.Transshipment2ETA);
            leg.ToDateTimeIsActual = $.trim(shipment.Transshipment2ATA) != "";
            leg.ToDateTimeVisibility = ($.trim(shipment.Transshipment2ETA) != "" || $.trim(shipment.Transshipment2ATA) != "") ? "visible" : "collapse";

            if ($.trim(leg.FromDate) == "") {
                leg.FromDate = "No Date";
            }

            if ($.trim(leg.ToDate) == "") {
                leg.ToDate = "No Date";
            }

            DataList.push(leg);
        }


        /* Transshipment3 */
        if ($.trim(shipment.Transshipment3FromPortId) != '' && $.trim(shipment.Transshipment3ToPortId) != '') {

            var leg = new RoutingItemClass();
            leg.LegHeader = "Main Carriage Leg 4";
            leg.FromFlagSRC = "../images/Flags/" + shipment.Transshipment3FromPortCountryCode + ".png";
            leg.FromPortCode = $.trim(shipment.Transshipment3FromPortCode);
            leg.FromPortName = $.trim(shipment.Transshipment3FromPortName);
            leg.FromDate = $.trim(shipment.Transshipment3ATD) != "" ? $.Convert.ToShortDate(shipment.Transshipment3ATD) : $.Convert.ToShortDate(shipment.Transshipment3ETD);
            leg.FromTime = $.trim(shipment.Transshipment3ATD) != "" ? $.Convert.ToShortTime(shipment.Transshipment3ATD) : $.Convert.ToShortTime(shipment.Transshipment3ETD);
            leg.FromDateTimeIsActual = $.trim(shipment.Transshipment3ATD) != "";
            leg.FromDateTimeVisibility = ($.trim(shipment.Transshipment3ETD) != "" || $.trim(shipment.Transshipment3ATD) != "") ? "visible" : "collapse";

            leg.ToFlagSRC = "../images/Flags/" + shipment.Transshipment3ToPortCountryCode + ".png";
            leg.ToPortCode = $.trim(shipment.Transshipment3ToPortCode);
            leg.ToPortName = $.trim(shipment.Transshipment3ToPortName);
            leg.ToDate = $.trim(shipment.Transshipment3ATA) != "" ? $.Convert.ToShortDate(shipment.Transshipment3ATA) : $.Convert.ToShortDate(shipment.Transshipment3ETA);
            leg.ToTime = $.trim(shipment.Transshipment3ATA) != "" ? $.Convert.ToShortTime(shipment.Transshipment3ATA) : $.Convert.ToShortTime(shipment.Transshipment3ETA);
            leg.ToDateTimeIsActual = $.trim(shipment.Transshipment3ATA) != "";
            leg.ToDateTimeVisibility = ($.trim(shipment.Transshipment3ETA) != "" || $.trim(shipment.Transshipment3ATA) != "") ? "visible" : "collapse";

            if ($.trim(leg.FromDate) == "") {
                leg.FromDate = "No Date";
            }

            if ($.trim(leg.ToDate) == "") {
                leg.ToDate = "No Date";
            }

            DataList.push(leg);
        }

        /* On Carriage */
        if ($.trim(shipment.OnCarriageFromPortId) != '' && $.trim(shipment.OnCarriageToPortId) != '') {

            var leg = new RoutingItemClass();
            leg.LegHeader = "On Carriage";
            leg.FromFlagSRC = "../images/Flags/" + shipment.OnCarriageFromPortCountryCode + ".png";
            leg.FromPortCode = $.trim(shipment.OnCarriageFromPortCode);
            leg.FromPortName = $.trim(shipment.OnCarriageFromPortName);
            leg.FromDate = $.trim(shipment.OnCarriageATD) != "" ? $.Convert.ToShortDate(shipment.OnCarriageATD) : $.Convert.ToShortDate(shipment.OnCarriageETD);
            leg.FromTime = $.trim(shipment.OnCarriageATD) != "" ? $.Convert.ToShortTime(shipment.OnCarriageATD) : $.Convert.ToShortTime(shipment.OnCarriageETD);
            leg.FromDateTimeIsActual = $.trim(shipment.OnCarriageATD) != "";
            leg.FromDateTimeVisibility = ($.trim(shipment.OnCarriageETD) != "" || $.trim(shipment.OnCarriageATD) != "") ? "visible" : "collapse";
            leg.ToFlagSRC = "../images/Flags/" + shipment.OnCarriageToPortCountryCode + ".png";
            leg.ToPortCode = $.trim(shipment.OnCarriageToPortCode);
            leg.ToPortName = $.trim(shipment.OnCarriageToPortName);
            leg.ToDate = $.trim(shipment.OnCarriageATA) != "" ? $.Convert.ToShortDate(shipment.OnCarriageATA) : $.Convert.ToShortDate(shipment.OnCarriageETA);
            leg.ToTime = $.trim(shipment.OnCarriageATA) != "" ? $.Convert.ToShortTime(shipment.OnCarriageATA) : $.Convert.ToShortTime(shipment.OnCarriageETA);
            leg.ToDateTimeIsActual = $.trim(shipment.OnCarriageATA) != "";
            leg.ToDateTimeVisibility = ($.trim(shipment.OnCarriageETA) != "" || $.trim(shipment.OnCarriageATA) != "") ? "visible" : "collapse";

            if ($.trim(leg.FromDate) == "") {
                leg.FromDate = "No Date";
            }

            if ($.trim(leg.ToDate) == "") {
                leg.ToDate = "No Date";
            }

            DataList.push(leg);
        }

        if (shipment.ShipmentDeliveries.length > 0) {

            $.each(shipment.ShipmentDeliveries, function (index, value) {

                var fullNumber = value.PickUpDeliveryNumber.split('/');
                var number = fullNumber[1];

                var leg = new RoutingItemClass();
                leg.LegHeader = "Delivery: " + number;
                leg.FromFlagSRC = "../images/Flags/" + ($.trim(value.FromPortCountryCode) != '' ? $.trim(value.FromPortCountryCode) : $.trim(value.FromAddressCountryCode)) + ".png";
                leg.FromPortCode = $.trim(value.FromPortCode) != '' ? $.trim(value.FromPortCode) : $.trim(value.FromAddressCity);
                leg.FromPortName = $.trim(value.FromPortName) != '' ? $.trim(value.FromPortName) : $.trim(value.FromAddressCountryName);
                leg.FromDate = $.trim(value.ATD) != "" ? $.Convert.ToShortDate(value.ATD) : $.Convert.ToShortDate(value.ETD);
                leg.FromTime = $.trim(value.ATD) != "" ? $.Convert.ToShortTime(value.ATD) : $.Convert.ToShortTime(value.ETD);
                leg.FromDateTimeIsActual = $.trim(value.ATD) != "";
                leg.FromDateTimeVisibility = ($.trim(value.ETD) != "" || $.trim(value.ATD) != "") ? "visible" : "collapse";
                leg.ToFlagSRC = "../images/Flags/" + ($.trim(value.ToPortCountryCode) != '' ? $.trim(value.ToPortCountryCode) : $.trim(value.ToAddressCountryCode)) + ".png";
                leg.ToPortCode = $.trim(value.ToPortCode) != '' ? $.trim(value.ToPortCode) : $.trim(value.ToAddressCity);
                leg.ToPortName = $.trim(value.ToPortName) != '' ? $.trim(value.ToPortName) : $.trim(value.ToAddressCountryName);
                leg.ToDate = $.trim(value.ATA) != "" ? $.Convert.ToShortDate(value.ATA) : $.Convert.ToShortDate(value.ETA);
                leg.ToTime = $.trim(value.ATA) != "" ? $.Convert.ToShortTime(value.ATA) : $.Convert.ToShortTime(value.ETA);
                leg.ToDateTimeIsActual = $.trim(value.ATA) != "";
                leg.ToDateTimeVisibility = ($.trim(value.ETA) != "" || $.trim(value.ATA) != "") ? "visible" : "collapse";

                if ($.trim(leg.FromDate) == "") {
                    leg.FromDate = "No Date";
                }

                if ($.trim(leg.ToDate) == "") {
                    leg.ToDate = "No Date";
                }

                DataList.push(leg);
            });
        }

        $("#GeneralRoutingListBox").kendoMobileListView(
        {
            dataSource: { data: DataList },
            template: kendo.template($("#RoutingItemDataTemplate").html())
        });
    });
    jQuery.BuildCargoPageData = (function (shipment) {

        var DataList = [];
        var template = "";

        var totalPieces = 0;
        var totalVolume = 0;
        var totalGrossWeight = 0;
        var totalVolumetricWeight = 0;
        var totalChargeableWeight = 0;

        var IsLCL = IsLCLShipment(shipment);
        var PackagesLabel = IsLCL ? "Packages" : "Containers";
        var ChargeableLabel = IsLCL ? "Chargeable Weight" : "Wt / Msr";


        if (shipment.ShipmentPackages.length > 0) {

            template = "#PackageItemDataTemplate";

            totalPieces = IsLCL ? ($.trim(shipment.NumberOfPackages) == "" ? 0 : shipment.NumberOfPackages) : ($.trim(shipment.NumberOfContainers) == "" ? 0 : shipment.NumberOfContainers);
            totalVolume = $.trim(shipment.Volume) == "" ? 0 : shipment.Volume;
            totalGrossWeight = $.trim(shipment.GrossWeight) == "" ? 0 : shipment.GrossWeight;
            totalVolumetricWeight = $.trim(shipment.VolumetricWeight) == "" ? 0 : shipment.VolumetricWeight;
            totalChargeableWeight = $.trim(shipment.ChargeableWeight) == "" ? 0 : shipment.ChargeableWeight;

            $.each(shipment.ShipmentPackages, function (index, item) {

                var dataItem = new PackageItemClass();

                if (IsLCL) {

                    var length = $.trim(item.Length) == "" ? "" : item.Length;
                    var width = $.trim(item.Width) == "" ? "" : item.Width;
                    var height = $.trim(item.Height) == "" ? "" : item.Height;

                    dataItem.ImageSRC = "Images/Icons/Package.png"
                    dataItem.MainTitle = $.trim(item.Quantity) == "" ? 0 : item.Quantity;
                    dataItem.SubTitle = length + "-" + width + "-" + height;

                }

                else {

                    dataItem.ImageSRC = "Images/Icons/Container.png"
                    dataItem.SubTitle = $.trim(item.PackageTypeName) == "" ? "" : item.PackageTypeName;
                    dataItem.MainTitle = $.trim(item.ContainerNumber) == "" ? "" : item.ContainerNumber;
                }

                var itemVolume = $.trim(item.Volume) == "" ? 0 : item.Volume;
                var itemGrossWeight = $.trim(item.Weight) == "" ? 0 : item.Weight;

                dataItem.Volume = itemVolume.toFixed(3);
                dataItem.GrossWeight = itemGrossWeight.toFixed(3);

                DataList.push(dataItem);
            });
        }

        else {

            template = "#PackageEstimateTemplate";

            totalPieces = $.trim(shipment.BookingNumberOfPackages) == "" ? 0 : shipment.BookingNumberOfPackages;
            totalVolume = $.trim(shipment.BookingVolume) == "" ? 0 : shipment.BookingVolume;
            totalGrossWeight = $.trim(shipment.OrderGrossWeight) == "" ? 0 : shipment.OrderGrossWeight;
            totalVolumetricWeight = $.trim(shipment.OrderVolumetricWeight) == "" ? 0 : shipment.OrderVolumetricWeight;
            totalChargeableWeight = $.trim(shipment.OrderChargeableWeight) == "" ? 0 : shipment.OrderChargeableWeight;

            $.each(shipment.ShipmentOrderPackages, function (index, item) {

                var dataItem = new PackageItemClass();

                if (IsLCL) {

                    dataItem.ImageSRC = "Images/Icons/Package.png"

                }

                else {

                    dataItem.ImageSRC = "Images/Icons/Container.png"
                }

                dataItem.MainTitle = $.trim(item.PackageTypeName) == "" ? "" : item.PackageTypeName;
                dataItem.SubTitle = $.trim(item.Quantity) == "" ? 0 : item.Quantity;

                DataList.push(dataItem);
            });
        }

        var GeneralCargoList = [];
        GeneralCargoList.push(new ListItemClass(PackagesLabel, totalPieces));
        GeneralCargoList.push(new ListItemClass("Gross Weight", totalGrossWeight.toFixed(3) + " (" + shipment.GrossWeightUnitCode + ")"));
        GeneralCargoList.push(new ListItemClass("Volume", totalVolume.toFixed(3) + " (" + shipment.VolumeUnitCode + ")"));
        //GeneralCargoList.push(new ListItemClass("Volumetric Weight", totalVolumetricWeight.toFixed(3) + " (" + shipment.ChargeableWeightUnitCode + ")"));
        GeneralCargoList.push(new ListItemClass(ChargeableLabel, totalChargeableWeight.toFixed(3) + " (" + shipment.ChargeableWeightUnitCode + ")"));

        $("#CargoSummaryListBox").kendoMobileListView(
        {
            dataSource: { data: GeneralCargoList },
            template: kendo.template($("#ListBoxItemTemplate").html())
        });

        $("#CargoDetailsListBox").kendoMobileListView(
        {
            dataSource: { data: DataList },
            template: kendo.template($(template).html())
        });
    });

    jQuery.GetSingleEntityPM = (function () {

        $("#GeneralPageBusyIndicator").show();
        $("#CargoPageBusyIndicator").show();

        var url = "../api/shipments/getsinglepmbykey/" + $.CurrentEntityKey + "/" + $.CurrentEntityId + "/" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (shipmentPM) {

                if (shipmentPM) {
                    $.CurrentEntityPM = shipmentPM;
                    $.CurrentEntityId = shipmentPM.Id;

                    $.BuildGeneralTabMainData($.CurrentEntityPM);
                    $.BuildGeneralTabRoutingData($.CurrentEntityPM);
                    $.BuildCargoPageData($.CurrentEntityPM);
                }

                else {

                    if ($.IsExternalURL) {

                        $("#Container").hide();
                        $("#InvalidKeyArea").show();
                    }
                }

                $("#GeneralPageBusyIndicator").hide();
                $("#CargoPageBusyIndicator").hide();
                //$.SendContactActivity($.CurrentEmail, "Shipment", "Shipment Display", $.CurrentTenant, $.CurrentCardId);
            },

            error: function (jqXHR, textStatus, errorThrown) {

                $.CheckUserException(jqXHR);
                $("#GeneralPageBusyIndicator").hide();
                $("#CargoPageBusyIndicator").hide();

                if ($.IsExternalURL) {

                    $("#Container").hide();
                    $("#InvalidKeyArea").show();
                }
            }
        });
    });
    jQuery.GetShipmentPartners = (function () {

        $("#PartnersPageBusyIndicator").show();

        var url = "../api/shipments?securitykey=" + $.CurrentEntityKey + "&shipmentId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $("#PartnersListBox").kendoMobileListView(
                {
                    dataSource: { data: result },
                    template: kendo.template($("#PartnerItemDataTemplate").html())
                });

                $("#PartnersPageBusyIndicator").hide();
                //$.SendContactActivity($.CurrentEmail, "Shipment", "Partners Display", $.CurrentTenant, $.CurrentCardId);
            },

            error: function (jqXHR, textStatus, errorThrown) {

                $.CheckUserException(jqXHR);
                $("#PartnersPageBusyIndicator").hide();

                if ($.IsExternalURL) {

                    $("#Container").hide();
                    $("#InvalidKeyArea").show();
                }
            }
        });

    });

    $(document).ready(function () {

        var isLocalTesting = false;

        if (isLocalTesting) {

            // LCL : 1-790 : e85d598362f242a4865a9c9bea2a0d0f
            // FCL : 1-616 : 0B61E840164C4347BE6B3B58321F3C69
            // LCL[OrderPackages] 1-787 : 345DF874DED542CCA051DF15E2346FA5

            $.CurrentTenant = 1;
            $.CurrentEntityId = "1-616";
            $.CurrentEntityKey = "0B61E840164C4347BE6B3B58321F3C69";
            $.CurrentEmail = "jalal@mail.com";
            $.CurrentCardId = "1-1009";
        }

        else {

            var hash = $(location).attr('href');
            var arr = hash.split('?');

            var dataParam = arr[1].split('=');

            var linkQuery = dataParam[1];
            var linkParameters = null;
            if (linkQuery && linkQuery.indexOf('%3A') > -1) {
                linkParameters = linkQuery.split('%3A')
            }
            else {
                linkParameters = linkQuery.split(':')
            }

            $.CurrentEntityKey = linkParameters[0];
            $.CurrentEntityId = linkParameters[1];
            $.CurrentTenant = linkParameters[2];

        }

        var app = new kendo.mobile.Application($(document.body), {

            //transition: 'slide',

            viewShow: function (e) {

                var id = $(e.view).attr("id");

                switch (id) {

                    case "#Tab_GEN":
                        {
                            if (!$.IsTabSelected_GEN) {

                                $.IsTabSelected_GEN = true;
                            }

                            break;
                        }

                    case "#Tab_PAR":
                        {
                            if (!$.IsTabSelected_PAR) {

                                $.IsTabSelected_PAR = true;
                                $.GetShipmentPartners();
                            }

                            break;
                        }

                    case "#Tab_CAR":
                        {
                            if (!$.IsTabSelected_CAR) {

                                $.IsTabSelected_CAR = true;
                            }

                            break;
                        }
                }
            }
        });

        $.GetSingleEntityPM();

    });



}(jQuery));