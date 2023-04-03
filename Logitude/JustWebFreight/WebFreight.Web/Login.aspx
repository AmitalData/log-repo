<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebFreight.Web.Login" %>

<!DOCTYPE html>

<html>

<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=11">
    <link id="logolink" rel="shortcut icon" />
    <title></title>




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
    <script src="Scripts/json2.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>     	
       
        
    <link href="css/asp.css" rel="stylesheet" type="text/css"/>

    <style type="text/css">
        span.k-icon.k-i-arrow-s {
            background-image: url('HtmlHelpers/Images/Icons/DropArrow.png');
            background-size: 12px 12px;
            background-position: 0 0;
        }
    </style>
</head>

<body onload="get_cookie_data()" onkeydown="capLock( event )">
    
    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>

    <% if (Simplog.Server.Infrastructure.LogitudeSettings.WorkEnvironment == "customs")
        {%>
        <script type="text/javascript" src="HtmlHelpers/JS/Amital.GatewayControl.js"></script>
    <%  }%>

    <div id="Container" style="display:none;">

        <div id="updatedate" style="margin-left:10px" hidden="hidden"></div>
        <input id="PartnerEnvironmentInput" type="hidden" runat="server" />

        <table style="width:100%;">
            <thead>
                <tr style="height:114px;">
                    <td></td>
                    <td style="width:1024px; text-align:center; vertical-align:top;" >                   
                        
                        <a id="imglink" target="_blank" style="display:none;">
                               <img   id="loginlogo" style="margin-top:50px;" /> 
                        </a>

                        <div id="Partnerdiv" style="display:none; margin-top:50px; width:800px; height:114px; margin-left:auto; margin-right:auto">
                            <div  style="display: table-row;">
                                <div id="othersTable" style="display:table-cell; width:290px;">
                                    <img id="PartnerImg" style="width:290px; height:114px;" />
                                    <img id="PartnerImgAtlas" style="width:290px;display:none; height:152px;margin-bottom:-20px" />

                                </div>


                                 <div id="pangeaTable" style="display:table-cell; width:350px;vertical-align:middle">
                                    <img id="PartnerImg2" style="width:350px; height:80px;margin-bottom:20px" />


                                </div>

