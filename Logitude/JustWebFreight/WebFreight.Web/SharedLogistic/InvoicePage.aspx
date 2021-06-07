<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoicePage.aspx.cs" Inherits="WebFreight.Web.SharedLogistic.InvoicePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Invoice</title>

    <link href="../css/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="../css/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <script src="../js/jquery-3.5.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.dateFormat-1.0.js" type="text/javascript"></script>
    <script src="../js/kendo.all.min.js" type="text/javascript"></script>
    <script src="../js/knockout-3.5.1.js" type="text/javascript"></script>
    <script src="../js/knockout-kendo.min.js" type="text/javascript"></script>

    <link href="../HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css"/>    
    <link href="../HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css"/>   
    <script src="../HtmlHelpers/JS/Logitude.Converters.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/Logitude.Entites.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/ContactActivityLog.js" type="text/javascript"></script>

<style type="text/css">

    #EntityHeaderArea
    {
        height: 60px;
        margin: 0 5px;
        background: #F7F7F7;
        border: 1px solid #DADADA;
        padding-top: 3px;
    }

    .SummaryItem
    {
        display:inline-block;
        text-align:center; 
        vertical-align:top;
    }

    .SummaryItemValue
    {
        font-size: 18px;
        color: #282E30;
        font-family: 'Lucida Sans Unicode';
        height: 30px;
    }

    .SummaryItemLabel
    {
        font-size: 11px;
        color: #6E7172;
        font-family: 'Lucida Sans Unicode';
        height: 15px;
    }

</style>

</head>

