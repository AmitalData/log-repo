(function (jQuery) {

    jQuery.Application = null;
    jQuery.Email = null;
    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;
    jQuery.CurrentCardType = null;
    jQuery.CurrentEntityId = null;
    jQuery.CurrentUserId = null;
    
    jQuery.CurrentEntityPM = null;
    jQuery.CurrentEntityList = null;

    jQuery.CurrentCustomersList = null;
    jQuery.CurrentOpportunitiesList = null;
    jQuery.CurrentActivitiesList = null;
    jQuery.CurrentCustomersAdressList = null;
    jQuery.ShipmentsChartDataList = null;

    jQuery.CurrentEntityPostId = null;
    jQuery.CurrentEntityPostList = null;
    jQuery.CurrentPostsList = null;


    jQuery.Login = (function () {

        $.Application.showLoading();
        $("#Error").animate({ height: '0px', opacity: '0' }, 0);

        var email = $("#EmailBox").val();
        var pass = $("#PassBox").val();

        if (email && pass) {
            $("#EmailBox,#PassBox").blur();
            $.Validate(email, pass);
        }
        else {
            $.Application.hideLoading();
            $("#Error").animate({ height: '25px', opacity: '1' }, 250);
        }
    });

    jQuery.Validate = (function (email, password) {

        $("#Error").animate({ height: '0px', opacity: '0' }, 0);

        var userData;
        var persist = false;


        var url = "../api/authentication";//?email=" + email + "&password=" + password;//+ "&persistCookie=" + persist;
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

                if (!userdata.HasError) {

                    var sharedcontacts = new Array();
                    var users = new Array();
                    if (userdata.CompanyLogins) {
                        for (var index in userdata.CompanyLogins) {
                            if (!userdata.CompanyLogins[index].IsUser) {
                                sharedcontacts.push(userdata.CompanyLogins[index]);
                            }
                            else {
                                users.push(userdata.CompanyLogins[index]);
                            }
                        }

                        if (sharedcontacts.length > 0) {

                            //debugger
                            //debugger
                            $.Email = sharedcontacts[0].Email;
                            $.CurrentTenant = sharedcontacts[0].Tenant;
                            $.CurrentCardId = sharedcontacts[0].CardId;
                            $.CurrentCardType = sharedcontacts[0].CardType;
                            $.CurrentUserId = sharedcontacts[0].ContactId;
                            $.GetLogginData(password);
                        }
                        else {

                            document.getElementById("ErrorText").innerHTML = "you don have mobile access permission for this account!";
                            $("#Error").animate({ height: '25px', opacity: '1' }, 250);

                            //if (users.length > 0) {
                            //}
                            //else {
                            //}
                        }
                    }
                    else {
                        if (!userdata.IsUser) {
                            //debugger
                            $.Email = userdata.UserName;
                            $.CurrentTenant = userdata.CurrentTenant;
                            $.CurrentCardId = userdata.CardId;
                            $.CurrentCardType = userdata.CardType;
                            $.GetLogginData(password);
                        }
                        else {

                            document.getElementById("ErrorText").innerHTML = "you don have mobile access permission for this account!";
                            $("#Error").animate({ height: '25px', opacity: '1' }, 250);
                        }
                    }



                }
                else {

                    $.Application.hideLoading();

                    if (userdata.MustChangePassword) {

                        //document.location.href = "PasswordChangePage.aspx?email=" + email
                    }
                    else {
                        var errorMessage = "Login failed! invalid user name or password." + "<br/>";


                        if (userdata.IpRestricted) {

                            errorMessage = "Trying to log in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "log in from is restricted for this user)";//

                        }

                        if (userdata.IsLocked) {

                            errorMessage = "Your account has been locked out!" + "<br/>" + "please contact your administrator.";
                        }

                        document.getElementById("ErrorText").innerHTML = errorMessage;
                        $("#Error").animate({ height: '25px', opacity: '1' }, 250);

                    }
                }


            },

            error: function (jqXHR, textStatus, errorThrown) {


                document.getElementById("ErrorText").innerHTML = errorThrown;
                $("#Error").animate({ height: 'auto', opacity: '1' }, 250);
                //                navigator.notification.alert(
                //errorThrown,  // message
                //alertDismissed,         // callback
                //'Login Failed',            // title
                //'Close'                  // buttonName
                //);

            }
        });
    });

    jQuery.GetLogginData = (function (password) {


        var url = "../api/Authentication/?tenant=" + $.CurrentTenant; //+ "&isUser=" + false + "&cardId=" + $.CurrentCardId + "&cardType=" + $.CurrentCardType;
        function LoginParameters() {

            this.Email = $.Email;
            this.Password = password;

            this.IsUser = false;
            this.CardId = $.CurrentCardId;
            this.CardType = $.CurrentCardType;


        };

        var param = new LoginParameters();
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify(param),
            contentType: 'application/json',

            success: function (userdata) {

                if (!userdata.HasError) {

                    // var logindata = userdata.UserName + ":" + userdata.CurrentTenant + ":" + userdata.CardId + ":" + userdata.CardType;
                    $.Application.navigate("views/MainPage.html");


                }

                else {

                    $.Application.hideLoading();

                    if (userdata.MustChangePassword) {

                        //document.location.href = "PasswordChangePage.aspx?email=" + email
                    }
                    else {
                        var errorMessage = "Login failed! invalid user name or password." + "<br/>";


                        if (userdata.IpRestricted) {

                            errorMessage = "Trying to log in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "log in from is restricted for this user)";//

                        }

                        if (userdata.IsLocked) {

                            errorMessage = "Your account has been locked out!" + "<br/>" + "please contact your administrator.";
                        }

                        document.getElementById("ErrorText").innerHTML = errorMessage;
                        $("#Error").animate({ height: '25px', opacity: '1' }, 250);
                    }
                }


            },

            error: function (jqXHR, textStatus, errorThrown) {

                document.getElementById("ErrorText").innerHTML = errorThrown;
                $("#Error").animate({ height: 'auto', opacity: '1' }, 250);
                //$("#error").text("errror");
                //$("#error").show();
                $.Application.hideLoading();
                //var errorMessage = '';
            }
        });



    });

    function setCaretToPos(id, cursorPosition) {
        document.getElementById(id).selectionStart = cursorPosition;
        document.getElementById(id).selectionEnd = cursorPosition;
    }

    $('#EmailBox').bind('input', function () {
        var emailstring = $("#EmailBox").val();
        if (emailstring) {

            var cursorPosition = document.getElementById("EmailBox").selectionStart;
            $("#EmailBox").val($.trim(emailstring));
            setCaretToPos("EmailBox", cursorPosition);

        }

        $("#Error").animate({ height: '0px', opacity: '0' }, 0);
    });

    $('#PassBox').bind('input', function () {
        var emailstring = $("#PassBox").val();
        if (emailstring) {

            var cursorPosition = document.getElementById("PassBox").selectionStart;
            $("#PassBox").val($.trim(emailstring));
            setCaretToPos("PassBox", cursorPosition);

        }

        $("#Error").animate({ height: '0px', opacity: '0' }, 0);
    });

    $("#EmailBox,#PassBox").keypress(function (e) {
        if (e.which == '13') {
            $.Login();
        }
    });

    $("#LoginButton").click(function () {
        $.Login();
    });

    $(document).ready(function () {

        var app = new kendo.mobile.Application($(document.body),
            {
                transition: "slide",
                skin: "flat",
                init: function () {
                    app.pane.loader.element.find("h1").text("");
                    $.Application = app;
                }
            });
    });

}(jQuery));