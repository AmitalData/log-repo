<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordResetRequestPage.aspx.cs" Inherits="WebFreight.Web.PasswordResetRequestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link id="logolink" rel="shortcut icon" />


    <title></title>

    <link href="css/asp.css" rel="stylesheet" type="text/css"/>
    <link href="css/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="css/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <script src="js/jquery-3.5.1.min.js" type="text/javascript"></script>
    <script src="js/kendo.all.min.js" type="text/javascript"></script>
    <script src="js/knockout-3.5.1.js" type="text/javascript"></script>
    <script src="js/knockout-kendo.min.js" type="text/javascript"></script>

    <link href="HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css"/>    
    <link href="HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css"/>
    <script src="HtmlHelpers/JS/LogitudeTools.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
</head>

<body>
   
    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>

     <div id="Container">

         <input id="PartnerEnvironmentInput" type="hidden" runat="server" />

        <table style="width:100%;">
            <thead>
            <tr style="height:114px;">
                <td></td>
                <td style="width:1024px; text-align:center; vertical-align:top;" >                   
                    
                    <a id="imglink" target="_blank" style="display:none;">
                        <img id="loginlogo" style="margin-top:50px; width:290px; height:114px;" /> 
                    </a>

                        <div id="Partnerdiv" style="display:none; margin-top:50px; width:800px; height:114px; margin-left:auto; margin-right:auto">
                            <div style="display: table-row;">
                                <div id="otherPartners" style="display:table-cell; width:290px;">
                                       <a  id="Image2Link" >
                                        <img id="PartnerImg" style="width:290px; height:114px;" />
                                       </a>
                                </div>

                                <div id="PangeaPartners" style="display:table-cell; width:350px;vertical-align:middle">
                                                                           <a  id="Image3Link" >
                                        <img id="PartnerImg2" style="width:350px; height:80px;margin-bottom:20px" />
                                                                               </a>


                                </div>

                                <div style="display:table-cell;"></div>

                                <div style="display:table-cell; width:290px;">
                                    <a id="imglink1" target="_blank">
                                        <img id="loginlogo1" style="width:290px; height:114px;" /> 
                                    </a>
                                </div>

                            </div>
                        </div>

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
                
                    <td style="width:1140px; text-align:center; vertical-align:top;">
                        <table style="width:100%; margin-top:80px;">
                            <tr><td><img width="980" height="16" style="opacity:0.5;margin-bottom:-8px" src="images/LoginScreen/shadow1.png"/></td></tr>

                            <tr>
                                <td style="background:white;">
                                    <table style="width:100%; height:260px; border-collapse:collapse; border-spacing:0; ">

                            <tr>
                                <td>
                                    <table>
                                        <tr style="height:245px">
                                                 <td style="background:white;">
                                    <div style="float: left;margin-left:60px;width:300px" id="myform">
                                      
                                        <table >

                                                         <tr>
                                                              <td class="column1" style="font-family:Lucida Sans Unicode; font-weight:bold; font-size:16px; color:steelblue"> 
                                                                    Reset Password 
                                                                  
                                                              </td>  
                                                         </tr>

                                                         <tr>
                                                              <td class="column1" style="font-family:Arial; font-size:12px; color:#4B4A4A"> 
                                                                  Please enter your Email and press submit 
                                                              </td>         
                                                         </tr>

                                                         <tr>
                                                              <td class="column1" style="font-family:Myriad Pro; font-size:14px; color:#4B4A4A"> 
                                                                  e-mail:
                                                              </td>        
                                                         </tr>

                                                         <tr>
                                                             <td class="column1">
                                                                  <input autocomplete="on"  size="10"  class="auto-style1" id="Email" type="email" name="Email" runat="server"   placeholder="e.g. myname@example.net"  required data-email-msg="Email format is not valid" oninput="onEmailChanges()"/>

                                                                 <%--<asp:TextBox ID="txtEmail" runat="server" Width="212px" Height="17px"></asp:TextBox>--%>
                                                             </td>
                                                         </tr>

