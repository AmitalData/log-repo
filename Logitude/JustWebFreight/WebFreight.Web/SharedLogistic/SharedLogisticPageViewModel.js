(function (jQuery) {
    jQuery.Token = null;
    jQuery.CurrentEmail = null;
    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;
    jQuery.CurrentCardType = null;
    jQuery.TenantDateTimeFormat = null;
    jQuery.IsBrandingEnabled = "";
    jQuery.IsInvoicesMenuEnabled = false;
    jQuery.IsAgentShared = false;
    jQuery.IsShipperShared = false;
    jQuery.IsConsigneeShared = false;
    jQuery.LoadingCount = 1001;

    jQuery.SearchText_SHI = null;
    jQuery.SearchText_INV = null;
    jQuery.SearchText_QUOTESREQUESTS = null;

    jQuery.SearchTimer_SHI = null;
    jQuery.SearchTimer_INV = null;
    jQuery.SelectedQuery_SHI = null;
    jQuery.SelectedQuery_INV = null;
    jQuery.SelectedDirectionId_SHI = null;
    jQuery.SelectedTransportId_SHI = null;
    jQuery.SelectedShipmentLevel = null;

    jQuery.IsDataCountLoaded = false;
    jQuery.IsShipmentsDataLoaded = false;
    jQuery.IsInvoicesDataLoaded = false;
    jQuery.IsQuotesRequestsDataLoaded = false;
    jQuery.IsReportsDataLoaded = false;

    
    jQuery.SelectedTabId = "TAB_SHI";
    jQuery.watermark_SHI = "Search partners / ports / ref.#";
    jQuery.watermark_INV = "Search Inv. # / bill to / ref.#";
    jQuery.watermark_QUOTESREQUESTS = "Search ref.#";



    jQuery.ResizePage = (function (myFixedHeight) {
        var minHeight = 400;
        var _height = $(window).height() - myFixedHeight;

        if (_height < minHeight) {
            _height = minHeight;
        }

        $(".tabPage").css({ height: _height });
        $(".tabPageContent").css({ height: _height });
        $(".ListBoxContainer").css({ height: _height - 40 });
    });

    jQuery.GetCompanyLogo = (function () {

        var url = "api/commondata/?companyId=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',
            headers: {
                'Token': $.Token
            },

            success: function (result) {


                var img = new Image();
                img.onload = function () {
                    var width = this.width > 200 ? "200px" : (this.width + "px"); 
                    jQuery("#companyLogo").attr('src', result);
                    jQuery("#companyLogo").css('width',width);
                    jQuery("#companyLogoArea").css('width', width);
                }
                img.src = result;


            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
            }
        });
    });

    jQuery.SetTabsHidden = (function (isEnabled) {
        if(isEnabled){
            $("#TAB_INV").show();
        }   

        else {
           $("#TAB_INV").hide();
        }
    });

    jQuery.SetQuotesRequestsTabVisibility = (function (isVisibly ) {
        $("#TAB_QUOTESREQUESTS").toggle(isVisibly);
    });



    jQuery.GetLogginData = (function () {

        var url = "api/commondata/?email=" + $.CurrentEmail + "&tenant=" + $.CurrentTenant + "&cardId=" + $.CurrentCardId;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',
            headers: {'Token': $.Token},
            
            success: function (result) {
                
                $("#CompanyText").html(result.TenantCompany);
                $("#MemberText").html(result.ContactName);
                $("#MemberCardText").html(" (" + result.CardName + ")");
                $.TenantDateTimeFormat = result.TenantDateTimeFormat;
                $.IsInvoicesMenuEnabled = result.IsInvoicesMenuEnabled;
                $.IsAgentShared = result.IsAgentShared;
                $.IsShipperShared = result.IsShipperShared;
                $.IsConsigneeShared = result.IsConsigneeShared;
                $.SetQuotesRequestsTabVisibility(result.IsQuotesRequestsMenuEnabled);
                $.SetTabsHidden($.IsInvoicesMenuEnabled);
                $.SetSelectedTab();                
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $.SetSelectedTab();
            }
        });
    });

    jQuery.SetSelectedTab = (function () {
        var id = $('#SavedSelectedTabId').val();

        if ($.trim(id) == "") {
            id = "TAB_SHI";
        }


        $.SelectedTabId = id;
        $('#SavedSelectedTabId').attr("value", $.SelectedTabId);
        $("#mainTabsDiv").data("kendoTabStrip").select($('#' + $.SelectedTabId));
    });

    jQuery.LoadInvoices = (function () {

        $("#InvoicesBusyIndicator").show();

        function InvoiceFilters() {
            this.PartnerId = $.CurrentCardId;
            this.PartnerType = $.CurrentCardType;
            this.SearchField = ($.trim($.SearchText_INV) == "" || $.trim($.SearchText_INV) == $.watermark_INV) ? null : $.trim($.SearchText_INV);
            this.PageSize = jQuery.LoadingCount;
            this.PageIndex = 0

            if ($.trim($.SelectedQuery_INV) == "Query_PRG_INV") {

                this.FilterName = "UnpaidInvoices";
            }

            else {

                this.FilterName = null;
            }
        };

        var filters = new InvoiceFilters();

        var url = "api/InvoicesData?tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            contentType: 'application/json',
            headers: {
                'Token': $.Token
            },

            success: function (result) {
                $.SendContactActivity($.CurrentEmail, "Invoice", "Invoices List", $.CurrentTenant, $.CurrentCardId);


                if (result.length >= $.LoadingCount) {
                    $("#InvoicesQueryCount").html("(" + ($.LoadingCount - 1) + "+)");
                }

                else {
                    $("#InvoicesQueryCount").html("(" + result.length + ")");
                }

                $("#InvoicesListBox").html("");
                $("#InvoicesListBox").kendoListView(
		        {
		            dataSource: { data: BuildInvoicesList(result, $.TenantDateTimeFormat) },
		            template: kendo.template($("#InvoiceListBoxItemDataTemplate").html())
		        });

                $("#InvoicesBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#InvoicesQueryCount").html("(0)");
                $("#InvoicesBusyIndicator").hide();
            }
        });

    });

    jQuery.LoadShipments = (function () {

        $("#ShipmentsBusyIndicator").show();


        function ShipmentFilters() {

            this.PartnerId = $.CurrentCardId;
            this.PartnerType = $.CurrentCardType;
            this.DirectionId = ($.trim($.SelectedDirectionId_SHI) == "" || $.trim($.SelectedDirectionId_SHI) == "All") ? null : $.SelectedDirectionId_SHI;
            this.TransportModeId = ($.trim($.SelectedTransportId_SHI) == "" || $.trim($.SelectedTransportId_SHI) == "All") ? null : $.SelectedTransportId_SHI;
            this.ShipmentLevelCode = ($.trim($.SelectedShipmentLevel) == "" || $.trim($.SelectedShipmentLevel) == "All") ? null : $.SelectedShipmentLevel;
            this.SearchField = ($.trim($.SearchText_SHI) == "" || $.trim($.SearchText_SHI) == $.watermark_SHI) ? null : $.trim($.SearchText_SHI);
            this.PageSize = jQuery.LoadingCount;
            this.PageIndex = 0

            if ($.trim($.SelectedQuery_SHI) == "Query_PRG_SHI") {

                this.IsOperationalClosed = false;
            }

            else {

                this.IsOperationalClosed = null;
            }
        };

        var filters = new ShipmentFilters();

        var url = "api/shipments?tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            contentType: 'application/json',
            headers: {
                'Token': $.Token
            },

            success: function (result) {                

                $.SendContactActivity($.CurrentEmail, "Shipment", "Shipments List", $.CurrentTenant, $.CurrentCardId);

                if (result.length >= $.LoadingCount) {
                    $("#ShipmentsQueryCount").html("(" + ($.LoadingCount - 1) + "+)");
                }

                else {
                    $("#ShipmentsQueryCount").html("(" + result.length + ")");
                }

                $("#ShipmentsListBox").html("");
                $("#ShipmentsListBox").kendoListView(
                    {
                        dataSource: { data: BuildShipmentsList(result, $.TenantDateTimeFormat, $.IsAgentShared, $.IsShipperShared, $.IsConsigneeShared) },
                        template: kendo.template($("#ShipmentListBoxItemDataTemplate").html())
                    });

                $("#ShipmentsBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#ShipmentsQueryCount").html("(0)");
                $("#ShipmentsBusyIndicator").hide();
            }
        });
    });


    jQuery.SetQuotesRequestQueryCount = (function (result) {
        let quotesRequestQueryCount = (result.length >= $.LoadingCount) ? "(" + ($.LoadingCount - 1) + "+)" : "(" + result.length + ")";
        $("#QuotesRequestQueryCount").html(quotesRequestQueryCount);
    });

    jQuery.FullQuotesRequestsListData = (function (result) {
        $("#QuotesRequestsListBox").html("");
        $("#QuotesRequestsListBox").kendoListView(
            {
                dataSource: { data: BuildQuotesRequests(result, $.TenantDateTimeFormat) },
                template: kendo.template($("#QuotesRequestsListBoxItemDataTemplate").html())
            });

    });
  
    function QuotesRequstFilters() {
        this.PartnerId = $.CurrentCardId;
        this.SearchField = ($.trim($.SearchText_QUOTESREQUESTS) == "" || $.trim($.SearchText_QUOTESREQUESTS) == $.watermark_QUOTESREQUESTS) ? null : $.trim($.SearchText_QUOTESREQUESTS);
        this.PageSize = jQuery.LoadingCount;
        this.PageIndex = 0
        this.Tenant = $.CurrentTenant
    };

    

    jQuery.LoadQuotesRequsts = (function () {

        $("#QuotesRequestsBusyIndicator").show();
        var filters = new QuotesRequstFilters();
        var url = "api/QuotesRequest";
        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            contentType: 'application/json',
            headers: { 'Token': $.Token },
            success: function (result) {
                $.QuotesRequstsLoadedSsuccess(result);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.QuotesRequstsLoadedFailure(jqXHR);
            }
        });

    });



    jQuery.QuotesRequstsLoadedSsuccess = (function (result) {
        $.SendContactActivity($.CurrentEmail, "QuoteRequsts", "Quote Requsts List", $.CurrentTenant, $.CurrentCardId);
        $.SetQuotesRequestQueryCount(result);
        $.FullQuotesRequestsListData(result);
        $("#QuotesRequestsBusyIndicator").hide();
    });
    

    jQuery.QuotesRequstsLoadedFailure = (function (jqXHR) {
        $.CheckUserException(jqXHR);
        $("#QuotesRequestQueryCount").html("(0)");
        $("#QuotesRequestsBusyIndicator").hide();
    });


    jQuery.LoadReports = (function () {

        let reports =  [{ "Name": "Shipments Reports", "Code": "SHRE" }];

        $("#ReportListBox").html("");
        $("#ReportListBox").kendoListView(
            {
                dataSource: { data: reports },
                template: kendo.template($("#ReportListBoxItemDataTemplate").html())
            });

    });

    






    jQuery.LoadDataCount = (function () {

        $.IsDataCountLoaded = $('#SavedIsDataCountLoaded').val();

        if ($.trim($.IsDataCountLoaded) == "") {
            $.IsDataCountLoaded = false;
        }

        if (!$.IsDataCountLoaded) {
            $.IsDataCountLoaded = true;
            $('#SavedIsDataCountLoaded').attr("value", $.IsDataCountLoaded);

        }
    });

    jQuery.LoadData = (function () {

        $.LoadDataCount();

        switch ($.SelectedTabId) {

            case "TAB_SHI": {
                $.LoadShipments();
                break;
            }

            case "TAB_INV": {
                $.LoadInvoices();
                break;
            }

            case "TAB_QUOTESREQUESTS": {
                $.LoadQuotesRequsts();
                break;
            }

            case "TAB_REPORTS": {
                $.LoadReports();
                break;
            }

        }
    });

    jQuery.SelectQuery = (function () {

        switch ($.SelectedTabId) {

            case "TAB_SHI": {

                $(".HyperLinkQuery_SHI").css({
                    "color": "#45494A",
                    "background": "transparent",
                });


                $("#" + $.SelectedQuery_SHI).css({
                    "color": "white",
                    "background": "url('HtmlHelpers/Images/Bars_Images/SelectedQuery.png') repeat-x",
                });

                var SelectedQueryLabel = "";
                switch ($.SelectedQuery_SHI) {

                    case "Query_PRG_SHI": {
                        SelectedQueryLabel = "In Progress";
                        break;
                    }

                    default: {
                        SelectedQueryLabel = "All Shipments";
                        break;
                    }
                }

                $("#ShipmentsQueryTitle").html(SelectedQueryLabel);
            }

            case "TAB_INV": {

                $(".HyperLinkQuery_INV").css({
                    "color": "#45494A",
                    "background": "transparent",
                });

                $("#" + $.SelectedQuery_INV).css({
                    "color": "white",
                    "background": "url('HtmlHelpers/Images/Bars_Images/SelectedQuery.png') repeat-x",
                });

                var SelectedQueryLabel = "";
                switch ($.SelectedQuery_INV) {

                    case "Query_PRG_INV": {
                        SelectedQueryLabel = "Unpaid Invoices";
                        break;
                    }

                    default: {
                        SelectedQueryLabel = "All Invoices";
                        break;
                    }
                }

                $("#InvoicesQueryTitle").html(SelectedQueryLabel);
            }


            case "TAB_QUOTESREQUESTS": {

                $(".HyperLinkQuery_QUOTESREQUESTS").css({
                    "color": "#45494A",
                    "background": "transparent",
                });

                $("#" + $.SelectedQuery_QuotesRequest).css({
                    "color": "white",
                    "background": "url('HtmlHelpers/Images/Bars_Images/SelectedQuery.png') repeat-x",
                });

                $("#QuotesRequestQueryTitle").html("All Quotes Requests");
            }
        }

    });

    jQuery.SelectFilter = (function (id) {

        var targetId = "#" + id;
        var sourceURL = $(targetId).css("background-image");
        var targetURL = sourceURL.replace("O.png", "S.png");

        if (sourceURL == targetURL) {

            targetURL = sourceURL.replace("N.png", "S.png");
        }

        var arr = id.split('_');
        var filterId = arr[0];
        var filterType = arr[1];

        switch (filterType) {

            case "Transport": {

                $("#All_Transport").css("background", "url('HtmlHelpers/Images/Filter_Images/All.N.png')");
                $("#A_Transport").css("background", "url('HtmlHelpers/Images/Filter_Images/Air.N.png')");
                $("#O_Transport").css("background", "url('HtmlHelpers/Images/Filter_Images/Ocean.N.png')");
                $("#I_Transport").css("background", "url('HtmlHelpers/Images/Filter_Images/Inland.N.png')");
                $(targetId).css("background", targetURL);

                $('#SavedTransportId_SHI').attr("value", filterId);
                $.SelectedTransportId_SHI = filterId;

                break;
            }

            case "Direction": {

                $("#All_Direction").css("background", "url('HtmlHelpers/Images/Filter_Images/All.N.png')");
                $("#E_Direction").css("background", "url('HtmlHelpers/Images/Filter_Images/Export.N.png')");
                $("#I_Direction").css("background", "url('HtmlHelpers/Images/Filter_Images/Import.N.png')");
                $("#R_Direction").css("background", "url('HtmlHelpers/Images/Filter_Images/Drop.N.png')");
                $("#D_Direction").css("background", "url('HtmlHelpers/Images/Filter_Images/Domestic.N.png')");
                $("#C_Direction").css("background", "url('HtmlHelpers/Images/Filter_Images/CustomsImport.N.png')");
                $(targetId).css("background", targetURL);

                $('#SavedDirectionId_SHI').attr("value", filterId);
                $.SelectedDirectionId_SHI = filterId;

                break;
            }

            case "ShipmentLevel": {

                $("#All_ShipmentLevel").css("background", "url('HtmlHelpers/Images/Filter_Images/All.N.png')");
                $("#D_ShipmentLevel").css("background", "url('HtmlHelpers/Images/Filter_Images/Filter_N.png')");
                $("#H_ShipmentLevel").css("background", "url('HtmlHelpers/Images/Filter_Images/Filter_N.png')");
                $("#D_ShipmentLevel").css("color", "#6E7172");
                $("#H_ShipmentLevel").css("color", "#6E7172");

                $(targetId).css("background", targetURL);
                $(targetId).css("color", "White");

                $('#SavedShipmentLevel').attr("value", filterId);
                $.SelectedShipmentLevel = filterId;
                break;
            }
        }
    });

    jQuery.SelectTab = (function (id) {

        switch (id) {

            case "TAB_SHI": {

                $.SearchText_SHI = $('#SavedSearchText_SHI').val();
                $.SelectedQuery_SHI = $('#SavedSelectedQuery_SHI').val();
                $.SelectedDirectionId_SHI = $('#SavedDirectionId_SHI').val();
                $.SelectedTransportId_SHI = $('#SavedTransportId_SHI').val();
                $.SelectedShipmentLevel = $('#SavedShipmentLevel').val();

                if ($.trim($.SelectedQuery_SHI) == "") {
                    $.SelectedQuery_SHI = "Query_PRG_SHI";
                    $('#SavedSelectedQuery_SHI').attr("value", $.SelectedQuery_SHI);
                }

                if ($.trim($.SelectedDirectionId_SHI) == "") {
                    $.SelectedDirectionId_SHI = "All";
                    $('#SavedDirectionId_SHI').attr("value", $.SelectedDirectionId_SHI);
                }

                if ($.trim($.SelectedTransportId_SHI) == "") {
                    $.SelectedTransportId_SHI = "All";
                    $('#SavedTransportId_SHI').attr("value", $.SelectedTransportId_SHI);
                }

                if ($.trim($.SelectedShipmentLevel) == "") {
                    $.SelectedShipmentLevel = "All";
                    $('#SavedShipmentLevel').attr("value", $.SelectedShipmentLevel);
                }

                $.SelectFilter($.SelectedDirectionId_SHI + "_Direction");
                $.SelectFilter($.SelectedTransportId_SHI + "_Transport");
                // $.SelectFilter($.SelectedShipmentLevel + "_ShipmentLevel");

                $.SelectQuery();

                if (!$.IsShipmentsDataLoaded) {

                    $.IsShipmentsDataLoaded = true;
                    $.LoadData();
                }

                break;
            }

            case "TAB_INV": {

                $.SearchText_INV = $('#SavedSearchText_INV').val();
                $.SelectedQuery_INV = $('#SavedSelectedQuery_INV').val();

                if ($.trim($.SelectedQuery_INV) == "") {
                    $.SelectedQuery_INV = "Query_PRG_INV";
                    $('#SavedSelectedQuery_INV').attr("value", $.SelectedQuery_INV);
                }

                $.SelectQuery();

                if (!$.IsInvoicesDataLoaded) {

                    $.IsInvoicesDataLoaded = true;
                    $.LoadData();
                }

                break;
            }

            case "TAB_QUOTESREQUESTS": {

                $.SearchText_QUOTESREQUESTS = $('#SavedSearchText_QUOTESREQUESTS').val();
                $.SelectedQuery_QuotesRequest = $('#SavedSelectedQuery_QUOTESREQUESTS').val();
                if ($.trim($.SelectedQuery_QUOTESREQUESTS) == "") {
                    $.SelectedQuery_QuotesRequest = "Query_PRG_QUOTESREQUESTS";
                    $('#SavedSelectedQuery_QUOTESREQUESTS').attr("value", $.SelectedQuery_QuotesRequest);
                }

                $.SelectQuery();

                if (!$.IsQuotesRequestsDataLoaded) {
                    $.IsQuotesRequestsDataLoaded = true;
                    $.LoadData();
                }

                break;
            }

            case "TAB_REPORTS": {

                if (!$.IsReportsDataLoaded) {
                    $.IsReportsDataLoaded = true;
                    $.LoadData();
                }

                break;
            }





        }

    });

    $("#mainTabsDiv").kendoTabStrip(
	{
	    animation: false,
	    select: function (e) {

	        var id = $(e.item).attr("id");
	        var clss = $("#" + id).attr('class');

	        var currentTarget = "#" + $.SelectedTabId + " div span .TabImage";
           
            var currentScr = $(currentTarget).attr('src');
            if (currentScr) {
                var currenttargetSRC = currentScr.replace('S.png', 'N.png');
                $(currentTarget).attr("src", currenttargetSRC);
            }
	        var target = "#" + id + " div span .TabImage";
            var scr = $(target).attr('src');
            if (scr) {
                var targetSRC = scr.replace('O.png', 'S.png');
                if (targetSRC == scr) {

                    targetSRC = scr.replace('N.png', 'S.png');
                }

                $(target).attr("src", targetSRC);
            }
	        $.SelectedTabId = $(e.item).attr("id");
	        $('#SavedSelectedTabId').attr("value", $.SelectedTabId);
	        $.SelectTab($.SelectedTabId);
	    }
	});

    jQuery.SearchTextChanged = (function (id) {

        switch (id) {

            case "SearchBox_SHI": {

                if ($("#SearchBox_SHI").val().length == 0 || $("#SearchBox_SHI").val() == $.watermark_SHI) {
                    $('#SearchDeleteButton_SHI').hide();
                }

                else {
                    $('#SearchDeleteButton_SHI').show();
                }

                $.SearchText_SHI = $("#SearchBox_SHI").val();
                $('#SavedSearchText_SHI').attr("value", $.SearchText_SHI);

                if ($.SearchTimer_SHI != null) {
                    clearTimeout($.SearchTimer_SHI);
                }

                $.SearchTimer_SHI = setTimeout(function () { $.LoadData() }, 500);

                break;
            }

            case "SearchBox_INV": {

                if ($("#SearchBox_INV").val().length == 0 || $("#SearchBox_INV").val() == $.watermark_INV) {
                    $('#SearchDeleteButton_INV').hide();
                }

                else {
                    $('#SearchDeleteButton_INV').show();
                }

                $.SearchText_INV = $("#SearchBox_INV").val();
                $('#SavedSearchText_INV').attr("value", $.SearchText_INV);

                if ($.SearchTimer_INV != null) {
                    clearTimeout($.SearchTimer_INV);
                }

                $.SearchTimer_INV = setTimeout(function () { $.LoadData() }, 500);

                break;
            }


            case "SearchBox_QUOTESREQUESTS": {

                if ($("#SearchBox_QUOTESREQUESTS").val().length == 0 || $("#SearchBox_QUOTESREQUESTS").val() == $.watermark_QUOTESREQUESTS) {
                    $('#SearchDeleteButton_QUOTESREQUESTS').hide();
                }

                else {
                    $('#SearchDeleteButton_QUOTESREQUESTS').show();
                }

                $.SearchText_QUOTESREQUESTS = $("#SearchBox_QUOTESREQUESTS").val();
                $('#SavedSearchText_QUOTESREQUESTS').attr("value", $.SearchText_QUOTESREQUESTS);

                if ($.SearchTimer_QUOTESREQUESTS != null) {
                    clearTimeout($.SearchTimer_QUOTESREQUESTS);
                }

                $.SearchTimer_QUOTESREQUESTS = setTimeout(function () { $.LoadData() }, 500);

                break;
            }

        }
    });

    $("#ShipmentsRefreshButton").click(function () {

        $("#ShipmentsListBox").html("");
        $.LoadShipments();
    });

    $("#InvoicesRefreshButton").click(function () {

        $("#InvoicesListBox").html("");
        $.LoadInvoices();
    });

    $("#QuotesRequestRefreshButton").click(function () {

        $("#QuotesRequestsListBox").html("");
        $.LoadQuotesRequsts();
    });




    $('.SearchDeleteButton').hide();
    $('#SearchBox_SHI').val($.watermark_SHI).addClass('watermark');
    $('#SearchBox_INV').val($.watermark_INV).addClass('watermark');
    $('#SearchBox_QUOTESREQUESTS').val($.watermark_QUOTESREQUESTS).addClass('watermark');


    $('.SearchBox').blur(function () {

        var id = $(this).attr('id');

        switch (id) {

            case "SearchBox_SHI": {

                if ($(this).val().length == 0) {
                    $(this).val($.watermark_SHI).addClass('watermark');
                    $('#SearchIcon_SHI').show();
                    $('#SearchDeleteButton_SHI').hide();
                }
            }

            case "SearchBox_INV": {

                if ($(this).val().length == 0) {
                    $(this).val($.watermark_INV).addClass('watermark');
                    $('#SearchIcon_INV').show();
                    $('#SearchDeleteButton_INV').hide();
                }
            }

            case "SearchBox_QUOTESREQUESTS": {

                if ($(this).val().length == 0) {
                    $(this).val($.watermark_QUOTESREQUESTS).addClass('watermark');
                    $('#SearchIcon_QUOTESREQUESTS').show();
                    $('#SearchDeleteButton_QUOTESREQUESTS').hide();
                }
            }


        }
    });
    $('.SearchBox').focus(function () {

        var id = $(this).attr('id');

        switch (id) {

            case "SearchBox_SHI": {

                if ($(this).val() == $.watermark_SHI) {
                    $(this).val('').removeClass('watermark');
                    $('#SearchIcon_SHI').hide();
                }
            }

            case "SearchBox_INV": {

                if ($(this).val() == $.watermark_INV) {
                    $(this).val('').removeClass('watermark');
                    $('#SearchIcon_INV').hide();
                }
            }

            case "SearchBox_QUOTESREQUESTS": {

                if ($(this).val() == $.watermark_QUOTESREQUESTS) {
                    $(this).val('').removeClass('watermark');
                    $('#SearchIcon_QUOTESREQUESTS').hide();
                }
            }

        }
    });
    $('.SearchBox').keyup(function () {
        $.SearchTextChanged($(this).attr('id'));
    });
    $(".SearchDeleteButton").click(function () {

        var id = $(this).attr('id');

        switch (id) {

            case "SearchDeleteButton_SHI": {
                $('#SearchIcon_SHI').show();
                $('#SearchBox_SHI').attr("value", "");
                $('#SearchBox_SHI').val($.watermark_SHI).addClass('watermark');
                $.SearchTextChanged("SearchBox_SHI");
            }

            case "SearchDeleteButton_INV": {
                $('#SearchIcon_INV').show();
                $('#SearchBox_INV').attr("value", "");
                $('#SearchBox_INV').val($.watermark_INV).addClass('watermark');
                $.SearchTextChanged("SearchBox_INV");
            }

            case "SearchDeleteButton_QUOTESREQUESTS": {
                $('#SearchIcon_QUOTESREQUESTS').show();
                $('#SearchBox_QUOTESREQUESTS').attr("value", "");
                $('#SearchBox_QUOTESREQUESTS').val($.watermark_QUOTESREQUESTS).addClass('watermark');
                $.SearchTextChanged("SearchBox_QUOTESREQUESTS");
            }


        }
    });

    $(".FilterListItem").mouseenter(function () {
        var id = $(this).attr('id');
        var targetId = "#" + id;
        var sourceURL = $(targetId).css("background-image");
        var targetURL = sourceURL.replace("N.png", "O.png");

        var arr = id.split('_');
        var filterId = arr[0];
        var filterType = arr[1];

        switch (filterType) {

            case "Transport": {
                if ($.SelectedTransportId_SHI != filterId) {
                    $(targetId).css("background", targetURL);
                }
                break;
            }

            case "Direction": {
                if ($.SelectedDirectionId_SHI != filterId) {
                    $(targetId).css("background", targetURL);
                }

                break;
            }

            case "ShipmentLevel": {
                if ($.SelectedShipmentLevel != filterId) {
                    $(targetId).css("background", targetURL);
                    $(targetId).css("color", "#1782B8");
                }

                break;
            }
        }
    });
    $(".FilterListItem").mouseleave(function () {
        var id = $(this).attr('id');
        var targetId = "#" + id;
        var sourceURL = $(targetId).css("background-image");
        var targetURL = sourceURL.replace("O.png", "N.png");

        var arr = id.split('_');
        var filterId = arr[0];
        var filterType = arr[1];

        switch (filterType) {

            case "Transport": {
                if ($.SelectedTransportId_SHI != filterId) {
                    $(targetId).css("background", targetURL);
                }
                break;
            }

            case "Direction": {
                if ($.SelectedDirectionId_SHI != filterId) {
                    $(targetId).css("background", targetURL);
                }
                break;
            }

            case "ShipmentLevel": {
                if ($.SelectedShipmentLevel != filterId) {
                    $(targetId).css("background", targetURL);
                    $(targetId).css("color", "#6E7172");
                }

                break;
            }
        }
    });
    $(".FilterListItem").click(function () {
        $.SelectFilter($(this).attr('id'));
        $.LoadData();
    });

    $(".k-tabstrip .k-item").mouseenter(function () {


        var id = $(this).attr('id');
        var clss = $(this).attr('class');

        if ($.trim(clss).indexOf("k-state-active") == -1) {

            var target = "#" + id + " div span .TabImage";
            var scr = $(target).attr('src');

            if (scr) {
                var targetSRC = scr.replace('N.png', 'O.png');

                $(target).attr("src", targetSRC);
            }
        }
    });
    $(".k-tabstrip .k-item").mouseleave(function () {

        var id = $(this).attr('id');
        var clss = $(this).attr('class');

        if ($.trim(clss).indexOf("k-state-active") == -1) {

            var target = "#" + id + " div span .TabImage";
            var scr = $(target).attr('src');
            if (scr) {
                var targetSRC = scr.replace('O.png', 'N.png');
                $(target).attr("src", targetSRC);
            }
        }
    });

    $(".HyperLinkQuery_SHI").mouseenter(function () {
        if ($(this).attr('id') != $.SelectedQuery_SHI) {
            $(this).css({ "color": "black", "background": "url('HtmlHelpers/Images/Bars_Images/tab-over.png') repeat-x" });
        }
    });
    $(".HyperLinkQuery_SHI").mouseleave(function () {
        if ($(this).attr('id') != $.SelectedQuery_SHI) {
            $(this).css({ "color": "#45494A", "background": "transparent" });
        }
    });
    $(".HyperLinkQuery_SHI").click(function () {
        if ($(this).attr('id') != $.SelectedQuery_SHI) {
            $.SelectedQuery_SHI = $(this).attr('id');
            $("#SavedSelectedQuery_SHI").attr("value", $.SelectedQuery_SHI);
            $.SelectQuery();
            $.LoadData();
        }
    });

    $(".HyperLinkQuery_INV").mouseenter(function () {
        if ($(this).attr('id') != $.SelectedQuery_INV) {
            $(this).css({ "color": "black", "background": "url('HtmlHelpers/Images/Bars_Images/tab-over.png') repeat-x" });
        }
    });
    $(".HyperLinkQuery_INV").mouseleave(function () {
        if ($(this).attr('id') != $.SelectedQuery_INV) {
            $(this).css({ "color": "#45494A", "background": "transparent" });
        }
    });
    $(".HyperLinkQuery_INV").click(function () {
        if ($(this).attr('id') != $.SelectedQuery_INV) {
            $.SelectedQuery_INV = $(this).attr('id');
            $("#SavedSelectedQuery_INV").attr("value", $.SelectedQuery_INV);
            $.SelectQuery();
            $.LoadData();
        }
    });


    $(".HyperLinkQuery_QUOTESREQUESTS").mouseenter(function () {
        if ($(this).attr('id') != $.SelectedQuery_QuotesRequest) {
            $(this).css({ "color": "black", "background": "url('HtmlHelpers/Images/Bars_Images/tab-over.png') repeat-x" });
        }
    });
    $(".HyperLinkQuery_QUOTESREQUESTS").mouseleave(function () {
        if ($(this).attr('id') != $.SelectedQuery_QuotesRequest) {
            $(this).css({ "color": "#45494A", "background": "transparent" });
        }
    });



    $(".HyperLinkQuery_QUOTESREQUESTS").click(function () {
        if ($(this).attr('id') != $.SelectedQuery_QuotesRequest) {
            $.SelectedQuery_QuotesRequest = $(this).attr('id');
            $("#SavedSelectedQuery_QUOTESREQUESTS").attr("value", $.SelectedQuery_QuotesRequest);
            $.SelectQuery();
            $.LoadData();
        }
    });








    $("#SignOutButton").click(function () {


        var url = "api/Authentication/?userEmail=" + $.CurrentEmail;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',
            headers: {
                'Token': $.Token
            },

            success: function (result) {
         
                window.localStorage.setItem("Token", "");
                window.localStorage.setItem("CardId", "");

                document.location.href = "../../Login.aspx";

            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#error").text("errror");
                $("#error").show();
                alert("logout failed!");
                var errorMessage = '';
            }
        });


    });

    $(window).resize(function () {
        $.ResizePage(125);
    });

    $(document).ready(function () {
        $("#TAB_INV").hide();
        $("#TAB_QUOTESREQUESTS").hide();

        
        $.ResizePage(130);

        var userdata = null;
        $.Token = $("#TokenInput").val();
        var linkQuery = $("#LoginInput").val();
        var linkParameters = null;

        if ($.trim($.Token) == "") {
            var link = $(location).attr('href');
            var linkArray = link.split('=')
            linkQuery = linkArray[1];
        }

        linkParameters = linkQuery.split(':')

        $.CurrentEmail = linkParameters[0];
        $.CurrentTenant = linkParameters[1];
        $.CurrentCardId = linkParameters[2];
        $.CurrentCardType = linkParameters[3];
        $.IsBrandingEnabled = linkParameters[4];
        
        if ($.IsBrandingEnabled == "true" || $.IsBrandingEnabled == "True") {
            $(".PoweredArea").hide();
        }

        $.GetCompanyLogo();
        $.GetLogginData();
    });

}(jQuery));


