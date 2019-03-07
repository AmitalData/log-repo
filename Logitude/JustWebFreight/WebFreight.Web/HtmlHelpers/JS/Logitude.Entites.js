
var HelperListClass = function () {
    this.Code = "";
    this.Name = "";
    this.CreateDate = "";
    this.UpdateDate = "";
    this.Language = "";
    this.Type = "";
    this.Category = "";
    this.VideoURL = "";
    this.Duration = "";
    this.FileName = ""
    this.TypeSRC = "";
};
 
var LogitudeRoutingClass = function () {

    this.LegHeader = "";
    this.FromFlagSRC = "";
    this.FromPortCode = "";
    this.FromPortName = "";
    this.FromDate = "";
    this.FromTime = "";
    this.FromDateColor = "#282E30";
    this.FromDateTimeIsActual = false;
    this.ToFlagSRC = "";
    this.ToPortCode = "";
    this.ToPortName = "";
    this.ToDate = "";
    this.ToTime = "";
    this.ToDateColor = "#282E30";
    this.ToDateTimeIsActual = false;
    this.FromDateTimeVisibility = " ";
    this.ToDateTimeVisibility = " ";
    this.Carrier = "";
    this.CarrierName = "";
    this.CarrierNumber = "";
    this.CarrierWebSite = "";
    this.CarrierHasWebSite = false;
    this.Master = "";
    this.MasterVisibility = "visible";
    this.CarrierVisibility = "visible";
    this.Vissel = "";
    this.VisselVisibility = "collapse";
    this.RoutingImageSRC = "images/Icons/Routing.A.png";
    this.DisplayCarrier = " ";
    this.DisplayAreaWhite = " ";
    this.DisplayAreaWhiteCarrier = " ";
    //this.EndListBorder = " ";
    this.DisplayAcutalEstimateAera =" ";
 
};

var ShipmentListClass = function () {

    this.ShipmentList = null;
    this.DirectionSRC = "";
    this.TransportSRC = "";
    this.DirectionName = "";
    this.TransportName = "";
    this.ShipmentId = "";
    this.ShipmentNumber = "";
    this.MyReference = "";
    this.MyReferenceLabel = "My Ref";
    this.MyPartnerName = "";
    this.IncotermCode = "";
    this.FromPortCode = ""
    this.FromPortName = "";
    this.FromCountyCode = "";
    this.FromFlagSRC = "";
    this.FromCountySRC = "";
    this.FromDate = "";
    this.FromTime = "";
    this.FromDateType = "";
    this.FromDateColor = "";
    this.FromDateTypeColor = "";
    this.ToPortCode = "";
    this.ToPortName = "";
    this.ToCountyCode = "";
    this.ToFlagSRC = "";
    this.ToCountySRC = "";
    this.ToDate = "";
    this.ToTime = "";
    this.ToDateType = "";
    this.ToDateColor = "";
    this.ToDateTypeColor = "";
    this.PackageTypeSRC = "";
    this.NumberOfPackages = "";
    this.ChargeableWeight = "";
    this.ShipmentType = "";
    this.Reference = "";
    this.ReferenceLabel = "";
    this.StatusName = "";
    this.StatusColor = "";
    this.StatusDate = "";
    this.DescriptionOfGoods = "";

    this.CarrierCode = "";
    this.CarrierName = "";
    this.CarrierNumber = "";
    this.CarrierText = "";
    this.CarrierVisibility = "collapse";
    this.NoCarrierTextVisibility = "collapse";
    this.ETA = "";
    this.ItemBackground = "#FFFFFF";
    this.StarImage = "images/starwhite.png";

    this.Displayfirstlistmargin = "";
    this.DesignMyReference = "";
    this.DesignMyReferenceVisibility = "collapse";
    this.Carrierlable = "";
    this.DateTimeVisibility = "collapse";


    this.House = "";
    this.Master = "";
    this.LableHouse = "";
    this.LableMaster = "";
    this.LableShipmentNumber = "";
    this.LastLogDateShort = "";
    this.LastLogDateLong = "";

    this.DeliveryDate = "";
    this.DeliveryDateVisibility = "collapse";
    this.MyPartnerVisibility = "collapse";
};

var InvoiceListClass = function () {

    this.EntityId = "";
    this.EntityNumber = "";
    this.OurRefNumber = "";
    this.InvoiceDate = "";

    this.DueDate = "";
    this.Amount = "";
    this.AmountDue = "";
    this.AmountDueColor = "#282E30";
    this.OpenAmount = "";

    this.StatusName = "";
    this.StatusColor = "#282E30";

}

var CustomerListClass = function () {

    this.EntityId = "";
    this.RankName = "";
    this.RankCode = "";
    this.BillToName = "";
    this.AccountManagerUserEnglishName = "";
    this.SalesmanUserEnglishName = "";
    this.PaymentTermEnglishName = "";
    this.Website = "";
    this.Code = "";
    this.EnglishName = "";
    this.VatNumber = "";
    this.LocalName = "";
    this.InActive = false;          
    this.AccountingCard = "";
    this.Notes = "";
    this.CreateDate = "";
    this.StartWorkingDate = "";
    this.StartWorkingManuallySet = false;
    this.LastShipmentDate = "";
    this.Activity = "";
    this.InvoicesDue = "";
    this.CityName = "";
    this.SharedLogisticsInvitationStatusName = "";
    this.LastLoginDate = "";
    this.InvitationDate = "";
}