<%--         
                                            <tr style="height:5px;"><td></td></tr>--%>

                                                
                                     
                                                  <tr id="Areacaptcha" style ="height:70px;margin-top:3px;display:none;">
                                                 <td>
                                              <img id="CaptchaImage" style="height:auto;width:auto;float:left;" />
                                                <input oninput="onCaptchaInPutChanged()" style="height:19px;width:260px;margin-bottom:5px;margin-top:5px;float:left;" type="text" placeholder="type the text you see" id="captchaTextBox"/>
                                                


                                            </td>
                                            </tr>




                                                         <tr>
                                                             <td class="column1">
                                                                  <%--<asp:Button ID="Button1" runat="server" Text="Submit" Width="94px" OnClick="btnReset_Click" />--%>
                                                                 <input class="cmdSubmit"  type="submit" value="Submit >" runat="server" id="cmdSubmit" data-bind="click: submitMethod"/>
                                                    
                                                             </td>
                                                         </tr>

                                                         <tr></tr>

                                                         <tr style="vertical-align:bottom;" >
                                                             <td >
                                                                  <p id="HavingtroubleId" style="font-family:Arial; font-size:12px;height:12px; color:#4B4A4A" class="column1"> 
                                                                         Having trouble logging in? 
                                                                         <a id="DefaultContactUs" href="mailto:info@logitudeworld.com" >Contact us</a>  
                                                                         <a id="LogBoxContactUs" style="display:none" href="mailto:sales@logbox.co.il" >Contact us</a>  
                                                                         <a id="AerolineasContactUs"  style="display:none" href="mailto:Leandro.Martinez@aerolineas.com.ar"  >Contact us</a> 
                                                                         <a id="AtlasContactUs"  style="display:none" href="mailto:Mirjam.Schubert@champ.aero"  >Contact us</a> 
                                                                         <a id="BrandingContactUs"  style="display:none"   >Contact us</a> 
                                                                        <a id="ConnectaContactUs"  style="display:none" href="mailto:admin@pangea-network.com" >Contact us</a> 
                                                                        <a id="PangeaContactUs"  style="display:none" href="mailto:admin@pangea-network.com"  >Contact us</a> 
 
                                                                     
                                                                  </p>
                                                             </td>
                                                         </tr>
                                                   
                                                         <tr>
                                                             <td >
                                                                  <p style="color:red;display:none;text-align:left" id="errorsList"></p>
                                                             </td>
                                                         </tr> 
                                              
                                            <tr>
                                                             <td >
                                                                  <p style="color:SteelBlue;display:none;text-align:left" id="message">Submiting completed successfully.<br /> A reset link has been sent to your Email</p>
                                                             </td>
                                                         </tr> 


                                                         <tr id="BusyindicatorArea">
                                                             <td>
                                                                  <p id="busyIndicator" style="display:none"><img id="ImagebusyIndicator" width="50" height="50" src="images/LoginScreen/indicator.gif" alt='loading' /></p>

                                                             </td>
                                                         </tr>                                                           

                                                     </table>

                                    </div>
                                </td>
                                        </tr>
                                        <tr style="height:15px;vertical-align:top">
                                            
                                            <td style="vertical-align:top">

                                               <a style="margin-left:10px;font-size:12px;font-family:Arial;cursor:pointer;float:left;margin-left:60px;margin-top:-10px;vertical-align:top;text-decoration:underline" onclick="backToLoginClick()">Back to login page</a>
                                            </td>


                                        </tr>
                                    </table>
                                </td>

                                            <td style=" width:2px; text-align:right;border:0;"><img src="images/LoginScreen/line.png" style="width:2px;height:260px;margin-right:-3px;border:thick"/></td>

                                            <td style="width:639px;">
                                                <img id="LayerImage" style="display:none; height:260px;width:650px;min-width:650px;border:0" src="images/LoginScreen/Layer.png"/>

                                                <div id="AerolineasDiv" style="display:none; height:260px;min-width:650px;width:650px; background-image:url(images/LoginScreen/Layer_0.png); background-size:100%; background-size:650px 257px; border:0">
                                                    <div style="margin:5px 15px 5px 15px; text-align:left">
                                                        <div style="font-weight:bold; font-size:12px;">INFORMACION IMMPORTANTE - IMPORTANT INFORMATION</div>
                                                        <div style="font-weight:normal; font-size:12px;">Vuelos fuselaje angosto / Flights narrow body</div>
                                                        <div style="font-weight:normal; font-size:12px;">Peso máximo por bulto / Maximum weight per piece 90 kg</div>                     
                                                        <div style="font-weight:normal; font-size:12px;">(consulte por más kgs / ask for more kgs)</div>
                                                        <div style="font-weight:normal; font-size:12px;">Dimensiones máximas por bulto / Per piece max. dimensions:</div>
                                                        <div style="font-weight:normal; font-size:12px;">E190 (Embraer 190): 120 x 100 x 70 cm</div>
                                                        <div style="font-weight:normal; font-size:12px;">B737NG: 134 x 120 x 86 cm</div>

                                                        <div style="font-weight:normal; font-size:12px; margin-top:12px;">Vuelos paletizados - fuselaje ancho / Flights palletized - wide body: PMC & PAG</div>
                                                        
                                                        <div style="font-weight:normal; font-size:12px; margin-top:10px;">Para Registrarse / To Register:</div>
                                                        <div style="font-weight:normal; font-size:12px;"><a target="_blank"; style="cursor:pointer;" href="http://ebooking.champ.aero/public_user_registration.asp?airline_id=AR">http://ebooking.champ.aero/public_user_registration.asp?airline_id=AR</a></div>
                                                        
                                                        <div style="font-weight:normal; font-size:12px; margin-top:10px;">Contactenos a / Contact us:</div>
                                                        <div style="font-weight:normal; font-size:12px;"><a target="_blank"; style="cursor:pointer" href="mailto:leandro.martinez@aerolineas.com.ar">leandro.martinez@aerolineas.com.ar</a></div>
                                                    </div>
                                                </div>


                                                      <div id="AtlasDiv" style="display:none; height:260px;min-width:650px;width:650px; background-image:url(images/LoginScreen/Layer_0.png); background-size:100%; background-size:650px 257px; border:0">
                                                    <div style="margin:5px 15px 5px 15px; text-align:left">
                                                    
                                                        <div style="font-weight:normal; font-size:12px; margin-top:10px;">For Registration:</div>
                                                        <div style="font-weight:normal; font-size:12px;"><a target="_blank"; style="cursor:pointer;" href="AtlasRegistrationPage.aspx">Click here</a></div>
                                                        
                                                     <%--   <div style="font-weight:normal; font-size:12px; margin-top:10px;">Contactenos a</div>
                                                        <div style="font-weight:normal; font-size:12px;"><a target="_blank"; style="cursor:pointer" href="mailto:Mirjam.Schubert@champ.aero">Mirjam.Schubert@champ.aero</a></div>--%>
                                                    </div>
                                                </div>

                                              <div id="ConnectaDiv" style="display:none; height:260px;min-width:650px;width:650px; background-image:url(images/LoginScreen/Layer_0.png); background-size:100%; background-size:650px 257px; border:0">
                                                    <div style="margin:5px 15px 5px 15px; text-align:left">
                                                    
                                                          <div style="font-weight:bold; font-size:12px;">INFORMACION IMMPORTANTE - IMPORTANT INFORMATION</div>
                                                        <div style="font-weight:normal; font-size:12px;">If you have a Logitude account please enter your login details below</div>
                                                        <div style="font-weight:normal; font-size:12px;">If you don’t have an account and would like to request one</div>
                                                        <div style="font-weight:normal; font-size:12px;"> please contact us at <a target="_blank"; style="cursor:pointer" href="mailto:admin@pangea-network.com">admin@pangea-network.com</a></div>                     
                                                        <div style="font-weight:normal; font-size:12px;">or register for a demo at <a target="_blank"; style="cursor:pointer" href="https://www.logitudeworld.com/demo-evn/"> Logitude demo page</a></div>                                                     
                                                        
                                                        <div style="font-weight:normal; font-size:12px;"></div>


                                               </div>
                                                </div>



                                                  <div id="pangeaDiv" style="display:none; height:260px;min-width:650px;width:650px; background-image:url(images/LoginScreen/Layer_0.png); background-size:100%; background-size:650px 257px; border:0">
                                                    <div style="margin:5px 15px 5px 15px; text-align:left">
                                                      <div style="font-weight:bold; font-size:12px;">INFORMACION IMMPORTANTE - IMPORTANT INFORMATION</div>
                                                        <div style="font-weight:normal; font-size:12px;">If you have a Logitude account please enter your login details below</div>
                                                        <div style="font-weight:normal; font-size:12px;">If you don’t have an account and would like to request one</div>
                                                        <div style="font-weight:normal; font-size:12px;"> please contact us at <a target="_blank"; style="cursor:pointer" href="mailto:admin@pangea-network.com">admin@pangea-network.com</a></div>                     
                                                        <div style="font-weight:normal; font-size:12px;">or register for a demo at <a target="_blank"; style="cursor:pointer" href="https://www.logitudeworld.com/demo-evn/"> Logitude demo page</a></div>                                                     
                                                        
                                                        <div style="font-weight:normal; font-size:12px;"></div>
                                                    </div>
                                                </div>

                                            </td>

                                            <td style="width:85px;"></td>
                            </tr>

                                    </table>
                                </td>
                            </tr>

                            <tr style="float:left;margin-top:-8.5px"><td><img width="980" height="16" style="opacity:0.5" src="images/LoginScreen/shadow2.png"/></td></tr>
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
 

    <script type="text/javascript">

        function backToLoginClick() {

            var myCode = document.getElementById('PartnerEnvironmentInput').value;

            switch (myCode) {

                case "connecta":
                    {
                        window.location.href = "Login.aspx?partner=" + myCode;
                        break;

                    }
                case "aerolineas":
                    {
                        window.location.href = "Login.aspx?partner=" + myCode;
                        break;
                    }

                case "atlas":
                    {
                        window.location.href = "Login.aspx?partner=" + myCode;
                        break;
                    }

                case "pangea": {

                    window.location.href = "Login.aspx?partner=" + myCode;
                    break;

                }

                default: {

                    //abed
                    if (IsBranding == "true") {
                        Tenant = window.sessionStorage.getItem("Tenant");
                        if (Tenant) {
                            window.location.href = "Login.aspx?tenant=" + parseInt(Tenant);
                        }
                    }
                    else {
                    window.location.href = "Login.aspx";
                    }
                    break;
                }
            }
        }

       var captchaKey = "";

        var IsChampLogin = false;
        function setCaretToPos(id, cursorPosition) {
            document.getElementById(id).selectionStart = cursorPosition;
            document.getElementById(id).selectionEnd = cursorPosition;
        }
        function onEmailChanges() {
            var emailstring = $("#Email").val();
            if (emailstring) {

              //  var cursorPosition = document.getElementById("Email").selectionStart;

                $("#Email").val($.trim(emailstring));

               // setCaretToPos("Email", cursorPosition);
            }
            //$("#cmbTenants").hide();
            $("#errorsList").hide();
        }

        function disableForm(disable) {
            if (disable) {
                $("input").prop('disabled', true);
            }
            else {
                $("input").prop('disabled', false);
            }
        }


            onCaptchaInPutChanged = function () {
            var x = document.getElementById("errorsList").innerHTML;
            if ( x == "Please re-enter the characters you see in the <br> image above") {
                if ($("#captchaTextBox").val())
                    $("#errorsList").hide();
            }
        }

        function HideCaptchaArea() {
            captchaKey = null;
            document.getElementById("captchaTextBox").value = null;
            document.getElementById("Areacaptcha").style.display = "none";
            document.getElementById("BusyindicatorArea").style.height = "";
            document.getElementById("BusyindicatorArea").style.width = "";
            document.getElementById("ImagebusyIndicator").width = "";
            document.getElementById("ImagebusyIndicator").height = "";
            document.getElementById("busyIndicator").style.marginTop = "0px";
            document.getElementById("HavingtroubleId").style.height = "";
            
        }


        function viewModel() {

            this.submitMethod = function () {
                $("#errorsList").hide();
                var validatable = $("#myform").kendoValidator().data("kendoValidator");
             var areacaptcha = document.getElementById("Areacaptcha");

                var hasError = false;


                if (areacaptcha.style.display == "block") {
                    document.getElementById("BusyindicatorArea").style.width = "0px";
                    document.getElementById("BusyindicatorArea").style.height = "0px";
                }


                if (validatable.validate() === false) {
                    // get the errors and write them out to the "errors" html container
                    var errors = validatable.errors();
                    $(errors).each(function () {
                        $("#errors").html(this);
                    });
                    return;
                }
       
                var heightWidthbusyIndicator = "40px";
                if (areacaptcha.style.display == "block") {
                    heightWidthbusyIndicator = "35px";
                    document.getElementById("busyIndicator").style.marginTop = "-8px";

                    if (!document.getElementById("captchaTextBox").value) {
                        document.getElementById("errorsList").innerHTML = "Please re-enter the characters you see in the <br /> image above"; 
                        $("#errorsList").show();
                        return;
                    }
                }
                
                document.getElementById("ImagebusyIndicator").width = heightWidthbusyIndicator.replace("px","");
                document.getElementById("ImagebusyIndicator").height = heightWidthbusyIndicator.replace("px","");

                document.getElementById("BusyindicatorArea").style.height = heightWidthbusyIndicator;
                document.getElementById("BusyindicatorArea").style.width = heightWidthbusyIndicator;


                disableForm(true);
                $("#busyIndicator").show();
                var email = $("#Email").val();
                Tenant = window.sessionStorage.getItem("Tenant");

                if (Tenant) {
                    email = email + "^" + Tenant;
                }

                 var url = "api/ResetPassword?PostResetPassword";
                function ResetPasswordParameters() {

                    this.Email = email;
                    this.IsChampLogin =IsChampLogin;
                    this.CaptchaKey = captchaKey;
                    this.CaptchaCode = document.getElementById("captchaTextBox").value;
                };

                
                var param = new ResetPasswordParameters();
                $.ajax({
                    url: url,
                    type: 'POST',
                    data: JSON.stringify(param),
                    contentType: 'application/json',
                    success: function (userdata) {

                        $("#busyIndicator").hide();
                        $("#message").hide();
                        $("#errorsList").hide();


                        disableForm(false);
                        
                        if (!userdata.HasError) {
                            $("#message").show();
                            HideCaptchaArea();
                        }
                        else {

                            captchaKey = userdata.CaptchaKey;

                           

                            if (userdata.InValidCaptcha) {
                                if (areacaptcha.style.display == "block") {
                                    document.getElementById("captchaTextBox").value = "";
                                }

                                document.getElementById("BusyindicatorArea").style.width = "0px";
                                document.getElementById("BusyindicatorArea").style.height = "0px";

                                areacaptcha.style.display = "block";
                                $("#CaptchaImage").attr("src", userdata.CaptchaImage);
                                document.getElementById("captchaTextBox").value = "";

                            }

                            var errorMessage = "";

                            if (userdata.ExceptionMessage) alert(userdata.ExceptionMessage);
                            else {

                                if (userdata.InValidCaptcha) errorMessage = "Please re-enter the characters you see in the <br /> image above";
                                else if (userdata.IpRestricted) errorMessage = "Unauthorized IP Address. Your IP is not authorized to access this account!";
                                else if (userdata.InActive) errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
                                if (errorMessage) {
                                    document.getElementById("errorsList").innerHTML = errorMessage;
                                    $("#errorsList").show();
                                } else {
                                    $("#message").show();
                                    HideCaptchaArea();
                                }
                            }
                        }


                    },

                    error: function (jqXHR, textStatus, errorThrown) {

                        disableForm(false);
                        $("#error").text("errror");
                        $("#error").show();
                        $("#loginBusyindicator").hide();
                        var errorMessage = '';
                    }
                });
            }
        }

        //function getURLParameter(name) {
        //    return decodeURI(
        //        (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
        //    );
        //}

      
        $(document).ready(function () {

            var logo = window.sessionStorage.getItem("loginlogo");

          if (logo) {
              $("#loginlogo").attr("src", logo);
              $("#loginlogo").css("width", "290px");
              $("#loginlogo").css("height", "114px");
      
          }
          else {
            var myLogoMethodUrl = "api/authentication?myDummyInteger=" + 0 + "&myDummyString=" + "0";
            $.ajax({
                url: myLogoMethodUrl,
                type: 'GET',
                contentType: 'application/json',

                success: function (myLogoCode) {
                    var myCode = document.getElementById('PartnerEnvironmentInput').value;
                  //  if (myCode != "connecta" && myCode != "pangea") {
                        $("#logolink").attr("href", GetApplicationLogoIcon(myLogoCode));
                        $("#loginlogo").attr("src", GetApplicationLogoSource(myLogoCode));
                        $("#imglink").attr("href", GetApplicationLogoUrl(myLogoCode));

                        $("#loginlogo1").attr("src", GetApplicationLogoSource(myLogoCode));
                        $("#imglink1").attr("href", GetApplicationLogoUrl(myLogoCode));
                   // }
                },
            });
          }



            //var ischamp = getURLParameter("ischamplogin");
            //if (ischamp) {
            //    ischamp = ischamp.toLowerCase();
            //}

            // if (ischamp == "true") {
            //     IsChampLogin = true;
            //     $("#loginlogo").attr("src", "images/LoginScreen/champ.png");
            //     $("#loginlogo").attr("width", "114");
            //     $("#imglink").attr("href", "http://www.cargoserv.com/");
            // }

            // else {
            //     $("#loginlogo").attr("src", "images/LoginScreen/header.jpg");
            // }

            var bindingNode = document.getElementById('Container');
            ko.cleanNode(bindingNode);
            ko.applyBindings(new viewModel(), bindingNode);
        });



    </script>

    <script type="text/javascript">

        var Tenant ="";
        var IsBranding = false;
        var IsPrivateLabel = false
        $(document).ready(function () {

            var myCode = document.getElementById('PartnerEnvironmentInput').value;
           
            $("#AerolineasContactUs").css("display", "none");
            $("#DefaultContactUs").css("display", "none");
            $("#LogBoxContactUs").css("display", "none");
            $("#BrandingContactUs").css("display", "none");
            $("#AtlasContactUs").css("display", "none");
            
            if (myCode == "aerolineas") {
                $("#AerolineasContactUs").css("display", "inline");
                $("#Image2Link").attr("href", "http://www.aerolineas.com.ar/Welcome");
            }
            else if (myCode == "atlas") {
                $("#AtlasContactUs").css("display", "inline");
                $("#Image2Link").attr("href", "http://www.atlas.com/Welcome");
            }
            else if (myCode == "connecta") {
                $("#ConnectaContactUs").css("display", "inline");
                $("#Image2Link").attr("href", "https://www.logitudeworld.com/demo-evn/");
            }

            else if (myCode == "pangea") {
                $("#PangeaContactUs").css("display", "inline");
                $("#Image3Link").attr("href", "https://www.logitudeworld.com/demo-evn/");
            }
            if (myCode != "pangea" && myCode != "connecta" && myCode != "aerolineas" && myCode != "atlas")
             {

                IsBranding = window.sessionStorage.getItem("IsBranding");
                IsPrivateLabel = window.sessionStorage.getItem("IsPrivateLabel");
                var contactEmail = window.sessionStorage.getItem("ContactEmail");

                if (IsBranding == "true" || IsPrivateLabel == "true") {
                    if (contactEmail) {
                        $("#BrandingContactUs").css("display", "inline");
                        $("#BrandingContactUs").attr("href", "mailto:" + contactEmail);

                    }

                }
                else {
                    if (myCode == "logbox") {
                        $("#LogBoxContactUs").css("display", "inline");
                    }
                    else {
                        $("#DefaultContactUs").css("display", "inline");
                    }


                }
           
            }


            $("#otherPartners").css("display", "table");
            $("#PangeaPartners").css("display", "none");



           

            switch (myCode) {



                
                case "connecta": {
                    $("#Partnerdiv").css("display", "table");
                    $("#PartnerImg").attr("src", "images/ApplicationLogo/ConnectaLogo.png");
                    $("#ConnectaDiv").css("display", "table");
                    break;
                }

                case "pangea": {
                    $("#otherPartners").css("display", "none");
                    $("#PangeaPartners").css("display", "table");                    
                    $("#Partnerdiv").css("display", "table");
                    $("#PartnerImg2").attr("src", "images/ApplicationLogo/PangeaLogo.png");
                    $("#ConnectaDiv").css("display", "table");

                    break;
                }

                case "aerolineas": {
                    $("#Partnerdiv").css("display", "table");
                    $("#PartnerImg").attr("src", "images/ApplicationLogo/AerolineasLogo.png");
                    $("#AerolineasDiv").css("display", "table");



                    break;
                }
                    
                case "atlas": {
                    $("#Partnerdiv").css("display", "table");
                    $("#PartnerImg").attr("src", "images/ApplicationLogo/AtlasAirLogo.png");
                    $("#AtlasDiv").css("display", "table");
                    break;
                }


                default: {
                    $("#LayerImage").css("display", "inline");
                    $("#imglink").css("display", "inline");
                    break;
                }
            }
        });
    </script>

</body>
</html>