<%--                                   <div style="display:table-cell; width:150px;">
                                  
                                </div>
                                  <div style="display:table-cell; width:290px;">
                                        <img id="loginlogo" style="width:290px; height:114px;" />

                                </div>--%>
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
                         <div id="mapBackground"> 
                     <table> 
                <tr>
              
                    <td></td>  
                
                    <td id="DefultLoginScreen" style="width:1140px; text-align:center; vertical-align:top;display:none">
                        <table style="width:100%; margin-top:80px;">
                            <tr><td><img width="980" height="16" style="opacity:0.5;margin-bottom:-8px" src="images/LoginScreen/shadow1.png"/></td></tr>

                            <tr>
                                <td style="background:white;">
                                    <table style="width:100%; height:226px; border-collapse:collapse; border-spacing:0; ">

                            <tr>
                                            <td>
                                                 
                                    <div style="float: left;margin-left:60px;width:300px;" id="myform">
                                      
                                          <table id="loginForm" style="display:normal">

                                         <tr>
                                            <td class="column1">
                                                e-mail: 
                                                <br />

                                                <div style="height: 33px; width: 267px; border: 1px solid lightgray; border-radius: 4px;" class="InputShow">
                                                    <input  class="auto-style1" style="width: 258px; border: 0px; height: 25px;" autocomplete="on" size="10" id="Email" type="email" name="Email" runat="server" placeholder="e.g. myname@example.net" required data-email-msg="Email format is not valid" onblur="onEmailBlur()" />
                                                </div>
                                            </td>
                                               
                                            
                                        </tr>
                                        

                                        <tr>
                        
                                            <td class="column1">
                                                Password: 
                                                <br />

                                                <div style="height: 33px; width: 267px; border: 1px solid lightgray; border-radius: 4px;" class="InputShow">
                                                    <input class="auto-style1" style="width: 230px; border: 0px; height: 25px;"  id="Password" type="password" autocomplete="off" runat="server" oninput="onPasswordChanged()" required data-email-msg="password is required!" onkeypress="capLock(event)" />
                                                    <img src="images/LoginScreen/password_eye_closed.png" id="ShowHidePasswordImageId" title="Show Password" alt="Show Password" style="padding-left: 2px; font-size: 12px; vertical-align: central; font-family: Arial; cursor: pointer; text-decoration: none; float: right; margin-top: 5px; margin-right: 2px;" onclick="ShowHidePasswordClick()" />
                                                </div>
                                            </td>

                                                  <%--          <td class="column1">Password: <br /><input class="auto-style1" id="Password" type="password" autocomplete="off" runat="server" oninput="onPasswordChanged()" required data-email-msg="password is required!" onkeypress="capLock(event)"/>
                                                            <a id="ShowHidePasswordLinkId" style="position:absolute;vertical-align:central;margin-left:-42px;margin-top:10px;font-family:Arial; cursor:pointer;" onclick="ShowHidePasswordClick()">Show</a>
                                                            </td>               --%>     
                                        </tr>

                                          
                                              <tr>
                                                 <td  class="column1">
                                                   
                                            <div id="divMayus" style="visibility:hidden">
                                                <img width="15" height="15" src="images/SimplogIcons/warning.png" /> Caps Lock is on. 
                                                
                                            </div> 
                                            </td>

                                            </tr>
                                              <%--Start Areacaptcha--%>
                                                 <tr id="Areacaptcha" style ="height:70px;margin-top:5px;display:none;">
                                                 <td>
                                               <img id="CaptchaImage" style="height:auto;width:auto;float:left"  /> 
                                                <input oninput="onCaptchaInPutChanged()" style="height:19px;width:260px;margin-bottom:5px;margin-top:5px;float:left;" type="text" placeholder="type the text you see" id="captchaTextBox"/>
                                                


                                            </td>
                                            </tr>

                                             <%--End Areacaptcha--%>
                                                
                                              <tr>
                                            <td  class="column1"> 
                                                <input class="loginButton" type="submit" value="Login >" runat="server" id="cmdLogin" data-bind="click: validateMethod" formmethod="post"/>
                                                <a style="margin-left:15px;color:#4B4A4A;font-size:12px;font-family:Arial; cursor:pointer;" onclick="resetpasswordclick()">Forgot your password?</a>
                                            </td>
                                        </tr>
                                              
                                              <tr>
                                                <td class="column1">
                                                      <span style="background-color:transparent">
                                                       <p style="color:red;display:none;text-align:left;background-color:transparent;margin-top:3px;width:268px" id="errorsList">Login failed! invalid e-mail or password.</p>

                                                    </span>
                                                </td>
                                            </tr>
                                            
                                         <tr id="BusyindicatorArea" style="height:50px;width:50px">
                                                  <td>
                                                         <div id="loginBusyindicator" style="display:none"><img width="50" height="50" src="images/LoginScreen/indicator.gif" alt='loading' /></div>
                                                  </td>
                                                  
                                                    <td>
                                                     
                                                    </td>
                                                </tr>

                                            </table> 

                                      
                                        <table id="comboForm" style="display:none;margin-right:20px">
                                             
                                             <tr>
                                            <td  style="text-align:left;width:300px;font-family:Myriad Pro;font-size:14px;color:teal;">
                                                You have more than one account, please choose which one you want to log in.
                                            </td>
                                               
                                             
                                        </tr>
                                            <tr>
                                                <td class="column1">
                                                    <div style="margin-top: 5px">
                                                        <input class="k-dropdown" id="cmbTenants" runat="server" style="width: 258px; border: 0px; height: 25px;" /></div>
                                                </td>
                                            </tr>

                                            <tr>
                                               <td></td>
                                            </tr>
                                             <tr>
                                               <td></td>
                                            </tr>
                                             <tr>
                                               <td></td>
                                            </tr>
                                          <tr>
                                              <td class="column1">

                                                     <input style="width:120px;height:35px;margin-left:0px;" class="cmdSubmit"  type="submit" value="Continue >" runat="server" id="cmdContinue" data-bind="click: continueMethod"/>
                                                 
                                                   <a id="BackToLogin" style="margin-left:15px;margin-top:50px;color:#4B4A4A;font-size:12px;font-family:Arial;vertical-align:central" href="login.aspx" >Back to login page</a>
                                           
                                                  
                                               </td>
                                             

                                          </tr>

                                              <tr>
                                                <td class="column1">
                                                      <span style="background-color:transparent">
                                                       <p style="color:red;display:none;text-align:left;background-color:transparent;margin-top:3px;width:268px" id="comboFormErrorsList">Login failed! invalid e-mail or password.</p>

                                                    </span>
                                                </td>
                                            </tr>
                                       
                                             </table>

                                        
                                                <table id="verificationForm" style="display:none">
                                                    <tr>
                                                        <td style="width: 400px;">

                                                            <table>
                                                              <%--  <tr style="height: 25px;">
                                                                    <td>
                                                                        <div></div>
                                                                    </td>
                                                                </tr>--%>
 
                                                                <tr style="height: 10px;">
                                                                    <td>
                                                                        <div></div>
                                                                    </td>
                                                                </tr>

                                                                <tr style="height:25px">
                                                                    <td>
                                                                        <div style="text-align:left;width:300px;font-family:Myriad Pro;font-size:22px;color:#747B83;">Enter Verification Code.</div>
                                                                        <div  style="text-align:left;width:300px;font-family:Myriad Pro;font-size:14px;color:#2A2E31;margin-top:10px;" id="VerificationMessage"></div>
                                                                      <%--  A verification code was sent to the following number {{UserMobileNumber}}
                                                                        <br />the verification code expires in : 10 minutes--%>
                                                                    </td>
                                                                    <%--<td style="text-align:left;width:300px;font-family:Myriad Pro;font-size:18px;color:teal;">
                                                                        A verification code is needed in order to complete the login
                                                                        <br />Please contact your system administrator to fill your mobile number in order to get the verification code
                                                                    </td>--%>

                                                                </tr>

                                                                
                                                                <tr style="height: 30px;">
                                                                    <td colspan="2">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width:110px">
                                                                                  <%--  <label style="text-align:left">Enter you verification code:</label>--%>
                                                                                    <div>
                                                                                        <input class="auto-style1"  style="width:220px;text-align:left" id="VerificationCode"/>
                                                                                    </div>

                                                                                </td>

                                                                                <td style="vertical-align:middle;margin-top:5px;">
                                                                             
                                                                                    <div style="text-align:left">
                                                                                       <%-- <a style="margin-left:5px;margin-top:10px;padding-top:5px;vertical-align:middle;cursor:pointer;" onclick="resendVerificationCodeClick()">Resend</a>--%>
                                                                                    </div>
                                                                                    
                                                                                </td>
                                                                            </tr>

                                                                           
                                                                        </table>
                                                                    </td>
                                                                   

                                                                </tr>
                                                                <!--<button style="width:50px;height:35px;background-color:green;color:white" (click)="ResendVerificationCodeClicked()">Resend</button>-->
                                                                <tr style="height: 10px;">
                                                                    <td>
                                                                        <div></div>
                                                                    </td>
                                                                </tr>

                                                                <tr style="height: 30px">
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 1px;">
                                                                                    <input class="cmdSubmit"  type="submit" value="Submit >" runat="server" id="cmdSubmit" data-bind="click: verifyClicked"/>
                                                                                  <%--  <button style="width:80px;height:35px;background-color:green;color:white" onclick="VerifyClicked()">Submit ></button>--%>
                                                                                    <!--<button class="LoginButton" (click)="VerifyClicked()">Submit ></button>-->
                                                                                </td>
                                                                                  <td style="text-align:left">
                                                                                    <div id="verificationBusyindicator" style="display: none">
                                                                                        <img width="30" height="30" src="images/LoginScreen/indicator.gif" alt='loading' /></div>
                                                                                </td>
                                                                                <%--<td>
                                                                                    <div></div>
                                                                                </td>--%>
                                                                            </tr>
                                                                            <tr style="height:30px;">
                                                                                <td colspan="2"  style="text-align:left;">
                                                                                    <a style="margin-left:5px;margin-top:0px;padding-top:0px;vertical-align:middle;cursor:pointer;font-size:12px;font-family:Arial;" onclick="resendVerificationCodeClick()">I didn't receive the code.</a>
                                                                                </td>
                                                                            </tr>

                                                                                <tr style="height:22px;">
                                                                                   <td colspan="2">
                                                                                       <div style="background-color: transparent">
                                                                                           <p style="color: red; display: none; text-align: left; background-color: transparent; margin-top: 0px; width: 268px" id="verErrorsList">Verification failed! invalid verification code.</p>

                                                                                       </div>
                                                                                   </td>
 
                                                                               </tr>

                                                                          

                                                                            <%--  <tr style="height: 30px; width: 30px">
                                                                                <td>
                                                                                    <div id="verificationBusyindicator" style="display: none">
                                                                                        <img width="30" height="30" src="images/LoginScreen/indicator.gif" alt='loading' /></div>
                                                                                </td>

                                                                                <td></td>
                                                                            </tr>--%>
                                                                        </table>
                                                                    </td>
                                                                </tr>

                                                             <%--   <tr style="height:25px">
                                                                    <td style="text-align:left;width:300px;font-family:Myriad Pro;font-size:14px;color:red;">
                                                                        The code you entered is incorrect or expired. Please try having the code sent to you again.
                                                                    </td>
                                                                 </tr>--%>
                                                                

                                                                

                                                             <%--   <tr style="height: 50px;">
                                                                    <td>
                                                                        <div *ngIf="HidePendingLoading"></div>
                                                                        <div *ngIf="!HidePendingLoading" style="width: 50px; height: 50px;float:left; margin: auto; background: url(Images/LoginScreen/indicator.gif) center center no-repeat; background-size: 50px 50px;"></div>
                                                                    </td>
                                                                    <td>
                                                                        <div></div>
                                                                    </td>
                                                                </tr>--%>
                                                                

                                                                <tr style="height: 10px;">
                                                                    <td>
                                                                        <div></div>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <div></div>
                                                                    </td>
                                                                </tr>

                                                               

                                                                <tr style="height: 10px;">
                                                                    <td>
                                                                        <div></div>
                                                                    </td>
                                                                </tr>

                                                            </table>

                                                        </td>

                                                       <%-- <td style="background:url(images/LoginScreen/Layer.png) center center no-repeat; background-size: 100% 100%;">
                                                            <div></div>
                                                        </td>--%>
                                                    </tr>
                                                </table>


                                            <%--   <table id="verificationForm" style="display:none;margin-right:20px">
                                             
                                             <tr>
                                            <td  style="text-align:left;width:300px;font-family:Myriad Pro;font-size:14px;color:teal;">
                                                You have more than one account, please choose which one you want to log in.
                                            </td>
                                               
                                             
                                        </tr>
                                             <tr>
                                                            <td class="column1"> 
                                                                <div style="margin-top:5px"><input class="k-dropdown" id="Text1" runat="server" style="display:normal;width:250px;margin-top:0px"/></div>
                                                            </td>                    
                                      
                                               
                                                   </tr>
                                            <tr>
                                               <td></td>
                                            </tr>
                                             <tr>
                                               <td></td>
                                            </tr>
                                             <tr>
                                               <td></td>
                                            </tr>
                                          <tr>
                                              <td class="column1">

                                                     <input style="width:120px;height:35px;margin-left:0px;" class="cmdSubmit"  type="submit" value="Continue >" runat="server" id="Submit1" data-bind="click: continueMethod"/>
                                                 
                                                   <a id="BackToLogin" style="margin-left:15px;margin-top:50px;color:#4B4A4A;font-size:12px;font-family:Arial;vertical-align:central" href="login.aspx" >Back to login page</a>
                                           
                                                  
                                               </td>
                                             

                                          </tr>
                                       
                                             </table>--%>

                                    </div>

                            
                                    <div id="PromptView" style="float: left;display:none;width:300px;background-color:white;height:100%;vertical-align:central;">
                                      
               

             <div style="font-size:17px;color:SteelBlue;margin-top:40px;height:40px"><b>Please select user interface</b></div>
             
                   <div id="DivMargin" style="font-size:16px;color:black;margin-top:60px"></div>
                       <div style="font-size:16px;color:black;margin-bottom:20px;margin-top:3px"> </div>
                  <table  style="display:normal;vertical-align:bottom;text-align:center;width:100%">
   
                    <tr >
                     <td>  <input class="promptButton"   style="margin-left:0px;"  type="submit" value="HTML5"   data-bind="click: promptButtonYes" formmethod="post"/></td>

                    <td>  <input    class="promptButton" type="submit" value="Silverlight" runat="server" id="Submit2" data-bind="click: promptButtonNo" formmethod="post"/></td>
                                          </tr>
                </table>

                                       
                                        <div style="height:10px"></div>

             <div style="height:100%">
                 <table  border="0">
                     <tr><td colspan="2" style="height:10px"></td></tr>
                     <tr style="height:20px">
                         <td>
                             
                         </td>
                
                     </tr>
                   <tr style="height:20px">
                         <td>
                    
                         </td>
                
                     </tr>

                     </table>

            

             </div>
                                       
                                    </div>




