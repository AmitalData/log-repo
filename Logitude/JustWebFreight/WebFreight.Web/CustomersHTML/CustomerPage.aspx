<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerPage.aspx.cs" Inherits="WebFreight.Web.CustomersHTML.CustomerPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <script src="../HtmlHelpers/JS/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/jquery.dateFormat-1.0.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/Logitude.Converters.js" type="text/javascript"></script>

    <style type="text/css">
        html, body {
            margin: 0px;
            padding: 0px;
            border: none;
            font-family: "Lucida Sans Unicode";
            font-size: 11px;
            min-width: 950px;
            height: 100%;
            background: white;
        }

        input::-ms-clear { display: none; }

        .k-dropdown {
            height:24px;
            font-size:11px;
            margin:0px;
            padding:0px;
        }

        .BusyIndicator {
            display: none;
            position: absolute;
            top: 50%;
            left: 50%;
            background: url("images/Progress.gif") no-repeat;
            width: 100px;
            height: 100px;
        }

        #BackButton:hover {
            background: url("images/Back-O.png") no-repeat;
        }

        #BackButton {
            cursor: pointer;
            width: 99px;
            height: 31px;
            background: url("images/Back-N.png") no-repeat;
            vertical-align: middle;
            font-size: 10px;
            text-indent: 15px;
        }

        .HeaderLable {
            font-family: "Lucida Sans Unicode";
            font-size:13px;
            font-weight:normal;
            font-style:normal;
            text-decoration:none;
            color:#999999;
            white-space:nowrap;
            vertical-align:middle;
        }

        .HeaderValue {
            font-family: "Lucida Sans Unicode";
            font-size:14px;
            font-weight:normal;
            font-style:normal;
            text-decoration:none;
            color:#333333;
            min-width:100px;
            white-space:nowrap;
            vertical-align:middle;
        }

        .HyperLinkQuery {
            cursor: default;
            font-size: 12px;
            color: #45494A;
            font-family: "Lucida Sans Unicode";
            text-indent: 10px;
            height: 25px;
            width: 100%;
            display: table;
            vertical-align: middle;
        }

        .TabHeader {
            font-family: "Lucida Sans Unicode";
            font-size: 18px;
            font-weight: normal;
            font-style: normal;
            text-decoration: none;
            color: #1B90CB;
        }

        .k-dropdown-wrap .k-input {
            background: white;
        }

        /*.k-state-focused.k-dropdown-wrap {
            background:Red;
        }

        .k-state-hover.k-dropdown-wrap {
            background:Blue;
        }*/

        .chart-wrapper, .chart-wrapper .k-chart {
            margin:0px;
            padding:0px;
        }

        .SectionHeader {
            font-size:12px;
            background: url('images/TabItem-N.png') repeat-x;
            color:#282E30;
            font-family:Arial;
            font-weight:bold;
            white-space:nowrap;
            height:25px;
        }

        .k-tabstrip .k-item {            
            height: 25px;
            font-size: 12px;
            font-family: "Lucida Sans Unicode";
            color: #282E30;
            cursor: default;
            background: url("images/TabItem-N.png");            
        }

        .k-tabstrip .k-state-active {
            border-color: #D1D1D1;
            background: white;
        }

        .k-tabstrip .k-state-default:hover {
            color: #1B90CB;
            background: url("images/TabItem-O.png");
            border-color: #D1D1D1;
        }

        .k-tabstrip .k-state-active:hover {
            color: #282E30;
            border-color: #D1D1D1;
            background: white;
        }

        .k-tabstrip {
            margin:0px;
            border: 0px;
            background: transparent;
        }

        .ListHeader {
            font-size:13px;
            color:black;
            white-space:nowrap;
        }

        .ListBoxItemTemplate table tr td, .AddressListBoxItem table tr td, .ContactListBoxItem table tr td {
            padding:0px;
            border:none;
            vertical-align:middle;
        }

        .AddressListBoxItem, .ContactListBoxItem {
            float: left;
            width: 48%;
            margin: 0 8px 8px 0;
            height:150px;
            background: url('../HtmlHelpers/Images/Bars_Images/PartnerItemBody.png') repeat-x;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            border: 1px solid #D1D1D1;
        }

        .ActionsHeader {
            height:25px; font-family:Arial; font-size:14px; font-weight:bold; color:#333333;
        }

        span.k-icon.k-i-arrow-s {
            background-image: url('../HtmlHelpers/Images/Icons/DropArrow.png');
            background-size: 12px 12px;
            background-position: 0 0;
        }
    </style>
