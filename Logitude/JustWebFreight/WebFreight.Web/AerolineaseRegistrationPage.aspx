<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AerolineaseRegistrationPage.aspx.cs" Inherits="WebFreight.Web.AerolineaseRegistrationPage" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
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
    <script src="HtmlHelpers/JS/highlight.pack.js" type="text/javascript"></script>
</head>
       
    <body>

    <script src="HtmlHelpers/JS/app.js" type="text/javascript"></script>
    <script src="HtmlHelpers/JS/Amital.GatewayControl.js" type="text/javascript"></script>
          
     <div id="Container">      

         <div style="display:block">

  <div style="width:300px; display:inline-block;height:150px;margin-top:70px;margin-left:80px">
             <img id="loginlogo" width="299" style="margin-top:0px;margin-bottom:10px;margin-left:0px;height: 100px" src="images/ApplicationLogo/AerolineasLogo2.png" />
              <div  style="font-family:Lucida Sans Unicode; font-weight:bold;height:23px; color:#0094FF"> <p style="display:inline; font-size:16px;"> Sign up  </p> </div> 
        
              <div style="font-family:LucidaSans Unicode;height:30px;text-align:left; font-size:11px; color:black"> <p style="margin-left:25px;font-size:11px;"> Please fill all the mandatory fields  </p> </div>  
              </div>

             <div style="width:300px;display:inline-block;height:100px;margin-top:-20px;margin-left:265px">
            
                  <img id="Img1" width="299" style="margin-top:0px;vertical-align:top;margin-bottom:10px;margin-left:0px;height: 100px" src="images/ApplicationLogo/LogitudeLogo.jpg" />
                
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
                      <tr><td colspan="3" ><p style="display:inline;margin-left:8px">Having trouble ?  </p> <p style="display:inline"> <a target="_blank"; style="cursor:pointer" href="mailto:leandro.martinez@aerolineas.com.ar">Contact us</a> </p></td></tr>  
                   
                   
                    
                    
                                                            
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
                              this.LeadSource = "Aerolineas";
                              this.PackageCode = "BUBK";
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

                              if ($("#ContactName").val().length < 1 )
                               {
                                
                                  document.getElementById("messageError").innerHTML += "Contact Name , ";
                                }


                              if ($("#Email").val().length < 1) {

                                  document.getElementById("messageError").innerHTML += "Email , ";
                              }


                              if ($("#IATACode").val().length < 1) {

                                  document.getElementById("messageError").innerHTML += "IATA Code , ";
                              }


                              if (document.getElementById("messageError").innerHTML.length > 1 && !isvalid)
                              {
                                 
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


                                  if ($("#CASSCode").val().length > 4 ||  ($("#CASSCode").val().length>0 &&  $("#CASSCode").val().length < 4 )) {

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

                                              $("#Company").val("") ;
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


                              //var combobox = $("#cmbCountries").data("kendoComboBox");
                              //var selectedItem = combobox.dataSource.view()[combobox._current.index()];
                              //selectedcompany = selectedItem;
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
