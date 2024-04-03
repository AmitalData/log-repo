(function (jQuery) {

    $(document).ready(function () {

        $.SetGeneralVariableData();
        $.GetLogginData();
        $.GetCompanyLogo();
        $.SetPoweredAreaVisibility();
        $.SetReportPageTitle();
        $.SetDefultReportFilterValue();
        $.GetDocumentDownloadToken();
        $.InitializeDocumentDownloadTokenTimer();
    });


    jQuery.SetGeneralVariableData = (function () {
        $.Token = $("#TokenInput").val();
        let linkParameters = $.GetLinkParameters();
        $.ReportName = linkParameters[0];
        $.CurrentCardId = linkParameters[1];
        $.CurrentTenant = linkParameters[2];
        $.CurrentEmail = linkParameters[3];
        $.CurrentCardType = linkParameters[4];
        $.IsBrandingEnabled = linkParameters[5];

    });


    jQuery.GetLinkParameters = (function () {

        var linkQuery = $.trim($.Token) == "" ? $.GetLinklinkQuery() :  $("#LoginInput").val();
        if (linkQuery && linkQuery.indexOf('%3A') > -1) {
            return linkQuery.split('%3A')
        }
        return linkQuery.split(':')
    });

    jQuery.GetLinklinkQuery = (function () {
        var link = $(location).attr('href');
        var linkArray = link.split('=')
       return linkArray[1];
    });


    jQuery.GetCompanyLogo = (function () {

        $.ajax({
            url: "../api/commondata/?companyId=" + $.CurrentTenant,
            type: 'GET',
            contentType: 'application/json',
            headers: {'Token': $.Token},
            success: function (result) {
                $.LoadCompanyLogoSsuccess(result);
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
            }
        });
    });


    jQuery.LoadCompanyLogoSsuccess = (function (result) {

        var img = new Image();
        img.onload = function () {
            var width = this.width > 200 ? "200px" : (this.width + "px");
            jQuery("#companyLogo").attr('src', result);
            jQuery("#companyLogo").css('width', width);
            jQuery("#companyLogoArea").css('width', width);
        }
        img.src = result;
    });


    jQuery.GetLogginData = (function () {

        $.ajax({
            url: "../api/commondata/?email=" + $.CurrentEmail + "&tenant=" + $.CurrentTenant + "&cardId=" + $.CurrentCardId,
            type: 'GET',
            contentType: 'application/json',
            headers: {'Token': $.Token},
            success: function (result) {
                $.LoadLogginDataSsuccess(result);
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
            }
        });

    });

    jQuery.LoadLogginDataSsuccess = (function (result) {

        $("#CompanyText").html(result.TenantCompany);
        $("#MemberText").html(result.ContactName);
        $("#MemberCardText").html(" (" + result.CardName + ")");
        $.ContactId = result.ContactId;
    });




    jQuery.SetPoweredAreaVisibility = (function () {
        let isPoweredAreaVisibly = ($.IsBrandingEnabled && $.IsBrandingEnabled.toLowerCase() == "true") ? false : true;
        $("#PoweredArea").toggle(isPoweredAreaVisibly);
    });

    jQuery.SetReportPageTitle = (function () {
        document.getElementById("ReportName").innerHTML = $.ReportName;
    });

    jQuery.SetDefultReportFilterValue = (function () {
        let previousMonthDate = new Date().setMonth(new Date().getMonth() - 1);
        document.getElementById("FromDate").value = $.format.date(previousMonthDate, "yyyy-MM-dd");
        document.getElementById("ToDate").value = $.format.date(new Date(), "yyyy-MM-dd");
    });

    jQuery.GetDocumentDownloadToken = (function () {

        $.ajax({
            url: "../api/DocumentDownloadToken",
            type: 'GET',
            contentType: 'application/json',
            headers: {
                'Token': $.Token
            },
            success: function (documentDownloadToken) {
                $.DocumentDownloadToken = documentDownloadToken;

            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.CheckUserException(jqXHR);
            }
        });

    });

    jQuery.InitializeDocumentDownloadTokenTimer = (function () {
        let millisecond = 600000;
        setInterval($.GetDocumentDownloadToken, millisecond);
    });


    $("#RunReportButton").click(function () {
        $("#ReportPageBusyIndicator").show();
        $("#DownloadReportLinkArea").hide();
        $.ReportExcelFileName = "";
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
                $.RunReportFailure(jqXHR);
            }
        });
    });

    jQuery.RunReportSsuccess = (function (result) {
        $.ReportExcelFileName = result.FileName;
        $("#ReportPageBusyIndicator").hide();
        $("#DownloadReportLinkArea").show();
    });

    jQuery.RunReportFailure = (function (jqXHR) {
        $.CheckUserException(jqXHR);
        $("#ReportPageBusyIndicator").hide();
    });

    $("#BackButton").click(function () {
        window.history.go(-1);
        return false;
    });

    $("#DownloadReportLink").click(function () {

        var reportDownloadURL = "../WebPages/DawnLoadExcelPage.aspx?fileName=" + $.ReportExcelFileName + "&tempId=" + $.DocumentDownloadToken + "&qname=" + $.ReportName + "&requestArea=SharedLogistic"  ;
        window.open(reportDownloadURL);
    });


}(jQuery));

function SharedLogisticReportFilters() {
    this.ReportName = $.ReportName;
    this.PartnerId = $.CurrentCardId;
    this.ContactId = $.ContactId;
    this.ObjectTableName = "Shipment";
    this.SortByColumnName = "StatusDate";
    this.QuerySection = "SharedLogisticShipment";
    this.SortDirectin = "Descending",
    this.QueryCode = "Shipment.SharedLogisticShipment",
    this.Tenant = $.CurrentTenant,
    this.QueryFilterItems = BuildQueryFilterItems();


};
function BuildQueryFilterItems() {

    let queryFilterItems = [];
    let toDate = new Date(document.getElementById("ToDate").value);
    if (toDate) toDate.setDate(toDate.getDate() + 1);

    queryFilterItems.push(new QueryFilterItem("CustomerId", $.CurrentCardId, "Equal"));
    queryFilterItems.push(new QueryFilterItem("CreateDateTime", new Date(document.getElementById("FromDate").value), "GreaterThanOrEqual"));
    queryFilterItems.push(new QueryFilterItem("CreateDateTime", toDate, "LessThan"));
    queryFilterItems.push(new QueryFilterItem("IsOperationalClosed", document.getElementById("OperationallyClosed").checked, "Equal"));
    return queryFilterItems;

}


function QueryFilterItem(fieldName, fieldValue, operator) {
    this.FieldName = fieldName;
    this.FieldValue = fieldValue;
    this.Operator = operator;
}



