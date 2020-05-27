<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SharedDocuments.aspx.cs" Inherits="WebFreight.Web.SharedDocuments" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <title>Shared Documents</title>

    <link href="HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css"/>    
    <link href="HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/kendo.dataviz.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/Kendo.2013.2.918/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/Kendo.2013.2.918/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css"/>

    <script src="HtmlHelpers/JS/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/jquery.dateFormat-1.0.js" type="text/javascript"></script>
    <script src="HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/Logitude.Converters.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/Logitude.Entites.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/ContactActivityLog.js" type="text/javascript"></script>   
    
    <style type="text/css">
        #EntityHeaderArea {
            height: 60px;
            margin: 0 5px;
            background: #F7F7F7;
            border: 1px solid #DADADA;
            padding-top: 3px;
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
    </style> 
</head>

<body>

    <script src="HtmlHelpers/JS/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/knockout-kendo.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>

    <div id="ContainerHeader" style="position:absolute; top:0px; z-index:0; width:100%; height:40px; background: url('HtmlHelpers/Images/Bars_Images/HeaderBar.png') repeat-x; border-bottom:1px solid #D1D1D1"></div>

    <div id="Container" style="position:absolute; top:0px; z-index:3; width:100%;display:normal">
        <table style="height:100%;">

            <thead>
                <tr style="height:40px;">
                    <td style="vertical-align:top;">
                    
                        <table style="margin:5px 0 0 0;">
                            <tr>
                                <td style="width:5px;"></td>

                                <td style="width:50px;">
                                    <img id="companyLogo" src="HtmlHelpers/Images/Icons/Logo.png" style="width:50px; height:35px; vertical-align:bottom; position:absolute; top:2px;"/>                    
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

<%--                                <td style="width:22px; vertical-align:top;">
                                    <div style="margin-top:-5px;" id="SignOutButton"></div> 
                                </td>--%>

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
                                    <a href="http://www.logitudeworld.com" target="_blank" style="padding:0; margin:0; cursor:pointer; text-decoration:none;">
                                        <table style="height:100%">
                                            <tr>
                                                <td style="width:65px; vertical-align:central; white-space:nowrap;"><p style="font-size:11px; color:#27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</p></td>                            
                                                <td style="width:40px; vertical-align:central;"><img src="HtmlHelpers/Images/Icons/LogitudeLogo.png" style="width:40px; height:20px;" /></td>
                                                <td style="width:85px; vertical-align:central;"><img src="HtmlHelpers/Images/Icons/Logitude.png" style="width:79px; height:25px;" /></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </a>
                                </td>

                                <td class="Footer_UNI" style="width:190px; display:none;">
                                    <a href="http://www.amital.co.il" target="_blank" style="padding:0; margin:0; cursor:pointer; text-decoration:none;">
                                        <table style="height:100%">
                                            <tr>
                                                <td style="width:65px; vertical-align:top; padding-top: 7px; white-space:nowrap; font-size:11px; color:#27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</td>                            
                                                <td style="width:28px; vertical-align:middle;"><img src="images/ApplicationLogo/UnifreightSmallLogo.png" style="width:28px; height:28px;" /></td>
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

