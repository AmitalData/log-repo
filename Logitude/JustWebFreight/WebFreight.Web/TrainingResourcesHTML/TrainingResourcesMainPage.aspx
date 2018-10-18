<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrainingResourcesMainPage.aspx.cs" Inherits="WebFreight.Web.TrainingResourcesHTML.TrainingResourcesMainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Help & Training Resources</title>

    <link href="../HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <link href="../HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css" />
    <link href="../HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css" />
    <link href="../HtmlHelpers/CSS/kendo.dataviz.min.css" rel="stylesheet" type="text/css" />
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <link href="../HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css" />

    <script src="../HtmlHelpers/JS/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/jquery.dateFormat-1.0.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/Logitude.Converters.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/Logitude.Entites.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/ContactActivityLog.js" type="text/javascript"></script>

    <style type="text/css">
        .ListItem tr td div, .ListItem tr td span, .ListItem tr td img {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .ListItem tr td {
            text-align: left;
        }

        .HelperListBoxItem:hover {
            background: gray;
        }

            .HelperListBoxItem:hover .TemplateItem {
                color: white;
            }

        .HelperListBoxItem {
            height: 25px;
            cursor: pointer;
            margin: 0 0 5px 0;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
        }

        .TemplateItem {
            padding: 0;
            margin: 0;
            vertical-align: central;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            text-align: left;
            vertical-align: central;
            height: 20px;
            line-height: 20px;
        }

        .HelpFilterListItem:hover {
            text-decoration: none;
        }

        .HelpFilterListItem {
            display: table-cell;
            vertical-align: middle;
            height: 20px;
            background: #E6E7E8;
            text-align: center;
            border-top: 1px solid #6A8299;
            border-bottom: 1px solid #6A8299;
            border-left: 1px solid #6A8299;
            cursor: default;
            font-family: "Lucida Sans Unicode";
            font-size: 11px;
            margin: 0px;
            color: #282E30;
            text-decoration: none;
        }

        td {
            position: relative;
            vertical-align: middle;
        }

        table {
            width: 100%;
            height: 100%;
            border-spacing: 0px;
            border-collapse: collapse;
            text-space-collapse: collapse;
            empty-cells: show;
        }

        .MediaFill {
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
        }

        .MediaFillAbsolute {
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            margin: auto;
        }

        .MarginAbsolute10 {
            position: absolute;
            top: 10px;
            bottom: 10px;
            left: 10px;
            right: 10px;
            margin: auto;
        }

        .TextTrimming {
            overflow: hidden;
            white-space: nowrap;
            text-overflow: ellipsis;
        }

        /*IE*/
        @media all and (-ms-high-contrast: none), (-ms-high-contrast: active) {
            td {
                position: static;
                vertical-align: middle;
            }

            .MediaFill {
                position: relative;
                width: 100%;
                height: 100%;
                min-width: 100%;
                min-height: 100%;
                max-width: 100%;
                max-height: 100%;
                float: left;
            }
        }
    </style>

</head>

<body>

    <form style="visibility: collapse;">
        <input id="SavedFilterId" />
        <input id="SavedSearchText" />
    </form>

    <form id="form1" runat="server">


        <div id="Container" class="MediaFillAbsolute">
            <div class="MarginAbsolute10" style="border: 1px solid #B9B9B9; border-radius: 8px;">
                <table>

                    <thead>
                        <tr style="height: 100px;">
                            <td style="vertical-align: top;">
                                <div style="margin: 10px;">
                                    <table>

                                        <tr style="height: 40px; vertical-align: central;">
                                            <td style="color: #1B90CB; font-weight: bold; font-size: 20px;">Help Center</td>

                                            <td style="width: 70px; font-size: 12px; color: grey; vertical-align: central;">Search:</td>

                                            <td style="width: 240px; vertical-align: central;">
                                                <input class="SearchBox" id="SearchBox" type="text" style="width: 240px;" />
                                                <div class="SearchIcon" id="SearchIcon" style="margin-left: 230px;"></div>
                                                <div class="SearchDeleteButton" id="SearchDeleteButton" style="margin-left: 230px;"></div>
                                            </td>
                                        </tr>

                                        <tr style="height: 40px; vertical-align: central;">

                                            <td style="vertical-align: central;">
                                                <table>
                                                    <tr>
                                                        <td style="width: 50px; vertical-align: central; font-size: 12px; color: grey;">View</td>

                                                        <td>
                                                            <div>
                                                                <ul id="HelpMenu">
                                                                    <li><a class="HelpFilterListItem" id="CAT" style="width: 89px; background: url('images/CAT_S.png')"></a></li>
                                                                    <li><a class="HelpFilterListItem" id="All" style="width: 49px; background: url('images/All_N.png')"></a></li>
                                                                    <li><a class="HelpFilterListItem" id="TYP" style="width: 59px; background: url('images/TYP_N.png'); border-right: 1px solid #6A8299;"></a></li>
                                                                </ul>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </table>
                                            </td>

                                            <td style="width: 70px; font-size: 12px; color: grey; vertical-align: central;">Language:</td>

                                            <td style="width: 240px; vertical-align: central;">
                                                <input id="LanguageComboBox" style="width: 200px;" />

                                                <a href="http://www.logitudeworld.com/faqs/?tab=setup" target="_blank" style="width: 30px; font-size: 16px; margin-left: 10px; vertical-align: middle;">FAQ </a>
                                            </td>
                                        </tr>

                                    </table>
                                </div>

                            </td>
                        </tr>
                    </thead>

                    <tbody>
                        <tr class="Page" id="ListGrid" style="display: none;">
                            <td style="vertical-align: top;">
                                <div class="MediaFill">
                                    <table>
                                        <tr>
                                            <td style="width: 5px;"></td>

                                            <td style="vertical-align: top; width: 600px; min-width:600px;">
                                                <div class="MediaFill">
                                                    <div class="ListBoxContainer MediaFillAbsolute" style="overflow: auto;">
                                                        <div id="ListHelpersListBox" class="ListBox" />
                                                    </div>
                                                </div>
                                            </td>

                                            <td style="width: 50px;"></td>

                                            <td style="vertical-align: top;">

                                                <div>
                                                    <div style="width: 550px; height: 25px;">
                                                        <span>
                                                            <img id="MyImage" style="height: 22px; width: 25px; padding: 0px; margin: 0px; border: hidden; display:none;" /></span>
                                                        <span class="ValueTextStyle" id="MyTitle" style="display: inline-block; width: 450px; font-size: 18px; vertical-align: top; margin-top: 3px"></span>
                                                    </div>

                                                    <iframe id="MyFrame" class="MyFrame" width="800" style="vertical-align: top; border: hidden;"></iframe>
                                                </div>
                                            </td>

                                            <td style="width: 5px;"></td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>

                        <tr class="Page" id="CategoryGrid" style="display: none;">
                            <td>
                                <div class="MediaFill">
                                    <table>

                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="width: 5px;"></td>

                                                        <td style="vertical-align: top; min-height: 100px; width: 550px;">
                                                            <table>
                                                                <tr style="height: 25px;">
                                                                    <td>
                                                                        <div class="BlueTextStyle" style="font-size: 18px; vertical-align: top;">Operational </div>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <div class="MediaFill">
                                                                            <div class="ListBoxContainer MediaFillAbsolute" style="overflow: auto;">
                                                                                <div id="OperationalHelpersListBox" class="ListBox" />
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                        <td style="width: 100px;"></td>

                                                        <td style="vertical-align: top; min-height: 100px; width: 550px;">
                                                            <table>
                                                                <tr style="height: 25px;">
                                                                    <td>
                                                                        <div class="BlueTextStyle" style="font-size: 18px; vertical-align: top;">Accounting </div>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <div class="MediaFill">
                                                                            <div class="ListBoxContainer MediaFillAbsolute" style="overflow: auto;">
                                                                                <div id="AccountingHelpersListBox" class="ListBox" />
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr style="height: 30px;">
                                            <td></td>
                                        </tr>

                                        <tr style="height: 200px;">
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="width: 5px;"></td>

                                                        <td style="vertical-align: top; min-height: 100px; width: 550px;">
                                                            <table>
                                                                <tr style="height: 25px;">
                                                                    <td>
                                                                        <div class="BlueTextStyle" style="font-size: 18px; vertical-align: top;">CRM </div>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <div class="MediaFill">
                                                                            <div class="ListBoxContainer MediaFillAbsolute" style="overflow: auto;">
                                                                                <div id="CRMHelpersListBox" class="ListBox" />
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                        <td style="width: 100px;"></td>

                                                        <td style="vertical-align: top; min-height: 100px; width: 550px;">
                                                            <table>
                                                                <tr style="height: 25px;">
                                                                    <td>
                                                                        <div class="BlueTextStyle" style="font-size: 18px; vertical-align: top;">E-AWB </div>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <div class="MediaFill">
                                                                            <div class="ListBoxContainer MediaFillAbsolute" style="overflow: auto;">
                                                                                <div id="AWBHelpersListBox" class="ListBox" />
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                            </td>
                        </tr>

                        <tr class="Page" id="TypeGrid" style="display: none;">
                            <td>
                                <div class="MediaFill">
                                    <table>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="width: 5px;"></td>

                                                        <td style="vertical-align: top; min-height: 100px; width: 550px;">
                                                            <table>
                                                                <tr style="height: 25px;">
                                                                    <td>
                                                                        <div class="BlueTextStyle" style="font-size: 18px; vertical-align: top;">Videos </div>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <div class="MediaFill">
                                                                            <div class="ListBoxContainer MediaFillAbsolute" style="overflow: auto;">
                                                                                <div id="VideosHelpersListBox" class="ListBox" />
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                        <td style="width: 100px;"></td>

                                                        <td style="vertical-align: top; min-height: 100px; width: 550px;">
                                                            <table>
                                                                <tr style="height: 25px;">
                                                                    <td>
                                                                        <div class="BlueTextStyle" style="font-size: 18px; vertical-align: top;">Tutorials </div>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <div class="MediaFill">
                                                                            <div class="ListBoxContainer MediaFillAbsolute" style="overflow: auto;">
                                                                                <div id="TutorialsHelpersListBox" class="ListBox" />
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr style="height: 30px;">
                                            <td></td>
                                        </tr>

                                        <tr style="height: 200px;">
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="width: 5px;"></td>

                                                        <td style="vertical-align: top; min-height: 100px; width: 550px;">
                                                            <table>
                                                                <tr style="height: 25px;">
                                                                    <td>
                                                                        <div class="BlueTextStyle" style="font-size: 18px; vertical-align: top;">How To </div>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <div class="MediaFill">
                                                                            <div class="ListBoxContainer MediaFillAbsolute" style="overflow: auto;">
                                                                                <div id="HowToHelpersListBox" class="ListBox" />
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                        <td style="width: 100px;"></td>

                                                        <td style="vertical-align: top; min-height: 100px; width: 550px;">
                                                            <table>
                                                                <tr style="height: 25px;">
                                                                    <td>
                                                                        <div class="BlueTextStyle" style="font-size: 18px; vertical-align: top;">Release Notes </div>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <div class="MediaFill">
                                                                            <div class="ListBoxContainer MediaFillAbsolute" style="overflow: auto;">
                                                                                <div id="ReleaseNotesHelpersListBox" class="ListBox" />
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </tbody>

                </table>
            </div>
        </div>

    </form>

    <script type="text/x-kendo-tmpl" id="HelperListBoxItemDataTemplate">
        <div class="HelperListBoxItem" >
            <div class="ListItem" style="width:100%; height: 25px;" id="#= Code #" OnClick="OpenDoc(id)">
                <div style="margin:3px 3px 0px 3px;">

                    <div style="vertical-align:central;">
                        <div style="display:inline-block; width:30px;"><img style="height:22px; width:20; padding:0px; margin:0px;" src="#= TypeSRC #"/></div>     
                        
                        <div style="display:inline-block; width:400px;">     
                             <div class="TemplateItem ValueTextStyle TextTrimming" style="display:inline-block; font-size:14px;">${Name}</div>   
                             <div class="TemplateItem ValueTextStyle" style="display:inline-block; width:30; font-size:11px;">${Duration}</div> 
                        </div>
                       
                        <div class="TemplateItem ValueTextStyle" style="display:inline-block; width:80px; font-size:11px;">${UpdateDate}</div>                           
                        
                    </div>

                </div>
            </div>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="ListBoxItemDataTemplate">
        <div class="HelperListBoxItem" >
            <div class="ListItem" style="width:100%; height: 25px;" id="#= Code #" OnClick="OpenListDoc(id)">
                <div style="margin:3px 3px 0px 3px;">

                    <div style="vertical-align:central;">
                        <div style="display:inline-block; width:30px;"><img style="height:22px; width:20; padding:0px; margin:0px;" src="#= TypeSRC #"/></div>                            
                       
                        <div style="display:inline-block; width:400px;">     
                             <div class="TemplateItem ValueTextStyle TextTrimming" style="display:inline-block; font-size:14px;">${Name}</div>   
                             <div class="TemplateItem ValueTextStyle" style="display:inline-block; width:30; font-size:11px;">${Duration}</div> 
                        </div>

                        <div class="TemplateItem ValueTextStyle" style="display:inline-block; width:80px; font-size:11px;">${UpdateDate}</div>    
                    </div>

                </div>
            </div>
        </div>
    </script>

    <script type="text/javascript" src="TrainingResourcesViewModel.js"></script>

    <script type="text/javascript">

        function OpenDoc(myCode) {

            if ($.DataResult != null) {

                for (var i = 0; i < $.DataResult.length; i++) {

                    var item = $.DataResult[i];
                    if (item.Code == myCode) {
                        $.SendContactActivity($.CurrentEmail, "Help Center", "How-To", $.CurrentTenant, null);

                        if (item.Type == "VID") {

                            window.open(item.VideoURL, '_blank');
                        }

                        else {

                            window.open("../WebPages/HowToDownloadPage.aspx?id=" + myCode, '_blank');
                        }

                        break;
                    }
                }
            }
        }

    </script>

    <script type="text/javascript">

        function OpenListDoc(myCode) {

            if ($("#MyFrame").length) {

                if ($.DataResult != null) {

                    for (var i = 0; i < $.DataResult.length; i++) {

                        var item = $.DataResult[i];
                        if (item.Code == myCode) {
                            $.SendContactActivity($.CurrentEmail, "Help Center", "How-To", $.CurrentTenant, null);

                            $("#MyTitle").text(item.Name);
                            $("#MyImage").css({ "display": "inline",});

                            if (item.Type == "TUT") {

                                $("#MyImage").attr('src', "../TrainingResourcesHTML/images/tutorial-icon.png");
                            }

                            else if (item.Type == "VID") {

                                $("#MyImage").attr('src', "../TrainingResourcesHTML/images/video-icon.png");
                            }

                            else if (item.Type == "HOW") {

                                $("#MyImage").attr('src', "../TrainingResourcesHTML/images/howto-icon.png");
                            }

                            else if (item.Type == "REL") {

                                $("#MyImage").attr('src', "../TrainingResourcesHTML/images/refresh-icon.png");
                            }

                            if (item.Type == "VID") {

                                if (item.VideoURL != null) {

                                    var url = item.VideoURL;
                                    url = url.replace("http://youtu.be/", "https://www.youtube.com/");
                                    url = url.replace("https://www.youtube.com/", "https://www.youtube.com/embed/");
                                    url = url.replace("watch?v=", "");
                                    url = url.replace("&hd=1", "?hd=1");

                                    $("#MyFrame").attr('src', url);
                                    $("#MyFrame").attr('height', 500);
                                }
                            }

                            else {

                                $("#MyFrame").attr('height', 700);

                                if (item.FileName.substr(-4) == ".pdf") {

                                    var url = "../WebPages/HowToDownloadPage.aspx?id=" + myCode;
                                    $("#MyFrame").attr('src', url);
                                }

                                else if (item.FileName.substr(-5) == ".html") {

                                    var url = "html/" + item.FileName;
                                    $("#MyFrame").attr('src', url);
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

    </script>

</body>
</html>
