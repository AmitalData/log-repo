(function (jQuery) {
    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;
    jQuery.CurrentCardType = "AG";
    jQuery.CurrentEntityId = null;
    jQuery.CurrentEntityPM = null;
    jQuery.CurrentEmail = null;
    jQuery.IsExternalURL = false;
    jQuery.CurrentEntityKey = null;
    jQuery.IsBrandingEnabled = "";
    jQuery.TenantDateTimeFormat = null;
    jQuery.DisplayDocumentsAndEvents = false;
    jQuery.DownloadAll = false;
    jQuery.DocumentUrl = "";
    jQuery.AllDocumentsFileName = "";

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

        var url = "api/commondata/?companyId=" + $.CurrentTenant;

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

    jQuery.GetCargoLoginTenant = (function (myDomain) {

        var url = "api/CargoTrackingBranding/?domain=" + myDomain;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {
                if (result != null && result.Result != null) {
                    $.CurrentTenant = result.Result;

                    $.GetLogginData();
                    $.GetCompanyLogo();
                }
                else {
                    $("#DocumentsPageBusyIndicator").hide();
                    $("#Container").hide();
                    $("#InvalidKeyArea").show();
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#DocumentsPageBusyIndicator").hide();
                $("#Container").hide();
                $("#InvalidKeyArea").show();
            }
        });

    });

    jQuery.GetLoginTenant = (function (myDomain) {

        var url = "api/CargoTrackingBranding/?domain=" + myDomain;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {
                if (result != null && result.Result != null) {
                    $.CurrentTenant = result.Result;

                    $.GetLogginData();
                    $.GetCompanyLogo();
                }
                else {
                    $.GetCargoLoginTenant(myDomain + "/cargotracking");
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#DocumentsPageBusyIndicator").hide();
                $("#Container").hide();
                $("#InvalidKeyArea").show();
            }
        });

    });

    jQuery.GetLogginData = (function () {

        var url = "api/commondata/?email=" + $.CurrentEmail + "&tenant=" + $.CurrentTenant + "&cardId=" + $.CurrentCardId;

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
            var url = "api/shipments/getsinglepmbykeyandtenant/" + $.CurrentEntityKey + "/" + $.CurrentTenant;
        }

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',
            success: function (shipmentPM) {
                if (shipmentPM) {
                    SetShipmentPM(shipmentPM);
                    ko.applyBindings(BuildShipmentHeaderViewModel(shipmentPM, $.TenantDateTimeFormat, ""), document.getElementById("EntityHeaderArea"));
                    BuildRoutingLegs(shipmentPM, $.TenantDateTimeFormat);
                    $(".ShowOnDataControl").show();
                    $("#DocumentsPageBusyIndicator").hide();
                    if ($.IsExternalURL) {
                        if ($.DisplayDocumentsAndEvents) {
                            $.GetShipmentDocuments($.CurrentEntityId, $.CurrentEntityKey, "MasterDocuments_"+ shipmentPM.LongMaster);
                            if (shipmentPM.ConnectedShipments > 0) {
                                $.GetMasterConnectedHouseShipments();
                            }
                        }
                        else {
                            $("#DocumentsTabPageControl" + $.CurrentEntityId).css({
                                "font-family": "Arial",
                                "color": "#8F9293",
                                "font-size": "16px",
                                "margin-top": "20px",
                                "text-align": "center",
                                "padding": "20px",
                            });

                            $("#DocumentsTabPageControl" + $.CurrentEntityId).html("Documents information is only available for logged-in users");
                        }
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
                $("#DocumentsPageBusyIndicator").hide();
                if ($.IsExternalURL) {
                    $("#Container").hide();
                    $("#InvalidKeyArea").show();
                }
            }
        });
    });

    jQuery.GetShipmentDocuments = (function (myEntityId, myEntityKey, fileName) {
        $("#DocumentsPageBusyIndicator").show();

        var url = null;

        if ($.IsExternalURL) {
            url = "api/DocumentsData?securitykey=" + myEntityKey + "&entityId=" + myEntityId + "&partnerType=" + $.CurrentCardType + "&tenant=" + $.CurrentTenant;
        }

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.SendContactActivity($.CurrentEmail, "Shipment", "Documents Display", $.CurrentTenant, $.CurrentCardId);
                let documentsTabPageControlId = "#DocumentsTabPageControl" + myEntityId;
                if (result.length > 0) {
                    $("#DownloadAllConnectedDocuments").show();
                    ko.applyBindings(BuildMasterDocumentsTabPageViewModel(myEntityId, result, fileName, ""), document.getElementById(documentsTabPageControlId));
                }

                else {
                    $(documentsTabPageControlId).css({
                        "font-family": "Arial",
                        "color": "#8F9293",
                        "font-size": "22px",
                        "margin-top": "20px",
                        "text-align": "center",
                        "padding": "20px",
                    });

                    $(documentsTabPageControlId).html("No Documents");
                }

                $("#DocumentsPageBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
                $("#DocumentsPageBusyIndicator").hide();
            }
        });

    });

    jQuery.GetMasterConnectedHouseShipments = (function () {
        $("#DocumentsPageBusyIndicator").show();
        var url = null;

        if ($.IsExternalURL) {
            url = "api/ShipmentDomain?entityId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;
        }

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.SendContactActivity($.CurrentEmail, "Shipment", "Get connected house shipments", $.CurrentTenant, $.CurrentCardId);
                if (result.length > 0) {
                    $.ConnectedHouses = result;
                    $.each(result, function (index, value) {
                        if (value) {
                            if ($.DisplayDocumentsAndEvents) {
                                $.GetShipmentDocuments(value.Id, value.SecurityKey, "HouseDocuments_"+value.House);
                            }
                            else {
                                $("#DocumentsTabPageControl" + value.Id).css({
                                    "font-family": "Arial",
                                    "color": "#8F9293",
                                    "font-size": "16px",
                                    "margin-top": "20px",
                                    "text-align": "center",
                                    "padding": "20px",
                                });

                                $("#DocumentsTabPageControl" + value.Id).html("Documents information is only available for logged-in users");
                            }
                        }
                    });
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
        var domain = link.split("/sharedmasterdocumentspage.aspx")[0];
        if (domain != null && domain.indexOf("//") > -1) {
            domain = domain.split("//")[1];
            if (domain.indexOf(":") > -1)
                domain = domain.split(":")[0];
        }
        if (linkQuery && linkQuery.indexOf('%3A') > -1) {
            linkParameters = linkQuery.split('%3A')
        }
        else {
            linkParameters = linkQuery.split(':')
        }

        if ($.trim(link).indexOf("securitykey") != -1) {
            $.CurrentEntityKey = linkParameters[0];
            $.IsExternalURL = true;

            $("#DownloadAllConnectedDocuments").hide();
            $("#DocumentsPageBusyIndicator").show();
            $.GetLoginTenant(domain);
        }
    });

}(jQuery));

