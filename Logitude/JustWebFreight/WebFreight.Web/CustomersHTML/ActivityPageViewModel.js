(function (jQuery) {

    jQuery.CurrentEmail = null;
    jQuery.CurrentTenant = null;
    jQuery.CurrentCardId = null;
    jQuery.CurrentEntityId = null;

    jQuery.ResizePage = (function () {

        var minHeight = 400;
        var fixedHeight = 50;
        var screenHeight = $(window).height();
        var PageHeight = screenHeight - fixedHeight;
        if (PageHeight < minHeight) {
            PageHeight = minHeight;
        }

        var PageContentHeight = PageHeight - 124;
        var tabContentHeight = PageContentHeight - 35;

        $("#Page").css({ height: PageHeight });
        $("#PageContent").css({ height: PageContentHeight });
        $(".TabContent").css({ height: tabContentHeight });
    });

    $("#BackButton").click(function () {
        parent.history.back();
        return false;
    });

    $(document).ready(function () {
        
        $.ResizePage();
        $(window).resize(function () {
            $.ResizePage();
        });

        var hash = $(location).attr('href');
        var hashSplit = hash.split("?");
        var loginText = hashSplit[1].toLowerCase();
        loginText = loginText.replace("id=", "");
        loginText = loginText.replace("email=", "");
        loginText = loginText.replace("tenant=", "");
        loginText = loginText.replace("cardid=", "");
        loginText = loginText.replace("ischamplogin=", "");
        var loginData = loginText.split('&');

        $.CurrentEntityId = loginData[0];
        $.CurrentTenant = loginData[1];
        $.CurrentEmail = loginData[2];
        $.CurrentCardId = loginData[3];


    });
}(jQuery));