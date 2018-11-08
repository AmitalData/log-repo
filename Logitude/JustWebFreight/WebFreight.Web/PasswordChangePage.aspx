<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordChangePage.aspx.cs" Inherits="WebFreight.Web.PasswordChangePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

    <link href="HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/kendo.dataviz.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/Kendo.2013.2.918/kendo.common.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/Kendo.2013.2.918/kendo.default.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css" />

    <script src="HtmlHelpers/JS/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/jquery.dateFormat-1.0.js" type="text/javascript"></script>
    <script src="HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js" type="text/javascript"></script>


    <style type="text/css">
        .auto-style1 {
            width: 200px;
        }

        .auto-style2 {
            width: 190px;
        }



        .pwdCheckCase0, .pwdCheckCase1, .pwdCheckCase2, .pwdCheckCase3, .pwdCheckCase4 {
            text-align: center;
            border-right-width: 1px;
            border-bottom-width: 1px;
            border-right-style: solid;
            border-bottom-style: solid;
        }

        .pwdCheckCase0 {
            background-color: #CCCCCC;
            border-right-color: #CCCCCC;
            border-bottom-color: #CCCCCC;
        }

        .pwdCheckCase1 {
            background-color: #FF0000;
            border-right-color: #FF0000;
            border-bottom-color: #FF0000;
        }

        .pwdCheckCase2 {
            background-color: #FFCC00;
            border-right-color: #FFCC00;
            border-bottom-color: #FFCC00;
        }

        .pwdCheckCase3 {
            background-color: #4DFF00;
            border-right-color: #3333FF;
            border-bottom-color: #3333FF;
        }

        .pwdCheckCase4 {
            background-color: #00CC00;
            border-right-color: #336600;
            border-bottom-color: #336600;
        }

        .pwdCheckTable {
            /*border: 3px outset #CCCCCC;*/
            padding: 0px 0px 0px 0px;
            width: 100%;
        }

        .pwdCheckInnerTable {
            font-family: Tahoma,sans-serif;
            font-weight: bold;
            width: 210px;
            border: 0px 0px 0px 0px;
            height: 19px;
            background-color: #FFFFFF;
        }

            .pwdCheckInnerTable span {
                font-size: 70%;
            }

        .pwdCheckLabelCell {
            font-size: 9pt;
            color: #000000;
            font-weight: bold;
            vertical-align: top;
            text-align: right;
            padding: 5px 20px 13px 5px;
        }

        .pwdCheckDataCell {
            width: 200px;
            vertical-align: top;
            padding: 0px 0px 13px 0px;
        }

        #inputPC {
            font-size: 70%;
            width: 210px;
            height: 19px;
            border: 1px solid #999999;
        }

        .buttons {
            cursor: pointer;
            display: inline;
            border: 1px groove #3399CC;
            background-color: #3399CC;
            padding: 0px 2px 0px 2px;
            text-align: center;
            vertical-align: middle;
        }
    </style>

    <style type="text/css">
        body, html {
            padding: 0;
            margin: 0;
            border: 0;
            height: 100%;
        }

        body {
            min-width: 980px;
            background: url("images/LoginScreen/map.png") no-repeat 50% 180px;
        }

        #mapBackground {
            background-image: url(images/LoginScreen/map.png);
            text-align: center;
            background-repeat: no-repeat;
            background-size: 100%;
            width: 907px;
            height: 466px;
            margin-top: 15px;
            background-position: center;
        }



        .auto-style1 {
            background: #dbdbdb; /* Old browsers */
            /* IE9 SVG, needs conditional override of 'filter' to 'none' */
            background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2RiZGJkYiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjI4JSIgc3RvcC1jb2xvcj0iI2YyZjJmMiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjQxJSIgc3RvcC1jb2xvcj0iI2ZmZmZmZiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmZmZmZmYiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
            background: -moz-linear-gradient(top, #dbdbdb 0%, #f2f2f2 28%, #ffffff 41%, #ffffff 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#dbdbdb), color-stop(28%,#f2f2f2), color-stop(41%,#ffffff), color-stop(100%,#ffffff)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, #dbdbdb 0%,#f2f2f2 28%,#ffffff 41%,#ffffff 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, #dbdbdb 0%,#f2f2f2 28%,#ffffff 41%,#ffffff 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, #dbdbdb 0%,#f2f2f2 28%,#ffffff 41%,#ffffff 100%); /* IE10+ */
            background: linear-gradient(to bottom, #dbdbdb 0%,#f2f2f2 28%,#ffffff 41%,#ffffff 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#dbdbdb', endColorstr='#ffffff',GradientType=0 ); /* IE6-8 */
            font-family: tahoma, arial, sans-serif;
            width: 260px;
            height: 25px;
            font-size: 13px;
        }

        .column1 {
            text-align: left;
            width: 300px;
            font-family: "Myriad Pro";
            font-size: 14px;
            color: #4B4A4A;
        }



        .cmdSubmit {
            -moz-box-shadow: inset 0px 1px 0px 0px #caefab;
            -webkit-box-shadow: inset 0px 1px 0px 0px #caefab;
            box-shadow: inset 0px 1px 0px 0px #caefab;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #77d42a), color-stop(1, #5cb811) );
            background: -moz-linear-gradient( center top, #77d42a 5%, #5cb811 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#77d42a', endColorstr='#5cb811');
            background-color: #77d42a;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #268a16;
            display: inline-block;
            color: #ffffff;
            font-family: arial;
            font-size: 15px;
            font-weight: bold;
            padding: 6px 24px;
            text-decoration: none;
            text-shadow: 1px 1px 0px #aade7c;
            width: 100px;
            margin-top: 2px;
        }

            .cmdSubmit:hover {
                background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #5cb811), color-stop(1, #77d42a) );
                background: -moz-linear-gradient( center top, #5cb811 5%, #77d42a 100% );
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#5cb811', endColorstr='#77d42a');
                background-color: #5cb811;
            }

            .cmdSubmit:active {
                position: relative;
                top: 1px;
            }
    </style>
    <script src="HtmlHelpers/JS/Logitude.Tools.js" type="text/javascript"></script>
</head>
<body>

    <script src="HtmlHelpers/JS/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/knockout-kendo.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>

    <form id="form1" runat="server">
        <div id="Container" style="display: none;">

            <table style="width: 100%; margin-top: -24px">
                <thead>
                    <tr style="height: 140px;">
                        <td></td>
                        <td style="width: 1024px; text-align: center; vertical-align: top;">
                            <img id="loginlogo" width="290" height="114" style="margin-top: 50px;" src="images/LoginScreen/header.jpg" />
                        </td>
                        <td></td>
                    </tr>
                </thead>

                <tbody>
                    <tr>
                        <td />
                        <td>
                            <div id="mapBackground">
                                <table>
                                    <tr>

                                        <td></td>

                                        <td style="width: 1140px; text-align: center; vertical-align: top;">
                                            <table style="width: 100%; margin-top: 80px;">
                                                <tr>
                                                    <td>
                                                        <img width="980" height="16" style="opacity: 0.5; margin-bottom: -8px" src="images/LoginScreen/shadow1.png" /></td>
                                                </tr>

                                                <tr>
                                                    <td style="background: white;">
                                                        <table style="width: 100%; height: 226px; border-collapse: collapse; border-spacing: 0;">

                                                            <tr>
                                                                <td style="background: white;">
                                                                    <div style="float: left; margin-left: 60px; width: 300px">


                                                                        <table>




                                                                            <%--   <tr>
                <td style="width:100px" >
                    <p style="text-align:center; margin-top:25px">
                        <img class="RedTxt" src="/images/ser-password-icon.png" style="width:50px;" />
                    </p>
                </td>
             
            </tr>--%>

                                                                            <tr id="CurrentPasswordArea">
                                                                                <td class="column1">Type current Password:
                 <br />
                                                                                    <input class="auto-style1" id="CurrentPassword" type="password" runat="server" required data-email-msg="current password is required!" onkeypress="capLock(event)" />

                                                                                </td>

                                                                            </tr>


                                                                            <tr>
                                                                                <%--  <td class="auto-style2">
                <p style="margin-top:0px">
                    Type new Password:
                </p>
              </td>--%>

                                                                                <td class="column1">Type new Password:
                 
                   <br />
                                                                                    <%-- <asp:TextBox TextMode="Password"  runat="server"  Width="160px" ID="txtNewPassword"></asp:TextBox>--%>
                                                                                    <input class="auto-style1" id="Password" type="password" runat="server" required data-email-msg="password is required!" onchange="onPasswordChanged()" value="" onkeyup="EvalPwdStrength(this.value);" onkeypress="capLock(event)" />

                                                                                </td>




                                                                                <td style="width: 25px">
                                                                                    <img class="RedTxt" src="images/change-password-icon.png" style="width: 25px; height: 22px; vertical-align: bottom; margin-left: 5px; margin-top: 20px" />

                                                                                </td>

                                                                            </tr>


                                                                            <tr>
                                                                                <td class="column1">Re-Type Password:
                 <br />
                                                                                    <input class="auto-style1" id="RetypedPassword" type="password" runat="server" required data-email-msg="password is required!" onkeypress="capLock(event)" onchange="onPasswordChanged2()" />

                                                                                </td>




                                                                            </tr>
                                                                            <tr>
                                                                                <td class="column1">
                                                                                    <div id="divMayus" style="visibility: hidden">
                                                                                        <img width="15" height="15" src="images/SimplogIcons/warning.png" />
                                                                                        Caps Lock is on.
                                                
                                                                                    </div>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <%--<td align="right" nowrap="nowrap" class="pwdCheckLabelCell">Strength:</td>--%>
                                                                                <td class="column1" valign="top" nowrap="nowrap" class="pwdCheckDataCell">
                                                                                    <table cellpadding="0" cellspacing="0" class="pwdCheckInnerTable" style="width: 267px;">
                                                                                        <tr>
                                                                                            <td id="idSM1" width="25%" class="pwdCheckCase0" align="center"><span id="pdw_check_msg1" style="display: none;">Weak</span></td>
                                                                                            <td id="idSM2" width="25%" class="pwdCheckCase0" align="center" style="border-left: solid 1px #FFFFFF"><span id="pdw_check_msg0" style="display: inline; font-weight: normal; color: #666">No rating</span><span id="pdw_check_msg2" style="display: none;">Medium</span></td>
                                                                                            <td id="idSM3" width="25%" class="pwdCheckCase0" align="center" style="border-left: solid 1px #FFFFFF"><span id="pdw_check_msg3" style="display: none;">Strong</span></td>
                                                                                            <td id="idSM4" width="25%" class="pwdCheckCase0" align="center" style="border-left: solid 1px #FFFFFF"><span id="pdw_check_msg4" style="display: none;">BEST</span></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>

                                                                            <tr>
                                                                                <td class="column1">
                                                                                    <p style="color: red; display: none; text-align: left" id="errorsList"></p>
                                                                                    <p style="color: teal; height: 50px" id="txtMessage">
                                                                                        Password must have at least 8 characters. You can combine uppercase letters, lowercase letters, numbers, and symbols.
                                                                                    </p>


                                                                                    <p style="text-align: left">

                                                                                        <input class="cmdSubmit" type="submit" value="Submit >" runat="server" id="cmdSubmit" data-bind="click: submitMethod" />

                                                                                        <a id="BackToLogin" style="margin-left: 15px; margin-top: 50px; color: #4B4A4A; font-size: 12px; font-family: Arial; vertical-align: central; display: none" href="login.aspx">Back to login page</a>

                                                                                        <%-- <a style="margin-left:15px;color:#4B4A4A;font-size:12px;font-family:Arial;vertical-align:central;display:normal" href="login.aspx" >Back to login page</a>--%>


                                                                                        <%--<asp:Button runat="server" Width="75px" id="btnSave"  Text="Submit" onclick="btnSave_Click"/>--%>
                                                                                    </p>






                                                                                    <p id="busyIndicator" style="display: none; text-align: center">
                                                                                        <img width="50" height="50" src="images/LoginScreen/indicator.gif" alt='loading' />
                                                                                    </p>
                                                                                </td>

                                                                            </tr>
                                                                        </table>


                                                                    </div>
                                                                </td>

                                                                <td style="width: 2px; text-align: right; border: 0;">
                                                                    <img src="images/LoginScreen/line.png" style="width: 2px; height: 260px; margin-right: -3px; border: thick" /></td>
                                                                <td style="width: 639px;">
                                                                    <img id="LoginScreen" style="height: 260px; width: 650px; min-width: 650px; border: 0" src="images/LoginScreen/Layer.png" /></td>
                                                                <td style="width: 85px;"></td>
                                                            </tr>

                                                        </table>
                                                    </td>
                                                </tr>

                                                <tr style="float: left; margin-top: -8.5px">
                                                    <td>
                                                        <img width="980" height="16" style="opacity: 0.5" src="images/LoginScreen/shadow2.png" /></td>
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

            <%--     <input id="cmdTenant" runat="server" type="hidden"/>--%>

            <%-- <asp:Label id="lblMsg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat="server" />
            --%>
        </div>
    </form>

    <script type="text/javascript">

         var x =  window.sessionStorage.getItem("PasswordChange");
 
         if (x == "ShowLink") {
             window.sessionStorage.setItem("PasswordChange", "");
             document.getElementById("BackToLogin").style.display = "";
           
         } else document.getElementById("BackToLogin").style.display = "none";
           var url = window.location.href;
           var isDSV = url.toLowerCase().indexOf("dsv") > -1 ? true : false;
           var myDomain = url.split('/')[2];
           if (isDSV == true) {
               window.sessionStorage.setItem("ResetPWD", "true");
               var myLogoMethodUrl = "api/PrivateLable/getisprivatelableurl/?url=" + myDomain;
               $.ajax({
                   url: myLogoMethodUrl,
                   type: 'GET',
                   contentType: 'application/json',

                   success: function (result) {
                       if (result) IsPrivateLabel = result.EnablePrivateLable;
                       if (IsPrivateLabel == true) {
                           window.sessionStorage.setItem("ContactEmail", result.ContactUsEmail);
                           window.sessionStorage.setItem("IsPrivateLabel", IsPrivateLabel);
                           window.sessionStorage.setItem("SmallLogoURL", result.SmallLogoURL);
                           window.sessionStorage.setItem("LogoURL", result.LogoURL);
                           window.sessionStorage.setItem("PrivateLabelUrl", result.PrivateLabelUrl);
                           window.sessionStorage.setItem("PrivateLabelShortName", result.PrivateLabelShortName);

                           //$("#BackToLogin").attr("href", "Login.aspx?tenant=" + BrandingTenant);

                       }
                       document.location.href = "AngularLogin" + "/index.html";
                   },
               });

               //var version = "";
               //if (userdata.HtmlVersion) version = userdata.HtmlVersion;
               //document.location.href = "Angular" + version + "/index.html";
           }
           else {
               var Containerelem = document.getElementById("Container");
               if (Containerelem) {
                   Containerelem.style.display = 'block';
               }
           }
           document.onkeypress = capLock;

           function capLock(e) {
               
               kc = e.keyCode ? e.keyCode : e.which;

               if (kc == 20) {

                   if (document.getElementById('divMayus').style.visibility == 'hidden') {
                       document.getElementById('divMayus').style.visibility = 'visible';
                   }
                   else {
                       document.getElementById('divMayus').style.visibility = 'hidden';
                   }
               }
               else {
                   sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
                   if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
                       document.getElementById('divMayus').style.visibility = 'visible';
                   else
                       document.getElementById('divMayus').style.visibility = 'hidden';
               }
           }
    </script>

    <script type="text/javascript">
        var isResetRequest = false;
        function setCaretToPos(id, cursorPosition) {
            document.getElementById(id).selectionStart = cursorPosition;
            document.getElementById(id).selectionEnd = cursorPosition;
        }
        onPasswordChanged2 = function () {
            $("#errorsList").hide();
        }
        onPasswordChanged = function () {
            $("#errorsList").hide();
            var passtring = $("#Password").val();
            if (passtring) {

                var cursorPosition = document.getElementById("Password").selectionStart;
                $("#Password").val($.trim(passtring));
                setCaretToPos("Password", cursorPosition);

                $("#errorsList").hide();
 
            }
            
        }

        function disableForm(disable) {
            if (disable) {
                $("input").prop('disabled', true);
            }
            else {
                $("input").prop('disabled', false);
            }
        }


        function validate(userEmail) {
            
            var valid = true;
            var newPassword = $("#Password").val();
            var retypedPassword = $("#RetypedPassword").val();
            var errorMessage = "";

            if (!isResetRequest) {
                var currentPassword = $("#CurrentPassword").val();
                if (!currentPassword && newPassword) {
                    errorMessage = "Current Password can't be empty!";
                    valid = false;
                }
            }


            if (valid) {
                errorMessage = PasswordValidation(newPassword,userEmail);
                if (errorMessage) valid = false;
            }


            if (valid) {
                if (newPassword == retypedPassword) {

                    if (newPassword.length < 8) {

                        errorMessage = "Passwords minimum length is 8 characters!";
                        valid = false;
                    }


                    if (newPassword.length > 16) {

                        errorMessage = "Passwords maximum length is 16 characters!";
                        valid = false;
                    }

                    if (!strongPassword && valid) {
                        errorMessage = "Password is not strong enough. Please use at least three of the four characters types possible.";//"The password you entered is invalid";
                        valid = false;
                    }
                }
                else {

                    errorMessage = "Passwords doesnt match!";
                    valid = false;
                }
            }

          

            if (!valid) {
                
                document.getElementById("errorsList").innerHTML = errorMessage;
                $("#errorsList").show();
            }

            return valid;
        }

        function PasswordValidation(password, userEmail) {

  

        //Password Contains User Email
        if (userEmail) {

            var ContainsEmail = false;
            if (password.toLowerCase().indexOf(userEmail.toLowerCase()) > -1) {
                ContainsEmail = true;
            }
            else {

                var emalData = [];
                var userEmailArray = userEmail.split('@');
                emalData.push(userEmailArray[0]);
                emalData.push(userEmailArray[1].split('.')[0]);
                emalData.push(userEmailArray[1].split('.')[1]);

                if (emalData) {
                    emalData.forEach((item) => {
                        if (item) {
                            if (password.toLowerCase().indexOf(item.toLowerCase()) > -1) {
                                ContainsEmail = true;
                            }
                        }

                    });
                }

            }
            if (ContainsEmail) {
                messageError = "Password mustn't contain user email!";
                return messageError

            }
        }


        // contain series(5 letters / numbers)
        if (IsPasswordContainsSeries(password)) {
            messageError = "Password should not contain series (4 letters/numbers)";
            return messageError;

        }

        return "";
           
    
        }

       function IsPasswordContainsSeries(password) {

        var result = false;
        if (password) password = password.toUpperCase();

         var passwordNumnberList = [];
        for (var i = 0; i < password.length; i++) {
            var char = password.charAt(i);
            var x = 0;
            if ('0123456789'.indexOf(char) !== -1) {
                x = Number(char);

            } else {
                x = char.charCodeAt(0);
            }

            passwordNumnberList.push(x);
        }

           result = IsSeries(passwordNumnberList ,"+");
           if(!result)  result = IsSeries(passwordNumnberList ,"-");
           if(!result)  result = IsSeries(passwordNumnberList ,"Same");

        return result;
    }

   
        function IsSeries(passwordNumnberList , operatorCode) {

              var result = false;

            var seriesNumnberCount = 0;
            var seriesNumnberList = [];
            passwordNumnberList.forEach((item) => {
                var IsNotSeriesNumnber = false;
                if (item <= 9 || ((item >= 65 && item <= 90))) {
                    if (seriesNumnberList.length == 0) {
                        seriesNumnberList.push(item);
                    }

                    else {

                        if (operatorCode == "+") {
                            if (seriesNumnberList[seriesNumnberList.length - 1] + 1 == item) {
                                seriesNumnberList.push(item);
                                seriesNumnberCount += 1;
                            } else IsNotSeriesNumnber = true;
                        }

                        else if (operatorCode == "-") {
                            if (seriesNumnberList[seriesNumnberList.length - 1] - 1 == item) {
                                seriesNumnberList.push(item);
                                seriesNumnberCount += 1;
                            } else IsNotSeriesNumnber = true;
                        }

                        else if (operatorCode == "Same") {
                            if (seriesNumnberList[seriesNumnberList.length - 1] == item) {
                                seriesNumnberList.push(item);
                                seriesNumnberCount += 1;
                            } else IsNotSeriesNumnber = true;
                        }
                    }

                } else IsNotSeriesNumnber = true;


                if (seriesNumnberCount == 3) {
                    result = true;
                    return;
                }

                if (IsNotSeriesNumnber) {
                    seriesNumnberCount = 0;
                    seriesNumnberList = [];
                    seriesNumnberList.push(item);
                }

            });


            return result;
        }
        
        function changepassword(email, currentPassword, requestNumber, newPassword, isResetRequest) {

            var url = "api/Authentication/PostChangePassword/?email=" + email;
            function ResetPasswordParameters() {

                //this.Email = email;
                this.OldPassword = currentPassword;
                this.RequestNumber = requestNumber;
                this.NewPassword = newPassword;
                this.IsResetRequest = isResetRequest;

            };

            var param = new ResetPasswordParameters();
            $.ajax({
                url: url,
                type: 'POST',
                data: JSON.stringify(param),
                contentType: 'application/json',

                success: function (result) {


                    $("#busyIndicator").hide();

                    if (result) {
                        document.location.href = "login.aspx";

                    }
                    else {

                        disableForm(false);
                        var errorMessage = "Reset password failed.";

                        if (isResetRequest) {
                            errorMessage = "Reset password failed." + "<br/>Invalid or expired Request number";
                        }

                        document.getElementById("errorsList").innerHTML = errorMessage;

                        $("#errorsList").show();
   
                    }


                },

                error: function (jqXHR, textStatus, errorThrown) {

                    disableForm(false);
                    $("#busyIndicator").hide();
                    alert(jqXHR.responseText);
 
                }
            });
        }

        function viewModel() {

              
            var email = "";
            var tenant = "";
            var ischamplogin = false;
           
            var requestNumber = "";




            var hash = $(location).attr('href');
            if (hash) {
                var arr = hash.split('?');
                if (arr.length > 1) {
                    var dataParam = arr[1].split('&');
                    if (dataParam.length == 1) {
                        var param = dataParam[0].split('=');
                        if (param.length > 1) email = param[1];
                    }
                    else {
                        if (dataParam.length > 0) {
                            var param = dataParam[0].split('=');
                            if (param.length > 1) {
                                email = param[1];
                            }

                            var param2 = dataParam[1].split('=');
                            if (param2.length > 1) {
                                requestNumber = param2[1];
                                isResetRequest = true;
                            }
                            if (dataParam.length >2) {
                                var param3 = dataParam[2].split('=');
                                if (param3.length > 1) {
                                    var ischamp = param3[1].toLowerCase();
                                }
                            }

                            if (dataParam.length > 3) {
                                if (dataParam[3]) {

                                    var param4 = dataParam[3].split('=');

                                    if (param4 && param4.length > 1) {
                                        tenant = param4[1].toLowerCase();
                                    }
                                }
                            }

                        }

                        if (ischamp == "true") {
                            ischamplogin = true;
                        }


                    }

                }
            }


            if (!isResetRequest) {
                document.getElementById("CurrentPassword").style.height = "14px";
                document.getElementById("Password").style.height = "14px";
                document.getElementById("RetypedPassword").style.height = "14px";
                document.getElementById("LoginScreen").style.height = "270px";

            } else {
                document.getElementById("CurrentPasswordArea").style.display = "none";
            }
           

          
  



            window.sessionStorage.setItem("email", email);
            window.sessionStorage.setItem("requestNumber", requestNumber); 


            

            if (!tenant) {
                if (ischamplogin == true) {
                    IsChampLogin = true;
                    $("#loginlogo").attr("src", "images/LoginScreen/champ.png");
                    $("#loginlogo").attr("width", "114");
                    $("#imglink").attr("href", "http://www.cargoserv.com/");

                }
                else {
                    //
                    var myLogoMethodUrl = "api/authentication?myDummyInteger=" + 0 + "&myDummyString=" + "0";
                    $.ajax({
                        url: myLogoMethodUrl,
                        type: 'GET',
                        contentType: 'application/json',

                        success: function (myLogoCode) {
                            if (myLogoCode == "L.O.B") {
                                window.sessionStorage.setItem("LogoCode", myLogoCode);
                                var hash = $(location).attr('href');
                                var domain = hash.split('/')[2];
                                var MethodUrl = "api/PrivateLable/getisprivatelableurl/?url=" + domain;
                                $.ajax({
                                    url: MethodUrl,
                                    type: 'GET',
                                    contentType: 'application/json',

                                    success: function (result) {
                                        if (result) IsPrivateLabel = result.EnablePrivateLable;
                                        if (IsPrivateLabel == true) {
                                            window.sessionStorage.setItem("LogoURL", result.LogoURL);
                                        }
                                        $("#loginlogo").attr("src", GetApplicationLogoSource(myLogoCode));
                                        $("#loginlogo").css("width", GetApplicationLogoWidth(myLogoCode));
                                        $("#loginlogo").css("height", GetApplicationLogoHeight(myLogoCode));

                                    },
                                });
                            }
                            else {
                                $("#loginlogo").attr("src", "images/LoginScreen/header.jpg");
                            }

                        },
                    });
                    
                   
                    //$("#loginlogo").attr("src", "images/LoginScreen/header.jpg");

                }

            }
            else {

                var myLogoMethodUrl = "api/branding/gettenantlogouri/?tenant=" +parseInt(tenant);
                $.ajax({
                    url: myLogoMethodUrl,
                    type: 'GET',
                    contentType: 'application/json',

                    success: function (result) {
                        $("#loginlogo").attr("src", result);
                        $("#loginlogo").css("width", "290px");
                        $("#loginlogo").css("height", "114px");

                    },
                });

            }




            this.submitMethod = function () {

                document.getElementById("errorsList").innerHTML = "";
                $("#errorsList").hide();

                if (!validate(email)) {

                    return;
                }

                disableForm(true);
                $("#busyIndicator").show();
                var currentPassword = $("#CurrentPassword").val();
                var newPassword = $("#Password").val();

                if (email) {
                    if (currentPassword) {

                        var url = "api/PasswordChange/PostCheckPasswordUser";
                        function ChangePasswordParameter() {
                            this.Email = email;
                            this.CurrentPassword = currentPassword;
                        };

                        var param = new ChangePasswordParameter();

                        $.ajax({
                            url: url,
                            type: 'POST',
                            data: JSON.stringify(param),
                            contentType: 'application/json',

                            success: function (result) {
                                var error = "";
                                if (result) {
                                    if (currentPassword == newPassword)   error = "New password can't be the same as the current password";
                                }
                                else error = "The current password is wrong!";
                       
                                if (error) {
                                    disableForm(false);
                                    document.getElementById("errorsList").innerHTML = error;
                                    $("#errorsList").show();
                                    $("#busyIndicator").hide();
                                }
                                else {
                                    changepassword(email, currentPassword, requestNumber, newPassword, isResetRequest);
                                }
                            },

                            error: function (jqXHR, textStatus, errorThrown) {
                                disableForm(false);
                                $("#busyIndicator").hide();
                                alert(jqXHR.responseText);


                            }
                        });

                    }
                    else changepassword(email, currentPassword, requestNumber, newPassword, isResetRequest);
                }
                else {
                    disableForm(false);
                    document.getElementById("errorsList").innerHTML = "Your email is empty.";
                    $("#errorsList").show();
                    $("#busyIndicator").hide();
                }

            }
        }

        $(document).ready(function () {
           
            ko.applyBindings(new viewModel());
        });
    </script>

    <script type="text/javascript">
 
    var kNoCanonicalCounterpart = 0;
    var kCapitalLetter = 0;
    var kSmallLetter = 1;
    var kDigit = 2;
    var kPunctuation = 3;
    var kAlpha =  4;
    var kCanonicalizeLettersOnly = true;
    var kCananicalizeEverything = false;
    var gDebugOutput = null;
    var kDebugTraceLevelNone = 0;
    var kDebugTraceLevelSuperDetail = 120;
    var kDebugTraceLevelRealDetail = 100;
    var kDebugTraceLevelAll = 80;
    var kDebugTraceLevelMost = 60;
    var kDebugTraceLevelFew = 40;
    var kDebugTraceLevelRare = 20;
    var gDebugTraceLevel = kDebugTraceLevelNone;
 
    function DebugPrint (){
        var string = "";
        if (gDebugTraceLevel && gDebugOutput && DebugPrint.arguments && (DebugPrint.arguments.length > 1) && (DebugPrint.arguments [0] <= gDebugTraceLevel)){
            for (var index = 1; index < DebugPrint.arguments.length; index++){
                string += DebugPrint.arguments[index] + " ";
            }
            string += "<br>\n";
            gDebugOutput (string);
        } else {}
    }
 
    function CSimilarityMap (){
        this.m_elements = "";
        this.m_canonicalCounterparts = "";
    }
 
    function SimilarityMap_Add (element, canonicalCounterpart){
        this.m_elements += element;
        this.m_canonicalCounterparts += canonicalCounterpart;
    }
 
    function SimilarityMap_Lookup (element){
        var canonicalCounterpart = kNoCanonicalCounterpart;
        var index = this.m_elements.indexOf (element);
        if (index >= 0){
            canonicalCounterpart = this.m_canonicalCounterparts.charAt (index);
        } else {}
 
        return canonicalCounterpart;
    }
 
    function SimilarityMap_GetCount (){
        return this.m_elements.length;
    }
 
    CSimilarityMap.prototype.Add = SimilarityMap_Add;
    CSimilarityMap.prototype.Lookup = SimilarityMap_Lookup;
    CSimilarityMap.prototype.GetCount = SimilarityMap_GetCount;
 
    function CDictionaryEntry (length, wordList){
        this.m_length = length;
        this.m_wordList = wordList;
    }
 
    function DictionaryEntry_Lookup (strWord){
        var fFound = false;
        if (strWord.length == this.m_length){
            var nFirst = 0;
            var nLast = this.m_wordList.length - 1;
            while (nFirst <= nLast){
                var nCurrent = Math.floor ((nFirst + nLast) / 2);
                if (strWord == this.m_wordList [nCurrent]){
                    fFound = true;
                    break;
                }
                else if (strWord > this.m_wordList [nCurrent]){
                    nLast = nCurrent - 1;
                }
                else {
                    nFirst = nCurrent + 1;
                }
            }
        } else {}
 
        return fFound;
    }
 
    CDictionaryEntry.prototype.Lookup = DictionaryEntry_Lookup;
 
    function CDictionary (){
        this.m_entries = new Array ();
    }
 
    function Dictionary_Lookup (strWord){
        for (var index = 0; index < this.m_entries.length; index++){
            if (this.m_entries [index].Lookup (strWord)){
                return true;
            } else {}
        }
    }
 
    function Dictionary_Add (length, wordList){
        var iL = this.m_entries.length;
        var cD = new CDictionaryEntry (length, wordList);
        this.m_entries [iL] = cD;
    }
 
    CDictionary.prototype.Lookup = Dictionary_Lookup;
    CDictionary.prototype.Add = Dictionary_Add;
    var gSimilarityMap = new CSimilarityMap ();
    var gDictionary = new CDictionary ();
 
    function CharacterSetChecks (type, fResult){
        this.type = type;
        this.fResult = fResult;
    }
 
    function isctype (character, type, nDebugLevel){
        var fResult = false;
        switch (type){
            case kCapitalLetter:
                if ((character >= 'A') && (character <= 'Z')){
                    fResult = true;
                } else {}
                break;
 
            case kSmallLetter:
                if ((character >= 'a') && (character <= 'z')){
                    fResult = true;
                } else {}
                break;
 
            case kDigit:
                if ((character >= '0') && (character <= '9')){
                    fResult = true;
                } else {}
                break;
 
            case kPunctuation:
                if ("!@#$%^&* ()_+-='\";:[{]}\|.>,</?`~".indexOf (character) >= 0){
                    fResult = true;
                } else {}
                break;
 
            case kAlpha:
                if (isctype (character, kCapitalLetter) || isctype (character, kSmallLetter)){
                    fResult = true;
                } else {}
                break;
 
            default:
                alert ("Unexpected Error!");
                break;
        }
 
        return fResult;
    }
 
    function CanonicalizeWord (strWord, similarityMap, fLettersOnly){
        var canonicalCounterpart = kNoCanonicalCounterpart;
        var strCanonicalizedWord = "";
        var nStringLength = 0;
        if ((strWord != null) && (strWord.length > 0)){
            strCanonicalizedWord = strWord;
            strCanonicalizedWord = strCanonicalizedWord.toLowerCase ();
            if (similarityMap.GetCount () > 0){
                nStringLength = strCanonicalizedWord.length;
                for (var index = 0; index < nStringLength; index++){
                    if (fLettersOnly && !isctype (strCanonicalizedWord.charAt (index), kSmallLetter, kDebugTraceLevelSuperDetail)){
                        continue;
                    } else {}
                    canonicalCounterpart = similarityMap.Lookup (strCanonicalizedWord.charAt (index));
                    if (canonicalCounterpart != kNoCanonicalCounterpart){
                        strCanonicalizedWord = strCanonicalizedWord.substring (0, index) + canonicalCounterpart +	strCanonicalizedWord.substring (index + 1, nStringLength);
                    } else {}
                }
            } else {}
        } else {}
 
        return strCanonicalizedWord;
    }
 
    function IsLongEnough (strWord, nAtLeastThisLong){
        if ((strWord == null) || isNaN (nAtLeastThisLong)){
            return false;
        }
        else if (strWord.length < nAtLeastThisLong){
            return false;
        } 
        else {
            return true;
        }
    }
 
    function SpansEnoughCharacterSets (strWord, nAtLeastThisMany){
        var nCharSets = 0;
        var characterSetChecks = new Array (
                new CharacterSetChecks (kCapitalLetter, false),
                new CharacterSetChecks (kSmallLetter, false),
                new CharacterSetChecks (kDigit, false),
                new CharacterSetChecks (kPunctuation, false)
            );
        if ((strWord == null) || isNaN (nAtLeastThisMany)){
            return false;
        } else {}
        for (var index = 0; index < strWord.length; index++){
            for (var nCharSet = 0; nCharSet < characterSetChecks.length;nCharSet++){
                if (!characterSetChecks [nCharSet].fResult && isctype (strWord.charAt (index), characterSetChecks [nCharSet].type, kDebugTraceLevelAll)){
                    characterSetChecks [nCharSet].fResult = true;
                    break;
                } else {}
            }
        }
        for (var nCharSet = 0; nCharSet < characterSetChecks.length; nCharSet++){
            if (characterSetChecks [nCharSet].fResult){
                nCharSets++;
            } else {}	
        }
        if (nCharSets < nAtLeastThisMany){
            return false;
        } else {}
 
        return true;
    }
 
    function FoundInDictionary (strWord, similarityMap, dictionary){
        var strCanonicalizedWord = "";
        if ((strWord == null) || (similarityMap == null) || (dictionary == null)){
            return true;
        } else {}
        strCanonicalizedWord = CanonicalizeWord (strWord, similarityMap, kCanonicalizeLettersOnly);
        if (dictionary.Lookup (strCanonicalizedWord)){
            return true;
        } else {}
 
        return false;
    }
 
    function IsCloseVariationOfAWordInDictionary (strWord, threshold, similarityMap, dictionary){
        var strCanonicalizedWord = "";
        var nMinimumMeaningfulMatchLength = 0;
        if ((strWord == null) || isNaN (threshold) || (similarityMap == null) || (dictionary == null)){
            return true;
        } else {}
        strCanonicalizedWord = CanonicalizeWord (strWord, similarityMap, kCananicalizeEverything);
        nMinimumMeaningfulMatchLength = Math.floor ((threshold) * strCanonicalizedWord.length);
        for (var nSubStringLength = strCanonicalizedWord.length; nSubStringLength >= nMinimumMeaningfulMatchLength; nSubStringLength--){
            for (var nSubStringStart = 0; (nSubStringStart + nMinimumMeaningfulMatchLength) < strCanonicalizedWord.length; nSubStringStart++){
                var strSubWord = strCanonicalizedWord.substr (nSubStringStart, nSubStringLength);
                if (dictionary.Lookup (strSubWord)){
                    return true;
                } else {}
            }
        }
 
        return false;
    }
 
    function Init (){
        gSimilarityMap.Add ('3', 'e');
        gSimilarityMap.Add ('x', 'k');
        gSimilarityMap.Add ('5', 's');
        gSimilarityMap.Add ('$', 's');
        gSimilarityMap.Add ('6', 'g');
        gSimilarityMap.Add ('7', 't');
        gSimilarityMap.Add ('8', 'b');
        gSimilarityMap.Add ('|', 'l');
        gSimilarityMap.Add ('9', 'g');
        gSimilarityMap.Add ('+', 't');
        gSimilarityMap.Add ('@', 'a');
        gSimilarityMap.Add ('0', 'o');
        gSimilarityMap.Add ('1', 'l');
        gSimilarityMap.Add ('2', 'z');
        gSimilarityMap.Add ('!', 'i');
 
        gDictionary.Add (3, "oat|not|ken|keg|ham|hal|gas|cpu|cit|bop|bah".split ("|"));
 
        gDictionary.Add (4, "zeus|ymca|yang|yaco|work|word|wool|will|viva|vito|vita|visa|vent|vain|uucp|util|utah|unix|trek|town|torn|tina|time|tier|tied|tidy|tide|thud|test|tess|tech|tara|tape|tapa|taos|tami|tall|tale|spit|sole|sold|soil|soft|sofa|soap|slav|slat|slap|slam|shit|sean|saud|sash|sara|sand|sail|said|sago|sage|saga|safe|ruth|russ|rusk|rush|ruse|runt|rung|rune|rove|rose|root|rick|rich|rice|reap|ream|rata|rare|ramp|prod|pork|pete|penn|penh|pend|pass|pang|pane|pale|orca|open|olin|olga|oldy|olav|olaf|okra|okay|ohio|oath|numb|null|nude|note|nosy|nose|nita|next|news|ness|nasa|mike|mets|mess|math|mash|mary|mars|mark|mara|mail|maid|mack|lyre|lyra|lyon|lynx|lynn|lucy|love|lose|lori|lois|lock|lisp|lisa|leah|lass|lash|lara|lank|lane|lana|kink|keri|kemp|kelp|keep|keen|kate|karl|june|judy|judo|judd|jody|jill|jean|jane|isis|iowa|inna|holm|help|hast|half|hale|hack|gust|gush|guru|gosh|gory|golf|glee|gina|germ|gatt|gash|gary|game|fred|fowl|ford|flea|flax|flaw|finn|fink|film|fill|file|erin|emit|elmo|easy|done|disk|disc|diet|dial|dawn|dave|data|derek|damn|dame|crab|cozy|coke|city|cite|chem|chat|cats|burl|bred|bill|bilk|bile|bike|beth|beta|benz|beau|bath|bass|bart|bank|bake|bait|bail|aria|anne|anna|andy|alex|abcd".split ("|"));
 
        gDictionary.Add (5, "yacht|xerox|wilma|willy|wendy|wendi|water|warez|vitro|vital|vitae|vista|visor|vicky|venus|venom|value|ultra|u.s.a|tubas|tress|tramp|trait|tracy|traci|toxic|tiger|tidal|thumb|texas|test2|test1|terse|terry|tardy|tappa|tapis|tapir|taper|tanya|tansy|tammy|tamie|taint|sybil|suzie|susie|susan|super|steph|stacy|staci|spark|sonya|sonia|solar|soggy|sofia|smile|slave|slate|slash|slant|slang|simon|shiva|shell|shark|sharc|shack|scrim|screw|scott|scorn|score|scoot|scoop|scold|scoff|saxon|saucy|satan|sasha|sarah|sandy|sable|rural|rupee|runty|runny|runic|runge|rules|ruben|royal|route|rouse|roses|rolex|robyn|robot|robin|ridge|rhode|revel|renee|ranch|rally|radio|quark|quake|quail|power|polly|polis|polio|pluto|plane|pizza|photo|phone|peter|perry|penna|penis|paula|patty|parse|paris|parch|paper|panic|panel|olive|olden|okapi|oasis|oaken|nurse|notre|notch|nancy|nagel|mouse|moose|mogul|modem|merry|megan|mckee|mckay|mcgee|mccoy|marty|marni|mario|maria|marcy|marci|maint|maine|magog|magic|lyric|lyons|lynne|lynch|louis|lorry|loris|lorin|loren|linda|light|lewis|leroy|laura|later|lasso|laser|larry|ladle|kinky|keyes|kerry|kerri|kelly|keith|kazoo|kayla|kathy|karie|karen|julie|julia|joyce|jenny|jenni|japan|janie|janet|james|irene|inane|impel|idaho|horus|horse|honey|honda|holly|hello|heidi|hasty|haste|hamal|halve|haley|hague|hager|hagen|hades|guest|guess|gucci|group|grahm|gouge|gorse|gorky|glean|gleam|glaze|ghoul|ghost|gauss|gauge|gaudy|gator|gases|games|freer|fovea|float|fiona|finny|filly|field|erika|erica|enter|enemy|empty|emily|email|elmer|ellis|ellen|eight|eerie|edwin|edges|eatme|earth|eager|dulce|donor|donna|diane|diana|delay|defoe|david|danny|daisy|cuzco|cubit|cozen|coypu|coyly|cowry|condo|class|cindy|cigar|chess|cathy|carry|carol|carla|caret|caren|candy|candi|burma|burly|burke|brian|breed|borax|booze|booty|bloom|blood|bitch|bilge|bilbo|betty|beryl|becky|beach|bathe|batch|basic|bantu|banks|banjo|baird|baggy|azure|arrow|array|april|anita|angie|amber|amaze|alpha|alisa|alike|align|alice|alias|album|alamo|aires|admin|adept|adele|addle|addis|added|acura|abyss|abcde|1701d|123go|!@#$%".split ("|"));
 
        gDictionary.Add (6, "yankee|yamaha|yakima|y7u8i9|xyzxyz|wombat|wizard|wilson|willie|weenie|warren|visual|virgin|viking|venous|venice|venial|vasant|vagina|ursula|urchin|uranus|uphill|umpire|u.s.a.|tuttle|trisha|trails|tracie|toyota|tomato|toggle|tidbit|thorny|thomas|terror|tennis|taylor|target|tardis|tappet|taoist|tannin|tanner|tanker|tamara|system|surfer|summer|subway|stacie|stacey|spring|sondra|solemn|soleil|solder|solace|soiree|soften|soffit|sodium|sodden|snoopy|snatch|smooch|smiles|slavic|slater|single|singer|simple|sherri|sharon|sharks|sesame|sensor|secret|second|season|search|scroll|scribe|scotty|scooby|schulz|school|scheme|saturn|sandra|sandal|saliva|saigon|sahara|safety|safari|sadism|saddle|sacral|russel|runyon|runway|runoff|runner|ronald|romano|rodent|ripple|riddle|ridden|reveal|return|remote|recess|recent|realty|really|reagan|raster|rascal|random|radish|radial|racoon|racket|racial|rachel|rabbit|qwerty|qawsed|puppet|puneet|public|prince|presto|praise|poster|polite|polish|policy|police|plover|pierre|phrase|photon|philip|persia|peoria|penmen|penman|pencil|peanut|parrot|parent|pardon|papers|pander|pamela|pallet|palace|oxford|outlaw|osiris|orwell|oregon|oracle|olivia|oliver|olefin|office|notion|notify|notice|notate|notary|noreen|nobody|nicole|newton|nevada|mutant|mozart|morley|monica|moguls|minsky|mickey|merlin|memory|mellon|meagan|mcneil|mcleod|mclean|mckeon|mchugh|mcgraw|mcgill|mccann|mccall|mccabe|mayfly|maxine|master|massif|maseru|marvin|markus|malcom|mailer|maiden|magpie|magnum|magnet|maggot|lorenz|lisbon|limpid|leslie|leland|latest|latera|latent|lascar|larkin|langur|landis|landau|lambda|kristy|kristi|krista|knight|kitten|kinney|kerrie|kernel|kermit|kennan|kelvin|kelsey|kelley|keller|keenan|katina|karina|kansas|juggle|judith|jsbach|joshua|joseph|johnny|joanne|joanna|jixian|jimmie|jimbob|jester|jeanne|jasmin|janice|jaguar|jackie|island|invest|instar|ingrid|ingres|impute|holmes|holman|hockey|hidden|hawaii|hasten|harvey|harold|hamlin|hamlet|halite|halide|haggle|haggis|hadron|hadley|hacker|gustav|gusset|gurkha|gurgle|guntis|guitar|gamlyn|gospel|gorton|gorham|gorges|golfer|glassy|ginger|gibson|ghetto|german|george|gauche|gasify|gambol|gamble|gambit|friend|freest|fourth|format|flower|flaxen|flaunt|flakes|finley|finite|fillip|fillet|filler|filled|fermat|fender|fatten|fatima|fathom|father|evelyn|euclid|estate|enzyme|engine|employ|emboss|elanor|elaine|eileen|eighty|eighth|effect|efface|eeyore|eerily|edwina|easier|durkin|durkee|during|durham|duress|duncan|donner|donkey|donate|donald|domino|disney|dieter|device|denise|deluge|delete|debbie|deaden|ddurer|dapper|daniel|dancer|damask|dakota|daemon|cuvier|cuddly|cuddle|cuckoo|cretin|create|cozier|coyote|cowpox|cooper|cookie|connie|coneck|condom|coffee|citrus|citron|citric|circus|charon|change|censor|cement|celtic|cecily|cayuga|catnip|catkin|cation|castle|carson|carrot|carrie|carole|carmen|caress|cantor|burley|burlap|buried|burial|brenda|bremen|breezy|breeze|breech|brandy|brandi|border|borden|borate|bloody|bishop|bilbao|bikini|bigred|betsie|berman|berlin|bedbug|became|beavis|beaver|beauty|beater|batman|bathos|barony|barber|baobab|bantus|banter|bantam|banish|bangui|bangor|bangle|bandit|banana|bakery|bailey|bahama|bagley|badass|aztecs|azsxdc|athena|asylum|arthur|arrest|arrear|arrack|arlene|anvils|answer|angela|andrea|anchor|analog|amazon|amanda|alison|alight|alicia|albino|albert|albeit|albany|alaska|adrian|adelia|adduce|addict|addend|accrue|access|abcdef|abcabc|abc123|a1b2c3|a12345|@#$%^&|7y8u9i|1qw23e|1q2w3e|1p2o3i|1a2b3c|123abc|10sne1|0p9o8i|!@#$%^".split ("|"));
 
        gDictionary.Add (7, "yolanda|wyoming|winston|william|whitney|whiting|whatnot|vitriol|vitrify|vitiate|vitamin|visitor|village|vertigo|vermont|venturi|venture|ventral|venison|valerie|utility|upgrade|unknown|unicorn|unhappy|trivial|torrent|tinfoil|tiffany|tidings|thunder|thistle|theresa|test123|terrify|teleost|tarbell|taproot|tapping|tapioca|tantrum|tantric|tanning|takeoff|swearer|suzanne|susanne|support|success|student|squires|sossina|soldier|sojourn|soignee|sodding|smother|slavish|slavery|slander|shuttle|shivers|shirley|sheldon|shannon|service|seattle|scooter|scissor|science|scholar|scamper|satisfy|sarcasm|salerno|sailing|saguaro|saginaw|sagging|saffron|sabrina|russell|rupture|running|runneth|rosebud|receipt|rebecca|realtor|raleigh|rainbow|quarrel|quality|qualify|pumpkin|protect|program|profile|profess|profane|private|prelude|porsche|politic|playboy|phoenix|persona|persian|perseus|perseid|perplex|penguin|pendant|parapet|panoply|panning|panicle|panicky|pangaea|pandora|palette|pacific|olivier|olduvai|oldster|okinawa|oakwood|nyquist|nursery|numeric|number1|nullify|nucleus|nuclear|notused|nothing|newyork|network|neptune|montana|minimum|michele|michael|merriam|mercury|melissa|mcnulty|mcnally|mcmahon|mckenna|mcguire|mcgrath|mcgowan|mcelroy|mcclure|mcclain|mccarty|mcbride|mcadams|mbabane|mayoral|maurice|marimba|manhole|manager|mammoth|malcolm|malaria|mailbox|magnify|magneto|losable|lorinda|loretta|lorelei|lockout|lioness|limpkin|library|lazarus|lathrop|lateran|lateral|kristin|kristie|kristen|kinsman|kingdom|kennedy|kendall|kellogg|keelson|katrina|jupiter|judaism|judaica|jessica|janeiro|inspire|inspect|insofar|ingress|indiana|include|impetus|imperil|holmium|holmdel|herbert|heather|headmen|headman|harmony|handily|hamburg|halifax|halibut|halfway|haggard|hafnium|hadrian|gustave|gunther|gunshot|gryphon|gosling|goshawk|gorilla|gleason|glacier|ghostly|germane|georgia|geology|gaseous|gascony|gardner|gabriel|freeway|fourier|flowers|florida|fishers|finnish|finland|ferrari|felicia|feather|fatigue|fairway|express|expound|emulate|empress|empower|emitted|emerald|embrace|embower|ellwood|ellison|egghead|durward|durrell|drought|donning|donahue|digital|develop|desiree|default|deborah|damming|cynthia|cyanate|cutworm|cutting|cuddles|cubicle|crystal|coxcomb|cowslip|cowpony|cowpoke|console|conquer|connect|comrade|compton|collins|cluster|claudia|classic|citroen|citrate|citizen|citadel|cistern|christy|chester|charles|charity|celtics|celsius|catlike|cathode|carroll|carrion|careful|carbine|carbide|caraway|caravan|camille|burmese|burgess|bridget|breccia|bradley|bopping|blondie|bilayer|beverly|bernard|bermuda|berlitz|berlioz|beowulf|beloved|because|beatnik|beatles|beatify|bassoon|bartman|baroque|barbara|baptism|banshee|banquet|bannock|banning|bananas|bainite|bailiff|bahrein|bagpipe|baghdad|bagging|bacchus|asshole|arrange|arraign|arragon|arizona|ariadne|annette|animals|anatomy|anatole|amatory|amateur|amadeus|allison|alimony|aliases|algebra|albumin|alberto|alberta|albania|alameda|aladdin|alabama|airport|airpark|airfoil|airflow|airfare|airdrop|adenoma|adenine|address|addison|accrual|acclaim|academy|abcdefg|!@#$%^&".split ("|"));
 
        gDictionary.Add (8, "yosemite|y7u8i9o0|wormwood|woodwind|whistler|whatever|warcraft|vitreous|virginia|veronica|venomous|trombone|transfer|tortoise|tientsin|tideland|ticklish|thailand|testtest|tertiary|terrific|terminal|telegram|tarragon|tapeworm|tapestry|tanzania|tantalus|tantalum|sysadmin|symmetry|sunshine|strangle|startrek|springer|sparrows|somebody|solecism|soldiery|softwood|software|softball|socrates|slatting|slapping|slapdash|slamming|simpsons|serenity|security|schwartz|sanctity|sanctify|samantha|salesman|sailfish|sailboat|sagittal|sagacity|sabotage|rushmore|rosemary|rochelle|robotics|reverend|regional|raindrop|rachelle|qwertyui|qwerasdf|qawsedrf|q1w2e3r4|protozoa|prodding|princess|precious|politics|politico|plymouth|pershing|penitent|penelope|pendulum|patricia|password|passport|paranoia|panorama|panicked|pandemic|pandanus|pakistan|painless|operator|olivetti|oleander|oklahoma|notocord|notebook|notarize|nebraska|napoleon|missouri|michigan|michelle|mesmeric|mercedes|mcmullen|mcmillan|mcknight|mckinney|mckinley|mckesson|mckenzie|mcintyre|mcintosh|mcgregor|mcgovern|mcginnis|mcfadden|mcdowell|mcdonald|mcdaniel|mcconnel|mccauley|mccarthy|mccallum|mayapple|masonite|maryland|marjoram|marinate|marietta|maneuver|mandamus|maledict|maladapt|magnuson|magnolia|magnetic|lyrebird|lymphoma|lorraine|lionking|linoleum|limitate|limerick|laterite|landmass|landmark|landlord|landlady|landhold|landfill|kristine|kirkland|kingston|kimberly|khartoum|keystone|kentucky|keeshond|kathrine|kathleen|jubilant|joystick|jennifer|jacobsen|irishman|interpol|internet|insulate|instinct|instable|insomnia|insolent|insolate|inactive|imperial|iloveyou|illinois|hydrogen|hutchins|homework|hologram|holocene|hibernia|hiawatha|heinlein|hebrides|headlong|headline|headland|hastings|hamilton|halftone|halfback|hagstrom|gunsling|gunpoint|gumption|gorgeous|glaucous|glaucoma|glassine|ginnegan|ghoulish|gertrude|geometry|geometer|garfield|gamesman|gamecock|fungible|function|frighten|freetown|foxglove|fourteen|foursome|forsythe|football|flaxseed|flautist|flatworm|flatware|fidelity|exposure|eternity|enthrone|enthrall|enthalpy|entendre|entangle|engineer|emulsion|emulsify|emporium|employer|employee|employed|emmanuel|elliptic|elephant|einstein|eighteen|duration|donnelly|dominion|dlmhurst|delegate|delaware|december|deadwood|deadlock|deadline|deadhead|danielle|cyanamid|cucumber|cristina|criminal|creosote|creation|cowpunch|couscous|conquest|comrades|computer|comprise|compress|colorado|clusters|citation|charming|cerulean|cenozoic|cemetery|cellular|catskill|cationic|catholic|cathodic|catheter|cascades|carriage|caroline|carolina|carefree|cardinal|burgundy|burglary|bumbling|broadway|breeches|bordello|bordeaux|bilinear|bilabial|bernardo|berliner|berkeley|bedazzle|beaumont|beatrice|beatific|bathrobe|baronial|baritone|bankrupt|banister|bakelite|azsxdcfv|asdfqwer|arkansas|appraise|apposite|anything|angerine|ancestry|ancestor|anatomic|anathema|ambiance|alphabet|albright|albrecht|alberich|albacore|alastair|alacrity|airspace|airplane|airfield|airedale|aircraft|airbrush|airborne|aerobics|adrianna|adelaide|additive|addition|addendum|accouter|academic|academia|abcdefgh|abcd1234|a1b2c3d4|7y8u9i0o|7890yuio|1234qwer|0p9o8i7u|0987poiu|!@#$%^&*".split ("|"));
 
        gDictionary.Add (9, "zimmerman|worldwide|wisconsin|wholesale|vitriolic|ventricle|ventilate|valentine|tidewater|testament|territory|tennessee|telephone|telepathy|teleology|telemetry|telemeter|telegraph|tarantula|tarantara|tangerine|supported|superuser|stuttgart|stratford|stephanie|solemnity|softcover|slaughter|slapstick|signature|sheffield|sarcastic|sanctuary|sagebrush|sagacious|runnymede|rochester|receptive|reception|racketeer|professor|princeton|pondering|politburo|policemen|policeman|persimmon|persevere|persecute|percolate|peninsula|penetrate|pendulous|paralytic|panoramic|panicking|panhandle|oligopoly|oligocene|oligarchy|olfactory|oldenburg|nutrition|nurturant|notorious|notoriety|minnesota|microsoft|mcpherson|mcfarland|mcdougall|mcdonnell|mcdermott|mccracken|mccormick|mcconnell|mccluskey|mcclellan|marijuana|malicious|magnitude|magnetron|magnetite|macintosh|lynchburg|louisiana|lissajous|limousine|limnology|landscape|landowner|kinshasha|kingsbury|kibbutzim|kennecott|jamestown|ironstone|invisible|invention|intuitive|intervene|intersect|inspector|insomniac|insolvent|insoluble|impetuous|imperious|imperfect|holocaust|hollywood|hollyhock|headphone|headlight|headdress|headcount|headboard|happening|hamburger|halverson|gustafson|gunpowder|glasswort|glassware|ghostlike|geometric|gaucherie|freewheel|freethink|freestone|foresight|foolproof|extension|expositor|establish|entertain|employing|emittance|ellsworth|elizabeth|eightieth|eightfold|eiderdown|dusenbury|dusenberg|donaldson|dominique|discovery|desperate|delegable|delectate|decompose|decompile|damnation|cutthroat|crabapple|cornelius|conqueror|connubial|commrades|citizenry|christine|christina|chemistry|cellulose|celluloid|catherine|carryover|burlesque|bloodshot|bloodshed|bloodroot|bloodline|bloodbath|bilingual|bilateral|bijective|bijection|bernadine|berkshire|beethoven|beatitude|bakhtiari|asymptote|asymmetry|apprehend|appraisal|apportion|ancestral|anatomist|alexander|albatross|alabaster|alabamian|adenosine|abcabcabc".split ("|"));
 
        gDictionary.Add (10, "washington|volkswagen|topography|tessellate|temptation|telephonic|telepathic|telemetric|telegraphy|tantamount|superstage|slanderous|salamander|qwertyuiop|polynomial|politician|phrasemake|photometry|photolytic|photolysis|photogenic|phosphorus|phosphoric|persiflage|persephone|perquisite|peninsular|penicillin|penetrable|panjandrum|oligoclase|oligarchic|oldsmobile|nottingham|noticeable|noteworthy|mcnaughton|mclaughlin|mccullough|mcallister|malconduct|maidenhair|limitation|lascivious|landowning|landlubber|landlocked|lamination|khrushchev|juggernaut|irrational|invariable|insouciant|insolvable|incomplete|impervious|impersonal|headmaster|glaswegian|geopolitic|geophysics|fourteenth|foursquare|expressive|expression|expository|exposition|enterprise|eightyfold|eighteenth|effaceable|donnybrook|delectable|decolonize|cuttlefish|cuttlebone|compromise|compressor|comprehend|cellophane|carruthers|california|burlington|burgundian|borderline|borderland|bloodstone|bloodstain|bloodhound|bijouterie|biharmonic|bernardino|beaujolais|basketball|bankruptcy|bangladesh|atmosphere|asymptotic|asymmetric|appreciate|apposition|ambassador|amateurish|alimentary|additional|accomplish|1q2w3e4r5t".split ("|"));
 
        gDictionary.Add (11, "yellowstone|venturesome|territorial|telekinesis|sagittarius|safekeeping|politicking|policewoman|photometric|photography|phosphorous|perseverant|persecutory|persecution|penitential|pandemonium|mississippi|marketplace|magnificent|irremovable|interrogate|institution|inspiration|incompetent|impertinent|impersonate|impermeable|headquarter|hamiltonian|halfhearted|hagiography|geophysical|expressible|emptyhanded|eigenvector|deleterious|decollimate|decolletage|connecticut|comptroller|compressive|compression|catholicism|bloodstream|bakersfield|arrangeable|appreciable|anastomotic|albuquerque".split ("|"));
 
        gDictionary.Add (12, "williamsburg|testamentary|qwerasdfzxcv|q1w2e3r4t5y6|perseverance|pennsylvania|penitentiary|malformation|liquefaction|interstitial|inconclusive|incomputable|incompletion|incompatible|incomparable|imperishable|impenetrable|headquarters|geometrician|ellipsometry|decomposable|decommission|compressible|burglarproof|bloodletting|bilharziasis|asynchronous|asymptomatic|ambidextrous|1q2w3e4r5t6y".split ("|"));
 
        gDictionary.Add (13, "ventriloquist|ventriloquism|poliomyelitis|phosphorylate|oleomargarine|massachusetts|jitterbugging|interpolatory|inconceivable|imperturbable|impermissible|decomposition|comprehensive|comprehension".split ("|"));
 
        gDictionary.Add (14, "slaughterhouse|irreproducible|incompressible|comprehensible|bremsstrahlung".split ("|"));
 
        gDictionary.Add (15, "irreconciliable|instrumentation|incomprehension".split ("|"));
 
        gDictionary.Add (16, "incomprehensible".split ("|"));
    }
 
    function ClientSideStrongPassword (){
        return (IsLongEnough (ClientSideStrongPassword.arguments [0], "8") && SpansEnoughCharacterSets (ClientSideStrongPassword.arguments [0], "3") && (! (IsCloseVariationOfAWordInDictionary (ClientSideStrongPassword.arguments [0], "0.6", ClientSideStrongPassword.arguments [1], ClientSideStrongPassword.arguments [2]))));
    }
 
    function ClientSideBestPassword (){
        return (IsLongEnough (ClientSideBestPassword.arguments [0], "14") && SpansEnoughCharacterSets (ClientSideBestPassword.arguments [0], "3") && (! (IsCloseVariationOfAWordInDictionary (ClientSideBestPassword.arguments [0], "0.6", ClientSideBestPassword.arguments [1], ClientSideBestPassword.arguments [2]))));
    }
 
    function ClientSideMediumPassword (){
        return (IsLongEnough (ClientSideMediumPassword.arguments [0], "8") && SpansEnoughCharacterSets (ClientSideMediumPassword.arguments [0], "2") && (! (FoundInDictionary (ClientSideMediumPassword.arguments [0], ClientSideMediumPassword.arguments [1], ClientSideMediumPassword.arguments [2]))));
    }
 
    function ClientSideWeakPassword (){
        return (IsLongEnough (ClientSideWeakPassword.arguments [0], "1") || (! (IsLongEnough (ClientSideWeakPassword.arguments [0], "0"))));
    }
 
    function GEId (sID){
        return document.getElementById (sID);
    }
    var strongPassword = false;
    function EvalPwdStrength(sP) {

        strongPassword = false;
        if (ClientSideBestPassword (sP,gSimilarityMap,gDictionary)){
            DispPwdStrength(4, 'pwdCheckCase4');
            strongPassword = true;
        }
        else if (ClientSideStrongPassword (sP,gSimilarityMap,gDictionary)){
            DispPwdStrength(3, 'pwdCheckCase3');
            strongPassword = true;
        }
        else if (ClientSideMediumPassword (sP,gSimilarityMap,gDictionary)){
            DispPwdStrength (2,'pwdCheckCase2');
        }
        else if (ClientSideWeakPassword (sP,gSimilarityMap,gDictionary)){
            DispPwdStrength (1,'pwdCheckCase1');
        }
        else{
            DispPwdStrength (0,'pwdCheckCase0');
        }
    }
 
    function DispPwdStrength (iN,sHL){ 
        if (iN > 4){
            iN = 4;
        } else {}
        for (var i = 0; i < 5; i++){ 
            var sHCR = "pwdCheckCase0";
            if (i <= iN){ 
                sHCR = sHL;
            } else {}
            if (i > 0){ 
                GEId ("idSM" + i).className = sHCR;
            } else {}
            GEId ("pdw_check_msg" + i).style.display = ((i == iN) ? "inline" : "none");
        }
    }
 
    function ToggleObject (object, mode){
        for (var ol = 0; ol < object.length; ++ol){
            theObject = document.getElementById (object [ol]);
            theObject.style.visibility = mode;
        }
    }
    
    </script>

</body>
</html>
