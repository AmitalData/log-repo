(function (jQuery) {

    jQuery.CurrentEmail = null;
    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;

    jQuery.SearchText_CUS = null;
    jQuery.SearchTimer_CUS = null;
    jQuery.watermark_CUS = "Search ..";
    jQuery.SelectedQuery_CUS = null;
    jQuery.IsCustomersDataLoaded = false;

    jQuery.SearchText_OPP = null;
    jQuery.SearchTimer_OPP = null;
    jQuery.watermark_OPP = "Search ..";
    jQuery.SelectedQuery_OPP = null;
    jQuery.IsOpportunitiesDataLoaded = false;

    jQuery.SearchText_ACT = null;
    jQuery.SearchTimer_ACT = null;
    jQuery.watermark_ACT = "Search ..";
    jQuery.SelectedQuery_ACT = null;
    jQuery.IsActivitiesDataLoaded = false;

    jQuery.SelectedTabId = "TAB_CUS";

    jQuery.ResizePage = (function () {

        var minHeight = 400;
        var PageHeight = $(window).height() - 50;
        if (PageHeight < minHeight) {
            PageHeight = minHeight;
        }

        $("#Page").css({ height: PageHeight })
        $("#PageContent").css({ height: PageHeight - 30 })
        $(".TabPage").css({ height: PageHeight - 35 })
        $(".TabPageContent").css({ height: PageHeight - 35 - 20 })
        $(".ListBoxContainer").css({ height: PageHeight - 35 - 20 - 35 });
    });

    jQuery.GetLogginData = (function () {
        
        var url = "../api/commondata/?email=" + $.CurrentEmail + "&tenant=" + $.CurrentTenant + "&cardId=" + $.CurrentCardId;

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {

                //$("#CompanyText").html(result.TenantCompany);
                $("#MemberText").html(result.ContactName);
                $("#MemberCardText").html(" (" + result.CardName + ")");
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

            if (navigator.userAgent.indexOf('Safari') != -1) {

                //http://momentjs.com/
            }
        }

        if (days == 0)
        {
            startDate = "Today";
        }

        if (days == 1)
        {
            startDate = "Yesterday";
        }

        if (days > 1 && days < 31)
        {
            startDate = days + " Days";
        }

        if (days >= 31 && days < 1095)
        {
            var months = days / 31;
            months = Math.round(months - 0.5);

            if (months == 1)
            {
                startDate = months + " Month";
            }

            else
            {
                startDate = months + " Months";
            }
        }

        if (days > 1095)
        {
            var years = days / 365;
            years = Math.round(years - 0.5);

            if (years == 1)
            {
                startDate = years + " Year";
            }

            else
            {
                startDate = years + " Years";
            }
        }


        return startDate;
    });

    jQuery.LoadCustomers = (function () {
            
        $("#CustomersBusyIndicator").show();
        
        function CustomerFilters() {
           
            this.SearchField = null;
            this.SearchField = ($.trim($.SearchText_CUS) == "" || $.trim($.SearchText_CUS) == $.watermark_CUS) ? null : $.trim($.SearchText_CUS);

            if ($.trim($.SelectedQuery_CUS) == "Query_MY_CUS") {

                this.IsMyCustomers = true;
            }

            else {

                this.IsMyCustomers = false;
            }
        };

        var filters = new CustomerFilters();

        var url = "../api/CustomersData?tenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            contentType: 'application/json',

            success: function (result) {               

                if (result != null) {
                    $("#CustomersQueryCount").html("(" + result.length + ")");

                    var DataSource = [];
                    $.each(result, function (index, item) {
                        DataSource.push({
                            Id: item.Id,
                            Rank: $.trim(item.RankName),
                            Name: $.trim(item.EnglishName),
                            Date: $.GetStartDate(item.StartWorkingDate),
                            Last: $.GetStartDate(item.LastShipmentDate),
                            City: $.trim(item.CityName)
                        });
                    });

                    $("#CustomersListBox").html("");
                    $("#CustomersListBox").kendoListView(
                    {
                        scrollable: true,
                        dataSource: { data: DataSource },
                        template: kendo.template($("#CustomerListBoxItemDataTemplate").html()),
                        selectable: true,

                        change: function () {

                            var selectedIndex = this.select().index();
                            var selectedItem = this.dataSource.view()[selectedIndex];

                            if (selectedItem != null) {

                                var id = selectedItem.Id;

                                if (id != null) {

                                    var linkString = "id=" + id + "&tenant=" + $.CurrentTenant + "&email=" + $.CurrentEmail + "&cardId=" + $.CurrentCardId;
                                    document.location.href = "CustomerPage.aspx?" + linkString;
                                }
                            }
                        }
                    });
                }
                
                $("#CustomersBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $("#CustomersQueryCount").html("(0)");
                $("#CustomersBusyIndicator").hide();
            }
        });
    });

    jQuery.LoadOpportunities = (function () {       

        $("#OpportunitiesBusyIndicator").show();
        
        function OpportunityFilters() {
            this.SearchField = null;
            this.SearchField = ($.trim($.SearchText_OPP) == "" || $.trim($.SearchText_OPP) == $.watermark_OPP) ? null : $.trim($.SearchText_OPP);

            if ($.trim($.SelectedQuery_OPP) == "Query_MY_OPP") {

                this.IsMyOpportunities = true;
            }
            else {

                this.IsMyOpportunities = false;
            }
        };

        var filters = new OpportunityFilters();

        var url = "../api/Opportunities?opportunitiesTenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            contentType: 'application/json',

            success: function (result) {

                $("#OpportunitiesQueryCount").html("(" + result.length + ")");

                var DataSource = [];
                $.each(result, function (index, item) {
                    DataSource.push({
                        Id: item.Id,
                        Topic: $.trim(item.Topic),
                        OwnerName: $.trim(item.OwnerName),
                        ClosingDate: $.GetStartDate(item.EstimatedClosingDate),
                        RatingCode: $.trim(item.RatingCode),
                        RatingName: $.trim(item.RatingName)
                    });
                });

                $("#OpportunitiesListBox").html("");
                $("#OpportunitiesListBox").kendoListView(
		        {
		            scrollable: true,
		            dataSource: { data: DataSource },
		            template: kendo.template($("#OpportunityListBoxItemDataTemplate").html()),
		            selectable: true,

		            change: function () {

		                var selectedIndex = this.select().index();
		                var selectedItem = this.dataSource.view()[selectedIndex];

		                if (selectedItem != null) {

		                    var id = selectedItem.Id;

		                    if (id != null) {

		                        var linkString = "id=" + id + "&tenant=" + $.CurrentTenant + "&email=" + $.CurrentEmail + "&cardId=" + $.CurrentCardId;
		                        document.location.href = "OpportunityPage.aspx?" + linkString;
		                    }
		                }
		            }
		        });

                $("#OpportunitiesBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $("#OpportunitiesQueryCount").html("(0)");
                $("#OpportunitiesBusyIndicator").hide();
            }
        });
    });

    jQuery.LoadActivities = (function () {
        $("#ActivitiesBusyIndicator").show();
        
        function ActivityFilters() {
           
            this.SearchField = null;
            this.SearchField = ($.trim($.SearchText_ACT) == "" || $.trim($.SearchText_ACT) == $.watermark_ACT) ? null : $.trim($.SearchText_ACT);

            if ($.trim($.SelectedQuery_ACT) == "Query_MY_ACT") {

                this.IsMyActivities = true;                
            }
            else {

                this.IsMyActivities = false;
            }
        };

        var filters = new ActivityFilters();

        var url = "../api/Activities?activitiesTenant=" + $.CurrentTenant;

        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            contentType: 'application/json',

            success: function (result) {               

                $("#ActivitiesQueryCount").html("(" + result.length + ")");

                var DataSource = [];

                $.each(result, function (index, item) {                    
                    DataSource.push({
                        Id: item.Id,
                        ActivityTypeCode: $.trim(item.ActivityTypeCode),
                        Subject: $.trim(item.Subject),
                        StartDateTime: $.GetStartDate(item.StartDateTime),
                        DueDate: $.GetStartDate(item.DueDate),
                        Owner: $.trim(item.OwnerName)
                    });
                });

                $("#ActivitiesListBox").html("");
                $("#ActivitiesListBox").kendoListView(
		        {
		            scrollable: true,
		            dataSource: { data: DataSource },
		            template: kendo.template($("#ActivityListBoxItemDataTemplate").html()),
		            selectable: true,

		            change: function () {
		                
		                var selectedIndex = this.select().index();
		                var selectedItem = this.dataSource.view()[selectedIndex];

		                if (selectedItem != null) {

		                    var id = selectedItem.Id;

		                    if (id != null) {

		                        var linkString = "id=" + id + "&tenant=" + $.CurrentTenant + "&email=" + $.CurrentEmail + "&cardId=" + $.CurrentCardId;
		                        document.location.href = "ActivityPage.aspx?" + linkString;
		                    }
		                }
		            }
		        });
                
                $("#ActivitiesBusyIndicator").hide();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $("#ActivitiesQueryCount").html("(0)");
                $("#ActivitiesBusyIndicator").hide();
            }
        });      
    });

    jQuery.SearchTextChanged = (function (id) {
        
        switch (id) {
            
            case "SearchBox_CUS":
                {
                    var searchText = $("#SearchBox_CUS").val();
                    searchText = $.trim(searchText);

                    if (searchText.length == 0 || searchText == $.watermark_CUS) {
                        $('#SearchDeleteButton_CUS').hide();
                    }

                    else {
                        $('#SearchDeleteButton_CUS').show();
                    }

                    $.SearchText_CUS = $("#SearchBox_CUS").val();
                    $('#SavedSearchText_CUS').attr("value", $.SearchText_CUS);

                    if ($.SearchTimer_CUS != null) {
                        clearTimeout($.SearchTimer_CUS);
                    }

                    $.SearchTimer_CUS = setTimeout(function () { $.LoadCustomers() }, 500);

                    break;
                }

            case "SearchBox_OPP":
                {

                    if ($("#SearchBox_OPP").val().length == 0 || $("#SearchBox_OPP").val() == $.watermark_OPP) {
                        $('#SearchDeleteButton_OPP').hide();
                    }

                    else {
                        $('#SearchDeleteButton_OPP').show();
                    }

                    $.SearchText_OPP = $("#SearchBox_OPP").val();
                    $('#SavedSearchText_OPP').attr("value", $.SearchText_OPP);

                    if ($.SearchTimer_OPP != null) {
                        clearTimeout($.SearchTimer_OPP);
                    }

                    $.SearchTimer_OPP = setTimeout(function () { $.LoadOpportunities() }, 500);

                    break;
                }

            case "SearchBox_ACT":
                {

                    if ($("#SearchBox_ACT").val().length == 0 || $("#SearchBox_ACT").val() == $.watermark_ACT) {
                        $('#SearchDeleteButton_ACT').hide();
                    }

                    else {
                        $('#SearchDeleteButton_ACT').show();
                    }

                    $.SearchText_ACT = $("#SearchBox_ACT").val();
                    $('#SavedSearchText_ACT').attr("value", $.SearchText_ACT);

                    if ($.SearchTimer_ACT != null) {
                        clearTimeout($.SearchTimer_ACT);
                    }

                    $.SearchTimer_ACT = setTimeout(function () { $.LoadActivities() }, 500);

                    break;
                }
        }
    });

    $('.SearchDeleteButton').hide();
    $('#SearchBox_CUS').val($.watermark_CUS).addClass('watermark');
    $('#SearchBox_OPP').val($.watermark_OPP).addClass('watermark');
    $('#SearchBox_ACT').val($.watermark_ACT).addClass('watermark');

    $('.SearchBox').blur(function () {

        var id = $(this).attr('id');

        switch (id) {

            case "SearchBox_CUS":
                {

                    if ($(this).val().length == 0) {
                        $(this).val($.watermark_CUS).addClass('watermark');
                        $('#SearchIcon_CUS').show();
                        $('#SearchDeleteButton_CUS').hide();
                    }

                    break;
                }

            case "SearchBox_OPP":
                {

                    if ($(this).val().length == 0) {
                        $(this).val($.watermark_OPP).addClass('watermark');
                        $('#SearchIcon_OPP').show();
                        $('#SearchDeleteButton_OPP').hide();
                    }

                    break;
                }

            case "SearchBox_ACT":
                {

                    if ($(this).val().length == 0) {
                        $(this).val($.watermark_ACT).addClass('watermark');
                        $('#SearchIcon_ACT').show();
                        $('#SearchDeleteButton_ACT').hide();
                    }

                    break;
                }
        }
    });
    $('.SearchBox').focus(function () {

        var id = $(this).attr('id');

        switch (id) {

            case "SearchBox_CUS":
                {

                    if ($(this).val() == $.watermark_CUS) {
                        $(this).val('').removeClass('watermark');
                        $('#SearchIcon_CUS').hide();
                    }

                    break;
                }

            case "SearchBox_OPP":
                {

                    if ($(this).val() == $.watermark_OPP) {
                        $(this).val('').removeClass('watermark');
                        $('#SearchIcon_OPP').hide();
                    }

                    break;
                }

            case "SearchBox_ACT":
                {

                    if ($(this).val() == $.watermark_ACT) {
                        $(this).val('').removeClass('watermark');
                        $('#SearchIcon_ACT').hide();
                    }

                    break;
                }
        }
    });
    $('.SearchBox').keyup(function () {
        $.SearchTextChanged($(this).attr('id'));
    });

    $(".SearchDeleteButton").click(function () {        

        var id = $(this).attr('id');

        switch (id) {

            case "SearchDeleteButton_CUS":
                {
                    $('#SearchIcon_CUS').show();
                    $('#SearchBox_CUS').attr("value", "");
                    $('#SearchBox_CUS').val($.watermark_CUS).addClass('watermark');
                    $.SearchTextChanged("SearchBox_CUS");
                    break;
                }

            case "SearchDeleteButton_OPP":
                {
                    $('#SearchIcon_OPP').show();
                    $('#SearchBox_OPP').attr("value", "");
                    $('#SearchBox_OPP').val($.watermark_OPP).addClass('watermark');
                    $.SearchTextChanged("SearchBox_OPP");
                    break;
                }

            case "SearchDeleteButton_ACT":
                {
                    $('#SearchIcon_ACT').show();
                    $('#SearchBox_ACT').attr("value", "");
                    $('#SearchBox_ACT').val($.watermark_ACT).addClass('watermark');
                    $.SearchTextChanged("SearchBox_ACT");
                    break;
                }
        }
    });

    $(".HyperLinkQuery_CUS").mouseenter(function () {
        if ($(this).attr('id') != $.SelectedQuery_CUS) {
            $(this).css({ "color": "black", "background": "url('images/TabItem-O.png') repeat-x" });
        }
    });
    $(".HyperLinkQuery_CUS").mouseleave(function () {
        if ($(this).attr('id') != $.SelectedQuery_CUS) {
            $(this).css({ "color": "#45494A", "background": "transparent" });
        }
    });
    $(".HyperLinkQuery_CUS").click(function () {
        if ($(this).attr('id') != $.SelectedQuery_CUS) {
            $.SelectedQuery_CUS = $(this).attr('id');
            $("#SavedSelectedQuery_CUS").attr("value", $.SelectedQuery_CUS);
            $.SelectQuery();
            $.LoadData();
        }
    });

    $(".HyperLinkQuery_OPP").mouseenter(function () {
        if ($(this).attr('id') != $.SelectedQuery_OPP) {
            $(this).css({ "color": "black", "background": "url('images/TabItem-O.png') repeat-x" });
        }
    });
    $(".HyperLinkQuery_OPP").mouseleave(function () {
        if ($(this).attr('id') != $.SelectedQuery_OPP) {
            $(this).css({ "color": "#45494A", "background": "transparent" });
        }
    });
    $(".HyperLinkQuery_OPP").click(function () {        
        if ($(this).attr('id') != $.SelectedQuery_OPP) {
            $.SelectedQuery_OPP = $(this).attr('id');
            $("#SavedSelectedQuery_OPP").attr("value", $.SelectedQuery_OPP);
            $.SelectQuery();
            $.LoadData();
        }
    });

    $(".HyperLinkQuery_ACT").mouseenter(function () {
        if ($(this).attr('id') != $.SelectedQuery_ACT) {
            $(this).css({ "color": "black", "background": "url('images/TabItem-O.png') repeat-x" });
        }
    });
    $(".HyperLinkQuery_ACT").mouseleave(function () {
        if ($(this).attr('id') != $.SelectedQuery_ACT) {
            $(this).css({ "color": "#45494A", "background": "transparent" });
        }
    });
    $(".HyperLinkQuery_ACT").click(function () {
        if ($(this).attr('id') != $.SelectedQuery_ACT) {
            $.SelectedQuery_ACT = $(this).attr('id');
            $("#SavedSelectedQuery_ACT").attr("value", $.SelectedQuery_ACT);
            $.SelectQuery();
            $.LoadData();
        }
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

    $("#CustomersRefreshButton").mouseenter(function () {
        $("#CustomersRefreshButton").attr("src", "images/Refresh-O.png");
    });
    $("#CustomersRefreshButton").mouseleave(function () {
        $("#CustomersRefreshButton").attr("src", "images/Refresh-N.png");
    });
    $("#CustomersRefreshButton").click(function () {        
        $.LoadCustomers();
    });

    $("#OpportunitiesRefreshButton").mouseenter(function () {
        $("#OpportunitiesRefreshButton").attr("src", "images/Refresh-O.png");
    });
    $("#OpportunitiesRefreshButton").mouseleave(function () {
        $("#OpportunitiesRefreshButton").attr("src", "images/Refresh-N.png");
    });
    $("#OpportunitiesRefreshButton").click(function () {
        $.LoadOpportunities();
    });

    $("#ActivitiesRefreshButton").mouseenter(function () {
        $("#ActivitiesRefreshButton").attr("src", "images/Refresh-O.png");
    });
    $("#ActivitiesRefreshButton").mouseleave(function () {
        $("#ActivitiesRefreshButton").attr("src", "images/Refresh-N.png");
    });
    $("#ActivitiesRefreshButton").click(function () {
        $.LoadActivities();
    });

    $(".k-tabstrip .k-item").mouseenter(function () {


        var id = $(this).attr('id');
        var clss = $(this).attr('class');

        if ($.trim(clss).indexOf("k-state-active") == -1) {

            var target = "#" + id + " div span .TabImage";
            var scr = $(target).attr('src');
            var targetSRC = scr.replace('N.png', 'O.png');

            $(target).attr("src", targetSRC);
        }
    });
    $(".k-tabstrip .k-item").mouseleave(function () {

        var id = $(this).attr('id');
        var clss = $(this).attr('class');

        if ($.trim(clss).indexOf("k-state-active") == -1) {

            var target = "#" + id + " div span .TabImage";
            var scr = $(target).attr('src');
            var targetSRC = scr.replace('O.png', 'N.png');

            $(target).attr("src", targetSRC);
        }
    });
    
    jQuery.LoadData = (function () {

        switch ($.SelectedTabId) {

            case "TAB_CUS": {
                $.LoadCustomers();
                break;
            }

            case "TAB_OPP": {
                $.LoadOpportunities();
                break;
            }

            case "TAB_ACT": {
                $.LoadActivities();
                break;
            }
        }
    });

    jQuery.SelectQuery = (function () {
        
        switch ($.SelectedTabId) {

            case "TAB_CUS": {

                $(".HyperLinkQuery_CUS").css({
                    "color": "#45494A",
                    "background": "transparent",
                });


                $("#" + $.SelectedQuery_CUS).css({
                    "color": "white",
                    "background": "url('images/SelectedQuery.png') repeat-x",
                });

                var SelectedQueryLabel = "";

                switch ($.SelectedQuery_CUS)
                {

                    case "Query_MY_CUS": {
                        SelectedQueryLabel = "My Customers";
                        break;
                    }

                    default: {
                        SelectedQueryLabel = "All Customers";
                        break;
                    }
                }

                $("#CustomersQueryTitle").html(SelectedQueryLabel);
                break;
            }

            case "TAB_OPP": {

                $(".HyperLinkQuery_OPP").css({
                    "color": "#45494A",
                    "background": "transparent",
                });

                $("#" + $.SelectedQuery_OPP).css({
                    "color": "white",
                    "background": "url('images/SelectedQuery.png') repeat-x",
                });

                var SelectedQueryLabel = "";

                switch ($.SelectedQuery_OPP) {

                    case "Query_MY_OPP": {
                        SelectedQueryLabel = "My Opportunities";
                        break;
                    }

                    default: {
                        SelectedQueryLabel = "All Opportunities";
                        break;
                    }
                }

                $("#OpportunitiesQueryTitle").html(SelectedQueryLabel);
                break;
            }

            case "TAB_ACT": {

                $(".HyperLinkQuery_ACT").css({
                    "color": "#45494A",
                    "background": "transparent",
                });


                $("#" + $.SelectedQuery_ACT).css({
                    "color": "white",
                    "background": "url('images/SelectedQuery.png') repeat-x",
                });

                var SelectedQueryLabel = "";

                switch ($.SelectedQuery_ACT) {

                    case "Query_MY_ACT": {
                        SelectedQueryLabel = "My Activities";
                        break;
                    }

                    default: {
                        SelectedQueryLabel = "All Activities";
                        break;
                    }
                }

                $("#ActivitiesQueryTitle").html(SelectedQueryLabel);
                break;
            }
        }
    });

    jQuery.SelectTab = (function (id) {
                
        switch (id) {
           
            case "TAB_CUS": {
                
                $.SearchText_CUS = $('#SavedSearchText_CUS').val();
                $.SelectedQuery_CUS = $('#SavedSelectedQuery_CUS').val();

                if ($.trim($.SelectedQuery_CUS) == "") {
                    $.SelectedQuery_CUS = "Query_MY_CUS";
                    $('#SavedSelectedQuery_CUS').attr("value", $.SelectedQuery_CUS);
                }

                $.SelectQuery();

                if (!$.IsCustomersDataLoaded) {
                    $.IsCustomersDataLoaded = true;
                    $.LoadData();
                }

                break;
            }

            case "TAB_OPP": {

                $.SearchText_OPP = $('#SavedSearchText_OPP').val();
                $.SelectedQuery_OPP = $('#SavedSelectedQuery_OPP').val();

                if ($.trim($.SelectedQuery_OPP) == "") {
                    $.SelectedQuery_OPP = "Query_MY_OPP";
                    $('#SavedSelectedQuery_OPP').attr("value", $.SelectedQuery_OPP);
                }

                $.SelectQuery();

                if (!$.IsOpportunitiesDataLoaded) {
                    $.IsOpportunitiesDataLoaded = true;
                    $.LoadData();
                }

                break;
            }

            case "TAB_ACT": {

                $.SearchText_ACT = $('#SavedSearchText_ACT').val();
                $.SelectedQuery_ACT = $('#SavedSelectedQuery_ACT').val();

                if ($.trim($.SelectedQuery_ACT) == "") {
                    $.SelectedQuery_ACT = "Query_MY_ACT";
                    $('#SavedSelectedQuery_ACT').attr("value", $.SelectedQuery_ACT);
                }

                $.SelectQuery();

                if (!$.IsActivitiesDataLoaded) {
                    $.IsActivitiesDataLoaded = true;
                    $.LoadData();
                }

                break;
            }
        }

    });

    $("#TabControl").kendoTabStrip({
        animation: false,

        select: function (e) {

            var id = $(e.item).attr("id");
            $.SelectedTabId = $(e.item).attr("id");
            $('#SavedSelectedTabId').attr("value", $.SelectedTabId);
            $.SelectTab($.SelectedTabId);
        }
    });
    
    $(document).ready(function () {
     
        $.ResizePage();

        $(window).resize(function () {
            $.ResizePage();
        });

        var hash = $(location).attr('href');
        var hashSplit = hash.split("?");
        var loginText = hashSplit[1].toLowerCase();
        loginText = loginText.replace("email=", "");
        loginText = loginText.replace("tenant=", "");
        loginText = loginText.replace("cardid=", "");
        loginText = loginText.replace("ischamplogin=", "");
        var loginData = loginText.split('&');

        $.CurrentTenant = loginData[0];
        $.CurrentEmail = loginData[1];        
        $.CurrentCardId = loginData[2];

        $.GetLogginData();
        
        var id = $('#SavedSelectedTabId').val();
        if ($.trim(id) == "") {
            id = "TAB_CUS";
        }

        $.SelectedTabId = id;
        $('#SavedSelectedTabId').attr("value", $.SelectedTabId);
        $("#TabControl").data("kendoTabStrip").select($('#' + $.SelectedTabId));
    });

}(jQuery));