</head>

<body>

<div style="display:table; table-layout:fixed; width:100%; height:40px; background: url('images/HeaderBar.png') repeat-x; border-bottom:1px solid #D1D1D1">
    <div style="display:table-cell; width:10px;"></div>

    <div style="display:table-cell; width:130px; vertical-align:middle; padding-top:5px;">
        <a href="http://www.logitudeworld.com" target="_blank" style="padding:0; margin:0; cursor:pointer; text-decoration:none;">
            <div style="display:table; table-layout:fixed; margin-top:0px;">
                <div style="display:table-cell; width:45px; vertical-align:middle;"><div><img src="images/LogitudeLogo.png" style="width:40px; height:30px; border:none; vertical-align:middle;" /></div></div>
                <div style="display:table-cell; width:85px; vertical-align:middle; font-size:20px; color:gray; font-weight:bold">Logitude</div>
            </div>
        </a>
    </div>
        
    <div style="display:table-cell;"></div>

    <div style="display:table-cell; vertical-align:middle;">
        <div style="float:right;">                               
            <span style="font-size:11px; color:#45494A" id="MemberText"></span>
            <span style="font-size:11px; color:#838889" id="MemberCardText"></span>
        </div>
    </div>

    <div style="display:table-cell; width:5px;"></div>

    <div style="display:table-cell; width:20px; vertical-align:middle;">
        <div id="SignOutButton" title="Log out" style="height:20px; width:20px; cursor:pointer; background: url('images/Signout-N.png') no-repeat;"></div>
    </div>

    <div style="display:table-cell; width:10px;"></div>
</div>

