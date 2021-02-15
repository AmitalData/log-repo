<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomersLoginPage.aspx.cs" Inherits="WebFreight.Web.CustomersLoginPage" %>

<!DOCTYPE html>

<html>

<head  id="Head1" runat="server">
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
    <script src="HtmlHelpers/JS/Amital.GatewayControl.js" type="text/javascript"></script>

    <div id="Container"> 
        <table style="width:100%;">
            <thead>
                <tr style="height:114px;">
                    <td></td>
                    <td style="width:1024px; text-align:center; vertical-align:top;" >                   
                        <a id="imglink" target="_blank">
                            <img id="loginlogo" width="290" height="114" style="margin-top:50px;"/>
                        </a>
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
                                                    <table style="width:100%; height:226px; border-collapse:collapse; border-spacing:0; ">

                                                        <tr>
                                                            <td style="background:white;">
                                                                <div style="float: left;margin-left:60px;width:300px" id="myform">                                      
                                                                    <table id="loginForm" style="display:normal">
                                                                        <tr>
                                                                            <td class="column1">E-mail: 
                                                                                <br /> 
                                                                                <input autocomplete="on"  size="10"  class="auto-style1" id="Email" type="email" name="Email" runat="server"  placeholder="e.g. myname@example.net"  required data-email-msg="Email format is not valid" oninput="onEmailChanges()"/>                                                                                                                           
                                                                            </td>
                                                                        </tr>                                        

                                                                        <tr>
                                                                            <td class="column1">Password: <br /><input class="auto-style1" id="Password" type="password" runat="server" onchange="onPasswordChanged()" required data-email-msg="password is required!" onkeypress="capLock(event)"/>
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
                                                                                <input class="loginButton" type="submit" value="Login >" runat="server" id="cmdLogin" data-bind="click: validateMethod"/>
                                                                                <a  style="margin-left:15px;color:#4B4A4A;font-size:12px;font-family:Arial"" onclick="resetpasswordclick()">Forgot your password?</a>
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
                                                                                <a style="margin-left:15px;margin-top:50px;color:#4B4A4A;font-size:12px;font-family:Arial;vertical-align:central" href="login.aspx" >Back to login page</a>
                                                                            </td>
                                                                        </tr>                                       
                                                                </table>
                                                                </div>
                                                            </td>

                                                            <td style=" width:2px; text-align:right;border:0;"><img src="images/LoginScreen/line.png" style="width:2px;height:260px;margin-right:-3px;border:thick"/></td>
                                                            <td style="width:639px;"><img width="650" style="height:260px;min-width:650px;width:650px;border:0" src="images/LoginScreen/Layer.png"/></td>
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

         var companyList;
         var password;
         var email;
         var currentTenant;

         function resetpasswordclick() {
             window.location.href = "PasswordResetRequestPage.aspx";
         }

         function setCaretToPos(id, cursorPosition) {
             document.getElementById(id).selectionStart = cursorPosition;
             document.getElementById(id).selectionEnd = cursorPosition;
         }
         
         onPasswordChanged = function () {
             var emailstring = $("#Password").val();
             if (emailstring) {

                 var cursorPosition = document.getElementById("Password").selectionStart;
                 $("#Password").val($.trim(emailstring));
                 setCaretToPos("Password", cursorPosition);

                 $("#errorsList").hide();
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

         function onEmailChanges() {
             var emailstring = $("#Email").val();
             if (emailstring) {

                 var cursorPosition = document.getElementById("Email").selectionStart;
                 $("#Email").val($.trim(emailstring));

                 setCaretToPos("Email", cursorPosition);
             }
             $("#errorsList").hide();
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
                     change: onChange,
                     filter: "contains",
                     suggest: false,

                     dataSource:
                     {
                         data: companies
                     }
                 });

                 disableForm(false);

                 $("#loginForm").hide();
                 $("#comboForm").show();
                 $("#busyIndicator").hide();
                 $("#loginBusyindicator").hide();
             }

             this.validateMethod = function () {
                 var userData;
                 password = $("#Password").val();
                 email = $("#Email").val();
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

                 save_data_to_cookie();
                 disableForm(true);
                 $("#loginBusyindicator").show();
                 var url = "api/Authentication";///?email=" + email + "&password=" + password;
                 function LoginParameters() {

                     this.Email = email;
                     this.Password = password;
                 };

                 var param = new LoginParameters();

                 $.ajax({
                     url: url,
                     type: 'POST',
                     data: JSON.stringify(param),
                     contentType: 'application/json',

                     success: function (userdata) {
                         disableForm(false);

                         if (!userdata.HasError) {                                                        

                             if (userdata.ContactsCount == 1) {

                                 var loginString = "tenant=" + userdata.CurrentTenant + "&email=" + userdata.UserName + "&cardId=" + userdata.Id;
                                 document.location.href = "CustomersHTML/CustomersListPage.aspx?" + loginString;

                                 $("#loginBusyindicator").hide();
                             }

                             else {

                                 showTenantsCombo(userdata.CompanyLogins);
                             }
                         }
                         else {
                             $("#loginBusyindicator").hide();
                             if (userdata.MustChangePassword) {

                                 document.location.href = "PasswordChangePage.aspx?email=" + email
                             }
                             else {
                                 var errorMessage = "Login failed! invalid user name or password." + "<br/>";
                                 if (userdata.IpRestricted) {
                                     errorMessage = "Trying to log in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "log in from is restricted for this user)";
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
                                 $("#errorsList").show();
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
         };

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

                     if (!userdata.HasError) {

                         var loginString = "tenant=" + userdata.CurrentTenant + "&email=" + userdata.UserName + "&cardId=" + userdata.Id;
                         document.location.href = "CustomersHTML/CustomersListPage.aspx?" + loginString;
                     }

                     else {
                         if (userdata.MustChangePassword) {
                             document.location.href = "PasswordChangePage.aspx?email=" + email
                         }
                         else {
                             var errorMessage = "Login failed! invalid user name or password." + "<br/>";

                             if (userdata.IpRestricted) {
                                 errorMessage = "Trying to log in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "log in from is restricted for this user)";
                             }

                             if (userdata.IsLocked) {
                                 errorMessage = "Your account has been locked out!" + "<br/>" + "please try again after 30 minutes.";
                             }

                             document.getElementById("errorsList").innerHTML = errorMessage;
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

         var _LoginViewModel;
         $(document).ready(function () {

             var myLogoMethodUrl = "api/authentication?myDummyInteger=" + 0 + "&myDummyString=" + "0";
             $.ajax({
                 url: myLogoMethodUrl,
                 type: 'GET',
                 contentType: 'application/json',

                 success: function (myLogoCode) {
                     $("#logolink").attr("href", GetApplicationLogoIcon(myLogoCode));
                     $("#loginlogo").attr("src", GetApplicationLogoSource(myLogoCode));
                     $("#imglink").attr("href", GetApplicationLogoUrl(myLogoCode));
                 },
             });

            _LoginViewModel = new LoginViewModel();
            var bindingNode = document.getElementById('Container');
            ko.cleanNode(bindingNode);
            ko.applyBindings(_LoginViewModel, bindingNode);
         });

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

    cookie_name = "email_cookie"
    expdays = 365

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
    }

</script> 

</body>

</html>
