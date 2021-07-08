<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SharedLogisticPage.aspx.cs" Inherits="WebFreight.Web.SharedLogisticPage" %>

<!DOCTYPE html>

<html>
<head runat="server">

    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <title>Shared Logistic</title>

    <link href="css/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="css/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-3.5.1.min.js" type="text/javascript"></script>
    <script src="js/jquery.dateFormat-1.0.js" type="text/javascript"></script>
    <script src="js/kendo.all.min.js" type="text/javascript"></script>
    <script src="js/knockout-3.5.1.js" type="text/javascript"></script>
    <script src="js/knockout-kendo.min.js" type="text/javascript"></script>

    <link href="HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css" />
    <script src="HtmlHelpers/JS/Logitude.Converters.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/Logitude.Entites.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/LogitudeTools.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/ContactActivityLog.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>

    <style type="text/css">
        .ShipmentListBoxItem:hover, .InvoiceListBoxItem:hover, .QuotesRequestsListBoxItem:hover {
            background: url("HtmlHelpers/Images/Bars_Images/BigBlueBar.png");
            border-color: #3BB3E2;
        }

        .ListItem tr td div, .ListItem tr td span, .ListItem tr td img {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        /*.ListItem tr td span
    {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }*/

        .ListItem tr td {
            text-align: left;
        }

        .ShipmentListBoxItem {
            height: 75px;
            cursor: pointer;
            border: 1px solid #D1D1D1;
            margin: 0 0 5px 0;
            background: #F7F7F7;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
        }

        .InvoiceListBoxItem {
            height: 75px;
            cursor: pointer;
            border: 1px solid #D1D1D1;
            margin: 0 0 5px 0;
            background: #F7F7F7;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
        }

        .QuotesRequestsListBoxItem {
            height: 75px;
            border: 1px solid #D1D1D1;
            margin: 0 0 5px 0;
            background: #F7F7F7;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
        }



        .k-tabstrip .k-state-active, .k-tabstrip .k-state-active:hover {
            background: white;
            /*border: 0px;*/
            border-bottom: 0px;
        }

        .TemplateItem {
            padding: 0;
            margin: 0;
            vertical-align: central;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            text-align: left;
            vertical-align: top;
            height: 20px;
            line-height: 20px;
        }

        .Hyperlink:hover:not(:disabled), .Hyperlink:focus:not(:disabled) {
            color: #1E8DC4;
        }

        .Hyperlink {
            height: 21px !important;
            width: auto !important;
            line-height: 21px !important;
            background: none !important;
            border: none !important;
            padding: 0 !important;
            padding-left: 10px !important;
            color: #282E30;
            font-size: 12px !important;
            cursor: pointer;
            display: block !important;
            white-space: nowrap;
        }
        
        .SelectOption {
            width: 100px;
            text-align: center;
            height: 12px;
            border: 1px solid #6A8299;
            color: #45494A;
            font-size: 11px;
            cursor: pointer;
            background: linear-gradient( 180deg , rgb(255, 255, 255) 0%, rgb(186, 206, 227) 100%);
            text-shadow: 1px 1px white;
        }

        .CheckBOX {
            width: 16px;
            height: 16px;
            cursor: pointer;
            display: block;
            background: white;
            border: 1px solid #AAAAAA;
            user-select: none;
            -ms-user-select: none;
            -moz-user-select: none;
            -webkit-user-select: none;
            box-shadow: inset 0 0 3px #AAAAAA;
            line-height: 15px;
            text-indent: 20px;
            font-size: 11px;
            color: #6E7172;
            position: absolute;
        }
    </style>

</head>

<body>

    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>

    <form style="visibility: collapse;">
        <input id="SavedIsDataCountLoaded" />
        <input id="SavedSelectedTabId" />
        <input id="SavedDirectionId_SHI" />
        <input id="SavedTransportId_SHI" />
        <input id="SavedShipmentLevel" />
        <input id="SavedSelectedQuery_SHI" />
        <input id="SavedSelectedQuery_INV" />
        <input id="SavedSelectedQuery_QUOTESREQUESTS" />


        <input id="SavedSearchText_SHI" />
        <input id="SavedSearchText_INV" />
        <input id="SavedSearchText_QUOTESREQUESTS" />


        <input id="TokenInput" runat="server" />
        <input id="LoginInput" runat="server" />
    </form>

    <div id="ContainerHeader" style="position: absolute; top: 0px; z-index: 0; width: 100%; height: 65px; background: url('HtmlHelpers/Images/Bars_Images/HeaderBar.png') repeat-x;"></div>

    <div id="Container" style="position: absolute; top: 0px; z-index: 3; width: 100%">
        <table style="height: 100%;">

            <thead>
                <tr style="height: 35px;">
                    <td style="vertical-align: top;">

                        <table style="margin: 5px 0 0 0;">
                            <tr>
                                <td style="width: 5px;"></td>

                                <td id="companyLogoArea" style="width: 50px;">
                                    <img id="companyLogo" src="HtmlHelpers/Images/Icons/Logo.png" style="width: 50px; height: 35px; vertical-align: bottom; position: absolute; top: 2px;" />
                                </td>



                                <td style="vertical-align: central; text-indent: 5px;">
                                    <span id="CompanyText" style="font-size: 13px; color: #45494A"></span>
                                </td>

                                <td style="text-align: right; vertical-align: top;">
                                    <div style="margin-top: -3px;">
                                        <span style="font-size: 11px; color: #45494A" id="MemberText"></span>
                                        <span style="font-size: 11px; color: #838889" id="MemberCardText"></span>
                                    </div>
                                </td>

                                <td style="width: 22px; vertical-align: top;">
                                    <div style="margin-top: -5px;" id="SignOutButton"></div>
                                </td>

                                <td style="width: 5px;"></td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </thead>

            <tfoot>
                <tr style="height: 30px;">
                    <td>
                        <table style="width: 100%; height: 100%">
                            <tr>
                                <td style="width: 20px;">
                                    <div></div>
                                </td>

                                <td class="Footer_LOG" style="width: 190px; display: none;">
                                    <a class="PoweredArea" href="http://www.logitudeworld.com" target="_blank" style="padding: 0; margin: 0; cursor: pointer; text-decoration: none;">
                                        <table style="height: 100%">
                                            <tr>
                                                <td style="width: 65px; vertical-align: central; white-space: nowrap;">
                                                    <p style="font-size: 11px; color: #27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</p>
                                                </td>
                                                <td style="width: 40px; vertical-align: central;">
                                                    <img src="HtmlHelpers/Images/Icons/LogitudeLogo.png" style="width: 40px; height: 20px;" /></td>
                                                <td style="width: 85px; vertical-align: central;">
                                                    <img src="HtmlHelpers/Images/Icons/Logitude.png" style="width: 79px; height: 25px;" /></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </a>
                                </td>

                                <td class="Footer_UNI" style="width: 190px; display: none;">
                                    <a class="PoweredArea" href="http://www.amital.co.il" target="_blank" style="padding: 0; margin: 0; cursor: pointer; text-decoration: none;">
                                        <table style="height: 100%">
                                            <tr>
                                                <td style="width: 65px; vertical-align: top; padding-top: 7px; white-space: nowrap; font-size: 11px; color: #27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</td>
                                                <td style="width: 28px; vertical-align: middle;">
                                                    <img src="images/ApplicationLogo/UnifreightSmallLogo.png" style="width: 28px; height: 28px;" /></td>
                                                <td style="width: 85px; vertical-align: middle; white-space: nowrap; font-size: 14px; color: #7F7F7F; font-weight: bold; font-family: 'Lucida Sans Unicode';">Unifreight Cloud Services</td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </a>
                                </td>

                                <td>
                                    <div></div>
                                </td>

                                <td style="width: 20px;">
                                    <div></div>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </tfoot>

            <tbody>
                <tr>
                    <td style="vertical-align: top;">
                        <div>

                            <div id="mainTabsDiv">

                                <ul>
                                    <li id="TAB_SHI">
                                        <div>
                                            <span>
                                                <img class="TabImage" src="HtmlHelpers/Images/Tabs_Images/Shipments-N.png" /></span>
                                            <span>Shipments</span>
                                        </div>
                                    </li>

                                    <li id="TAB_INV">
                                        <div>
                                            <span>
                                                <img class="TabImage" src="HtmlHelpers/Images/Tabs_Images/Invoices-N.png" /></span>
                                            <span>Invoices</span>
                                        </div>
                                    </li>

                                    <li id="TAB_QUOTESREQUESTS">
                                        <div>
                                            <span>Quotes Requests</span>
                                        </div>
                                    </li>

                                    <li id="TAB_REPORTS">
                                        <div>
                                            <span>Reports</span>
                                        </div>
                                    </li>

                                </ul>







                                <%-- Shipments Tab --%>
                                <div class="tabPage" style="margin: 0 -1px;">
                                    <div style="margin-top: 5px">

                                        <div style="width: 100%; height: 10px;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px;">
                                                <tr>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/top-left.png') no-repeat"></td>
                                                    <td style="background: url('HtmlHelpers/Images/bars_Images/top-middle.png') repeat-x"></td>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/top-right.png') no-repeat"></td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="tabPageContent" style="width: 100%; position: relative; margin: -4px 0 -1px 0;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px; border: 0px;">
                                                <tr>
                                                    <td style="padding: 0; width: 1px; height: 100%; vertical-align: top;">
                                                        <div style="width: 1px; height: 100%; background: #D1D1D1; margin-left: 4px; margin-top: 4px;"></div>
                                                    </td>

                                                    <td style="padding: 0; vertical-align: top;">
                                                        <table style="width: 100%; border: 0; border-collapse: collapse; border-spacing: 0;">

                                                            <tr style="height: 25px;">

                                                                <td style="width: 143px; background: url('HtmlHelpers/Images/Bars_Images/QueryArea.png') repeat-y; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0; padding: 0;">
                                                                    <div style="height: 25px; width: 143px; font-size: 16px; font-family: 'Lucida Sans Unicode'; color: #45494A; text-indent: 10px; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0;">
                                                                        Views

                                                                    </div>
                                                                </td>

                                                                <td style="width: 10px; padding: 0;">
                                                                    <%--<div style="height:38px; width:1px; overflow:hidden; background:#D1D1D1;"></div>--%>
                                                                </td>

                                                                <td style="vertical-align: top; padding: 0;">
                                                                    <table style="margin-top: 5px;">
                                                                        <tr>
                                                                            <td>
                                                                                <div style="font-size: 16px; font-family: 'Lucida Sans Unicode'; color: #1B90CB;">
                                                                                    <span id="ShipmentsRefreshButton">
                                                                                        <img style="width: 16px; height: 16px; cursor: pointer; margin-bottom: -3px;" src="HtmlHelpers/Images/refresh.png" /></span>
                                                                                    <span id="ShipmentsQueryTitle">All Shipments</span>
                                                                                    <span id="ShipmentsQueryCount">(0)</span>
                                                                                </div>
                                                                            </td>

                                                                            <td style="width: 200px;">
                                                                                <input class="SearchBox" id="SearchBox_SHI" type="text" style="width: 200px; margin: 0; z-index: 0;" />
                                                                                <div class="SearchIcon" id="SearchIcon_SHI"></div>
                                                                                <div class="SearchDeleteButton" id="SearchDeleteButton_SHI"></div>
                                                                            </td>

                                                                            <td style="width: 10px;"></td>

                                                                            <td style="width: 55px;" class="LabelTextStyle">Filter by: </td>

                                                                            <td style="width: 107px;">
                                                                                <div>
                                                                                    <ul id="TransportMenu">
                                                                                        <li><a class="FilterListItem" id="All_Transport" style="background: url('HtmlHelpers/Images/Filter_Images/All.S.png')"></a></li>
                                                                                        <li><a class="FilterListItem" id="A_Transport" style="background: url('HtmlHelpers/Images/Filter_Images/Air.N.png')" title="Air"></a></li>
                                                                                        <li><a class="FilterListItem" id="O_Transport" style="background: url('HtmlHelpers/Images/Filter_Images/Ocean.N.png')" title="Ocean"></a></li>
                                                                                        <li><a class="FilterListItem" id="I_Transport" style="background: url('HtmlHelpers/Images/Filter_Images/Inland.N.png')" title="Inland"></a></li>
                                                                                    </ul>
                                                                                </div>
                                                                            </td>

                                                                            <td style="width: 5px;"></td>

                                                                            <td style="width: 162px">
                                                                                <div>
                                                                                    <ul id="DirectionMenu">
                                                                                        <li><a class="FilterListItem" id="All_Direction" style="background: url('HtmlHelpers/Images/Filter_Images/All.S.png')"></a></li>
                                                                                        <li><a class="FilterListItem" id="E_Direction" style="background: url('HtmlHelpers/Images/Filter_Images/Export.N.png')" title="Export"></a></li>
                                                                                        <li><a class="FilterListItem" id="I_Direction" style="background: url('HtmlHelpers/Images/Filter_Images/Import.N.png')" title="Import"></a></li>
                                                                                        <li><a class="FilterListItem" id="R_Direction" style="background: url('HtmlHelpers/Images/Filter_Images/Drop.N.png')" title="Drop"></a></li>
                                                                                        <li><a class="FilterListItem" id="D_Direction" style="background: url('HtmlHelpers/Images/Filter_Images/Domestic.N.png')" title="Domestic"></a></li>
                                                                                        <li><a class="FilterListItem" id="C_Direction" style="background: url('HtmlHelpers/Images/Filter_Images/CustomsImport.N.png')" title="Customs Import"></a></li>
                                                                                    </ul>
                                                                                </div>
                                                                            </td>

                                                                            <td style="width: 5px;"></td>

                                                                            <td style="width: 82px; display: none;">
                                                                                <div>
                                                                                    <ul id="ShipmentLevelMenu">
                                                                                        <li><a class="FilterListItem" id="All_ShipmentLevel" style="background: url('HtmlHelpers/Images/Filter_Images/All.S.png')"></a></li>
                                                                                        <li><a class="FilterListItem" id="D_ShipmentLevel" style="background: url('HtmlHelpers/Images/Filter_Images/Filter_N.png') repeat-x; font-family: 'Lucida Sans Unicode'; font-size: 11px; color: #6E7172;" title="Direct">
                                                                                            <div>D</div>
                                                                                        </a></li>
                                                                                        <li><a class="FilterListItem" id="H_ShipmentLevel" style="background: url('HtmlHelpers/Images/Filter_Images/Filter_N.png') repeat-x; font-family: 'Lucida Sans Unicode'; font-size: 11px; color: #6E7172;" title="House">
                                                                                            <div>H</div>
                                                                                        </a></li>
                                                                                    </ul>
                                                                                </div>
                                                                            </td>

                                                                            <td style="width: 0px;"></td>

                                                                        </tr>
                                                                    </table>
                                                                </td>

                                                            </tr>

                                                            <tr>

                                                                <td class="box" style="background: url('HtmlHelpers/Images/Bars_Images/QueryArea.png') repeat-y; padding: 0; border-radius: 0 0 0 5px; -moz-border-radius: 0 0 0 5px; -webkit-border-radius: 0 0 0 5px;">
                                                                    <div style="margin-top: 5px; background: url(HtmlHelpers/Images/Bars_Images/Queries.png) repeat-y; border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC;">
                                                                        <div class="HyperLinkQuery_SHI" id="Query_PRG_SHI">
                                                                            <div class="HyperLinkQueryContent">In Progress</div>
                                                                        </div>
                                                                        <div class="HyperLinkQuery_SHI" id="Query_ALL_SHI">
                                                                            <div class="HyperLinkQueryContent">All Shipments</div>
                                                                        </div>
                                                                    </div>
                                                                </td>

                                                                <td class="box" style="padding: 0; text-align: center;">
                                                                    <%--<div class="VerticalLine" style="width:1px; overflow:hidden; background:#D1D1D1;"></div>--%>
                                                                </td>

                                                                <td class="box" style="padding: 0;">
                                                                    <div class="ListBoxContainer" style="overflow: auto;">
                                                                        <div id="ShipmentsListBox" class="ListBox"></div>
                                                                    </div>
                                                                </td>

                                                            </tr>

                                                        </table>
                                                    </td>

                                                    <td style="padding: 0; width: 10px; height: 100%; vertical-align: top;">
                                                        <div style="width: 1px; height: 100%; background: #D1D1D1; margin-left: 5px; margin-top: 4px;"></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div style="width: 100%; height: 10px;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px;">
                                                <tr>
                                                    <td style="width: 15px; background: url('HtmlHelpers/images/bars_Images/bottom-left.png') no-repeat"></td>
                                                    <td style="background: url('HtmlHelpers/images/bars_Images/bottom-middle.png') repeat-x"></td>
                                                    <td style="width: 15px; background: url('HtmlHelpers/images/bars_Images/bottom-right.png') no-repeat"></td>
                                                </tr>
                                            </table>
                                        </div>

                                    </div>

                                    <div class="BusyIndicator" id="ShipmentsBusyIndicator"></div>
                                </div>

                                <%-- Invoices Tab --%>
                                <div class="tabPage" style="margin: 0 -1px;">
                                    <div style="margin-top: 5px">

                                        <div style="width: 100%; height: 13px;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px;">
                                                <tr>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/top-left.png') no-repeat"></td>
                                                    <td style="background: url('HtmlHelpers/images/bars_Images/top-middle.png') repeat-x"></td>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/top-right.png') no-repeat"></td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="tabPageContent" style="width: 100%; position: relative; margin: -7px 0 -1px 0;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px; border: 0px">
                                                <tr>
                                                    <td style="padding: 0; width: 1px; height: 100%; vertical-align: top;">
                                                        <div style="width: 1px; height: 100%; background: #D1D1D1; margin-left: 4px; margin-top: 4px;"></div>
                                                    </td>

                                                    <td style="padding: 0; vertical-align: top;">
                                                        <table style="width: 100%; border: 0; border-collapse: collapse; border-spacing: 0;">

                                                            <tr style="height: 25px;">

                                                                <td style="width: 143px; margin-top: -5px; background: url('HtmlHelpers/Images/Bars_Images/QueryArea.png') repeat-y; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0; padding: 0;">
                                                                    <div style="height: 25px; width: 143px; font-size: 16px; font-family: 'Lucida Sans Unicode'; color: #45494A; text-indent: 10px; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0;">
                                                                        Views

                                                                    </div>
                                                                </td>

                                                                <td style="width: 10px; padding: 0;">
                                                                    <%--<div style="height:38px; width:1px; overflow:hidden; background:#D1D1D1;"></div>--%>
                                                                </td>

                                                                <td style="vertical-align: top; padding: 0;">
                                                                    <table style="margin-top: 5px;">
                                                                        <tr>
                                                                            <td>
                                                                                <div style="font-size: 16px; font-family: 'Lucida Sans Unicode'; color: #1B90CB;">
                                                                                    <span id="InvoicesRefreshButton">
                                                                                        <img style="width: 16px; height: 16px; cursor: pointer; margin-bottom: -3px;" src="HtmlHelpers/Images/refresh.png" /></span>
                                                                                    <span id="InvoicesQueryTitle">All Invoices</span>
                                                                                    <span id="InvoicesQueryCount">(0)</span>
                                                                                </div>
                                                                            </td>

                                                                            <td style="width: 200px;">
                                                                                <input class="SearchBox" id="SearchBox_INV" type="text" style="width: 200px; margin: 0; z-index: 0" />
                                                                                <div class="SearchIcon" id="SearchIcon_INV"></div>
                                                                                <div class="SearchDeleteButton" id="SearchDeleteButton_INV"></div>
                                                                            </td>

                                                                            <td style="width: 5px;"></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>

                                                            </tr>

                                                            <tr>

                                                                <td class="box" style="background: url('HtmlHelpers/Images/Bars_Images/QueryArea.png') repeat-y; padding: 0; border-radius: 0 0 0 5px; -moz-border-radius: 0 0 0 5px; -webkit-border-radius: 0 0 0 5px;">
                                                                    <div style="margin-top: 5px; background: url(HtmlHelpers/Images/Bars_Images/Queries.png) repeat-y; border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC;">
                                                                        <div class="HyperLinkQuery_INV" id="Query_PRG_INV">
                                                                            <div class="HyperLinkQueryContent">Unpaid</div>
                                                                        </div>
                                                                        <div class="HyperLinkQuery_INV" id="Query_ALL_INV">
                                                                            <div class="HyperLinkQueryContent">All Invoices</div>
                                                                        </div>
                                                                    </div>
                                                                </td>

                                                                <td class="box" style="padding: 0; text-align: center;">
                                                                    <%--<div class="VerticalLine" style="width:1px; overflow:hidden; background:#D1D1D1;"></div>--%>
                                                                </td>

                                                                <td class="box" style="padding: 0;">
                                                                    <div class="ListBoxContainer" style="overflow: auto;">
                                                                        <div id="InvoicesListBox" class="ListBox"></div>
                                                                    </div>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </td>

                                                    <td style="padding: 0; width: 10px; height: 100%; vertical-align: top;">
                                                        <div style="width: 1px; height: 100%; background: #D1D1D1; margin-left: 5px; margin-top: 4px;"></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div style="width: 100%; height: 13px;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px;">
                                                <tr>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/bottom-left.png') no-repeat"></td>
                                                    <td style="background: url('HtmlHelpers/Images/bars_Images/bottom-middle.png') repeat-x"></td>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/bottom-right.png') no-repeat"></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>

                                    <div class="BusyIndicator" id="InvoicesBusyIndicator"></div>
                                </div>

                                <div class="tabPage" style="margin: 0 -1px;">
                                    <div style="margin-top: 5px">

                                        <div style="width: 100%; height: 13px;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px;">
                                                <tr>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/top-left.png') no-repeat"></td>
                                                    <td style="background: url('HtmlHelpers/images/bars_Images/top-middle.png') repeat-x"></td>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/top-right.png') no-repeat"></td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="tabPageContent" style="width: 100%; position: relative; margin: -7px 0 -1px 0;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px; border: 0px">
                                                <tr>
                                                    <td style="padding: 0; width: 1px; height: 100%; vertical-align: top;">
                                                        <div style="width: 1px; height: 100%; background: #D1D1D1; margin-left: 4px; margin-top: 4px;"></div>
                                                    </td>

                                                    <td style="padding: 0; vertical-align: top;">
                                                        <table style="width: 100%; border: 0; border-collapse: collapse; border-spacing: 0;">

                                                            <tr style="height: 25px;">

                                                                <td style="width: 143px; margin-top: -5px; background: url('HtmlHelpers/Images/Bars_Images/QueryArea.png') repeat-y; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0; padding: 0;">
                                                                    <div style="height: 25px; width: 143px; font-size: 16px; font-family: 'Lucida Sans Unicode'; color: #45494A; text-indent: 10px; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0;">
                                                                        Views

                                                                    </div>
                                                                </td>

                                                                <td style="width: 10px; padding: 0;">
                                                                    <%--<div style="height:38px; width:1px; overflow:hidden; background:#D1D1D1;"></div>--%>
                                                                </td>

                                                                <td style="vertical-align: top; padding: 0;">
                                                                    <table style="margin-top: 5px;">
                                                                        <tr>
                                                                            <td>
                                                                                <div style="font-size: 16px; font-family: 'Lucida Sans Unicode'; color: #1B90CB;">
                                                                                    <span id="QuotesRequestRefreshButton">
                                                                                        <img style="width: 16px; height: 16px; cursor: pointer; margin-bottom: -3px;" src="HtmlHelpers/Images/refresh.png" /></span>
                                                                                    <span id="QuotesRequestQueryTitle">All Quotes Requests</span>
                                                                                    <span id="QuotesRequestQueryCount">(0)</span>
                                                                                </div>
                                                                            </td>

                                                                            <td style="width: 200px;">
                                                                                <input class="SearchBox" id="SearchBox_QUOTESREQUESTS" type="text" style="width: 200px; margin: 0; z-index: 0" />
                                                                                <div class="SearchIcon" id="SearchIcon_QUOTESREQUESTS"></div>
                                                                                <div class="SearchDeleteButton" id="SearchDeleteButton_QUOTESREQUESTS"></div>
                                                                            </td>

                                                                            <td style="width: 5px;"></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>

                                                            </tr>

                                                            <tr>

                                                                <td class="box" style="background: url('HtmlHelpers/Images/Bars_Images/QueryArea.png') repeat-y; padding: 0; border-radius: 0 0 0 5px; -moz-border-radius: 0 0 0 5px; -webkit-border-radius: 0 0 0 5px;">
                                                                    <div style="margin-top: 5px; background: url(HtmlHelpers/Images/Bars_Images/Queries.png) repeat-y; border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC;">
                                                                        <div class="HyperLinkQuery_QUOTESREQUESTS" id="Query_PRG_QUOTESREQUESTS">
                                                                            <div class="HyperLinkQueryContent">All Quotes Requests </div>
                                                                        </div>

                                                                    </div>
                                                                </td>

                                                                <td class="box" style="padding: 0; text-align: center;">
                                                                    <%--<div class="VerticalLine" style="width:1px; overflow:hidden; background:#D1D1D1;"></div>--%>
                                                                </td>

                                                                <td class="box" style="padding: 0;">
                                                                    <div class="ListBoxContainer" style="overflow: auto;">
                                                                        <div class="ListBox" style="width:1100px;">
                                                                            <div style="height:28px; vertical-align:central;">
                                                                                <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:20px;min-width:20px"></div>
                                                                                <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:80px;min-width:80px; color:black">Requested by:</div>
                                                                                <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:170px;min-width:170px;height:22px;">
                                                                                    <select Id="RequestedByOption" class="SelectOption" OnChange="RequestedByChanged()" style="background:white">
                                                                                        <option Id="RequestedByContact"></option>
                                                                                        <option>All</option>
                                                                                    </select>
                                                                                </div>
                                                                                <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:50px;min-width:50px;color:black">Opened</div>
                                                                                <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:60px;min-width:60px;height:22px;">
                                                                                    <input  type="checkbox">
                                                                                </div>
                                                                                <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:40px;min-width:40px;color:black">Status:</div>
                                                                                <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:180px;min-width:180px;height:22px;">
                                                                                    <select class="SelectOption" style="background:white; width:120px;min-width:120px;">
                                                                                        <option>Request Received</option>
                                                                                        <option>Quote Process</option>
                                                                                        <option selected>Pending Approval</option>
                                                                                        <option>Pending Decision</option>
                                                                                        <option>Approved</option>
                                                                                        <option>Rejected</option>
                                                                                    </select>
                                                                                </div>
                                                                                <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:35px;min-width:35px;color:black">From:</div>
                                                                                <div class="ValueTextStyle TemplateItem" style="display:inline-block;width:190px;min-width:190px;height:25px;">
                                                                                    <input type="datetime-local" style="width:160px;min-width:160px;">
                                                                                </div>
                                                                                <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:20px;min-width:20px;color:black">To:</div>
                                                                                <div class="ValueTextStyle TemplateItem" style="display:inline-block;width:180px;min-width:180px;height:25px;">
                                                                                    <input type="datetime-local" style="width:160px;min-width:160px;">
                                                                                </div>
                                                                                <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:20px;min-width:20px"></div>
                                                                            </div>
                                                                        </div>
                                                                        <div id="QuotesRequestsListBox" class="ListBox"></div>
                                                                    </div>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </td>

                                                    <td style="padding: 0; width: 10px; height: 100%; vertical-align: top;">
                                                        <div style="width: 1px; height: 100%; background: #D1D1D1; margin-left: 5px; margin-top: 4px;"></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div style="width: 100%; height: 13px;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px;">
                                                <tr>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/bottom-left.png') no-repeat"></td>
                                                    <td style="background: url('HtmlHelpers/Images/bars_Images/bottom-middle.png') repeat-x"></td>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/bottom-right.png') no-repeat"></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>

                                    <div class="BusyIndicator" id="QuotesRequestsBusyIndicator"></div>
                                </div>



                                <div class="tabPage" style="margin: 0 -1px;">
                                    <div style="margin-top: 5px">

                                        <div style="width: 100%; height: 13px;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px;">
                                                <tr>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/top-left.png') no-repeat"></td>
                                                    <td style="background: url('HtmlHelpers/images/bars_Images/top-middle.png') repeat-x"></td>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/top-right.png') no-repeat"></td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="tabPageContent" style="width: 100%; position: relative; margin: -7px 0 -1px 0;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px; border: 0px">
                                                <tr>
                                                    <td style="padding: 0; width: 1px; height: 100%; vertical-align: top;">
                                                        <div style="width: 1px; height: 100%; background: #D1D1D1; margin-left: 4px; margin-top: 4px;"></div>
                                                    </td>

                                                    <td style="padding: 0; vertical-align: top;">
                                                        <table style="width: 100%; border: 0; border-collapse: collapse; border-spacing: 0;">

                                                            <tr style="height: 25px;">

                                                                <%--     <td style="width:143px; margin-top:-5px; background: url('HtmlHelpers/Images/Bars_Images/QueryArea.png') repeat-y; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0; padding:0;">
                                                               <div style="
                                                                   height:25px;
                                                                   width:143px;
                                                                   font-size:16px;
                                                                   font-family:'Lucida Sans Unicode'; 
                                                                   color: #45494A;
                                                                   text-indent:10px;
                                                                   
                                                                   border-radius: 5px 0 0 0; 
                                                                   -moz-border-radius: 5px 0 0 0; 
                                                                   -webkit-border-radius: 5px 0 0 0;
                                                                   "
                                                                   >
                                                                   Views

                                                               </div>
                                                            </td>--%>

                                                                <td style="width: 10px; padding: 0;"></td>

                                                                <td style="vertical-align: top; padding: 0;">
                                                                    <table style="margin-top: 5px;">
                                                                        <tr>
                                                                            <td>
                                                                                <div style="font-size: 16px; font-family: 'Lucida Sans Unicode'; color: #1B90CB;">
                                                                                    <%--                                                                                <span ><img style="width:16px; height:16px; cursor:pointer; margin-bottom:-3px;" src="HtmlHelpers/Images/refresh.png"/></span>                                                                                --%>
                                                                                    <span>Reports</span>
                                                                                    <span></span>
                                                                                </div>
                                                                            </td>

                                                                            <td style="width: 200px;">
                                                                                <%--<input class="SearchBox" id="SearchBox_QUOTESREQUESTS" type="text" style="width: 200px; margin:0; z-index:0"/>
                                                                            <div class="SearchIcon" id="SearchIcon_QUOTESREQUESTS"></div>
                                                                            <div class="SearchDeleteButton" id="SearchDeleteButton_QUOTESREQUESTS"></div>--%>
                                                                            </td>

                                                                            <td style="width: 5px;"></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>

                                                            </tr>

                                                            <tr>

                                                                <%--         <td class="box" style="background:url('HtmlHelpers/Images/Bars_Images/QueryArea.png') repeat-y; padding:0; border-radius: 0 0 0 5px; -moz-border-radius: 0 0 0 5px; -webkit-border-radius: 0 0 0 5px;">
                                  
                                                            </td>--%>

                                                                <td class="box" style="padding: 0; text-align: center;">
                                                                    <%--<div class="VerticalLine" style="width:1px; overflow:hidden; background:#D1D1D1;"></div>--%>
                                                                </td>

                                                                <td class="box" style="padding: 0;">
                                                                    <div class="ListBoxContainer" style="overflow: auto;">
                                                                        <div id="ReportListBox" class="ListBox"></div>
                                                                    </div>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </td>

                                                    <td style="padding: 0; width: 10px; height: 100%; vertical-align: top;">
                                                        <div style="width: 1px; height: 100%; background: #D1D1D1; margin-left: 5px; margin-top: 4px;"></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div style="width: 100%; height: 13px;">
                                            <table style="width: 100%; height: 100%; border-collapse: collapse; border-spacing: 0px;">
                                                <tr>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/bottom-left.png') no-repeat"></td>
                                                    <td style="background: url('HtmlHelpers/Images/bars_Images/bottom-middle.png') repeat-x"></td>
                                                    <td style="width: 15px; background: url('HtmlHelpers/Images/bars_Images/bottom-right.png') no-repeat"></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>

                                    <%--                                <div class="BusyIndicator" id="QuotesRequestsBusyIndicator"></div>--%>
                                </div>







                                <%--               <div class="tabPage" style="margin:0 -1px;">
                                <div style="margin-top:5px">
                                    <div style="width:100%; height:13px;">
                                        <table style="width:100%; height:100%; border-collapse:collapse; border-spacing:0px;">
                                            <tr>
                                               <td style="width:15px; background:url('HtmlHelpers/Images/bars_Images/top-left.png') no-repeat"></td>
                                               <td style="background:url('HtmlHelpers/images/bars_Images/top-middle.png') repeat-x"></td>
                                               <td style="width:15px; background:url('HtmlHelpers/Images/bars_Images/top-right.png') no-repeat"></td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="tabPageContent" style="width:100%; position:relative; margin:-3px 0 -3px 0;"">
                                        <table style="width:100%; height:100%; border-collapse:collapse; border-spacing:0px; border:0px">
                                            <tr>
                                               <td style="width:15px; height:100%;"><div style="width:1px; height:100%; background:#D1D1D1; margin-left:3px;"></div></td>
                                               <td style="vertical-align:central; text-align:center; font-family: Arial; color: #8F9293; font-style:italic; font-size:22px;">Next Version</td>
                                               <td style="width:15px; height:100%;"><div style="width:1px; height:100%; background:#D1D1D1; margin-left:12px;"></div></td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div style="width:100%; height:13px;">
                                        <table style="width:100%; height:100%; border-collapse:collapse; border-spacing:0px;">
                                            <tr>
                                               <td style="width:15px; background:url('HtmlHelpers/Images/bars_Images/bottom-left.png') no-repeat"></td>
                                               <td style="background:url('HtmlHelpers/Images/bars_Images/bottom-middle.png') repeat-x"></td>
                                               <td style="width:15px; background:url('HtmlHelpers/Images/bars_Images/bottom-right.png') no-repeat"></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>--%>
                            </div>

                        </div>
                    </td>
                </tr>
            </tbody>

        </table>
    </div>


        <script type="text/x-kendo-tmpl" id="ReportListBoxItemDataTemplate">

                           <div>

             <a class="Hyperlink"  id="#= Name #" OnClick="ViewReport(id)" style="padding: 0; margin: 0; cursor: pointer; text-decoration: none;">${Name}   </a>
           
               </div>
                                                  


       
    </script>




    <script type="text/x-kendo-tmpl" id="QuotesRequestsListBoxItemDataTemplate">
        <div class="QuotesRequestsListBoxItem">
            <div class="ListItem" style="width:100%; height:100%;">
                <div style="margin:3px 3px 0px 3px;">

                            <div style="height:5px;">      </div>


                    <div style="height:24px; vertical-align:central;">
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:20px;min-width:20px;"></div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:50px;min-width:50px;">Quote \#:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px;min-width:130px;height:22px;color:\\#1B90CB;">${QuoteNumber}</div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:50px;min-width:50px;">Subject:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:140px;min-width:140px;height:22px;">${Subject}</div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:50px;min-width:50px;">Status:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:175px;min-width:175px;height:22px;">${Status}</div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:120px;min-width:120px;visibility: #= QuotationPreparedTickVisibility #;">Comments:</div>
                       <div class="ValueTextStyle TemplateItem" style="display:inline-block; height: 35px;visibility: #= QuotationPreparedTickVisibility #;">
                            <textarea readonly id="OLDComment#= Id #" style="height: 22px;max-height: 18px;max-width: 400px;" rows = "5" cols = "60">${Comments}</textarea>
                        </div>
                    </div>
                    <div style="height:23px; vertical-align:central;">
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:20px;"><img id="#= DocumentSecurityId #" OnClick="ViewQuotationDocument(id)" src="images/FileIcons/File-pdf-48.png" style="width: 20px; height: 20px; position:relative; cursor: pointer;visibility: #= QuotationPreparedTickVisibility #;" /></div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:80px;min-width:80px;">Requested by:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:100px;min-width:100px;height:22px;">${ContactName}</div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:90px;min-width:90px;">Reference \#/PO:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:100px;min-width:100px;height:22px;">${ReferenceNumber}/${PONumber}</div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:95px;min-width:95px;visibility: #= QuotationPreparedTickVisibility #;">Updated Status:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px;height:22px;visibility: #= QuotationPreparedTickVisibility #;">
                            <select class="SelectOption" id="Option#= Id #" OnChange="SendApprovalQuotesRequstEmailFeedback(id)"  #= OptionDisabledProperty # >
                                <option style="display:none">Updated Status</option>
                                <option>Send Approval</option>
                                <option>Send Rejection</option>
                            </select>
                        </div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:120px;min-width:120px;visibility: #= QuotationPreparedTickVisibility #;">Updated Comments:</div>
                       <div class="ValueTextStyle TemplateItem" style="display:inline-block; height: 35px;visibility: #= QuotationPreparedTickVisibility #;">
                            <textarea id="Comment#= Id #" style="height: 22px;max-height: 18px;max-width: 400px;margin-top: 5px;" oninput="OnQuoteRequestCommentsChanged(id)" rows = "5" cols = "60"></textarea>
                        </div>
                    </div>
                    <div style="height:22px; vertical-align:central;">
                      <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:20px;"></div>

                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:40px;min-width:40px;">Owner:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:140px;min-width:140px;height:22px;">${OwnerName}</div>
                        
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:40px;min-width:40px;">Brand:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:150px;min-width:150px;height:22px;">${Brand}</div>
                    </div>
                </div>
            </div>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="InvoiceListBoxItemDataTemplate">
        <div class="InvoiceListBoxItem">
            <div class="ListItem" style="width:100%; height:100%;" id="#= EntityId #" OnClick="ViewInvoice(id)">
                <div style="margin:3px 3px 0px 3px;">

                    <div style="height:25px; vertical-align:central;">
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:70px;">Invoice No:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:100px; color:\\#1B90CB;">${EntityNumber}</div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:80px;">Total Amount:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:100px;">${Amount}</div>                        
                    </div>

                    <div style="height:25px; vertical-align:central;">
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:70px;">Invoice Date:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:100px;">${InvoiceDate}</div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:80px;">Due Date:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:100px; color: #= AmountDueColor #;">${DueDate}</div>
                    </div>

                    <div style="height:25px; vertical-align:central;">
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:70px;">Our Ref No:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:100px;">${OurRefNumber}</div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:80px;">Open Amount:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:100px;">${AmountDue}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:100px; color: #= StatusColor #;">${StatusName}</div>
                    </div>

                </div>
            </div>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="ShipmentListBoxItemDataTemplate">
        <div class="ShipmentListBoxItem">
            <div class="ListItem" style="width:100%; height:100%; " id="#= ShipmentId #" OnClick="ViewShipment(id)">
                <div style="margin:3px 3px 0px 3px;">

                    <div style="height:25px; vertical-align:central;">
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:25px; text-align:center;"><img style="height:20px; width:20px;" src="#= DirectionSRC #" title="#= DirectionName #" /></div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:60px;">Ref No:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:90px; color:\\#1B90CB;">${ShipmentNumber}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px; visibility: #= MyPartnerVisibility #;">${MyPartnerName}</div>

                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:30px;"><img style="height:20px; width:22px; padding:0px; margin:0px;" src="#= FromCountySRC #"/></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px;" title="#= FromPortName #">${FromPortName}</div>
                        
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:10px;"></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:22px;"><img style="height:20px; width:22px; padding:0px; margin:0px;" src="HtmlHelpers/Images/Arrow.png"/></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:10px;"></div>

                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:30px;"><img style="height:20px; width:22px; padding:0px; margin:0px;" src="#= ToCountySRC #"/></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px;" title="#= ToPortName #">${ToPortName}</div>
                        
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:7px;"></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px; font-size:11px; color: #= StatusColor #;">${StatusName}</div>                       

                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; visibility: #= DeliveryDateVisibility #; width:7px;"></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; visibility: #= DeliveryDateVisibility #; width:150px; font-size:11px; color: #= StatusColor #;">Requested Delivery Date</div>  
                    </div>

                    <div style="height:25px; vertical-align:central;">
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:25px; text-align:center;"><img style="height:20px; width:20px;" src="#= TransportSRC #" title="#= TransportName #"/></div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:60px;">${MyReferenceLabel}:</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:90px; font-size:10px;" title="#= MyReference #">${MyReference}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px;">${IncotermCode}</div>

                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:30px; color:\\#1B90CB;">${FromPortCode}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:80px; color: #= FromDateColor #;">${FromDate}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:50px; font-size:10px; color: #= FromDateTypeColor #;">${FromDateType}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:45px;"></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:30px; color:\\#1B90CB;">${ToPortCode}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:80px; color: #= ToDateColor #;">${ToDate}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:50px; font-size:10px; color: #= ToDateTypeColor #;">${ToDateType}</div>

                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:4px;"></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px; font-size:11px; color: #= StatusColor #; margin-top:-10px;">${StatusDate}</div> 

                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; visibility: #= DeliveryDateVisibility #; width:7px;"></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; visibility: #= DeliveryDateVisibility #; width:150px; font-size:11px; color: #= StatusColor #; margin-top:-10px;">${DeliveryDate}</div> 

                    </div>

                    <div style="height:25px; vertical-align:central; position: relative;">
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:25px; font-size:10px; color:Green;"></div>
                        <div class="LabelTextStyle TemplateItem" style="display:inline-block; width:60px;">${ReferenceLabel}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:90px; font-size:11px;">${Reference}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px;">${ShipmentType}</div>

                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:30px;"></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:80px; margin-top:-10px;">${FromTime}</div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px;"></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:80px; margin-top:-10px;">${ToTime}</div>

                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:60px;"></div>
                        <div class="ValueTextStyle TemplateItem" style="display:inline-block; width:130px; font-size:11px; color: Orange;" title="#= LastLogDateLong #"></div>

                        <div class="ValueTextStyle TemplateItem" style="position: absolute; left: 300px; bottom: 5px; height: 15px; width: 400px; line-height: 15px; font-size:10px;" title="#= DescriptionOfGoods #">${DescriptionOfGoods}</div> 
                    </div>

                </div>
            </div>
        </div>
    </script>


    <script type="text/javascript" src="SharedLogistic/SharedLogisticPageViewModel.js"></script>

    <script type="text/javascript">

        function CheckIfApprovedSelected(Feedback) {
            if (Feedback == "Approved")
                return true;
            return false;
        }

        function CheckIfRejectedSelected(Feedback) {
            if (Feedback == "Rejected")
                return true;
            return false;
        }

        function ViewShipment(ShipmentId) {
            ChangePage("SharedLogistic/ShipmentPage.aspx", ShipmentId);
        }

        function ViewInvoice(InvoiceId) {
            ChangePage("SharedLogistic/InvoicePage.aspx", InvoiceId);
        }

        function ViewReport(name) {
            ChangePage("SharedLogistic/ReportViewPage.aspx", name);
        }

        function RequestedByChanged() {
            var requestedBy = document.getElementById("RequestedByOption").value;
            $.RequestedBy = requestedBy;
            RefreshQuotesRequstsData();
        }

        function OnQuoteRequestCommentsChanged(QuoteRequestId) {
            var selectedQuoteRequest = $.AllQuotesRequests.find(d => d.Id == QuoteRequestId.replace('Comment', ''));
            $('#Option' + selectedQuoteRequest.Id).removeAttr('disabled');
            $('#Option' + selectedQuoteRequest.Id).css("background", "linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%)");
        }


        function ViewQuotationDocument(QuotationDocumentSecurityId) {

            var sharedDownloadURL = "WebPages/DownloadPage.aspx?securityId=" + QuotationDocumentSecurityId + "&tempId=";
            $.ajax({
                url: "api/DocumentDownloadToken",
                type: 'GET',
                contentType: 'application/json',
                headers: {
                    'Token': $.Token
                },
                success: function (documentDownloadToken) {
                    window.open(sharedDownloadURL + documentDownloadToken);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    window.open(sharedDownloadURL);
                }
            });
        }

        function SendApprovalQuotesRequstEmailFeedback(QuoteRequestOptionId) {
            var selectedQuoteRequest = $.AllQuotesRequests.find(d => d.Id == QuoteRequestOptionId.replace('Option', ''));
            var feedback = document.getElementById(QuoteRequestOptionId).value;
            selectedQuoteRequest.Feedback = feedback.indexOf('Approv') > -1 ? "Approved" : "Rejected";
            selectedQuoteRequest.Comments = $('#Comment' + selectedQuoteRequest.Id).val();
            DisabledQuotesRequestProperties(selectedQuoteRequest);
            SendQuotesRequstEmailFeedback(selectedQuoteRequest);
        }

        function DisabledQuotesRequestProperties(selectedQuoteRequest) {
            //$('#OLDComment' + selectedQuoteRequest.Id).val($('#Comment' + selectedQuoteRequest.Id).val());
            //$('#Comment' + selectedQuoteRequest.Id).val('');
            $('#Option' + selectedQuoteRequest.Id).attr('disabled', 'disabled');
        }

        function RefreshQuotesRequstsData() {
            $("#QuotesRequestsBusyIndicator").show();
            $.LoadQuotesRequsts();
        }

        function SendQuotesRequstEmailFeedback(selectedQuoteRequest) {
            $("#QuotesRequestsListBox").html("");
            $("#QuotesRequestsBusyIndicator").show();
            var quotesRequestEmailFeedbackArgs = new QuotesRequestEmailFeedback(selectedQuoteRequest);
            var url = "api/QuotesRequest/UpdateQuotesRequestAndSendEmailFeedback";
            $.ajax({
                url: url,
                data: JSON.stringify(quotesRequestEmailFeedbackArgs),
                type: 'POST',
                contentType: 'application/json',
                headers: { 'Token': $.Token },
                success: function () {
                    $.LoadQuotesRequsts();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $.CheckUserException(jqXHR);
                }
            });
        }

        function QuotesRequestEmailFeedback(quoteRequest) {
            this.PartnerId = $.CurrentCardId;
            this.From = $.CurrentEmail;
            this.QuotesRequest = quoteRequest;
        };
        
        function ChangePage(pageURL, entityId) {

            var loginData = entityId + ":" + $.CurrentCardId + ":" + $.CurrentTenant + ":" + $.CurrentEmail + ":" + $.CurrentCardType + ":" + $.IsBrandingEnabled;

            var link = document.location.href.toLowerCase();;
            var linkArray = link.split('sharedlogisticpage');
            url = linkArray[0];

            if (url.endsWith('/')) {
                url += pageURL;
            }

            else {
                url += "/" + pageURL;
            }
            var params = [];
            params.push({ name: "Token", value: $.Token });
            params.push({ name: "LoginData", value: loginData });
            PostFormParams(url, params);
        }





    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            var myLogoMethodUrl = "api/authentication?myDummyInteger=" + 0 + "&myDummyString=" + "0";
            $.ajax({
                url: myLogoMethodUrl,
                type: 'GET',
                contentType: 'application/json',

                success: function (myLogoCode) {

                    switch (myLogoCode) {
                        case "U.N.I": {
                            $(".Footer_UNI").show();
                            break;
                        }

                        default: {
                            $(".Footer_LOG").show();
                            break;
                        }
                    }
                },
            });
        });
    </script >
</body >
</html >
