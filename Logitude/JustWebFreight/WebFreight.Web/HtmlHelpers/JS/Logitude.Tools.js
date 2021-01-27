

function GetApplicationLogoIcon(myLogoCode) {

    var myResult = null;
    var SmallLogoURL = window.sessionStorage.getItem("SmallLogoURL");
     
    if ($.trim(myLogoCode) == "C.R.M") {
        myResult = "images/ApplicationLogo/CRMIcon.png";
    }

    else if ($.trim(myLogoCode) == "A.N.G") {
        myResult = "images/ApplicationLogo/Angular/AngularSmallLogo.png";
    }

    else if ($.trim(myLogoCode) == "U.N.I") {
        myResult = "images/ApplicationLogo/UnifreightIcon.ico";
    }

    else if ($.trim(myLogoCode) == "L.O.B") {
        if (SmallLogoURL) {
            myResult = SmallLogoURL;
        }
        else {
            myResult = "images/ApplicationLogo/LogBoxIcon.png";
        }
        
    }

    else {
        myResult = "images/ApplicationLogo/LogitudeIcon.png";
    }

    return myResult;
}
function GetApplicationLogoSource(myLogoCode) {

    var myResult = null;
    var LogoURL = window.sessionStorage.getItem("LogoURL");
    if ($.trim(myLogoCode) == "C.R.M") {
        myResult = "images/ApplicationLogo/CRMLogo.png";
    }

    else if ($.trim(myLogoCode) == "A.N.G") {
        myResult = "images/ApplicationLogo/Angular/AngularLogo.png";
    }

    else if ($.trim(myLogoCode) == "U.N.I") {
        myResult = "images/ApplicationLogo/UnifreightLogo.jpg";
    }

    else if ($.trim(myLogoCode) == "L.O.B") {
        if (LogoURL) {
            myResult = LogoURL;
        }
        else {
            myResult = "images/ApplicationLogo/LogBox.png";
        }
       
    }

    else {
        myResult = "images/ApplicationLogo/LogitudeLogo.jpg";
    }

    return myResult;
}
function GetApplicationLogoUrl(myLogoCode) {

    var myResult = null;
    var PrivateLabelUrl = window.sessionStorage.getItem("PrivateLabelUrl");
    if ($.trim(myLogoCode) == "C.R.M") {
        myResult = "http://www.logitudeworld.com/";
    }

    else if ($.trim(myLogoCode) == "A.N.G") {
        myResult = "https://angularjs.org/";
    }

    else if ($.trim(myLogoCode) == "U.N.I") {
        myResult = "http://www.amital.co.il";
    }

    else if ($.trim(myLogoCode) == "L.O.B") {
        if (PrivateLabelUrl) {
            myResult = PrivateLabelUrl;
        }
        else {
            myResult = "http://www.logbox.co.il";
        }
       
    }

    else {
        myResult = "http://www.logitudeworld.com/";
    }

    return myResult;
}
function GetApplicationTitle(myLogoCode) {
    
    var myResult = null;
    var PrivateLabelShortName = window.sessionStorage.getItem("PrivateLabelShortName");
    if (myLogoCode && $.trim(myLogoCode) == "L.O.B") {
        if (PrivateLabelShortName) {
            myResult = PrivateLabelShortName;
        }
        else {
            myResult = "LogBox";
        }
      
    }
    else if (myLogoCode && $.trim(myLogoCode) == "U.N.I") {
        myResult = "Unifreight";
    }
    else {
        myResult = "Logitude";
    }

    return myResult;
}
function GetApplicationLogoWidth(myLogoCode) {

    var myResult = "290px";

    switch ($.trim(myLogoCode)) {
        case "A.N.G": {
            myResult = "337px";
            break;
        }

        default: {
            myResult = "290px";
            break;
        }
    }

    return myResult;
}
function GetApplicationLogoHeight(myLogoCode) {

    var myResult = "114px";

    switch ($.trim(myLogoCode)) {
        case "A.N.G": {
            myResult = "79px";
            break;
        }

        default: {
            myResult = "114px";
            break;
        }
    }

    return myResult;
}

function PostFormParams(formURL, params) {    
    var mapForm = document.createElement("form");
    mapForm.target = "_self";
    mapForm.method = "POST";
    mapForm.action = formURL;

    for (var i = 0; i < params.length; i++) {
        var mapInput = document.createElement("input");
        mapInput.type = "hidden";
        mapInput.name = params[i].name;
        mapInput.setAttribute("value", params[i].value);
        mapForm.appendChild(mapInput);
    }

    document.body.appendChild(mapForm);
    mapForm.submit();
    document.body.removeChild(mapForm);
}

