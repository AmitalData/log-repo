<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordResetRequestPage.aspx.cs" Inherits="WebFreight.Web.PasswordResetRequestPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

   <%-- <style type="text/css">
        p
        {
            color:#4B4A4A;
            font-size:12px;
            font-family:Arial
        }
    </style>--%>


    <style type="text/css">
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



        .auto-style1
        {
   background: #dbdbdb; /* Old browsers */
/* IE9 SVG, needs conditional override of 'filter' to 'none' */
background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2RiZGJkYiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjI4JSIgc3RvcC1jb2xvcj0iI2YyZjJmMiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjQxJSIgc3RvcC1jb2xvcj0iI2ZmZmZmZiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmZmZmZmYiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
background: -moz-linear-gradient(top,  #dbdbdb 0%, #f2f2f2 28%, #ffffff 41%, #ffffff 100%); /* FF3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#dbdbdb), color-stop(28%,#f2f2f2), color-stop(41%,#ffffff), color-stop(100%,#ffffff)); /* Chrome,Safari4+ */
background: -webkit-linear-gradient(top,  #dbdbdb 0%,#f2f2f2 28%,#ffffff 41%,#ffffff 100%); /* Chrome10+,Safari5.1+ */
background: -o-linear-gradient(top,  #dbdbdb 0%,#f2f2f2 28%,#ffffff 41%,#ffffff 100%); /* Opera 11.10+ */
background: -ms-linear-gradient(top,  #dbdbdb 0%,#f2f2f2 28%,#ffffff 41%,#ffffff 100%); /* IE10+ */
background: linear-gradient(to bottom,  #dbdbdb 0%,#f2f2f2 28%,#ffffff 41%,#ffffff 100%); /* W3C */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#dbdbdb', endColorstr='#ffffff',GradientType=0 ); /* IE6-8 */





            font-family: tahoma, arial, sans-serif;
            width: 260px;
            height: 22px;
            font-size: 13px;
        }

        .column1
        {
            text-align:left;
            width:100%;
           font-family:"Myriad Pro";
           font-size:14px;
           color:#4B4A4A;
        }

        
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
 
     
    </style>

    <title></title>

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
</head>
<body>

    
    <script src="HtmlHelpers/JS/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/knockout-kendo.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>

     <div id="Container">      
 
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
                     <table style="background-color:transparent"> 
            <tr>
              
                    <td></td>  

                
                
                    <td style="width:1140px; text-align:center; vertical-align:top;background-color:transparent">
                        <table style="width:100%; margin-top:0px;background-color:transparent">
                          

                            <tr>
                                <td style="background-color:transparent">
                                    <table style="width:100%; height:226px; border-collapse:collapse; border-spacing:0;background-color:transparent ">

                            <tr>
                                            <td style="background-color:transparent">
                                    <div style="float: left;margin-left:60px;width:auto" id="myform">
                                      
                                        <table >

                                                         <tr>
                                                              <td class="column1" style="font-family:Lucida Sans Unicode; font-weight:bold; font-size:16px; color:steelblue"> 
                                                                    Sign up for a free 7-day trial
 
                                                                  
                                                              </td>  
                                                         </tr>

                                                         <tr>
                                                              <td class="column1" style="font-family:Arial; font-size:12px; color:#4B4A4A"> 
                                                                  Please fill in all fields 
                                                              </td>         
                                                         </tr>

                                                       <tr>
                                                              <td class="column1" style="font-family:Myriad Pro; font-size:14px; color:#4B4A4A"> 
                                                                  Name  <input autocomplete="on" style="margin-left:32px"  size="10"  class="auto-style1" id="Name"  name="Name" runat="server"   required data-email-msg="Name field is required"/>

                                                              </td> 
                                                             <td>
                                                                
                                                             </td>
                                                             
                                                         </tr>
                                                         <tr>
                                                              <td class="column1" style="font-family:Myriad Pro; font-size:14px; color:#4B4A4A"> 
                                                                  Email  <input autocomplete="on" style="margin-left:35px"  size="10"  class="auto-style1" id="Email" type="email" name="Email" runat="server"   placeholder="e.g. myname@example.net"  required data-email-msg="Email format is not valid" oninput="onEmailChanges()"/>

                                                              </td> 
                                                             <td>
                                                                
                                                             </td>
                                                             
                                                         </tr>

                                                       <tr>
                                                              <td class="column1" style="font-family:Myriad Pro; font-size:14px; color:#4B4A4A"> 
                                                                  Company  <input autocomplete="on" style="margin-left:10px"  size="10"  class="auto-style1" id="Company"  name="Company" runat="server"   required data-email-msg="Company field is required" />

                                                              </td> 
                                                             <td>
                                                                
                                                             </td>
                                                             
                                                         </tr>

                                                      

                                                         <tr>
                                                             <td class="column1">
                                                                  <%--<asp:Button ID="Button1" runat="server" Text="Submit" Width="94px" OnClick="btnReset_Click" />--%>
                                                                 <input class="cmdSubmit" style="margin-top:15px" type="submit" value="Submit >" runat="server" id="cmdSubmit" data-bind="click: submitMethod"/>
                                                                  <a style="margin-left:10px;color:#4B4A4A;font-size:12px;font-family:Arial;vertical-align:central;display:none" href="login.aspx" >Back to login page</a>
                                                             </td>
                                                         </tr>

                                                         <tr></tr>

                                                         <tr style="vertical-align:bottom">
                                                             <td >
                                                                  <p style="font-family:Arial; font-size:12px; color:#4B4A4A" class="column1"> 
                                                                         Having trouble logging in? 
                                                                         <a href="mailto:info@logitudeworld.com" >Contact us</a>  
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
                                                                  <p style="color:SteelBlue;display:none;text-align:left" id="message">Submiting completed successfully.<br /> You should shortly receive an email with your user name and password information.</p>
                                                             </td>
                                                         </tr> 
                                            
                                                         <tr>
                                                             <td>
                                                                  <p id="busyIndicator" style="display:none"><img width="50" height="50" src="images/LoginScreen/indicator.gif" alt='loading' /></p>

                                                             </td>
                                                         </tr>                                                           

                                                     </table>

                                    </div>
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

   <%--     <input id="cmdTenant" runat="server" type="hidden"/>--%>

       <%-- <asp:Label id="lblMsg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat="server" />
     --%>

        </div>
 

    <script type="text/javascript">

       
        function setCaretToPos(id, cursorPosition) {
            document.getElementById(id).selectionStart = cursorPosition;
            document.getElementById(id).selectionEnd = cursorPosition;
        }
        function onEmailChanges() {
            var emailstring = $("#Email").val();
            if (emailstring) {

                var cursorPosition = document.getElementById("Email").selectionStart;

                $("#Email").val($.trim(emailstring));

                setCaretToPos("Email", cursorPosition);
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

        function viewModel() {

            this.submitMethod = function () {

                var validatable = $("#myform").kendoValidator().data("kendoValidator");

                if (validatable.validate() === false) {
                    // get the errors and write them out to the "errors" html container
                    var errors = validatable.errors();
                    $(errors).each(function () {
                        $("#errors").html(this);
                    });
                    return;
                }

                disableForm(true);
                $("#busyIndicator").show();
                var name = $("#Name").val();
                var email = $("#Email").val();
                var company = $("#Company").val();

                 
                var url = "api/Authentication/?name=" + name + "&email=" + email + "&company=" + company;

                $.ajax({
                    url: url,
                    type: 'POST',
                    contentType: 'application/json',

                    success: function (userdata) {
                         

                        $("#busyIndicator").hide();

                        if (!userdata.HasError) {

                            //string logindata = user.UserName + ":" + user.Id + ":" + user.CurrentTenant + ":" + computerId + ":" + user.IsAuthenticated;

                            $("#message").show();
                            //alert("submit completed");
                            //document.location.href = "login.aspx";

                        }
                        else {

                            disableForm(false);
                            var errorMessage = "Submit failed! invalid email." + "<br/>";


                            if (userdata.IpRestricted) {

                                errorMessage = "Trying to submit in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "submit from is restricted for this user)";//

                            }

                            if (userdata.IsLocked) {

                                errorMessage = "Your account has been locked out!" + "<br/>" + "please contact your administrator.";
                            }

                            document.getElementById("errorsList").innerHTML = errorMessage;
                            // $("#errorsList").text(errorMessage);
                            $("#errorsList").show();

                        }


                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                         debugger
                        disableForm(false);
                        $("#error").text("errror");
                        $("#error").show();
                        $("#loginBusyindicator").hide();
                        var errorMessage = '';
                    }
                });
            }
        }



        function getURLParameter(name) {
            return decodeURI(
                (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
            );
        }

        $(document).ready(function () {
             
            ko.applyBindings(new viewModel());
        });



    </script>
</body>
</html>
