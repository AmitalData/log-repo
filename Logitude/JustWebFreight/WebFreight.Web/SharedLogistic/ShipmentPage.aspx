<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipmentPage.aspx.cs" Inherits="WebFreight.Web.SharedLogistic.ShipmentPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <title>Shipment</title>

    <link href="../css/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="../css/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <script src="../js/jquery-3.5.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.dateFormat-1.0.js" type="text/javascript"></script>
    <script src="../js/kendo.all.min.js" type="text/javascript"></script>
    <script src="../js/knockout-3.5.1.js" type="text/javascript"></script>
    <script src="../js/knockout-kendo.min.js" type="text/javascript"></script>

    <link href="../HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="../HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="../HtmlHelpers/CSS/sunburst.css" rel="stylesheet" />
    <link href="../HtmlHelpers/CSS/app.css" rel="stylesheet" />
    <link href="../HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css"/>
    <script src="../HtmlHelpers/JS/Logitude.Converters.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/Logitude.Entites.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/ContactActivityLog.js" type="text/javascript"></script>


<style type="text/css">
    img[src] {
        visibility: visible;
    }

    img {
        border: 0px;
        outline: none;
        visibility: hidden;
        vertical-align: middle;
    }

    #EntityHeaderArea {
        height: 60px;
        margin: 0 5px;
        background: #F7F7F7;
        border: 1px solid #DADADA;
        padding-top: 3px;
    }

    .RoutingListBoxItem {
        float: left;
        width: 99%;
        height: 100px;
        margin: 0 0 5px 0;
        border: 1px solid #D1D1D1;
        -webkit-border-radius: 5px;
        -moz-border-radius: 5px;
        border-radius: 5px;
        background: url('../HtmlHelpers/Images/Bars_Images/RoutingItemBody.png') repeat-x;
    }

    .PartnerListBoxItem {
        float: left;
        width: 49%;
        margin: 0 2px 5px 2px;
        height: 150px;
        background: url('../HtmlHelpers/Images/Bars_Images/PartnerItemBody.png') repeat-x;
        -webkit-border-radius: 5px;
        -moz-border-radius: 5px;
        border-radius: 5px;
        border: 1px solid #D1D1D1;
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

    .EventListBoxItem {
        width: 100%;
        height: 80px;
        margin: 0px 0 5px 0;
        border: 1px solid #D1D1D1;
        background: #F7F7F7;
        text-indent: 5px;
        /*display: table;*/
        border-radius: 1px;
        -webkit-border-radius: 1px;
        -moz-border-radius: 1px;
    }
</style>

</head>

<body>


    <script src="../HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/app.js" type="text/javascript"></script>

    <div id="ContainerHeader" style="position:absolute; top:0px; z-index:0; width:100%; height:40px; background: url('../HtmlHelpers/Images/Bars_Images/HeaderBar.png') repeat-x; border-bottom:1px solid #D1D1D1"></div>

    <div id="Container" style="position:absolute; top:0px; z-index:3; width:100%;display:normal">
        <table style="height:100%;">

            <thead>
                <tr style="height:40px;">
                    <td style="vertical-align:top;">
                    
                        <table style="margin:5px 0 0 0;">
                            <tr>
                                <td style="width:5px;"></td>

                                <td id="companyLogoArea" style="width:50px;">
                                    <img id="companyLogo" src="../HtmlHelpers/Images/Icons/Logo.png" style="width:50px; height:35px; vertical-align:bottom; position:absolute; top:2px;"/>                    
                                </td>

                                <td style="vertical-align:central; text-indent: 5px;">
                                    <span id="CompanyText" style="font-size:13px; color:#45494A"></span>
                                </td>

                                <td style="text-align:right; vertical-align:top;">
                                    <div style="margin-top:-3px;">                               
                                        <span style="font-size:11px; color:#45494A" id="MemberText"></span>
                                        <span style="font-size:11px; color:#838889" id="MemberCardText"></span>
                                    </div>
                                </td>

                                <td style="width:22px; vertical-align:top;">
                                    <div style="margin-top:-5px;" id="SignOutButton"></div> 
                                </td>

                                <td style="width:5px;"></td>
                            </tr>
                        </table>
                 
                    </td>
                </tr>
            </thead>

            <tfoot>
                <tr style="height:30px">
                    <td>
                        <table style="width:100%; height:100%">
                            <tr>
                                <td style="width:20px;"><div></div></td>

                                <td class="Footer_LOG" style="width:190px; display:none;">
                                    <a class="PoweredArea" href="http://www.logitudeworld.com" target="_blank" style="padding:0; margin:0; cursor:pointer; text-decoration:none;">
                                        <table style="height:100%">
                                            <tr>
                                                <td style="width:65px; vertical-align:central; white-space:nowrap;"><p style="font-size:11px; color:#27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</p></td>                            
                                                <td style="width:40px; vertical-align:central;"><img src="../HtmlHelpers/Images/Icons/LogitudeLogo.png" style="width:40px; height:20px;" /></td>
                                                <td style="width:85px; vertical-align:central;"><img src="../HtmlHelpers/Images/Icons/Logitude.png" style="width:79px; height:25px;" /></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </a>
                                </td>

                                <td class="Footer_UNI" style="width:190px; display:none;">
                                    <a class="PoweredArea" href="http://www.amital.co.il" target="_blank" style="padding:0; margin:0; cursor:pointer; text-decoration:none;">
                                        <table style="height:100%">
                                            <tr>
                                                <td style="width:65px; vertical-align:top; padding-top: 7px; white-space:nowrap; font-size:11px; color:#27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</td>                            
                                                <td style="width:28px; vertical-align:middle;"><img src="../images/ApplicationLogo/UnifreightSmallLogo.png" style="width:28px; height:28px;" /></td>
                                                <td style="width:85px; vertical-align:middle; white-space:nowrap; font-size:14px; color:#7F7F7F; font-weight: bold; font-family: 'Lucida Sans Unicode';">Unifreight Cloud Services</td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </a>
                                </td>

                                <td><div></div></td>
                                <td style="width:20px;"><div></div></td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </tfoot>

            <tbody>

                <tr style="height:40px;">                   
                    <td style="vertical-align:central;">
                        <table style="width:100%;" id="BackArea">
                            <tr>
                                <td style="width:5px;"></td>

                                <td style="width:70px; vertical-align:bottom;">
                                    <div id="BackButton"></div>                                    
                                </td>

                                <td style="vertical-align:central;">
                                    <div class="ShowOnDataControl" style="display:none; background:#F2F2F2; float:left; padding:2px 10px 2px 2px">
                                        <img style="width:20px; display:inline; height:20px; vertical-align:bottom" data-bind="attr: { src: DirectionSRC }" />
                                        <img style="width:20px; display:inline; height:20px; vertical-align:bottom" data-bind="attr: { src: TransportSRC }" />
                                        <span style="font-size:15px; display:inline; color:#1B90CB;" data-bind="text: ShipmentNumber"> </span>
                                    </div>
                                </td>

                                <td style="width:10px;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr style="height:60px;">
                    <td style="vertical-align:top;">
                                        <div id="EntityHeaderArea" style="margin:0 10px;">
                                            <table style="width:100%; margin:0; padding:0px;">
                                                <tr>
                                                    <td style="width:270px; vertical-align:top;">
                                                        <%--<p class="BlueLabel">General</p>--%>
                                                        <div style="margin:0 0 0 5px;">
                                                        <div>
                                                        <span class="LabelTextStyle" style="display:inline-block; width:75px;" data-bind="text: MyReferenceLabel"></span>
                                                        <span class="ValueTextStyle" data-bind="text: MyReference"></span>
                                                        </div>
                                                        <div>
                                                        <span class="LabelTextStyle" style="display:inline-block; width:75px;">House:</span>
                                                        <span class="ValueTextStyle" data-bind="text: House"></span>
                                                        </div>

                                                        <div class="ShowTenant1495Data">
                                                        <span class="LabelTextStyle" style="display:inline-block; width:75px;">Delivery Date:</span>
                                                        <span class="ValueTextStyle" data-bind="text: DeliveryDate"></span>
                                                        </div>
                                                            
                                                        </div>
                                                    </td>

                                                    <td style="width:1px; vertical-align:top;"><div style="height:50px; width:1px; background:#DADADA; margin-top:4px;"></div></td>

                                                    <td style="width:450px; vertical-align:top;">
                                                        
                                                        <div style="margin:0 0 0 5px;">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 50px;">Routing:</td>
                                                                    <td style="width: 20px;">
                                                                        <img style="width:20px; display:inline; height:20px; vertical-align:middle" data-bind="attr: { src: FromCountySRC }" />
                                                                    </td>
                                                                    <td style="width: 1px; max-width: 160px;">
                                                                        <div class="ValueTextStyle" style="max-width: 160px; white-space:nowrap; overflow: hidden; text-overflow: ellipsis;" data-bind="text: FromPortName"></div> 
                                                                    </td>

                                                                    <td style="width: 20px;">
                                                                        <img class="ShowOnDataControl" src="../HtmlHelpers/Images/Arrow.png" style="height: 16px; width: 16px; display:block; margin:auto;"/>
                                                                    </td>

                                                                    <td style="width: 20px;">
                                                                        <img style="width:20px; display:inline; height:20px; vertical-align:middle" data-bind="attr: { src: ToCountySRC }" />
                                                                    </td>

                                                                    <td style="width: 1px; max-width: 160px;">
                                                                        <div class="ValueTextStyle" style="max-width: 160px; white-space:nowrap; overflow: hidden; text-overflow: ellipsis;" data-bind="text: ToPortName"></div> 
                                                                    </td>
                                                                    <td><div></div></td>
                                                                </tr>
                                                            </table>

                                                            <div>
                                                                <span class="LabelTextStyle" style="display:inline-block; width:50px;">Status:</span>
                                                                <span class="ValueTextStyle" data-bind="style: { color: StatusColor }, text: Status"></span>
                                                            </div>

                                                        </div>
                                                    </td>

                                                    <td style="width:1px; vertical-align:top;"><div style="height:50px; width:1px; background:#DADADA; margin-top:4px;"></div></td>

                                                    <td style="vertical-align:top;">
                                                        
                                                        <div style="margin:0 0 0 5px;" data-bind="style: { visibility: PartnerVisibility }">
                                                            <div>
                                                                <span class="LabelTextStyle" data-bind="text: PartnerTitle" style="display:inline-block; width:65px;"> </span>                                                                
                                                                <span class="ValueTextStyle" data-bind="text: PartnerName"> </span>
                                                            </div>

                                                            <div>
                                                                <span class="LabelTextStyle" style="display:inline-block; width:65px;"></span> 
                                                                <span class="ValueTextStyle" data-bind="text: PartnerAddress"></span>
                                                                <img style="width:20px; display:inline; height:20px; vertical-align:middle; margin-left: 5px;" data-bind="attr: { src: PartnerCountrySRC }" class="ShowPartnerData" />
                                                            </div>

                                                            <div>
                                                                <span class="LabelTextStyle" style="display:inline-block; width:65px;"></span> 
                                                                <span class="ValueTextStyle" data-bind="text: PartnerContact"> </span>
                                                            </div>
                                                        </div>
                                                    </td>                                
                                                </tr>
                                            </table>
                                        </div>
                    </td>
                </tr>

                <tr style="height:16px;">
                    <td style="vertical-align:top;">
                            <div style="width:100%; height:16px; position:relative; margin-top:-5px;">
                                <table style="width:100%; height:100%; border-collapse:collapse; border-spacing:0px;">
                                    <tr>                   
                                       <td style="width:3px;"></td>                    
                                       <td style="width:21px; vertical-align:top; text-align:left;"><div style="margin:-1px -1px 0 1px; height:16px; width:21px; background:url('../HtmlHelpers/Images/bars_Images/Left-side.png') no-repeat";></div></td>
                                       <td style="background:url('../HtmlHelpers/Images/bars_Images/Middle-side.png') repeat-x"></td>
                                       <td style="width:21px; background:url('../HtmlHelpers/Images/bars_Images/Right-side.png') no-repeat"></td>
                                       <td style="width:3px;"></td>
                                    </tr>
                                </table>
                            </div>
                    </td>
                </tr>

                <tr>
                    <td style="vertical-align:top;">
                        <div class="pageContent" style="width:100%; position:relative; margin:-10px 0 0px 0;">
                             <table style="width:100%; height:100%; border-collapse:collapse; border-spacing:0px;">
                                <tr>
                                    <td style="width:10px; height:100%;"><div style="width:1px; height:100%; background:#D1D1D1; margin-left:9px; margin-bottom:-0px;"></div></td>

                                    <td style="vertical-align:top;">
                                        <div>

                                            <div id="mainTabsDiv">
                                                <ul>
                                                    <li id="TAB_ROU" class="k-state-active">
                                                        <div>
                                                            <span><img class="TabImage" src="../HtmlHelpers/Images/Tabs_Images/Routing.png" style="opacity:0.8"/></span>
                                                            <span>Routing</span>
                                                        </div>
                                                    </li>

                                                    <li id="TAB_PAR">
                                                        <div>
                                                            <span><img class="TabImage" src="../HtmlHelpers/Images/Tabs_Images/Partners.png" style="opacity:0.8"/></span>
                                                            <span>Partners</span>
                                                        </div>
                                                    </li>

                                                    <li id="TAB_PAC">
                                                        <div>
                                                            <span><img class="TabImage" src="../HtmlHelpers/Images/Tabs_Images/Cargo.png" style="opacity:0.8"/></span>
                                                            <span>Cargo Info</span>
                                                        </div>
                                                    </li>

                                                    <li id="TAB_MON">
                                                        <div>
                                                            <span><img class="TabImage" src="../HtmlHelpers/Images/Tabs_Images/Money.png" style="opacity:0.8"/></span>
                                                            <span>Money</span>
                                                        </div>
                                                    </li>

                                                    <li id="TAB_DOC">
                                                        <div>
                                                            <span><img class="TabImage" src="../HtmlHelpers/Images/Tabs_Images/Document.png" style="opacity:0.8"/></span>
                                                            <span>Documents</span>
                                                        </div>
                                                    </li>

                                                    <li id="TAB_EVE">
                                                        <div>
                                                            <span><img class="TabImage" src="../HtmlHelpers/Images/Tabs_Images/Events.png" style="opacity:0.8"/></span>
                                                            <span>Events</span>
                                                        </div>
                                                    </li>

                                                </ul>

                                                <%-- Routing --%>
                                                <div class="tabPage">
                                                    <div class="ScrollViewer" style="overflow:auto;">
                                                        <div id="RoutingsListBox" class="ListBox"></div>
                                                    </div> 
                                                    
                                                    <div class="BusyIndicator" id="RoutingsPageBusyIndicator"></div>                                                                               
                                                </div> 

                                                <%-- Partners --%>
                                                <div class="tabPage">
                                                    <div class="ScrollViewer" style="overflow:auto;">
                                                        <div id="PartnersListBox" class="ListBox"></div>
                                                    </div> 

                                                    <div class="BusyIndicator" id="PartnersPageBusyIndicator"></div>
                                                </div> 

                                                <%-- Packages --%>
                                                <div class="tabPage" style="padding-left:0px; padding-right:0px;">

                                                    <table style="width:100%; height:100%;" id="PackagesTabPageControl">

                                                        <tr>
                                                            <td style="vertical-align:top;">    
                                                                <div id="EstimatePackagesControl" class="InfoDiv">Estimate Order Data</div>                                                                                                                          
                                                                <div id="PackagesContainer" style="overflow:auto;">
                                                                    <div id="PackagesGrid" style="margin-top:5px;"></div>                                                                    
                                                                </div>
                                                            </td>
                                                        </tr>

                                                        <tr style="height:10px;"><td style="height:10px; background:url('../HtmlHelpers/Images/Bars_Images/top-middle.png') repeat-x"></td></tr>

                                                        <tr style="height:70px;">
                                                            <td style="height:70px;">
                                                                <table style="width:100%; height:100%;">

                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width: 250px;" />
                                                                    </colgroup>

                                                                    <tr>

                                                                        <td style="vertical-align:top; text-align:left;">
                                                                            <div class="BlueTextStyle" style="height:20px; text-indent:10px;">Summary</div>

                                                                            <div class="ShowOnDataControl" style="margin:0; padding:0; height:50px; text-indent:10px;">

                                                                                <div style="display:inline-block; text-align:center;">
                                                                                    <div style="font-size:20px; color: #282E30; font-family: 'Lucida Sans Unicode'; height:30px;" data-bind="text: TotalPieces"></div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px;">Packages</div>
                                                                                </div>

                                                                                <div style="display:inline-block; text-align:center;">
                                                                                    <div style="font-size:20px; color: #282E30; font-family: 'Lucida Sans Unicode'; height:30px;" data-bind="text: TotalGrossWeight"></div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px;" data-bind="text: GrossLabel"></div>
                                                                                </div>

                                                                                <div style="display:inline-block; text-align:center;">
                                                                                    <div style="font-size:20px; color: #282E30; font-family: 'Lucida Sans Unicode'; height:30px;" data-bind="text: TotalVolume"></div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px;" data-bind="text: VolumeLabel"></div>
                                                                                </div>

<%--                                                                                <div style="display:inline-block; text-align:center;">
                                                                                    <div style="font-size:20px; color: #282E30; font-family: 'Lucida Sans Unicode'; height:30px;" data-bind="text: TotalVolumetricWeight"></div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px;" data-bind="text: VolumetricLabel"></div>
                                                                                </div>--%>

                                                                                <div style="display:inline-block; text-align:center;">
                                                                                    <div style="font-size:20px; color: #282E30; font-family: 'Lucida Sans Unicode'; height:30px;" data-bind="text: TotalChargeableWeight"></div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px;" data-bind="text: ChargeableLabel"></div>
                                                                                </div>                                                                                                                                     
                                                                            </div>
                                                                        </td>

                                                                        <td style="width: 250px;">
                                                                            <div class="LabelTextStyle" style="height:15px; font-size:11px; text-indent:2px;">Description of goods</div>
                                                                            <div style="height:50px; background: #FFFADC; border: 1px solid #D1D1D1; overflow:auto; white-space:normal; padding:1px 3px;" data-bind="text: DescriptionOfGoods"></div>
                                                                        </td>

                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>

                                                        
                                                    </table>

                                                    <div class="BusyIndicator" id="PackagesPageBusyIndicator"></div>
                                                </div> 
                                                         
                                                <%-- Money --%>                                       
                                                <div class="tabPage">
                                                    <table id="MoneyTabPageControl" style="border:0; border-collapse:collapse; border-spacing:0;">
                                                        <tr>

                                                            <td style="width:50%; vertical-align:top; margin:0; padding:0;">
                                                                <div class="BlueTextStyle" id="ChargesTitle" style="margin-top:10px; margin-bottom:2px;">Charges</div>
                                                                <div class="ScrollViewer" style="overflow:auto;">
                                                                    <div id="ChargesGrid"></div>
                                                                </div>
                                                            </td>

                                                            <td style="width:15px;">
                                                                <div style="width:15px;"></div>
                                                            </td>

                                                            <td style="width:50%; vertical-align:top; margin:0; padding:0;">
                                                                <div class="BlueTextStyle" id="InvoicesTitle" style="margin-top:10px; margin-bottom:2px;">Invoices</div>
                                                                <div class="ScrollViewer" style="overflow:auto;">
                                                                    <div id="InvoicesGrid"></div>
                                                                </div>
                                                            </td>

                                                        </tr>
                                                    </table>

                                                    <div class="BusyIndicator" id="MoneyPageBusyIndicator"></div>
                                                </div>

                                                <div class="tabPage">
                   <%--                                 <div class="ScrollViewer" style="overflow:auto;">
                                                        <div id="DocumentsListBox" class="ListBox"></div>
                                                    </div>  --%>
                                                    
                                                    <table style="width:100%; height:100%;" id="DocumentsTabPageControl">
                                                        <tr>
                                                            <td style="vertical-align: top;">
                                                                <div id="DocumentsContainer" style="overflow:auto;">
                                                                    <table>
                                                                        <tr>
                                                                            <td>

                                                                            </td>
                                                                            <td style="width:100px;text-align:right">
                                                                                <a  id="DownloadAll" onclick='OnDownloadAllDocument()'>
                                                                                <div style="cursor:pointer; font-size:13px; color:#27AAE1; text-align:right;padding-right:5px"> Download All</div>
                                                                                    </a>
                                                                               </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                    <div id="DocumentsGrid" style="margin-top:5px;"></div>                                                                    
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <div class="BusyIndicator" id="DocumentsPageBusyIndicator"></div>                      
                                                </div>                                                

                                                <div class="tabPage">
                                                    <div class="ScrollViewer" style="overflow:auto;">
                                                        <div id="EventsListBox" class="ListBox"></div>
                                                    </div>  
                                                    
                                                    <div class="BusyIndicator" id="EventsPageBusyIndicator"></div>                          
                                                </div> 

                                            </div>                                               

                                        </div>
                                    </td>

                                    <td style="width:10px; height:100%;"><div style="width:1px; height:100%; background:#D1D1D1; margin-left:0px; margin-bottom:-0px;"></div></td>                                
                               </tr>
                            </table>
                        </div>
                    </td>
                </tr>

                <tr style="height:16px;">
                    <td>
                            <div style="width:100%; height:16px; margin-top:0px;">
                                <table style="width:100%; height:100%; border-collapse:collapse; border-spacing:0px;">
                                    <tr>                                                                             
                                       <td style="width:21px;"><div style="width:21px; height:16px; margin:-1px -5px 0 5px; background:url('../HtmlHelpers/Images/bars_Images/bottom-left.png') no-repeat"></div></td>
                                       <td style="background:url('../HtmlHelpers/Images/bars_Images/bottom-middle.png') repeat-x"></td>
                                       <td style="width:21px; background:url('../HtmlHelpers/Images/bars_Images/bottom-right.png') no-repeat"></td>     
                                    </tr>
                                </table>
                            </div>
                    </td>
                </tr>

            </tbody>

        </table>
    </div>
    
    <div id="Container2" class="LogitudeWindow">

        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                            </td>   
                            <td style="background: #F2F2F2;width:24%;border-radius:10px;">
                                <div style="border:1px solid #C8C8C8;border-radius:5px;">
                                    <div style="border:1px solid #C8C8C8;border-radius:5px;margin:8px;">
                                    
                                <table>
                        <tr>

                            <td style="vertical-align: central;width:100%">
                                <div>
                                    <span style="font-family:auto;font-size: 15px; display: inline; color: black;">Please approve to download/view documents. </span>
                                </div>
                            </td>

                        </tr>

                        <tr>

                            <td style="vertical-align: central;">
                                <div>
                                    <span style="font-family:auto;font-size: 13px; display: inline; color: black">(Shipment received confirmation will be sent) </span>
                                </div>
                            </td>

                        </tr>
                                     <tr>

                            <td style="vertical-align: central;">
                                <div id="ErrorMessage" style="height:10px">
                                    <span style="font-family:auto;font-size: 13px; display: inline; color: red">*</span>
                                </div>
                            </td>

                        </tr>

                        <tr>

                            <td style="vertical-align: central;width:40%">
                                <div style="width:97%">
                                    <input id="ApprovalName" class="Input" type="text" placeholder="Please enter your name" />
                                </div>
                            </td>

                        </tr>

                        <tr>

                            <td style="vertical-align: central; width:40%">
                                <div style="margin-top: 9px; float: right;margin-right:-2%">
                        <button class="RedButton" onclick="CloseButtonClicked()" style="float: right; margin-left: 7px; margin-right: 7px;" title="Close">Close</button>
                    </div>
                    <div style="margin-left: 3px; margin-top: 9px; float: right">
                        <button class="GreenButton" onclick="ApproveButtonClicked()" style="float: right; margin-left: 7px;" title="Approve">Approve</button>
                    </div>
                            </td>

                        </tr>
                            </table>
                                </div>
                                </div>

                            </td>  
                            <td style="width:40%">
                            </td>                           
                        </tr>
                    </table>
                </td>
            </tr>
        </table>


</div>
    <div id="InvalidKeyArea" style="position:absolute; top:0px; z-index:3; width:100%;display:none">
        <table style="height:100%;">

            <thead>
                <tr style="height:40px;">
                    <td style="vertical-align:top;">
                    
                        <table style="margin:5px 0 0 0;">
                            <tr>
                                <td style="width:5px;"></td>

                                <td style="width:50px;">
                                    <img id="Img1" src="../HtmlHelpers/Images/Icons/Logo.png" style="width:50px; height:35px; vertical-align:bottom; position:absolute; top:2px;"/>                    
                                </td>

                                <td style="vertical-align:central; text-indent: 5px;">
                                    <span id="Span1" style="font-size:13px; color:#45494A">Company</span>
                                </td>

                                <td style="text-align:right; vertical-align:top;">
                                   
                                </td>

                                

                                <td style="width:5px;"></td>
                            </tr>
                        </table>
                 
                    </td>
                </tr>
            </thead>

                     <tbody>

                <tr style="height:40px;">                   
                    <td style="vertical-align:central;">
                        <table style="width:100%;" id="Table1">
                            <tr>
                                <td style="width:5px;"></td>

                                <td style="width:70px; vertical-align:bottom;">
                                    <div id="Div1"></div>                                    
                                </td>

                                <td style="vertical-align:central;">
                                     
                                </td>

                                <td style="width:10px;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr style="height:60px;">
                    <td style="vertical-align:top;">
                                        <div id="Div2" style="margin:0 10px;">
                                            <table style="width:100%; margin:0; padding:0px;">
                                                
                                               <tr>
                                                   <td>
                                                        <p style="color:black;font-size:30px;font-family:Arial;vertical-align:top">Link invalid</p>
                                                   </td>
                                               </tr>

                                                  <tr>
                                                   <td>
                                                        <p style="color:black;font-size:14px;font-family:Arial;vertical-align:bottom;margin-top:15px">Shipment link is invalid.</p>
                                                   </td>
                                               </tr>
                                            </table>
                                        </div>
                    </td>
                </tr>

                <tr style="height:16px;">
                    <td style="vertical-align:top;">
                            <div style="width:100%; height:16px; position:relative; margin-top:-5px;">
                               <%-- <table style="width:100%; height:100%; border-collapse:collapse; border-spacing:0px;">
                                    <tr>                   
                                       <td style="width:3px;"></td>                    
                                       <td style="width:21px; vertical-align:top; text-align:left;"><div style="margin:-1px -1px 0 1px; height:16px; width:21px; background:url('../HtmlHelpers/Images/bars_Images/Left-side.png') no-repeat";></div></td>
                                       <td style="background:url('../HtmlHelpers/Images/bars_Images/Middle-side.png') repeat-x"></td>
                                       <td style="width:21px; background:url('../HtmlHelpers/Images/bars_Images/Right-side.png') no-repeat"></td>
                                       <td style="width:3px;"></td>
                                    </tr>
                                </table>--%>
                            </div>
                    </td>
                </tr>

                <tr>
                    <td style="vertical-align:top;">
                        <div class="pageContent" style="width:100%; position:relative; margin:-10px 0 0px 0;">
                          <%--  <img width="60" height="60" src="../HtmlHelpers/Images/SimplogIcons/warning.png" /> --%>
                           
                        </div>
                    </td>
                </tr>

                <tr style="height:16px;">
                    <td>
                            <div style="width:100%; height:16px; margin-top:0px;">
                              <%--  <table style="width:100%; height:100%; border-collapse:collapse; border-spacing:0px;">
                                    <tr>                                                                             
                                       <td style="width:21px;"><div style="width:21px; height:16px; margin:-1px -5px 0 5px; background:url('../HtmlHelpers/Images/bars_Images/bottom-left.png') no-repeat"></div></td>
                                       <td style="background:url('../HtmlHelpers/Images/bars_Images/bottom-middle.png') repeat-x"></td>
                                       <td style="width:21px; background:url('../HtmlHelpers/Images/bars_Images/bottom-right.png') no-repeat"></td>     
                                    </tr>
                                </table>--%>
                            </div>
                    </td>
                </tr>

 

            </tbody>

            <tfoot>
                <tr style="height:30px">
                    <td>
                        <table style="width:100%; height:100%">
                            <tr>
                                <td style="width:20px;"><div></div></td>

                                <td class="Footer_LOG" style="width:190px; display:none;">
                                    <a class="PoweredArea" href="http://www.logitudeworld.com" target="_blank" style="padding:0; margin:0; cursor:pointer; text-decoration:none;">
                                        <table style="height:100%">
                                            <tr>
                                                <td style="width:65px; vertical-align:central; white-space:nowrap;"><p style="font-size:11px; color:#27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</p></td>                            
                                                <td style="width:40px; vertical-align:central;"><img src="../HtmlHelpers/Images/Icons/LogitudeLogo.png" style="width:40px; height:20px;" /></td>
                                                <td style="width:85px; vertical-align:central;"><img src="../HtmlHelpers/Images/Icons/Logitude.png" style="width:79px; height:25px;" /></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </a>
                                </td>

                                <td class="Footer_UNI" style="width:190px; display:none;">
                                    <a class="PoweredArea" href="http://www.amital.co.il" target="_blank" style="padding:0; margin:0; cursor:pointer; text-decoration:none;">
                                        <table style="height:100%">
                                            <tr>
                                                <td style="width:65px; vertical-align:top; padding-top: 7px; white-space:nowrap; font-size:11px; color:#27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</td>                            
                                                <td style="width:28px; vertical-align:middle;"><img src="../images/ApplicationLogo/UnifreightSmallLogo.png" style="width:28px; height:28px;" /></td>
                                                <td style="width:85px; vertical-align:middle; white-space:nowrap; font-size:14px; color:#7F7F7F; font-weight: bold; font-family: 'Lucida Sans Unicode';">Unifreight Cloud Services</td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </a>
                                </td>

                                <td><div></div></td>
                                <td style="width:20px;"><div></div></td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </tfoot>

   

        </table>
    </div>
  
    <script type="text/x-kendo-tmpl" id="RoutingListBoxItemDataTemplate">
        <div class="RoutingListBoxItem">
            <div class="headerDiv" style="height:25px;">
                <div class="content">
                    <table cellpadding="0" cellspacing="0" style="width:100%;">
                        <tr>
                            <td style="width: 5px;"></td>
                            <td style="width:20px; vertical-align:central;"><img src="#= RoutingImageSRC #" style="width:20px; height:20px; display:block;"/></td>
                            <td style="width: 2px;"></td>
                            <td style="vertical-align: middle;"><span class="headerTitle">${LegHeader}</span></td>
                        </tr>
                    </table>                                 
                </div>
            </div>

            <div class="bodyDiv" style="height:75px;">
                <table cellpadding="0" cellspacing="0" style="width:100%; height:52px;">
                    <tr style="height:50px;">
                                                           
                        <td style="width:45px;">
                            <div style="width:45px; height:45px;">
                                <img src="${FromFlagSRC}" />
                            </div>
                        </td>                        

                        <td style="width:50%; vertical-align:central;">
                            <div style="margin-left: 8px;">
                                <div style="height:30px;">
                                  <span class="BlueTextStyle" style="display: inline-block; font-size:16px; white-space: nowrap;">${FromPortCode}</span>
                                  <span style="width:5px; visibility: #= PortNameMarginVisibility #;"></span>
                                  <span class="ValueTextStyle" style="display: inline-block; white-space: nowrap;">${FromPortName}</span>
                                </div>

                                <div style="height:22px;">
                                  <span style="font-size:12px; display: inline-block; width: 80px; color: #= FromDateColor #;">${FromDate}</span>
                                  <span style="font-size:12px; display: inline-block; width: 30px; color: \\#282E30;">${FromTime}</span>
                                  <span style="height:20px; display: inline-block; visibility: #= FromDateTimeVisibility #;">#= FromDateTimeIsActual ? '<img src="../HtmlHelpers/Images/Icons/Tick.png" style="margin-left: 10px; width: 20px; height: 20px; position:relative; margin-top:-5px;" />' : '<span style="margin-left: 10px;">(Estimate)</span>' #</span>
                                </div>
                            </div>
                        </td>

                        <td style="width:45px;">
                            <div style="width:45px; height:45px;">
                                <img src="${ToFlagSRC}" />
                            </div>
                        </td>

                        <td style="width:50%; vertical-align:central;">
                           <div style="margin-left: 8px">
                               <div style="height:30px;">
                                  <span class="BlueTextStyle" style="display: inline-block; font-size:16px; white-space: nowrap;">${ToPortCode}</span>
                                  <span style="width:5px; visibility: #= PortNameMarginVisibility #"></span>
                                  <span class="ValueTextStyle" style="display: inline-block; white-space: nowrap;">${ToPortName}</span>
                               </div>

                               <div style="height:22px;">
                                  <span style="font-size:12px; display: inline-block; width: 80px; color: #= ToDateColor #;">${ToDate}</span>
                                  <span style="font-size:12px; display: inline-block; width: 30px; color: \\#282E30;">${ToTime}</span>
                                  <span style="height:20px; display: inline-block; visibility: #= ToDateTimeVisibility #;">#= ToDateTimeIsActual ? '<img src="../HtmlHelpers/Images/Icons/Tick.png" style="margin-left: 10px; width: 20px; height: 20px; position:relative; margin-top:-5px;" />' : '<span style="margin-left: 10px;">(Estimate)</span>' #</span>
                               </div>                              
                           </div>
                        </td>

                        <td style="width:250px; vertical-align:central; text-align:left;">
                            <div style="width:250px; margin:0; padding:0">

                                <div style="visibility: #= CarrierVisibility #;">
                                  <span style="width:40px;">Carrier:</span>
                                  <span class="ValueTextStyle" style="display:inline-block; vertical-align: middle; font-size: 11px; width:200px; max-width:200px; white-space:nowrap; overflow: hidden; text-overflow: ellipsis; margin:auto;">${Carrier}</span>                                   
                                </div>

                                <div style="visibility: #= CarrierVisibility #;">
                                  <span style="display:inline-block; width:40px;">No:</span>
                                  <span class="ValueTextStyle" style="font-size: 11px">${CarrierNumber}</span>
                                </div>

                                <div style="visibility: #= MasterVisibility #;">                                  
                                   <span style="display:inline-block; width:45px;">Master:</span>
                                   <span class="ValueTextStyle" style="font-size: 11px">${Master}</span>                                  
                                </div>

                                <div style="visibility: #= VisselVisibility #;">
                                    <span style="display:inline-block; width:40px;">Vessel:</span>
                                    <span class="ValueTextStyle" style="font-size: 11px">${Vissel}</span>
                                </div>

                            </div>
                        </td>                    
                     </tr>
                  </table>                  
            </div>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="PartnerListBoxItemDataTemplate">
        <div class="PartnerListBoxItem">
            <table style="width:100%; height:100%;">
                <thead style="height:25px;">
                <tr style="height:25px;">
                    <td>
                        <div class="headerDiv">
                            <div class="content">
                                <span class="headerTitle" style="line-height:20px;">${PartnerType}</span>
                                <span class="headerTitle" style="line-height:20px;">: </span>
                                <span class="headerValue" style="line-height:20px;">${PartnerName}</span>
                            </div>
                         </div>       
                    </td>
                </tr>
                </thead>

                <tfoot style="height:45px;">
                <tr style="height:45px;">
                    <td style="vertical-align:top;">

                        <table width="100%" style="margin-top: -17px;">
                            <col width="5">                    
                            <col width="*">
                            <col width="45">
                            <col width="2">

                            <tr>
                                <td></td>
                                <td>
                                    <table style="width:100%; visibility: #= ReferenceVisibility #;">
                                        <tr>
                                            <td style="width:50%;">
                                                <span class="LineClass" style='font-size:11px; color:black;'>Ref1: </span>
                                                <span class="LineClass">${Reference1}</span>
                                            </td>

                                            <td style="width:50%;">
                                                <span class="LineClass" style='visibility: #= Reference2Visibility #; font-size:11px; color:black;'>Ref2: </span>
                                                <span class="LineClass" style="visibility: #= Reference2Visibility #;">${Reference2}</span>
                                            </td>
                                        </tr>
                                    </table> 
                                </td>
     
                                <td style="width:45px; height:45px; vertical-align:top;">
                                 <div style="visibility: #= FlagSRCVisibility #;">
                                    <img src="${FlagSRC}" style="width:45px; height:45px; line-height:45px; margin:0; padding:0; margin-top: -7px;visibility: inherit" />
                                </div>
                                </td>

                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </tfoot>

                <tbody style="height:100px;">
                <tr>
                    <td style="vertical-align:top;">
                        <div class="bodyDiv">
                            <div class="LineClass">${Name}</div>
                            <div class="LineClass">${Address1}</div>
                            <div class="LineClass">${Address2}</div>

                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td style="width:50%;"><div class="LineClass">${CityZipCode}</div></td>
                                    <td style="width:50%;">
                                        <span class="LineClass" style='font-size:11px; color:black;'>Tel: </span>
                                        <span class="LineClass">${Phone}</span>
                                    </td>
                                </tr>
                            </table>
                
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td style="width:50%;"><div class="LineClass">${CountryName}</div></td>
                                    <td style="width:50%;">
                                        <span class="LineClass" style='font-size:11px; color:black;'>Fax: </span>
                                        <span class="LineClass">${Fax}</span>
                                    </td>
                                </tr>
                            </table>
        
                            <div>
                                <span class="LineClass" style='font-size:11px; color:black;'>Contact: </span>
                                <span class="LineClass">${ContactName}</span>
                            </div>
                        
                        </div>
                    </td>
                </tr>
                </tbody>

            </table>

        
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="DocumentListBoxItemDataTemplate">
        <a class="DocumentListBoxItem" id="#= Id #" OnClick="OnDownloadDocument(Id)">
             <div class="content">
                 <div>${Name}</div>
             </div>
        </a>
    </script>
    
    <script type="text/x-kendo-tmpl" id="EventListBoxItemDataTemplate">
        <div class="EventListBoxItem">
            <div>
                <table width="100%" style="border-collapse: collapse; border-spacing: 0px; border: 0px;">
                    <col width="8">
                    <col width="*">
                    <col width="250">

                    <tr>
                        <td></td>
                        <td style="vertical-align:top; padding:5px 0 0 0;">
                            <div style="height:20px; text-indent:5px; font-family:'Lucida Sans Unicode'; font-size: 16px; color: \\#1B90CB;">${Name}</div>

                            <div style="margin-top:5px; height:20px; text-indent:5px;">
                                <span class="LabelTextStyle" style="display: inline-block; width: 70px;">Log Date:</span>
                                <span class="ValueTextStyle" style="display: inline-block; width: 90px;">${LogDate}</span>
                                <span class="ValueTextStyle" style="display: inline-block; width: 70px;">${LogTime}</span>

                                <span class="LabelTextStyle" style="display: inline-block; width: 30px;">User:</span>
                                <span class="ValueTextStyle" style="display: inline-block;">${Username}</span>
                            </div>

                            <div style="height:20px; text-indent:5px;">
                                <span class="LabelTextStyle" style="display: inline-block; width: 70px;">Event Date:</span>
                                <span class="ValueTextStyle" style="display: inline-block; width: 90px;">${EventDate}</span>
                                <span class="ValueTextStyle" style="display: inline-block; width: 70px;">${EventTime}</span>
                            </div>
                        </td>

                        <td>
                            <div style="height:73px; margin:2px; background: \\#FFFADC; border: 1px solid \\#D1D1D1; overflow:auto; white-space:normal; padding:1px 3px;">
                                ${Notes}
                            </div>


                        </td>

                    </tr>

                </table>
                 

            </div>
        </div>
    </script>
    <script type="text/javascript">
        function OnDownloadDocument(url) {
            if ($.IsDocumentsApprovalRequried) {
                $.DownloadAll = false;
                $.DocumentUrl = url;
                $('#Container2').show();
            }
            else {
                $.SendContactActivity($.CurrentEmail, "Shipment", "Document Download", $.CurrentTenant, $.CurrentCardId);
                window.open(url);
            }
            //const left = document.documentElement.clientWidth / 2.5;
            //const top = document.documentElement.clientHeight / 2.5;
            //var winFeature = 'width=330,height=125,top = ' + top + ',left = ' + left + '';
            //var win = window.open('DocumentsApprovalPage.aspx', 'popup_window', winFeature);
            //var timer = setInterval(function () {
            //    if (win.closed) {
            //        clearInterval(timer);
            //        //$('#Container').show();
            //        $('#Container2').hide();
            //    }
            //}, 1000);
        }


        function CloseButtonClicked() {
            $('#Container2').hide();
        }

        function ApproveButtonClicked() {
            if (!$('#ApprovalName').val()) {
                $('#ErrorMessage').show();
            }
            else {
                $('#ErrorMessage').hide();
                $.DocumentsApprovalName = $('#ApprovalName').val();
                $.PutDocumentsApprovedByUserName();
                $('#Container2').hide();
                if ($.DownloadAll) {
                    $.DownloadAll = false;
                    $.SendContactsActivity($.CurrentEmail, "Shipment", "Document Download", $.CurrentTenant, $.CurrentCardId);
                    window.open("../WebPages/SharedDownloadPage.aspx?id=" + $.CurrentTenant + ":" + null + ":ship:" + $.CurrentEntityId + ":" + $.CurrentCardType);
                }
                else {
                    $.SendContactActivity($.CurrentEmail, "Shipment", "Document Download", $.CurrentTenant, $.CurrentCardId);
                    window.open($.DocumentUrl);
                }
            }
        }

        function OnDownloadAllDocument() {
            if ($.IsDocumentsApprovalRequried) {
                $.DownloadAll = true;
                $('#Container2').show();
            }
            else {
                $.SendContactsActivity($.CurrentEmail, "Shipment", "Document Download", $.CurrentTenant, $.CurrentCardId);
                window.open("../WebPages/SharedDownloadPage.aspx?id=" + $.CurrentTenant + ":" + null + ":ship:" + $.CurrentEntityId + ":" + $.CurrentCardType);
            }
        }

        function GetURL() {
            return null;
            //return "../WebPages/SharedDownloadPage.aspx?id=" + $.CurrentTenant + ":" + null + ":ship:" + $.CurrentEntityId + ":" + $.CurrentCardType;
        }
    </script>
   
    <script type="text/javascript" src="ShipmentPageViewModel.js"></script>
    
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
