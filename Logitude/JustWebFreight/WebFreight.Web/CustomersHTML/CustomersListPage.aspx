<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomersListPage.aspx.cs" Inherits="WebFreight.Web.CustomersHTML.CustomersListPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <title>Customers</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <script src="../HtmlHelpers/JS/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js" type="text/javascript"></script>
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.default.min.css" rel="stylesheet" type="text/css"/>

    <style type="text/css">

        html, body {
            margin: 0px;
            padding: 0px;
            border: none;
            font-family: "Lucida Sans Unicode";
            font-size: 11px;
            min-width: 900px;
            height: 100%;
            background: white;
        }

        .MainListBoxItem table tr td {
            border:none;
            vertical-align:middle;
        }

        .LabelTextStyle {
            font-size: 11px;
            font-family: "Lucida Sans Unicode";
            color: #999999;
            white-space: nowrap;
        }

        .ValueTextStyle div {
            white-space: nowrap;
            overflow: hidden; 
            text-overflow: ellipsis;
        }

        .ValueTextStyle {
            font-size: 12px;
            font-family: "Lucida Sans Unicode";
            color: #282E30;
            white-space: nowrap;
        }

        .MainListBoxItem:hover {
            background: url("../HtmlHelpers/Images/Bars_Images/BigBlueBar.png");
            border-color: #3BB3E2;
        }

        .MainListBoxItem {
            height: 50px;
            cursor: pointer;
            border: 1px solid #D1D1D1;
            margin: 0 0 5px 0;
            background: #F7F7F7;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
        }

        input:hover, input:focus {
            outline-style: solid;
            outline-width: 0px;
            border: 1px solid #3BB3E2;
        }

        input[type=text]::-ms-clear {
            display: none;
        }

        input {
            margin: 0px;
            height: 20px;
            border: 1px solid #D1D1D1;
            outline-style: solid;
            outline-width: 0px;
            font: 11px "Lucida Sans Unicode";
            color: #45494A;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            -moz-box-shadow: inset 0 0 10px #D1D1D1;
            -webkit-box-shadow: inset 0 0 10px #D1D1D1;
            box-shadow: inset 0 0 10px #D1D1D1;
            background: white;
        }

        .SearchIcon {
            width: 14px;
            height: 14px;
            z-index: 1;
            background: url('images/Search.png') no-repeat;
            background-size: 14px 14px;
            position: absolute;
            margin-top: -18px;
            margin-left: 190px;
        }

        .SearchDeleteButton {
            width: 11px;
            height: 11px;
            z-index: 2;
            background: url('images/x.png') no-repeat;
            background-size: 11px 11px;
            position: absolute;
            margin-top: -18px;
            margin-left: 195px;
            cursor: pointer;
        }

        input.watermark {
            font-family: Arial;
            color: #8F9293;
            font-size: 11px;
            font-style: italic;
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

        .HyperLinkQuery_CUS, .HyperLinkQuery_OPP, .HyperLinkQuery_ACT {
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

        .k-grid .k-state-selected, .k-grid .k-state-selected:hover {
            background: url('images/MainBar.png') repeat-x;
        }

        .k-grid tr:hover {
            background: url('images/BigBlueBar.png') repeat-x;
        }

        /*
        .k-grid-header {
	        background:orange;
	
        }

        .k-grid-header .k-header {
	        background:blue;
	        color:yellow;
        }
        */

        .k-grid tbody tr td {
            cursor: pointer;
            height: 35px;
        }

        #MainGridView thead tr th, .k-grid-header .k-header {
            font-size: 11px;
            font-style: normal;
            font-weight: normal;
            font-family: "Lucida Sans Unicode";
        }

         /* TabControl */
        .k-tabstrip {
            border: none;
            background: transparent;
        }
        
        .k-tabstrip .k-item {
            width: 120px;
            height: 25px;
            font-size: 12px;
            font-family: "Lucida Sans Unicode";
            /*color: #282E30;*/
            color:red;
            cursor: default;
            background: url("../HtmlHelpers/Images/Bars_Images/tab-normal.png");
        }

        .k-tabstrip .k-link {
            text-decoration: none;
            width: 100%;
        }

        .k-tabstrip .k-item div {
            /*margin: 3px 5px;*/
        }

        .k-tabstrip .k-state-active {
            border-color: #D1D1D1;
            background: white;
        }

        .k-tabstrip .k-state-default:hover {
            color: #1B90CB;
            background: url("../HtmlHelpers/Images/Bars_Images/tab-over.png");
            /*background:#7C7C68;*/
            border-color: #D1D1D1;
        }

        .k-tabstrip .k-state-active:hover {
            color: #282E30;
            border-color: #D1D1D1;
            background: white;
        }

</style>

</head>

<body>

    <form style="visibility:collapse;">
        <input id="SavedIsDataCountLoaded" />
        <input id="SavedSelectedTabId" />
        <input id="SavedSelectedQuery_CUS" />
        <input id="SavedSelectedQuery_OPP" />
        <input id="SavedSelectedQuery_ACT" />
        <input id="SavedSearchText_CUS" />
        <input id="SavedSearchText_OPP" />
        <input id="SavedSearchText_ACT" />
    </form>
        
<div style="position:absolute; left:0px; top:0px; z-index:0; height:67px; width:100%; background: url('images/HeaderBar.png') repeat-x;"></div>

<div style="position:absolute; left:0px; top:0px; z-index:1;">

<div style="display:table; table-layout:fixed; width:100%; height:40px;">
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

<div id="Page" style="padding:0px;">
    <div id="TabControl">
        <ul style="padding-top:0px;">
            <li id="TAB_CUS" style="margin-left:3px;">Customers</li>
            <li id="TAB_OPP">Opportunities</li>
            <li id="TAB_ACT">Activities</li>
        </ul>

        <!-- Customers Page -->
        <div class="TabPage" style="padding:0px; border:none;">

            <div style="height:5px;"></div>

            <div style="display:table; height:13px; table-layout:fixed; width:100%;">
                <div style="display:table-cell; width:8px;"></div>
                <div style="display:table-cell; width:144px; background:url('images/top-left.png') no-repeat"></div>
                <div style="display:table-cell; background:url('images/top-middle.png') repeat-x"></div>
                <div style="display:table-cell; width:18px; background:url('images/top-right.png') no-repeat"></div>
                <div style="display:table-cell; width:4px;"></div>
            </div>

            <div class="TabPageContent" style="display:table; table-layout:fixed; width:100%;">
                <div style="display:table-cell; width:8px;"></div>
                <div style="display:table-cell; width:1px; background:#D1D1D1;"></div>

                <!-- Views -->
                <div style="display:table-cell; vertical-align:top; width:143px; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0; padding:0; background: url('images/QueryArea.png') repeat-y;">
                    <div style="font-family:'Lucida Sans Unicode'; font:14px; color: #45494A; text-indent:10px; vertical-align:top;">Views</div>

                    <div style="vertical-align:top; margin-top:5px; border-top:1px solid #CCCCCC; border-bottom:1px solid #CCCCCC; background:url(images/Queries.png) repeat-y;">
                         <div class="HyperLinkQuery_CUS" id="Query_MY_CUS"><div style="display:table-cell; vertical-align:middle;">My Customers</div></div>
                         <div class="HyperLinkQuery_CUS" id="Query_ALL_CUS"><div style="display:table-cell; vertical-align:middle;">All Customers</div></div>
                    </div>
                </div>

                <div style="display:table-cell; width:8px;"></div>

                <div style="display:table-cell; vertical-align:top;">
                    <table style="height:35px; vertical-align:top; border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                        <tr>                    

                            <td style="vertical-align:middle; font-size:16px; font-family:'Lucida Sans Unicode'; color: #27AAE1;">
                                <div style="margin-top:-7px;">
                                    <span><img id="CustomersRefreshButton" title="Refresh" alt="Refresh" style="width:23px; height:23px; cursor:pointer; margin-bottom:-3px;" src="images/Refresh-N.png"/></span>
                                    <span id="CustomersQueryTitle">My Customers</span>
                                    <span id="CustomersQueryCount">(0)</span>
                                </div>
                            </td>

                            <td style="width:5px;"></td>

                            <td style="width:205px; padding:0px;">
                                <input class="SearchBox" id="SearchBox_CUS" type="text" style="width: 205px; margin:0; z-index:0;"/>
                                <div class="SearchIcon" id="SearchIcon_CUS"></div>
                                <div class="SearchDeleteButton" id="SearchDeleteButton_CUS"></div>
                            </td>

                        </tr>
                    </table>

                    <div class="ListBoxContainer" style="overflow:auto;">
                        <div id="CustomersListBox" style="border:0px;"></div>
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

            <div class="BusyIndicator" id="CustomersBusyIndicator"></div>
        </div>
        
        <!-- Opportunities Page -->
        <div class="TabPage" style="padding:0px; border:none;">

            <div style="height:5px;"></div>

            <div style="display:table; height:13px; table-layout:fixed; width:100%;">
                <div style="display:table-cell; width:8px;"></div>
                <div style="display:table-cell; width:144px; background:url('images/top-left.png') no-repeat"></div>
                <div style="display:table-cell; background:url('images/top-middle.png') repeat-x"></div>
                <div style="display:table-cell; width:18px; background:url('images/top-right.png') no-repeat"></div>
                <div style="display:table-cell; width:4px;"></div>
            </div>

            <div class="TabPageContent" style="display:table; table-layout:fixed; width:100%;">
                <div style="display:table-cell; width:8px;"></div>
                <div style="display:table-cell; width:1px; background:#D1D1D1;"></div>
                
                <!-- Views -->
                <div style="display:table-cell; vertical-align:top; width:143px; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0; padding:0; background: url('images/QueryArea.png') repeat-y;">
                    <div style="font-family:'Lucida Sans Unicode'; font:14px; color: #45494A; text-indent:10px; vertical-align:top;">Views</div>

                    <div style="vertical-align:top; margin-top:5px; border-top:1px solid #CCCCCC; border-bottom:1px solid #CCCCCC; background:url(images/Queries.png) repeat-y;">
                         <div class="HyperLinkQuery_OPP" id="Query_MY_OPP"><div style="display:table-cell; vertical-align:middle;">My Opportunities</div></div>
                         <div class="HyperLinkQuery_OPP" id="Query_ALL_OPP"><div style="display:table-cell; vertical-align:middle;">All Opportunities</div></div>
                    </div>
                </div>

                <div style="display:table-cell; width:8px;"></div>

                <div style="display:table-cell; vertical-align:top;">
                    <table style="height:35px; vertical-align:top; border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                        <tr>                    

                            <td style="vertical-align:middle; font-size:16px; font-family:'Lucida Sans Unicode'; color: #27AAE1;">
                                <div style="margin-top:-7px;">
                                    <span><img id="OpportunitiesRefreshButton" title="Refresh" alt="Refresh" style="width:23px; height:23px; cursor:pointer; margin-bottom:-3px;" src="images/Refresh-N.png"/></span>
                                    <span id="OpportunitiesQueryTitle">My Opportunities</span>
                                    <span id="OpportunitiesQueryCount">(0)</span>
                                </div>
                            </td>

                            <td style="width:5px;"></td>

                            <td style="width:205px; padding:0px;">
                                <input class="SearchBox" id="SearchBox_OPP" type="text" style="width: 205px; margin:0; z-index:0;"/>
                                <div class="SearchIcon" id="SearchIcon_OPP"></div>
                                <div class="SearchDeleteButton" id="SearchDeleteButton_OPP"></div>
                            </td>

                        </tr>
                    </table>

                    <div class="ListBoxContainer" style="overflow:auto;">
                        <div id="OpportunitiesListBox" style="border:0px;"></div>
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

            <div class="BusyIndicator" id="OpportunitiesBusyIndicator"></div>
        </div>

        <!-- Activities Page -->
        <div class="TabPage" style="padding:0px; border:none;">

            <div style="height:5px;"></div>

            <div style="display:table; height:13px; table-layout:fixed; width:100%;">
                <div style="display:table-cell; width:8px;"></div>
                <div style="display:table-cell; width:144px; background:url('images/top-left.png') no-repeat"></div>
                <div style="display:table-cell; background:url('images/top-middle.png') repeat-x"></div>
                <div style="display:table-cell; width:18px; background:url('images/top-right.png') no-repeat"></div>
                <div style="display:table-cell; width:4px;"></div>
            </div>

            <div class="TabPageContent" style="display:table; table-layout:fixed; width:100%;">
                <div style="display:table-cell; width:8px;"></div>
                <div style="display:table-cell; width:1px; background:#D1D1D1;"></div>

                <!-- Views -->
                <div style="display:table-cell; vertical-align:top; width:143px; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0; padding:0; background: url('images/QueryArea.png') repeat-y;">
                    <div style="font-family:'Lucida Sans Unicode'; font:14px; color: #45494A; text-indent:10px; vertical-align:top;">Views</div>

                    <div style="vertical-align:top; margin-top:5px; border-top:1px solid #CCCCCC; border-bottom:1px solid #CCCCCC; background:url(images/Queries.png) repeat-y;">
                         <div class="HyperLinkQuery_ACT" id="Query_MY_ACT"><div style="display:table-cell; vertical-align:middle;">My Activities</div></div>
                         <div class="HyperLinkQuery_ACT" id="Query_ALL_ACT"><div style="display:table-cell; vertical-align:middle;">All Activities</div></div>
                    </div>
                </div>

                <div style="display:table-cell; width:8px;"></div>

                <div style="display:table-cell; vertical-align:top;">
                    <table style="height:35px; vertical-align:top; border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%;">
                        <tr>                    

                            <td style="vertical-align:middle; font-size:16px; font-family:'Lucida Sans Unicode'; color: #27AAE1;">
                                <div style="margin-top:-7px;">
                                    <span><img id="ActivitiesRefreshButton" title="Refresh" alt="Refresh" style="width:23px; height:23px; cursor:pointer; margin-bottom:-3px;" src="images/Refresh-N.png"/></span>
                                    <span id="ActivitiesQueryTitle">My Activities</span>
                                    <span id="ActivitiesQueryCount">(0)</span>
                                </div>
                            </td>

                            <td style="width:5px;"></td>

                            <td style="width:205px; padding:0px;">
                                <input class="SearchBox" id="SearchBox_ACT" type="text" style="width: 205px; margin:0; z-index:0;"/>
                                <div class="SearchIcon" id="SearchIcon_ACT"></div>
                                <div class="SearchDeleteButton" id="SearchDeleteButton_ACT"></div>
                            </td>

                        </tr>
                    </table>

                    <div class="ListBoxContainer" style="overflow:auto;">
                        <div id="ActivitiesListBox" style="border:0px;"></div>
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

            <div class="BusyIndicator" id="ActivitiesBusyIndicator"></div>
        </div>

    </div>
</div>

<div id="Page1" style="display:none;">

    <div style="height:5px;"></div>

    <div style="display:table; height:13px; table-layout:fixed; width:100%;">
        <div style="display:table-cell; width:8px;"></div>
        <div style="display:table-cell; width:144px; background:url('images/top-left.png') no-repeat"></div>
        <div style="display:table-cell; background:url('images/top-middle.png') repeat-x"></div>
        <div style="display:table-cell; width:18px; background:url('images/top-right.png') no-repeat"></div>
        <div style="display:table-cell; width:4px;"></div>
    </div>

    <div id="PageContent" style="display:table; table-layout:fixed; width:100%;">
    
        <div style="display:table-cell; width:8px;"></div>
        <div style="display:table-cell; width:1px; background:#D1D1D1;"></div>

        <div style="display:table-cell; vertical-align:top; width:143px; border-radius: 5px 0 0 0; -moz-border-radius: 5px 0 0 0; -webkit-border-radius: 5px 0 0 0; padding:0; background: url('images/QueryArea.png') repeat-y;">                         
            <div style="font-family:'Lucida Sans Unicode'; font:14px; color: #45494A; text-indent:10px; vertical-align:top;">Views</div>

                                      
        </div>

        <div style="display:table-cell; width:8px;"></div>

        <div style="display:table-cell; vertical-align:top;">                            
   
                                  
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
</body>

<script type="text/x-kendo-tmpl" id="CustomerListBoxItemDataTemplate">
    <div class="MainListBoxItem">
        <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">

            <tr style="height:25px;">
                <td style="width:5px;"><div style="width:5px;"></div></td>

                <td style="width:65px;">
                    <div title="#= Rank #" style="height: 20px;">
                        #
                        if (Rank == 'Silver') 
                        { 
                            #
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/Rank.png') no-repeat; background-size:18px 18px; border:none; margin:0px;"></div>
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/NoRank.png') no-repeat; background-size:18px 18px; border:none; margin:0px; opacity:0.4;"></div>
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/NoRank.png') no-repeat; background-size:18px 18px; border:none; margin:0px; opacity:0.4;"></div>
                            # 
                        } 
        
                        else if (Rank == 'Gold') 
                        { 
                            #
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/Rank.png') no-repeat; background-size:18px 18px; border:none; margin:0px;"></div>
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/Rank.png') no-repeat; background-size:18px 18px; border:none; margin:0px;"></div>
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/NoRank.png') no-repeat; background-size:18px 18px; border:none; margin:0px; opacity:0.4;"></div>
                            # 
                        } 

                        else if (Rank == 'Platinum') 
                        { 
                            #
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/Rank.png') no-repeat; background-size:18px 18px; border:none; margin:0px;"></div>
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/Rank.png') no-repeat; background-size:18px 18px; border:none; margin:0px;"></div>
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/Rank.png') no-repeat; background-size:18px 18px; border:none; margin:0px;"></div>
                            # 
                        } 

                        else 
                        { 
                            #
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/NoRank.png') no-repeat; background-size:18px 18px; border:none; margin:0px; opacity:0.4;"></div>
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/NoRank.png') no-repeat; background-size:18px 18px; border:none; margin:0px; opacity:0.4;"></div>
                            <div style="display:inline-block; width: 18px; height:18px; background:url('images/NoRank.png') no-repeat; background-size:18px 18px; border:none; margin:0px; opacity:0.4;"></div>
                            #
                        }
                        #
                    </div>
                </td>

                <td class="ValueTextStyle"><div style="font-size: 13px; color: \\#333333;">${Name}</div></td>

                <td style="width:85px;" class="LabelTextStyle">Start working: </td>
                <td style="width:60px;">
                    <div>
                        # 
                        if($.trim(Date).indexOf('Month') == -1) 
                        {
                            #<div style='color: Orange;'>#= Date #</div>#
                        } 

                        else 
                        {
                            #<div style='color: Red;'>#= Date #</div>#
                        }
                        #
                    </div>
                </td>

                <td style="width:5px;"><div style="width:5px;"></div></td>
            </tr>

            <tr style="height:25px;">
                <td style="width:5px;"><div style="width:5px;"></div></td>

                <td class="ValueTextStyle" colspan="2" style="color: \\#1B90CB;">${City}</td>

                <td style="width:85px;" class="LabelTextStyle">Last Shipment: </td>
                <td style="width:60px;">
                    <div>
                        # 
                        if($.trim(Last).indexOf('Month') == -1) 
                        {
                            #<div style='color: Orange;'>#= Last #</div>#
                        } 

                        else 
                        {
                            #<div style='color: Red;'>#= Last #</div>#
                        }
                        #
                    </div>
                </td>
                <td style="width:5px;"><div style="width:5px;"></div></td>
            </tr>

        </table>
    </div>
</script>

<script type="text/x-kendo-tmpl" id="OpportunityListBoxItemDataTemplate">
    <div class="MainListBoxItem">
        <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">

            <tr style="height:25px;">
                <td style="width:5px;"><div style="width:5px;"></div></td>

                <td style="width:65px;">                    
                </td>

                <td class="ValueTextStyle"><div style="font-size: 13px; color: \\#333333;">${Topic}</div></td>

                <td style="width:85px;" class="LabelTextStyle">Closing Date: </td>
                <td style="width:60px;">
                    <div>
                        # 
                        if($.trim(ClosingDate).indexOf('Month') == -1) 
                        {
                            #<div style='color: Orange;'>#= ClosingDate #</div>#
                        } 

                        else 
                        {
                            #<div style='color: Red;'>#= ClosingDate #</div>#
                        }
                        #
                    </div>
                </td>

                <td style="width:5px;"><div style="width:5px;"></div></td>
            </tr>

            <tr style="height:25px;">
                <td style="width:5px;"><div style="width:5px;"></div></td>

                <td class="ValueTextStyle" colspan="2" style="color: \\#1B90CB;">${OwnerName}</td>
                    
                <td style="width:5px;"><div style="width:5px;"></div></td>
            </tr>

        </table>
    </div>
</script>

<script type="text/x-kendo-tmpl" id="ActivityListBoxItemDataTemplate">
    <div class="MainListBoxItem">
        <table style="border-collapse:collapse; text-space-collapse:collapse; border:none; width:100%; height:100%;">

            <tr style="height:25px;">
                <td style="width:5px;"><div style="width:5px;"></div></td>

                <td style="width:65px;">                    
                </td>

                <td class="ValueTextStyle"><div style="font-size: 13px; color: \\#333333;">${Subject}</div></td>

                <td style="width:85px;" class="LabelTextStyle">Due Date: </td>
                <td style="width:60px;">
                    <div>
                        # 
                        if($.trim(DueDate).indexOf('Month') == -1) 
                        {
                            #<div style='color: Orange;'>#= DueDate #</div>#
                        } 

                        else 
                        {
                            #<div style='color: Red;'>#= DueDate #</div>#
                        }
                        #
                    </div>
                </td>

                <td style="width:5px;"><div style="width:5px;"></div></td>
            </tr>

            <tr style="height:25px;">
                <td style="width:5px;"><div style="width:5px;"></div></td>

                <td class="ValueTextStyle" colspan="2" style="color: \\#1B90CB;">${Owner}</td>
                    
                <td style="width:5px;"><div style="width:5px;"></div></td>
            </tr>

        </table>
    </div>
</script>

<script type="text/javascript" src="CustomersListViewModel.js"></script>

</html>
