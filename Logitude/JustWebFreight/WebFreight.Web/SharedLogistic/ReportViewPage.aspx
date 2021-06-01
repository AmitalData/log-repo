<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewPage.aspx.cs" Inherits="WebFreight.Web.SharedLogistic.ReportViewPage" %>

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

    .greenButton {
        outline: none;
        text-align: right;
        font-size: 11px;
        color: #45494A;
        width: 100%;
        height: 22px;
        line-height: 21px;
        border: 1px solid #EDC093;
        border-radius: 3px;
        -moz-border-radius: 3px;
        -webkit-border-radius: 3px;
        position: relative;
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -webkit-box-sizing: border-box;
               background: -moz-linear-gradient(20% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgb(0, 255, 33) 70%);
        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgb(106, 190, 163) 70%);
        background: -webkit-gradient(linear,20% 0%,50% 70%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(237, 192, 147, 1) ));
        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 20%, rgb(106, 190, 163) 70%);
        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgb(106, 190, 163) 70%);
        -moz-box-shadow: 0px 0px 6px 0px green;
        -moz-box-shadow: 0px 0px 6px 0px green;
        text-shadow: 1px 1px white;
    }

        .greenButton:hover {
            background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgb(0, 255, 33) 100%);
        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgb(106, 190, 163) 100%);
        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(237, 192, 147, 1) ));
        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgb(106, 190, 163) 100%);
        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgb(106, 190, 163) 100%);
        }

         .greenButton:active {
            -webkit-box-shadow: 0px 0px 5px 1px rgba(0,148,118,1);
-moz-box-shadow: 0px 0px 5px 1px rgba(0,148,118,1);
box-shadow: 0px 0px 5px 1px rgba(0,148,118,1);

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
                                    <div class="ShowOnDataControl" style="background:#F2F2F2; float:left; padding:2px 10px 2px 2px">
                                        <span style="font-size:15px; display:inline; color:#1B90CB;"> Shipments Reports</span>
                                    </div>
                                </td>

                                <td style="width:10px;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr style="height:80px;">
                    <td style="padding:7px">
                        <div style="width:100%;height:100%;vertical-align:central">

                            <table>
                                <tr>
                                    <td style="width:350px">
                                        <label style="display:inline" for="start">From Date:</label>
                                        <input type="date" id="fromDate" name="fromDate" style="margin-left:5px" value="2018-07-22" />

                                    </td>

                                        <td style="width:350px">
                                         <label style="display:inline" for="start">To Date:</label>

                                        <input type="date" id="toDate" name="toDate" style="margin-left:5px" value="2018-07-22" />

                                    </td>

                                    <td style="width:200px">
                                       <label for="vehicle1" style="display:inline;vertical-align:bottom"> Operationally Closed</label>

                                          <input type="checkbox" id="vehicle1" name="vehicle1" value="Bike"/>  
                                    </td>
                                    <td style="width:100px">
                                              <button (onclick)="RunReport()" class="greenButton" style="width:90px;height:22px;border:1px solid green;border-radius:3px;">
                            <div style="color:black;  text-align:center;vertical-align:central;font-size:12px;">Run Report</div>
                        </button>          
                                    </td>
                                    <td></td>
                                </tr>
                            </table>


                                </div>
                    </td>
                </tr>

                <tr style="height:10px"><td></td></tr>


                <tr>
                    <td style="padding:7px;padding-right:10px">
                        <div style="border:1px solid gray;background-color:#D1D6D8;height:100%;width:100%;vertical-align:central">
           <div style="height:47%"></div>
          <div  style="text-align:center;vertical-align:central;font-size:15px">The report is too large to preview. Please use Print/Save buttons to download it. </div>  
                        </div>
                    </td>
                </tr>

           

            </tbody>

        </table>
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