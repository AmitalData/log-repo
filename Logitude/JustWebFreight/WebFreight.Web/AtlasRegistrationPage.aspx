<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AtlasRegistrationPage.aspx.cs" Inherits="WebFreight.Web.AtlasRegistrationPage" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

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
background: url('data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iI2RiZGJkYiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjI4JSIgc3RvcC1jb2xvcj0iI2YyZjJmMiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjQxJSIgc3RvcC1jb2xvcj0iI2ZmZmZmZiIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiNmZmZmZmYiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+'); /* W3C */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#dbdbdb', endColorstr='#ffffff',GradientType=0 ); /* IE6-8 */





            font-family: tahoma, arial, sans-serif;
            width: 273px;
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

     
        .auto-style2 {
            text-align: left;
            width: 100%;
            font-family: "Myriad Pro";
            font-size: 14px;
            color: #4B4A4A;
            height: 3px;
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
    <script src="HtmlHelpers/JS/Amital.GatewayControl.js" type="text/javascript"></script>
          
     <div id="Container">      

         <div style="display:block">

  <div style="width:300px; display:inline-block;height:150px;margin-top:70px;margin-left:80px">
             <img id="loginlogo" width="299" style="margin-top:0px;margin-bottom:10px;margin-left:0px;height: 152px" src="images/ApplicationLogo/AtlasAirLogo.png" />
              <div  style="font-family:Lucida Sans Unicode; font-weight:bold;height:23px; color:#0094FF"> <p style="display:inline; font-size:16px;"> Sign up  </p> </div> 
        
              <div style="font-family:LucidaSans Unicode;height:30px;text-align:left; font-size:11px; color:black"> <p style="margin-left:25px;font-size:11px;"> Please fill all the mandatory fields  </p> </div>  
              </div>

             <div style="width:300px;display:inline-block;height:152px;margin-top:-20px;margin-left:265px;vertical-align:central">
            
                  <img id="Img1" width="299" style="margin-top:0px;vertical-align:top;margin-bottom:10px;margin-left:0px;height: 100px" src="images/LoginScreen/header.jpg" />
                
                 <div style="height:30px"></div>
                <div  style="font-family:Lucida Sans Unicode;visibility:collapse; font-weight:bold;height:23px; color:#0094FF"> <p style="display:inline; font-size:16px;"> Sign up  </p> </div> 
        
              <div style="font-family:LucidaSans Unicode;visibility:collapse;height:30px;text-align:left; font-size:11px; color:black"> <p style="margin-left:25px;font-size:11px;"> Please fill all the mandatory fields  </p> </div>  
              </div>
         </div>
       



     
                   <table style="float: left;width :auto;margin-left:75px;margin-top:10px;" >
                  
                       <tr>  
                                                     <td> <svg width="8" height="8"> <circle cx="3.2" cy="3.2" r="3.2" stroke="red" stroke-width="0" fill="red" id="circleCompanyRead" /></svg></td>
                           <%--<td style ="width:15px;"> <svg height="3" width="3" style="margin-bottom:3px;margin-right:5px" ><circle cx="2" cy="2" r="2" stroke="red" stroke-width="2" fill="red" id="circleCompanyRead" /></svg></td>--%>
                           <td style ="width:90px">Company</td> 
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="Company"  name="Company" runat="server" onKeyUp="onCompanyChanges()" /></td>
                       </tr>

                       <tr>  
                         <td style ="width:15px;"> </td>
                           <td style ="width:90px">Street</td>  
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="Street"  name="Street" runat="server"  /></td>
                       </tr>

                       <tr>  
                         <td style ="width:15px;"> </td>
                           <td style ="width:90px">City</td>  
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="City"  name="City" runat="server"  /></td>
                       </tr>

                       <tr>  
                        <td style ="width:15px;"> </td>
                           <td style ="width:90px">State</td>  
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="State"  name="State" runat="server"  /></td>
                       </tr>

                       <tr>  
                        <td style ="width:15px;"> </td>
                           <td style ="width:90px">Zip Code</td>  
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="ZipCode"  name="ZipCode" runat="server"  /></td>
                       </tr>

                       <tr style="height:31px;">
                          <td> <svg width="8" height="8"> <circle cx="3.2" cy="3.2" r="3.2" stroke="red" stroke-width="0" fill="red" id="circleRedCountry" /></svg></td>
                      
                         <%--<td style ="width:15px;"> <svg height="3" width="3" style="margin-bottom:3px;margin-right:5px" ><circle cx="2" cy="2" r="2" stroke="red" stroke-width="2" fill="red" id="circleRedCountry" /></svg></td>--%>
                           <td style ="width:90px">Country</td>  
                            <td style="width:20px;"><div style="margin-top:2px;margin-bottom:1px;height:31px"><input type="text" style="width:283px;height: 25px;"    autocomplete="on"  size="10"   id="cmbCountries"  name="Country" runat="server" onKeyUp="onCountryChanges()" /></div></td>
                       </tr>

                       <tr style="margin-top:5px">  
                        <td style ="width:15px;"> </td>
                           <td style ="width:90px">Phone</td>  
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="Phone"  name="Phone" runat="server" /></td>
                       </tr>

                       <tr>  
                        <td style ="width:15px;"> </td>
                           <td style ="width:90px">Fax</td>  
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="Fax"  name="Fax" runat="server"  /></td>
                       </tr>

                       <tr>  <td>
                       <svg width="8" height="8"> <circle cx="3.2" cy="3.2" r="3.2" stroke="red" stroke-width="0" fill="red" id="circleRedIATACode" /></svg></td>
                      
                          
                           <td style ="width:90px">IATA (7 digits)</td>  
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="IATACode"  name="IATACode" runat="server" onKeyUp="onIATACodeChanges()"   /></td>
                       </tr>
                       
                       <tr>  
                        <td style ="width:15px;"> </td>
                           <td style ="width:90px">CASS (4 digits)</td>  
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="CASSCode"  name="CASSCode" runat="server" /></td>
                       </tr>


                   </table>

                <table style="float: left;width :450px;margin-left:75px;margin-top:10px;" >
                        
                    
                      <tr> <td><svg width="8" height="8"> <circle cx="3.2" cy="3.2" r="3.2" stroke="red" stroke-width="0" fill="red" id="circleRedEmail" /></svg></td>               
                                       <td style ="width:85px">E-mail</td>  
                           <td style="width:20px"><input  autocomplete="on"    size="10"   class="auto-style1" id="Email" type="Email" name="Email" runat="server"  onKeyUp="onEmailChanges()" /></td>
                      
                           </tr>

                      <tr>  <td>
                       <svg width="8" height="8"> <circle cx="3.2" cy="3.2" r="3.2" stroke="red" stroke-width="0" fill="red" id="circleRedName" /></svg></td>
 

                           <td style ="width:85px">Name</td>  
                            <td style="width:20px"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="ContactName"  name="ContactName" runat="server" onKeyUp="onNameChanges()" /></td>
                     
                            </tr>

                      <tr>  
                         <td style ="width:15px;"> </td>
                           <td style ="width:85px">Note</td>  
                            <td style="width:20px"><textarea class="auto-style1" style="width:273px; height:71px;resize: none;margin-top:1px;  text-wrap:normal; text-align:initial" id="Comments"   name="Comments" ></textarea></td>
                           
                           </tr>

                      <tr> <td colspan="3" style="margin-left:50px" class="column1"><input class="cmdSubmit" style="margin-top:15px;margin-left:8px" type="submit" value="Submit >" runat="server" id="cmdSubmit1"  onclick="SubmitMethod()"  /></td></tr>
                      <tr><td colspan="3" ><p style="display:inline;margin-left:8px">Having trouble ?  </p> <p style="display:inline"> <a target="_blank"; style="cursor:pointer" href="mailto:Mirjam.Schubert@champ.aero">Contact us</a> </p></td></tr>  
                   
                   
                    
                    
                                                            
                      <tr>    <td  colspan="3">  <span style="background-color:red;"><p style="color:white;margin-left:8px;display:none;text-align:left;margin-top:3px;font-size:12px;width:auto;background-color:red;" id="messageError">  Save failed please enter the required field</p>  </span>  </td> </tr> 
                      <tr>    <td  colspan  ="3" >  <span style="background-color:red;"><p style="color:white;margin-left:8px;display:none;text-align:left;width:auto;margin-top:-5px;background-color:red" id="messageError2"></p>  </span>  </td> </tr> 
                      <tr>    <td colspan="3">  <span style="background-color:red;"><p style="color:white;display:none;margin-left:8px;text-align:left;width:auto;margin-top:-5px;background-color:red" id="messageError3"></p>  </span>  </td> </tr>                                    
                      <tr style="height:40px;">  <td colspan="3" > <p style="color:SteelBlue;margin-left:8px;text-align:left;display:none" id="message"> completed successfully</p>  </td> </tr>                                  
                      <tr>  <td colspan="3">  <div style="display:none;margin-top:-50px;margin-left:8px" id="busyIndicator"><img width="100" height="100" src="HtmlHelpers/Images/Icons/Progress.gif" alt='loading' /></div>      </td> </tr> 

                 
                </table>

        
      
         </div>
          
                      <script type="text/javascript">

                          var Street;
                          var Fax;
                          var ZipCode;
                          var Country;
                          var City;
                          var PhoneNumber;
                          var CompanyName;
                          var ContactName;
                          var Email;
                          var State;
                          var Comments;
                          var NumberOfBranches;
                          var NumberOfUsers;
                          var StatusCode;
                          var LeadSource;
                          var IATACode;
                          var CASSCode;
                          var PackageCode;
                          var RequestType;




                          function LogitudeLoadPM() {


                              this.CompanyName = $("#Company").val();
                              this.Street = $("#Street").val();
                              this.City = $("#City").val();
                              this.State = $("#State").val();
                              this.ZipCode = $("#ZipCode").val();
                              this.Country = Country;

                              this.PhoneNumber = $("#Phone").val();
                              this.Fax = $("#Fax").val();
                              this.IATACode = $("#IATACode").val();
                              this.CASSCode = $("#CASSCode").val();
                              this.Email = $("#Email").val();
                              this.ContactName = $("#ContactName").val();
                              this.Comments = $("#Comments").val();

                              this.RequestType = "DemoTenant";
                              this.LeadSource = "Atlas";
                              this.PackageCode = "EAWB";
                              this.NumberOfUsers = 0;
                              this.NumberOfBranches = 1;
                          };

                          function SubmitMethod() {
                              $("#message").hide();
                              $("#messageError").hide();
                              $("#messageError2").hide();
                              $("#messageError3").hide();
                              $("#busyIndicator").hide();
                              document.getElementById("messageError").innerHTML = "";

                              var isvalid = true;
                              if ($("#Company").val().length < 1 || $("#cmbCountries").val().length < 1 || $("#ContactName").val().length < 1 || $("#Email").val().length < 1 || $("#IATACode").val().length < 1) {
                                  document.getElementById("messageError").innerHTML = "Please enter the required field : ";
                                  isvalid = false;
                              }

                              if ($("#Company").val().length < 1) {
                                  document.getElementById("messageError").innerHTML += "Company , ";
                              }


                              if ($("#cmbCountries").val().length < 1) {
                                  document.getElementById("messageError").innerHTML += "Country , ";
                              }

                              if ($("#ContactName").val().length < 1) {

                                  document.getElementById("messageError").innerHTML += "Contact Name , ";
                              }


                              if ($("#Email").val().length < 1) {

                                  document.getElementById("messageError").innerHTML += "Email , ";
                              }


                              if ($("#IATACode").val().length < 1) {

                                  document.getElementById("messageError").innerHTML += "IATA Code , ";
                              }


                              if (document.getElementById("messageError").innerHTML.length > 1 && !isvalid) {

                                  document.getElementById("messageError").innerHTML += "@";

                                  document.getElementById("messageError").innerHTML = document.getElementById("messageError").innerHTML.replace(", @", "");
                                  $("#messageError").show();

                              }




                              if (isvalid) {

                                  var isnotvalid = false;
                                  var isnotvalidIATACode = false;
                                  var isnotvalidCASSCode = false;
                                  var isnotvalidEmail = false;


                                  if ($("#IATACode").val().length > 7 || $("#IATACode").val().length < 7) {

                                      document.getElementById("messageError").innerHTML = " &nbsp;  IATA code code must have 7 digits";
                                      $("#messageError").show();


                                      isnotvalidIATACode = true;
                                      isnotvalid = true;
                                  }


                                  if ($("#CASSCode").val().length > 4 || ($("#CASSCode").val().length > 0 && $("#CASSCode").val().length < 4)) {

                                      if (isnotvalidIATACode) {
                                          document.getElementById("messageError2").innerHTML = " &nbsp;  CASS code code must have 4 digits";
                                          $("#messageError2").show();

                                      }
                                      else {

                                          document.getElementById("messageError").innerHTML = "&nbsp;  CASS code code must have 4 digits";
                                          $("#messageError").show();

                                      }

                                      isnotvalidCASSCode = true;
                                      isnotvalid = true;
                                  }



                                  if (!ValidateEmail($("#Email").val())) {

                                      if (isnotvalidIATACode) {

                                          if (isnotvalidCASSCode) {
                                              document.getElementById("messageError3").innerHTML = " &nbsp;  You have entered an invalid email address!";
                                              $("#messageError3").show();

                                          }
                                          else {
                                              document.getElementById("messageError2").innerHTML = " &nbsp;  You have entered an invalid email address!";
                                              $("#messageError2").show();

                                          }
                                      }
                                      else {
                                          if (isnotvalidCASSCode) {

                                              document.getElementById("messageError2").innerHTML = " &nbsp;  You have entered an invalid email address!";
                                              $("#messageError2").show();

                                          }
                                          else {
                                              document.getElementById("messageError").innerHTML = " &nbsp;  You have entered an invalid email address!";
                                              $("#messageError").show();


                                          }
                                      }

                                      isnotvalid = true;
                                  }


                                  if (!isnotvalid) {

                                      disableForm(true);
                                      $("#busyIndicator").show();


                                      var logitudeLoadpm = new LogitudeLoadPM();
                                      var url = "api/AerolineaseAtlasRegistrationLeads";

                                      $.ajax({
                                          url: url,
                                          data: JSON.stringify(logitudeLoadpm),
                                          type: 'POST',
                                          contentType: 'application/json',
                                          success: function (result) {



                                              $("#message").show();

                                              $("#Company").val("");
                                              $("#Street").val("");
                                              $("#City").val("");
                                              $("#State").val("");
                                              $("#ZipCode").val("");
                                              $("#cmbCountries").val("");
                                              $("#Phone").val("");
                                              $("#Fax").val("");
                                              $("#IATACode").val("");
                                              $("#CASSCode").val("");
                                              $("#Email").val("");
                                              $("#ContactName").val("");
                                              $("#Comments").val("");


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
                                                      data: Countries

                                                  }
                                                  //template: kendo.template($("#partnerTemplate").html())
                                              });
                                              $("#circleCompanyRead").show();
                                              $("#circleRedCountry").show();
                                              $("#circleRedEmail").show();
                                              $("#circleRedName").show();
                                              $("#circleRedIATACode").show();
                                              $("#messageError").hide();
                                              $("#busyIndicator").hide();
                                          },

                                          error: function (jqXHR, textStatus, errorThrown) {



                                              $("#busyIndicator").hide();
                                              disableForm(false);
                                              $("#message").hide();
                                              document.getElementById("messageError").innerHTML = "Save failed! Connection to the server failed.";
                                              $("#messageError").show();

                                          }
                                      });

                                  }


                              }
                          }

                          function ValidateEmail(mail) {
                              if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {

                                  return true;

                              }
                              else {
                                  return false;

                              }


                          }


                          function onCompanyChanges() {
                              if ($("#Company").val() != "") { $("#circleCompanyRead").hide(); }
                              else { $("#circleCompanyRead").show(); }
                          }


                          function onCountryChanges() {
                              if ($("#cmbCountries").val() != "") { $("#circleRedCountry").hide(); }
                              else { $("#circleRedCountry").show(); }
                          }


                          function onEmailChanges() {
                              if ($("#Email").val() != "") { $("#circleRedEmail").hide(); }
                              else { $("#circleRedEmail").show(); }
                          }


                          function onNameChanges() {
                              if ($("#ContactName").val() != "") { $("#circleRedName").hide(); }
                              else { $("#circleRedName").show(); }
                          }

                          function onIATACodeChanges() {
                              if ($("#IATACode").val() != "") { $("#circleRedIATACode").hide(); }
                              else { $("#circleRedIATACode").show(); }
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




                          var Countries;
                          jQuery.LoadCountries = (function () {

                              var url = "api/Country?bycrmTenant=true" + "&tenant=" + 0;
                              $.ajax({
                                  url: url,
                                  type: 'Get',
                                  contentType: 'application/json',
                                  success: function (result) {

                                      if (result != null) {
                                          Countries = result;
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

                              if ($("#cmbCountries").val() != "") { $("#circleRedCountry").hide(); }
                              else { $("#circleRedCountry").show(); }
                              // var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType;

                          }



                          $(document).ready(function () {

                              $("#cmbCountries").kendoComboBox(
                                       {
                                           dataTextField: "EnglishName",
                                           dataValueField: "EnglishName",
                                           placeholder: "Choose your Country",
                                           change: onChange,
                                           filter: "contains",
                                           suggest: false,


                                           //template: kendo.template($("#partnerTemplate").html())
                                       });


                              $.LoadCountries();


                          });


       </script>
        </body>
    
 
         
</html>