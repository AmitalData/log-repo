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
    jQuery.DisplayDocumentsAndEvents = false;
    jQuery.IsDocumentsApprovalRequried = false;
    jQuery.DocumentsApprovalName = "";
    jQuery.DownloadAll = false;
    jQuery.DocumentUrl = "";

    jQuery.ResizePage = (function (myFixedHeight) {
        var minHeight = 400;
        var _height = $(window).height() - myFixedHeight;

        if (_height < minHeight) {
            _height = minHeight;
        }

        $(".pageContent").css({ height: _height });
        $(".tabPage").css({ height: _height - 35 });
        $("#DocumentsContainer").css({ height: _height - 40 });
        $(".ScrollViewer").css({ height: _height - 40 });
    });

    jQuery.GetCompanyLogo = (function () {

        var url = "../api/commondata/?companyId=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {
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

                if (!$.IsExternalURL) {
                    $("#MemberText").html(result.ContactName);
                    $("#MemberCardText").html(" (" + result.CardName + ")");
                }

                $.TenantDateTimeFormat = result.TenantDateTimeFormat;
                $.DisplayDocumentsAndEvents = result.DisplayDocumentsAndEvents;

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
        $("#DocumentsPageBusyIndicator").show();

        if ($.IsExternalURL) {
            var url = "../api/shipments/getsinglepmbykey/" + $.CurrentEntityKey + "/" + $.CurrentEntityId + "/" + $.CurrentTenant;
        }

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (shipmentPM) {

                if (shipmentPM) {
                    $.CurrentEntityPM = shipmentPM;
                    $.CurrentEntityId = shipmentPM.Id;
                    ko.applyBindings(BuildShipmentBackAreaViewModel(shipmentPM, "../"), document.getElementById("BackArea"));
                    ko.applyBindings(BuildShipmentHeaderViewModel(shipmentPM, $.TenantDateTimeFormat, "../"), document.getElementById("EntityHeaderArea"));

                    BuildRoutingLegs(shipmentPM, $.TenantDateTimeFormat);

                    $.SetTabsEnabled(true);

                    $(".ShowOnDataControl").show();
                    $("#DocumentsPageBusyIndicator").hide();
                    if ($.IsExternalURL) {

                        if ($.DisplayDocumentsAndEvents) {
                            $.GetShipmentDocuments();
                        }

                        else {
                            $("#DocumentsTabPageControl").css({
                                "font-family": "Arial",
                                "color": "#8F9293",
                                "font-size": "16px",
                                "margin-top": "20px",
                            });

                            $("#DocumentsTabPageControl").html("Documents information is only available for logged-in users");
                        }
                    }

                    else {
                        $.GetShipmentDocuments();
                    }
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
                $("#DocumentsPageBusyIndicator").hide();

                if ($.IsExternalURL) {

                    $("#Container").hide();
                    $("#InvalidKeyArea").show();
                }
            }
        });
    });

    jQuery.GetShipmentDocuments = (function () {

        $("#DocumentsPageBusyIndicator").show();

        var url = null;

        if ($.IsExternalURL) {
            url = "../api/DocumentsData?securitykey=" + $.CurrentEntityKey + "&entityId=" + $.CurrentEntityId + "&partnerType=" + $.CurrentCardType + "&tenant=" + $.CurrentTenant;
        }

        else {
            url = "../api/DocumentsData?entityId=" + $.CurrentEntityId + "&partnerType=" + $.CurrentCardType + "&tenant=" + $.CurrentTenant;
        }

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.SendContactActivity($.CurrentEmail, "Shipment", "Documents Display", $.CurrentTenant, $.CurrentCardId);

                if (result.length > 0) {

                    ko.applyBindings(BuildDocumentsTabPageViewModel(result, "../", false), document.getElementById("DocumentsTabPageControl"));
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

    $(window).resize(function () {
        $.ResizePage(210);
    });

    $(document).ready(function () {
        $.ResizePage(210);

        var link = $(location).attr('href');
        var linkArray = link.split('=')
        var linkQuery = linkArray[1];
        var linkParameters = null;

        if (linkQuery && linkQuery.indexOf('%3A') > -1) {
            linkParameters = linkQuery.split('%3A')
        }
        else {
            linkParameters = linkQuery.split(':')
        }

        if ($.trim(link).indexOf("securitykey") != -1) {
            $.CurrentEntityKey = linkParameters[0];
            $.CurrentEntityId = linkParameters[1];
            $.CurrentTenant = linkParameters[2];
            $.IsBrandingEnabled = linkParameters[3];

            $.IsExternalURL = true;
            if ($.IsBrandingEnabled == "true") {
                //Do Something
            }

            $.GetLogginData();
            $.GetCompanyLogo();
        }
    });

}(jQuery));