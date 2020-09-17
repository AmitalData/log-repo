(function (jQuery) {

    jQuery.CurrentEmail = null;
    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;
    jQuery.LocalCurrencyCode = null;
    jQuery.ProfitCurrencyCode = null;
    jQuery.CurrentEntityId = null;
    jQuery.CurrentEntityPM = null;

    jQuery.SelectedTabCode = "STST";
    jQuery.IsTabSelected_STST = false;
    jQuery.IsTabSelected_OVER = false;
    jQuery.IsTabSelected_ADDR = false;
    jQuery.IsTabSelected_CONT = false;
    jQuery.IsTabSelected_DOCS = false;

    jQuery.ActualDataList = null;
    jQuery.ShipmentsChartDataList = null;
    jQuery.QuotesChartDataList = null;
    jQuery.ShipmentChartTimeValue = null;

    jQuery.IsChart1Loaded = false;
    jQuery.IsChart2Loaded = false;
    jQuery.IsChart3Loaded = false;

    jQuery.GetAxisValues = (function (value) {
        
        var Step;
        var MaxValue;

        var digit = parseInt(value);

        if (digit < 5) {
            this.MaxValue = 5;
            this.Step = 1;
        }

        else if (digit < 10) {
            this.MaxValue = 10;
            this.Step = 2;
        }

        else {

            var myString = "" + value;
            var unitString = "1" + Array(myString.length + 1).join("0");
            var unitInteger = parseInt(unitString);

            if ((unitInteger / 5) > digit)
            {
                this.MaxValue = unitInteger / 5;
                this.Step = unitInteger / 20;
            }

            else if ((unitInteger / 4) > digit)
            {
                this.MaxValue = unitInteger / 4;
                this.Step = unitInteger / 20;
            }

            else if ((unitInteger / 2) > digit)
            {
                this.MaxValue = unitInteger / 2;
                this.Step = unitInteger / 10;
            }

            else
            {
                this.MaxValue = unitInteger;
                this.Step = unitInteger / 5;
            }
        }

        var myValues =
            {
                Step: this.Step,
                MaxValue: this.MaxValue
            };

        return myValues;
    });

    jQuery.ResizePage = (function () {
       
        var minHeight = 400;
        var fixedHeight = 50;
        var screenHeight = $(window).height();
        var PageHeight = screenHeight - fixedHeight;
        if (PageHeight < minHeight) {
            PageHeight = minHeight;
        }

        var PageContentHeight = PageHeight - 124;
        
        //if (navigator.appName == 'Microsoft Internet Explorer') {
        //    PageContentHeight = PageHeight - 124;
        //}

        //if (navigator.userAgent.indexOf('Firefox') != -1) {
        //    PageContentHeight = PageHeight - 123;
        //}

        //if (navigator.userAgent.indexOf('Chrome') != -1) {
        //    PageContentHeight = PageHeight - 125;
        //}        

        var tabContentHeight = PageContentHeight - 35;
        var chartHeight = (tabContentHeight - 70) / 2;
        var tabItemHeight = tabContentHeight - 41;

        $("#Page").css({ height: PageHeight });
        $("#PageContent").css({ height: PageContentHeight });
        $(".TabContent").css({ height: tabContentHeight });
        $("#ActionItemsScroller").css({ height: tabContentHeight - 30 });
        

               
        $("#ActualEstimateChart").css({ height: chartHeight });
        $(".k-content").css({ height: chartHeight });
        $("#QuotesPieChart").css({ height: chartHeight });
        $("#ShipmentsChart").css({ height: chartHeight });
        
        var minWidth = 950;
        var fixedWidth = 180;
        var screenWidth = $(window).width();
        if (screenWidth < minWidth) {
            screenWidth = minWidth;
        }

        var PageWidth = screenWidth - fixedWidth;
        var fullChartWidth = PageWidth;
        var halfChartWidth = (fullChartWidth / 2) - 10;

        $("#ActualEstimateChart").css({ width: fullChartWidth })        
        $("#QuotesPieChart").css({ width: halfChartWidth })
        $("#ShipmentsChart").css({ width: halfChartWidth })

        if ($.IsChart1Loaded) {            
            var chart = $("#ActualEstimateChart").data("kendoChart");
            //chart.redraw();
            chart.refresh();
        }

        if ($.IsChart2Loaded) {
            var chart = $("#ShipmentsChart").data("kendoChart");
            //chart.redraw();
            chart.refresh();
        }

        if ($.IsChart3Loaded) {
            var chart = $("#QuotesPieChart").data("kendoChart");
            //chart.redraw();
            chart.refresh();
        }
    });

    jQuery.GetLogginData = (function () {

        var url = "../api/commondata/?email=" + $.CurrentEmail + "&tenant=" + $.CurrentTenant + "&cardId=" + $.CurrentCardId;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.LocalCurrencyCode = result.LocalCurrencyCode;
                $.ProfitCurrencyCode = result.ProfitCurrencyCode;
                $("#CompanyText").html($.trim(result.TenantCompany));
                $("#MemberText").html($.trim(result.ContactName));
                $("#MemberCardText").html(" (" + $.trim(result.CardName) + ")");
                $(".LocalCurrencyCode").html(" (" + $.trim(result.LocalCurrencyCode) + ")");

                var data = [
                    { text: "Shipments", value: "SP" },
                    { text: "Gross weight", value: "GW" },
                    { text: "Chargeable weight", value: "CW" },
                    { text: "Profit (" + $.LocalCurrencyCode + ")", value: "PL" },
                    { text: "Profit (" + $.ProfitCurrencyCode + ")", value: "PP" },
                    { text: "Receivables (" + $.LocalCurrencyCode + ")", value: "RL" },
                    { text: "Receivables (" + $.ProfitCurrencyCode + ")", value: "RP" }
                ];

                
                var dropDown = $("#ShipmentsFieldDropDown").data("kendoDropDownList");
                dropDown.setDataSource(data);
            },

            error: function (jqXHR, textStatus, errorThrown) {
                //$.CheckUserException(jqXHR);
            }
        });

    });

    jQuery.GetStartDate = (function (date) {
        
        var days;
        var startDate = "";

        if (date != null) {
            var tv1 = Date.parse(date);
            var tv2 = Date.parse(new Date());
            days = (tv2 - tv1) / 1000 / 86400;
            days = Math.round(days - 0.5);
        }

        if (days == 0) {
            startDate = "Today";
        }

        if (days == 1) {
            startDate = "Yesterday";
        }

        if (days > 1 && days < 31) {
            startDate = days + " Days";
        }

        if (days >= 31 && days < 1095) {
            var months = days / 31;
            months = Math.round(months - 0.5);

            if (months == 1) {
                startDate = months + " Month";
            }

            else {
                startDate = months + " Months";
            }
        }

        if (days > 1095) {
            var years = days / 365;
            years = Math.round(years - 0.5);

            if (years == 1) {
                startDate = years + " Year";
            }

            else {
                startDate = years + " Years";
            }
        }

        return startDate;
    });

    jQuery.UpdateHeader = (function () {                

        $("#NameControl").html($.trim($.CurrentEntityPM.EnglishName));

        switch ($.CurrentEntityPM.RankName) {

            case "Silver": {
                $(".Rank1").show();
                break;
            }

            case "Gold": {
                $(".Rank2").show();
                break;
            }

            case "Platinum": {
                $(".Rank3").show();
                break;
            }

            default: {
                $(".Rank0").show();
                break;
            }
        }
              
        $("#Code").html($.trim($.CurrentEntityPM.Code));
        $("#Industry").html($.trim($.CurrentEntityPM.IndustryName));
        $("#StartDate").html($.GetStartDate($.CurrentEntityPM.StartWorkingDate));
        $("#LastShipment").html($.GetStartDate($.CurrentEntityPM.LastShipmentDate));
        $("#CountryImg").attr('src', "../images/Flags/" + $.CurrentEntityPM.CountryCode + ".png");
        $("#CityText").html($.trim($.CurrentEntityPM.CityName));
        $("#Salesman").html($.trim($.CurrentEntityPM.SalesmanUserEnglishName));
        $("#ATTN").html($.trim($.CurrentEntityPM.ATTN));       
        $("#HeaderData").show();
    });

    jQuery.BuildOverviewScreenData = (function () {


        //Responsibility
        $("#AccountManager").html($.trim($.CurrentEntityPM.AccountManagerUserEnglishName));
        $("#Classifier").html($.trim($.CurrentEntityPM.ClassifierName));
        $("#Collector").html($.trim($.CurrentEntityPM.CollectorName));
        $("#SalesmanText").html($.trim($.CurrentEntityPM.SalesmanUserEnglishName));


        //AdditionalServices
        if ($.CurrentEntityPM.CustomerAdditionalServices.length == 0) {

            $("#ServicesListBox").css({
                "font-family": "Arial",
                "color": "#8F9293",
                "font-size": "16px",
                "margin-top": "10px",
                "text-indent": "10px"
            });

            $("#ServicesListBox").html("No Services");
        }

        else {

            var DataList = [];

            $.each($.CurrentEntityPM.CustomerAdditionalServices, function (index, item) {
                DataList.push({
                    Name: $.trim(item.ProductTypeName),
                });
            });

            $("#ServicesListBox").html("");
            $("#ServicesListBox").kendoListView(
            {
                dataSource: { data: DataList },
                template: kendo.template($("#ServicesListBoxItemDataTemplate").html())
            });
        }

        //Competitors
        if ($.CurrentEntityPM.CustomerCompetitors.length == 0) {

            $("#CompetitorsListBox").css({
                "font-family": "Arial",
                "color": "#8F9293",
                "font-size": "16px",
                "margin-top": "10px",
                "text-indent": "10px"
            });

            $("#CompetitorsListBox").html("No Competitors");
        }

        else {

            var DataList = [];

            $.each($.CurrentEntityPM.CustomerCompetitors, function (index, item) {
                DataList.push({
                    Name: $.trim(item.CompetitorName),
                });
            });

            $("#CompetitorsListBox").html("");
            $("#CompetitorsListBox").kendoListView(
            {
                dataSource: { data: DataList },
                template: kendo.template($("#CompetitorsListBoxItemDataTemplate").html())
            });
        }

    });

    jQuery.IsCustomerPMLoaded = false;
    jQuery.IsProductsChartDataLoaded = false;
    jQuery.IsShipmentsChartDataLoaded = false;
    jQuery.IsQuotesChartDataLoaded = false;
    jQuery.StopStatisticsTabBusyIndicator = (function () {
        if ($.IsCustomerPMLoaded && $.IsProductsChartDataLoaded && $.IsShipmentsChartDataLoaded && $.IsQuotesChartDataLoaded) {
            $("#StatisticsTabBusyIndicator").hide();
        }
    });

    jQuery.IsMoneyInformationDataLoaded = false;
    jQuery.IsOpportunitesListDataLoaded = false;   
    jQuery.IsActivitiesListDataLoaded = false;
    jQuery.IsQuotesListDataLoaded = false;
    jQuery.IsCompetitorsListDataLoaded = false;
    jQuery.StopOverviewTabBusyIndicator = (function () {
        if ($.IsMoneyInformationDataLoaded && $.IsOpportunitesListDataLoaded && $.IsActivitiesListDataLoaded && $.IsQuotesListDataLoaded && $.IsCompetitorsListDataLoaded) {
            $("#OverviewTabBusyIndicator").hide();
        }
    });

    jQuery.GetMoneyInformation = (function () {

        $("#OverviewTabBusyIndicator").show();

        var url = "../api/CustomersData?CRMMoneyCustomerId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                var Outstanding = $.trim(result.OpenARInvoices) == "" ? 0 : result.OpenARInvoices;
                var Overdue = $.trim(result.InvoicesDue) == "" ? 0 : result.InvoicesDue;
                var OpenReceivables = $.trim(result.OpenReceivables) == "" ? 0 : result.OpenReceivables;
                var OpenPayments = $.trim(result.ARPayments) == "" ? 0 : result.ARPayments;

                $("#Outstanding").html(Outstanding.toFixed(2));
                $("#Overdue").html(Overdue.toFixed(2));
                $("#OpenReceivables").html(OpenReceivables.toFixed(2));
                $("#OpenPayments").html(OpenPayments.toFixed(2));

                $.IsMoneyInformationDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.IsMoneyInformationDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            }
        });
    });

    jQuery.GetOpportunitesListData = (function () {

        $("#OverviewTabBusyIndicator").show();

        function Filters() {
            this.CustomerId = $.CurrentEntityId;
            this.IsClosed = false;
        };

        var filters = new Filters();

        var url = "../api/Opportunities?tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            contentType: 'application/json',

            success: function (result) {

                if (result == null || result.length == 0) {

                    $("#OpportunitiesListBox").css({
                        "font-family": "Arial",
                        "color": "#8F9293",
                        "font-size": "16px",
                        "margin-top": "10px",
                        "text-indent": "10px"
                    });

                    $("#OpportunitiesListBox").html("No Opportunities");
                }

                else {

                    var DataList = [];

                    $.each(result, function (index, item) {
                        
                        var brush;

                        switch (item.RatingCode) {
                            case "H": { brush = "rgb(206, 17, 17)"; break; }
                            case "C": { brush = "rgb(39, 114, 208)"; break; }
                            case "W": { brush = "rgb(255, 137, 59)"; break; }
                            default: { brush = "rgb(153, 153, 153)"; break; }
                        }

                        DataList.push({
                            Id: item.Id,
                            Topic: $.trim(item.Topic),
                            Shipments: $.trim(item.NumberOfShipments) == "" ? "0" : $.trim(item.NumberOfShipments),
                            Stage: $.trim(item.StageName),
                            RatingColor: brush,
                        });
                    });

                    $("#OpportunitiesListBox").html("");
                    $("#OpportunitiesListBox").kendoListView(
                    {
                        dataSource: { data: DataList },
                        template: kendo.template($("#OpportunityListBoxItemDataTemplate").html())
                    });
                }

                $.IsOpportunitesListDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.IsOpportunitesListDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            }
        });
    });

    jQuery.GetActivitiesListData = (function () {

        $("#OverviewTabBusyIndicator").show();

        function Filters() {
            this.EntityId = $.CurrentEntityId;
            this.IsOpen = true;
        };

        var filters = new Filters();

        var url = "../api/Activities?tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            contentType: 'application/json',

            success: function (result) {

                if (result == null || result.length == 0) {

                    $("#ActivitiesListBox").css({
                        "font-family": "Arial",
                        "color": "#8F9293",
                        "font-size": "16px",
                        "margin-top": "10px",
                        "text-indent": "10px"
                    });

                    $("#ActivitiesListBox").html("No Activities");
                }

                else {

                    var DataList = [];

                    var type
                    $.each(result, function (index, item) {

                        var typeSRC = "images/" + $.trim(item.ActivityTypeCode) + ".png";

                        DataList.push({
                            Id: item.Id,
                            Subject: $.trim(item.Subject),
                            TypeSRC: typeSRC,
                            TypeName: $.trim(item.ActivityTypeName),
                            DueDate: $.trim(item.DueDate) == "" ? "" : $.Convert.ToShortDate(item.DueDate),
                            DueTime: $.trim(item.DueDate) == "" ? "" : $.Convert.ToShortTime(item.DueDate),
                        });
                    });

                    $("#ActivitiesListBox").html("");
                    $("#ActivitiesListBox").kendoListView(
                    {
                        dataSource: { data: DataList },
                        template: kendo.template($("#ActivityListBoxItemDataTemplate").html())
                    });
                }

                $.IsActivitiesListDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.IsActivitiesListDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            }
        });
    });

    jQuery.GetQuotesListData = (function () {

        $("#OverviewTabBusyIndicator").show();

        function Filters() {
            this.CustomerId = $.CurrentEntityId;
            this.IsCancelled = false;
        };

        var filters = new Filters();

        var url = "../api/QuotesMobile?tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            contentType: 'application/json',

            success: function (result) {                               

                if (result == null || result.length == 0) {

                    $("#QuotesListBox").css({
                        "font-family": "Arial",
                        "color": "#8F9293",
                        "font-size": "16px",
                        "margin-top": "10px",
                        "text-indent": "10px"
                    });

                    $("#QuotesListBox").html("No Quotes");
                }

                else {

                    var DataList = []

                    $.each(result, function (index, Quote) {

                        FromCountySRC = "../images/Flags/" + Quote.FromCountryCode + ".png";
                        ToCountySRC = "../images/Flags/" + Quote.ToCountryCode + ".png";

                        var DirectionSRC = "";
                        var DirectionName = "";
                        switch (Quote.DirectionId) {

                            case "E": {
                                DirectionSRC = "../HtmlHelpers/Images/Icons/Export.png";
                                DirectionName = "Export";
                                break;
                            }

                            case "I": {
                                DirectionSRC = "../HtmlHelpers/Images/Icons/Import.png";
                                DirectionName = "Import";
                                break;
                            }

                            case "D": {
                                DirectionSRC = "../HtmlHelpers/Images/Icons/Domestic.png";
                                DirectionName = "Domestic";
                                break;
                            }
                        }

                        var TransportSRC = "";
                        var TransportName = "";
                        switch (Quote.TransportModeId) {

                            case "A": {
                                TransportSRC = "../HtmlHelpers/Images/Icons/Air.png";
                                TransportName = "Air";
                                break;
                            }

                            case "I": {
                                TransportSRC = "../HtmlHelpers/Images/Icons/Inland.png";
                                TransportName = "Inland";
                                break;
                            }

                            case "O": {
                                TransportSRC = "../HtmlHelpers/Images/Icons/Ocean.png";
                                TransportName = "Ocean";
                                break;
                            }
                        }

                        DataList.push({
                            Id: Quote.Id,
                            FromCountySRC: FromCountySRC,
                            ToCountySRC :ToCountySRC,
                            DirectionSRC: DirectionSRC,
                            DirectionName: DirectionName,
                            TransportSRC: TransportSRC,
                            TransportName: TransportName,
                            FromPortName: Quote.FromPortName,
                            ToPortName: Quote.ToPortName,
                            OpenDate:  $.trim(Quote.OpenDate) == "" ? "" : $.Convert.ToShortDate(Quote.OpenDate),
                        });

                    });

                    $("#QuotesListBox").html("");
                    $("#QuotesListBox").kendoListView(
                    {
                        dataSource: { data: DataList },
                        template: kendo.template($("#QuoteListBoxItemDataTemplate").html())
                    });
                }
                
                $.IsQuotesListDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                
                $.IsQuotesListDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            }
        });
    });

    jQuery.GetCompetitorsListData = (function () {

        $("#OverviewTabBusyIndicator").show();

        var url = "../api/CustomersData?quotesChartCustomerId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.IsCompetitorsListDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.IsCompetitorsListDataLoaded = true;
                $.StopOverviewTabBusyIndicator();
            }
        });
    });

    jQuery.DrawProductsChart = (function (dataCode) {                
        if ($.CurrentEntityPM && $.ActualDataList) {
                        
            if (dataCode == null) {
                dataCode = "S";
            }

            var ProductTypes = [];
            //ProductTypes.push({ Code: "AD", Name: "Air Domestic" });
            ProductTypes.push({ Code: "AE", Name: "Air Export" });
            ProductTypes.push({ Code: "AI", Name: "Air Import" });
            //ProductTypes.push({ Code: "OD", Name: "Ocean Domestic" });
            ProductTypes.push({ Code: "OE", Name: "Ocean Export" });
            ProductTypes.push({ Code: "OI", Name: "Ocean Import" });
            //ProductTypes.push({ Code: "ID", Name: "Inland Domestic" });
            ProductTypes.push({ Code: "IE", Name: "Inland Export" });
            ProductTypes.push({ Code: "II", Name: "Inland Import" });

            var ChartLabels = [];
            var ActualValues = [];
            var PetentialValues = [];
            var dataMaxValue = 0;

            $.each(ProductTypes, function (index, Type) {
                
                var ActualValue = 0;
                var PetentialValue = 0;

                $.each($.ActualDataList, function (index1, item1) {
                    
                    if (item1.ProductTypeCode == Type.Code) {

                        switch (dataCode) {
                            case "S": { ActualValue = item1.NumberOfShipments; break; }
                            case "T": { ActualValue = item1.TEU; break; }
                            case "R": { ActualValue = item1.Revenue; break; }
                            case "C": { ActualValue = item1.ChargeableWeight; break; }
                        }                        

                        if (ActualValue > dataMaxValue) {
                            dataMaxValue = ActualValue;
                        }
                        return;
                    }
                });
                
                $.each($.CurrentEntityPM.CustomerProducts, function (index2, item2) {
                                        
                    if (item2.ProductTypeCode == Type.Code) {                        

                        switch (dataCode) {
                            case "S": { PetentialValue = item2.PotentialNumberOfShipments; break; }
                            case "T": { PetentialValue = item2.PotentialTEU; break; }
                            case "R": { PetentialValue = item2.PotentialRevenue; break; }
                            case "C": { PetentialValue = item2.PotentialChargeableWeight; break; }
                        }

                        if (PetentialValue > dataMaxValue) {
                            dataMaxValue = PetentialValue;
                        }
                        return;
                    }
                });

                ChartLabels.push(Type.Name);
                ActualValues.push(ActualValue);
                PetentialValues.push(PetentialValue);
            });            
            
            var AxisValues = $.GetAxisValues(dataMaxValue);            

            $("#ActualEstimateChart").kendoChart({

                title: {
                    visible: false,
                    text: ""
                },

                legend: {
                    visible: false,
                    position: "bottom"
                },

                seriesDefaults: {
                    type: "column"
                },

                series: [{
                    name: "Potential Data",
                    data: PetentialValues,
                    spacing: 0.1,
                    gap: 0.5,
                    color: "#7DC3D5"
                }, {
                    name: "Actual Data",
                    data: ActualValues,
                    spacing: 0.1,
                    gap: 0.5,
                    color:"#E3697B"
                }],

                valueAxis: {
                    line: {
                        visible: false,
                    },
                    
                    minorGridLines: {
                        visible: false
                    },                    

                    labels: {
                        format: "{0}",
                        //color: "#aa00bb"
                        //step: AxisValues.Step,
                        //template: "Year: #: value #"                      
                    },

                    min:0,
                    max: AxisValues.MaxValue,
                    step: AxisValues.Step,
                },

                categoryAxis: {
                    categories: ChartLabels,
                    majorGridLines: {
                        visible: false
                    }
                },

                tooltip: {
                    visible: true,
                    format: "{0}"
                }
            });
        }

        $.IsChart1Loaded = true;
    });

    jQuery.DrawShipmentsChart = (function (dataCode, dataName) {                

        if ($.ShipmentsChartDataList) {

            if (dataCode == null) {
                dataCode = "SP";
            }

            if (dataName == null) {
                dataName = "Shipments";
            }

            var ChartLabels = [];
            var ChartValues = [];
            //var dataMinValue = 0;
            //var dataMaxValue = 0;

            $.each($.ShipmentsChartDataList, function (index, item) {

                var fieldValue = 0;                
                ChartLabels.push(item.LabelProperty);

                switch (dataCode) {

                    case "SP": { fieldValue = item.Shipments; break; }
                    case "GW": { fieldValue = item.GrossWeight; break; }
                    case "CW": { fieldValue = item.ChargeableWeight; break; }
                    case "PL": { fieldValue = item.ProfitInLocal; break; }
                    case "PP": { fieldValue = item.ProfitInProfit; break; }
                    case "RL": { fieldValue = item.ReceivablesInLocal; break; }
                    case "RP": { fieldValue = item.ReceivablesInProfit; break; }
                    default: { fieldValue = item.Shipments; break; }
                }

                ChartValues.push(fieldValue);

                //if (fieldValue < dataMinValue) {
                //    dataMinValue = fieldValue;
                //}

                //if (fieldValue > dataMaxValue) {
                //    dataMaxValue = fieldValue;
                //}
            });
            
            //var axisFormat = null;
            //var axisMaxValue = null;
            //var axisStep = null;
            //var AxisValues = $.GetAxisValues(dataMaxValue);

            $("#ShipmentsChart").kendoChart({

                title: {
                    visible: false,
                    text: ""
                },

                legend: {
                    visible: false,
                    position: "bottom"
                },

                seriesDefaults: {
                    type: "area"
                },

                series: [{
                    name: dataName,
                    data: ChartValues,
                    color: "Orange",
                    opacity: 1,
                }],

                valueAxis: {
                    labels: {
                        //format: axisFormat
                    },

                    line: {
                        visible: false
                    },

                    //axisCrossingValue: -10,

                    //min: dataMinValue,
                    //max: AxisValues.MaxValue,
                    //step: AxisValues.Step,
                },

                categoryAxis: {
                    categories: ChartLabels,
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: -90
                    },
                },

                tooltip: {
                    visible: true,
                    //format: "N0",
                    background: "Teal",
                    color: "white",
                    template: "#= value #"
                    //template: "#= series.name #: #= value #"
                }
            });
        }

        $.IsChart2Loaded = true;
    });

    jQuery.DrawQuotesChart = (function () {

        if ($.QuotesChartDataList == null || $.QuotesChartDataList.length == 0) {

            $("#QuotesPieChart").css({
                "font-family": "Arial",
                "color": "#8F9293",
                "font-size": "16px",
                "margin-top": "10px",
                "text-indent": "10px"
            });

            $("#QuotesPieChart").html("No Quotes");
        }

        else {
            var myData = [];
            var myLabel;
            var myColor;
            $.each($.QuotesChartDataList, function (index, item) {

                myLabel = item.StringProperty;

                switch (item.StringProperty) {

                    case "Used": {
                        myColor = "#999999";
                        break;
                    }

                    case "Created": {
                        myColor = "Teal";
                        break;
                    }

                    case "No Answer": {
                        myColor = "#282E30";
                        break;
                    }

                    case "In Progress": {
                        myColor = "#42a7ff";
                        break;
                    }

                    case "Sent To Customer": {
                        myColor = "Orange";
                        myLabel = "Sent";
                        break;
                    }

                    case "Rejected By Customer": {
                        myLabel = "Rejected";
                        myColor = "Red";
                        break;
                    }

                    case "Approved By Customer": {
                        myColor = "#9de219";
                        myLabel = "Approved";
                        break;
                    }
                }

                myData.push({
                    Name: myLabel,
                    Amount: item.IntegerProperty,
                    color: myColor,
                    //explode: false
                });
            });


            //var data = [
            //    {
            //        "source": "Hydro",
            //        "percentage": 22,
            //        "explode": true
            //    },
            //    {
            //        "source": "Solar",
            //        "percentage": 8
            //    },
            //    {
            //        "source": "Nuclear",
            //        "percentage": 49
            //    },
            //    {
            //        "source": "Wind",
            //        "percentage": 27
            //    }
            //];

            $("#QuotesPieChart").kendoChart({

                title: {
                    visible: false,
                    text: ""
                },

                legend: {
                    position: "right",
                    labels: {
                        template: "#= text # (#= value #)"
                    }
                },

                dataSource: {
                    data: myData
                },

                seriesDefaults: {
                    labels: {
                        visible: false,
                        background: "transparent",
                        template: "#= category #: #= value #"
                    }
                },

                series: [{
                    type: "pie",
                    field: "Amount",
                    categoryField: "Name",
                    explodeField: "explode",
                    color: "color"
                }],

                //seriesColors: ["Red", "Green", "Teal", "Orange"],

                tooltip: {
                    visible: true,
                    template: "${ category }: ${ value }"
                }
            });

            $.IsChart3Loaded = true;
        }        
    });

    jQuery.GetActualProductData = (function () {
        
        $("#StatisticsTabBusyIndicator").show();

        var url = "../api/CustomersData?actualDataCustomerId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.ActualDataList = result;
                $.DrawProductsChart();
                $.IsProductsChartDataLoaded = true;
                $.StopStatisticsTabBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.IsProductsChartDataLoaded = true;
                $.StopStatisticsTabBusyIndicator();
            }
        });
    });

    jQuery.GetShipmentsChartData = (function () {
                
        $("#StatisticsTabBusyIndicator").show();

        if ($.ShipmentChartTimeValue == null) {
            $.ShipmentChartTimeValue = -12;
        }

        var url = "../api/CustomersData?customerid=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant + "&lastMonthsCount=" + $.ShipmentChartTimeValue;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {
                
                $.ShipmentsChartDataList = result;
                $.DrawShipmentsChart();

                $.IsShipmentsChartDataLoaded = true;
                $.StopStatisticsTabBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.IsShipmentsChartDataLoaded = true;
                $.StopStatisticsTabBusyIndicator();
            }
        });
    });

    jQuery.GetQuotesChartData = (function () {

        $("#StatisticsTabBusyIndicator").show();

        var url = "../api/CustomersData?quotesChartCustomerId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.QuotesChartDataList = result;
                $.DrawQuotesChart();
                $.IsQuotesChartDataLoaded = true;
                $.StopStatisticsTabBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.IsQuotesChartDataLoaded = true;
                $.StopStatisticsTabBusyIndicator();
            }
        });
    });

    jQuery.GetSingleEntityPM = (function () {       
        
        $("#StatisticsTabBusyIndicator").show();

        var url = "../api/CustomersData?singleCustomerId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (entityPM) {                                
                                
                $.CurrentEntityPM = entityPM;

                if (entityPM) {
                    
                    $.UpdateHeader();
                    $.DrawProductsChart();
                }

                $.IsCustomerPMLoaded = true;
                $.StopStatisticsTabBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {                
                $.IsCustomerPMLoaded = true;
                $.StopStatisticsTabBusyIndicator();
            }
        });
    });

    jQuery.GetAllAddresses = (function () {

        $("#AddressesTabBusyIndicator").show();

        var url = "../api/CustomersData?addressesCustomerId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {
                
                var DataList = []

                $.each(result, function (index, item) {

                    var placeField = $.trim(item.CountryEnglishName);

                    if ($.trim(item.StateEnglishName) != "") {
                        placeField = placeField + ", " + $.trim(item.StateEnglishName);
                    }

                    placeField = placeField + ", " + $.trim(item.City);
                    
                    if ($.trim(item.ZipCode) != "") {
                        placeField = placeField + ", " + $.trim(item.ZipCode);
                    }

                    var CountySRC = "../images/Flags/" + item.CountryCode + ".png";

                    DataList.push({
                        Id: item.Id,
                        Name: $.trim(item.Name),
                        Address1: $.trim(item.Address1),
                        Address2: $.trim(item.Address2),
                        Phone: $.trim(item.PhoneNumber),
                        Fax: $.trim(item.FaxNumber),
                        Place: placeField,
                        CountySRC: CountySRC,                        
                    });
                });

                $("#AddressesListBox").html("");
                $("#AddressesListBox").kendoListView(
                {
                    dataSource: { data: DataList },
                    template: kendo.template($("#AddressListBoxItemDataTemplate").html())
                });

                $("#AddressesTabBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $("#AddressesTabBusyIndicator").hide();
            }
        });
    });

    jQuery.GetAllContacts = (function () {

        $("#ContactsTabBusyIndicator").show();

        var url = "../api/CustomersData?contactsCustomerId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {
                
                var DataList = []

                if (result.length > 0) {
                    $.each(result, function (index, item) {

                        DataList.push({
                            Id: item.Id,
                            Name: $.trim(item.EnglishName),
                            Position: $.trim(item.Position),
                            Email: $.trim(item.Email),
                            Mobile: $.trim(item.Mobile),
                            Fax: $.trim(item.Fax),
                            Phone: $.trim(item.BusinessPhone),
                        });
                    });

                    $("#ContactsListBox").html("");
                    $("#ContactsListBox").kendoListView(
                    {
                        dataSource: { data: DataList },
                        template: kendo.template($("#ContactListBoxItemDataTemplate").html())
                    });
                }

                else {
                    $("#ContactsListBox").css({
                        "font-family": "Arial",
                        "color": "#8F9293",
                        "font-size": "16px",
                        "margin-top": "10px",
                        "text-indent": "10px"
                    });

                    $("#ContactsListBox").html("No Contacts");
                }

                $("#ContactsTabBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $("#ContactsTabBusyIndicator").hide();
            }
        });
    });

    $(".HyperLinkQuery").mouseenter(function () {
        if ($(this).attr('id') != $.SelectedTabCode) {
            $(this).css({ "color": "black", "background": "url('images/TabItem-O.png') repeat-x" });
        }
    });
    $(".HyperLinkQuery").mouseleave(function () {
        if ($(this).attr('id') != $.SelectedTabCode) {
            $(this).css({ "color": "#45494A", "background": "transparent" });
        }
    });
    $(".HyperLinkQuery").click(function () {
        if ($(this).attr('id') != $.SelectedTabCode) {
            $.SelectedTabCode = $(this).attr('id');
            $.SelectTab();
        }
    });
    $("#BackButton").click(function () {
        parent.history.back();
        return false;
    });

    $("#SignOutButton").mouseenter(function () {
        $("#SignOutButton").css({ "background": "url('images/Signout-O.png') no-repeat" });
    });
    $("#SignOutButton").mouseleave(function () {
        $("#SignOutButton").css({ "background": "url('images/Signout-N.png') no-repeat" });
    });
    $("#SignOutButton").click(function () {

        var url = "../api/Authentication/?userEmail=" + $.CurrentEmail;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {
                document.location.href = "../CustomersLoginPage.aspx";
            },

            error: function (jqXHR, textStatus, errorThrown) {
                //$.CheckUserException(jqXHR);
                alert("logout failed!");
            }
        });
    });

    jQuery.BuildStatisticsPageFilters = (function (tabCode) {

        $("#ActualsChartDeopDown").kendoDropDownList({

            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Shipments", value: "S" },
                { text: "TEU", value: "T" },
                { text: "Revenue", value: "R" },
                { text: "Chargeable Weight", value: "C" }
            ],

            suggest: false,
            index: 0,

            change: function (e) {
                $.DrawProductsChart(this.value());
            }
        });

        $("#ShipmentsTimeDropDown").kendoDropDownList({

            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Last year", value: -12 },
                { text: "Last 3 year", value: -36 }
            ],

            suggest: false,
            index: 0,

            change: function (e) {
                $.ShipmentChartTimeValue = this.value();
                $.GetShipmentsChartData();
            }
        });

        $("#ShipmentsFieldDropDown").kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "Shipments", value: "SP" },
                { text: "Gross weight", value: "GW" },
                { text: "Chargeable weight", value: "CW" },
            ],

            suggest: false,
            index: 0,

            change: function (e) {

                $.DrawShipmentsChart(this.value(), this.text());
            }
        });
    });

    jQuery.SelectTab = (function (tabCode) {       

        $(".TabPage").hide();
        $(".HyperLinkQuery").css({ "color": "#45494A", "background": "transparent" });

        if ($.SelectedTabCode == null) {
            $.SelectedTabCode = "STST";
        }

        switch ($.SelectedTabCode) {

            case "STST": {
                $("#STST_Page").show();

                if (!$.IsTabSelected_STST) {
                    $.IsTabSelected_STST = true

                    $.BuildStatisticsPageFilters();
                    $.GetSingleEntityPM();
                    $.GetActualProductData();
                    $.GetShipmentsChartData();
                    $.GetQuotesChartData();
                }

                break;
            }

            case "OVER": {
                $("#OVER_Page").show();

                if (!$.IsTabSelected_OVER) {
                    $.IsTabSelected_OVER = true;

                    $.BuildOverviewScreenData();
                    $.GetMoneyInformation();
                    $.GetOpportunitesListData();
                    $.GetActivitiesListData();
                    $.GetQuotesListData();
                    $.GetCompetitorsListData();
                }
                break;
            }

            case "ADDR": {
                $("#ADDR_Page").show();

                if (!$.IsTabSelected_ADDR) {
                    $.IsTabSelected_ADDR = true

                    $.GetAllAddresses();
                }

                break;
            }

            case "CONT": {
                $("#CONT_Page").show();

                if (!$.IsTabSelected_CONT) {
                    $.IsTabSelected_CONT = true

                    $.GetAllContacts();
                }

                break;
            }

            case "DOCS": {
                $("#DOCS_Page").show();

                if (!$.IsTabSelected_DOCS) {
                    $.IsTabSelected_DOCS = true
                }

                break;
            }
        }

        $("#" + $.SelectedTabCode).css({ "color": "white", "background": "url('images/SelectedQuery.png') repeat-x" });
    });

    $(document).ready(function () {
                
        $.ResizePage();
        $(window).resize(function () {
            $.ResizePage();
        });
                
        var hash = $(location).attr('href');
        var hashSplit = hash.split("?");
        var loginText = hashSplit[1].toLowerCase();
        loginText = loginText.replace("id=", "");
        loginText = loginText.replace("email=", "");
        loginText = loginText.replace("tenant=", "");
        loginText = loginText.replace("cardid=", "");
        loginText = loginText.replace("ischamplogin=", "");
        var loginData = loginText.split('&');

        $.CurrentEntityId = loginData[0];
        $.CurrentTenant = loginData[1];
        $.CurrentEmail = loginData[2];        
        $.CurrentCardId = loginData[3];

        $.GetLogginData();
        $.SelectTab();
    });

}(jQuery));