<div id="Page">
    <div>       

        <div style="display:table; vertical-align:middle; height:40px; table-layout:fixed; width:100%;">

            <div style="display:table-cell; width:5px;"></div>

            <div style="display:table-cell; vertical-align:middle; width:100px;">
                <div id="BackButton" style="display:table;">
                    <div style="display:table-cell; vertical-align:middle; font-family:Arial;font-size:11px;font-weight:normal;font-style:normal;text-decoration:none;color:#AE5300;">Customers List</div>                    
                </div>
            </div> 

            <div style="display:table-cell; width:8px;"></div>

            <div style="display:table-cell; vertical-align:middle;">
                <div style="display:table; height:31px; padding-left:8px; padding-right:8px;">                   
                    <div style="display:table-cell; vertical-align:middle;">
                        
                        <div style="display:inline-block; vertical-align:middle; margin-top:5px;">
                            <span class="Rank0" style="display:none;"><img src="images/NoRank.png" style="width: 18px; height:18px; opacity:0.4;"/></span>
                            <span class="Rank0" style="display:none;"><img src="images/NoRank.png" style="width: 18px; height:18px; opacity:0.4;"/></span>
                            <span class="Rank0" style="display:none;"><img src="images/NoRank.png" style="width: 18px; height:18px; opacity:0.4;"/></span>

                            <span class="Rank1" style="display:none;"><img src="images/Rank.png" style="width: 18px; height:18px;"/></span>
                            <span class="Rank1" style="display:none;"><img src="images/NoRank.png" style="width: 18px; height:18px; opacity:0.4;"/></span>
                            <span class="Rank1" style="display:none;"><img src="images/NoRank.png" style="width: 18px; height:18px; opacity:0.4;"/></span>

                            <span class="Rank2" style="display:none;"><img src="images/Rank.png" style="width: 18px; height:18px;"/></span>
                            <span class="Rank2" style="display:none;"><img src="images/Rank.png" style="width: 18px; height:18px;"/></span>
                            <span class="Rank2" style="display:none;"><img src="images/NoRank.png" style="width: 18px; height:18px; opacity:0.4;"/></span>

                            <span class="Rank3" style="display:none;"><img src="images/Rank.png" style="width: 18px; height:18px; opacity:0.4;"/></span>
                            <span class="Rank3" style="display:none;"><img src="images/Rank.png" style="width: 18px; height:18px; opacity:0.4;"/></span>
                            <span class="Rank3" style="display:none;"><img src="images/Rank.png" style="width: 18px; height:18px; opacity:0.4;"/></span>
                        </div>

                        <div style="display:inline-block; vertical-align:middle; font-family: 'Arial'; font-size:20px; font-weight:bold; font-style:normal; text-decoration:none; color:#333333;" id="NameControl"></div>


                    </div>                                        
                </div>
            </div>
            
            <div style="display:table-cell;"></div>

            <div style="display:table-cell; width:5px;"></div>
        </div>

        <div id="EntityHeaderArea" style="display:table; table-layout:fixed; height:60px; width:100%;">
            <div style="display:table-cell; width:8px;"></div>
            <div style="display:table-cell; width:100%; border-top: 1px solid #DADADA; border-left: 1px solid #DADADA; border-right: 1px solid #DADADA; background: #F7F7F7;">
                <table id="HeaderData" style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; display:none;">
                    <tr style="height:30px;">
                        <td style="width:10px;"></td>
                        <td class="HeaderLable" style="width:65px;">Code:</td>
                        <td class="HeaderValue" id="Code"></td>

                        <td class="HeaderLable" style="width:110px;">Started Working:</td>
                        <td class="HeaderValue" id="StartDate"></td>

                        <td class="HeaderLable" style="width:70px;">Address:</td>
                        <td class="HeaderValue">
                            <div>
                                <span><img id="CountryImg" style="height:20px; width:22px; vertical-align:middle"/></span>
                                <span id="CityText"></span>
                            </div>                                                        
                        </td>

                        <td class="HeaderLable" style="width:110px;">ATTN:</td>
                        <td class="HeaderValue" id="ATTN"></td>

                        <td style="width:10px;"></td>
                    </tr>
                    <tr style="height:30px;">
                        <td style="width:10px;"></td>
                        <td class="HeaderLable" style="width:65px;">Industry:</td>
                        <td class="HeaderValue" id="Industry"></td>

                        <td class="HeaderLable" style="width:110px;">Last Shipment:</td>
                        <td class="HeaderValue" id="LastShipment"></td>

                        <td class="HeaderLable" style="width:70px;">Salesman:</td>
                        <td class="HeaderValue" id="Salesman"></td>

                        <td class="HeaderLable" style="width:110px;"></td>
                        <td class="HeaderValue"></td>

                        <td style="width:10px;"></td>
                    </tr>
                </table>
            </div>
            <div style="display:table-cell; width:8px;"></div>
        </div>

        <div id="PageContent" style="margin-top:-5px;">

            <div style="padding-left:8px; padding-right:5px; height:16px;">
                <div style="display:table; height:16px; table-layout:fixed; width:100%;">
                    <div style="display:table-cell; width:144px; background:url('images/10.png') no-repeat;"></div>
                    <div style="display:table-cell; background:url('images/Middle-side.png') repeat-x"></div>
                    <div style="display:table-cell; width:21px; background:url('images/Right-side.png') no-repeat"></div>
                </div>
            </div>

            <div style="display:table; table-layout:fixed; width:100%;">
                <div style="display:table-cell; width:8px;"></div>
                <div style="display:table-cell; width:1px; background:#D1D1D1;"></div>

                <div style="display:table-cell; vertical-align:top; width:143px; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0; padding:0; background: url('images/QueryArea.png') repeat-y;">                                                 
                    <div style="vertical-align:top; margin-top:5px; border-top:1px solid #CCCCCC; border-bottom:1px solid #CCCCCC; background:url(images/Queries.png) repeat-y;">
                        <div class="HyperLinkQuery" id="STST"><div style="display:table-cell; vertical-align:middle;">Statistics</div></div>
                        <div class="HyperLinkQuery" id="OVER"><div style="display:table-cell; vertical-align:middle;">Overview</div></div>
                        <div class="HyperLinkQuery" id="ADDR"><div style="display:table-cell; vertical-align:middle;">Addresses</div></div>
                        <div class="HyperLinkQuery" id="CONT"><div style="display:table-cell; vertical-align:middle;">Contacts</div></div>
                        <div class="HyperLinkQuery" id="DOCS"><div style="display:table-cell; vertical-align:middle;">Docs</div></div>
                    </div>                                      
                </div>

                <div style="display:table-cell; width:8px;"></div>

                <div style="display:table-cell; vertical-align:top; padding:0px;">

                    <!-- Statistics -->
                    <div class="TabPage" id="STST_Page" style="width:100%;">
                        <div style="display:table; table-layout:fixed; width:100%; height:35px;">
                            <div style="display:table-cell; vertical-align:middle;" class="TabHeader">Statistics</div>
                        </div>

                        <div class="TabContent" style="display:table; table-layout:fixed; width:100%; padding:0px;">

                            <div style="display:table-cell; vertical-align:top; padding:0px;">
                                
                                <div style=" height:50%; border:1px solid #D1D1D1;">
                                        <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                                            
                                            <tr style="height:25px;" class="SectionHeader">
                                                <td style="vertical-align:middle; text-indent:5px;">
                                                    <div style="display:table; table-layout:fixed; width:100%;">
                                                        <div style="display:table-cell; vertical-align:middle; padding:0px;">
                                                            Actual vs. Estimated Sales
                                                        </div>
                                                        
                                                        <div style="display:table-cell; vertical-align:middle; width: 140px; padding:0px;">            
                                                            <input id="ActualsChartDeopDown" style="width: 140px;"/>
                                                        </div>

                                                        <div style="display:table-cell; vertical-align:middle; width:10px; padding:0px;"></div>
                                                    </div>                                                    
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>                                                   
                                                    <div class="k-content">
                                                        <div class="chart-wrapper">
                                                            <div id="ActualEstimateChart"></div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>

                                        </table>
                                </div>

                                <div style="height:10px;"></div>

                                <div style="height:50%; display:table; table-layout:fixed; width:100%;">

                                    <div style="display:table-cell; width:50%; border:1px solid #D1D1D1; vertical-align:top; padding:0px;">
                                        <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                                            
                                            <tr style="height:25px;" class="SectionHeader">
                                                <td style="vertical-align:middle; text-indent:5px;">                                                    
                                                    <div style="display:table; table-layout:fixed; width:100%;">

                                                        <div style="display:table-cell; vertical-align:middle; padding:0px;">
                                                            Shipments Summary
                                                        </div>
                                                        
                                                        <div style="display:table-cell; vertical-align:middle; width: 90px; padding:0px;">            
                                                            <input style="width: 90px;" id="ShipmentsTimeDropDown" />
                                                        </div>

                                                        <div style="display:table-cell; vertical-align:middle; width: 140px; padding:0px;">            
                                                            <input style="width: 140px;" id="ShipmentsFieldDropDown" />
                                                        </div>

                                                        <div style="display:table-cell; vertical-align:middle; width:10px; padding:0px;"></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td>                                                   
                                                    <div class="k-content">
                                                        <div class="chart-wrapper">
                                                            <div id="ShipmentsChart"></div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>

                                        </table>
                                    </div>

                                    <div style="display:table-cell; width:10px;"></div>

                                    <div style="display:table-cell; width:50%; border:1px solid #D1D1D1; vertical-align:top; padding:0px;">
                                        <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">

                                            <tr style="height:25px; padding:0px;" class="SectionHeader">
                                                <td style="vertical-align:middle; text-indent:5px;">
                                                    <div style="display:table; table-layout:fixed; width:100%;">
                                                        <div style="display:table-cell; vertical-align:middle; padding:0px;">
                                                            Quotes
                                                        </div>

                                                        <div style="display:table-cell; vertical-align:middle; width:10px; padding:0px;"></div>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <div class="k-content">
                                                        <div class="chart-wrapper">
                                                            <div id="QuotesPieChart"></div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>

                            <div style="display:table-cell; width:10px; padding:0px; display:none;"></div>
                            <div style="display:table-cell; width:1px; background:#D1D1D1; padding:0px; display:none;"></div>
                            <div style="display:table-cell; width:7px; padding:0px; display:none;"></div>

                            <div style="display:table-cell; width:350px; vertical-align:top; padding:0px; display:none;">
                                
                            </div>
                        </div>

                        <div class="BusyIndicator" id="StatisticsTabBusyIndicator"></div>
                    </div>

                    <!-- Overview -->
                    <div class="TabPage" id="OVER_Page" style="width:100%; display:none;">
                        <div style="display:table; table-layout:fixed; width:100%; height:35px;">
                            <div style="display:table-cell; vertical-align:middle;" class="TabHeader">Overview</div>
                        </div>

                        <div class="TabContent" style="display:table; table-layout:fixed; width:100%; padding:0px;">
                            <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                                <tr>

                                    <td style="vertical-align:top; padding:0px;">
                                        <table class="TabContent" style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                                            <tr style="height:50%;">

                                                <!-- Money Information -->
                                                <td style="border:1px solid #D1D1D1; padding:0px; vertical-align:top; width:50%;">
                                                    <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                                                        <tr class="SectionHeader">
                                                            <td style="text-indent:5px;">
                                                                <span>Money Information</span>
                                                                <span class="LocalCurrencyCode" style="font-weight:normal; font-size:12px;"></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="vertical-align:top; padding:0px; border:none;">
                                                                <div class="k-content" style="overflow:auto; border:none;">

                                                                    <div style="display:table; table-layout:fixed; width:100%; height:25px; margin-top:5px;">
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                        <div style="display:table-cell; font-size:11px; width:120px;" class="HeaderLable">Outstanding: </div>
                                                                        <div style="display:table-cell; font-size:12px;" class="HeaderValue" id="Outstanding"></div>
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                    </div>

                                                                    <div style="display:table; table-layout:fixed; width:100%; height:25px;">
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                        <div style="display:table-cell; font-size:11px; width:120px;" class="HeaderLable">Overdue: </div>
                                                                        <div style="display:table-cell; font-size:12px;" class="HeaderValue" id="Overdue"></div>
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                    </div>

                                                                    <div style="display:table; table-layout:fixed; width:100%; height:25px; margin-top:5px;">
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                        <div style="display:table-cell; font-size:11px; width:120px;" class="HeaderLable">Open Receivables: </div>
                                                                        <div style="display:table-cell; font-size:12px;" class="HeaderValue" id="OpenReceivables"></div>
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                    </div>

                                                                    <div style="display:table; table-layout:fixed; width:100%; height:25px; margin-top:5px;">
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                        <div style="display:table-cell; font-size:11px; width:120px;" class="HeaderLable">Open A/R Payments: </div>
                                                                        <div style="display:table-cell; font-size:12px;" class="HeaderValue" id="OpenPayments"></div>
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                    </div>
                                                                    
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                                <td style="width:6px;"></td>

                                                <!-- Responsibility -->
                                                <td style="border:1px solid #D1D1D1; padding:0px; vertical-align:top;">
                                                    <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                                                        <tr class="SectionHeader"><td style="text-indent:5px;">Responsibility</td></tr>
                                                        <tr>
                                                            <td style="vertical-align:top; padding:0px; border:none;">
                                                                <div class="k-content" style="overflow:auto; border:none;">
                                                                    <div style="display:table; table-layout:fixed; width:100%; height:25px; margin-top:5px;">
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                        <div style="display:table-cell; font-size:11px; width:105px;" class="HeaderLable">Account Manager: </div>
                                                                        <div style="display:table-cell; font-size:12px;" class="HeaderValue" id="AccountManager"></div>
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                    </div>

                                                                    <div style="display:table; table-layout:fixed; width:100%; height:25px;">
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                        <div style="display:table-cell; font-size:11px; width:105px;" class="HeaderLable">Classifier: </div>
                                                                        <div style="display:table-cell; font-size:12px;" class="HeaderValue" id="Classifier"></div>
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                    </div>

                                                                    <div style="display:table; table-layout:fixed; width:100%; height:25px; margin-top:5px;">
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                        <div style="display:table-cell; font-size:11px; width:105px;" class="HeaderLable">Collector: </div>
                                                                        <div style="display:table-cell; font-size:12px;" class="HeaderValue" id="Collector"></div>
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                    </div>

                                                                    <div style="display:table; table-layout:fixed; width:100%; height:25px; margin-top:5px;">
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                        <div style="display:table-cell; font-size:11px; width:105px;" class="HeaderLable">Salesman: </div>
                                                                        <div style="display:table-cell; font-size:12px;" class="HeaderValue" id="SalesmanText"></div>
                                                                        <div style="display:table-cell; width:5px;"></div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr style="height:8px;">
                                                <td></td>
                                                <td style="width:8px;"></td>
                                                <td></td>
                                            </tr>

                                            <tr style="height:50%;">

                                                <!-- Services -->
                                                <td style="border:1px solid #D1D1D1; padding:0px; vertical-align:top;">
                                                    <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                                                        <tr class="SectionHeader"><td style="text-indent:5px;">Additional Services</td></tr>
                                                        <tr>
                                                            <td style="vertical-align:top; padding:0px; border:none;">
                                                                <div class="k-content" style="overflow:auto; border:none;">
                                                                    <div id="ServicesListBox" style="border:none;"></div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                                <td style="width:6px;"></td>

                                                <!-- Competitors -->
                                                <td style="border:1px solid #D1D1D1; padding:0px; vertical-align:top;">
                                                    <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                                                        <tr class="SectionHeader"><td style="text-indent:5px;">Competitors</td></tr>
                                                        <tr>
                                                            <td style="vertical-align:top; padding:0px; border:none;">
                                                                <div class="k-content" style="overflow:auto; border:none;">
                                                                    <div id="CompetitorsListBox" style="border:none;"></div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                        </table>

                                    </td>

                                    <td style="width:7px; padding:0px; border:none;"></td>
                                    <td style="width:1px; padding:0px; border:none; background:#D1D1D1;"></td>
                                    <td style="width:7px; padding:0px; border:none;"></td>

                                    <!-- Actions -->
                                    <td style="width:350px; vertical-align:top;">
                                        <div class="SectionHeader" style="display:table; table-layout:fixed; width:100%; border:1px solid #D1D1D1; height:25px;">
                                            <div style="display:table-cell; text-indent:5px; vertical-align:middle;">Action Items</div>
                                        </div>

                                        <div id="ActionItemsScroller" style="overflow:auto; border:none;">
                                            
                                            <div style="height:10px;"></div>
                                            <div class="ActionsHeader">Open Opportunities</div>
                                            <div id="OpportunitiesListBox" style="border:0px;"></div>
                                            <div style="height:25px;"></div>

                                            <div class="ActionsHeader">Open Activities</div>
                                            <div id="ActivitiesListBox" style="border:0px;"></div>
                                            <div style="height:25px;"></div>

                                            <div class="ActionsHeader">Open Quotes</div>
                                            <div id="QuotesListBox" style="border:0px;"></div>
                                        </div> 
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div class="BusyIndicator" id="OverviewTabBusyIndicator"></div>                    

                    </div>

                    <!-- Addresses -->
                    <div class="TabPage" id="ADDR_Page" style="width:100%; display:none;">
                        <div style="display:table; table-layout:fixed; width:100%; height:35px;">
                            <div style="display:table-cell; vertical-align:middle;" class="TabHeader">Addresses</div>
                        </div>

                        <div class="TabContent" style="display:table; table-layout:fixed; width:100%; border:none;">
                            <div class="TabContent" style="overflow:auto; border:none;">
                                <div id="AddressesListBox" style="border:none;"></div>
                            </div>                            
                        </div>

                        <div class="BusyIndicator" id="AddressesTabBusyIndicator"></div>
                    </div>

                    <!-- Contacts -->
                    <div class="TabPage" id="CONT_Page" style="width:100%; display:none;">
                        <div style="display:table; table-layout:fixed; width:100%; height:35px;">
                            <div style="display:table-cell; vertical-align:middle;" class="TabHeader">Contacts</div>
                        </div>

                        <div class="TabContent" style="display:table; table-layout:fixed; width:100%; border:none;">
                            <div class="TabContent" style="overflow:auto; border:none;">
                                <div id="ContactsListBox" style="border:none;"></div>
                            </div>                              
                        </div>

                        <div class="BusyIndicator" id="ContactsTabBusyIndicator"></div>
                    </div>

                    <!-- Docs -->
                    <div class="TabPage" id="DOCS_Page" style="width:100%; display:none;">
                        <div style="display:table; table-layout:fixed; width:100%; height:35px;">
                            <div style="display:table-cell; vertical-align:middle;" class="TabHeader">Docs</div>
                        </div>

                        <div class="TabContent" style="display:table; table-layout:fixed; width:100%;">
                            <div style="color:#999999; font-size:26px; opacity:0.5; font-family:Arial; font-style:italic; margin-top:20px; text-indent:20px;">Coming Soon</div>
                        </div>

                        <div class="BusyIndicator" id="DocsTabBusyIndicator"></div>
                    </div>

                </div>

                <div style="display:table-cell; width:8px;"></div>
                <div style="display:table-cell; width:1px; background:#D1D1D1;"></div>
                <div style="display:table-cell; width:8px;"></div>
            </div>

            <div style="display:table; height:13px; table-layout:fixed; width:100%;">
                <div style="display:table-cell; width:8px;"></div>
                <div style="display:table-cell; width:144px; background:url('images/bottom-left.png') no-repeat"></div>
                <div style="display:table-cell; background:url('images/bottom-middle.png') repeat-x"></div>
                <div style="display:table-cell; width:18px; background:url('images/bottom-right.png') no-repeat"></div>
                <div style="display:table-cell; width:4px;"></div>
            </div>
            
        </div>                               

    </div>
