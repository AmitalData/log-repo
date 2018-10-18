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

           
                <tr>
                    <td />
                    <td>

                         <div id="MessageCompleteAddLead" style="font-family:Lucida Sans Unicode;height:40px; text-align:left; display:none; font-weight:bold; font-size:16px; color:steelblue">Your Sing Up Information Submit completed successfully </div>
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
                                    <div style="float:left;margin-left:60px;width:auto" id="myform">
                                      

                                        <%--start table--%>
                                                   
                <div  style="font-family:Lucida Sans Unicode;text-align:left; font-weight:bold;height:23px; color:#0094FF"> <p style="display:inline; font-size:20px;"> Sign up  </p> </div> 
                                        
                                          <table style="float: left;width :100%;text-align:left;margin-left:0px;margin-top:10px;" >
                  
                      
                       
                            <tr style="margin-top:5px">  
                   
                           <td style ="width:125px;text-align:left">Email</td>  
                            <td style="text-align:left;"><input  type="email"   autocomplete="on"  size="10"  class="auto-style1" id="Email" name="Email" runat="server"  required data-email-msg="Email field is required" /></td>
                       </tr>

                                              <tr>
                                                 <td style="text-align:left;"colspan="2">
                                                      <div style="height:5px"></div>
                                                  </td>
                                              </tr>

                             <tr style="margin-top:5px"> 
                         
                           <td style ="width:125px;text-align:left">Company</td> 
                            <td style="text-align:left;"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="CompanyName"    name="Company"  runat="server" required data-email-msg="Company field is required"  /></td>
                       </tr>

                  
    <tr>
                                                  <td style="text-align:left;" colspan="2">
                                                      <div style="height:5px"></div>
                                                  </td>

                                              </tr>
                                 <tr style="margin-top:5px">
                        
                           <td style ="width:125px;text-align:left">Contact Name</td>  
                            <td style="text-align:left;"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="ContactName"  name="ContactName" runat="server"  required data-email-msg="Contact Name field is required"  /></td>
                       </tr>

                                                  <tr>
                                                  <td style="text-align:left;" colspan="2">
                                                      <div style="height:5px"></div>
                                                  </td>
                                              </tr>

                              <tr > 
                     
                           <td style ="width:125px;text-align:left">Phone Number</td>  
                            <td style="text-align:left;"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="Phone"  name="Phone" runat="server"  required data-email-msg="Phone field is required"  /></td>
                       </tr>

                                                  <tr>
                                                  <td  style="text-align:left;" colspan="2">
                                                      <div style="height:5px"></div>
                                                  </td>
                                              </tr>
       
                            <tr>  
                      
                           <td style ="width:125px;text-align:left">Request Type</td>  
                            <td style="text-align:left;">
                                
                                                      <select     style="width:283px;height: 35px;" name="RequestType"  runat="server" id="RequesTypee" aria-required="true"   >
                                                                              <option  value="Demo Tenant">Demo Tenant</option>
                                                                              <option value="Full Tenant">Full Tenant</option>
                                                     
                                                                    </select>
                            </td>
                       </tr>

                            <tr>  
                     
                           <td style ="width:125px;text-align:left">Number Of Branches</td>  
                            <td style="text-align:left;"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="NumberOfBranche"      runat="server"  required data-email-msg="Number Of Branches field is required"  /></td>
                       </tr>


                                                  <tr>
                                                  <td style="text-align:left;" colspan="2">
                                                      <div style="height:5px"></div>
                                                  </td>
                                              </tr>

                            <tr>  
                     
                           <td style ="width:125px;text-align:left">Number Of Users</td>  
                            <td style="text-align:left;"><input type="text"   autocomplete="on"  size="10"  class="auto-style1" id="NumberOfUser"      runat="server"  required data-email-msg="Number Of Users field is required"  /></td>
                       </tr>
    <tr>
                                                  <td style="text-align:left;" colspan="2">
                                                      <div style="height:5px"></div>
                                                  </td>
                                              </tr>

                            <tr style="height:31px;">
       
                        
                           <td style ="width:125px;text-align:left">Country Name</td>  
                            <td style="text-align:left;"><div style="margin-top:0px;margin-bottom:1px;height:31px;text-align:left"><input type="text" style="width:283px;height: 25px;text-align:left"    autocomplete="on"  size="10"   id="Country"  name="Country" runat="server" required data-email-msg="Country field is required"  /></div></td>
                       </tr>

                            <tr>  
                                 
                                                  <td style="text-align:left;" colspan="2">
                                                      <div style="height:5px"></div>
                                                  </td>
                                              </tr>
                     <tr>
                           <td style ="width:125px;text-align:left">Comments</td>  
                            <td style="text-align:left;">
                                
                               <textarea       style="height:71px; resize: none;margin-top:1px;  text-wrap:normal;text-align:initial"  size="10"   class="auto-style1" id="Comment"   name="Comment"  runat="server"/>

                            </td>
                       </tr>

                 
                       
                      


                   </table>
                                        
                                            
                                                             <div class="column1" style="text-align:left">
                                                                  <%--<asp:Button ID="Button1" runat="server" Text="Submit" Width="94px" OnClick="btnReset_Click" />--%>
                                                                 <input class="cmdSubmit" style="margin-top:15px" type="submit" value="Save" runat="server" id="cmdSubmit"  data-bind="click: submitMethod"/>

                                                             </div>

                                        <table style="text-align:left;margin-top:5px;">

                                            <tr style="height:40px;">
                                                             <td >
                                                                  <p style="color:SteelBlue;text-align:left;display:none;font-size:14px;" id="message">Save Completed Successfully</p>
                                                             </td>
                                                         </tr> 
                                                        <tr style="height:40px;">
                                                             <td >
                                                                  <span style="background-color:red">
                                                                      <p style="color:white;display:none;text-align:left;background-color:red;margin-top:3px;width:268px" id="messageError">Save failed please Enter the required Field</p>
                                                                  </span>
                                                                 <%-- <p style="color:SteelBlue;text-align:left;display:none;font-size:14px;" id="messageError">Save failed please Enter the required Field</p>--%>
                                                             </td>
                                                         </tr> 

                                                         <tr>
                                                             <td>
                                                                  <p id="busyIndicator" style="display:none"><img width="50" height="50" src="images/LoginScreen/indicator.gif" alt='loading' /></p>

                                                             </td>
                                                         </tr>  
                                            </table>


                                        <%--/////end--%>


                                                    
                                        <%--//End Table--%> 

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
        

     
 
  
           <script type="text/javascript">
               var id = null;
               var LogitudeLoadId = null;
               var StatusCode;
               var IsSentToCustomer;
               var TenantNumber;
               var IsEmailVerified;
               var CreateDate;
         
               var CurrentCoutry
               
     
               function LogitudeLoadPM() {

                   this.PhoneNumber = $("#Phone").val();
                   this.CompanyName = $("#CompanyName").val();
                   this.ContactName = $("#ContactName").val();
                   this.NumberOfBranches = $("#NumberOfBranche").val();
                   
                   this.Country = CurrentCoutry;
                 //  CurrentCoutry
                   //this.Country = $("#Country").val();
                   this.Email = $("#Email").val();
                
                       this.Id = LogitudeLoadId;
                 
                       if ($('select[name=RequesTypee]').val() == "Full Tenant") {

                           this.RequestType = "FullTenant";
                       }
                       else
                           if ($('select[name=RequesTypee]').val() == "Demo Tenant") {
                   
                               this.RequestType = "DemoTenant";
                     
                           }

                   this.NumberOfUsers = $("#NumberOfUser").val();
                   this.Comments = $("#Comment").val();


                   this.StatusCode = StatusCode;
                   
                   this.IsSentToCustomer = IsSentToCustomer;
                   this.TenantNumber = TenantNumber;
                   this.IsEmailVerified = IsEmailVerified;
                   this.CreateDate = CreateDate;
                   
               };




               function disableForm(disable) {
                   if (disable) {
                       $("input").prop('disabled', true);
                   }
                   else {
                       $("input").prop('disabled', false);
                   }
               }




               jQuery.LoadLead = (function (id) {

                   var url = "api/LogitudeLeads?id=" + id;
                   $.ajax({
                       url: url,

                       type: 'Get',
                       contentType: 'application/json',
                       success: function (result) {

                           if (result != null) {
                               LogitudeLoadId = result.Id;
                               StatusCode = result.StatusCode;
                               IsSentToCustomer = result.IsSentToCustomer;
                               TenantNumber = result.TenantNumber;
                               IsEmailVerified = result.IsEmailVerified;
                               CreateDate = result.CreateDate;
                               Country = result.Country;


                               $("#Email").val(result.Email);

                               $("#ContactName").val(result.ContactName);

                               $("#CompanyName").val(result.CompanyName);

                               $("#NumberOfBranche").val(result.NumberOfBranches);

                               $("#Country").val(result.Country);

                               $("#NumberOfUser").val(result.NumberOfUsers);

                               if (result.RequestType == "FullTenant") {

                                   $("#RequesTypee").val("Full Tenant");
                               }
                               else
                                   if (result.RequestType == "DemoTenant") {

                                       $("#RequesTypee").val("Demo Tenant");
                                   }

                               $("#Comment").val(result.Comments);

                               $("#Phone").val(result.PhoneNumber);
                         
                              
                               $.LoadCountries();
                           }
                       },
                       error: function (jqXHR, textStatus, errorThrown) {



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
                          
                              
                            
                  $("#Country").kendoComboBox(
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



                       }
                   });

               })
               




               function viewModel() {

                   this.submitMethod = function () {
                       
                       $("#busyIndicator").show();
                  
                    var validatable = $("#myform").kendoValidator().data("kendoValidator");

                       //if (!CurrentCoutry) {
                       //    document.getElementById("errorsList").innerHTML = "Please fill in the required fields.";
                       //    $("#errorsList").show();
                       //}
                   
                     if (validatable.validate() === false) {
                      
                           // get the errors and write them out to the "errors" html container
                           var errors = validatable.errors();
                           $(errors).each(function () {
                               disableForm(false);
                               $("#busyIndicator").hide();
                           });
                           return;
                         
                      }
                   
                   
                       if ($("#Email").val() != "" && $("#ContactName").val() != "" && $("#Phone").val() != "" && $("#NumberOfBranche").val() != "" && $("#NumberOfUser").val() != "" && $("#RequesTypee").val() != "" && CurrentCoutry != "") {
                         
                          
                           var logitudeLoadpm = new LogitudeLoadPM();
                      
                           var url = "api/LogitudeLeads";

                           $.ajax({
                               url: url,
                               data: JSON.stringify(logitudeLoadpm),
                               type: 'POST',
                               contentType: 'application/json',
                               success: function (result) {
                                    
                                   if (id == "null") {
                                        
                                       $("#MessageCompleteAddLead").show();

                                       $("#mapBackground").hide();
                                        disableForm(true);
                                   }
                                   else {
                                        
                                       disableForm(true);
                                   document.location.href = "SignUpVerification.aspx?id=" + id;
                          
                               }
                                   $("#busyIndicator").hide();
                               },
                               error: function (jqXHR, textStatus, errorThrown) {
                                   //$("#messageError").show();
                                   //document.getElementById("errorsList").innerHTML = "Save failed! Connection to the server failed.";
                                   //$("#errorsList").show();

                                   disableForm(false);

                                   $("#busyIndicator").hide();

                               }
                           });


                       }
                       else {
                      
                           $("#busyIndicator").hide();
                          disableForm(false);
                          $("#messageError").show();
                       }
                     


                    
                      
                 

                   }
               }
                          
       

               function onChange() {

                   var combobox = $("#Country").data("kendoComboBox");
                   var selectedItem = combobox.dataSource.view()[combobox._current.index()];
                   selectedcompany = selectedItem;
                    
                   CurrentCoutry = $('#Country').data('kendoComboBox').value();
                   // var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType;

               }


               function getURLParameter(name) {
                   return decodeURI(
                       (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
                   );
               }

               $(document).ready(function () {
                   

                   ko.applyBindings(new viewModel());

                    id = getURLParameter("id");
                
                   if (id != "null") {

                        $.LoadLead(id);
                    }
                    else {

                        $.LoadCountries();
                    }

               });

    </script> 
 </table>
         </div>
        
</body>

    
         
</html>
