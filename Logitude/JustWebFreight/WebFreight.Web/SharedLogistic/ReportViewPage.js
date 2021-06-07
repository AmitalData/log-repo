(function (jQuery) {



    $(document).ready(function () {

        $("#DownLoadReportMessage").hide();


        $.Token = $("#TokenInput").val();
        var linkQuery = $("#LoginInput").val();
        var linkParameters = null;

        if ($.trim($.Token) == "") {
            var link = $(location).attr('href');
            var linkArray = link.split('=')
            linkQuery = linkArray[1];
        }

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
        $.SetDefultReportFilterValue();

    });

    jQuery.GetCompanyLogo = (function () {

        var url = "../api/commondata/?companyId=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',
            headers: {
                'Token': $.Token
            },

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
            headers: {
                'Token': $.Token
            },

            success: function (result) {

                $("#CompanyText").html(result.TenantCompany);
                $("#MemberText").html(result.ContactName);
                $("#MemberCardText").html(" (" + result.CardName + ")");
                $.ContactId = result.ContactId; 
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
            }
        });

    });





    jQuery.SetDefultReportFilterValue = (function () {
        let previousMonthDate = new Date().setMonth(new Date().getMonth() - 1);
        document.getElementById("FromDate").value = $.format.date(previousMonthDate, "yyyy-MM-dd");
        document.getElementById("ToDate").value = $.format.date(new Date(), "yyyy-MM-dd");
    });

    $("#RunReport").click(function () {
        $("#ReportPageBusyIndicator").show();
        $("#DownLoadReportMessage").hide();
        var reportfilters = new SharedLogisticReportFilters();
   
        var url = "../api/SharedLogisticReport";
        $.ajax({
            url: url,
            data: JSON.stringify(reportfilters),
            type: 'POST',
            contentType: 'application/json',
            headers: { 'Token': $.Token },
            success: function (result) {
                $.RunReportSsuccess(result);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.ReportLoadedFailure(jqXHR);
            }
        });
    });

    jQuery.RunReportSsuccess = (function (jqXHR) {
        $("#ReportPageBusyIndicator").hide();
        $("#DownLoadReportMessage").show();


    });

    jQuery.RunReportFailure = (function (jqXHR) {
        $.CheckUserException(jqXHR);
        $("#ReportPageBusyIndicator").hide();
    });

    $("#BackButton").click(function () {
        //parent.history.back();
        window.history.go(-1);
        return false;
    });

}(jQuery));




function SharedLogisticReportFilters() {

    this.PartnerId = $.CurrentCardId;
    this.ContactId = $.ContactId;
    this.ObjectTableName = "Shipment";
    this.SortByColumnName = "StatusDate";
    this.QuerySection = "ShipmentSharedLogistic";
    this.SortDirectin = "Descending",
    this.QueryCode = "Shipment.Shipments",
    this.QueryFilterItems = BuildQueryFilterItems();


};



function BuildQueryFilterItems() {

    let queryFilterItems = [];
    queryFilterItems.push(new QueryFilterItem("CustomerId", $.CurrentCardId, "Equal"));
    queryFilterItems.push(new QueryFilterItem("CreateDateTime", new Date(document.getElementById("FromDate").value), "GreaterThanOrEqual"));
    queryFilterItems.push(new QueryFilterItem("CreateDateTime", new Date(document.getElementById("ToDate").value), "LessThanOrEqual"));
    queryFilterItems.push(new QueryFilterItem("IsOperationalClosed", document.getElementById("OperationallyClosed").checked, "Equal"));
    return queryFilterItems;




}
function QueryFilterItem(fieldName, fieldValue, operator) {

    this.FieldName = fieldName;
    this.FieldValue = fieldValue;
    this.Operator = operator;



}