<body>

    <form style="visibility:collapse;">
        <input id="TokenInput" runat="server" />
        <input id="LoginInput" runat="server" />
    </form>

    <script src="../HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/app.js" type="text/javascript"></script>

    <div id="ContainerHeader" style="position:absolute; top:0px; z-index:0; width:100%; height:40px; background: url('../HtmlHelpers/Images/Bars_Images/HeaderBar.png') repeat-x; border-bottom:1px solid #D1D1D1"></div>

    <div id="Container" style="position:absolute; top:0px; z-index:3; width:100%">
        <table style="height:100%;">

            <thead>
                <tr style="height:40px;">
                    <td style="vertical-align:top;">
                    
                        <table style="margin:5px 0 0 0;">
                            <tr>
                                <td style="width:5px;"></td>

                                <td id ="companyLogoArea" style="width:50px;">
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
                <tr style="height:30px;">
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
                                        <span style="font-size:15px; display:inline; color:#1B90CB;" data-bind="text: EntityNumber"> </span>
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
                                                    <td style="width:250px; vertical-align:top;">
                                                        <%--<p class="BlueLabel">General</p>--%>
                                                        <div style="margin:0 0 0 5px;">
                                                            <div>
                                                            <span class="LabelTextStyle" style="display:inline-block; width:67px;" >Our Ref No: </span>
                                                            <span class="ValueTextStyle" data-bind="text: Reference"></span>
                                                            </div>

                                                            <div>
                                                            <span class="LabelTextStyle" style="display:inline-block; width:67px;">Status: </span>
                                                            <span class="ValueTextStyle" data-bind="style: {color: StatusColor}, text: Status"></span>
                                                            </div>
                                                        </div>
                                                    </td>

                                                    <td style="width:1px; vertical-align:top;"><div style="height:50px; width:1px; background:#DADADA; margin-top:4px;"></div></td>

                                                    <td style="width:350px; vertical-align:top;">
                                                        <%--<p class="BlueLabel">Routing</p>--%>
                                                        <div style="margin:0 0 0 5px;">
                                                            <div>
                                                            <span class="LabelTextStyle" style="display:inline-block; width:75px;">Amount Due: </span>
                                                            <span class="ValueTextStyle" data-bind="text: AmountDue"></span>
                                                            </div>

                                                            <div>
                                                            <span class="LabelTextStyle" style="display:inline-block; width:75px;">Due Date: </span>
                                                            <span class="ValueTextStyle" data-bind="text: DueDate"></span>
                                                            </div>
                                                        </div>
                                                    </td>

                                                    <td style="width:1px; vertical-align:top;"><div style="height:50px; width:1px; background:#DADADA; margin-top:4px;"></div></td>

                                                    <td style="vertical-align:top;">
                                                        <%--<p class="BlueLabel" data-bind="text: PartnerTitle"> </p>--%>
                                                        <div style="margin:0 0 0 5px;">
                                                            <div>
                                                            <span class="LabelTextStyle" style="display:inline-block; width:85px;">Payment Term: </span>
                                                            <span class="ValueTextStyle" data-bind="text: PaymentTermName"></span>
                                                            </div>

                                                            <div>
                                                            <span class="LabelTextStyle" style="display:inline-block; width:85px;">Invoice Date: </span>
                                                            <span class="ValueTextStyle" data-bind="text: InvoiceDate"></span>
                                                            </div>

                                                        </div>

                                                        <div>
                                                              <a  data-bind="attr: { href: ReportUrl}" target="_blank" onclick="OnDownloadDocument()">
                                                                  <div class="content">        
                                                                      <div style="display:inline-block;"><img style="width:20px; height:20px;" src="../HtmlHelpers/Images/pdf.png" title="Download"/></div>
                                                                      <div style="display:inline-block; vertical-align:top; margin-top:3px;">Download</div>                                                                                                                                    
                                                                   </div>
                                                              </a>
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
                                    <td style="width:10px; height:100%;"><div style="width:1px; height:100%; background:#D1D1D1; margin-left:9px;"></div></td>

                                    <td style="vertical-align:top;">
                                        <div>

                                            <div id="mainTabsDiv">

                                                <ul>
                                                    <li id="TAB_DET" class="k-state-active">
                                                        <div>
                                                            <span><img class="TabImage" src="../HtmlHelpers/Images/Tabs_Images/Lines.png" style="opacity:0.8"/></span>
                                                            <span>Details</span>
                                                        </div>
                                                    </li>

                                                    <li id="TAB_PAY">
                                                        <div>
                                                            <span><img class="TabImage" src="../HtmlHelpers/Images/Tabs_Images/Document.png" style="opacity:0.8"/></span>
                                                            <span>Payments</span>
                                                        </div>
                                                    </li>

                                                </ul>

                                                <div class="tabPage" style="padding-left:0px; padding-right:0px;">

                                                    <table style="width:100%; height:100%;">
                                                        
                                                        <tr>
                                                            <td style="vertical-align:top;">
                                                                <div class="LinesScrollViewer" style="overflow:auto;">
                                                                    <div id="InvoiceLinesGrid" style="margin-top:5px;"></div>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                        <tr style="height:10px;"><td style="height:10px; background:url('../HtmlHelpers/Images/Bars_Images/top-middle.png') repeat-x"></td></tr>

                                                        <tr style="height:70px;">
                                                            <td style="height:70px;">

                                                                <table id="InvoiceSummary" style="width:100%; height:100%;">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width: 250px;" />
                                                                    </colgroup>

                                                                    <tr>

                                                                        <td style="vertical-align:top; text-align:left;">
                                                                            <div class="BlueTextStyle" style="height:20px; text-indent:10px;">Totals</div>

                                                                            <div class="ShowOnDataControl" id="InvoiceTotalsControl" style="margin:0; padding:0; height:50px; text-indent:10px;">
                                                                                       
                                                                                <div class="SummaryItem" id="SubTotal">
                                                                                    <div class="SummaryItemValue" data-bind="text: SubTotals"></div>
                                                                                    <div class="SummaryItemLabel">subtotal</div>
                                                                                </div>
                                                                                                                                                                                                                                                           
                                                                                <div class="SummaryItem">
                                                                                    <div class="SummaryItemValue">=</div>
                                                                                    <div class="SummaryItemLabel"></div>
                                                                                </div>

                                                                                <div style="display:inline-block; text-align:center; vertical-align:top; ">
                                                                                    <div class="SummaryItemValue" style="font-size:24px; margin-top:-5px;" data-bind="text: InvoiceAmount"></div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px; margin-top:5px;">
                                                                                        <span style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode';">Invoice Amount</span>
                                                                                        <span style="font-size:11px; color: #282E30; font-family: 'Lucida Sans Unicode';" data-bind="text: InvoiceCurrencyCode"></span>
                                                                                    </div>
                                                                                </div>                                                                                                                                                                                                                                                                                   
                                                                            </div>
                                                                        </td>

                                                                        <td style="width: 250px;">
                                                                            <div class="LabelTextStyle" style="height:15px; font-size:11px; text-indent:2px;">Print notes</div>
                                                                            <div style="height:50px; background: #FFFADC; border: 1px solid #D1D1D1; overflow:auto; white-space:normal; padding:1px 3px;" data-bind="text: PrintNotes"></div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    
                                                </div> 
                                                                                                
                                                <div class="tabPage" style="padding-left:0px; padding-right:0px;">

                                                    <table style="width:100%; height:100%;" id="PaymentsTabPageControl">

                                                        <tr>
                                                            <td style="vertical-align:top;">
                                                                <div class="LinesScrollViewer" style="overflow:auto;">
                                                                    <div id="PaymentsGrid" style="margin-top:5px;"></div>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                        <tr style="height:10px;"><td style="height:10px; background:url('../HtmlHelpers/Images/Bars_Images/top-middle.png') repeat-x"></td></tr>

                                                        <tr style="height:70px;">
                                                            <td style="height:70px;">

                                                                <table id="PaymentsSummary" style="width:100%; height:100%;">
                                                                    <colgroup>
                                                                        <col />
                                                                        <col style="width: 250px;" />
                                                                    </colgroup>

                                                                    <tr>
                                                                        <td style="vertical-align:top; text-align:left;">
                                                                            <div class="BlueTextStyle" style="height:20px; text-indent:10px;">Totals</div>

                                                                            <div class="ShowOnDataControl" style="margin:0; padding:0; height:50px; text-indent:10px;">
                                                                                     
                                                                                <div style="display:inline-block; text-align:center; vertical-align:top;">
                                                                                    <div class="SummaryItemValue" data-bind="text: InvoiceAmount"></div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px;">Invoice Amount</div>
                                                                                </div>
                                                                                     
                                                                                <div style="display:inline-block; text-align:center; vertical-align:top;">
                                                                                    <div class="SummaryItemValue">-</div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px;"></div>
                                                                                </div>
                                                                                     
                                                                                <div style="display:inline-block; text-align:center; vertical-align:top;">
                                                                                    <div class="SummaryItemValue" data-bind="text: PaidAmount"></div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px;">Amount Paid</div>
                                                                                </div>
                                                                                     
                                                                                <div style="display:inline-block; text-align:center; vertical-align:top;">
                                                                                    <div class="SummaryItemValue">=</div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px;"></div>
                                                                                </div>
                                                                                    
                                                                                <div style="display:inline-block; text-align:center; vertical-align:top;">
                                                                                    <div class="SummaryItemValue" style="font-size:24px; margin-top:-5px;" data-bind="text: AmountDue"></div>
                                                                                    <div style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode'; height:15px; margin-top:5px;">
                                                                                        <span style="font-size:11px; color: #6E7172; font-family: 'Lucida Sans Unicode';">Amount Due</span>
                                                                                        <span style="font-size:11px; color: #282E30; font-family: 'Lucida Sans Unicode';" data-bind="text: InvoiceCurrencyCode"></span>
                                                                                    </div>
                                                                                </div>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </td>
                                                        </tr>
                                                    </table> 
                                                    
                                                    <div class="BusyIndicator" id="PaymentsPageBusyIndicator"></div>                      
                                                </div>                                                

                                            </div>                                               

                                        </div>
                                    </td>

                                    <td style="width:10px; height:100%;"><div style="width:1px; height:100%; background:#D1D1D1; margin-left:0px;"></div></td>                                
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
                                       <td style="width:21px;"><div style="width:21px; height:16px; margin:-1px -5px 0 5px; background:url('../HtmlHelpers/images/bars_Images/bottom-left.png') no-repeat"></div></td>
                                       <td style="background:url('../HtmlHelpers/images/bars_Images/bottom-middle.png') repeat-x"></td>
                                       <td style="width:21px; background:url('../HtmlHelpers/images/bars_Images/bottom-right.png') no-repeat"></td>     
                                    </tr>
                                </table>
                            </div>
                    </td>
                </tr>

            </tbody>

        </table>

      <div class="BusyIndicator" id="DetailsPageBusyIndicator"></div>                                                                               

    </div>

     <script type="text/javascript">
         function OnDownloadDocument() {             
             $.SendContactActivity($.CurrentEmail, "Invoice", "Document Download", $.CurrentTenant,$.CurrentCardId);
         }
    </script>

    <script type="text/javascript" src="InvoicePageViewModel.js"></script>

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

</body>
</html>
