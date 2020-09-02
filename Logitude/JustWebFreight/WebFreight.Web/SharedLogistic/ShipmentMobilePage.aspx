<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShipmentMobilePage.aspx.cs" Inherits="WebFreight.Web.SharedLogistic.ShipmentMobilePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>

    <link href="http://cdn.kendostatic.com/2013.1.319/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="http://cdn.kendostatic.com/2013.1.319/styles/kendo.default.min.css" rel="stylesheet" />
    <link href="http://cdn.kendostatic.com/2013.1.319/styles/kendo.mobile.all.min.css" rel="stylesheet" />

    <script src="http://code.jquery.com/jquery-1.8.2.min.js"></script>
    <script src="http://cdn.kendostatic.com/2013.1.319/js/kendo.all.min.js"></script>

    <script type="text/javascript" src="../HtmlHelpers/JS/jquery.dateFormat-1.0.js"></script>
    <script type="text/javascript" src="../HtmlHelpers/JS/Logitude.Converters.js"></script>
    <script type="text/javascript" src="../HtmlHelpers/JS/ContactActivityLog.js"></script>

<style type="text/css">
                      
    .km-GEN { -webkit-mask-box-image: url("../HtmlHelpers/Images/Tabs_Images/Home.png"); }           
    .km-ROU { -webkit-mask-box-image: url("../HtmlHelpers/Images/Tabs_Images/Routing.png"); }
    .km-PAR { -webkit-mask-box-image: url("../HtmlHelpers/Images/Tabs_Images/Partners.png"); }
    .km-CAR { -webkit-mask-box-image: url("../HtmlHelpers/Images/Tabs_Images/Cargo.png"); }
    .km-MON { -webkit-mask-box-image: url("../HtmlHelpers/Images/Tabs_Images/Money.png"); }
    .km-DOC { -webkit-mask-box-image: url("../HtmlHelpers/Images/Tabs_Images/Document.png"); }
    .km-EVE { -webkit-mask-box-image: url("../HtmlHelpers/Images/Tabs_Images/Events.png"); }

    .km-ROU,
    .km-PAR,
    .km-CAR,
    .km-MON,
    .km-DOC,                
    .km-EVE
    {
        background-color: red;        
    }
    
    .km-root .km-pane .km-view .km-icon {
        background-size: 100% 100%;
        -webkit-background-clip: border-box;
        background-color: currentcolor;
    }

    html, body
    {
        overflow: hidden;
    }

    .BlueLabel
    {
        font-size: 12px;
        color: #369;
        font-family: "Arial";
    }

    .ItemLabel
    {
        font-size: 12px;
        color: #777;
        font-family: "Arial";
        vertical-align:central;
    }

    .ItemValue
    {
        font-size: 12px;
        float: right;
        font-family: "Arial";
        vertical-align:central;
    }

    .ItemValueStyle
    {
        font-size: 12px;
        font-family: "Arial";
    }

    .BusyIndicator
    {
        display: none;
        position: absolute;
        top: 40%;
        left: 40%;
        background: url("../HtmlHelpers/Images/Icons/Progress.gif") no-repeat;
        width: 100px;
        height: 100px;
        
    }

        /*
    .km-listview .km-list > li
    {
        IPhone: background: #F0F0F0;
        background: WhiteSmoke;

    }

    .km-listview
    {
        background:red;
    }

    #tabstrip
    {
        background:blue;
    }

    #Tab_GEN
    {
        background:yellow;
    }

    .km-view
    {
        background:yellow;
    }

    .km-listview li
    {
        background: green;
    }

    .km-ios
    {
        background:orange;
    }
    */

</style>

</head>