var EventClass = function () {
    
    this.Name = "";
    this.LogDate = "";
    this.LogTime = "";
    this.EventDate = "";
    this.EventTime = "";
    this.Username = "";
    this.Notes = "";
    this.DisplayEventNotesMobile = " ";
    this.displayendline = " ";
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

function BuildShipmentsList(shipments, TenantDateTimeFormat, IsAgentShared, IsShipperShared, IsConsigneeShared) {
    
    var ShipmentsList = [];

    $.each(shipments, function (index, shipment) {

        var item = new ShipmentListClass();
        item.ShipmentId = shipment.Id;
        item.ShipmentNumber = $.trim(shipment.ShipmentNumber);
        item.IncotermCode = $.trim(shipment.IncotermCode);
        item.DescriptionOfGoods = $.trim(shipment.DescriptionOfGoods);
        item.CarrierCode = $.trim(shipment.MainCarriageCarrierCode);
        item.CarrierName = $.trim(shipment.MainCarriageCarrierName);
        item.CarrierNumber = $.trim(shipment.MainCarriageCarrierNumber);
        item.ETA = $.trim(shipment.MainCarriageETA);

        item.LastLogDateShort = $.Convert.ToDateShortAge(shipment.LastStatusLogDate);
        item.LastLogDateLong = $.Convert.ToDateLongAge(shipment.LastStatusLogDate);
        
        if ($.trim(item.LastLogDateShort) == "") {
            item.LastLogDateShort = "";
        }

        if ($.trim(item.LastLogDateLong) == "") {
            item.LastLogDateLong = "";
        }

        if (shipment.DirectionId == "D" && shipment.TransportModeId == "I") {
            
            item.FromPortName = $.trim(shipment.MainCarriageFromCity);
            item.FromCountyCode = $.trim(shipment.MainCarriageFromCountryCode);
            item.FromCountySRC = "images/Flags/" + item.FromCountyCode + ".png";

            item.ToPortName = $.trim(shipment.MainCarriageToCity);
            item.ToCountyCode = $.trim(shipment.MainCarriageToCountryCode);
            item.ToCountySRC = "images/Flags/" + item.ToCountyCode + ".png";
        }

        else {

            item.FromPortCode = $.trim(shipment.FromPort);
            item.FromPortName = $.trim(shipment.FromPortName);
            item.FromCountyCode = $.trim(shipment.FromCountryCode);
            item.FromCountySRC = "images/Flags/" + item.FromCountyCode + ".png";

            item.ToPortCode = $.trim(shipment.ToPort);
            item.ToPortName = $.trim(shipment.ToPortName);
            item.ToCountyCode = $.trim(shipment.ToCountryCode);
            item.ToCountySRC = "images/Flags/" + item.ToCountyCode + ".png";
        }

        var _myRef = "";
        var _myPartnerName = "";

        if (shipment.ShipmentLevelCode == "C") {
            if (!IsAgentShared) {
                item.MyPartnerVisibility = "collapse";
            }
            else {
                item.MyPartnerVisibility = "visible";
            }

            _myPartnerName = shipment.AgentName;

            _myRef = shipment.AgentReference1;
            if ($.trim(shipment.AgentReference2) != "") {
                _myRef = ($.trim(_myRef) == "") ? shipment.AgentReference2 : _myRef + ", " + shipment.AgentReference2;
            }
        }

        else {
            if (shipment.DirectionId == "I") {
                if (!IsShipperShared) {
                    item.MyPartnerVisibility = "collapse";
                }
                else {
                    item.MyPartnerVisibility = "visible";
                }

                _myPartnerName = shipment.Shipper;
            }

            else {
                if (!IsConsigneeShared) {
                    item.MyPartnerVisibility = "collapse";
                }
                else {
                    item.MyPartnerVisibility = "visible";
                }

                _myPartnerName = shipment.Consignee;
            }

            _myRef = shipment.CustomerReference1;
            if ($.trim(shipment.CustomerReference2) != "") {
                _myRef = ($.trim(_myRef) == "") ? shipment.CustomerReference2 : _myRef + ", " + shipment.CustomerReference2;
            }
        }

        item.MyReference = $.trim(_myRef) != "" ? _myRef : "";
        item.MyPartnerName = $.trim(_myPartnerName) != "" ? _myPartnerName : "";

        if (shipment.Tenant == 1495) {
            item.MyReferenceLabel = "Project #";
            item.MyReference = $.trim(shipment.ProjectNumber) != "" ? shipment.ProjectNumber : "";


            if ($.trim(shipment.Field2) != "") {
                var fieldValue = shipment.Field2;

                if ($.trim(fieldValue) != "") {

                    var date = new Date();
                    date.setUTCFullYear(Number(fieldValue.substr(0, 4)));
                    date.setUTCMonth(Number(fieldValue.substr(4, 2)) - 1);
                    date.setUTCDate(Number(fieldValue.substr(6, 2)));
                    date.setUTCHours(Number(fieldValue.substr(8, 2)));
                    date.setUTCMinutes(Number(fieldValue.substr(10, 2)));
                    date.setUTCSeconds(Number(fieldValue.substr(12, 2)));

                    item.DeliveryDate = $.trim(date) != "" ? $.Convert.ToShortDate(date, TenantDateTimeFormat) : "";

                }
            } 

            item.DeliveryDateVisibility = "visible";
        }

        switch (shipment.DirectionId) {

            case "E": {
                item.DirectionSRC = "HtmlHelpers/Images/Icons/Export.png";
                item.DirectionName = "Export";
                break;
            }

            case "I": {
                item.DirectionSRC = "HtmlHelpers/Images/Icons/Import.png";
                item.DirectionName = "Import";
                break;
            }

            case "D": {
                item.DirectionSRC = "HtmlHelpers/Images/Icons/Domestic.png";
                item.DirectionName = "Domestic";
                break;
            }

            case "R": {
                item.DirectionSRC = "HtmlHelpers/Images/Icons/Drop.png";
                item.DirectionName = "Drop";
                break;
            }

            case "C": {
                item.DirectionSRC = "HtmlHelpers/Images/Icons/CustomsImport.png";
                item.DirectionName = "Customs Import";
                break;
            }
        }

        switch (shipment.TransportModeId) {

            case "A": {
                item.TransportSRC = "HtmlHelpers/Images/Icons/Air.png";
                item.TransportName = "Air";
                break;
            }

            case "I": {
                item.TransportSRC = "HtmlHelpers/Images/Icons/Inland.png";
                item.TransportName = "Inland";
                break;
            }

            case "O": {
                item.TransportSRC = "HtmlHelpers/Images/Icons/Ocean.png";
                item.TransportName = "Ocean";
                break;
            }
        }

        item.PackageTypeSRC = IsLCLShipment(shipment) ? "HtmlHelpers/Images/Icons/Package.png" : "HtmlHelpers/Images/Icons/Container.png";

        if (IsLCLShipment(shipment)) {            
            var num = $.trim(shipment.NumberOfPackages) == "" ? 0 : shipment.NumberOfPackages;

            if (num == 0) {
                num = $.trim(shipment.BookingNumberOfPackages) == "" ? 0 : shipment.BookingNumberOfPackages;
            }

            item.NumberOfPackages = num + " Packages";
        }

        else {            
            var num = $.trim(shipment.NumberOfContainers) == "" ? 0 : shipment.NumberOfContainers;

            if (num == 0) {
                num = $.trim(shipment.BookingNumberOfPackages) == "" ? 0 : shipment.BookingNumberOfPackages;
            }

            item.NumberOfPackages = num + " Containers";
        }

        var charg = $.trim(shipment.ChargeableWeight) == "" ? 0 : shipment.ChargeableWeight;
        if (charg == 0) {
            charg = $.trim(shipment.OrderChargeableWeight) == "" ? 0 : shipment.OrderChargeableWeight;
        }

        item.ChargeableWeight = charg.toFixed(3) + " (" + shipment.ChargeableWeightUnitCode + ")";

        if ($.trim(shipment.MainCarriageATD) != "") {

            item.FromDate = $.Convert.ToShortDate(shipment.MainCarriageATD, TenantDateTimeFormat);
            item.FromTime = $.Convert.ToShortTime(shipment.MainCarriageATD);
            item.FromDateType = "(Actual)";
            item.FromDateColor = "#282E30";
            item.FromDateTypeColor = "#009161";
        }

        else {

            if ($.trim(shipment.MainCarriageETD) != "") {
                item.FromDate = $.Convert.ToShortDate(shipment.MainCarriageETD, TenantDateTimeFormat);
                item.FromTime = $.Convert.ToShortTime(shipment.MainCarriageETD);
                item.FromDateType = "(Estimate)";
                item.FromDateColor = "#282E30";
                item.FromDateTypeColor = "#6E7172";
            }

            else {
                item.FromDate = "No Date";
                item.FromDateColor = "Silver";
            }
        }       

        if ($.trim(shipment.MainCarriageFinalDestinationATA) != "") {

            item.ToDate = $.Convert.ToShortDate(shipment.MainCarriageFinalDestinationATA, TenantDateTimeFormat);
            item.ToTime = $.Convert.ToShortTime(shipment.MainCarriageFinalDestinationATA);
            item.ToDateType = "(Actual)";
            item.ToDateColor = "#282E30";
            item.ToDateTypeColor = "#009161";
        }

        else {

            if ($.trim(shipment.MainCarriageFinalDestinationETA) != "") {
                item.ToDate = $.Convert.ToShortDate(shipment.MainCarriageFinalDestinationETA, TenantDateTimeFormat);
                item.ToTime = $.Convert.ToShortTime(shipment.MainCarriageFinalDestinationETA);
                item.ToDateType = "(Estimate)";
                item.ToDateColor = "#282E30";
                item.ToDateTypeColor = "#6E7172";
            }

            else {
                item.ToDate = "No Date";
                item.ToDateColor = "Silver";
            }
        }

        if (shipment.ShipmentLevelCode == "H") {

            item.Reference = $.trim(shipment.House);
            item.ReferenceLabel = "House:";
        }

        else {
            item.Reference = $.trim(shipment.LongMaster);
            item.ReferenceLabel = "Master:";
        }

        
        item.ShipmentType = $.trim(shipment.ShipmentType);        
        item.StatusColor = $.Convert.ToColor(shipment.StatusName);
        item.StatusDate = $.Convert.ToShortDate(shipment.StatusDate, TenantDateTimeFormat);
        
        if ($.trim(shipment.StatusName) != "")
        {
            item.StatusName = $.trim(shipment.StatusName);

            if ($.trim(shipment.StatusLocation) != "")
            {
                item.StatusName += " (" + $.trim(shipment.StatusLocation) + ")";
            }
        }

        ShipmentsList.push(item);
    });

    return ShipmentsList;
}

function BuildShipmentBackAreaViewModel(shipment, PathPrefix) {

    var viewModel =
        {
            ShipmentNumber: ko.observable(shipment.ShipmentNumber),
            DirectionSRC: ko.observable(""),
            TransportSRC: ko.observable(""),
        };

    switch (shipment.DirectionId) {
        case "E": {
            viewModel.DirectionSRC = PathPrefix + "HtmlHelpers/Images/Icons/Export.png";
            break;
        }
        case "I": {
            viewModel.DirectionSRC = PathPrefix + "HtmlHelpers/Images/Icons/Import.png";
            break;
        }
        case "D": {
            viewModel.DirectionSRC = PathPrefix + "HtmlHelpers/Images/Icons/Domestic.png";
            break;
        }
        case "R": {
            viewModel.DirectionSRC = PathPrefix + "HtmlHelpers/Images/Icons/Drop.png";
            break;
        }

        case "C": {
            viewModel.DirectionSRC = PathPrefix + "HtmlHelpers/Images/Icons/CustomsImport.png";
            break;
        }
    }

    switch (shipment.TransportModeId) {
        case "A": {
            viewModel.TransportSRC = PathPrefix + "HtmlHelpers/Images/Icons/Air.png";
            break;
        }
        case "O": {
            viewModel.TransportSRC = PathPrefix + "HtmlHelpers/Images/Icons/Ocean.png";
            break;
        }
        case "I": {
            viewModel.TransportSRC = PathPrefix + "HtmlHelpers/Images/Icons/Inland.png";
            break;
        }
    }

    return viewModel;
}
function BuildShipmentHeaderViewModel(shipment, TenantDateTimeFormat, PathPrefix) {

    $(".ShowPartnerData").hide();
    $(".ShowTenant1495Data").hide();

    var viewModel =
    {
        MyReference: ko.observable(""),
        MyReferenceLabel: ko.observable("Ref."),
        House: ko.observable(shipment.House),
        Routing: ko.observable(shipment.Routing),
        Status: ko.observable(shipment.StatusName),
        StatusColor: $.Convert.ToColor(shipment.StatusName),

        ShipmentNumber: ko.observable(shipment.ShipmentNumber),
        PartnerTitle: ko.observable(""),
        PartnerName: ko.observable(""),
        PartnerAddress: ko.observable(""),
        PartnerContact: ko.observable(""),
        PartnerCountrySRC: ko.observable(""),
        PartnerVisibility: "collapse",

        FromCountySRC: ko.observable(""),
        ToCountySRC: ko.observable(""),

        FromPortName: ko.observable(shipment.MainCarriageFromPortName),
        ToPortName: ko.observable(shipment.MainCarriageFinalDestinationPortName),

        DeliveryDate: ko.observable(""),
    };
    
    viewModel.FromCountySRC = PathPrefix + "images/Flags/" + shipment.MainCarriageFromPortCountryCode + ".png";
    viewModel.ToCountySRC = PathPrefix + "images/Flags/" + shipment.MainCarriageFinalDestinationPortCountryCode + ".png";

    if ($.trim(shipment.StatusName) != "") {

        if ($.trim(shipment.StatusLocation) != "") {

            var myStatus = shipment.StatusName + " (" + shipment.StatusLocation + ")";
            viewModel.Status = myStatus;
        }
    }

    var _myRef = "";
    
    if (shipment.ShipmentLevelCode == "C") {
        if (!shipment.IsSharedLogisticsAgentVisible) {
            viewModel.PartnerVisibility = "collapse";
        }
        else {
            viewModel.PartnerVisibility = "visible";
        }

        viewModel.PartnerTitle = "Agent: ";
        viewModel.PartnerName = shipment.AgentName;
        viewModel.PartnerAddress = shipment.AgentAddressText;

        if (shipment.IsSharedLogisticsAgentVisible) {
            if ($.trim(shipment.AgentAddressCountryCode) != "") {
                viewModel.PartnerCountrySRC = PathPrefix + "images/Flags/" + shipment.AgentAddressCountryCode + ".png";
                $(".ShowPartnerData").show();
            }
        }

        _myRef = shipment.AgentReference1;
        if ($.trim(shipment.AgentReference2) != "") {
            _myRef = ($.trim(_myRef) == "") ? shipment.AgentReference2 : _myRef + ", " + shipment.AgentReference2;
        }
    }

    else {
        if (shipment.DirectionId == "I") {
            if (!shipment.IsSharedLogisticsShipperVisible) {
                viewModel.PartnerVisibility = "collapse";
            }
            else {
                viewModel.PartnerVisibility = "visible";
            }

            viewModel.PartnerTitle = "Shipper: ";
            viewModel.PartnerName = shipment.ShipperName;
            viewModel.PartnerAddress = shipment.ShipperAddressText;

            if (shipment.IsSharedLogisticsShipperVisible) {
                if ($.trim(shipment.ShipperAddressCountryCode) != "") {
                    viewModel.PartnerCountrySRC = PathPrefix + "images/Flags/" + shipment.ShipperAddressCountryCode + ".png";
                    $(".ShowPartnerData").show();
                }
            }
        }

        else {
            if (!shipment.IsSharedLogisticsConsigneeVisible) {
                viewModel.PartnerVisibility = "collapse";
            }
            else {
                viewModel.PartnerVisibility = "visible";
            }

            viewModel.PartnerTitle = "Consignee: ";
            viewModel.PartnerName = shipment.ConsigneeName;
            viewModel.PartnerAddress = shipment.ConsigneeAddressText;

            if (shipment.IsSharedLogisticsConsigneeVisible) {
                if ($.trim(shipment.ConsigneeAddressCountryCode) != "") {
                    viewModel.PartnerCountrySRC = PathPrefix + "images/Flags/" + shipment.ConsigneeAddressCountryCode + ".png";
                    $(".ShowPartnerData").show();
                }
            }
        }

        _myRef = shipment.CustomerReference1;
        if ($.trim(shipment.CustomerReference2) != "") {
            _myRef = ($.trim(_myRef) == "") ? shipment.CustomerReference2 : _myRef + ", " + shipment.CustomerReference2;
        }
    }

    viewModel.MyReference = $.trim(_myRef) != "" ? _myRef : "";

    if (shipment.Tenant == 1495) {
        viewModel.MyReferenceLabel = "Project #";
        viewModel.MyReference = $.trim(shipment.ProjectNumber) != "" ? shipment.ProjectNumber : "";

        if ($.trim(shipment.Field2) != "") {
            var fieldValue = shipment.Field2.Value;

            if ($.trim(fieldValue) != "") {

                var date = new Date();
                date.setUTCFullYear(Number(fieldValue.substr(0, 4)));
                date.setUTCMonth(Number(fieldValue.substr(4, 2)) - 1);
                date.setUTCDate(Number(fieldValue.substr(6, 2)));
                date.setUTCHours(Number(fieldValue.substr(8, 2)));
                date.setUTCMinutes(Number(fieldValue.substr(10, 2)));
                date.setUTCSeconds(Number(fieldValue.substr(12, 2)));

                viewModel.DeliveryDate = $.trim(date) != "" ? $.Convert.ToShortDate(date, TenantDateTimeFormat) : "";
            }
        }        

        $(".ShowTenant1495Data").show();
    }

    return viewModel;
}
function BuildDocumentsTabPageViewModel(documents, PathPrefix, showIsDigitallySigned) {

    var GridColumns = [];
    var GridDataSource = [];

    var iconTemplate = "";
    iconTemplate += "<a id='#= Id #' href='#= Url #' target='_blank' OnClick='OnDownloadDocument()' style='width:25px; height:25px; vertical-align:middle; display: block; margin: auto; margin-left: -3px;'>";
    iconTemplate += "<img src='#= FileType #' style='width:25px; height:25px; vertical-align:middle; display: block; margin: auto;' />";
    iconTemplate += "</a>";
    GridColumns.push({ title: " ", field: "FileType", width: 35, template: iconTemplate });

    //GridColumns.push({ title: " ", field: "FileType", width: 35, template: "<img src='#= FileType #' style='width:25px; height:25px; vertical-align:middle; display: block; margin: auto;' />" });

    GridColumns.push({ title: "File Name", field: "FileName", width: 350 });
    GridColumns.push({ title: "Name", field: "Name", width: 350 });
    GridColumns.push({ title: "Reference No.", field: "Reference", width: 150 });

    if (showIsDigitallySigned) {
        var imgTemplate = "";
        //imgTemplate += "<a id='#= Id #' href='#= Url #' target='_blank' OnClick='OnDownloadDocument()' style='width:20px; height:20px; vertical-align:middle; margin-left: 35px;'>";
        //imgTemplate += "<img src='#= DigitallySignedSRC #' style='width:20px; height:20px; vertical-align:middle;' />";
        //imgTemplate += "</a>";
        imgTemplate += "<div style='width:20px; height:20px; vertical-align:middle; margin-left: 35px;'>";
        imgTemplate += "<img src='#= DigitallySignedSRC #' style='width:20px; height:20px; vertical-align:middle;' />";
        imgTemplate += "</div>";

        GridColumns.push({ title: "Digitally Signed", width: 100, field: "IsDigitallySigned", template: imgTemplate });
    }

    var linkTemplate = "";
    linkTemplate += "<a id='#= Id #' href='#= Url #' target='_blank' OnClick='OnDownloadDocument()'>";
    linkTemplate += "<div style='cursor:pointer; font-size:11px; color:\\#27AAE1; text-align:right; padding-right: 10px;'>View</div>";
    linkTemplate += "</a>";

    GridColumns.push({ title: " ", template: linkTemplate });

    $.each(documents, function (index, item) {

        var itemId = $.trim(item.Id) == "" ? "" : item.Id;
        var itemUrl = $.trim(item.Url) == "" ? "" : item.Url;
        var itemFileType = PathPrefix + "images/FileIcons/" + $.Convert.ToFileExtentionImage(item.FileExtension);
        var itemFileName = $.trim(item.FileName) == "" ? "" : item.FileName;
        var itemName = $.trim(item.Name) == "" ? "" : item.Name;
        var itemReference = $.trim(item.Reference) == "" ? "" : item.Reference;
        var itemIsDigitallySigned = item.IsDigitallySigned;
        var itemDigitallySignedSRC = PathPrefix + "images/icons/DigitallyNotSigned.png";

        if (itemIsDigitallySigned) {
            itemDigitallySignedSRC = PathPrefix + "images/icons/DigitallySigned.png";
        }

        GridDataSource.push({
            FileType: itemFileType,
            FileName: itemFileName,
            Name: itemName,
            Reference: itemReference,
            IsDigitallySigned: itemIsDigitallySigned,
            DigitallySignedSRC: itemDigitallySignedSRC,
            Id: itemId,
            Url: itemUrl
        });
    });

    $("#DocumentsGrid").kendoGrid(
    {
        columns: GridColumns,
        dataSource: {
            data: GridDataSource
        }
    });

    $("#DocumentsPageBusyIndicator").hide();
}

function BuildRoutingLegs(shipment, TenantDateTimeFormat) {

    var RoutingLegs = [];
    
    if (shipment.ShipmentPickUps.length > 0) {

        $.each(shipment.ShipmentPickUps, function (index, value) {
            
            var leg = new LogitudeRoutingClass();
            leg.LegHeader = "Pick Up: " + value.PickUpDeliveryNumber;
            leg.MasterVisibility = "collapse";
            leg.FromFlagSRC = "../images/Flags/" + ($.trim(value.FromPortCountryCode) != '' ? $.trim(value.FromPortCountryCode) : $.trim(value.FromAddressCountryCode)) + ".png";

            if (value.PickUpDeliveryFromTypeCode == "PART") {
                leg.FromPortCode = $.trim(value.FromAddressCity_Dummy);
            }

            else {
                leg.FromPortCode = $.trim(value.FromPortCode) != '' ? $.trim(value.FromPortCode) : $.trim(value.FromAddressCity);
            }

            leg.FromPortName = $.trim(value.FromPortName) != '' ? $.trim(value.FromPortName) : $.trim(value.FromAddressCountryName);
            leg.FromDate = $.trim(value.ATD) != "" ? $.Convert.ToShortDate(value.ATD, TenantDateTimeFormat) : $.Convert.ToShortDate(value.ETD, TenantDateTimeFormat);
            leg.FromTime = $.trim(value.ATD) != "" ? $.Convert.ToShortTime(value.ATD) : $.Convert.ToShortTime(value.ETD);
            leg.FromDateTimeIsActual = $.trim(value.ATD) != "";
            leg.FromDateTimeVisibility = ($.trim(value.ETD) != "" || $.trim(value.ATD) != "") ? "visible" : "collapse";
            leg.ToFlagSRC = "../images/Flags/" + ($.trim(value.ToPortCountryCode) != '' ? $.trim(value.ToPortCountryCode) : $.trim(value.ToAddressCountryCode)) + ".png";

            if (value.PickUpDeliveryToTypeCode == "PART") {
                leg.ToPortCode = $.trim(value.ToAddressCity_Dummy);
            }

            else {
                leg.ToPortCode = $.trim(value.ToPortCode) != '' ? $.trim(value.ToPortCode) : $.trim(value.ToAddressCity);
            }

            leg.ToPortName = $.trim(value.ToPortName) != '' ? $.trim(value.ToPortName) : $.trim(value.ToAddressCountryName);
            leg.ToDate = $.trim(value.ATA) != "" ? $.Convert.ToShortDate(value.ATA, TenantDateTimeFormat) : $.Convert.ToShortDate(value.ETA, TenantDateTimeFormat);
            leg.ToTime = $.trim(value.ATA) != "" ? $.Convert.ToShortTime(value.ATA) : $.Convert.ToShortTime(value.ETA);
            leg.ToDateTimeIsActual = $.trim(value.ATA) != "";
            leg.ToDateTimeVisibility = ($.trim(value.ETA) != "" || $.trim(value.ATA) != "") ? "visible" : "collapse";
            leg.Carrier = $.trim($.trim(value.CarrierCode) + " " + $.trim(value.CarrierName));
            leg.CarrierNumber = $.trim(value.CarrierNumber);

            if (!shipment.IsSharedLogisticsPickDelvCarrierVisible) {
                leg.CarrierVisibility = "collapse";
            }
            else {
                leg.CarrierVisibility = "visible";   
            }
                        
            leg.CarrierWebSite = $.trim(value.CarrierWebSite) == "" ? "" : ($.trim(value.CarrierWebSite).indexOf("http://") == -1 ? "http://" + $.trim(value.CarrierWebSite) : $.trim(value.CarrierWebSite));
            leg.CarrierHasWebSite = $.trim(value.CarrierWebSite) != "";
            leg.RoutingImageSRC = "../HtmlHelpers/images/Icons/Routing.I.png";

            if ($.trim(leg.FromDate) == "") {
                leg.FromDate = "No Date";
                leg.FromDateColor = "#6E7172";
            }

            if ($.trim(leg.ToDate) == "") {
                leg.ToDate = "No Date";
                leg.ToDateColor = "#6E7172";
            }

            RoutingLegs.push(leg);
        });
    }

    /* Pre Carriage */
    if ($.trim(shipment.PreCarriageFromPortId) != '' && $.trim(shipment.PreCarriageToPortId) != '') {

        var leg = new LogitudeRoutingClass();
        leg.LegHeader = "Pre Carriage";
        leg.MasterVisibility = "collapse";
        leg.FromFlagSRC = "../images/Flags/" + shipment.PreCarriageFromPortCountryCode + ".png";
        leg.FromPortCode = $.trim(shipment.PreCarriageFromPortCode);
        leg.FromPortName = $.trim(shipment.PreCarriageFromPortName);
        leg.FromDate = $.trim(shipment.PreCarriageATD) != "" ? $.Convert.ToShortDate(shipment.PreCarriageATD, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.PreCarriageETD, TenantDateTimeFormat);
        leg.FromTime = $.trim(shipment.PreCarriageATD) != "" ? $.Convert.ToShortTime(shipment.PreCarriageATD) : $.Convert.ToShortTime(shipment.PreCarriageETD);
        leg.FromDateTimeIsActual = $.trim(shipment.PreCarriageATD) != "";
        leg.ToFlagSRC = "../images/Flags/" + shipment.PreCarriageToPortCountryCode + ".png";
        leg.ToPortCode = $.trim(shipment.PreCarriageToPortCode);
        leg.ToPortName = $.trim(shipment.PreCarriageToPortName);
        leg.ToDate = $.trim(shipment.PreCarriageATA) != "" ? $.Convert.ToShortDate(shipment.PreCarriageATA, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.PreCarriageETA, TenantDateTimeFormat);
        leg.ToTime = $.trim(shipment.PreCarriageATA) != "" ? $.Convert.ToShortTime(shipment.PreCarriageATA) : $.Convert.ToShortTime(shipment.PreCarriageETA);
        leg.ToDateTimeIsActual = $.trim(shipment.PreCarriageATA) != "";
        leg.Carrier = $.trim($.trim(shipment.PreCarriageCarrierCode) + " " + $.trim(shipment.PreCarriageCarrierName));
        leg.CarrierNumber = $.trim(shipment.PreCarriageCarrierNumber);
        leg.CarrierWebSite = $.trim(shipment.PreCarriageCarrierWebSite) == "" ? "" : ($.trim(shipment.PreCarriageCarrierWebSite).indexOf("http://") == -1 ? "http://" + $.trim(shipment.PreCarriageCarrierWebSite) : $.trim(shipment.PreCarriageCarrierWebSite));
        leg.CarrierHasWebSite = $.trim(shipment.PreCarriageCarrierWebSite) != "";        
        leg.FromDateTimeVisibility = ($.trim(shipment.PreCarriageETD) != "" || $.trim(shipment.PreCarriageATD) != "") ? "visible" : "collapse";
        leg.ToDateTimeVisibility = ($.trim(shipment.PreCarriageETA) != "" || $.trim(shipment.PreCarriageATA) != "") ? "visible" : "collapse";
        leg.Vissel = $.trim(shipment.PreCarriageVesselName);
        leg.VisselVisibility = $.trim(shipment.PreCarriageTransportModeId) == "O" ? "visible" : "collapse";
        leg.RoutingImageSRC = "../HtmlHelpers/images/Icons/Routing." + shipment.PreCarriageTransportModeId + ".png";

        if ($.trim(leg.FromDate) == "") {
            leg.FromDate = "No Date";
            leg.FromDateColor = "#6E7172";
        }

        if ($.trim(leg.ToDate) == "") {
            leg.ToDate = "No Date";
            leg.ToDateColor = "#6E7172";
        }

        RoutingLegs.push(leg);
    }

    /* Main Carriage */
    var leg = new LogitudeRoutingClass();
    leg.LegHeader = "Main Carriage Leg 1";
    leg.FromFlagSRC = "../images/Flags/" + shipment.MainCarriageFromPortCountryCode + ".png";
    leg.FromPortCode = $.trim(shipment.MainCarriageFromPortCode);
    leg.FromPortName = $.trim(shipment.MainCarriageFromPortName);
    leg.FromDate = $.trim(shipment.MainCarriageATD) != "" ? $.Convert.ToShortDate(shipment.MainCarriageATD, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.MainCarriageETD, TenantDateTimeFormat);
    leg.FromTime = $.trim(shipment.MainCarriageATD) != "" ? $.Convert.ToShortTime(shipment.MainCarriageATD) : $.Convert.ToShortTime(shipment.MainCarriageETD);
    leg.FromDateTimeIsActual = $.trim(shipment.MainCarriageATD) != "";
    leg.ToFlagSRC = "../images/Flags/" + shipment.MainCarriageToPortCountryCode + ".png";
    leg.ToPortCode = $.trim(shipment.MainCarriageToPortCode);
    leg.ToPortName = $.trim(shipment.MainCarriageToPortName);
    leg.ToDate = $.trim(shipment.MainCarriageATA) != "" ? $.Convert.ToShortDate(shipment.MainCarriageATA, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.MainCarriageETA, TenantDateTimeFormat);
    leg.ToTime = $.trim(shipment.MainCarriageATA) != "" ? $.Convert.ToShortTime(shipment.MainCarriageATA) : $.Convert.ToShortTime(shipment.MainCarriageETA);
    leg.ToDateTimeIsActual = $.trim(shipment.MainCarriageATA) != "";
    leg.Carrier = $.trim($.trim(shipment.MainCarriageCarrierCode) + " " + $.trim(shipment.MainCarriageCarrierName));
    leg.CarrierNumber = $.trim(shipment.MainCarriageCarrierNumber);

    if(!shipment.IsSharedLogisticsMainCarrierVisible) {
        leg.CarrierVisibility = "collapse";         
    }
    else {
        leg.CarrierVisibility = "visible";
    }
        
    leg.CarrierWebSite = $.trim(shipment.MainCarriageCarrierWebSite) == "" ? "" : ($.trim(shipment.MainCarriageCarrierWebSite).indexOf("http://") == -1 ? "http://" + $.trim(shipment.MainCarriageCarrierWebSite) : $.trim(shipment.MainCarriageCarrierWebSite));
    leg.CarrierHasWebSite = $.trim(shipment.MainCarriageCarrierWebSite) != "";
    leg.Master = $.trim(shipment.LongMaster);
    leg.FromDateTimeVisibility = ($.trim(shipment.MainCarriageETD) != "" || $.trim(shipment.MainCarriageATD) != "") ? "visible" : "collapse";
    leg.ToDateTimeVisibility = ($.trim(shipment.MainCarriageETA) != "" || $.trim(shipment.MainCarriageATA) != "") ? "visible" : "collapse";
    leg.Vissel = $.trim(shipment.MainCarriageVesselName);
    leg.VisselVisibility = $.trim(shipment.TransportModeId) == "O" ? "visible" : "collapse";
    leg.RoutingImageSRC = "../HtmlHelpers/images/Icons/Routing." + shipment.TransportModeId + ".png";

    if ($.trim(leg.FromDate) == "") {
        leg.FromDate = "No Date";
        leg.FromDateColor = "#6E7172";
    }

    if ($.trim(leg.ToDate) == "") {
        leg.ToDate = "No Date";
        leg.ToDateColor = "#6E7172";
    }

    RoutingLegs.push(leg);

    /* Transshipment1 */
    if ($.trim(shipment.Transshipment1FromPortId) != '' && $.trim(shipment.Transshipment1ToPortId) != '') {

        var leg = new LogitudeRoutingClass();
        leg.LegHeader = "Main Carriage Leg 2";
        leg.FromFlagSRC = "../images/Flags/" + shipment.Transshipment1FromPortCountryCode + ".png";
        leg.FromPortCode = $.trim(shipment.Transshipment1FromPortCode);
        leg.FromPortName = $.trim(shipment.Transshipment1FromPortName);
        leg.FromDate = $.trim(shipment.Transshipment1ATD) != "" ? $.Convert.ToShortDate(shipment.Transshipment1ATD, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.Transshipment1ETD, TenantDateTimeFormat);
        leg.FromTime = $.trim(shipment.Transshipment1ATD) != "" ? $.Convert.ToShortTime(shipment.Transshipment1ATD) : $.Convert.ToShortTime(shipment.Transshipment1ETD);
        leg.FromDateTimeIsActual = $.trim(shipment.Transshipment1ATD) != "";
        leg.ToFlagSRC = "../images/Flags/" + shipment.Transshipment1ToPortCountryCode + ".png";
        leg.ToPortCode = $.trim(shipment.Transshipment1ToPortCode);
        leg.ToPortName = $.trim(shipment.Transshipment1ToPortName);
        leg.ToDate = $.trim(shipment.Transshipment1ATA) != "" ? $.Convert.ToShortDate(shipment.Transshipment1ATA, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.Transshipment1ETA, TenantDateTimeFormat);
        leg.ToTime = $.trim(shipment.Transshipment1ATA) != "" ? $.Convert.ToShortTime(shipment.Transshipment1ATA) : $.Convert.ToShortTime(shipment.Transshipment1ETA);
        leg.ToDateTimeIsActual = $.trim(shipment.Transshipment1ATA) != "";
        leg.Carrier = $.trim($.trim(shipment.Transshipment1CarrierCode) + " " + $.trim(shipment.Transshipment1CarrierName));
        leg.CarrierNumber = $.trim(shipment.Transshipment1CarrierNumber);
        leg.Master = $.trim(shipment.Transshipment1AdditionalMAWBOBLBL);
        leg.FromDateTimeVisibility = ($.trim(shipment.Transshipment1ETD) != "" || $.trim(shipment.Transshipment1ATD) != "") ? "visible" : "collapse";
        leg.ToDateTimeVisibility = ($.trim(shipment.Transshipment1ETA) != "" || $.trim(shipment.Transshipment1ATA) != "") ? "visible" : "collapse";
        leg.Vissel = $.trim(shipment.Transshipment1VesselName);
        leg.VisselVisibility = $.trim(shipment.TransportModeId) == "O" ? "visible" : "collapse";
        leg.RoutingImageSRC = "../HtmlHelpers/images/Icons/Routing." + shipment.TransportModeId + ".png";

        if (!shipment.IsSharedLogisticsMainCarrierVisible) {
            leg.CarrierVisibility = "collapse";
        }
        else {
            leg.CarrierVisibility = "visible";
        }

        if ($.trim(leg.FromDate) == "") {
            leg.FromDate = "No Date";
            leg.FromDateColor = "#6E7172";
        }

        if ($.trim(leg.ToDate) == "") {
            leg.ToDate = "No Date";
            leg.ToDateColor = "#6E7172";
        }

        RoutingLegs.push(leg);
    }


    /* Transshipment2 */
    if ($.trim(shipment.Transshipment2FromPortId) != '' && $.trim(shipment.Transshipment2ToPortId) != '') {

        var leg = new LogitudeRoutingClass();
        leg.LegHeader = "Main Carriage Leg 3";
        leg.FromFlagSRC = "../images/Flags/" + shipment.Transshipment2FromPortCountryCode + ".png";
        leg.FromPortCode = $.trim(shipment.Transshipment2FromPortCode);
        leg.FromPortName = $.trim(shipment.Transshipment2FromPortName);
        leg.FromDate = $.trim(shipment.Transshipment2ATD) != "" ? $.Convert.ToShortDate(shipment.Transshipment2ATD, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.Transshipment2ETD, TenantDateTimeFormat);
        leg.FromTime = $.trim(shipment.Transshipment2ATD) != "" ? $.Convert.ToShortTime(shipment.Transshipment2ATD) : $.Convert.ToShortTime(shipment.Transshipment2ETD);
        leg.FromDateTimeIsActual = $.trim(shipment.Transshipment2ATD) != "";
        leg.ToFlagSRC = "../images/Flags/" + shipment.Transshipment2ToPortCountryCode + ".png";
        leg.ToPortCode = $.trim(shipment.Transshipment2ToPortCode);
        leg.ToPortName = $.trim(shipment.Transshipment2ToPortName);
        leg.ToDate = $.trim(shipment.Transshipment2ATA) != "" ? $.Convert.ToShortDate(shipment.Transshipment2ATA, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.Transshipment2ETA, TenantDateTimeFormat);
        leg.ToTime = $.trim(shipment.Transshipment2ATA) != "" ? $.Convert.ToShortTime(shipment.Transshipment2ATA) : $.Convert.ToShortTime(shipment.Transshipment2ETA);
        leg.ToDateTimeIsActual = $.trim(shipment.Transshipment2ATA) != "";
        leg.Carrier = $.trim($.trim(shipment.Transshipment2CarrierCode) + " " + $.trim(shipment.Transshipment2CarrierName));
        leg.CarrierNumber = $.trim(shipment.Transshipment2CarrierNumber);
        leg.Master = $.trim(shipment.Transshipment2AdditionalMAWBOBLBL);
        leg.FromDateTimeVisibility = ($.trim(shipment.Transshipment2ETD) != "" || $.trim(shipment.Transshipment2ATD) != "") ? "visible" : "collapse";
        leg.ToDateTimeVisibility = ($.trim(shipment.Transshipment2ETA) != "" || $.trim(shipment.Transshipment2ATA) != "") ? "visible" : "collapse";
        leg.Vissel = $.trim(shipment.Transshipment2VesselName);
        leg.VisselVisibility = $.trim(shipment.TransportModeId) == "O" ? "visible" : "collapse";
        leg.RoutingImageSRC = "../HtmlHelpers/images/Icons/Routing." + shipment.TransportModeId + ".png";

        if (!shipment.IsSharedLogisticsMainCarrierVisible) {
            leg.CarrierVisibility = "collapse";
        }
        else {
            leg.CarrierVisibility = "visible";
        }

        if ($.trim(leg.FromDate) == "") {
            leg.FromDate = "No Date";
            leg.FromDateColor = "#6E7172";
        }

        if ($.trim(leg.ToDate) == "") {
            leg.ToDate = "No Date";
            leg.ToDateColor = "#6E7172";
        }

        RoutingLegs.push(leg);
    }


    /* Transshipment3 */
    if ($.trim(shipment.Transshipment3FromPortId) != '' && $.trim(shipment.Transshipment3ToPortId) != '') {

        var leg = new LogitudeRoutingClass();
        leg.LegHeader = "Main Carriage Leg 4";
        leg.FromFlagSRC = "../images/Flags/" + shipment.Transshipment3FromPortCountryCode + ".png";
        leg.FromPortCode = $.trim(shipment.Transshipment3FromPortCode);
        leg.FromPortName = $.trim(shipment.Transshipment3FromPortName);
        leg.FromDate = $.trim(shipment.Transshipment3ATD) != "" ? $.Convert.ToShortDate(shipment.Transshipment3ATD, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.Transshipment3ETD, TenantDateTimeFormat);
        leg.FromTime = $.trim(shipment.Transshipment3ATD) != "" ? $.Convert.ToShortTime(shipment.Transshipment3ATD) : $.Convert.ToShortTime(shipment.Transshipment3ETD);
        leg.FromDateTimeIsActual = $.trim(shipment.Transshipment3ATD) != "";
        leg.ToFlagSRC = "../images/Flags/" + shipment.Transshipment3ToPortCountryCode + ".png";
        leg.ToPortCode = $.trim(shipment.Transshipment3ToPortCode);
        leg.ToPortName = $.trim(shipment.Transshipment3ToPortName);
        leg.ToDate = $.trim(shipment.Transshipment3ATA) != "" ? $.Convert.ToShortDate(shipment.Transshipment3ATA, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.Transshipment3ETA, TenantDateTimeFormat);
        leg.ToTime = $.trim(shipment.Transshipment3ATA) != "" ? $.Convert.ToShortTime(shipment.Transshipment3ATA) : $.Convert.ToShortTime(shipment.Transshipment3ETA);
        leg.ToDateTimeIsActual = $.trim(shipment.Transshipment3ATA) != "";
        leg.Carrier = $.trim($.trim(shipment.Transshipment3CarrierCode) + " " + $.trim(shipment.Transshipment3CarrierName));
        leg.CarrierNumber = $.trim(shipment.Transshipment3CarrierNumber);
        leg.Master = $.trim(shipment.Transshipment3AdditionalMAWBOBLBL);
        leg.FromDateTimeVisibility = ($.trim(shipment.Transshipment3ETD) != "" || $.trim(shipment.Transshipment3ATD) != "") ? "visible" : "collapse";
        leg.ToDateTimeVisibility = ($.trim(shipment.Transshipment3ETA) != "" || $.trim(shipment.Transshipment3ATA) != "") ? "visible" : "collapse";
        leg.Vissel = $.trim(shipment.Transshipment3VesselName);
        leg.VisselVisibility = $.trim(shipment.TransportModeId) == "O" ? "visible" : "collapse";
        leg.RoutingImageSRC = "../HtmlHelpers/images/Icons/Routing." + shipment.TransportModeId + ".png";

        if (!shipment.IsSharedLogisticsMainCarrierVisible) {
            leg.CarrierVisibility = "collapse";
        }
        else {
            leg.CarrierVisibility = "visible";
        }

        if ($.trim(leg.FromDate) == "") {
            leg.FromDate = "No Date";
            leg.FromDateColor = "#6E7172";
        }

        if ($.trim(leg.ToDate) == "") {
            leg.ToDate = "No Date";
            leg.ToDateColor = "#6E7172";
        }

        RoutingLegs.push(leg);
    }

    /* On Carriage */
    if ($.trim(shipment.OnCarriageFromPortId) != '' && $.trim(shipment.OnCarriageToPortId) != '') {

        var leg = new LogitudeRoutingClass();
        leg.LegHeader = "On Carriage";
        leg.MasterVisibility = "collapse";
        leg.FromFlagSRC = "../images/Flags/" + shipment.OnCarriageFromPortCountryCode + ".png";
        leg.FromPortCode = $.trim(shipment.OnCarriageFromPortCode);
        leg.FromPortName = $.trim(shipment.OnCarriageFromPortName);
        leg.FromDate = $.trim(shipment.OnCarriageATD) != "" ? $.Convert.ToShortDate(shipment.OnCarriageATD, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.OnCarriageETD, TenantDateTimeFormat);
        leg.FromTime = $.trim(shipment.OnCarriageATD) != "" ? $.Convert.ToShortTime(shipment.OnCarriageATD) : $.Convert.ToShortTime(shipment.OnCarriageETD);
        leg.FromDateTimeIsActual = $.trim(shipment.OnCarriageATD) != "";
        leg.ToFlagSRC = "../images/Flags/" + shipment.OnCarriageToPortCountryCode + ".png";
        leg.ToPortCode = $.trim(shipment.OnCarriageToPortCode);
        leg.ToPortName = $.trim(shipment.OnCarriageToPortName);
        leg.ToDate = $.trim(shipment.OnCarriageATA) != "" ? $.Convert.ToShortDate(shipment.OnCarriageATA, TenantDateTimeFormat) : $.Convert.ToShortDate(shipment.OnCarriageETA, TenantDateTimeFormat);
        leg.ToTime = $.trim(shipment.OnCarriageATA) != "" ? $.Convert.ToShortTime(shipment.OnCarriageATA) : $.Convert.ToShortTime(shipment.OnCarriageETA);
        leg.ToDateTimeIsActual = $.trim(shipment.OnCarriageATA) != "";
        leg.Carrier = $.trim($.trim(shipment.OnCarriageCarrierCode) + " " + $.trim(shipment.OnCarriageCarrierName));
        leg.CarrierNumber = $.trim(shipment.OnCarriageCarrierNumber);
        leg.FromDateTimeVisibility = ($.trim(shipment.OnCarriageETD) != "" || $.trim(shipment.OnCarriageATD) != "") ? "visible" : "collapse";
        leg.ToDateTimeVisibility = ($.trim(shipment.OnCarriageETA) != "" || $.trim(shipment.OnCarriageATA) != "") ? "visible" : "collapse";
        leg.Vissel = $.trim(shipment.OnCarriageVesselName);
        leg.VisselVisibility = $.trim(shipment.OnCarriageTransportModeId) == "O" ? "visible" : "collapse";
        leg.RoutingImageSRC = "../HtmlHelpers/images/Icons/Routing." + shipment.OnCarriageTransportModeId + ".png";

        if ($.trim(leg.FromDate) == "") {
            leg.FromDate = "No Date";
            leg.FromDateColor = "#6E7172";
        }

        if ($.trim(leg.ToDate) == "") {
            leg.ToDate = "No Date";
            leg.ToDateColor = "#6E7172";
        }

        RoutingLegs.push(leg);
    }

    if (shipment.ShipmentDeliveries.length > 0) {
        $.each(shipment.ShipmentDeliveries, function (index, value) {

            var leg = new LogitudeRoutingClass();
            leg.LegHeader = "Delivery: " + value.PickUpDeliveryNumber;
            leg.MasterVisibility = "collapse";
            leg.FromFlagSRC = "../images/Flags/" + ($.trim(value.FromPortCountryCode) != '' ? $.trim(value.FromPortCountryCode) : $.trim(value.FromAddressCountryCode)) + ".png";

            if (value.PickUpDeliveryFromTypeCode == "PART") {
                leg.FromPortCode = $.trim(value.FromAddressCity_Dummy);
            }

            else {
                leg.FromPortCode = $.trim(value.FromPortCode) != '' ? $.trim(value.FromPortCode) : $.trim(value.FromAddressCity);
            }

            leg.FromPortName = $.trim(value.FromPortName) != '' ? $.trim(value.FromPortName) : $.trim(value.FromAddressCountryName);
            leg.FromDate = $.trim(value.ATD) != "" ? $.Convert.ToShortDate(value.ATD, TenantDateTimeFormat) : $.Convert.ToShortDate(value.ETD, TenantDateTimeFormat);
            leg.FromTime = $.trim(value.ATD) != "" ? $.Convert.ToShortTime(value.ATD) : $.Convert.ToShortTime(value.ETD);
            leg.FromDateTimeIsActual = $.trim(value.ATD) != "";
            leg.FromDateTimeVisibility = ($.trim(value.ETD) != "" || $.trim(value.ATD) != "") ? "visible" : "collapse";
            leg.ToFlagSRC = "../images/Flags/" + ($.trim(value.ToPortCountryCode) != '' ? $.trim(value.ToPortCountryCode) : $.trim(value.ToAddressCountryCode)) + ".png";

            if (value.PickUpDeliveryToTypeCode == "PART") {
                leg.ToPortCode = $.trim(value.ToAddressCity_Dummy);
            }

            else {
                leg.ToPortCode = $.trim(value.ToPortCode) != '' ? $.trim(value.ToPortCode) : $.trim(value.ToAddressCity);
            }

            leg.ToPortName = $.trim(value.ToPortName) != '' ? $.trim(value.ToPortName) : $.trim(value.ToAddressCountryName);
            leg.ToDate = $.trim(value.ATA) != "" ? $.Convert.ToShortDate(value.ATA, TenantDateTimeFormat) : $.Convert.ToShortDate(value.ETA, TenantDateTimeFormat);
            leg.ToTime = $.trim(value.ATA) != "" ? $.Convert.ToShortTime(value.ATA) : $.Convert.ToShortTime(value.ETA);
            leg.ToDateTimeIsActual = $.trim(value.ATA) != "";
            leg.ToDateTimeVisibility = ($.trim(value.ETA) != "" || $.trim(value.ATA) != "") ? "visible" : "collapse";
            leg.Carrier = $.trim($.trim(value.CarrierCode) + " " + $.trim(value.CarrierName));
            leg.CarrierNumber = $.trim(value.CarrierNumber);

            if (!shipment.IsSharedLogisticsPickDelvCarrierVisible) {
                leg.CarrierVisibility = "collapse";                  
            }
            else {
                leg.CarrierVisibility = "visible";
            }
            
            leg.CarrierWebSite = $.trim(value.CarrierWebSite) == "" ? "" : ($.trim(value.CarrierWebSite).indexOf("http://") == -1 ? "http://" + $.trim(value.CarrierWebSite) : $.trim(value.CarrierWebSite));
            leg.CarrierHasWebSite = $.trim(value.CarrierWebSite) != "";
            leg.RoutingImageSRC = "../HtmlHelpers/images/Icons/Routing.I.png";

            if ($.trim(leg.FromDate) == "") {
                leg.FromDate = "No Date";
                leg.FromDateColor = "#6E7172";
            }

            if ($.trim(leg.ToDate) == "") {
                leg.ToDate = "No Date";
                leg.ToDateColor = "#6E7172";
            }

            RoutingLegs.push(leg);
        });
    }

    $("#RoutingsListBox").kendoListView(
    {
        dataSource: { data: RoutingLegs },
        template: kendo.template($("#RoutingListBoxItemDataTemplate").html())
    });

    $("#RoutingsPageBusyIndicator").hide();

    //return RoutingLegs;
}

function BuildPackagesTabPageViewModel(shipment) {
    
    var DimensionsHeader = $.trim(shipment.DimensionsUnitCode) != '' ? "Dimensions (L-W-H)" + " (" + shipment.DimensionsUnitCode + ")" : "Dimensions (L-W-H)";
    var VolumeTitle = $.trim(shipment.VolumeUnitCode) != '' ? "Volume" + " (" + shipment.VolumeUnitCode + ")" : "Volume";
    var GrossTitle = $.trim(shipment.GrossWeightUnitCode) != '' ? "Gross Weight" + " (" + shipment.GrossWeightUnitCode + ")" : "Gross Weight";
    //var VolumetricTitle = $.trim(shipment.ChargeableWeightUnitCode) != '' ? "Volumetric Weight" + " (" + shipment.ChargeableWeightUnitCode + ")" : "Volumetric Weight";
    var ChargeableTitle = shipment.TransportModeId == "A" ? "Chargeable Weight" : "Wt / Msr";
    if ($.trim(shipment.ChargeableWeightUnitCode) != '') {
        ChargeableTitle = ChargeableTitle + " (" + shipment.ChargeableWeightUnitCode + ")";
    }

    var PackagesGridColumns = [];
    var PackagesGridDataSource = [];

    if (shipment.ShipmentPackages.length > 0) {

        if (shipment.TransportModeId == "A") {

            PackagesGridColumns.push({ title: "Packages", field: "Quantity", width: 70, template: "<div class='k-numeric'>#= Quantity #</div>" });
            PackagesGridColumns.push({ title: DimensionsHeader, field: "Dimensions" });
            PackagesGridColumns.push({ title: VolumeTitle, field: "Volume", width: "100px", template: "<div class='k-numeric'>#= Volume #</div>" });
            //PackagesGridColumns.push({ title: VolumetricTitle, field: "VolumetricWeight", width: "150px", template: "<div class='k-numeric'>#= VolumetricWeight #</div>" });
            PackagesGridColumns.push({ title: GrossTitle, field: "GrossWeight", width: "120px", template: "<div class='k-numeric'>#= GrossWeight #</div>" });

            $.each(shipment.ShipmentPackages, function (index, item) {

                var length = $.trim(item.Length) == "" ? "" : item.Length;
                var width = $.trim(item.Width) == "" ? "" : item.Width;
                var height = $.trim(item.Height) == "" ? "" : item.Height;

                var itemDimensions = length + "-" + width + "-" + height;
                var itemVolume = $.trim(item.Volume) == "" ? 0 : item.Volume;
                //var itemVolumetricWeight = $.trim(item.VolumetricWeight) == "" ? 0 : item.VolumetricWeight;
                var itemGrossWeight = $.trim(item.Weight) == "" ? 0 : item.Weight;

                PackagesGridDataSource.push({
                    Quantity: item.Quantity,
                    Dimensions: itemDimensions,
                    Volume: itemVolume.toFixed(3),
                    //VolumetricWeight: itemVolumetricWeight.toFixed(3),
                    GrossWeight: itemGrossWeight.toFixed(3)
                });

            });
        }

        else {

            var imgTemplate = "";
            imgTemplate += "<div style='width:20px; height:20px; vertical-align:middle; margin-left: -5px; position: relative;'>";
            imgTemplate += "<img src='../images/icons/infoICON.png' style='width:20px; height:20px; vertical-align:middle; visibility: #= DescriptionIconVisibility #;' onmouseover='OnMouseOverPackageDescriptionIcon(this)' onmouseleave='OnMouseLeavePackageDescriptionIcon(this)' />";
            imgTemplate += "<div style='width: 270px; height: 130px; margin-top: -75px; position: fixed; right: 60px; background: url(\"../images/icons/CellTooltip.png\") no-repeat; background-size: 100% 100%; visibility: #= DescriptionHelpVisibility #;'>";
            imgTemplate += "<div style='color: \\#1B90CB; height: 13px; font-size: 13px; line-height: 13px; margin-left: 10px; margin-top: 13px;'>Description</div>";
            imgTemplate += "<textarea style='width: 225px; height: 85px; margin-left: 10px; margin-top: 0px; line-height: 11px; background: transparent; font-size: 11px; resize: none; border: none !important; outline: none !important; -webkit-box-shadow: none; -moz-box-shadow: none; box-shadow: none;' [readonly]='true' autocomplete='off' autocorrect='off' autocapitalize='off' spellcheck='false'>#= Description #</textarea>";
            imgTemplate += "</div>";
            imgTemplate += "</div>";

            var imgCarTemplate = "";
            imgCarTemplate += "<div style='width:20px; height:20px; vertical-align:middle; margin-left: -5px; position: relative;'>";
            imgCarTemplate += "<img src='../images/icons/infoICON.png' style='width:20px; height:20px; vertical-align:middle; visibility: #= CarIconVisibility #;' onmouseover='OnMouseOverPackageCarIcon(this)' onmouseleave='OnMouseLeavePackageCarIcon(this)' />";
            imgCarTemplate += "<div style='width: 270px; height: 130px; margin-top: -75px; position: fixed; right: 60px; background: url(\"../images/icons/CellTooltip.png\") no-repeat; background-size: 100% 100%; visibility: #= CarHelpVisibility #;'>";
            imgCarTemplate += "<div style='color: \\#1B90CB; height: 13px; font-size: 13px; line-height: 13px; margin-left: 10px; margin-top: 13px;'>Vehicle Details</div>";
            imgCarTemplate += "<textarea style='width: 225px; height: 85px; margin-left: 10px; margin-top: 0px; line-height: 11px; background: transparent; font-size: 11px; resize: none; border: none !important; outline: none !important; -webkit-box-shadow: none; -moz-box-shadow: none; box-shadow: none;' [readonly]='true' autocomplete='off' autocorrect='off' autocapitalize='off' spellcheck='false'>#= Vehicle Details  #</textarea>";
            imgCarTemplate += "</div>";
            imgCarTemplate += "</div>";

            if (IsLCLShipment(shipment)) {
               

                if (shipment.Tenant == 1495) {
                    PackagesGridColumns.push({ title: "Package Type", field: "Type", width: "150px" });
                }

                else {
                    PackagesGridColumns.push({ title: "Package Type", field: "Type" });
                }

                PackagesGridColumns.push({ title: "Packages", field: "Quantity", width: 70, template: "<div class='k-numeric'>#= Quantity #</div>" });

                if (shipment.Tenant == 1495) {
                    PackagesGridColumns.push({ title: "Commodity Code", field: "CommodityCode", width: "120px" });
                    PackagesGridColumns.push({ title: "Commodity Name", field: "CommodityName" });
                }

                PackagesGridColumns.push({ title: "Container #", field: "ContainerNumber", width: "140px" });
                PackagesGridColumns.push({ title: VolumeTitle, field: "Volume", width: "100px", template: "<div class='k-numeric'>#= Volume #</div>" });
                //PackagesGridColumns.push({ title: VolumetricTitle, field: "VolumetricWeight", width: "150px", template: "<div class='k-numeric'>#= VolumetricWeight #</div>" });
                PackagesGridColumns.push({ title: GrossTitle, field: "GrossWeight", width: "120px", template: "<div class='k-numeric'>#= GrossWeight #</div>" });
                PackagesGridColumns.push({ title: "", width: "25px", template: imgTemplate });
                PackagesGridColumns.push({ title: "", width: "25px", template: imgCarTemplate });

                $.each(shipment.ShipmentPackages, function (index, item) {

                    var itemType = $.trim(item.PackageTypeName) == "" ? "" : item.PackageTypeName;
                    var itemQuantity = $.trim(item.Quantity) == "" ? 0 : item.Quantity;
                    var itemContainer = $.trim(item.ContainerNumber) == "" ? "" : item.ContainerNumber;
                    var itemVolume = $.trim(item.Volume) == "" ? 0 : item.Volume;
                    //var itemVolumetricWeight = $.trim(item.VolumetricWeight) == "" ? 0 : item.VolumetricWeight;
                    var itemGrossWeight = $.trim(item.Weight) == "" ? 0 : item.Weight;

                    var iCommodityCode = $.trim(item.CommodityNumber) == "" ? "" : item.CommodityNumber;
                    var iCommodityName = $.trim(item.CommodityName) == "" ? "" : item.CommodityName;

                    PackagesGridDataSource.push({
                        Type: itemType,
                        Quantity: itemQuantity,
                        ContainerNumber: itemContainer,
                        Volume: itemVolume.toFixed(3),
                        //VolumetricWeight: itemVolumetricWeight.toFixed(3),
                        GrossWeight: itemGrossWeight.toFixed(3),
                        CommodityCode: iCommodityCode,
                        CommodityName: iCommodityName,

                        Description: item.Description,
                        DescriptionIconVisibility: $.trim(item.Description) != "" ? "visible" : "collapse",
                        DescriptionHelpVisibility: "collapse",
                        showDescription: function (e) {
                            if (e == true)
                            {
                                this.set("DescriptionHelpVisibility", "visible");
                            }
                            else {
                                this.set("DescriptionHelpVisibility", "collapse");
                            }
                        },

                        CarIconVisibility: $.trim(item.Description) != "" ? "visible" : "collapse",
                        CarHelpVisibility: "collapse",
                        showCarsIcon: function (e) {
                            if (e == true) {
                                this.set("CarHelpVisibility", "visible");
                            }
                            else {
                                this.set("CarHelpVisibility", "collapse");
                            }
                        }

                    });
                });
            }

            else {
                
                PackagesGridColumns.push({ title: "Container Type", field: "Type" });
                PackagesGridColumns.push({ title: "Containers", field: "Quantity", width: 70, template: "<div class='k-numeric'>#= Quantity #</div>" });
                PackagesGridColumns.push({ title: "Container #", field: "ContainerNumber", width: "150px" });
                PackagesGridColumns.push({ title: "Number of inside packages", field: "NumberOfInsidePackages", width: "160px", template: "<div class='k-numeric'>#= NumberOfInsidePackages #</div>" });
                PackagesGridColumns.push({ title: "Seal", field: "Seal", width: "100px" });
                PackagesGridColumns.push({ title: VolumeTitle, field: "Volume", width: "100px", template: "<div class='k-numeric'>#= Volume #</div>" });
                //PackagesGridColumns.push({ title: VolumetricTitle, field: "VolumetricWeight", width: "150px", template: "<div class='k-numeric'>#= VolumetricWeight #</div>" });
                PackagesGridColumns.push({ title: GrossTitle, field: "GrossWeight", width: "120px", template: "<div class='k-numeric'>#= GrossWeight #</div>" });
                PackagesGridColumns.push({ title: "", width: "25px", template: imgTemplate });
                PackagesGridColumns.push({ title: "", width: "25px", template: imgCarTemplate });

                $.each(shipment.ShipmentPackages, function (index, item) {

                    var itemType = $.trim(item.PackageTypeName) == "" ? "" : item.PackageTypeName;
                    var itemQuantity = $.trim(item.Quantity) == "" ? 0 : item.Quantity;
                    var itemContainer = $.trim(item.ContainerNumber) == "" ? "" : item.ContainerNumber;
                    var itemNumberOfInsidePackages = $.trim(item.NumberOfInsidePackages) == "" ? 0 : item.NumberOfInsidePackages;                    
                    var itemSeal = $.trim(item.ShipperSeal) == "" ? "" : item.ShipperSeal;
                    var itemVolume = $.trim(item.Volume) == "" ? 0 : item.Volume;
                    //var itemVolumetricWeight = $.trim(item.VolumetricWeight) == "" ? 0 : item.VolumetricWeight;
                    var itemGrossWeight = $.trim(item.Weight) == "" ? 0 : item.Weight;

                    PackagesGridDataSource.push({
                        Type: itemType,
                        Quantity: itemQuantity,
                        ContainerNumber: itemContainer,
                        NumberOfInsidePackages: itemNumberOfInsidePackages,
                        Seal :itemSeal,
                        Volume: itemVolume.toFixed(3),
                        //VolumetricWeight: itemVolumetricWeight.toFixed(3),
                        GrossWeight: itemGrossWeight.toFixed(3),
                        Description: item.Description,
                        DescriptionIconVisibility: $.trim(item.Description) != "" ? "visible" : "collapse",
                        DescriptionHelpVisibility: "collapse",

                        showDescription: function (e) {
                            if (e == true) {
                                this.set("DescriptionHelpVisibility", "visible");
                            }

                            else {
                                this.set("DescriptionHelpVisibility", "collapse");
                            }
                        }
                    });
                });
            }
        }
    }

    else {        

        PackagesGridColumns.push({ title: "Packages", field: "Quantity", width: 70 });

        if (IsLCLShipment(shipment)) {
            PackagesGridColumns.push({ title: "Package Type", field: "PackageTypeName" });
        }

        else {
            PackagesGridColumns.push({ title: "Container Type", field: "PackageTypeName" });
        }

        $.each(shipment.ShipmentOrderPackages, function (index, item) {
            PackagesGridDataSource.push({
                Quantity: item.Quantity,
                PackageTypeName: item.PackageTypeName
            });
        });
    }
          
    $("#PackagesGrid").kendoGrid(
    {
        columns: PackagesGridColumns,
        dataSource: {
            data: PackagesGridDataSource
        }
    });
    
    // Summary
    this.GrossLabel = GrossTitle;
    this.VolumeLabel = VolumeTitle;
    //this.VolumetricLabel = VolumetricTitle;
    this.ChargeableLabel = ChargeableTitle;
    this.DescriptionOfGoods = $.trim(shipment.DescriptionOfGoods);

    var totalPieces = 0;
    var totalVolume = 0;
    var totalGrossWeight = 0;
    var totalVolumetricWeight = 0;
    var totalChargeableWeight = 0;

    if (shipment.ShipmentPackages.length > 0) {

        totalPieces = IsLCLShipment(shipment) ? ($.trim(shipment.NumberOfPackages) == "" ? 0 : shipment.NumberOfPackages) : ($.trim(shipment.NumberOfContainers) == "" ? 0 : shipment.NumberOfContainers);
        totalVolume = $.trim(shipment.Volume) == "" ? 0 : shipment.Volume;
        totalGrossWeight = $.trim(shipment.GrossWeight) == "" ? 0 : shipment.GrossWeight;
        //totalVolumetricWeight = $.trim(shipment.VolumetricWeight) == "" ? 0 : shipment.VolumetricWeight;
        totalChargeableWeight = $.trim(shipment.ChargeableWeight) == "" ? 0 : shipment.ChargeableWeight;
    }

    else {

        totalPieces = $.trim(shipment.BookingNumberOfPackages) == "" ? 0 : shipment.BookingNumberOfPackages;
        totalVolume = $.trim(shipment.BookingVolume) == "" ? 0 : shipment.BookingVolume;
        totalGrossWeight = $.trim(shipment.OrderGrossWeight) == "" ? 0 : shipment.OrderGrossWeight;
        //totalVolumetricWeight = $.trim(shipment.OrderVolumetricWeight) == "" ? 0 : shipment.OrderVolumetricWeight;
        totalChargeableWeight = $.trim(shipment.OrderChargeableWeight) == "" ? 0 : shipment.OrderChargeableWeight;

        if (shipment.ShipmentOrderPackages.length > 0) {

            $("#EstimatePackagesControl").show();
        }

        else {

            if (this.TotalPieces > 0 || this.TotalVolume > 0 || this.TotalGrossWeight > 0 || this.TotalVolumetricWeight > 0 || this.TotalChargeableWeight > 0) {

                $("#EstimatePackagesControl").show();
            }
        }
    }


    this.TotalPieces = totalPieces;
    this.TotalVolume = totalVolume.toFixed(3);
    this.TotalGrossWeight = totalGrossWeight.toFixed(3);
    //this.TotalVolumetricWeight = totalVolumetricWeight.toFixed(3);
    this.TotalChargeableWeight = totalChargeableWeight.toFixed(3);

    $("#PackagesPageBusyIndicator").hide();
}

function OnMouseOverPackageDescriptionIcon(sender) {
    var grid = $('#PackagesGrid').data('kendoGrid');

    if (grid) {
        var data = grid.dataItem($(sender).closest("tr"));
        if (data) {
            data.showDescription(true);
        }
    }
}
function OnMouseLeavePackageDescriptionIcon(sender) {
    var grid = $('#PackagesGrid').data('kendoGrid');

    if (grid) {
        var data = grid.dataItem($(sender).closest("tr"));
        if (data) {
            data.showDescription(false);
        }
    }
}

function OnMouseOverPackageCarIcon(sender) {
    var grid = $('#PackagesGrid').data('kendoGrid');

    if (grid) {
        var data = grid.dataItem($(sender).closest("tr"));
        if (data) {
            data.showCarsIcon(true);
        }
    }
}
function OnMouseLeavePackageCarIcon(sender) {
    var grid = $('#PackagesGrid').data('kendoGrid');

    if (grid) {
        var data = grid.dataItem($(sender).closest("tr"));
        if (data) {
            data.showCarsIcon(false);
        }
    }
}

function BuildEventList(traceEvents, TenantDateTimeFormat) {

    var Events = [];

    $.each(traceEvents, function (index, traceEvent) {

        var item = new EventClass();
        item.Name = $.trim(traceEvent.EventTypeEnglishName);
        item.Username = $.trim(traceEvent.ContactEnglishFirstName);
        item.Notes = $.trim(traceEvent.Notes);
        item.LogDate = $.trim(traceEvent.LogDateTime) == "" ? "" : $.Convert.ToShortDate(traceEvent.LogDateTime, TenantDateTimeFormat);
        item.LogTime = $.trim(traceEvent.LogDateTime) == "" ? "" : $.Convert.ToShortTime24(traceEvent.LogDateTime);
        item.EventDate = $.trim(traceEvent.EventDateTime) == "" ? "" : $.Convert.ToShortDate(traceEvent.EventDateTime, TenantDateTimeFormat);
        item.EventTime = $.trim(traceEvent.EventDateTime) == "" ? "" : $.Convert.ToShortTime24(traceEvent.EventDateTime);
        
        if ($.trim(traceEvent.EventTypeEnglishName) != "") {

            if ($.trim(traceEvent.Location) != "") {

                var myEventName = traceEvent.EventTypeEnglishName + " (" + traceEvent.Location + ")";
                item.Name = myEventName;
            }
        }

        if ($.trim(traceEvent.Notes) == "") {            
            item.DisplayEventNotesMobile = "none";
        }

        if (traceEvents.length == 0) {
            item.displayendline = "none";
        }
        
        item.LogDateTime = item.LogDate + ", " + item.LogTime;
        item.EventDateTime = item.EventDate + ", " + item.EventTime;
        Events.push(item);
    });

    return Events;
}

function BuildInvoicesList(invoices, TenantDateTimeFormat) {
    
    var InvoicesList = [];

    var todayDate = new Date();

    $.each(invoices, function (index, invoice) {

        var item = new InvoiceListClass();

        item.EntityId = invoice.Id;
        item.EntityNumber = $.trim(invoice.InvoiceNumber);
        item.OurRefNumber = $.trim(invoice.MainEntityReference);
        item.InvoiceDate = $.Convert.ToShortDate(invoice.InvoiceDate, TenantDateTimeFormat);
        item.DueDate = $.Convert.ToShortDate(invoice.DueDate, TenantDateTimeFormat);

        var amount = $.trim(invoice.AmountInInvoiceCurrency) == "" ? 0 : invoice.AmountInInvoiceCurrency;
        var amountDue = $.trim(invoice.AmountDue) == "" ? 0 : invoice.AmountDue;

        item.Amount = amount.toFixed(2) + " (" + invoice.InvoiceCurrencyCode + ")";
        item.AmountDue = amountDue.toFixed(2) + " (" + invoice.InvoiceCurrencyCode + ")";

        if (invoice.IsClosed == false) {

            if (invoice.DueDate < todayDate) {
                item.AmountDueColor = "#E53030";
            }
        }

        item.StatusName = invoice.StatusName;
        item.StatusColor = $.Convert.ToColor(invoice.StatusName);

        InvoicesList.push(item);
    });

    return InvoicesList;
}

function BuildCustomersList(entities, TenantDateTimeFormat) {

    var ResultList = [];

    $.each(entities, function (index, entityList) {

        var item = new CustomerListClass();

        item.EntityId = entityList.Id;
        item.RankName = $.trim(entityList.RankName);
        item.RankCode = $.trim(entityList.RankCode);
        item.BillToName = $.trim(entityList.BillToName);
        item.AccountManagerUserEnglishName = $.trim(entityList.AccountManagerUserEnglishName);
        item.SalesmanUserEnglishName = $.trim(entityList.SalesmanUserEnglishName);
        item.PaymentTermEnglishName = $.trim(entityList.PaymentTermEnglishName);
        item.Website = $.trim(entityList.Website);
        item.Code = $.trim(entityList.Code);
        item.EnglishName = $.trim(entityList.EnglishName);
        item.VatNumber = $.trim(entityList.VatNumber);
        item.LocalName = $.trim(entityList.LocalName);
        item.InActive = entityList.InActive;
        item.AccountingCard = $.trim(entityList.AccountingCard);
        item.Notes = $.trim(entityList.Notes);
        item.CreateDate = $.Convert.ToShortDate(entityList.CreateDate, TenantDateTimeFormat);
        item.StartWorkingDate = $.trim(entityList.StartWorkingDate);
        item.StartWorkingManuallySet = entityList.StartWorkingManuallySet;
        item.LastShipmentDate = $.trim(entityList.LastShipmentDate);
        item.Activity = $.trim(entityList.Activity);
        item.InvoicesDue = $.trim(entityList.InvoicesDue);
        item.CityName = $.trim(entityList.CityName);
        item.SharedLogisticsInvitationStatusName = $.trim(entityList.SharedLogisticsInvitationStatusName);
        item.LastLoginDate = $.trim(entityList.LastLoginDate);
        item.InvitationDate = $.trim(entityList.InvitationDate);

        ResultList.push(item);
    });

    return ResultList;
}

function BuildInvoiceBackAreaViewModel(invoice) {

    var viewModel =
        {
            EntityNumber: ko.observable(invoice.InvoiceNumber),
        };

    return viewModel;
}

function BuildInvoiceHeaderViewModel(invoice, TenantDateTimeFormat) {

    this.Reference = $.trim(invoice.MainEntityReference) == "" ? "" : invoice.MainEntityReference;
    this.Status = $.trim(invoice.StatusName) == "" ? "" : invoice.StatusName;
    this.StatusColor = $.Convert.ToColor(invoice.StatusName);

    var amountDue = $.trim(invoice.AmountDue) == "" ? 0 : invoice.AmountDue;
    this.AmountDue = amountDue.toFixed(2) + " (" + invoice.InvoiceCurrencyCode + ")";
    this.DueDate = $.Convert.ToShortDate(invoice.DueDate, TenantDateTimeFormat);

    this.PaymentTermName = $.trim(invoice.PaymentTermName) == "" ? "" : invoice.PaymentTermName;
    this.InvoiceDate = $.Convert.ToShortDate(invoice.InvoiceDate, TenantDateTimeFormat);
     
    this.ReportUrl = invoice.ReportUrl;
}

function BuildInvoiceSummaryViewModel(invoicePM) {

    this.PrintNotes = $.trim(invoicePM.PrintNotes) == "" ? "" : invoicePM.PrintNotes;
    this.InvoiceCurrencyCode = $.trim(invoicePM.InvoiceCurrencyCode) == "" ? "" : " (" + invoicePM.InvoiceCurrencyCode + ")";

    var invoiceAmount = $.trim(invoicePM.AmountInInvoiceCurrency) == "" ? 0 : invoicePM.AmountInInvoiceCurrency;
    var subTotals = $.trim(invoicePM.SubTotalInInvoiceCurrency) == "" ? 0 : invoicePM.SubTotalInInvoiceCurrency;

    this.InvoiceAmount = invoiceAmount.toFixed(2);
    this.SubTotals = subTotals.toFixed(2);





    var groupByData = [];
    var groubClass = function () {

        this.VatTypeId = "";
        this.VatTypeName = "";
        this.VatPercentage = "";
    }
       
    $.each(invoicePM.InvoiceLines, function (index, item) {
                       
        var obj = new groubClass();
        obj.VatTypeId = item.VatTypeId;
        obj.VatTypeName = item.VatTypeName
        obj.VatPercentage = item.VatPercentage;

        var contains = false;
        var i = groupByData.length;
        while (i--) {

            if (groupByData[i].VatTypeId == obj.VatTypeId && groupByData[i].VatTypeName == obj.VatTypeName && groupByData[i].VatPercentage == obj.VatPercentage) {
                contains = true;
                break;
            }
        }
        
        if (!contains) {
            groupByData.push(obj);
        }       
    });


    var PlusIndex = 0;
    var TargetId = "#SubTotal";
    $.each(groupByData, function (k, obj) {

        
        PlusIndex += 1;
        $(TargetId).after("<div class='SummaryItem' id='Plus" + PlusIndex + "'><div class='SummaryItemValue'>" + "+" + "</div><div class='SummaryItemLabel'></div></div>");
        TargetId = "#Plus" + PlusIndex;

        var itemLabel = obj.VatTypeName + " (" + obj.VatPercentage + "%)";
        var itemValue = 0;

        $.each(invoicePM.InvoiceLines, function (x, line) {

            if (line.VatTypeId == obj.VatTypeId && line.VatTypeName == obj.VatTypeName && line.VatPercentage == obj.VatPercentage) {

                itemValue += line.InvoiceCurrencyAmount * line.VatPercentage / 100;
            }
        });

        PlusIndex += 1;
        $(TargetId).after("<div class='SummaryItem' id='Plus" + PlusIndex + "'><div class='SummaryItemValue'>" + itemValue.toFixed(2) + "</div><div class='SummaryItemLabel'>" + itemLabel + "</div></div>");
        TargetId = "#Plus" + PlusIndex;
    });
}

function BuildPaymentsTabPageViewModel(invoice,payments) {

    if (payments.length == 0) {

        $("#PaymentsGrid").css({
            "font-family": "Arial",
            "color": "#8F9293",
            "font-size": "22px",
            "margin-top": "20px",
            "margin-left": "10px",
        });

        $("#PaymentsGrid").html("No Payments");
    }

    else {

        var GridColumns = [];
        var GridDataSource = [];

        GridColumns.push({ title: "Payment #", field: "PaymentNo" });
        GridColumns.push({ title: "Payment Type", field: "PaymentType", width: "120px" });
        GridColumns.push({ title: "Payment Ref", field: "PaymentRef", width: "120px" });
        GridColumns.push({ title: "Payment Amount", field: "PaymentAmount", width: "120px", template: "<div class='k-numeric'>#= PaymentAmount #</div>" });
        GridColumns.push({ title: "Amount Paid", field: "PaidAmount", width: "120px", template: "<div class='k-numeric'>#= PaidAmount #</div>" });
        GridColumns.push({ title: "Open Amount", field: "OpenAmount", width: "120px", template: "<div class='k-numeric'>#= OpenAmount #</div>" });
        //GridColumns.push({ title: "Status", field: "Status", width: "120px" });

        $.each(payments, function (index, item) {

            var itemNumber = $.trim(item.PaymentNo) == "" ? "" : item.PaymentNo;
            var itemType = $.trim(item.PaymentMethodName) == "" ? "" : item.PaymentMethodName;
            var itemRef = $.trim(item.ChequeOrPaymentRef) == "" ? "" : item.ChequeOrPaymentRef;
            var itemAmount = $.trim(item.AmountInPaymentCurrency) == "" ? 0 : item.AmountInPaymentCurrency;
            var itemOpenAmount = $.trim(item.OpenAmount) == "" ? 0 : item.OpenAmount;
            var itemPaidAmount = itemAmount - itemOpenAmount;

            GridDataSource.push({
                PaymentNo: item.PaymentNo,
                PaymentType: itemType,
                PaymentRef : itemRef,
                PaymentAmount: itemAmount.toFixed(2) + " " + item.PaymentCurrencyCode,
                OpenAmount: itemOpenAmount.toFixed(2),
                PaidAmount: itemPaidAmount.toFixed(2),
            });
        });

        $("#PaymentsGrid").kendoGrid(
        {
            columns: GridColumns,
            dataSource: {
                data: GridDataSource
            }
        });
    }

    var invoiceAmount = $.trim(invoice.AmountInInvoiceCurrency) == "" ? 0 : invoice.AmountInInvoiceCurrency;
    var amountDue = $.trim(invoice.AmountDue) == "" ? 0 : invoice.AmountDue;
    var paidAmount = invoiceAmount - amountDue;

    this.InvoiceAmount = invoiceAmount.toFixed(2);
    this.AmountDue = amountDue.toFixed(2);
    this.PaidAmount = paidAmount.toFixed(2);
    this.InvoiceCurrencyCode = $.trim(invoice.InvoiceCurrencyCode) == "" ? "" : " (" + invoice.InvoiceCurrencyCode + ")";
}

//customers
function BuildCustomerBackAreaViewModel(customer) {

    var viewModel =
        {
            EntityNumber: ko.observable(customer.Code),
        };

    return viewModel;
}

function BuildCustomerHeaderViewModel(customer) {

}

function BuildCustomerSummaryViewModel(invoicePM) {

    //this.PrintNotes = $.trim(invoicePM.PrintNotes) == "" ? "" : invoicePM.PrintNotes;
    //this.InvoiceCurrencyCode = $.trim(invoicePM.InvoiceCurrencyCode) == "" ? "" : " (" + invoicePM.InvoiceCurrencyCode + ")";

    //var invoiceAmount = $.trim(invoicePM.AmountInInvoiceCurrency) == "" ? 0 : invoicePM.AmountInInvoiceCurrency;
    //var subTotals = $.trim(invoicePM.SubTotalInInvoiceCurrency) == "" ? 0 : invoicePM.SubTotalInInvoiceCurrency;

    //this.InvoiceAmount = invoiceAmount.toFixed(2);
    //this.SubTotals = subTotals.toFixed(2);

    //var groupByData = [];
    //var groubClass = function () {

    //    this.VatTypeId = "";
    //    this.VatTypeName = "";
    //    this.VatPercentage = "";
    //}

    //$.each(invoicePM.InvoiceLines, function (index, item) {

    //    var obj = new groubClass();
    //    obj.VatTypeId = item.VatTypeId;
    //    obj.VatTypeName = item.VatTypeName
    //    obj.VatPercentage = item.VatPercentage;

    //    var contains = false;
    //    var i = groupByData.length;
    //    while (i--) {

    //        if (groupByData[i].VatTypeId == obj.VatTypeId && groupByData[i].VatTypeName == obj.VatTypeName && groupByData[i].VatPercentage == obj.VatPercentage) {
    //            contains = true;
    //            break;
    //        }
    //    }

    //    if (!contains) {
    //        groupByData.push(obj);
    //    }
    //});
    
    //var PlusIndex = 0;
    //var TargetId = "#SubTotal";
    //$.each(groupByData, function (k, obj) {


    //    PlusIndex += 1;
    //    $(TargetId).after("<div class='SummaryItem' id='Plus" + PlusIndex + "'><div class='SummaryItemValue'>" + "+" + "</div><div class='SummaryItemLabel'></div></div>");
    //    TargetId = "#Plus" + PlusIndex;

    //    var itemLabel = obj.VatTypeName + " (" + obj.VatPercentage + "%)";
    //    var itemValue = 0;

    //    $.each(invoicePM.InvoiceLines, function (x, line) {

    //        if (line.VatTypeId == obj.VatTypeId && line.VatTypeName == obj.VatTypeName && line.VatPercentage == obj.VatPercentage) {

    //            itemValue += line.InvoiceCurrencyAmount * line.VatPercentage / 100;
    //        }
    //    });

    //    PlusIndex += 1;
    //    $(TargetId).after("<div class='SummaryItem' id='Plus" + PlusIndex + "'><div class='SummaryItemValue'>" + itemValue.toFixed(2) + "</div><div class='SummaryItemLabel'>" + itemLabel + "</div></div>");
    //    TargetId = "#Plus" + PlusIndex;
    //});
}

function BuildHelpersList(helpers) {

    var HelpersList = [];

    $.each(helpers, function (index, helper) {

        var item = new HelperListClass();
        item.Code = helper.Code;
        item.Name = helper.Name;
        item.CreateDate = $.Convert.ToShortMonthYear(helper.CreateDate);
        item.UpdateDate = $.Convert.ToShortMonthYear(helper.UpdateDate);
        item.Language = helper.Language;
        item.Type = helper.Type;
        item.Category = helper.Category;
        item.VideoURL = helper.VideoURL;

        if (helper.Duration != "" && helper.Duration != null) {
            item.Duration = "( " + $.trim(helper.Duration) + " )";
        }

        item.FileName = helper.FileName;

        if (helper.Type == "TUT") {

            item.TypeSRC = "../TrainingResourcesHTML/images/tutorial-icon.png";
        }
        
        else if (helper.Type == "VID") {

            item.TypeSRC = "../TrainingResourcesHTML/images/video-icon.png";
        }

        else if (helper.Type == "HOW") {

            item.TypeSRC = "../TrainingResourcesHTML/images/howto-icon.png";
        }

        else if (helper.Type == "REL") {

            item.TypeSRC = "../TrainingResourcesHTML/images/refresh-icon.png";
        }

        HelpersList.push(item);
    });

    return HelpersList;
}