(function (jQuery) {

    jQuery.SendContactActivity = (function (email, module, activity, tenant,cardId) {

        if (email && module && activity) {
            var url = "../api/activitylog?email=" + email + "&module=" + module + "&activity=" + activity + "&tenant=" + tenant + "&cardId=" + cardId;//email, string module, string activity, int tenant)

            $.ajax({
                url: url,
                //data: JSON.stringify(filters),
                type: 'POST',
                contentType: 'application/json',

                success: function (result) {


                },

                error: function (jqXHR, textStatus, errorThrown) {
                    
                }
            });
        }
    }

    );

    jQuery.SendContactsActivity = (function (email, module, activity, tenant, cardId) {

        if (email && module && activity) {
            var url = "../api/activitylog?email=" + email + "&module=" + module + "&activity=" + activity + "&tenant=" + tenant + "&cardId=" + cardId;//email, string module, string activity, int tenant)

            $.ajax({
                url: url,
                //data: JSON.stringify(filters),
                type: 'POST',
                contentType: 'application/json',

                success: function (result) {


                },

                error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }
    }

   );


}(jQuery));


(function (jQuery) {
    
    jQuery.CheckUserException = (function (jqXHR) {
        if (jqXHR) {
            var responseText = jQuery.parseJSON(jqXHR.responseText);
            var responseTitle = $(responseText).attr('ExceptionType');

            var ss = $.trim(responseTitle).indexOf("AutenticationException");
            if (ss != -1) {

                //$.LogoutUser($.CurrentEmail);
                
                alert("Sorry! this user is not authorized!");
                document.location.href = "../../Login.aspx";
            }


        }
        //if (email) {
        //    var url = "api/Authentication/?userEmail=" + email;

        //    $.ajax({
        //        url: url,
        //        type: 'GET',
        //        contentType: 'application/json',

        //        success: function (result) {

        //            document.location.href = "../../Login.aspx";

        //        },

        //        error: function (jqXHR, textStatus, errorThrown) {

        //            $("#error").text("errror");
        //            $("#error").show();
        //            alert("logout failed!");
        //            var errorMessage = '';
        //        }

        //    });
        //}
    }
    );

}(jQuery));