</div>

</body>

    <script type="text/x-kendo-tmpl" id="QuoteListBoxItemDataTemplate">
        <div class="ListBoxItemTemplate" style="height:40px; border:1px solid \\#D1D1D1; margin-bottom:2px;">
             <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">
                <tr style="height:20px;">
                    <td style="width:5px;"></td>
                    <td style="width:30px; text-align:left;"><img style="height:20px; width:22px; padding:0px; margin:0px;" src="#= FromCountySRC #"/></td>
                    <td style="width:120px; text-align:left; vertical-align:top;" title="#= FromPortName #"><div style="width:120px; color:rgb(51, 51, 51); white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${FromPortName}</div></td>
                    <td style="width:30px; text-align:left;"><img style="height:20px; width:22px; padding:0px; margin:0px;" src="#= ToCountySRC #"/></td>
                    <td style="width:120px; text-align:left; vertical-align:top;" title="#= ToPortName #"><div style="width:120px; color:rgb(51, 51, 51); white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${ToPortName}</div></td>
                    <td></td>
                    <td rowspan="2" style="width:25px; text-align:center; vertical-align:middle;"><img style="height:20px; width:20px; vertical-align:middle;" src="#= DirectionSRC #" title="#= DirectionName #" /></td>
                    <td rowspan="2" style="width:20px; text-align:center; vertical-align:middle;"><img style="height:20px; width:20px; vertical-align:middle;" src="#= TransportSRC #" title="#= TransportName #" /></td>
                    <td style="width:5px;"></td>
                </tr>
                <tr style="height:20px;">
                    <td style="width:5px;"></td>
                    <td colspan="5" style="vertical-align:top; font-size:11px; color: rgb(153, 153, 153);">${OpenDate}</td>     
                    <td style="width:5px;"></td>              
                </tr>
             </table>              
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="OpportunityListBoxItemDataTemplate">
        <div class="ListBoxItemTemplate" style="height:40px; border:1px solid \\#D1D1D1; margin-bottom:2px;">
             <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">
                <tr style="height:20px;">
                    <td style="width:5px; background: #= RatingColor #;"></td>
                    <td style="width:5px;"></td>
                    <td style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; color:rgb(51, 51, 51);">${Topic}</td>
                    <td rowspan="2" style="width:150px; font-size:16px; color: rgb(51, 51, 51); text-align:right;">${Shipments}</td>
                    <td style="width:5px;"></td>
                </tr>
                <tr style="height:20px;">
                    <td style="width:5px; background: #= RatingColor #;"></td>
                    <td style="width:5px;"></td>
                    <td style="font-size:11px; color: rgb(153, 153, 153);">${Stage}</td>      
                    <td style="width:5px;"></td>              
                </tr>
             </table>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="ActivityListBoxItemDataTemplate">
        <div class="ListBoxItemTemplate" style="height:40px; border:1px solid \\#D1D1D1; margin-bottom:2px;">
             <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">
                <tr style="height:20px;">
                    <td style="width:5px;"></td>
                    <td style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; color:rgb(51, 51, 51);">${Subject}</td>
                    <td rowspan="2" style="width:32px; vertical-align:middle;"><img style="height:32px; width:32px; padding:0px; margin:0px; border:none; vertical-align:middle;" src="#= TypeSRC #" title="#= TypeName #"/></td>
                    <td style="width:1px;"></td>
                </tr>
                <tr style="height:20px;">
                    <td style="width:5px;"></td>
                    <td style="font-size:11px; color: rgb(153, 153, 153);">
                        <span>${DueDate}</span>
                        <span style="margin-left:5px;">${DueTime}</span>
                    </td>      
                    <td style="width:1px;"></td>              
                </tr>
             </table>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="ServicesListBoxItemDataTemplate">
        <div class="ListBoxItemTemplate" style="height:30px; border:1px solid \\#D1D1D1; margin:2px;">
            <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">
                <tr>
                    <td style="width:5px;"></td>
                    <td>${Name}</td>
                    <td style="width:5px;"></td>
                </tr>
            </table>             
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="CompetitorsListBoxItemDataTemplate">
        <div class="ListBoxItemTemplate" style="height:30px; border:1px solid \\#D1D1D1; margin:2px;">
            <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">
                <tr>
                    <td style="width:5px;"></td>
                    <td>${Name}</td>
                    <td style="width:5px;"></td>
                </tr>
            </table>             
        </div>
    </script>    

    <script type="text/x-kendo-tmpl" id="AddressListBoxItemDataTemplate">
        <div class="AddressListBoxItem">
        <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">
            <tr style="height:25px; background: url('images/TabItem-N.png') repeat-x; text-indent:8px;"><td style="font-family: 'Arial'; font-size:14px; font-weight:bold; color:\\#1B90CB;">${Name}</td></tr>
            <tr>
                <td style="padding:5px; vertical-align:top;">
                    <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:115px;">

                        <tr>                            
                            <td colspan="2" style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Place}</td>                            
                            <td style="width:48px; vertical-align:bottom;" rowspan="5">
                                <img style="height:48px; width:48px; padding:0px; margin-bottom:-6px; vertical-align:bottom;" src="#= CountySRC #"/>
                            </td>
                        </tr>

                        <tr>
                            <td style="font-size:11px; color:\\#6E7172; width:65px;">Address1:</td>
                            <td style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Address1}</td>
                        </tr>

                        <tr>
                            <td style="font-size:11px; color:\\#6E7172; width:65px;">Address2:</td>
                            <td style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Address2}</td>
                        </tr>

                        <tr>
                            <td style="font-size:11px; color:\\#6E7172; width:65px;">Phone:</td>
                            <td style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Phone}</td>
                        </tr>

                        <tr>
                            <td style="font-size:11px; color:\\#6E7172; width:65px;">Fax:</td>
                            <td style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Fax}</td>
                        </tr>
                        
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="ContactListBoxItemDataTemplate">
        <div class="ContactListBoxItem">
        <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">
            <tr style="height:25px; background: url('images/TabItem-N.png') repeat-x; text-indent:8px;"><td style="font-family: 'Arial'; font-size:14px; font-weight:bold; color:\\#1B90CB;">${Name}</td></tr>
            <tr>
                <td style="padding:5px; vertical-align:top;">
                    <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:115px;">

                        <tr>
                            <td style="font-size:11px; color:\\#6E7172; width:65px;">Email:</td>
                            <td style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Email}</td>
                        </tr>

                        <tr>
                            <td style="font-size:11px; color:\\#6E7172; width:65px;">Position:</td>
                            <td style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Position}</td>
                        </tr>

                        <tr>
                            <td style="font-size:11px; color:\\#6E7172; width:65px;">Mobile:</td>
                            <td style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Mobile}</td>
                        </tr>

                        <tr>
                            <td style="font-size:11px; color:\\#6E7172; width:65px;">Phone:</td>
                            <td style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Phone}</td>
                        </tr>

                        <tr>
                            <td style="font-size:11px; color:\\#6E7172; width:65px;">Fax:</td>
                            <td style="font-size:12px; color:\\#282E30; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">${Fax}</td>
                        </tr>
                        
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </script>
        
    <script type="text/javascript" src="CustomerPageViewModel.js"></script>
</html>
