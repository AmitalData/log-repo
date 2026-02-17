(function (jQuery) {

    jQuery.CurrentTenant = null;
    jQuery.CurrentEntityId = null;
    jQuery.CurrentEntityPM = null;
    jQuery.CurrentEntityKey = null;
    jQuery.CurrentCardType = "CS";
    jQuery.IsExternalURL = true;

    jQuery.IsLoadingShipmentCompleted = false;
    jQuery.IsLoadingDocumentsCompleted = false;
    jQuery.RunBusyIndicator = (function () {
        $.IsLoadingShipmentCompleted = false;
        $.IsLoadingDocumentsCompleted = false;
        $("#PageBusyIndicator").show();
    });

    jQuery.StopBusyIndicator = (function () {
        if ($.IsLoadingShipmentCompleted && $.IsLoadingDocumentsCompleted) {
            $("#PageBusyIndicator").hide();            
        }
    });

    jQuery.ResizePage = (function (myFixedHeight) {
        var minHeight = 400;
        var _height = $(window).height() - myFixedHeight;

        if (_height < minHeight) {
            _height = minHeight;
        }

        $(".pageContent").css({ height: _height });
        $(".tabPage").css({ height: _height - 35 });
        //$("#PackagesContainer").css({ height: _height - 40 - 100 });
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
                if (result != null) {
                    jQuery("#companyLogo").attr('src', result);
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
            }
        });
    });

    jQuery.GetSingleEntityPM = (function () {
       
        $(".ShowOnDataControl").hide();

        $.IsLoadingShipmentCompleted = false;

        var url = "api/shipments/getsinglepmbykey/" + $.CurrentEntityKey + "/" + $.CurrentEntityId + "/" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (shipmentPM) {
                
                if (shipmentPM) {
                    $.CurrentEntityPM = shipmentPM;
                    
                    ko.applyBindings(BuildShipmentBackAreaViewModel(shipmentPM, ""), document.getElementById("BackArea"));
                    ko.applyBindings(BuildShipmentHeaderViewModel(shipmentPM, ""), document.getElementById("EntityHeaderArea"));

                    $(".ShowOnDataControl").show();
                    $.SendContactActivity($.CurrentEmail, "Shipment", "Shipment Display", $.CurrentTenant, $.CurrentCardId);
                }

                else {
                    if ($.IsExternalURL) {
                        $("#Container").hide();
                        $("#InvalidKeyArea").show();
                    }
                }

                $.IsLoadingShipmentCompleted = true;
                $.StopBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                
                $.CheckUserException(jqXHR);

                if ($.IsExternalURL) {

                    $("#Container").hide();
                    $("#InvalidKeyArea").show();
                }

                $.IsLoadingShipmentCompleted = true;
                $.StopBusyIndicator();
            }
        });        
    });

    jQuery.GetShipmentDocuments = (function () {

        $.IsLoadingDocumentsCompleted = false;

        var url = "api/DocumentsData?securitykey=" + $.CurrentEntityKey + "&entityId=" + $.CurrentEntityId + "&partnerType=" + $.CurrentCardType + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                $.SendContactActivity($.CurrentEmail, "Shipment", "Documents Display", $.CurrentTenant, $.CurrentCardId);

                if (result.length > 0) {

                    ko.applyBindings(BuildDocumentsTabPageViewModel(result, "", true), document.getElementById("DocumentsTabPageControl"));

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

                $.IsLoadingDocumentsCompleted = true;
                $.StopBusyIndicator();
            },

            error: function (jqXHR, textStatus, errorThrown) {

                $.CheckUserException(jqXHR);
                $.IsLoadingDocumentsCompleted = true;
                $.StopBusyIndicator();
            }
        });
    });

    $("#mainTabsDiv").kendoTabStrip(
    {
        animation: false,
    });

    $(window).resize(function () {

        $.ResizePage(210);
    });

    $(document).ready(function () {
       
        $.ResizePage(210);

        var link = $(location).attr('href');
        if ($.trim(link).indexOf("securitykey") != -1) {
            var linkArray = link.split('=')
            var linkQuery = linkArray[1];

            if (linkQuery != null) {
                var linkParameters = linkQuery.split(':')
                $.CurrentEntityKey = linkParameters[0];
                $.CurrentEntityId = linkParameters[1];
                $.CurrentTenant = linkParameters[2];
                
                $.RunBusyIndicator();
                $.GetCompanyLogo();
                $.GetSingleEntityPM();
                $.GetShipmentDocuments();               
            }
        }
    });

}(jQuery));