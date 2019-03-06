<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsvLoginUC.ascx.cs" Inherits="WebFreight.Web.LogInUserControls.DsvLoginUC" %>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=11">
    <link id="logolink" rel="shortcut icon" />
    <title></title>

    <script src="HtmlHelpers/JS/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/Logitude.Tools.js" type="text/javascript"></script>

    <style type="text/css">

                       
.cmdSubmit {
	-moz-box-shadow:inset 0px 1px 0px 0px #caefab;
	-webkit-box-shadow:inset 0px 1px 0px 0px #caefab;
	box-shadow:inset 0px 1px 0px 0px #caefab;
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #77d42a), color-stop(1, #5cb811) );
	background:-moz-linear-gradient( center top, #77d42a 5%, #5cb811 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#77d42a', endColorstr='#5cb811');
	background-color:#77d42a;
	-moz-border-radius:6px;
	-webkit-border-radius:6px;
	border-radius:6px;
	border:1px solid #268a16;
	display:inline-block;
	 color: #ffffff;
	font-family:arial;
	font-size:15px;
	font-weight:bold;
	padding:6px 24px;
	text-decoration:none;
	text-shadow:1px 1px 0px #aade7c;
     width: 100px;
     margin-top:2px;
}.cmdSubmit:hover {
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #5cb811), color-stop(1, #77d42a) );
	background:-moz-linear-gradient( center top, #5cb811 5%, #77d42a 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#5cb811', endColorstr='#77d42a');
	background-color:#5cb811;
}.cmdSubmit:active {
	position:relative;
	top:1px;
}

        .loginButton
        {
            -moz-box-shadow: inset 0px 1px 0px 0px #f29c93;
            -webkit-box-shadow: inset 0px 1px 0px 0px #f29c93;
            box-shadow: inset 0px 1px 0px 0px #f29c93;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #fe1a00), color-stop(1, #cc0029) );
            background: -moz-linear-gradient( center top, #fe1a00 5%, #cc0029 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#fe1a00', endColorstr='#cc0029');
            background-color: #fe1a00;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #d83526;
            display: inline-block;
            color: #ffffff;
            font-family: arial;
            font-size: 15px;
            font-weight: bold;
            padding: 6px 24px;
            text-decoration: none;
            text-shadow: 1px 1px 0px #b03466;
            width: 100px;
        }

            .loginButton:hover
            {
                background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #cc0029), color-stop(1, #fe1a00) );
                background: -moz-linear-gradient( center top, #cc0029 5%, #fe1a00 100% );
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#cc0029', endColorstr='#fe1a00');
                background-color: #cc0029;
            }

            .loginButton:active
            {
                position: relative;
                top: 1px;
            }



   
     .promptButton {
	display: inline-block;
	outline: none;
	cursor: pointer;
	text-align: center;
	text-decoration: none;
	font: 14px/100% Arial, Helvetica, sans-serif;
	padding: .5em 2em .55em;
	text-shadow: 0 1px 1px rgba(0,0,0,.3);
	-webkit-border-radius: .5em; 
	-moz-border-radius: .5em;
	border-radius: .5em;
	-webkit-box-shadow: 0 1px 2px rgba(0,0,0,.2);
	-moz-box-shadow: 0 1px 2px rgba(0,0,0,.2);
	box-shadow: 0 1px 2px rgba(0,0,0,.2);
}
.promptButton:hover {
	text-decoration: none;
}
.promptButton:active {
	position: relative;
	top: 1px;
}
            
    </style>

    <style type="text/css">
        body, html
        {
            padding: 0;
            margin: 0;
            border: 0;
            height: 100%;
        }

        body
        {
            min-width: 980px;
            background: url("images/LoginScreen/map.png") no-repeat 50% 180px;
        }

        #mapBackground
        {
            background-image: url("images/LoginScreen/map.png");
            text-align: center;
            background-repeat: no-repeat;
            background-size: 100%;
            width: 907px;
            height: 466px;
            margin-top: 15px;
            background-position: center;
        }



        .auto-style1
        {
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

        .column1
        {
            text-align: left;
            width: 300px;
            font-family: "Myriad Pro";
            font-size: 14px;
            color: #4B4A4A;
        }

        span.k-icon.k-i-arrow-s {
            background-image: url('HtmlHelpers/Images/Icons/DropArrow.png');
            background-size: 12px 12px;
            background-position: 0 0;
        }
        /*
        #cmbTenants-list .k-item
        {
            background:transparent;
            color: black;
            border:0px;
        }

        #cmbTenants-list .k-item:hover
        {
            background:gray;
            color: white;
        } 
    */
    </style>

    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/Kendo.2013.2.918/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css"/>    
    <link href="../HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css"/>
    <link href="../HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css"/>
     
    <!--  AmitalBrowserWpfApplication\Views\GatewayUserControl.cs GOOD (Itzik )-->        	
    
    <script src="Scripts/json2.min.js" type="text/javascript"></script>
<%--	<script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="http://cdn.kendostatic.com/2012.1.322/js/kendo.all.min.js" type="text/javascript"></script>--%>

    <!--  AmitalBrowserWpfApplication\Views\GatewayUserControl.cs Bad (Itzik )-->        	
   
     
    <script type="text/javascript" src="../HtmlHelpers/JS/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="../HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js"></script>
</head>

<body onload="get_cookie_data()" onkeydown="capLock( event )">
    
    <script src="../HtmlHelpers/JS/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/knockout-kendo.min.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
    <script src="../HtmlHelpers/JS/app.js" type="text/javascript"></script>
    <% if (Simplog.Server.Infrastructure.LogitudeSettings.WorkEnvironment == "customs") {%>
        <script type="text/javascript" src="../HtmlHelpers/JS/Amital.GatewayControl.js"></script>
    <%  }%>

    <div id="Container">

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
                
                    <td style="width:1140px; text-align:center; vertical-align:top;">
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
                                            <td class="column1">e-mail: 
                                                <br /> 
                                                                <input autocomplete="on"  size="10"  class="auto-style1" id="Email" type="email" name="Email" runat="server"  placeholder="e.g. myname@example.net"  required data-email-msg="Email format is not valid"  onblur="onEmailBlur()"/>
                                                     
                                                        <%-- <img id="busyIndicator" style="height:25px;width:25px;vertical-align:bottom;display:none" src="images/LoginScreen/indicator.gif" alt='loading' />--%>
                                            </td>
                                               
                                            
                                        </tr>
                                        

                                        <tr>
                                                            <td class="column1">Password: <br /><input class="auto-style1" id="Password" type="password" autocomplete="off" runat="server" oninput="onPasswordChanged()" required data-email-msg="password is required!" onkeypress="capLock(event)"/>
                                                            </td>                    
                                        </tr>

                                          
                                              <tr>
                                                 <td  class="column1">
                                                   
                                            <div id="divMayus" style="visibility:hidden">
                                                <img width="15" height="15" src="images/SimplogIcons/warning.png" /> Caps Lock is on. 
                                                
                                            </div> 
                                            </td>
                                            </tr>
                                                
                                              <tr>
                                            <td  class="column1"> 
                                                <input class="loginButton" type="submit" value="Login >" runat="server" id="cmdLogin" data-bind="click: validateMethod" formmethod="post"/>
                                                <a style="margin-left:15px;color:#4B4A4A;font-size:12px;font-family:Arial; cursor:pointer;" onclick="resetpasswordclick()">Forgot your password?</a>
                                            </td>
                                        </tr>
                                              
                                              <tr>
                                                <td class="column1">
                                                      <span style="background-color:red">
                                                       <p style="color:white;display:none;text-align:left;background-color:red;margin-top:3px;width:268px" id="errorsList">Login failed! invalid e-mail or password.</p>

                                                    </span>
                                                </td>
                                            </tr>
                                            
                                              <tr style="height:50px;width:50px">
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
                                                                <div style="margin-top:5px"><input class="k-dropdown" id="cmbTenants" runat="server" style="display:normal;width:250px;margin-top:0px"/></div>
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
                                       
                                             </table>

                                    </div>

                                           <%--     abed--%>
                                    <div id="PromptView" style="float: left;display:none;width:300px;background-color:white;height:100%;vertical-align:central;">
                                      
               

             <div style="font-size:17px;color:SteelBlue;margin-top:40px;height:40px"><b>Please select user interface</b></div>
             
                   <div style="font-size:16px;color:black;margin-top:60px"></div>
                            <div style="font-size:16px;color:black;margin-bottom:20px;margin-top:3px"> </div>

                  <table  style="display:normal;vertical-align:bottom;text-align:center;width:100%">
   
                    <tr >
                     <td>  <input class="promptButton"   style="margin-left:0px;"  type="submit" value="HTML5"   data-bind="click: promptButtonYes" formmethod="post"/></td>

                    <td>  <input    class="promptButton" type="submit" value="Silverlight" runat="server" id="Submit2" data-bind="click: promptButtonNo" formmethod="post"/></td>
                                          </tr>
                </table>

                                       
                                        <div style="height:50px"></div>

      <%--       <div style="height:100%">
                 <table border="0">
                     <tr><td colspan="2" style="height:10px"></td></tr>
                     <tr>
                         <td> <input style="text-align:left;margin-left:15px;" id="setAngularasDefaultId" onchange="SetAngularasDefaultCheckBoxChange()" type="checkbox"  name="vehicle" /></td>
                         <td  style="font-size:13px"> Set angular as default</td>
                         <td style="width:120px"></td>
                     </tr>
                                             </table>

            

             </div>--%>
                                       
                                    </div>
                                                   



<%--//Abed prompt--%>




                                                   
                                </td>









                                            <td style=" width:2px; text-align:right;border:0;"><img src="images/LoginScreen/line.png" style="width:2px;height:260px;margin-right:-3px;border:thick"/></td>
                                            <td style="width:639px;">
                                                
                                                <img id="LayerImage" width="650" style="display:none; height:260px;min-width:650px;width:650px;border:0" src="images/LoginScreen/Layer.png"/>
                                                
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



        window.history.forward();







        var companyList;
        var password;
        var email;
        var currentTenant;
        var Technology = null;//"Angular";
        var SetAngularCheckBoxValue = false;
            
        var UserDataPrompt = null;
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

        function setCaretToPos(id, cursorPosition) {
            document.getElementById(id).selectionStart = cursorPosition;
            document.getElementById(id).selectionEnd = cursorPosition;
        }
        //data-bind="event: {blur: getContacts}"
        onPasswordChanged = function () {
             
            $("#errorsList").hide();
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

                   // var cursorPosition = document.getElementById("Email").selectionStart;
                    $("#Email").val($.trim(emailstring));

                    //setCaretToPos("Email", cursorPosition);
                    //$("#Email").sele
                    //setCaretToPos($("#Email"), cursorPosition);

                   
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

                    var combobox = $("#cmbTenants").data("kendoComboBox");
                    var selectedItem = combobox.dataSource.view()[combobox._current.index()];
                    selectedcompany = selectedItem;
                    currentTenant = $('#cmbTenants').data('kendoComboBox').value();
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
                    //$("#busyIndicator").hide();
                }



                this.validateMethod = function () {

                    password = $("#Password").val();
                    email = $("#Email").val();
                    var persist = false;
                    //var tenant = $("#cmdTenant").val();
                    // get the dataItem corresponding to the selectedIndex.
                    //var dataItem = combobox.dataItem();
                    //var combobox = $("#cmbTenants").data("kendoComboBox");
                    //$("#cmdTenant").val(tenant);
                    // currentTenant, string email, string password, bool persistCookie
                   

                
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
                  
                    
                 
                    save_data_to_cookie();
                    disableForm(true);
                     
                    $("#loginBusyindicator").show();

                    var url = "api/authentication";//?email=" + email + "&password=" + password;//+ "&persistCookie=" + persist;
                    function LoginParameters() {

                        this.Email = $.trim(email.toLowerCase());
                        this.Password = password;
                        this.GetToken = true;
                    };


 
                    var param = new LoginParameters();

                    $.ajax({
                        url: url,
                        type: 'POST',
                        data: JSON.stringify(param),
                        contentType: 'application/json',

                        success: function (userdata) {
                            
                            disableForm(false);
                            UserDataPrompt = userdata;

                            if (!userdata.HasError) {


                                logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType;
                                if (userdata.ContactsCount == 1) {
                                    Technology = userdata.Technology;
                                    if (userdata.IsUser) {
                                        if (Technology != "PR") {
                                            if (Technology && Technology == "AG") LoginToAngular(userdata);
                                            else {
                                                LoginToSliverLight(userdata);
                                                $("#loginBusyindicator").hide();
                                            }
                                        }
                                        else {


                                            document.getElementById("PromptView").style.display = "block";
                                            document.getElementById("myform").style.display = "none";
                                        }
                                    }
                                    else
                                    {
                                        // ShardLogistics
                                        LoginToShardLogistics(userdata);
                                    }
                                }
                                else {

                                    showTenantsCombo(userdata.CompanyLogins);
                                }

                                }
                            else {

                                $("#loginBusyindicator").hide();

                                if (userdata.ExceptionMessage) {

                                    alert(userdata.ExceptionMessage);
                            }
                            else {

                                if (userdata.MustChangePassword) {

                                    document.location.href = "PasswordChangePage.aspx?email=" + email
                                }
                                else {
                                    var errorMessage = "Login failed! invalid user name or password." + "<br/>";


                                    if (userdata.IpRestricted) {

                                        errorMessage = "Trying to log in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "log in from is restricted for this user)";//

                                    }

                                    if (userdata.IsLocked) {

                                        errorMessage = "Your account has been locked out!" + "<br/>" + "please try again after 30 minutes.";
                                    }
                                    if (userdata.InActive) {
                                        errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
                                    }
                                    if (userdata.Unlicensed) {

                                        errorMessage = "Your account is unlicensed!" + "<br/>" + "please contact your administrator.";
                                    }
                                    

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


            };


            LoginToSliverLight = function (userdata) {

                Technology = "SL";
       
                 var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType + ":"+userdata.IsBrandingEnabled;
                 logindata = UserDataPrompt.UserName + ":" + UserDataPrompt.Id + ":" + UserDataPrompt.CurrentTenant;
                 document.location.href = "default.aspx?userdata=" + logindata;
                 $("#loginBusyindicator").hide();
 
            };



            LoginToShardLogistics = function (userdata) {
                var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType + ":" + userdata.IsBrandingEnabled;
                document.location.href = "SharedLogisticPage.aspx?userdata=" + logindata;
                $("#loginBusyindicator").hide();
            };






            LoginToAngular = function (userdata) {

                var isTenantAllowed = false;
                var Tenant = userdata.CurrentTenant;
                if (Tenant == 42 || Tenant == 1232 || Tenant == 1586) {
                    isTenantAllowed = true;
                }

                var url = "api/LogitudeApplication"
                $.ajax({
                    url: url,
                    type: 'GET',
                    contentType: 'application/json',

                    success: function (result) {
                        if (result == true) {
                            document.location.href = "WebPages/UpgradeScreen.aspx";
                        }
                        else {

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


                                //var safari = navigator.sayswho.split('Safari')[1];
                                //var safariversionstring = safari.trim()
                                //if (safariversionstring) {
                                //    var safariversion = Number(safariversionstring);
                                //    // Chrome
                                //    if (safariversion < 10) {
                                //        alert("The current version of safari doesn't support HTML5 ,Please upgrade to version 10 or higher");
                                //        return;
                                //    }
                                //}
                            }



                            var data = JSON.stringify(userdata);
                            window.sessionStorage.setItem("userdata", data);
                            var version = "";
                            if (userdata.HtmlVersion) version = userdata.HtmlVersion;
                            document.location.href = "Angular" + version + "/index.html";

                            $("#loginBusyindicator").hide();

                        }
                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                     
                    }
                });


            
            };



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

                };

                var param = new LoginParameters();


                $.ajax({
                    url: url,
                    type: 'POST',
                    data: JSON.stringify(param),
                    contentType: 'application/json',

                    success: function (userdata) {

                        disableForm(false);
                        $("#loginBusyindicator").hide();

                        if (!userdata.HasError){

                            if(!Technology) Technology = userdata.Technology;

                            UserDataPrompt = userdata;

                            if (userdata.IsUser) {
                                if (Technology != "PR") {

                                    if (Technology && Technology == "AG") LoginToAngular(userdata);
                                    else LoginToSliverLight(userdata);
                                }

                                else {
                                    document.getElementById("PromptView").style.display = "block";
                                    document.getElementById("myform").style.display = "none";
                                }

                            }
                            else
                            {
                                // ShardLogistics
                                LoginToShardLogistics(userdata);
                            }

                        }
                        else {

                            if (userdata.ExceptionMessage) {

                                alert(userdata.ExceptionMessage);
                        }

                         else  if (userdata.MustChangePassword) {

                                document.location.href = "PasswordChangePage.aspx?email=" + email
                            }
                            else {
                                var errorMessage = "Login failed! invalid user name or password." + "<br/>";


                                if (userdata.IpRestricted) {

                                    errorMessage = "Trying to log in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "log in from is restricted for this user)";//

                                }

                                if (userdata.IsLocked) {

                                    errorMessage = "Your account has been locked out!" + "<br/>" + "please try again after 30 minutes.";
                                }

                                document.getElementById("errorsList").innerHTML = errorMessage;
                                // $("#errorsList").text(errorMessage);
                                $("#errorsList").show();
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

            $(document).ready(function () {
                window.sessionStorage.setItem("IsBranding", false);
                window.sessionStorage.setItem("IsPrivateLabel", false);
                window.sessionStorage.setItem("ContactEmail","");
                window.sessionStorage.setItem("loginlogo", "");
                var hash = $(location).attr('href');
                var domain = hash.split('/')[2];
                //alert(hash.split('/')[2]);
                if (hash) {
                    var hashSplit = hash.toLowerCase().split("tenant");
                    if (hashSplit) {
                        var Key = hashSplit[1];
                        if (hashSplit[1]) {
                            BrandingTenant = hashSplit[1].split('=')[1];
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
                //else LoadLogo();
         
            
                _LoginViewModel = new LoginViewModel();
                ko.applyBindings(_LoginViewModel);

                $("#Password").keyup(function (e) {
                    if (e.which == 13) {
                        _LoginViewModel.validateMethod();
                    }
                });
            });


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
                    },
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
                        },
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
                        },
                });

                   
                }
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


            function SetAngularasDefaultCheckBoxChange()
            {
                var result = document.getElementById('setAngularasDefaultId').value;
                if (result == "on") SetAngularCheckBoxValue = true;
            }
                
    </script>

    <script type="text/javascript">
        document.onkeypress = capLock;

        function capLock(e) {
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
</script>

    <script type="text/javascript"> 
<!-- 
    cookie_name = "email_cookie" // added 
    expdays = 365
    var IsBranding = false;
    var IsPrivateLabel = false;
    
    var BrandingTenant ="";
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

    function get_update_date()
    {
        var url = window.location.href;
        if (url != "http://system.logitudeworld.com/")
        {
            $('#updatedate').show();
        }
    }

    // --> 




</script> 
        
    <script type="text/javascript">
        $(document).ready(function () {

       

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
                    $("#imglink").css("display", "inline");
                    break;
                }
            }
        });
    </script>

</body>
</html>
