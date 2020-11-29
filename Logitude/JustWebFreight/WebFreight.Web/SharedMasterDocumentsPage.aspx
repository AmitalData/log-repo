<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SharedMasterDocumentsPage.aspx.cs" Inherits="WebFreight.Web.SharedMasterDocumentsPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <title>Documents</title>

    <link href="css/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="css/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-3.5.1.min.js" type="text/javascript"></script>
    <script src="js/jquery.dateFormat-1.0.js" type="text/javascript"></script>
    <script src="js/kendo.all.min.js" type="text/javascript"></script>
    <script src="js/knockout-3.5.1.js" type="text/javascript"></script>
    <script src="js/knockout-kendo.min.js" type="text/javascript"></script>

    <link href="HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="HtmlHelpers/CSS/sunburst.css" rel="stylesheet" />
    <link href="HtmlHelpers/CSS/app.css" rel="stylesheet" />
    <link href="HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css" />
    <script src="HtmlHelpers/JS/Logitude.Converters.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/Logitude.Entites.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/ContactActivityLog.js" type="text/javascript"></script>


    <style type="text/css">
        img[src] {
            visibility: visible;
        }

        img {
            border: 0px;
            outline: none;
            visibility: hidden;
        }

        .DocumentEntityArea {
            margin: 0 5px;
            background: #FFFFFF;
            border: 2px solid #639527;
            padding-top: 3px;
            margin: 0 50px;
        }

        .DocumentListBoxItem {
            width: 100%;
            height: 30px;
            margin: 0px 0 5px 0;
            border: 1px solid #D1D1D1;
            background: #F7F7F7;
            text-indent: 5px;
            display: table;
            border-radius: 5px;
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
        }

        .k-grid td {
            border: 0;
            border-width: 0;
        }

        .k-grid .k-header {
            height: 1px;
            border: 0;
            border-width: 0;
            border-color: white;
            background: white;
            color: black;
            font-size: 13px;
        }

        .k-grid {
            border: 0;
            border-width: 0;
            border-color: white;
        }

        .k-grid-content {
            overflow-y: visible;
        }

        .k-grid-header {
            border: 0;
            border-width: 0;
            border-color: white;
            background-color: white;
        }

        .k-grid-header-wrap {
            border-color: white;
        }
    </style>

</head>

