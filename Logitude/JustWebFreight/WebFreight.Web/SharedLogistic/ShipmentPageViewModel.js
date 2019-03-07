
(function (jQuery) {

    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;
    jQuery.CurrentCardType = null;
    jQuery.CurrentEntityId = null;
    jQuery.CurrentEntityPM = null;
    jQuery.CurrentEmail = null;
    jQuery.IsExternalURL = false;
    jQuery.CurrentEntityKey = null;
    jQuery.IsBrandingEnabled = "";
    jQuery.TenantDateTimeFormat = null;
    jQuery.IsMoneyTabEnabled = false;

    var IsTabSelected_PAR = false;
    var IsTabSelected_PAC = false;
    var IsTabSelected_MON = false;
    var IsTabSelected_DOC = false;
    var IsTabSelected_EVE = false;

    jQuery.ResizePage = (function (myFixedHeight) {
        var minHeight = 400;
        var _height = $(window).height() - myFixedHeight;

        if (_height < minHeight) {
            _height = minHeight;
        }
        
        $(".pageContent").css({ height: _height });
        $(".tabPage").css({ height: _height - 35 });
        $("#PackagesContainer").css({ height: _height - 40 - 100 });
        $("#DocumentsContainer").css({ height: _height - 40 });
        $(".ScrollViewer").css({ height: _height - 40 });
    });

    jQuery.SetTabsEnabled = (function (isEnabled) {

        var tabControl = $("#mainTabsDiv").data("kendoTabStrip");

        tabControl.enable($("#TAB_PAR"), isEnabled);
        tabControl.enable($("#TAB_PAC"), isEnabled);
        tabControl.enable($("#TAB_MON"), isEnabled);
        tabControl.enable($("#TAB_DOC"), isEnabled);
        tabControl.enable($("#TAB_EVE"), isEnabled);
    });

    jQuery.SetTabsHidden = (function (isEnabled) {
        if(isEnabled){
            $("#TAB_MON").show();
        }   

        else {
            $("#TAB_MON").hide();
        }
    });

    jQuery.GetCompanyLogo = (function () {

        var url = "../api/commondata/?companyId=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',
            
            success: function (result) {                
                jQuery("#companyLogo").attr('src', result);
            },

            error: function (jqXHR, textStatus, errorThrown) {                
                $.CheckUserException(jqXHR);
            }
        });
    });

    jQuery.GetLogginData = (function () {

        var url = "../api/commondata/?email=" + $.CurrentEmail + "&tenant=" + $.CurrentTenant + "&cardId=" + $.CurrentCardId;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $("#CompanyText").html(result.TenantCompany);

                if (!$.IsExternalURL) {
                    $("#MemberText").html(result.ContactName);
                    $("#MemberCardText").html(" (" + result.CardName + ")");
                }

                $.TenantDateTimeFormat = result.TenantDateTimeFormat;
                $.GetSingleEntityPM();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $.GetSingleEntityPM();
            }
        });

    });

    jQuery.GetSingleEntityPM = (function () {

        $(".ShowOnDataControl").hide();
        $("#RoutingsPageBusyIndicator").show();

        if ($.IsExternalURL) {
            var url = "../api/shipments/getsinglepmbykey/" + $.CurrentEntityKey + "/" + $.CurrentEntityId + "/" + $.CurrentTenant;
        }

        else {
            var url = "../api/shipments/getsinglepm/" + $.CurrentEntityId + "/" + $.CurrentTenant;
        }

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (shipmentPM) {

                if (shipmentPM) {
                    $.CurrentEntityPM = shipmentPM;
                    $.CurrentEntityId = shipmentPM.Id;
                    $.IsMoneyTabEnabled = shipmentPM.IsSharedLogisticsMoneyTabEnabled;
                    ko.applyBindings(BuildShipmentBackAreaViewModel(shipmentPM, "../"), document.getElementById("BackArea"));
                    ko.applyBindings(BuildShipmentHeaderViewModel(shipmentPM, $.TenantDateTimeFormat, "../"), document.getElementById("EntityHeaderArea"));

                    BuildRoutingLegs(shipmentPM, $.TenantDateTimeFormat);

                    $.SetTabsEnabled(true);
                    $.SetTabsHidden($.IsMoneyTabEnabled);

                    $(".ShowOnDataControl").show();
                    $("#RoutingsPageBusyIndicator").hide();
                    $.SendContactActivity($.CurrentEmail, "Shipment", "Shipment Display", $.CurrentTenant, $.CurrentCardId);
                }

                else {
                    if ($.IsExternalURL) {
                        $("#Container").hide();
                        $("#InvalidKeyArea").show();
                    }
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $.SetTabsEnabled(true);
                $("#RoutingsPageBusyIndicator").hide();

                if ($.IsExternalURL) {

                    $("#Container").hide();
                    $("#InvalidKeyArea").show();
                }
            }
        });
    });

    jQuery.GetShipmentPartners = (function () {

        $("#PartnersPageBusyIndicator").show();

        if ($.IsExternalURL) {
            var url = "../api/shipments?securitykey=" + $.CurrentEntityKey + "&shipmentId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;
        }

        else {
            var url = "../api/shipments?shipmentId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;
        }


        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.SendContactActivity($.CurrentEmail, "Shipment", "Partners Display", $.CurrentTenant, $.CurrentCardId);

                $("#PartnersListBox").kendoListView(
                {
                    dataSource: { data: result },
                    template: kendo.template($("#PartnerListBoxItemDataTemplate").html())
                });

                $("#PartnersPageBusyIndicator").hide();
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

    jQuery.GetShipmentARMoney = (function () {

        $("#MoneyPageBusyIndicator").show();

        var url = "../api/InvoicesData?shipmentId=" + $.CurrentEntityId + "&cardId=" + $.CurrentCardId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.SendContactActivity($.CurrentEmail, "Shipment", "Money Display", $.CurrentTenant, $.CurrentCardId);

                if (result.ARInvoices.length > 0) {

                    $("#ChargesTitle").show();
                    $("#InvoicesTitle").show();

                    var InvoicesGridColumns = [];
                    var InvoicesGridDataSource = [];

                    InvoicesGridColumns.push({ title: "Invoice #", field: "InvoiceNumber" });
                    InvoicesGridColumns.push({ title: "Amount", field: "Amount", template: "<div class='k-numeric'>#= Amount #</div>" });
                    InvoicesGridColumns.push({ title: "Amount Due", field: "AmountDue", template: "<div class='k-numeric'>#= AmountDue #</div>" });
                    InvoicesGridColumns.push({ title: "Due Date", field: "DueDate" });

                    $.each(result.ARInvoices, function (index, item) {

                        var amount = $.trim(item.AmountInInvoiceCurrency) == "" ? 0 : item.AmountInInvoiceCurrency;
                        var amountDue = $.trim(item.AmountDue) == "" ? 0 : item.AmountDue;

                        InvoicesGridDataSource.push({
                            InvoiceNumber: item.InvoiceNumber,
                            Amount: amount.toFixed(3) + " (" + item.InvoiceCurrencyCode + ")",
                            AmountDue: amountDue.toFixed(3) + " (" + item.InvoiceCurrencyCode + ")",
                            DueDate: $.trim(item.DueDate) == "" ? "" : $.Convert.ToShortDate(item.DueDate),
                        });
                    });

                    $("#InvoicesGrid").kendoGrid(
                    {
                        columns: InvoicesGridColumns,
                        dataSource: {
                            data: InvoicesGridDataSource
                        }
                    });

                    var ChargesGridColumns = [];
                    var ChargesGridDataSource = [];

                    ChargesGridColumns.push({ title: "Description", field: "Description" });
                    ChargesGridColumns.push({ title: "Invoice Amount", field: "InvoiceAmount", template: "<div class='k-numeric'>#= InvoiceAmount #</div>" });
                    ChargesGridColumns.push({ title: "Local Amount", field: "LocalAmount", template: "<div class='k-numeric'>#= LocalAmount #</div>" });

                    $.each(result.ARCharges, function (index, item) {

                        var invoiceAmount = $.trim(item.AmountInInvoiceCurrency) == "" ? 0 : item.AmountInInvoiceCurrency;
                        var localAmount = $.trim(item.AmountInLocalCurrency) == "" ? 0 : item.AmountInLocalCurrency;

                        ChargesGridDataSource.push({
                            Description: item.Description,
                            InvoiceAmount: invoiceAmount.toFixed(3) + " (" + item.InvoiceCurrencyCode + ")",
                            LocalAmount: localAmount.toFixed(3) + " (" + item.LocalCurrencyCode + ")",
                        });
                    });


                    $("#ChargesGrid").kendoGrid(
                    {
                        columns: ChargesGridColumns,
                        dataSource: {
                            data: ChargesGridDataSource
                        }
                    });

                    $("#MoneyPageBusyIndicator").hide();
                }

                else {

                    $("#ChargesGrid").css({
                        "font-family": "Arial",
                        "color": "#8F9293",
                        "font-size": "22px",
                        "margin-top": "20px",
                    });

                    $("#ChargesGrid").html("No Invoices");
                    $("#MoneyPageBusyIndicator").hide();
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#MoneyPageBusyIndicator").hide();
            }
        });
    });

    jQuery.GetShipmentDocuments = (function () {

        $("#DocumentsPageBusyIndicator").show();

        var url = "../api/DocumentsData?entityId=" + $.CurrentEntityId + "&partnerType=" + $.CurrentCardType + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.SendContactActivity($.CurrentEmail, "Shipment", "Documents Display", $.CurrentTenant, $.CurrentCardId);

                if (result.length > 0) {

                    ko.applyBindings(BuildDocumentsTabPageViewModel(result, "../", false), document.getElementById("DocumentsTabPageControl"));

                    //$("#DocumentsListBox").kendoListView(
                    //{
                    //    dataSource: { data: result },
                    //    template: kendo.template($("#DocumentListBoxItemDataTemplate").html())
                    //});
                }

                else {

                    $("#DocumentsTabPageControl").css({
                        "font-family": "Arial",
                        "color": "#8F9293",
                        "font-size": "22px",
                        "margin-top": "20px",
                    });

                    $("#DocumentsTabPageControl").html("No Documents");
                }

                $("#DocumentsPageBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#DocumentsPageBusyIndicator").hide();
            }
        });

    });

    jQuery.GetShipmentEvents = (function () {

        $("#EventsPageBusyIndicator").show();

        var url = "../api/CommonData?entityId=" + $.CurrentEntityId + "&objectTableName=Shipment" + "&partnerType=" + $.CurrentCardType + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.SendContactActivity($.CurrentEmail, "Shipment", "Events Display", $.CurrentTenant, $.CurrentCardId);

                if (result.length > 0) {
                    $("#EventsListBox").kendoListView(
                    {
                        dataSource: { data: BuildEventList(result, $.TenantDateTimeFormat) },
                        template: kendo.template($("#EventListBoxItemDataTemplate").html())
                    });
                }

                else {
                    $("#EventsListBox").css({
                        "font-family": "Arial",
                        "color": "#8F9293",
                        "font-size": "22px",
                        "margin-top": "20px",
                    });

                    $("#EventsListBox").html("No Events");
                }

                $("#EventsPageBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#EventsPageBusyIndicator").hide();
            }
        });

    });

    $("#ChargesTitle").hide();
    $("#InvoicesTitle").hide();

    $("#mainTabsDiv").kendoTabStrip(
    {
        animation: false,
        select: function (e) {

            var tabId = $(e.item).attr("id");

            switch (tabId) {

                case "TAB_ROU": {

                    break;
                }

                case "TAB_PAR": {

                    if (!IsTabSelected_PAR) {

                        IsTabSelected_PAR = true;
                        $.GetShipmentPartners();
                    }

                    break;
                }

                case "TAB_PAC": {

                    if (!IsTabSelected_PAC) {

                        IsTabSelected_PAC = true;
                        $("#EstimatePackagesControl").hide();
                        $("#PackagesPageBusyIndicator").show();
                        ko.applyBindings(BuildPackagesTabPageViewModel($.CurrentEntityPM), document.getElementById("PackagesTabPageControl"));

                        $.SendContactActivity($.CurrentEmail, "Shipment", "Cargo info Display", $.CurrentTenant, $.CurrentCardId);
                    }

                    break;
                }

                case "TAB_MON": {

                    if (!IsTabSelected_MON) {

                        IsTabSelected_MON = true;

                        if ($.IsExternalURL) {

                            $("#ChargesGrid").css({
                                "font-family": "Arial",
                                "color": "#8F9293",
                                "font-size": "16px",
                                "margin-top": "20px",
                            });

                            $("#ChargesGrid").html("Money information is only available for logged-in users");
                        }

                        else {
                            $.GetShipmentARMoney();
                        }
                    }

                    break;
                }

                case "TAB_DOC": {

                    if (!IsTabSelected_DOC) {

                        IsTabSelected_DOC = true;

                        if ($.IsExternalURL) {

                            $("#DocumentsTabPageControl").css({
                                "font-family": "Arial",
                                "color": "#8F9293",
                                "font-size": "16px",
                                "margin-top": "20px",
                            });

                            $("#DocumentsTabPageControl").html("Documents information is only available for logged-in users");
                        }

                        else {
                            $.GetShipmentDocuments();
                        }
                    }

                    break;
                }

                case "TAB_EVE": {

                    if (!IsTabSelected_EVE) {

                        IsTabSelected_EVE = true;

                        if ($.IsExternalURL) {

                            $("#EventsListBox").css({
                                "font-family": "Arial",
                                "color": "#8F9293",
                                "font-size": "16px",
                                "margin-top": "20px",
                            });

                            $("#EventsListBox").html("Events information is only available for logged-in users");
                        }

                        else {
                            $.GetShipmentEvents();
                        }
                    }

                    break;
                }
            }
        }
    });
    $("#BackButton").click(function () {
        parent.history.back();
        return false;
    });

    $("#SignOutButton").click(function () {

       
        var url = "../api/Authentication/?userEmail=" + $.CurrentEmail;
        
        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {
                window.localStorage.setItem("Token", "");
                window.localStorage.setItem("CardId", "");
                document.location.href = "../Login.aspx";

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

        $.ResizePage(210);
    });

    $(document).ready(function () {
        $("#TAB_MON").hide();

        $.ResizePage(210);
        $.SetTabsEnabled(false);

        var link = $(location).attr('href');
        var linkArray = link.split('=')
        var linkQuery = linkArray[1];
        var linkParameters = null;

        if (linkQuery && linkQuery.indexOf('%3A') > -1)
        {
            linkParameters = linkQuery.split('%3A')
        }

        else
        {
             linkParameters = linkQuery.split(':')
        }
    
        if ($.trim(link).indexOf("securitykey") != -1) {
            $.CurrentEntityKey = linkParameters[0];
            $.CurrentEntityId = linkParameters[1];
            $.CurrentTenant = linkParameters[2];
            $.IsBrandingEnabled = linkParameters[3];
            $.IsExternalURL = true;
          
            if ($.IsBrandingEnabled == "true" || $.IsBrandingEnabled == "True") {
               
                $("#PoweredArea2").hide();
                $("#PoweredArea1").hide();
            }

            $("#SignOutButton").hide();
            $("#userInfo").hide();
            $("#BackButton").hide();            

            $.GetLogginData();
            $.GetCompanyLogo();            
        }

        else {

            $.CurrentEntityId = linkParameters[0];
            $.CurrentCardId = linkParameters[1];
            $.CurrentTenant = linkParameters[2];
            $.CurrentEmail = linkParameters[3];
            $.CurrentCardType = linkParameters[4];

            $.IsBrandingEnabled = linkParameters[5];

            if ($.IsBrandingEnabled == "true" || $.IsBrandingEnabled == "True") {               
                //$(".PoweredArea").hide();
            }

            $.GetLogginData();
            $.GetCompanyLogo();
        }

    });

}(jQuery));