<body>

     <div data-role="view" data-layout="overview-layout" id="Tab_GEN" data-title="General">
         <ul data-role="listview" data-style="inset" data-type="group">
             <li>
                 Main
                 <div id="GeneralMainListBox"></div>
            </li>

             <li>
                 Routing
                 <div id="GeneralRoutingListBox"></div>
            </li>
         </ul>

         <div class="BusyIndicator" id="GeneralPageBusyIndicator"></div> 

    </div>

     <div data-role="view" data-layout="overview-layout" id="Tab_PAR" data-title="Partners">
        <ul data-role="listview" data-style="inset" data-type="group">
             <div id="PartnersListBox"></div>
        </ul>

         <div class="BusyIndicator" id="PartnersPageBusyIndicator"></div>
     </div>

     <div data-role="view" data-layout="overview-layout" id="Tab_CAR" data-title="Cargo">
        <ul data-role="listview" data-style="inset" data-type="group">
             <li>
                 Summary
                 <div id="CargoSummaryListBox"></div>
            </li>

             <li>
                 Details
                 <div id="CargoDetailsListBox"></div>
            </li>
        </ul>

         <div class="BusyIndicator" id="CargoPageBusyIndicator"></div>
     </div>

     <div data-role="layout" data-id="overview-layout">
        <header data-role="header">
            <div data-role="navbar">
               <!--a class="nav-button" data-align="left" data-role="backbutton">Back</a-->
                <span data-role="view-title">General</span>
                <%--<a data-align="right" data-role="button" class="nav-button" href="#Tab_GEN">Index</a>--%>
            </div>
        </header>

        <div data-role="footer">
            <div data-role="tabstrip" id="tabstrip">
                <a href="#Tab_GEN" data-icon="GEN">General</a>
                <a href="#Tab_PAR" data-icon="PAR">Partners</a>
                <a href="#Tab_CAR" data-icon="CAR">Cargo Info</a>
            </div>
        </div>
    </div>


    <script type="text/x-kendo-tmpl" id="ListBoxItemTemplate">
        <div>
            <span class="ItemLabel">${Label}</span>
            <span class="ItemValue">${Value}</span>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="MainItemDataTemplate">
        <div>
            <span class="ItemLabel">${Label}</span>
            
            <div style="display: inline-block; float: right;">

                <span style="font-size: 12px; font-family: Arial; vertical-align:central;">${Value}</span>

                <span style="position:relative; margin-right:20px; display: #= IconDisplay #;">
                    <img src="${Image1SRC}" style="width:20px; height:20px; line-height:20px; padding:0; position:absolute;top:0;bottom:0;margin:auto;" />
                </span>

                <span style="position:relative; margin-right:20px; display: #= IconDisplay #;">
                    <img src="${Image2SRC}" style="width:20px; height:20px; line-height:20px; padding:0; position:absolute;top:0;bottom:0;margin:auto;" />
                </span>

            </div>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="RoutingItemDataTemplate">
        <div style="width:100%; height:70px; vertical-align:top;">
            
            <div class="BlueLabel" style="height:20px;">${LegHeader}</div>

            <div style="display:inline-block; width:49%; height:50px;">

                <div style="height:30px; vertical-align:top;">
                    <img style="margin:0px; vertical-align:top; height:28px; width:28px;" src="#= FromFlagSRC #" />
                    <div class="ItemValueStyle" style="margin:0px; vertical-align:top; display:inline-block; margin-top:4px; font-size:15px; ">${FromPortCode}</div>
                </div>

                <div style="height:20px; vertical-align:top;">
                    <span class="ItemValueStyle" style="font-size:11px; font-Weight:normal;">${FromDate}</span>
                    <span class="ItemValueStyle" style="font-size:11px; font-Weight:normal; margin-left:2px;">${FromTime}</span>
                    <span style="margin-left: 2px; visibility: #= FromDateTimeVisibility #;">#= FromDateTimeIsActual ? '<img src="../HtmlHelpers/Images/Icons/Tick.png" style="width: 20px; height: 20px; position:relative;" />' : '<span class="ItemLabel" style="font-Weight:normal; font-size:11px;">(Estimate)</span>' #</span>
                </div>
            </div>

            <div style="display:inline-block; width:49%; height:50;">

                <div style="height:30px; vertical-align:top;">
                    <img style="margin:0px; vertical-align:top; height:28px; width:28px;" src="#= ToFlagSRC #" />
                    <div class="ItemValueStyle" style="margin:0px; vertical-align:top; display:inline-block; margin-top:4px; font-size:15px;">${ToPortCode}</div>
                </div>

                <div style="height:20px; vertical-align:top;">
                    <span class="ItemValueStyle" style="font-size:11px; font-Weight:normal;">${ToDate}</span>
                    <span class="ItemValueStyle" style="font-size:11px; font-Weight:normal; margin-left:2px;">${ToTime}</span>
                    <span style="margin-left: 2px; visibility: #= ToDateTimeVisibility #;">#= ToDateTimeIsActual ? '<img src="../HtmlHelpers/Images/Icons/Tick.png" style="width: 20px; height: 20px; position:relative; " />' : '<span class="ItemLabel" style="font-Weight:normal; font-size:11px;">(Estimate)</span>' #</span>
                </div>
            </div>

        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="PartnerItemDataTemplate">
        <div style="height:75px;">

            <div><span class="BlueLabel">${PartnerType}</span><span class="ItemValue" style="float: right;">${PartnerName}</span></div>
            
            <div style="vertical-align:top;">

                <span style="display: inline-block; vertical-align:top; margin-top:3px; margin-left:5px;">
                    <div class="ItemValueStyle" style="font-size:11px; font-weight:normal;">${Address1}</div>
                    <div class="ItemValueStyle" style="font-size:11px; font-weight:normal;">${Address2}</div> 
                    <div class="ItemValueStyle" style="font-size:11px; font-weight:normal;">${CityZipCode}</div>
                    <div class="ItemValueStyle" style="font-size:11px; font-weight:normal;">${CountryName}</div>
                </span>

                <span style="display: inline-block; vertical-align:top; float:right;">
                    <img src="${FlagSRC}" style="width:45px; height:45px; line-height:45px; margin:0; padding:0;" />
                </span>

            </div>
        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="PackageItemDataTemplate">
        <div style="height:50px; vertical-align:top;">

            <div>
                <span style="display: inline-block; vertical-align:top;">
                    <img src="${ImageSRC}" style="width:26px; height:20px; line-height:20px; margin:0; padding:0;" />
                </span>

                <span class="BlueLabel">${MainTitle}</span>
                <span class="BlueLabel" style="float: right;">${SubTitle}</span>
            </div>

            <div style="vertical-align:top;">
                <span class="ItemLabel">Gross Weight</span>
                <span class="ItemValue">${GrossWeight}</span>
            </div>
            
            <div style="vertical-align:top;">
                <span class="ItemLabel">Volume</span>
                <span class="ItemValue">${Volume}</span>
            </div>

        </div>
    </script>

    <script type="text/x-kendo-tmpl" id="PackageEstimateTemplate">
            <div style="height:20px; vertical-align:top;">

                <span style="display: inline-block; vertical-align:top;">
                    <img src="${ImageSRC}" style="width:26px; height:20px; line-height:20px; margin:0; padding:0;" />
                </span>

                <span class="ItemLabel">${MainTitle}</span>
                <span class="ItemValue">${SubTitle}</span>
            </div>
    </script>

    <script type="text/javascript" src="ShipmentMobileViewModel.js"></script>

</body>
</html>
