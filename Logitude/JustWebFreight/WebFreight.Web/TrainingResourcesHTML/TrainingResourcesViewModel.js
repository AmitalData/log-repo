
(function (jQuery) {

    jQuery.DataResult = null;
    jQuery.SelectedFilter = "CAT";
    jQuery.SelectedLanguageCode = null;
    jQuery.SearchText = null;
    jQuery.SearchTimer = null;
    jQuery.watermark = "Search...";
    jQuery.WorkEnvironment = null;

    jQuery.BuildCategoryHelpersList = (function (helpers) {

        var OPEHelpersList = [];
        var ACCHelpersList = [];
        var CRMHelpersList = [];
        var AWBHelpersList = [];

        $.each(helpers, function (index, helper) {

            var item = new HelperListClass();
            item.Code = helper.Code;
            item.Name = helper.Name;
            item.CreateDate = $.Convert.ToShortMonthYear(helper.CreateDate);
            item.UpdateDate = $.Convert.ToShortMonthYear(helper.UpdateDate);
            item.Language = helper.Language;
            item.Type = helper.Type;
            item.Category = helper.Category;
            item.VideoURL = helper.VideoURL;

            if (helper.Duration != "" && helper.Duration != null) {
                item.Duration = "( " + $.trim(helper.Duration) + " )";
            }

            item.FileName = helper.FileName;

            if (helper.Type == "TUT") {

                item.TypeSRC = "../TrainingResourcesHTML/images/tutorial-icon.png";
            }

            else if (helper.Type == "VID") {

                item.TypeSRC = "../TrainingResourcesHTML/images/video-icon.png";
            }

            else if (helper.Type == "HOW") {

                item.TypeSRC = "../TrainingResourcesHTML/images/howto-icon.png";
            }

            else if (helper.Type == "REL") {

                item.TypeSRC = "../TrainingResourcesHTML/images/refresh-icon.png";
            }

            if (helper.Category == "OPE") {
                OPEHelpersList.push(item);
            }

            else if (helper.Category == "ACC") {
                ACCHelpersList.push(item);
            }

            else if (helper.Category == "CRM") {
                CRMHelpersList.push(item);
            }

            else if (helper.Category == "AWB") {
                AWBHelpersList.push(item);
            }
        });

        $("#OperationalHelpersListBox").html("");
        $("#OperationalHelpersListBox").kendoListView(
            {
                scrollable: false,
                dataSource: OPEHelpersList,
                template: kendo.template($("#HelperListBoxItemDataTemplate").html())
            });

        $("#AccountingHelpersListBox").html("");
        $("#AccountingHelpersListBox").kendoListView(
            {
                scrollable: false,
                dataSource: ACCHelpersList,
                template: kendo.template($("#HelperListBoxItemDataTemplate").html())
            });

        $("#CRMHelpersListBox").html("");
        $("#CRMHelpersListBox").kendoListView(
            {
                scrollable: false,
                dataSource: CRMHelpersList,
                template: kendo.template($("#HelperListBoxItemDataTemplate").html())
            });

        $("#AWBHelpersListBox").html("");
        $("#AWBHelpersListBox").kendoListView(
            {
                scrollable: false,
                dataSource: AWBHelpersList,
                template: kendo.template($("#HelperListBoxItemDataTemplate").html())
            });
    });

    jQuery.BuildTypeHelpersList = (function (helpers) {

        var VIDHelpersList = [];
        var TUTHelpersList = [];
        var HOWHelpersList = [];
        var RELHelpersList = [];

        $.each(helpers, function (index, helper) {

            var item = new HelperListClass();
            item.Code = helper.Code;
            item.Name = helper.Name;
            item.CreateDate = $.Convert.ToShortMonthYear(helper.CreateDate);
            item.UpdateDate = $.Convert.ToShortMonthYear(helper.UpdateDate);
            item.Language = helper.Language;
            item.Type = helper.Type;
            item.Category = helper.Category;
            item.VideoURL = helper.VideoURL;

            if (helper.Duration != "" && helper.Duration != null) {
                item.Duration = "( " + $.trim(helper.Duration) + " )";
            }

            item.FileName = helper.FileName;

            if (helper.Type == "TUT") {

                item.TypeSRC = "../TrainingResourcesHTML/images/tutorial-icon.png";
            }

            else if (helper.Type == "VID") {

                item.TypeSRC = "../TrainingResourcesHTML/images/video-icon.png";
            }

            else if (helper.Type == "HOW") {

                item.TypeSRC = "../TrainingResourcesHTML/images/howto-icon.png";
            }

            else if (helper.Type == "REL") {

                item.TypeSRC = "../TrainingResourcesHTML/images/refresh-icon.png";
            }

            if (helper.Type == "VID") {
                VIDHelpersList.push(item);
            }

            else if (helper.Type == "TUT") {
                TUTHelpersList.push(item);
            }

            else if (helper.Type == "HOW") {
                HOWHelpersList.push(item);
            }

            else if (helper.Type == "REL") {
                RELHelpersList.push(item);
            }
        });

        $("#VideosHelpersListBox").html("");
        $("#VideosHelpersListBox").kendoListView(
            {
                scrollable: false,
                dataSource: VIDHelpersList,
                template: kendo.template($("#HelperListBoxItemDataTemplate").html())
            });

        $("#TutorialsHelpersListBox").html("");
        $("#TutorialsHelpersListBox").kendoListView(
            {
                scrollable: false,
                dataSource: TUTHelpersList,
                template: kendo.template($("#HelperListBoxItemDataTemplate").html())
            });

        $("#HowToHelpersListBox").html("");
        $("#HowToHelpersListBox").kendoListView(
            {
                scrollable: false,
                dataSource: HOWHelpersList,
                template: kendo.template($("#HelperListBoxItemDataTemplate").html())
            });

        $("#ReleaseNotesHelpersListBox").html("");
        $("#ReleaseNotesHelpersListBox").kendoListView(
            {
                scrollable: false,
                dataSource: RELHelpersList,
                template: kendo.template($("#HelperListBoxItemDataTemplate").html())
            });
    });

    jQuery.LoadData = (function () {

        function TrainingResourcesFilters() {

            this.FilterId = ($.trim($.SelectedFilter) == "") ? null : $.SelectedFilter;
            this.Language = ($.trim($.SelectedLanguageCode) == "") ? null : $.SelectedLanguageCode;
            this.SearchField = ($.trim($.SearchText) == "" || $.trim($.SearchText) == $.watermark) ? null : $.trim($.SearchText);
        };

        var filters = new TrainingResourcesFilters();

        var url = "../api/trainingresources";

        $.ajax({
            url: url,
            data: JSON.stringify(filters),
            type: 'POST',
            beforeSend: function (request) {
                if ($.Token) {
                    request.setRequestHeader("Token", $.Token);
                }
            },
            contentType: 'application/json',

            success: function (result) {

                $.DataResult = result;

                switch ($.SelectedFilter) {

                    case "All": {

                        $("#ListHelpersListBox").html("");
                        $("#ListHelpersListBox").kendoListView(
                            {
                                scrollable: false,
                                dataSource: { data: BuildHelpersList(result) },
                                template: kendo.template($("#ListBoxItemDataTemplate").html()),
                            });

                        break;
                    }

                    case "CAT": {
                        $.BuildCategoryHelpersList(result);

                        break;
                    }

                    case "TYP": {
                        $.BuildTypeHelpersList(result);

                        break;
                    }
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    });

    jQuery.SearchTextChanged = (function () {

        if ($("#SearchBox").val().length == 0 || $("#SearchBox").val() == $.watermark) {
            $('#SearchDeleteButton').hide();
        }

        else {
            $('#SearchDeleteButton').show();
        }

        $.SearchText = $("#SearchBox").val();
        $('#SavedSearchText').attr("value", $.SearchText);

        if ($.SearchTimer != null) {
            clearTimeout($.SearchTimer);
        }

        $.SearchTimer = setTimeout(function () { $.LoadData() }, 500);
    });

    $('.SearchDeleteButton').hide();
    $('#SearchBox').val($.watermark).addClass('watermark');

    $('.SearchBox').blur(function () {

        if ($(this).val().length == 0) {
            $(this).val($.watermark).addClass('watermark');
            $('#SearchIcon').show();
            $('#SearchDeleteButton').hide();
        }
    });
    $('.SearchBox').focus(function () {

        if ($(this).val() == $.watermark) {
            $(this).val('').removeClass('watermark');
            $('#SearchIcon').hide();
        }
    });
    $('.SearchBox').keyup(function () {
        $.SearchTextChanged();
    });

    $(".SearchDeleteButton").click(function () {

        $('#SearchIcon').show();
        $('#SearchBox').attr("value", "");
        $('#SearchBox').val($.watermark).addClass('watermark');
        $.SearchTextChanged("SearchBox");
    });

    jQuery.SelectPage = (function () {
        $(".Page").hide();

        switch ($.SelectedFilter) {
            case "All": {
                $("#ListGrid").show();
                break;
            }

            case "CAT": {
                $("#CategoryGrid").show();
                break;
            }

            case "TYP": {
                $("#TypeGrid").show();
                break;
            }
        }
    });

    $(".HelpFilterListItem").mouseenter(function () {

        var filterId = $(this).attr('id');

        if ($.SelectedFilter != filterId) {
            var targetId = "#" + filterId;
            var sourceURL = $(targetId).css("background-image");
            var targetURL = sourceURL.replace("N.png", "O.png");
            $(targetId).css("background", targetURL);
        }
    });
    $(".HelpFilterListItem").mouseleave(function () {

        var filterId = $(this).attr('id');

        if ($.SelectedFilter != filterId) {
            var targetId = "#" + filterId;
            var sourceURL = $(targetId).css("background-image");
            var targetURL = sourceURL.replace("O.png", "N.png");
            $(targetId).css("background", targetURL);
        }
    });
    $(".HelpFilterListItem").click(function () {

        var filterId = $(this).attr('id');
        if ($.SelectedFilter != filterId) {

            $.SelectedFilter = filterId;

            var targetId = "#" + $.SelectedFilter;
            var sourceURL = $(targetId).css("background-image");
            var targetURL = sourceURL.replace("O.png", "S.png");

            if (sourceURL == targetURL) {
                targetURL = sourceURL.replace("O.png", "S.png");
            }

            $("#All").css("background", "url('images/All_N.png')");
            $("#CAT").css("background", "url('images/CAT_N.png')");
            $("#TYP").css("background", "url('images/TYP_N.png')");
            $(targetId).css("background", targetURL);

            $.SelectPage();
            $.LoadData();
        }
    });

    jQuery.GetLogginData = (function () {
        var url = "../api/commondata/?settingId=" + "1";

        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/json',

            success: function (result) {
                $.WorkEnvironment = result;

                $.FillLanguageComboBox();
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $.FillLanguageComboBox();
            }
        });
    });

    jQuery.FillLanguageComboBox = (function () {
        if ($.WorkEnvironment == "cloud") {
            $("#LanguageComboBox").kendoComboBox({
                placeholder: "Select language",
                dataTextField: "text",
                dataValueField: "value",
                dataSource: [
                    { text: "English", value: "EN" },
                    { text: "French", value: "FR" },
                    { text: "Spanish", value: "SP" },
                    { text: "Hebrew", value: "HE" },
                ],
                filter: "contains",
                suggest: true,
                index: 4,
                select: function (e) {

                },

                change: function (e) {

                    $.SelectedLanguageCode = this.value();

                    if ($.SelectedLanguageCode == "") {
                        $.SelectedLanguageCode = null;
                    }

                    $.LoadData();
                }
            });
        }

        else {
            $("#LanguageComboBox").kendoComboBox({
                placeholder: "Select language",
                dataTextField: "text",
                dataValueField: "value",
                dataSource: [
                    { text: "English", value: "EN" },
                    { text: "French", value: "FR" },
                    { text: "Spanish", value: "SP" },
                ],
                filter: "contains",
                suggest: true,
                index: 3,
                select: function (e) {

                },

                change: function (e) {

                    $.SelectedLanguageCode = this.value();

                    if ($.SelectedLanguageCode == "") {
                        $.SelectedLanguageCode = null;
                    }

                    $.LoadData();
                }
            });
        }
    });

    $(document).ready(function () {
        var hash = $(location).attr('href');
        var arr = hash.split('tempId=');
        $.Token = arr[1];

        if ($.Token) {
            $.GetLogginData();
            $.SelectPage();
            $.LoadData();
        }
    });

}(jQuery));