<body>


    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>
    
    <div class="BusyIndicator" style="top:47.5%;width:20%;height:20%" id="DocumentsPageBusyIndicator"></div>  

    <div id="Container" style="position: absolute; top: 0px; z-index: 3; width: 100%; display: normal">
        <table style="height: 100%;">

            <thead style="background-color: #639527">
                <tr style="height: 50px;">
                    <td style="vertical-align: top;">
                        <table style="margin: 5px 0 0 0;">
                            <tr>
                                <td style="width: 10px;"></td>
                                <td id="companyLogoArea" style="width: 50px;">
                                    <img id="companyLogo" src="HtmlHelpers/Images/Icons/Logo.png" style="width: 50px; height: 55px;" />
                                </td>
                                <td style="vertical-align: central; text-indent: 5px;">
                                    <%--<span id="CompanyText" style="font-size:13px; color:#FFFF"></span>--%>
                                </td>
                                <td style="width: 5px;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </thead>

            <tfoot>
                <tr style="height: 30px">
                    <td>
                        <table style="width: 100%; height: 100%">
                            <tr>
                                <td style="width: 20px;">
                                    <div></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tfoot>

            <tbody>
                <tr style="height: 40px;">
                    <td style="vertical-align: central;">
                        <table>
                            <tr>
                                <td>
                                    <div style="margin: 0 60px; font-size: 18px; color: #464951">
                                        Master <span id="LongMasterNumber"></span><span id="ConnectedShipments" style="font-size: 14px;"></span>
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <a id="DownloadAllConnectedDocuments" onclick='OnDownloadAllConnectedDocuments()'>
                                            <div style="cursor: pointer; font-size: 13px; color: rgb(0, 136, 204); text-align: right; padding-right: 50px;">Download All Master & House Documents</div>
                                        </a>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr style="height: 60px;">
                    <td style="vertical-align: top;">
                        <div id="EntityHeaderArea" class="DocumentEntityArea" style="height: 40px;">
                            <table>
                                <tr>
                                    <td style="height: 7px;"></td>
                                </tr>
                                <tr style="font-size: 15px;">
                                    <td style="width: 15px;"></td>
                                    <td style="width: 70px;">Routing : </td>
                                    <td style="width: 20px;">
                                        <img style="width: 20px; display: inline; height: 20px; vertical-align: middle" data-bind="attr: { src: FromCountySRC }" />
                                    </td>
                                    <td style="width: 1px; max-width: 160px;">
                                        <div class="ValueTextStyle" style="max-width: 160px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;" data-bind="text: FromPortName"></div>
                                    </td>

                                    <td style="width: 20px;">
                                        <img class="ShowOnDataControl" src="HtmlHelpers/Images/Arrow.png" style="height: 16px; width: 16px; display: block; margin: auto;" />
                                    </td>

                                    <td style="width: 20px;">
                                        <img style="width: 20px; display: inline; height: 20px; vertical-align: middle" data-bind="attr: { src: ToCountySRC }" />
                                    </td>

                                    <td style="width: 1px; max-width: 160px;">
                                        <div class="ValueTextStyle" style="max-width: 160px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;" data-bind="text: ToPortName"></div>
                                    </td>
                                    <td style="width: 70px;"></td>
                                    <td style="width: 180px;">ATD : <span id="MainCarriageATD"></span></td>
                                    <td style="width: 50px;"></td>
                                    <td>Airline : <span id="MainCarriageCarrierName"></span></td>
                                    <td>
                                        <div></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>

                <tr style="height: 60px;">
                    <td style="vertical-align: top;">
                        <div class="DocumentEntityArea">
                            <table>
                                <tr>
                                    <td id="DocumentsTabPageControl<%=CurrentEntityId%>">
                                        <div id="DocumentsGrid<%=CurrentEntityId%>" style="padding-bottom: 20px;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>

                <%  foreach (var houseShipment in ConnectedHousesShipments) {  %>
                    <tr style="height: 10px;">
                        <td>
                            <div></div>
                        </td>
                    </tr>
                
                    <tr style="height: 40px;">
                        <td style="vertical-align: central;">
                            <div style="margin: 0 60px; font-size: 18px; color: #464951">
                                House <span id="HouseNumberField<%=houseShipment.Id%>"><%=houseShipment.House%></span>
                            </div>
                        </td>
                    </tr>

                    <tr style="height: 60px;">
                        <td style="vertical-align: top;">
                            <div id="EntityHeaderArea<%=houseShipment.Id%>" class="DocumentEntityArea" style="height: 40px;">
                                <table>
                                    <tr>
                                        <td style="height: 7px;"></td>
                                    </tr>
                                    <tr style="font-size: 15px;">
                                        <td style="width: 15px;"></td>
                                        <td>
                                            Shipper : <%=houseShipment.ShipperName != null ? houseShipment.ShipperName : "---"%>
                                        </td>
                                        <td style="width: 70px;"></td>
                                        <td>
                                            Consignee : <%=houseShipment.ConsigneeName != null ? houseShipment.ConsigneeName : "---"%>
                                        </td>
                                        <td style="width: 70px;"></td>
                                        <td>
                                            Qty / weight : <%=houseShipment.BookingNumberOfPackages != null && houseShipment.BookingNumberOfPackages.ToString() != null? houseShipment.BookingNumberOfPackages.ToString() : "0"%> / <%=houseShipment.OrderGrossWeight != null && houseShipment.OrderGrossWeight.ToString() != null ? houseShipment.OrderGrossWeight.ToString() : "0"%>Kg
                                        </td>
                                        <td style="width:35%">
                                            <div></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>

                    <tr style="height: 60px;">
                        <td style="vertical-align: top;">
                            <div class="DocumentEntityArea">
                                <table>
                                    <tr>
                                        <td id="DocumentsTabPageControl<%=houseShipment.Id%>">
                                            <div id="DocumentsGrid<%=houseShipment.Id%>" style="padding-bottom: 20px;"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                <%  }  %>

                <tr>
                    <td>
                        <div></div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div id="InvalidKeyArea" style="position: absolute; top: 0px; z-index: 3; width: 100%; display: none">
        <table style="height: 100%;">

            <thead>
                <tr style="height: 40px;">
                    <td style="vertical-align: top;">

                        <table style="margin: 5px 0 0 0;">
                            <tr>
                                <td style="width: 5px;"></td>

                                <td style="width: 50px;">
                                    <img id="invalidCompanyLogo" src="HtmlHelpers/Images/Icons/Logo.png" style="width: 50px; height: 35px; vertical-align: bottom; position: absolute; top: 2px;" />
                                </td>



                                <td style="text-align: right; vertical-align: top;"></td>

                                <td style="width: 5px;"></td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </thead>

            <tbody>

                <tr style="height: 40px;">
                    <td style="vertical-align: central;">
                        <table style="width: 100%;" id="Table1">
                            <tr>
                                <td style="width: 5px;"></td>

                                <td style="width: 70px; vertical-align: bottom;">
                                    <div id="Div1"></div>
                                </td>

                                <td style="vertical-align: central;"></td>

                                <td style="width: 10px;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr style="height: 60px;">
                    <td style="vertical-align: top;">
                        <div id="Div2" style="margin: 0 10px;">
                            <table style="width: 100%; margin: 0; padding: 0px;">

                                <tr>
                                    <td>
                                        <p style="color: black; font-size: 30px; font-family: Arial; vertical-align: top">Link invalid</p>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <p style="color: black; font-size: 14px; font-family: Arial; vertical-align: bottom; margin-top: 15px">Shipment link is invalid.</p>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>

                <tr style="height: 16px;">
                    <td style="vertical-align: top;">
                        <div style="width: 100%; height: 16px; position: relative; margin-top: -5px;">
                        </div>
                    </td>
                </tr>

                <tr>
                    <td style="vertical-align: top;">
                        <div class="pageContent" style="width: 100%; position: relative; margin: -10px 0 0px 0;">
                        </div>
                    </td>
                </tr>

                <tr style="height: 16px;">
                    <td>
                        <div style="width: 100%; height: 16px; margin-top: 0px;">
                        </div>
                    </td>
                </tr>
            </tbody>

            <tfoot>
                <tr style="height: 30px">
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
        </table>
    </div>

    <script type="text/x-kendo-tmpl" id="RoutingListBoxItemDataTemplate">
        <div class="RoutingListBoxItem">
                      
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="DocumentListBoxItemDataTemplate">
        <a class="DocumentListBoxItem" id="#= Id #" OnClick="OnDownloadDocument(Id)">
             <div class="content">
                 <div>${Name}</div>
             </div>
        </a>
    </script>

    <script type="text/javascript">
        function OnDownloadDocument(url) {
            $.SendContactActivity($.CurrentEmail, "Shipment", "Document Download", $.CurrentTenant, $.CurrentCardId);
            window.open(url);
        }

        function OnDownloadAllDocument() {
            $.SendContactsActivity($.CurrentEmail, "Shipment", "Document Download", $.CurrentTenant, $.CurrentCardId);
            window.open("../WebPages/SharedDownloadPage.aspx?id=" + $.CurrentTenant + ":" + null + ":ship:" + $.CurrentEntityId + ":" + $.CurrentCardType);
        }

        function OnDownloadAllConnectedDocuments() {
            window.open("../WebPages/SharedDownloadPage.aspx?id=" + $.CurrentTenant + ":" + null + ":master:" + $.CurrentEntityId + ":" + $.CurrentCardType);
        }

        function GetURL() {
            return "../WebPages/SharedDownloadPage.aspx?id=" + $.CurrentTenant + ":" + null + ":ship:" + $.CurrentEntityId + ":" + $.CurrentCardType;
        }
    </script>

    <script type="text/javascript" src="SharedDocuments/SharedMasterDocumentsPageViewModel.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var myLogoMethodUrl = "../api/authentication?myDummyInteger=" + 0 + "&myDummyString=" + "0";
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
    </script>

    <style>
        .headerDiv {
            height: 25px;
            background: url("../HtmlHelpers/Images/Bars_Images/ItemHead.png") repeat-x;
            border-radius: 5px 5px 0 0;
            -webkit-border-radius: 5px 5px 0 0;
            -moz-border-radius: 5px 5px 0 0;
            display: table;
            text-indent: 5px;
            width: 100%;
        }

        .content {
            display: table-cell;
            vertical-align: middle;
        }

        .headerDiv .headerTitle {
            font-size: 13px;
            color: #1B90CB;
        }

        .headerDiv .headerValue {
            font-size: 13px;
            color: #282E30;
        }

        .bodyDiv {
            padding: 5px;
            font-size: 10px;
            color: #6E7172;
        }

        .GreenButton {
            border: 1px solid #009161;
            background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(0, 145, 97, 0.6) 100%);
            background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(0, 145, 97, 0.6) 100%);
            background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(0, 145, 97, 0.6) ));
            background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(0, 145, 97, 0.6) 100%);
            background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(0, 145, 97, 0.6) 100%);
        }

        .RedButton {
            background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(237, 192, 147, 1) 100%) !important;
            background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%) !important;
            background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(237, 192, 147, 1) )) !important;
            background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%) !important;
            background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%) !important;
        }

        .Button {
            background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(186, 206, 227, 1) 100%);
            background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
            background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(186, 206, 227, 1) ));
            background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
            background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%);
        }

        .Button, .RedButton, .GreenButton {
            /*display: block;*/
            outline: none;
            text-align: center;
            font-size: 11px;
            color: #45494A;
            /*width: 100%;*/
            height: 22px;
            border: 1px solid #6A8299;
            border-radius: 3px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            text-shadow: 1px 1px white;
            position: relative;
            cursor: pointer;
        }

        .LogitudeWindow {
            position: absolute;
            border-radius: 8px;
            -moz-border-radius: 8px;
            -webkit-border-radius: 8px;
            overflow: hidden;
            width: 100%;
            padding-top: 20%;
            z-index: 10;
        }

        .Input {
            height: 15px;
            min-height: 15px;
            max-height: 15px;
            width: 100%;
            border: 1px solid #AAAAAA;
            outline: none;
            font-size: 11px;
            color: #45494A;
            background: white;
            border-radius: 3px;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            -moz-box-shadow: inset 0 0 3px #AAAAAA;
            -webkit-box-shadow: inset 0 0 3px #AAAAAA;
            box-shadow: inset 0 0 3px #AAAAAA;
        }

            .Input:hover {
                border: 1px solid #3BB3E2;
            }
    </style>

</body>
</html>
