<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AmitalDefault.aspx.cs" Inherits="WebFreight.Web.AmitalDefault" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=11">
<link id="logolink" rel="shortcut icon" />

    <title>Logitude</title>

        
    <style type="text/css">
    
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
    }

      body, html
        {
            padding:0;
            margin:0;
            border:0;
            height:100%;
        }
        
        body
        {
            min-width: 980px;
            background:url("images/LoginScreen/map.png") no-repeat 50% 180px;
        }

        #mapBackground
        {
            background-image: url(images/LoginScreen/map.png);            
            text-align:center;
            background-repeat: no-repeat;            
            background-size: 100%;
            width: 907px;
            height: 466px;
            margin-top: 15px;
            background-position:center; 
        }
    </style>

   
    <!--  AmitalBrowserWpfApplication\Views\GatewayUserControl.cs GOOD (Itzik )-->        	
    
    <script src="Scripts/json2.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/kendo.all.min.js" type="text/javascript"></script>
    
    <script type="text/javascript" src="HtmlHelpers/JS/jquery.dateFormat-1.0.js"></script>
    <!--  AmitalBrowserWpfApplication\Views\GatewayUserControl.cs Bad (Itzik )-->        	
<!--

    <script type="text/javascript" src="HtmlHelpers/JS/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js"></script>
-->
        
    <script type="text/javascript" src="HtmlHelpers/JS/Logitude.Converters.js"></script>
    <script type="text/javascript" src="HtmlHelpers/JS/Logitude.Entites.js"></script>
    <script type="text/javascript" src="HtmlHelpers/JS/ContactActivityLog.js"></script>
    <% if (Simplog.Server.Infrastructure.LogitudeSettings.WorkEnvironment == "customs") {%>
        <script type="text/javascript" src="HtmlHelpers/JS/Amital.GatewayControl.js"></script>
    <%  }%>

    <script type="text/javascript" src="Silverlight.js"></script>
  
    <script type="text/javascript">

        function getURLParameter(name) {
            return decodeURI(
                (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
            );
        }

        $(document).ready(function () {

            var ischamp = getURLParameter("ischamplogin");
            if (ischamp == "true") {

                $("#logolink").attr("href", "logos/champ.png");



            }
            else {
                $("#logolink").attr("href", "logos/logitudeMap3.ico");

            }


        });
    </script>
    <script type="text/javascript">


        function signOut(email) {

            var url = "api/Authentication/?userEmail=" + email;

            $.ajax({
                url: url,
                type: 'GET',
                contentType: 'application/json',

                success: function (result) {

                    document.location.href = "Login.aspx";

                },

                error: function (jqXHR, textStatus, errorThrown) {


                    $("#error").text("errror");
                    $("#error").show();

                    var errorMessage = '';
                }
            });

        }

        var needToConfirm = true;

        function setconfirmFlag() {
            needToConfirm = true; //Call this function if some changes is made to the web page and requires an alert
            // Of-course you could call this is Keypress event of a text box or so...
        }

        function releaseConfirmFlag() {

            needToConfirm = false; //Call this function if dosent requires an alert.
            //this could be called when save button is clicked 
        }


        window.onbeforeunload = confirmExit;
        function confirmExit() {
            if (needToConfirm)
                return "You might have unsaved data.  If you continue, your work will not be saved."
        }



        //        window.onbeforeunload = function () {
        //            return "You might have unsaved data.  If you continue, your work will not be saved."
        //        }

        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
</head>
<body>
   
   <form id="form1" runat="server" style="height:100%">
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <param id="xapSource" runat="server" name="source" value="ClientBin/Simplog.Infrastructure.xap" />
        <%--  XapFileDownloadHandler.ashx?xapname=Simplog.Infrastructure.xap&xapdate--%>
       <%--   <param name="splashscreensource" value="SplashScreen.xaml"/>
          <param name="onSourceDownloadProgressChanged" value="onSourceDownloadProgressChanged" />
  --%>
         
          <%--<param name="initparams" id="initParams" runat="server" value=""/>--%>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="5.1.10411.0" />
		  <param name="autoUpgrade" value="true" />
           
            <!-- itzik 2013.09.09  /-->  
           <param name="onLoad" value="Amital_PluginLoaded" />
            <!-- itzik 2013.09.09  /-->  

          <param runat="server" name="p" id="p" />
		<%--  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.1.20913.0" style="text-decoration:none" onmousedown="releaseConfirmFlag()">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>--%>

                <table style="width:100%;margin-top:-24px">
            <thead>
            <tr style="height:140px;">
                <td></td>
                <td style="width:1024px; text-align:center; vertical-align:top;">                   
                     <img id="loginlogo" width="290" height="114" style="margin-top:50px;" src="images/LoginScreen/header.jpg" />                        
                </td>
                <td></td>
            </tr>
            </thead>

            <tbody>
                <tr>
                    <td />
                    <td>
                 <div  id="mapBackground"> 
                     <table> 
            <tr>
              
                    <td></td>  
                
                    <td style="width:100%; text-align:center; vertical-align:top;">
                        <table style="width:100%; margin-top:80px;">
                          <tr>
                              <td>
                                  <div style="float: left;margin-left:60px;width:100%; margin-top: 0px;font-weight:normal;font-size:16px;" id="myform">
                                      
                                       
                                       <p style="">Logitude requires Microsoft Silverlight plugin to run. <br />Click on the image below to install the latest version of Microsoft Silverlight plugin.<br /><a  href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.1.20913.0">Install now</a></p>




                                         <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=5.1.20913.0" style="text-decoration:none" onmousedown="releaseConfirmFlag()">
 			                                              <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="height:260px;width:300px;min-width:300px;border:0;margin-top:-30px"/>

                                                         </a>

                                  </div>
									   
                                   
                                
                                      
									    
                              </td>
                          </tr>
 
                        </table>    
                </td>

                    <td></td>  
                
                    
            </tr>
                         </table>
                     </div>
                        </td>
                    </tr>
            </tbody>
        </table>
	    </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe></div>
    </form>
</body>
</html>
