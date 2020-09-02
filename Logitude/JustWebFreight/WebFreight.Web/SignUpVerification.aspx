<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUpVerification.aspx.cs" Inherits="WebFreight.Web.SignUpVerification" %>

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
        
/*.BusyIndicator
{
    display:none;
     
    background:url("../Images/Icons/Progress.gif") no-repeat;
   
}*/

        body, html
        {
            padding:0;
            margin:0;
            border:0;
            height:100%;
        }

 
    input[disabled="disabled"] {
        color:gray;
  background-image: url(images/LogitudeImages/disabled2Text.png);   
  cursor:default;
  
        }
        body
        {
            min-width: 980px;
            background:url("images/LogitudeImages/map.png") no-repeat 50% 180px;
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

  <link href="HtmlHelpers/Kendo.2013.2.918/kendo.common.min.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/Kendo.2013.2.918/kendo.default.min.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/bootstrap-responsive.min.css" rel="stylesheet" type="text/css"/>    
    <link href="HtmlHelpers/CSS/sunburst.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/app.css" rel="stylesheet" type="text/css"/>
    <link href="HtmlHelpers/CSS/LogitudeMainCss.css" rel="stylesheet" type="text/css"/>
     
    <script src="HtmlHelpers/JS/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/Kendo.2013.2.918/kendo.all.min.js" type="text/javascript"></script>
</head>
<body>

    
    <script src="HtmlHelpers/JS/knockout-2.2.0.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/knockout-kendo.min.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>
    <% if (Simplog.Server.Infrastructure.LogitudeSettings.WorkEnvironment == "customs") {%>
        <script type="text/javascript" src="HtmlHelpers/JS/Amital.GatewayControl.js"></script>
    <%  }%>

    <div>
        <table style="width: 100%; margin-top: -24px; table-layout:fixed;">
            <thead>
                <tr style="height: 140px;">
                    <td></td>
                    <td style="width: 1024px; text-align: center; vertical-align: top;">
                        <a href="http://www.logitudeworld.com/" target="_blank">
                            <img width="290" height="114" style="margin-top: 50px;" src="images/LoginScreen/header.jpg" />
                        </a>
                    </td>
                    <td></td>
                </tr>

            </thead>

            <tbody>
                <tr>
                    <td />
                    <td>
                        <div id="mapBackground">
                            <table style="background-color: transparent; table-layout:fixed;width: 100%;">
                                <tr>

                                    <td></td>



                                    <td style="width: 1140px; text-align: center; vertical-align: top; background-color: transparent">
                                        <table style="width: 100%; margin-top: 0px; background-color: transparent; table-layout:fixed;">


                                            <tr>
                                                <td style="background-color: transparent">
                                                    <div id="MessagesuccessVerified" style="font-family: Lucida Sans Unicode; height: 40px; text-align: left; display: none; font-weight: bold; font-size: 16px; color: steelblue">
                                                        Verification completed successfully: 
                                                  <br />
                                                        <br />
                                                        <p>A username and password will be sent to you via e-mail.</p>
                                                    </div>
                                                    <div id="MessageAlreadyVerified" style="font-family: Lucida Sans Unicode; height: 40px; text-align: left; display: none; font-weight: bold; font-size: 16px; color: steelblue">Your Sign Up Request Is Already Verified </div>
                                                    <table id="verifiedLeadView" style="width: 100%; height: 226px; border-collapse: collapse; border-spacing: 0; background-color: transparent">

                                                        <tr>
                                                            <td style="background-color: transparent">

                                                                <div style="float: left; margin-left: 60px; width: auto" id="myform">
 
                                                                    <table>

                                                                        <tr>
                                                                            <td class="column1" style="font-family: Lucida Sans Unicode; height: 40px; font-weight: bold; font-size: 16px; color: steelblue"></td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2">

                                                                                <table>
                                                                                <%--    Contact Name--%>
                                                                                <tr style="height: 37px;"> 
                                                                                    
                                                                                   <td style="width:8px"> <svg width="8" height="8"> <circle cx="3.2" cy="3.2" r="3.2" stroke="red"  fill="red" id="circleRedContactName" /></svg></td>
                                                                                    <td style ="width:120px;text-align:left;font-family: Myriad Pro; font-size: 14px; color: #4B4A4A">Contact Name</td>  
                                                                                    <td style="text-align:left"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="ContactName"  name="ContactName" runat="server" onKeyUp="onNameChanges()"  /></td>
                                                                                 </tr>
                                                                                        <%--Email Name--%>
                                                                                <tr style="height: 37px;">  
                                                                                        <td style="width:8px"> <span style="width:8px"></span></td>
                                                                                    <td style ="width:120px;text-align:left;font-family: Myriad Pro; font-size: 14px; color: #4B4A4A">Email</td>  
                                                                                    <td style="text-align:left"><input  disabled="disabled" type="email"   autocomplete="on"  size="10"  class="auto-style1" id="Email"  name="Email" runat="server"  /></td>
                                                                                 </tr>
                                                                                 <%--Company--%>
                                                                                 <tr style="height: 37px;">   
                                                                                       <td style="width:8px"> <svg width="8" height="8"> <circle cx="3.2" cy="3.2" r="3.2" stroke="red"  fill="red" id="circleCompanyRead" /></svg></td>
                                                                                    <td style ="width:120px;text-align:left;font-family: Myriad Pro; font-size: 14px; color: #4B4A4A">Company</td>  
                                                                                    <td style="text-align:left"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="CompanyName"  name="Company" runat="server"  onKeyUp="onCompanyChanges()"  /></td>
                                                                                 </tr>

                                                                                      <%--Phone Number --%>
                                                                                 <tr style="height: 37px;">   
                                                                                    <td style="width:8px"> <span style="width:8px"></span></td>
                                                                                    <td style ="width:120px;text-align:left;font-family: Myriad Pro; font-size: 14px; color: #4B4A4A">Phone Number</td>  
                                                                                    <td style="text-align:left"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="PhoneNumber"  name="PhoneNumber" runat="server"  /></td>
                                                                                 </tr>


                                                                                            <%--Number Of Users  --%>
                                                                                 <tr style="height: 37px;">   
                                                                                          <td style="width:8px"> <span style="width:8px"></span></td>
                                                                                    <td style ="width:120px;text-align:left;font-family: Myriad Pro; font-size: 14px; color: #4B4A4A">Number Of Users </td>  
                                                                                    <td style="text-align:left"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="NumberOfUsers"  name="NumberOfUsers" runat="server"  /></td>
                                                                                 </tr>

                                                                                  <%--Request Type  --%>
                                                                                 <tr style="height: 37px;">   
                                                                                        <td style="width:8px"> <span style="width:8px"></span></td>
                                                                                    <td style ="width:120px;text-align:left;font-family: Myriad Pro; font-size: 14px; color: #4B4A4A">Request Type </td>  
                                                                                    <td style="text-align:left"><input disabled="disabled" type="text"   autocomplete="on"  size="10"  class="auto-style1" id="RequestType"  name="RequestType" runat="server"  /></td>
                                                                                 </tr>

                                                                                  <%--Country  --%>
                                                                                    
                                                                                  <tr style="height:37px;">
                                                                                         <td style="width:8px"> <svg width="8" height="8"> <circle cx="3.2" cy="3.2" r="3.2" stroke="red"  fill="red" id="circleCountryRead" /></svg></td>
                                                                                        <td style ="width:120px;text-align:left;font-family: Myriad Pro; font-size: 14px; color: #4B4A4A">Country</td>  
                                                                                          <td style="text-align:left">
                                                                                         <div style="margin-top:2px;margin-bottom:1px;height:31px">
                                                                                           <input type="text" style="width:260px;height: 25px;"  autocomplete="on"  size="10"   id="cmbCountries"  name="Country"   runat="server"   /></div></td>
                                                                                  </tr>

                                                                                    <tr style="height:2px;">
                                                                                       <td style="height:3px" colspan="3"><div style="height:3px"></div></td>
                                                                                  </tr>



                                                                                         <%--Comment  --%>
                                                                             <tr>  
                                                                                    <td style="width:8px"> <span style="width:8px"></span></td>
                           <td style ="width:120px;text-align:left;font-family: Myriad Pro; font-size: 14px; color: #4B4A4A">Note</td>  
                        
                            <td style="text-align:left"><textarea class="auto-style1" style="width:260px; height:71px;resize: none;margin-top:1px;  text-wrap:normal; text-align:initial" id="Comments"   name="Comments" ></textarea></td>
                           
                           </tr>
                 

                                                                                </table>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td class="column1">
                                                                                <%--<asp:Button ID="Button1" runat="server" Text="Submit" Width="94px" OnClick="btnReset_Click" />--%>
                                                                                <input class="cmdSubmit" style="margin-top: 15px" type="button" value="Verify" runat="server" id="Verification" onclick="VerificationMethod()" />
                                                                                <%-- <input class="cmdSubmit" style="margin-top:15px; margin-left:220px"  type="button" value="Edit" runat="server" id="Button1"  onclick="EditMethod()"/></td>--%>
                                                                            </td>
                                                                            <td class="column1">
                                                                                <%--<asp:Button ID="Button1" runat="server" Text="Submit" Width="94px" OnClick="btnReset_Click" />--%>
                                                                 &nbsp;</td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td>
                                                                                <span style="background-color: red" id="errorsSpan">
                                                                                    <p style="color: white; display: none; text-align: left; background-color: red; margin-top: 3px; width: 268px" id="messageError">Save failed please Enter the required Field</p>
                                                                                </span>

                                                                            </td>
                                                                        </tr>

                                                                        <tr style="height: 40px;">
                                                                            <td>
                                                                                <p style="color: SteelBlue; text-align: left; display: none;" id="message">verified completed successfully</p>
                                                                            </td>
                                                                        </tr>


                                                                        <tr>
                                                                            <td>

                                                                                <div style="display: none; margin-top: -50px" id="busyIndicator">
                                                                                    <img width="100" height="100" src="HtmlHelpers/Images/Icons/Progress.gif" alt='loading' /></div>
                                                                                <%--<p id="busyIndicator" style="display:none"><img width="50" height="50" src="images/LoginScreen/indicator.gif" alt='loading' /></p>--%>

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



    </div>


    <script type="text/javascript">
        var Id;
        var LogitudeLeadId;
        var PhoneNumber;
        var CompanyName;
        var ContactName;
        var Country;
        var Email;
        var NumberOfUsers;
        var Comments;
        var RequestType;
        var IsEmailVerified;
        var TenantNumber;

       function LogitudeLoadPM() {
           
           this.Id = LogitudeLeadId;
           this.PhoneNumber = PhoneNumber;
           this.CompanyName = CompanyName;
           this.ContactName = ContactName;
           this.Country = Country;
           this.Email = Email;
           this.NumberOfUsers = NumberOfUsers;
           this.Comments = Comments;
           this.IsEmailVerified = IsEmailVerified;
           this.TenantNumber = TenantNumber;
     

          
       };
        
       function VerificationMethod() {

           Email = document.getElementById('Email').value;
           ContactName = document.getElementById('ContactName').value;
           CompanyName = document.getElementById('CompanyName').value;
           PhoneNumber = document.getElementById('PhoneNumber').value;
         
           NumberOfUsers = document.getElementById('NumberOfUsers').value;
           RequestType = document.getElementById('RequestType').value;
           Comments = document.getElementById('Comments').value;

           var validatable = $("#myform").kendoValidator().data("kendoValidator");
           validatable.validate();
           

           var errorMessage = "";

       
           if (!ContactName || (ContactName && !ContactName.trim())) {
               errorMessage += "<p>Please fill in the Contact Name field.</p>";

           }
           if (!CompanyName || (CompanyName && !CompanyName.trim())) {
               errorMessage += "<p>Please fill in the Company Name field.</p>";

           }
           if (!Country || (Country && !Country.trim())) {
               errorMessage += "<p>Please fill in the Country field.</p>";
           }

           if (errorMessage != "") {
               document.getElementById("messageError").innerHTML = errorMessage;
               $("#messageError").show();
               return;
           }

           disableForm(true);
           $("#busyIndicator").show();

           IsEmailVerified = true;

           var logitudeLoadpm = new LogitudeLoadPM();
           var url = "api/LogitudeLeadVerification";

           $.ajax({
               url: url,
               data: JSON.stringify(logitudeLoadpm),
               type: 'POST',
               contentType: 'application/json',
               success: function (result) {
                   $("#MessagesuccessVerified").show();
                   $("#verifiedLeadView").hide();
                   //disableForm(true);

                   $("#busyIndicator").hide();
               },

               error: function (jqXHR, textStatus, errorThrown) {
              
                   $("#MessagesuccessVerified").hide();
                   $("#verifiedLeadView").show();
                   $("#busyIndicator").hide();
                   disableForm(false);

                   document.getElementById("messageError").innerHTML = "Save failed! Connection to the server failed.";
                   $("#messageError").show();

               }
           });




       }

       function disableForm(disable) {
           if (disable) {
               $("cmdSubmit").prop('disabled', true);
               $("cmbCountries").prop('disabled', true);
           }
           else {
               $("cmdSubmit").prop('disabled', false);
               $("cmbCountries").prop('disabled', false);
           }
       }

       function EditMethod() {

           var id = getURLParameter("id");

           if (id !== "null") {
               document.location.href = "SignUp.aspx?id=" + id;
           }
       }

       jQuery.LoadLead = (function (id) {

           $("#busyIndicator").show();
           var url = "api/LogitudeLeadVerification?id=" + id;
           $.ajax({
               url: url,
               type: 'Get',
               contentType: 'application/json',
               success: function (result) {
                   $("#busyIndicator").hide();
                   if (result != null) {
                         
                       LogitudeLeadId = result.Id;
                       PhoneNumber = result.PhoneNumber;
                       CompanyName = result.CompanyName;
                       ContactName = result.ContactName;
                       Country = result.Country;
                       Email = result.Email;
                       NumberOfUsers = result.NumberOfUsers;
                       Comments = result.Comments;
                       RequestType = result.RequestType;
                       IsEmailVerified = result.IsEmailVerified;
                       TenantNumber = result.TenantNumber;
             
                       if (result.IsEmailVerified == true) {
                           $("#MessageAlreadyVerified").show();
                           $("#verifiedLeadView").hide();

                       }
                       else {

                           $("#MessageAlreadyVerified").hide();

                           $("#verifiedLeadView").show();
                           $("#Email").val(result.Email);
                           $("#ContactName").val(result.ContactName);

                           $("#CompanyName").val(result.CompanyName);
                           $("#NumberOfUsers").val(result.NumberOfUsers);

                           $("#RequestType").val(result.RequestType);

                           $("#PhoneNumber").val(result.PhoneNumber);

                           $("#Comments").val(result.Comments);
                           $("#cmbCountries").val(result.Country);
                           
                          
                           if (!result.Country) {
                               $.LoadCountries();
                           }
                           else {

                               $("#cmbCountries").prop('disabled', true);
                               
                           }

                       }
                   }


                   if (ContactName)
                   {
                       $("#circleRedContactName").hide();
                   }

                   if (CompanyName) {
                       $("#circleCompanyRead").hide();
                   }

                   if (Country) {
                       $("#circleCountryRead").hide();
                   }

        
               },
               error: function (jqXHR, textStatus, errorThrown) {
                   $("#busyIndicator").hide();

                   document.getElementById("messageError").innerHTML = "Loading failed! Connection to the server failed.";
                   $("#messageError").show();
               }
           });

       })

       jQuery.LoadCountries = (function () {
            
           var url = "api/Country?bycrmTenant=true" + "&tenant=" + 0;
           $.ajax({
               url: url,
               type: 'Get',
               contentType: 'application/json',
               success: function (result) {

                   if (result != null) {

                       $("#cmbCountries").kendoComboBox(
                       {
                           dataTextField: "EnglishName",
                           dataValueField: "EnglishName",
                           placeholder: "Choose your Country",
                           change: onChange,
                           filter: "contains",
                           suggest: false,

                           dataSource:
                           {
                               data: result

                           }
                           //template: kendo.template($("#partnerTemplate").html())
                       });


                   }
               },
               error: function (jqXHR, textStatus, errorThrown) {

                   document.getElementById("messageError").innerHTML = "Loading failed! Connection to the server failed.";
                   $("#messageError").show();

               }
           });

       })

       function onChange() {

            
           var combobox = $("#cmbCountries").data("kendoComboBox");
           var selectedItem = combobox.dataSource.view()[combobox._current.index()];
           selectedcompany = selectedItem;
           var CurrentCountry = $('#cmbCountries').data('kendoComboBox').value();
           Country = CurrentCountry;

           if (Country.trim()) { $("#circleCountryRead").hide(); }
           else { $("#circleCountryRead").show(); }


           // var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType;

       }





       function onCompanyChanges() {
           if ($("#CompanyName").val().trim()) { $("#circleCompanyRead").hide(); }
           else { $("#circleCompanyRead").show(); }
       }


  
       function onNameChanges() {

           if ($("#ContactName").val().trim()) {

               $("#circleRedContactName").hide();
           }
           else { $("#circleRedContactName").show(); }
       }



       function getURLParameter(name) {
           return decodeURI(
               (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
           );
       }

       $(document).ready(function () {
           
           var id = getURLParameter("id");
           LogitudeLeadId = getURLParameter("id");
           if (id) {
               $.LoadLead(id);
           }

       });


       </script>
</body>
</html>
