
(function (jQuery) {

    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;
    jQuery.CurrentCardType = null;
    jQuery.CurrentEntityId = null;
    jQuery.CurrentEntityPM = null;
    jQuery.CurrentEmail = null;
    jQuery.IsExternalURL = false;
    jQuery.IsBrandingEnabled = "";
    jQuery.TenantDateTimeFormat = null;

    var IsTabSelected_DET = false;
    var IsTabSelected_PAY = false;

    jQuery.ResizePage = (function (myFixedHeight) {
        var minHeight = 400;
        var _height = $(window).height() - myFixedHeight;

        if (_height < minHeight) {
            _height = minHeight;
        }

        $(".pageContent").css({ height: _height });
        $(".tabPage").css({ height: _height - 35 });
        $(".LinesScrollViewer").css({ height: _height - 40 - 100 });
        $(".ScrollViewer").css({ height: _height - 40 });
    });

    jQuery.SetTabsEnabled = (function (isEnabled) {

        var tabControl = $("#mainTabsDiv").data("kendoTabStrip");

        tabControl.enable($("#TAB_DET"), isEnabled);
        tabControl.enable($("#TAB_PAY"), isEnabled);
    });

    jQuery.GetCompanyLogo = (function () {

        var url = "../api/commondata/?companyId=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                //jQuery("#companyLogo").attr('src', result);
                var img = new Image();
                img.onload = function () {
                    var width = this.width > 200 ? "200px" : (this.width + "px");
                    jQuery("#companyLogo").attr('src', result);
                    jQuery("#companyLogo").css('width', width);
                    jQuery("#companyLogoArea").css('width', width);
                }
                img.src = result;
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
                $("#MemberText").html(result.ContactName);
                $("#MemberCardText").html(" (" + result.CardName + ")");

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

        $("#DetailsPageBusyIndicator").show();

        var url = "../api/InvoicesData?invoiceId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (invoicePM) {

                $.CurrentEntityPM = invoicePM;
                $.CurrentEntityId = invoicePM.Id;

                ko.applyBindings(BuildInvoiceBackAreaViewModel(invoicePM), document.getElementById("BackArea"));
                ko.applyBindings(BuildInvoiceHeaderViewModel(invoicePM, $.TenantDateTimeFormat), document.getElementById("EntityHeaderArea"));
                ko.applyBindings(BuildInvoiceSummaryViewModel(invoicePM), document.getElementById("InvoiceSummary"));

                if (invoicePM.InvoiceLines.length > 0) {

                    var LinesGridColumns = [];
                    var LinesGridDataSource = [];

                    LinesGridColumns.push({ title: "Description", field: "Description" });
                    LinesGridColumns.push({ title: "UOM", field: "MeasurementCode", width: 60 });
                    LinesGridColumns.push({ title: "Quantity", field: "Quantity", width: 100, template: "<div class='k-numeric'>#= Quantity #</div>" });
                    LinesGridColumns.push({ title: "Unit Price", field: "UnitPrice", width: 100, template: "<div class='k-numeric'>#= UnitPrice #</div>" });
                    LinesGridColumns.push({ title: "Amount", field: "Amount", width: 120, template: "<div class='k-numeric'>#= Amount #</div>" });
                    LinesGridColumns.push({ title: "VAT Type", field: "VATType", width: 130 });

                    if (invoicePM.LocalCurrencyId != invoicePM.InvoiceCurrencyId || invoicePM.IsShowAmountLocalCurrencyColumnInSharedLogistics == false) {

                        LinesGridColumns.push({ title: "Amount (" + invoicePM.InvoiceCurrencyCode + ")", field: "AmountInvoice", width: 120, template: "<div class='k-numeric'>#= AmountInvoice #</div>" });
                    }

                    if (invoicePM.IsShowAmountLocalCurrencyColumnInSharedLogistics == true) {
                        LinesGridColumns.push({ title: "Amount (" + invoicePM.LocalCurrencyCode + ")", field: "AmountLocal", width: 120, template: "<div class='k-numeric'>#= AmountLocal #</div>" });
                    }

  

                    $.each(invoicePM.InvoiceLines, function (index, item) {

                        var varQuantity = $.trim(item.Quantity) == "" ? 0 : item.Quantity;
                        var varUnitPrice = $.trim(item.UnitPrice) == "" ? 0 : item.UnitPrice;
                        var varAmount = $.trim(item.ForiegnCurrencyAmount) == "" ? 0 : item.ForiegnCurrencyAmount;
                        var varAmountLocal = $.trim(item.LocalCurrencyAmount) == "" ? 0 : item.LocalCurrencyAmount;
                        var varAmountInvoice = $.trim(item.InvoiceCurrencyAmount) == "" ? 0 : item.InvoiceCurrencyAmount;

                        var varVatType = $.trim(item.VatPercentage) == "" ? ("No VAT for this date") : (item.VatTypeName + " (" + item.VatPercentage + "%)");

                        LinesGridDataSource.push({
                            Description: item.Description,
                            MeasurementCode: item.MeasurementCode,
                            Quantity: varQuantity.toFixed(2),
                            UnitPrice: varUnitPrice.toFixed(3),
                            Amount: varAmount.toFixed(2) + " (" + item.ForiegnCurrencyCode + ")",
                            VATType: varVatType,
                            AmountInvoice: varAmountInvoice.toFixed(2),
                            AmountLocal: varAmountLocal.toFixed(2),
                        });

                    });

                    $("#InvoiceLinesGrid").kendoGrid(
                    {
                        columns: LinesGridColumns,
                        dataSource: {
                            data: LinesGridDataSource
                        }
                    });
                }

                $.SetTabsEnabled(true);
                $(".ShowOnDataControl").show();
                $("#DetailsPageBusyIndicator").hide();
                $.SendContactActivity($.CurrentEmail, "Invoice", "Invoice Display", $.CurrentTenant, $.CurrentCardId);
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $.SetTabsEnabled(true);
                $("#DetailsPageBusyIndicator").hide();
            }
        });

    });

    jQuery.GetInvoicePayments = (function () {

        $("#PaymentsPageBusyIndicator").show();

        var url = "../api/InvoicesData?arInvoiceId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (paymentsList) {

                ko.applyBindings(BuildPaymentsTabPageViewModel($.CurrentEntityPM, paymentsList), document.getElementById("PaymentsTabPageControl"));

                $("#PaymentsPageBusyIndicator").hide();
                $.SendContactActivity($.CurrentEmail, "Invoice", "Payments Display", $.CurrentTenant, $.CurrentCardId);
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#PaymentsPageBusyIndicator").hide();
            }
        });
    });

    $("#mainTabsDiv").kendoTabStrip(
    {
        animation: false,

        select: function (e) {

            var tabId = $(e.item).attr("id");

            switch (tabId) {

                case "TAB_DET": {

                    break;
                }

                case "TAB_PAY": {

                    if (!IsTabSelected_PAY) {

                        IsTabSelected_PAY = true;
                        $.GetInvoicePayments();
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

        $.ResizePage(210);
        $.SetTabsEnabled(false);

        $(".ShowOnDataControl").hide();

        var hash = $(location).attr('href');
        var dataParam = hash.split('=');
        var linkQuery = dataParam[1];
        var linkParameters = null;
        if (linkQuery && linkQuery.indexOf('%3A') > -1) {
            linkParameters = linkQuery.split('%3A')
        }
        else {
            linkParameters = linkQuery.split(':')
        }

        $.CurrentEntityId = linkParameters[0];
        $.CurrentCardId = linkParameters[1];
        $.CurrentTenant = linkParameters[2];
        $.CurrentEmail = linkParameters[3];
        $.CurrentCardType = linkParameters[4];
        $.IsBrandingEnabled = linkParameters[5];

        if ($.IsBrandingEnabled == "true" || $.IsBrandingEnabled == "True") {
            $(".PoweredArea").hide();
        }

        $.GetLogginData();
        $.GetCompanyLogo();
    });

}(jQuery));