<%--test--%>



   <div id="PasswordExpirationDateView" style="float: left;display:none;width:350px;background-color:white;height:100%;vertical-align:central;">
                                      
 <%--                <div  style="font-size:17px;color:red;margin-top:20px;line-height:1.4;height:20px"><b>Password Expiration</b></div>--%>

             <div  style="font-size:17px;color:SteelBlue;margin-top:50px;line-height:1.4;height:40px"><b id="DivPasswordExpiration"></b></div>
             
                   <div  style="font-size:16px;color:black;margin-top:60px"></div>
                       <div style="font-size:16px;color:black;margin-bottom:20px;margin-top:3px"> </div>
                  <table  style="display:normal;vertical-align:bottom;text-align:center;width:100%">
   
                    <tr >
                               <td style="width:30px"><div  style="width:30px"></div></td>
                     <td>  <input class="promptButton"   style="margin-left:0px;"  type="submit" value="Yes"   data-bind="click: promptPasswordExpirationButtonYes" formmethod="post"/></td>

                    <td>  <input    class="promptButton" type="submit" value="No" runat="server" id="Submit1" data-bind="click: promptPasswordExpirationButtonNo" formmethod="post"/></td>
                                       
                               <td style="width:30px"><div  style="width:30px"></div></td>
                           </tr>
                </table>

                                       
                                        <div style="height:10px"></div>

             <div style="height:100%">             </div>
                   
                                    </div>
                                                   