<%--                                <td style="width:70px; vertical-align:bottom;">
                                    <div id="BackButton"></div>    
                                </td>--%>

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
                                                    <td style="width:250px; vertical-align:top;">
                                                        <%--<p class="BlueLabel">General</p>--%>
                                                        <div style="margin:0 0 0 5px;">
                                                        <div>
                                                        <span class="LabelTextStyle" style="display:inline-block; width:40px;">Ref.:</span>
                                                        <span class="ValueTextStyle" data-bind="text: MyReference"></span>
                                                        </div>
                                                        <div>
                                                        <span class="LabelTextStyle" style="display:inline-block; width:40px;">House:</span>
                                                        <span class="ValueTextStyle" data-bind="text: House"></span>
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
                                                                        <img class="ShowOnDataControl" src="HtmlHelpers/Images/Arrow.png" style="height: 16px; width: 16px; display:block; margin:auto;"/>
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
                                                        <%--<p class="BlueLabel" data-bind="text: PartnerTitle"> </p>--%>
                                                        <div style="margin:0 0 0 5px;">
                                                            <div>
                                                                <span class="LabelTextStyle" data-bind="text: PartnerTitle" style="display:inline-block; width:65px;"> </span>                                                                
                                                                <span class="ValueTextStyle" data-bind="text: PartnerName"> </span>
                                                            </div>

                                                            <div>
                                                                <span class="LabelTextStyle" style="display:inline-block; width:65px;"></span> 
                                                                <span class="ValueTextStyle" data-bind="text: PartnerAddress"> </span>
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
                                       <td style="width:21px; vertical-align:top; text-align:left;"><div style="margin:-1px -1px 0 1px; height:16px; width:21px; background:url('HtmlHelpers/Images/bars_Images/Left-side.png') no-repeat";></div></td>
                                       <td style="background:url('HtmlHelpers/Images/bars_Images/Middle-side.png') repeat-x"></td>
                                       <td style="width:21px; background:url('HtmlHelpers/Images/bars_Images/Right-side.png') no-repeat"></td>
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


                                                    <li id="TAB_DOC" class="k-state-active">
                                                        <div>
                                                            <span><img class="TabImage" src="HtmlHelpers/Images/Tabs_Images/Document.png" style="opacity:0.8"/></span>
                                                            <span>Documents</span>
                                                        </div>
                                                    </li>



                                                </ul>


                                                <div class="tabPage">
