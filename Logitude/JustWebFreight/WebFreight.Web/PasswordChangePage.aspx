<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordChangePage.aspx.cs" Inherits="WebFreight.Web.PasswordChangePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

    <link href="css/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="css/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <script src="js/jquery-3.5.1.min.js" type="text/javascript"></script>
    <script src="js/kendo.all.min.js" type="text/javascript"></script>
    <script src="js/knockout-3.5.1.js" type="text/javascript"></script>
    <script src="js/knockout-kendo.min.js" type="text/javascript"></script>

    <link href="HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css" />
    <link href="HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css" />
    <script src="HtmlHelpers/JS/LogitudeTools.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>

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
            font-family: "Arial";
            font-size: 13px;
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
</head>

<body>
    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>

    <form id="form1" runat="server">
        <div id="Container" style="display: none;">

            <table style="width: 100%; margin-top: -24px">
                <thead>
                    <tr style="height: 140px;">
                        <td></td>
                        <td style="width: 1024px; text-align: center; vertical-align: top;">
                            <img id="loginlogo" width="290" height="114" style="margin-top: 50px;" [src]="LoginLogo" />
                        </td>
                        <td></td>
                    </tr>
                </thead>

                <tbody>
                    <tr>
                        <td></td>
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

                                                                            <tr  id="CurrentPasswordArea" style="height:20px;">
                                                                                 <td class="column1">Current password:
                                                                                    <input class="auto-style1" style="height:20px" id="CurrentPassword" type="password" runat="server" required data-email-msg="current password is required!"  onchange="onCurrentPasswordChanged()" onkeypress="capLock(event)" />
                                                                              <br />
                                                                                     </td>
                                                                            </tr>

                                                                   
                                                                                 <tr style="height:20px">

                                                                                     <td>
                                                                                         <table>
                                                                                             <tr>
                                                                                                    <td class="column1">Password:
                                                                                     <br />
                                                                                    <input class="auto-style1" style="height:20px" id="Password" type="password" runat="server" required data-email-msg="password is required!" onchange="onPasswordChanged()" value="" onkeyup="Passwordkeyup(this.value);" onkeypress="capLock(event)"  />
                                                                                
                                                                                 </td>
                                                                                    <td style="width: 70px">
                                                                                    <img class="RedTxt" src="images/ChangePassword.png" style="width: 16px; height: 16px; vertical-align: bottom; margin-left: -10px; margin-top: 20px" />
                                                                        </td>
                                                                                             </tr>
                                                                                         </table>
                                                                                     </td>


                                                                              


                                                                            </tr>

                                                                       


                                                                                 <tr style="height:20px">
                                                                                 <td class="column1">Confirm password:
                                                                                  <input class="auto-style1" style="height:20px" id="ConfirmPassword" type="password" runat="server" required data-email-msg="password is required!" onkeypress="capLock(event)" onchange="onConfirmPasswordChanged()" />
                                                                                </td>
                                                                            </tr>
                                                                      

                                                                
                                                                           <tr style="height:10px">
                                                                                <td class="column1">
                                                                                    <div id="divMayus" style="visibility: hidden">
                                                                                        <img width="15" height="15" src="images/SimplogIcons/warning.png" />
                                                                                        Caps Lock is on.
                                                
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        
                                                                            <tr style="height:60px;">
                                                                                <td>
                                                                                    <table>
                                                                                        <tr>
                                                                                        <td style="vertical-align:top;">
                                                                                
                                                                                <div id="PasswordMustHaveArea" style="vertical-align:top;text-align:left;margin-top:-15px">
                                                                                    <div style="font-size:13px;font-family:'Arial'">Your Password must have : </div>
                                                                          <div style="height:20px">
                                                                                         <table>
                                                                                             <tr>           
                                                                                                 <td style="width:16px;">  <img id="PasswordLenghtImg" width="16" src="images/verified.png" alt='loading' /></td>
                                                                                                 <td><div id="PasswordLenghtDiv" style="font-size:13px;color:gray;margin-left:5px;margin-top:-5px;vertical-align:central;font-family:'Arial'">8 or more characters</div></td>
                                                                                             </tr>
                                                                                         </table>
                                                                                     </div>

                                                                                      <div style="height:20px">
                                                                                         <table>
                                                                                             <tr>
                                                                                              <td style="width:16px;">  <img id="PasswordContainsCharactersImg" width="16" src="images/verified.png" alt='loading' /></td>
                                                                                                 <td><div id="PasswordContainsCharactersDiv" style="font-size:13px;color:gray;margin-left:5px;margin-top:-5px;vertical-align:central;font-family:'Arial'">Upper & lowercase letters</div></td>
                                                                                             </tr>
                                                                                         </table>
                                                                                     </div>


                                                                                    <div style="height:20px">
                                                                                         <table>
                                                                                             <tr>
                                                                                 <td style="width:16px;">  <img id="PasswordContainsNumberImg" width="16" src="images/verified.png" alt='loading' /></td>
                                                                                                 <td><div id="PasswordContainsNumberDiv" style="font-size:13px;color:gray;margin-left:5px;margin-top:-5px;vertical-align:central;font-family:'Arial'">At least one number</div></td>
                                                                                             </tr>
                                                                                         </table>
                                                                                     </div>

                                                                               </div>

                                                                               

                                                                           
                                                                                </td>

                                                                                            <td style="width:90px">
                                                                                    <p id="busyIndicator" style="display: none; text-align: left;margin-top:10px;">
                                                                                        <img width="40" height="40" src="images/LoginScreen/indicator.gif" alt='loading' />
                                                                                    </p>

                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>


                                                                            


                                                                            </tr>



                                                                            <tr style="min-height:15px">
                                                                                <td style="min-height:15px"><p  style="color: red; text-align: left;height:auto;display:block" id="errorsList"></p></td>
                                                                            </tr>

                                                                      <tr style="height:20px">
                                                                                <td class="column1">
                                                                                   
                                                                                    <p style="text-align: left">

                                                                                        <input class="cmdSubmit" type="submit" value="Submit >" runat="server" id="cmdSubmit" data-bind="click: submitMethod" />
                    
                                                                                        <a id="BackToLogin" style=" font-size: 12px;margin-top:5px;margin-left:15px; font-family: Arial; vertical-align: central; display: none;text-decoration:underline" href="login.aspx">Back to login page</a>
                                                                                    </p>

                                                                                </td>

                                                                            </tr>


                                                                        </table>


                                                                    </div>
                                                                </td>

                                                                <td style="width: 2px; text-align: right; border: 0;">
                                                                    <img src="images/LoginScreen/line.png" style="width: 2px; height: 240px; margin-right: -3px; border: thick" /></td>
                                                                <td style="width: 639px;">
                                                                    <img id="LoginScreen" style="height: 240px; width: 650px; min-width: 650px; border: 0" src="images/LoginScreen/Layer.png" /></td>
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
         var LoginLogo = "images/LoginScreen/header.jpg"; 
         var IsLogitude = window.location.href.indexOf("logitudeworld") > -1

        if (IsLogitude == false) {
            $("#loginlogo").attr("src", ""); 
        } else 
            $("#loginlogo").attr("src", LoginLogo);

         if (x == "ShowLink") {
             window.sessionStorage.setItem("PasswordChange", "");
             document.getElementById("BackToLogin").style.display = "";
           
         } else document.getElementById("BackToLogin").style.display = "none";
        var url = window.location.href;
        


        //var isDSV = url.toLowerCase().indexOf("system.dsv.co.il") > -1 ? true : false;
        var myDomain = url.split('/')[2].split(':')[0];
           
               var myLogoMethodUrl = "api/PrivateLable/getisprivatelableurl/?url=" + myDomain;
               $.ajax({
                   url: myLogoMethodUrl,
                   type: 'GET',
                   contentType: 'application/json',

                   success: function (result) {
                       if (result) IsPrivateLabel = result.EnablePrivateLable;
                       if (IsPrivateLabel == true) {
                           window.sessionStorage.setItem("ResetPWD", "true");
                           window.sessionStorage.setItem("ContactEmail", result.ContactUsEmail);
                           window.sessionStorage.setItem("IsPrivateLabel", IsPrivateLabel);
                           window.sessionStorage.setItem("SmallLogoURL", result.SmallLogoURL);
                           window.sessionStorage.setItem("LogoURL", result.LogoURL);
                           window.sessionStorage.setItem("PrivateLabelUrl", result.PrivateLabelUrl);
                           window.sessionStorage.setItem("PrivateLabelShortName", result.PrivateLabelShortName);
                           window.sessionStorage.setItem("IsDSV", result.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1);

                           //$("#BackToLogin").attr("href", "Login.aspx?tenant=" + BrandingTenant);
                           document.location.href = "AngularLogin" + "/index.html";
                       }
                       else {
                           var Containerelem = document.getElementById("Container");
                           if (Containerelem) {
                               Containerelem.style.display = 'block';
                           }
                       }
                   },
               });

               //var version = "";
               //if (userdata.HtmlVersion) version = userdata.HtmlVersion;
               //document.location.href = "Angular" + version + "/index.html";
           
           
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


               if (document.getElementById('divMayus').style.visibility == 'visible') {
                   document.getElementById('PasswordMustHaveArea').style.marginTop = "0px";
               } else {
                       document.getElementById('PasswordMustHaveArea').style.marginTop = "-17px";
               }

           }
    </script>

    <script type="text/javascript">


        var isResetRequest = false;
        var UserEmailText;
        var IsWarringErrorShow = false;


        function setCaretToPos(id, cursorPosition) {
            document.getElementById(id).selectionStart = cursorPosition;
            document.getElementById(id).selectionEnd = cursorPosition;
        }



            onCurrentPasswordChanged = function () {
             if (!IsWarringErrorShow) {
                $("#errorsList").hide();
            }
            var passtring = $("#CurrentPassword").val();
            if (passtring) {
                var cursorPosition = document.getElementById("CurrentPassword").selectionStart;
                $("#CurrentPassword").val($.trim(passtring));
                setCaretToPos("CurrentPassword", cursorPosition);
 

            }
        }







        onConfirmPasswordChanged = function () {
            if (!IsWarringErrorShow) {
                $("#errorsList").hide();
            }
            var passtring = $("#ConfirmPassword").val();
            if (passtring) {
                var cursorPosition = document.getElementById("ConfirmPassword").selectionStart;
                $("#ConfirmPassword").val($.trim(passtring));
                setCaretToPos("ConfirmPassword", cursorPosition);
 

            }
        }




        onPasswordChanged = function () {
            if (!IsWarringErrorShow) {
                  $("#errorsList").hide();
            }
         
            var passtring = $("#Password").val();
            if (passtring) {
                var cursorPosition = document.getElementById("Password").selectionStart;
                $("#Password").val($.trim(passtring));
                setCaretToPos("Password", cursorPosition);


            }

        }


        Passwordkeyup = function (passtring) {
            $("#errorsList").hide();

            document.getElementById("PasswordLenghtDiv").style.color = "gray";
            document.getElementById("PasswordContainsCharactersDiv").style.color = "gray";
            document.getElementById("PasswordContainsNumberDiv").style.color = "gray";

            document.getElementById("PasswordLenghtImg").src = "images/verified.png";
            document.getElementById("PasswordContainsCharactersImg").src = "images/verified.png";
            document.getElementById("PasswordContainsNumberImg").src = "images/verified.png";



            if (passtring) {

                if (passtring.length >= 8) {
                    document.getElementById("PasswordLenghtDiv").style.color = "green";
                    document.getElementById("PasswordLenghtImg").src = "images/verifiedGreen.png";
                }

                if (IsContainsLowerUpperCase(passtring)) {
                    document.getElementById("PasswordContainsCharactersDiv").style.color = "green";
                    document.getElementById("PasswordContainsCharactersImg").src = "images/verifiedGreen.png";
                }

                if (IsContainsNumber(passtring)) {
                    document.getElementById("PasswordContainsNumberDiv").style.color = "green";
                    document.getElementById("PasswordContainsNumberImg").src = "images/verifiedGreen.png";
                }

         

                var errorMessage = PasswordValidation(passtring, UserEmailText);

                     document.getElementById("errorsList").innerHTML = errorMessage;
                    if (errorMessage) {
                        $("#errorsList").show();
                        IsWarringErrorShow = true;
                    }
                    else  $("#errorsList").hide();
                 
                
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
            var confirmPassword = $("#ConfirmPassword").val();
            var errorMessage = "";
            
            if (valid) {
                if (newPassword == confirmPassword) {
                    if (newPassword) {
                        errorMessage = PasswordValidation(newPassword, userEmail);
                        if (errorMessage) valid = false;
                        else if (!IsContainsLowerUpperCase(newPassword)) {
                            errorMessage = "Your password must include an uppercase and lowercase letter.";
                            valid = false;
                        } else if (!IsContainsNumber(newPassword)) {
                            errorMessage = "Your password must include a number.";
                            valid = false;
                        }
                     
                        else if (newPassword.length < 8) {
                            errorMessage = "Your password must be at least 8 characters.";
                            valid = false;
                        }
                        //else if (newPassword.length > 16) {
                        //    errorMessage = "Passwords maximum length is 16 characters!";
                        //    valid = false;
                        //}
                    }
                    else {

                            errorMessage = "Confirm your password.";
                        valid = false;
                    }
                }
                else {

                    errorMessage = "The passwords you entered do not match.";
                    valid = false;
                }
            }

            if (valid) {
                if (!isResetRequest) {
                    var currentPassword = $("#CurrentPassword").val();
                    if (!currentPassword) {
                        errorMessage = "Current Password can't be empty!";
                        valid = false;
                    } else if (currentPassword == newPassword) {
                        errorMessage = "New password can't be the same as the current password";
                        valid = false;
                    }
                }
            }

            if (!valid) {
                
                document.getElementById("errorsList").innerHTML = errorMessage;
                $("#errorsList").show();
            }

            return valid;
        }


        function IsContainsLowerUpperCase(str) {
            return str.match(/[a-z]/) && str.match(/[A-Z]/);
        }

        function IsContainsSymbol(str) {
            return str.match(/[|\\/~^:,;?!&%$@*+]/);
        }

 



        function isLowerCase(str) {
            return str == str.toLowerCase() && str != str.toUpperCase();
        }


             function isUpperCase(str) {
            return str == str.toUpperCase() && str != str.toLowerCase();
        }


        function IsContainsNumber(str) {
            var regex = /\d/g;
            return regex.test(str);
        }

        
        


        function PasswordValidation(password, userEmail) {

  
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
                    emalData.forEach(function(item) {
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


         var seriesError = IsPasswordContainsSeries(password);
            if (seriesError) {
                messageError = seriesError;
                return messageError;
            }

        return "";
           
    
        }

       function IsPasswordContainsSeries(password) {
        var error = "";
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
           if (!result) result = IsSeries(passwordNumnberList, "-");

           if (result) error = "Password should not contain more than 3 following characters";

           if (!result) {
               result = IsSeries(passwordNumnberList, "Same");
               if (result) error = "Password should not contain more than 3 consecutive repeating characters";
           }

      
        return error;
    }

   
        function IsSeries(passwordNumnberList , operatorCode) {

              var result = false;

            var seriesNumnberCount = 0;
            var seriesNumnberList = [];
            passwordNumnberList.forEach(function(item) {
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
                document.getElementById("ConfirmPassword").style.height = "14px";
                document.getElementById("LoginScreen").style.height = "280px";
                document.getElementById("LoginScreen").style.width = "690px";
                document.getElementById("LoginScreen").style.minWidth = "690px";

            } else {
                document.getElementById("CurrentPasswordArea").style.display = "none";
            }
           

          
  

            UserEmailText = email;

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

                                $("#loginlogo").attr("src", GetApplicationLogoSource(myLogoCode));

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
                IsWarringErrorShow = false;
         

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
            var bindingNode = document.getElementById('Container');
            ko.cleanNode(bindingNode);
            ko.applyBindings(new viewModel(), bindingNode);
        });
    </script>




</body>
</html>