<%--//Abed prompt--%>




                                                   
                                </td>









                                            <td style=" width:2px; text-align:right;border:0;"><img src="images/LoginScreen/line.png" style="width:2px;height:260px;margin-right:-3px;border:thick"/></td>
                                            <td style="width:639px;">
                                                
                                                <img id="DirectlyLayerImage" width="650" style="display:none; height:260px;min-width:650px;width:650px;border:0" src="images/LoginScreen/Layer.png"/>
                                                
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
                                                        <div style="font-weight:normal; font-size:12px;"><a target="_blank"; style="cursor:pointer;" href="AerolineaseRegistrationPage.aspx">Click here</a></div>
                                                        
                                                        <div style="font-weight:normal; font-size:12px; margin-top:10px;">Contactenos a / Contact us:</div>
                                                        <div style="font-weight:normal; font-size:12px;"><a target="_blank"; style="cursor:pointer" href="mailto:leandro.martinez@aerolineas.com.ar">leandro.martinez@aerolineas.com.ar</a></div>
                                                    </div>
                                                </div>


                                                   <div id="AtlasDiv" style="display:none; height:260px;min-width:650px;width:650px; background-image:url(images/LoginScreen/Layer_0.png); background-size:100%; background-size:650px 257px; border:0">
                                                    <div style="margin:5px 15px 5px 15px; text-align:left">
                                                    
                                                        <div style="font-weight:normal; font-size:12px; margin-top:10px;">For Registration:</div>
                                                        <div style="font-weight:normal; font-size:12px;"><a target="_blank"; style="cursor:pointer;" href="AtlasRegistrationPage.aspx">Click here</a></div>
                                                       <%-- 
                                                        <div style="font-weight:normal; font-size:12px; margin-top:10px;">Contactenos a</div>
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


                               <td id="DirectlyLoginScreen" style="width:1140px; text-align:center; vertical-align:top;">
                        <table style="width:100%; margin-top:80px;">
                            <tr><td><img width="980" height="16" style="opacity:0.5;margin-bottom:-8px" src="images/LoginScreen/shadow1.png"/></td></tr>

                            <tr>
                                <td style="background:white;">
                                    <table style="width:100%; height:226px; border-collapse:collapse; border-spacing:0; ">

                            <tr>
                                            <td>
                                              <img id="LayerImage"  style="display:none; height:260px;width:100%;border:0" src="images/LoginScreen/Layer.png"/>    
 
                                </td>

                                       
                            </tr>

                                    </table>
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

    </div>
          







    <script type="text/javascript">

        let newSystemTenant = getCookie("newSystemTenant");
        if (newSystemTenant != "") {
            if (document.location.href.indexOf("system.logitudeworld") > 0) {
                document.location.href = document.location.href.replace("system.", "systemnew.");
            } else if (document.location.href.indexOf("staging.logitudeworld") > 0) {
                document.location.href = document.location.href.replace("staging.", "stagingnew.");
            } else if (document.location.href.indexOf("test.logitudeworld") > 0) {
                document.location.href = document.location.href.replace("test.", "testnew.");
            }
        }

        function getTwoFactorKeys() {
            var allKeys = [];
            for (var key in window.localStorage) {

                if (key.indexOf("TwoFactorkey") != -1) {
                    console.log(key);
                    allKeys.push(window.localStorage[key]);
                }
            }
            console.log(allKeys);
            return allKeys;
        }

        function verifyClicked() {
            var verificationCode = $("#VerificationCode").val();
            if (!verificationCode || verificationCode == '') {
                document.getElementById("verErrorsList").innerHTML = "The code you entered is incorrect or expired. Please try having the code sent to you again.";
                $("#verErrorsList").show();
            }
            else {
                $("#verificationBusyindicator").show();

                var url = "api/Authentication/PostAuthenticationDeviceVerificationCode?deviceKey=" + UserDataPrompt.TwoFactorkey + "&verificationCode=" + verificationCode + "&tenant=" + UserDataPrompt.CurrentTenant;

                $.ajax({
                    url: url,
                    type: 'POST',

                    contentType: 'application/json',

                    success: function (result) {

                        //disableForm(false);
                        $("#verificationBusyindicator").hide();
                        if (result) {

                            $("#verificationBusyindicator").show();
                            var twoFactorKey = getTwoFactorKeys();
                            var loginParameters;
                            var loginurl = "";
                            if (LoginParametersData) {
                                loginurl = "api/authentication/?tenant=" + LoginParametersData.Tenant;
                                loginParameters = JSON.stringify(LoginParametersData);
                            }
                            else if (LoginTokenParameterData) {
                                var isAngular = true;
                                loginurl = "api/authentication/?isAngular=" + isAngular;
                                loginParameters = JSON.stringify(LoginTokenParameterData);
                            }

                            $.ajax({
                                url: loginurl,
                                headers: { 'TwoFactorkey': twoFactorKey },
                                type: 'POST',
                                data: loginParameters,
                                contentType: 'application/json',

                                success: function (userdata) {

                                    disableForm(false);
                                    $("#verificationBusyindicator").hide();

                                    if (!userdata.HasError) {
                                        UserDataPrompt = userdata;
                                        RunLogin(userdata);
                                    }
                                    else {

                                        if (userdata.ExceptionMessage) {

                                            alert(userdata.ExceptionMessage);
                                        }

                                        else if (userdata.MustChangePassword) {

                                            document.location.href = "PasswordChangePage.aspx?email=" + email
                                        }
                                        else {
                                            var errorMessage = "Login failed! invalid user name or password." + "<br/>";


                                            if (userdata.IpRestricted) {

                                                errorMessage = "Unauthorized IP Address. Your IP is not authorized to access this account!";

                                            }

                                            if (userdata.IsLocked) {

                                                errorMessage = "Your account has been locked out!" + "<br/>" + "please try again after 30 minutes.";
                                            }

                                            document.getElementById("verErrorsList").innerHTML = errorMessage;
                                            // $("#errorsList").text(errorMessage);
                                            $("#verErrorsList").show();
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
                            //RunLogin(UserDataPrompt);
                        }
                        else {
                            document.getElementById("verErrorsList").innerHTML = "The code you entered is incorrect or expired. Please try having the code sent to you again.";
                            $("#verErrorsList").show();
                        }


                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                        document.getElementById("errorsList").innerHTML = "Login failed.Server error.";
                        $("#errorsList").show();

                        disableForm(false);
                        $("#error").text("errror");
                        $("#error").show();
                        $("#loginBusyindicator").hide();
                        var errorMessage = '';
                    }
                });

                //return this._http.post(url, { headers: this.AuthHeader }).map(response => {
                //    return response.json();
                //});
            }
        }

        function resendVerificationCodeClick() {

            //var url = this.baseUrlApi + "Authentication/PostResendAuthenticationDeviceVerificationCode?deviceKey=" + deviceKey + "&userId=" + userId + "&tenant=" + this.CurrentTenant;

            $("#verificationBusyindicator").show();

            var url = "api/Authentication/PostResendAuthenticationDeviceVerificationCode?deviceKey=" + UserDataPrompt.TwoFactorkey + "&userId=" + UserDataPrompt.Id + "&tenant=" + UserDataPrompt.CurrentTenant;

            $.ajax({
                url: url,
                type: 'POST',

                contentType: 'application/json',

                success: function (result) {

                    //disableForm(false);
                    $("#verificationBusyindicator").hide();
                    //if (result) {
                    //    //RunLogin(UserDataPrompt);
                    //}
                    //else {
                    //    //document.getElementById("verErrorsList").innerHTML = "The code you entered is incorrect or expired. Please try having the code sent to you again.";
                    //    //$("#verErrorsList").show();
                    //}


                },

                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById("errorsList").innerHTML = "Login failed.Server error.";
                    $("#errorsList").show();

                    disableForm(false);
                    $("#error").text("errror");
                    $("#error").show();
                    $("#loginBusyindicator").hide();
                    var errorMessage = '';
                }
            });

        }

        window.history.forward();
        var url = window.location.href;


        var companyList;
        var password;
        var captchaKey
        var email;
        var currentTenant;
        var Technology = null;//"Angular";
        var SilverlightEndDate = null;

        var SetAngularCheckBoxValue = false;
        var TokenIncludeTenant = false;
        var ExternalTenant = "";
        var UserDataPrompt = null;
        var LoginParametersData = null;
        var LoginTokenParameterData = null;
        function resetpasswordclick() {

            var myCode = document.getElementById('PartnerEnvironmentInput').value;
            if (!myCode) {
                myCode = "";
            }


            switch (myCode.toLowerCase()) {

                case "connecta":
                    {
                        window.location.href = "PasswordResetRequestPage.aspx?partner=" + myCode;
                        break;
                    }

                case "pangea":
                    {
                        window.location.href = "PasswordResetRequestPage.aspx?partner=" + myCode;
                        break;
                    }

                case "aerolineas":
                    {
                        window.location.href = "PasswordResetRequestPage.aspx?partner=" + myCode;
                        break;
                    }

                case "atlas":
                    {
                        window.location.href = "PasswordResetRequestPage.aspx?partner=" + myCode;
                        break;
                    }


                default: {
                    window.location.href = "PasswordResetRequestPage.aspx";
                    break;
                }
            }
        }


        function ShowHidePasswordClick() {
            var showHidePasswordImage = document.getElementById("ShowHidePasswordImageId");
            if (showHidePasswordImage.attributes.src != null) {
                if (showHidePasswordImage.attributes.src.value == "images/LoginScreen/password_eye_closed.png") {
                    $("#ShowHidePasswordImageId").attr("src", "images/LoginScreen/password_eye.png");
                    $("#ShowHidePasswordImageId").attr("alt", "Hide Password");
                    $("#ShowHidePasswordImageId").attr("title", "Hide Password");
                }
                else {
                    $("#ShowHidePasswordImageId").attr("src", "images/LoginScreen/password_eye_closed.png");
                    $("#ShowHidePasswordImageId").attr("alt", "Show Password");
                    $("#ShowHidePasswordImageId").attr("title", "Show Password");
                }
            }
            var passwordInput = document.getElementById("Password");
            if (passwordInput.type === "password") passwordInput.type = "text";
            else passwordInput.type = "password";



        };








        function setCaretToPos(id, cursorPosition) {
            document.getElementById(id).selectionStart = cursorPosition;
            document.getElementById(id).selectionEnd = cursorPosition;
        }



        onCaptchaInPutChanged = function () {

            if (document.getElementById("errorsList").innerHTML == "Please re-enter the characters you see in the image above") {
                if ($("#captchaTextBox").val())
                    $("#errorsList").hide();
            }
        }


        //data-bind="event: {blur: getContacts}"

        onPasswordChanged = function () {

            if (document.getElementById("errorsList").innerHTML != "Please re-enter the characters you see in the image above") {
                $("#errorsList").hide();
            }

            var emailstring = $("#Password").val();
            if (emailstring) {

                var cursorPosition = document.getElementById("Password").selectionStart;
                $("#Password").val($.trim(emailstring));
                setCaretToPos("Password", cursorPosition);
            }
        }

        var TenantsQueryNamespace = {};

        function disableForm(disable) {
            if (disable) {
                $("input").prop('disabled', true);
            }
            else {
                $("input").prop('disabled', false);
            }
        }

        function onEmailBlur() {
            $("#errorsList").hide();
            var emailstring = $("#Email").val();
            if (emailstring) {
                $("#Email").val($.trim(emailstring));

                if (email != $("#Email").val()) {

                    var areacaptcha = document.getElementById("Areacaptcha");
                    if (areacaptcha && areacaptcha.style.display == "block") {
                        document.getElementById("captchaTextBox").value = "";
                        areacaptcha.style.display = "none";
                        captchaKey = "";
                    }


                }


            }
            //$("#cmbTenants").hide();

        }

        function LoginViewModel() {

            var selectedcompany;

            this.continueMethod = function () {

                if (selectedcompany) {

                    loginMethod(selectedcompany);

                }
            }

            function onChange() {
                //var combobox = $("#cmbTenants").data("kendoComboBox");
                //var selectedItem = combobox.dataSource.view()[combobox._current.index()];
                //selectedcompany = selectedItem;

                currentTenant = $('#cmbTenants').data('kendoComboBox').dataItem();
                selectedcompany = currentTenant;
                // var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType;

            }


            function verifyEmail() {
                var status = true;
                var emailRegEx = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
                if ($("#Email").val().search(emailRegEx) == -1) {
                    status = false;
                }
                return status;
            }

            function showTenantsCombo(companies) {

                companyList = companies;


                $("#cmbTenants").kendoComboBox(
                    {
                        dataTextField: "CompanyName",
                        dataValueField: "Tenant",
                        placeholder: "Choose your company",
                        //index: 0,
                        //select: onSelectedTenantChanged,
                        change: onChange,
                        filter: "contains",
                        suggest: false,

                        // template:
                        //     '<dd>${ CompanyName }</dd>'
                        //,

                        dataSource:
                        {
                            //type: "odata",
                            data: companies
                            //serverFiltering: true,
                            //serverPaging: true,
                            //pageSize: 20,
                        }
                        //template: kendo.template($("#partnerTemplate").html())
                    });


                disableForm(false);

                $("#loginForm").hide();
                $("#comboForm").show();
                $("#busyIndicator").hide();
                $("#loginBusyindicator").hide();


                //$("#cmbTenants").kendoComboBox();
                var combobox = $("#cmbTenants").data("kendoComboBox");
                combobox.focus();


                //$("#busyIndicator").hide();
            }


            this.validateMethod = function () {

                password = $("#Password").val();
                email = $("#Email").val();
                $("#errorsList").hide();


                var areacaptcha = document.getElementById("Areacaptcha");
                if (areacaptcha.style.display == "block") {
                    document.getElementById("BusyindicatorArea").style.width = "0px";
                    document.getElementById("BusyindicatorArea").style.height = "0px";
                }


                var persist = false;


                var validatable = $("#myform").kendoValidator().data("kendoValidator");

                if (validatable.validate() === false) {
                    document.getElementById("errorsList").innerHTML = "Login failed! invalid user name or password.";
                    $("#errorsList").show();
                    var errors = validatable.errors();
                    $(errors).each(function () {
                        $("#errors").html(this);
                    });
                    return;
                }




                var BusyindicatorAreawidthHeight = "50px";

                if (areacaptcha.style.display == "block") {
                    if (!document.getElementById("captchaTextBox").value) {
                        document.getElementById("errorsList").innerHTML = "Please re-enter the characters you see in the image above";
                        $("#errorsList").show();
                        return;
                    }
                    BusyindicatorAreawidthHeight = "40px";
                    document.getElementById("loginBusyindicator").style.marginTop = "-7px";
                }


                document.getElementById("BusyindicatorArea").style.width = BusyindicatorAreawidthHeight;
                document.getElementById("BusyindicatorArea").style.height = BusyindicatorAreawidthHeight;

                save_data_to_cookie();
                disableForm(true);

                $("#loginBusyindicator").show();

                var url = "api/authentication";//?email=" + email + "&password=" + password;//+ "&persistCookie=" + persist;
                function LoginParameters() {

                    this.Email = $.trim(email.toLowerCase());
                    this.Password = password;
                    this.GetToken = true;
                    this.ClientType = "Web";
                    this.CaptchaKey = captchaKey;
                    this.CaptchaCode = document.getElementById("captchaTextBox").value;
                };


                var param = new LoginParameters();
                LoginParametersData = param;

                var twoFactorKey = getTwoFactorKeys();
                $.ajax({
                    url: url,
                    type: 'POST',
                    headers: { 'TwoFactorkey': twoFactorKey },
                    data: JSON.stringify(param),
                    contentType: 'application/json',

                    success: function (userdata) {

                        disableForm(false);
                        UserDataPrompt = userdata;

                        if (!userdata.HasError) {
                            areacaptcha.style.display = "none";
                            ComplateProcessLogin(userdata);
                        }
                        else {

                            $("#loginBusyindicator").hide();
                            if (userdata.ExceptionMessage) {

                                alert(userdata.ExceptionMessage);
                            }
                            else {

                                captchaKey = userdata.CaptchaKey;


                                if (userdata.MustChangePassword) {

                                    document.location.href = "PasswordChangePage.aspx?email=" + email
                                }

                                else if (userdata.PasswordExpirationDateMessage) {
                                    document.getElementById("myform").style.display = "none";
                                    document.getElementById("verificationForm").style.display = "none";
                                    document.getElementById("PromptView").style.display = "none";
                                    document.getElementById("DivPasswordExpiration").innerHTML = userdata.PasswordExpirationDateMessage;
                                    document.getElementById("PasswordExpirationDateView").style.display = "block";
                                }


                                else {



                                    if (userdata.InValidCaptcha) {
                                        if (areacaptcha.style.display == "block") {
                                            document.getElementById("captchaTextBox").value = "";
                                        }

                                        areacaptcha.style.display = "block";
                                        $("#CaptchaImage").attr("src", userdata.CaptchaImage);
                                        document.getElementById("divMayus").style.display = "none";
                                        document.getElementById("BusyindicatorArea").style.width = "0px";
                                        document.getElementById("BusyindicatorArea").style.height = "0px";
                                    }


                                    var errorMessage = "";

                                    if (userdata.IpRestricted) errorMessage = "Unauthorized IP Address. Your IP is not authorized to access this account!";
                                    else if (userdata.InActive) errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
                                    else if (userdata.Unlicensed) errorMessage = "Your account is unlicensed!" + "<br/>" + "please contact your administrator.";
                                    else if (userdata.InValidMailOrPassword) errorMessage = "Login failed! invalid user name or password.";
                                    else if (userdata.InValidCaptcha && userdata.CaptchaImage) errorMessage = "Please re-enter the characters you see in the image above";
                                    else errorMessage = "Login failed! invalid user name or password." + "<br/>";




                                    document.getElementById("errorsList").innerHTML = errorMessage;
                                    // $("#errorsList").text(errorMessage);
                                    $("#errorsList").show();
                                }
                            }
                        }


                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                        document.getElementById("errorsList").innerHTML = "Login failed.Server error.";
                        $("#errorsList").show();

                        disableForm(false);
                        $("#error").text("errror");
                        $("#error").show();
                        $("#loginBusyindicator").hide();
                        var errorMessage = '';
                    }
                });
            }


            this.promptButtonYes = function () {

                LoginToAngular(UserDataPrompt);

                if (SetAngularCheckBoxValue == true) {
                    SetAngularasDefault();
                }

            };

            this.promptButtonNo = function () {

                LoginToSliverLight(UserDataPrompt);


            };


            this.promptPasswordExpirationButtonYes = function () {
                window.sessionStorage.setItem("PasswordChange", "ShowLink");
                document.location.href = "PasswordChangePage.aspx?email=" + email

            };

            this.promptPasswordExpirationButtonNo = function () {

                ComplateProcessLogin(UserDataPrompt);

            };

            function ComplateProcessLogin(userdata) {

                document.getElementById("myform").style.display = "block";
                document.getElementById("verificationForm").style.display = "none";
                document.getElementById("PromptView").style.display = "none";
                document.getElementById("DivPasswordExpiration").style.display = "none";
                document.getElementById("PasswordExpirationDateView").style.display = "none";



                logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType;
                if (userdata.ContactsCount == 1) {
                    LoginParametersData.Tenant = userdata.CurrentTenant;
                    LoginParametersData.IsUser = userdata.IsUser;
                    LoginParametersData.CardId = userdata.CardId;
                    LoginParametersData.CardType = userdata.CardType;

                    if (userdata.TwoFactorkey) {
                        window.localStorage.setItem('TwoFactorkey_' + userdata.Id + '_' + userdata.CurrentTenant, userdata.TwoFactorkey);
                    }
                    Technology = userdata.Technology;
                    SilverlightEndDate = userdata.SilverlightEndDate;

                    if (SilverlightEndDate) {
                        document.getElementById("TechnologyErrorMessage").innerHTML = "Please note, after " + SilverlightEndDate + " the Silverlight technology won't be supported.";
                        document.getElementById("TechnologyErrorMessage2").innerHTML = "Please start to use the HTML5 version.";
                        //"After " + SilverlightEndDate + " the Silverlight technology won't be supported. You will be directed to the HTML version";
                        document.getElementById("DivMargin").style.marginTop = "30px";
                    }


                    if (userdata.IsTwoFactorAuthenticationRequired == true) {

                        ShowVerificationForm(userdata);

                    }
                    else {
                        RunLogin(userdata);
                    }
                }
                else {

                    showTenantsCombo(userdata.CompanyLogins);
                }

            };



        };


        LoginToSliverLight = function (userdata) {


            if (SilverlightEndDate) {
                alert("Please note that the Silverlight version won't be supported any more after  " + SilverlightEndDate + ".\n" + "Please start to use the HTML5 version.");
                // alert("Silverlight version won't be supported any more after " + SilverlightEndDate);
            }
            Technology = "SL";

            var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType + ":" + userdata.IsBrandingEnabled;
            logindata = UserDataPrompt.UserName + ":" + UserDataPrompt.Id + ":" + UserDataPrompt.CurrentTenant;
            document.location.href = "default.aspx?userdata=" + logindata;
            $("#loginBusyindicator").hide();



        };

        LoginToShardLogistics = function (userdata) {

            if (userdata.KeepUserLoggedIn == true && userdata.Token) {
                var tokenKey = ExternalTenant ? "Token_" + userdata.CurrentTenant : "Token";
                var tokenCard = ExternalTenant ? "CardId_" + userdata.CurrentTenant : "CardId";
                window.localStorage.setItem(tokenKey, userdata.Token);
                window.localStorage.setItem(tokenCard, userdata.CardId);
            }

            var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType + ":" + userdata.IsBrandingEnabled;
            //document.location.href = "SharedLogisticPage.aspx?userdata=" + logindata;

            window.sessionStorage.setItem("IsSharedLogistics", true);

            var link = document.location.href.toLowerCase();
            var linkArray = link.split('login');
            url = linkArray[0];

            if (url.indexOf('/?tenant=') > -1) {
                url = url.split('/?tenant=')[0];
            }

            if (url.endsWith('/')) {
                url += "SharedLogisticPage.aspx";
            }

            else {
                url += "/SharedLogisticPage.aspx";
            }

            var params = [];
            params.push({ name: "Token", value: userdata.Token });
            params.push({ name: "LoginData", value: logindata });
            PostFormParams(url, params);

            $("#loginBusyindicator").hide();
        };


        LoginToAngular = function (userdata) {

            if (document && document.location && document.location.href &&  document.location.href.indexOf('?HowToDownloadPage=') > 0) {
                let documentArgs = document.location.href.split('?HowToDownloadPage=');
                let documentId = documentArgs.length > 1 ? documentArgs[1] : null;
                if (documentId) {
                    OpenHowToDownloadPage(userdata, documentId);
                    return;
                }
            }
            var isTenantAllowed = false;
            var Tenant = userdata.CurrentTenant;
            if (Tenant == 42 || Tenant == 1232 || Tenant == 1586 || Tenant == 1637 || Tenant == 1638 || Tenant == 341) {
                isTenantAllowed = true;
            }

            if (navigator.sayswho && navigator.sayswho.toString().indexOf("IE") > -1) {
                alert("Internet explorer is not supported in HTML5 version, please use Chrome, Firefox or Opera.");
                return;
            }

            if (navigator.sayswho && navigator.sayswho.toString().indexOf("Edge") > -1) {
                alert("Edge is currently not supported in HTML5 version, please use Chrome, Firefox or Opera.");
                return;
            }

            if (navigator.userAgent != null) {
                if (navigator.userAgent.toString().toLowerCase().indexOf("iphone") > -1) {
                    if (!isTenantAllowed) {
                        alert("IOS is currently not supported in HTML5 version");
                        return;
                    }
                }

                else if (navigator.userAgent.toString().toLowerCase().indexOf("ipad") > -1) {
                    if (!isTenantAllowed) {
                        alert("IOS is currently not supported in HTML5 version");
                        return;
                    }
                }

                else if (navigator.userAgent.toString().toLowerCase().indexOf("ipod") > -1) {
                    if (!isTenantAllowed) {
                        alert("IOS is currently not supported in HTML5 version");
                        return;
                    }
                }
            }


            //if (navigator.sayswho && navigator.sayswho.toString().indexOf("Safari") > -1) {
            //    var issafari = true;
            //    if (navigator.userAgent && (navigator.userAgent.toString().indexOf("Chrome") > -1 || navigator.userAgent.toString().indexOf("Firefox") > -1 || navigator.userAgent.toString().indexOf("FxiOS") > -1 || navigator.userAgent.toString().indexOf("ChiOS") > -1)) issafari = false;
            //    if (issafari == true) {
            //        alert("Safari is currently not supported in HTML5 version, please use Chrome, Firefox or Opera.");
            //        return;
            //    }
            //}

            var pageUrl = document.URL;
            var additionalParturl = "";

            if (pageUrl && pageUrl.indexOf("Menu=") > -1) {
                additionalParturl = pageUrl.split("Menu=")[1];
            }
            var data = JSON.stringify(userdata);
            window.sessionStorage.setItem("userdata", data);

            if (userdata.KeepUserLoggedIn == true && userdata.Token) {
                window.localStorage.setItem("Token_" + userdata.CurrentTenant, userdata.Token);
                window.localStorage.setItem("Token", userdata.Token);

                window.localStorage.setItem("CardId", "");
                window.localStorage.setItem("CardId_" + userdata.CurrentTenant, "");
            }

            var version = "";
            if (userdata.HtmlVersion) version = userdata.HtmlVersion;
            var angularUrl = "Angular" + version + "/index.html";
            if (additionalParturl) {
                angularUrl += ("?Menu=" + additionalParturl);
                if (ExternalTenant) {
                    angularUrl = angularUrl.replace("&Tenant=" + ExternalTenant, "");
                }

            }

            let prodNewEnvTenants = [2889, 341, 1, 42, 1489, 0, 1688, 2655, 2138, 3018, 2086, 1604, 3017, 558, 2860, 194, 2915,
                2838, 2780, 2779, 2770, 2742, 2601, 2591, 2580, 2531, 2526, 2511, 2331, 2240, 2037, 1681,
                1595, 1530, 1445, 807, 3000, 2999, 2998, 2983, 2964, 2961, 2935, 2927, 2921, 2915, 2899,
                2886, 2878, 2838, 2780, 2779, 2770, 2742, 2741, 2740, 2713, 2711, 2680, 2679, 2601, 2591,
                2580, 2531, 2526, 2511, 2510, 2470, 2450, 2448, 2383, 2366, 2331, 2268, 2240, 2219, 2211,
                2199, 2170, 2169, 2086, 2037, 1681, 1604, 1595, 1530, 1484, 1469, 1445, 1433, 1151, 1056,
                807, 802, 799, 468, 331, 293, 291, 289, 288, 286, 284, 283, 282, 281, 277, 275, 274, 272,
                270, 268, 266, 255, 253, 252, 251, 250, 248, 247, 242, 239, 237, 235];
            let testNewEnvTenants = [951, 1022];

            let newSystemTenant = getCookie("newSystemTenant");
            if (newSystemTenant == "") {
                if (document.location.href.indexOf("system.logitudeworld") > 0 || document.location.href.indexOf("staging.logitudeworld")>0) {
                    if (prodNewEnvTenants.indexOf(Tenant) >= 0) {
                        setCookie("newSystemTenant", Tenant, 70);
                    }
                } else if (document.location.href.indexOf("test.logitudeworld") > 0) {
                    if (testNewEnvTenants.indexOf(Tenant) >= 0) {
                        setCookie("newSystemTenant", Tenant, 70);
                    }
                }
            }
            if (document.location.href.indexOf('?Menu=') > 0) {
                document.location.href = document.location.href.replace("/Login.aspx", "/").replace("/login.aspx", "/").split('?')[0] + angularUrl;
            }
            else {
                document.location.href = document.location.href.replace("/Login.aspx", "/").replace("/login.aspx", "/") + angularUrl;
            }

            $("#loginBusyindicator").hide();

        };

        OpenHowToDownloadPage = function (userdata, DocumentId) {
            var url = document.location.href.replace("/Login.aspx", "/").split('?')[0] + 'WebPages/HowToDownloadPage.aspx?id=' + DocumentId;
            var params = [{ name: "Token", value: userdata.DocumentDownloadToken }, { name: "Code", value: DocumentId }]
            var form = document.createElement("form");
            form.target = "_self";
            form.method = "POST";
            form.action = url;
            for (var i = 0; i < params.length; i++) {
                var input = PrepareInput(params[i]);
                form.appendChild(input);
            }
            document.body.appendChild(form);
            form.submit();
            document.body.removeChild(form);
        }
        PrepareInput = function (input) {
            var mappedInput = document.createElement("input");
            mappedInput.type = "hidden";
            mappedInput.name = input.name;
            mappedInput.setAttribute("value", input.value);
            return mappedInput;
        }

        function setCookie(cname, cvalue, exdays) {
            const d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            let expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        }

        function getCookie(cname) {
            let name = cname + "=";
            let decodedCookie = decodeURIComponent(document.cookie);
            let ca = decodedCookie.split(';');
            for (let i = 0; i < ca.length; i++) {
                let c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }


        function ShowVerificationForm(userdata) {
            $("#verificationForm").show();

            $("#loginForm").hide();
            $("#comboForm").hide();
            $("#busyIndicator").hide();
            $("#loginBusyindicator").hide();

            if (userdata.UserMobileNumber) {
                document.getElementById("VerificationMessage").innerHTML = "We just sent an SMS with a verification code to " + userdata.UserMobileNumber + " Enter that code below." +
                    "<br />The verification code expires in 10 minutes";


            }
            else {
                document.getElementById("VerificationMessage").innerHTML = "A verification code is needed in order to complete the login" + ".Please contact your system administrator to fill your mobile number in order to get the verification code.";
            }
        }



        function RunLogin(userdata) {

            if (!Technology) Technology = userdata.Technology;
            if (userdata.IsUser) {


                if (Technology != "PR") {

                    if (Technology && Technology == "AG") {
                        // if (userdata.ContactsCount == 1)
                        LoginToAngular(userdata);
                    }
                    else LoginToSliverLight(userdata);
                }

                else {
                    document.getElementById("myform").style.display = "none";
                    document.getElementById("verificationForm").style.display = "none";
                    document.getElementById("PasswordExpirationDateView").style.display = "none";
                    document.getElementById("PromptView").style.display = "block";

                }

            }
            else {
                // ShardLogistics
                LoginToShardLogistics(userdata);
            }
        }
        var _LoginViewModel;//itzik need it !! 
        loginMethod = function (companyLogin) {


            disableForm(true);
            $("#loginBusyindicator").show();//bool isUser,string cardId,string cardType
            var url = "api/Authentication?tenant=" + companyLogin.Tenant;//?email=" + companyLogin.Email + "&password=" + password + "&tenant=" + companyLogin.Tenant + "&isUser=" + companyLogin.IsUser + "&cardId=" + companyLogin.CardId + "&cardType=" + companyLogin.CardType;
            function LoginParameters() {

                this.Email = companyLogin.Email;
                this.Password = password;

                this.IsUser = companyLogin.IsUser;
                this.CardId = companyLogin.CardId;
                this.CardType = companyLogin.CardType;
                this.GetToken = true;
                this.ClientType = "Web";
                this.CaptchaKey = captchaKey;
                this.CaptchaCode = document.getElementById("captchaTextBox").value;

            };


            var param = new LoginParameters();
            LoginParametersData = param;
            LoginParametersData.Tenant = companyLogin.Tenant;
            var twoFactorKey = getTwoFactorKeys();

            $.ajax({
                url: url,
                headers: { 'TwoFactorkey': twoFactorKey },
                type: 'POST',
                data: JSON.stringify(param),
                contentType: 'application/json',

                success: function (userdata) {

                    disableForm(false);
                    $("#loginBusyindicator").hide();

                    if (!userdata.HasError) {

                        if (userdata.TwoFactorkey) {
                            window.localStorage.setItem('TwoFactorkey_' + userdata.Id + '_' + LoginParametersData.Tenant, userdata.TwoFactorkey);
                        }

                        if (!Technology) Technology = userdata.Technology;
                        SilverlightEndDate = userdata.SilverlightEndDate;
                        if (SilverlightEndDate) {
                            //document.getElementById("TechnologyErrorMessage").innerHTML =  "After " + SilverlightEndDate + " the Silverlight technology won't be supported. You will be directed to the HTML version";
                            document.getElementById("TechnologyErrorMessage").innerHTML = "Please note, after " + SilverlightEndDate + " the Silverlight technology won't be supported.";
                            document.getElementById("TechnologyErrorMessage2").innerHTML = "Please start to use the HTML5 version.";
                            document.getElementById("DivMargin").style.marginTop = "30px";


                        }


                        UserDataPrompt = userdata;



                        if (userdata.IsTwoFactorAuthenticationRequired == true) {

                            ShowVerificationForm(userdata);

                        }
                        else {
                            RunLogin(userdata);
                        }




                    }
                    else {

                        if (userdata.ExceptionMessage) {

                            alert(userdata.ExceptionMessage);
                        }
                        else if (userdata.MustChangePassword) {

                            document.location.href = "PasswordChangePage.aspx?email=" + email
                        }
                        else {
                            var errorMessage = "Login failed! invalid user name or password." + "<br/>";
                            if (userdata.InValidCaptcha) {

                                errorMessage = "Please re-enter the characters you see in the image above";
                            }
                            else {
                                if (userdata.IpRestricted) errorMessage = "Unauthorized IP Address. Your IP is not authorized to access this account!";
                                if (userdata.IsLocked) errorMessage = "Your account has been locked out!" + "<br/>" + "please try again after 30 minutes.";
                            }

                            document.getElementById("comboFormErrorsList").innerHTML = errorMessage;
                            // $("#errorsList").text(errorMessage);

                            $("#comboFormErrorsList").show();
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

        var _LoginViewModel;//itzik need it !!




        navigator.sayswho = (function () {
            var ua = navigator.userAgent, tem,
                M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
            if (/trident/i.test(M[1])) {
                tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
                return 'IE';
            }
            if (M[1] === 'Chrome') {
                tem = ua.match(/\b(OPR|Edge)\/(\d+)/);
                if (tem != null) return tem.slice(1).join(' ').replace('OPR', 'Opera');
            }
            M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
            if ((tem = ua.match(/version\/(\d+)/i)) != null) M.splice(1, 1, tem[1]);
            return M.join(' ');
        })();



        function LoadLogo() {

            if (IsBranding == false && IsPrivateLabel == false) {

                var myLogoMethodUrl = "api/authentication?myDummyInteger=" + 0 + "&myDummyString=" + "0";
                $.ajax({
                    url: myLogoMethodUrl,
                    type: 'GET',
                    contentType: 'application/json',

                    success: function (myLogoCode) {
                        window.sessionStorage.setItem("LogoCode", myLogoCode);
                        $("#logolink").attr("href", GetApplicationLogoIcon(myLogoCode));
                        $("#imglink").attr("href", GetApplicationLogoUrl(myLogoCode));
                        $("#loginlogo").attr("src", GetApplicationLogoSource(myLogoCode));
                        $("#loginlogo").css("width", GetApplicationLogoWidth(myLogoCode));
                        $("#loginlogo").css("height", GetApplicationLogoHeight(myLogoCode));

                        $("#imglink1").attr("href", GetApplicationLogoUrl(myLogoCode));
                        $("#loginlogo1").attr("src", GetApplicationLogoSource(myLogoCode));

                        document.title = GetApplicationTitle(myLogoCode);

                        if (IsShowUpgradeScreen) ShowUpgradeScreen();

                    }, error: function (jqXHR, textStatus, errorThrown) {

                        if (IsShowUpgradeScreen) ShowUpgradeScreen();
                    }
                });


            }
            else if (IsPrivateLabel) {
                var hash = $(location).attr('href');
                var domain = hash.split('/')[2];
                var myLogoMethodUrl = "api/PrivateLable/getprivatelabellogouri/?url=" + domain;
                $.ajax({
                    url: myLogoMethodUrl,
                    type: 'GET',
                    contentType: 'application/json',

                    success: function (result) {
                        $("#loginlogo").attr("src", result);
                        $("#loginlogo").css("width", "290px");
                        $("#loginlogo").css("height", "114px");

                        window.sessionStorage.setItem("loginlogo", result);
                        if (IsShowUpgradeScreen) ShowUpgradeScreen();
                    }, error: function (jqXHR, textStatus, errorThrown) {
                        if (IsShowUpgradeScreen) ShowUpgradeScreen();

                    }
                });
            }
            else {

                var myLogoMethodUrl = "api/branding/gettenantlogouri/?tenant=" + BrandingTenant;
                $.ajax({
                    url: myLogoMethodUrl,
                    type: 'GET',
                    contentType: 'application/json',

                    success: function (result) {
                        $("#loginlogo").attr("src", result);
                        $("#loginlogo").css("width", "290px");
                        $("#loginlogo").css("height", "114px");

                        window.sessionStorage.setItem("loginlogo", result);
                        if (IsShowUpgradeScreen) ShowUpgradeScreen();
                    }, error: function (jqXHR, textStatus, errorThrown) {
                        if (IsShowUpgradeScreen) ShowUpgradeScreen();

                    }
                });


            }
        }

        function ShowUpgradeScreen() {

            document.location.href = "WebPages/UpgradeScreen.aspx";
        }

        function SetAngularasDefault() {

            var url = "api/UserExtended?userId=" + UserDataPrompt.UserId + "&setAngularAsDefault=" + SetAngularCheckBoxValue + "&tenant=" + UserDataPrompt.Tenant;

            $.ajax({
                url: url,
                type: 'GET',
                contentType: 'application/json',

                success: function (result) {


                },

                error: function (jqXHR, textStatus, errorThrown) {


                }
            });


        }


        function SetAngularasDefaultCheckBoxChange() {
            var result = document.getElementById('setAngularasDefaultId').value;
            if (result == "on") SetAngularCheckBoxValue = true;
        }

    </script>

    <script type="text/javascript">

        document.onkeypress = capLock;

        function capLock(e) {

            var s = "";
            //if (!e.ctrlKey) {
            //    kc = e.keyCode ? e.keyCode : e.which;
            //    if (kc == 20) {

            //        if (document.getElementById('divMayus').style.visibility == 'hidden') {
            //            document.getElementById('divMayus').style.visibility = 'visible';
            //        }
            //        else {
            //            document.getElementById('divMayus').style.visibility = 'hidden';
            //        }
            //    }
            //    else {
            //        sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
            //        if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
            //            document.getElementById('divMayus').style.visibility = 'visible';
            //        else
            //            document.getElementById('divMayus').style.visibility = 'hidden';
            //    }
            //}
        }


        function BrandingFunction() {
            window.sessionStorage.setItem("IsBranding", false);
            window.sessionStorage.setItem("IsPrivateLabel", false);
            window.sessionStorage.setItem("ContactEmail", "");
            window.sessionStorage.setItem("loginlogo", "");
            var hash = $(location).attr('href');
            var domain = hash.split('/')[2];
            if (hash) {
                var hashSplit = hash.toLowerCase().split("tenant");
                if (hashSplit) {
                    var Key = hashSplit[1];
                    if (hashSplit[1]) {
                        BrandingTenant = hashSplit[1].split('=')[1];
                        BrandingTenant = BrandingTenant.split('/')[0];
                    }
                }

            }
            if (BrandingTenant) {

                var myLogoMethodUrl = "api/branding/getisbrandingtenant/?tenant=" + BrandingTenant;
                $.ajax({
                    url: myLogoMethodUrl,
                    type: 'GET',
                    contentType: 'application/json',

                    success: function (result) {
                        if (result) IsBranding = result.EnableBranding;
                        if (IsBranding == true) {
                            window.sessionStorage.setItem("ContactEmail", result.ContactEmail);
                            window.sessionStorage.setItem("IsBranding", IsBranding);
                            window.sessionStorage.setItem("Tenant", BrandingTenant);

                            $("#BackToLogin").attr("href", "Login.aspx?tenant=" + BrandingTenant);

                        }
                        LoadLogo();
                    },
                });

            }
            else {
                var myLogoMethodUrl = "api/PrivateLable/getisprivatelableurl/?url=" + domain;
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
                        LoadLogo();
                    },
                });
            }

            _LoginViewModel = new LoginViewModel();
            var bindingNode = document.getElementById('Container');
            ko.cleanNode(bindingNode);
            ko.applyBindings(_LoginViewModel, bindingNode);

            $("#Password").keyup(function (e) {
                if (e.which == 13) {
                    _LoginViewModel.validateMethod();
                }
            });

        }


        function OnLoad() {
            const myDomain = GetLoggedDomain();
            //var isDSV = (url.toLowerCase().indexOf("dsv.co.il") > -1 || IsDSVLocalRun()) ? true : false;
            

            var plUrl = "api/PrivateLable/getisprivatelableurl/?url=" + myDomain;
            $.ajax({
                url: plUrl,
                type: 'GET',
                contentType: 'application/json',

                success: function (result) {
                    if (result && result.EnablePrivateLable) {
                        SystemLogin(result)
                    }
                    else {
                        SystemLogin(null);
                    }
                },

                error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }

        function GetLoggedDomain() {
            const url = window.location.href;
            var myDomain = url.split('/')[2];
            myDomain = myDomain.split(':')[0];

            return myDomain;
        }
        function SystemLogin(privateLable) {
            var isCargoTracking = IsaCargoTrackingDomain(window.location.href);
            var url = "api/LogitudeApplication"
            $.ajax({
                url: url,
                type: 'GET',
                contentType: 'application/json',

                success: function (result) {
                    if (result == true) {
                        IsShowUpgradeScreen = true;
                        if (privateLable) {
                            window.sessionStorage.setItem("Environment", "LogBox");
                            document.location.href = "WebPages/UpgradeScreen.aspx";
                        }
                        else BrandingFunction();
                    }
                    else {
                        IsShowUpgradeScreen = false;
                        if (isCargoTracking) {
                            RedirectToCargotrackingSite(window.location.href);
                        }
                        else if (privateLable) {
                            PrivateLableLogin(privateLable);
                        }
                        else {
                            ComplateLoadProess();
                            BrandingFunction();
                            var Containerelem = document.getElementById("Container");
                            if (Containerelem) {
                                Containerelem.style.display = 'block';
                            }
                        }
                    }
                },

                error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }

        function IsLocalRun() {
            const url = window.location.href;
            if (url)
                return url.toLowerCase().indexOf("localhost") > -1;
            else
                return false;
        }

        function PrivateLableLogin(privateLable) {
            if (privateLable) {
                window.sessionStorage.setItem("ContactEmail", privateLable.ContactUsEmail);
                window.sessionStorage.setItem("IsPrivateLabel", true);
                window.sessionStorage.setItem("SmallLogoURL", privateLable.SmallLogoURL);
                window.sessionStorage.setItem("LogoURL", privateLable.LogoURL);
                window.sessionStorage.setItem("PrivateLabelUrl", privateLable.PrivateLabelUrl);
                window.sessionStorage.setItem("PrivateLabelShortName", privateLable.PrivateLabelShortName);
                window.sessionStorage.setItem("IsDSV", privateLable.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1);
            }

            var urlMenu = "";
            var mypageUrl = document.URL;
            if (mypageUrl && mypageUrl.indexOf("Menu=") > -1) {
                urlMenu = mypageUrl.split("Menu=")[1];
                document.location.href = "AngularLogin" + "/index.html" + ("?Menu=" + urlMenu);
            }
            else {
                const privateLableLocal = IsLocalRun() ? "?PLlocal" : "";
                document.location.href = "AngularLogin" + "/index.html" + privateLableLocal;
            }
        }

        function ComplateLoadProess() {
            ExternalTenant = "";
            var urlPage = window.location.href;
            if (urlPage) {
                var args = urlPage.split('&');
                if (args[1] && args[1].indexOf('Tenant=') != -1) {
                    ExternalTenant = args[1].split('=')[1];

                }

            }

            var tokenKey = ExternalTenant ? "Token_" + ExternalTenant : "Token";
            var tokenCard = ExternalTenant ? "CardId_" + ExternalTenant : "CardId";

            var token = window.localStorage.getItem(tokenKey);
            var cardId = window.localStorage.getItem(tokenCard);

            if (token) {
                var isAngular = true;
                var url = "api/authentication/?isAngular=" + isAngular;
                function LoginTokenParameter() {
                    this.Token = token;
                    this.CardId = cardId ? cardId : "";

                };

                var param = new LoginTokenParameter();
                LoginTokenParameterData = param;
                var twoFactorKey = getTwoFactorKeys();
                $.ajax({
                    url: url,
                    type: 'POST',
                    headers: { 'TwoFactorkey': twoFactorKey },
                    data: JSON.stringify(param),
                    contentType: 'application/json',

                    success: function (userdata) {

                        disableForm(false);
                        UserDataPrompt = userdata;
                        if (!userdata.HasError) {

                            if (userdata.TwoFactorkey) {
                                window.localStorage.setItem('TwoFactorkey_' + userdata.Id + '_' + userdata.CurrentTenant, userdata.TwoFactorkey);
                            }

                            if (userdata.IsTwoFactorAuthenticationRequired == true) {

                                $("#DefultLoginScreen").css("display", "block");
                                $("#DirectlyLoginScreen").css("display", "none");

                                ShowVerificationForm(userdata);

                            }
                            else {
                                if (!userdata.IsUser) {
                                    LoginToShardLogistics(userdata);
                                }
                                else {

                                    LoginToAngular(userdata);
                                }
                            }
                        } else {
                            $("#DefultLoginScreen").css("display", "block");
                            $("#DirectlyLoginScreen").css("display", "none");
                        }

                    }
                });
            }
            else {
                $("#DefultLoginScreen").css("display", "block");
                $("#DirectlyLoginScreen").css("display", "none");
            }

        }

        function IsaCargoTrackingDomain(domain) {
            const cargoTrackingDomainKeywords = ["tracking.", "ecommerce."];
            for (var i = 0; i < cargoTrackingDomainKeywords.length; i++) {
                if (domain.indexOf(cargoTrackingDomainKeywords[i])>-1) {
                    return true;
                }
            }
            return false;
        }

        function RedirectToCargotrackingSite(domain) {
            //var d = window.location.href + "/CargoTracking";
            window.location.href = window.location.href + "CargoTracking";
        }

    </script>

    <script type="text/javascript"> 
<!-- 
    cookie_name = "email_cookie" // added 
    expdays = 365
    var IsBranding = false;
    var IsPrivateLabel = false;
    var IsShowUpgradeScreen = false;
    var BrandingTenant = "";
    // An adaptation of Dorcht's cookie functions 

    function set_cookie(name, value, expires, path, domain, secure) {

        if (!expires) { expires = new Date() }
        document.cookie = name + "=" + escape(value) +
            ((expires == null) ? "" : "; expires=" + expires.toGMTString()) +
            ((path == null) ? "" : "; path=" + path) +
            ((domain == null) ? "" : "; domain=" + domain) +
            ((secure == null) ? "" : "; secure");
    }

    function get_cookie(name) {

        var arg = name + "=";
        var alen = arg.length;
        var clen = document.cookie.length;
        var i = 0;
        while (i < clen) {
            var j = i + alen;
            if (document.cookie.substring(i, j) == arg) {
                return get_cookie_val(j);
            }
            i = document.cookie.indexOf(" ", i) + 1;
            if (i == 0) break;
        }
        return null;
    }

    function get_cookie_val(offset) {

        var endstr = document.cookie.indexOf(";", offset);
        if (endstr == -1)
            endstr = document.cookie.length;
        return unescape(document.cookie.substring(offset, endstr));
    }

    function delete_cookie(name, path, domain) {

        document.cookie = name + "=" +
            ((path == null) ? "" : "; path=" + path) +
            ((domain == null) ? "" : "; domain=" + domain) +
            "; expires=Thu, 01-Jan-00 00:00:01 GMT";
    }

    // ********** Pass the data to the cookie ********** 
    function save_data_to_cookie() {

        var expdate = new Date();
        expdate.setTime(expdate.getTime() + (expdays * 24 * 60 * 60 * 1000)); // expiry date 

        if ($("#Email").val() == "") { return }

        Data = $("#Email").val();

        set_cookie(cookie_name, Data, expdate)
    }

    // ********** recieve (and reformat) the cookie Data ********** 
    function get_cookie_data() {

        inf = get_cookie(cookie_name)
        if (!inf) { return }
        $("#Email").val(inf);

        get_update_date();
    }

    function get_update_date() {
        var url = window.location.href;
        if (url != "http://system.logitudeworld.com/") {
            $('#updatedate').show();
        }
    }

    // --> 




</script> 
        
    <script type="text/javascript">
        $(document).ready(function () {

            OnLoad();

            var myCode = document.getElementById('PartnerEnvironmentInput').value;
            $("#PartnerImgAtlas").css("display", "none");
            $("#othersTable").css("display", "table");
            $("#pangeaTable").css("display", "none");
            $("#PartnerImg").css("display", "table");


            switch (myCode) {

                case "connecta": {
                    $("#ConnectaDiv").css("display", "table");
                    $("#Partnerdiv").css("display", "table");
                    $("#PartnerImg").attr("src", "images/ApplicationLogo/ConnectaLogo.png");
                    break;
                }

                case "pangea": {
                    $("#pangeaTable").css("display", "table");
                    $("#Partnerdiv").css("display", "table");
                    $("#pangeaDiv").css("display", "table");
                    $("#PartnerImg2").attr("src", "images/ApplicationLogo/PangeaLogo.png");
                    $("#PartnerImg").css("display", "none");
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
                    $("#PartnerImg").css("display", "none");
                    $("#PartnerImgAtlas").css("display", "table");

                    $("#PartnerImgAtlas").attr("src", "images/ApplicationLogo/AtlasAirLogo.png");
                    $("#AtlasDiv").css("display", "table");
                    break;
                }

                default: {
                    $("#LayerImage").css("display", "inline");
                    $("#DirectlyLayerImage").css("display", "inline");

                    $("#imglink").css("display", "inline");
                    break;
                }
            }
        });
    </script>

</body>
</html>
