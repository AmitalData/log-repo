(function (jQuery) {

    jQuery.CurrentEmail = null;
    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;
    jQuery.CurrentEntityId = null;
    jQuery.CurrentEntityPM = null;

    jQuery.SelectedTabCode = "OVER";
    jQuery.IsTabSelected_OVER = false;
    jQuery.IsTabSelected_GENE = false;
    jQuery.IsTabSelected_PROD = false;

    jQuery.IsOpportunityPMLoaded = false;
    
    jQuery.ResizePage = (function () {

        var minHeight = 400;
        var fixedHeight = 50;
        var screenHeight = $(window).height();
        var PageHeight = screenHeight - fixedHeight;
        if (PageHeight < minHeight) {
            PageHeight = minHeight;
        }

        var PageContentHeight = PageHeight - 124;
        var tabContentHeight = PageContentHeight - 35;

        $("#Page").css({ height: PageHeight });
        $("#PageContent").css({ height: PageContentHeight });
        $(".TabContent").css({ height: tabContentHeight });
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

        $("#NameControl").html($.trim($.CurrentEntityPM.CustomerName));
        
        $("#Topic").html($.trim($.CurrentEntityPM.Topic));
        $("#Stage").html($.trim($.CurrentEntityPM.StageName));
        $("#Value").html($.trim($.CurrentEntityPM.Value));
        $("#Owner").html($.trim($.CurrentEntityPM.OwnerName));
        $("#CustomerAge").html($.trim($.CurrentEntityPM.Age));
        $("#ClosingDate").html($.GetStartDate($.CurrentEntityPM.EstimatedClosingDate));
        $("#HeaderData").show();
    });

    jQuery.GetSingleEntityPM = (function () {

        $("#OverviewTabBusyIndicator").show();

        var url = "../api/Opportunities?singleOpportunityId=" + $.CurrentEntityId + "&tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (entityPM) {

                $.CurrentEntityPM = entityPM;

                if (entityPM) {

                    $.UpdateHeader();
                }

                $.IsOpportunityPMLoaded = true;
                $("#OverviewTabBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.IsOpportunityPMLoaded = true;
                $("#OverviewTabBusyIndicator").hide();
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

    jQuery.SelectTab = (function (tabCode) {

        $(".TabPage").hide();
        $(".HyperLinkQuery").css({ "color": "#45494A", "background": "transparent" });

        if ($.SelectedTabCode == null) {
            $.SelectedTabCode = "OVER";
        }

        switch ($.SelectedTabCode) {

            case "OVER": {
                $("#OVER_Page").show();

                if (!$.IsTabSelected_OVER) {
                    $.IsTabSelected_OVER = true

                    //$.BuildStatisticsPageFilters();
                    $.GetSingleEntityPM();
                    //$.GetActualProductData();
                    //$.GetShipmentsChartData();
                    //$.GetQuotesChartData();
                }

                break;
            }

            case "GENE": {
                $("#GENE_Page").show();

                if (!$.IsTabSelected_GENE) {
                    $.IsTabSelected_GENE = true;

                    //$.BuildOverviewScreenData();
                    //$.GetMoneyInformation();
                    //$.GetOpportunitesListData();
                    //$.GetActivitiesListData();
                    //$.GetQuotesListData();
                    //$.GetCompetitorsListData();
                }
                break;
            }

            case "PROD": {
                $("#PROD_Page").show();

                if (!$.IsTabSelected_PROD) {
                    $.IsTabSelected_PROD = true

                    //$.GetAllAddresses();
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

        $.SelectTab();
    });

}(jQuery));