<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpgradeScreen.aspx.cs" Inherits="WebFreight.Web.WebPages.UpgradeScreen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   
    <title>System Upgrade</title>

    <link href="../HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css"/>    
    <link href="../HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/kendo.dataviz.min.css" rel="stylesheet" type="text/css" />
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css"/>

    <script src="../HtmlHelpers/JS/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/jquery.dateFormat-1.0.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/Logitude.Tools.js" type="text/javascript"></script>
    
   
</head>

<body>

    <script src="../HtmlHelpers/JS/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/knockout-kendo.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/app.js" type="text/javascript"></script>

       <form id="form1" runat="server">
        <div style="text-align:center;">
 
    <img  id="loginlogo" style="width:410px; height:172px;" /> 
    
    <br />
    <br />
    <br />



    <h1 style=" font-weight: bold;">
    &nbsp;&nbsp;&nbsp;
    Updating System to New Version...
    </h1>
    <h3>
    &nbsp;&nbsp;&nbsp;&nbsp; We are currently performing updates that couldn&#39;t be done with System running.</h3>
    <h3>
    &nbsp;&nbsp;&nbsp;&nbsp; Apologize for the inconvenience ...</h3>
    <h3>
     &nbsp;&nbsp;&nbsp;&nbsp;
     
    </h3>
    </div>

        </form>



        <script type="text/javascript">
            $(document).ready(function () {
                var environment = window.sessionStorage.getItem("Environment");
                if (environment == 'LogBox') {

                    $("#loginlogo").css("display", "none");
                }
                else {

                    var logo = window.sessionStorage.getItem("loginlogo");

                    if (logo) {
                        $("#loginlogo").attr("src", logo);
                        $("#loginlogo").css("width", "410px");
                        $("#loginlogo").css("height", "172px");

                    }
                    else {
                        var myLogoMethodUrl = "../api/authentication?myDummyInteger=" + 0 + "&myDummyString=" + "0";
                        $.ajax({
                            url: myLogoMethodUrl,
                            type: 'GET',
                            contentType: 'application/json',

                            success: function (myLogoCode) {


                                var scr = "../" + GetApplicationLogoSource(myLogoCode);

                                $("#loginlogo").attr("src", scr);

                                // }
                            },
                        });
                    }
                }
        });
    </script>
</body>
</html>