<%--                                                    <div class="ScrollViewer" style="overflow:auto;">
                                                        <div id="DocumentsListBox" class="ListBox"></div>
                                                    </div> --%> 
                                                    
                                                    <table style="width:100%; height:100%;" id="DocumentsTabPageControl">
                                                        <tr>
                                                            <td style="vertical-align: top;">
                                                                  <table>
                                                                        <tr>
                                                                            <td>

                                                                            </td>
                                                                            <td style="width:100px;text-align:right">
                                                                                <a  id="DownloadAll"  href='javascript:document.location.href=GetURL();' target="_blank" onclick='OnDownloadAllDocument()'>
                                                                                <div style="cursor:pointer; font-size:13px; color:#27AAE1; text-align:right;padding-right:5px"> Download All</div>
                                                                                    </a>
                                                                               </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                    <div id="DocumentsGrid" style="margin-top:5px;overflow:auto;" ></div>                                                                    
                                                                            </td>
                                                                        </tr>
                                                                    </table>                                                              
                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <div class="BusyIndicator" id="PageBusyIndicator"></div>                      
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
                                       <td style="width:21px;"><div style="width:21px; height:16px; margin:-1px -5px 0 5px; background:url('HtmlHelpers/Images/bars_Images/bottom-left.png') no-repeat"></div></td>
                                       <td style="background:url('HtmlHelpers/Images/bars_Images/bottom-middle.png') repeat-x"></td>
                                       <td style="width:21px; background:url('HtmlHelpers/Images/bars_Images/bottom-right.png') no-repeat"></td>     
                                    </tr>
                                </table>
                            </div>
                    </td>
                </tr>

            </tbody>

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
                                    <img id="Img1" src="HtmlHelpers/Images/Icons/Logo.png" style="width:50px; height:35px; vertical-align:bottom; position:absolute; top:2px;"/>                    
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
                                       <td style="width:21px; vertical-align:top; text-align:left;"><div style="margin:-1px -1px 0 1px; height:16px; width:21px; background:url('HtmlHelpers/Images/bars_Images/Left-side.png') no-repeat";></div></td>
                                       <td style="background:url('HtmlHelpers/Images/bars_Images/Middle-side.png') repeat-x"></td>
                                       <td style="width:21px; background:url('HtmlHelpers/Images/bars_Images/Right-side.png') no-repeat"></td>
                                       <td style="width:3px;"></td>
                                    </tr>
                                </table>--%>
                            </div>
                    </td>
                </tr>

                <tr>
                    <td style="vertical-align:top;">
                        <div class="pageContent" style="width:100%; position:relative; margin:-10px 0 0px 0;">
                          <%--  <img width="60" height="60" src="HtmlHelpers/Images/SimplogIcons/warning.png" /> --%>
                           
                        </div>
                    </td>
                </tr>

                <tr style="height:16px;">
                    <td>
                            <div style="width:100%; height:16px; margin-top:0px;">
                              <%--  <table style="width:100%; height:100%; border-collapse:collapse; border-spacing:0px;">
                                    <tr>                                                                             
                                       <td style="width:21px;"><div style="width:21px; height:16px; margin:-1px -5px 0 5px; background:url('HtmlHelpers/Images/bars_Images/bottom-left.png') no-repeat"></div></td>
                                       <td style="background:url('HtmlHelpers/Images/bars_Images/bottom-middle.png') repeat-x"></td>
                                       <td style="width:21px; background:url('HtmlHelpers/Images/bars_Images/bottom-right.png') no-repeat"></td>     
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
                                    <a href="http://www.logitudeworld.com" target="_blank" style="padding:0; margin:0; cursor:pointer; text-decoration:none;">
                                        <table style="height:100%">
                                            <tr>
                                                <td style="width:65px; vertical-align:central; white-space:nowrap;"><p style="font-size:11px; color:#27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</p></td>                            
                                                <td style="width:40px; vertical-align:central;"><img src="HtmlHelpers/Images/Icons/LogitudeLogo.png" style="width:40px; height:20px;" /></td>
                                                <td style="width:85px; vertical-align:central;"><img src="HtmlHelpers/Images/Icons/Logitude.png" style="width:79px; height:25px;" /></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </a>
                                </td>

                                <td class="Footer_UNI" style="width:190px; display:none;">
                                    <a href="http://www.amital.co.il" target="_blank" style="padding:0; margin:0; cursor:pointer; text-decoration:none;">
                                        <table style="height:100%">
                                            <tr>
                                                <td style="width:65px; vertical-align:top; padding-top: 7px; white-space:nowrap; font-size:11px; color:#27AAE1; font-family: 'Lucida Sans Unicode';">Powered by</td>                            
                                                <td style="width:28px; vertical-align:middle;"><img src="images/ApplicationLogo/UnifreightSmallLogo.png" style="width:28px; height:28px;" /></td>
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

    <script type="text/x-kendo-tmpl" id="DocumentListBoxItemDataTemplate">
        <a class="DocumentListBoxItem" id="#= Id #"  href="#= Url #" target="_blank" OnClick="OnDownloadDocument()">
             <div class="content">
                 <div>${Name}</div>
             </div>
        </a>
    </script>
        
    <script type="text/javascript">
        function OnDownloadDocument(documentId) {
            $.SendContactActivity($.CurrentEmail, "Shipment", "Document Download", $.CurrentTenant, $.CurrentCardId);
        }

        function OnDownloadAllDocument() {
            $.SendContactsActivity($.CurrentEmail, "Shipment", "Document Download", $.CurrentTenant, $.CurrentCardId);
            window.open("../WebPages/CorrespondenceDownloadpage.aspx?securitykey=" + $.CurrentEntityKey + ":" + $.CurrentEntityId + ":" + $.CurrentCardType + ":" + $.CurrentTenant + "&DA=1");
        }

        function GetURL() {
            return "../WebPages/CorrespondenceDownloadpage.aspx?securitykey=" + $.CurrentEntityKey + ":" + $.CurrentEntityId + ":" + $.CurrentCardType + ":" + $.CurrentTenant + "&DA=1";
        }
    </script>

    <script type="text/javascript" src="SharedDocuments/SharedDocumentsViewModel.js"></script>

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
    </script>
</body>
</html>
