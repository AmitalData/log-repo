<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityPage.aspx.cs" Inherits="WebFreight.Web.CustomersHTML.ActivityPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Activity</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <script src="../HtmlHelpers/JS/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js" type="text/javascript"></script>

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
                <div style="display:inline-block; vertical-align:middle; font-family: 'Arial'; font-size:20px; font-weight:bold; font-style:normal; text-decoration:none; color:#333333;" id="NameControl">Topic</div>
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
                        <div class="HyperLinkQuery" id="GNRL"><div style="display:table-cell; vertical-align:middle;">General</div></div>
                    </div>                                      
                </div>

                <div style="display:table-cell; width:8px;"></div>

                <div style="display:table-cell; vertical-align:top; padding:0px;">

                    <!-- General -->
                    <div class="TabPage" id="GNRL_Page" style="width:100%;">
                        <div style="display:table; table-layout:fixed; width:100%; height:35px;">
                            <div style="display:table-cell; vertical-align:middle;" class="TabHeader">General</div>
                        </div>

                        <div class="TabContent" style="display:table; table-layout:fixed; width:100%; padding:0px;">

                        </div>

                        <div class="BusyIndicator" id="GeneralTabBusyIndicator"></div>
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

    <script type="text/javascript" src="ActivityPageViewModel.js"></script>
</html>