function SetShipmentPM(shipmentPM) {
    $.CurrentEntityPM = shipmentPM;
    $.CurrentEntityId = shipmentPM.Id;
    $("#LongMasterNumber").html(shipmentPM.LongMaster);
    $.AllDocumentsFileName = "MasterDocuments_" + shipmentPM.LongMaster;
    $("#ConnectedShipments").html(" (" + shipmentPM.ConnectedShipments + " Houses)");
    $("#MainCarriageCarrierName").html(shipmentPM.MainCarriageCarrierName != null ? shipmentPM.MainCarriageCarrierName : "---");
    let mainCarriageATD = shipmentPM.MainCarriageATD;
    if (mainCarriageATD != null) {
        let mainCarriageATDToString = formatDate(new Date(mainCarriageATD));
        $("#MainCarriageATD").html(mainCarriageATDToString);
    }
    else
        $("#MainCarriageATD").html("---");
}
function formatDate(date) {
    var myDate = "";
    let day = ("0" + date.getDate()).slice(-2);
    let month = ("0" + (date.getMonth() + 1)).slice(-2);
    let year = date.getFullYear();
    let hours = ("0" + date.getHours()).slice(-2);
    let minutes = ("0" + date.getMinutes()).slice(-2);
    myDate = day + "." + month + "." + year + " " + hours + ":" + minutes;

    return myDate;
}