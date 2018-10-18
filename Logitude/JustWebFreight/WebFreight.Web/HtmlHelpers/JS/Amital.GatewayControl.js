var _JavascriptGateway = new Object();
var _SilverlightCtl = null;

//From silverlight to OCX
function SendGatewayControlEvent(SenderID, ReceiverID, MessageID, MessageCompress, MessageXML, MoreParams) {
    //alert("HTML:SendGatewayControlEvent");
    try {

        window.external.SendGatewayControlEvent(SenderID, ReceiverID, MessageID, MessageCompress, MessageXML, MoreParams);

    } catch (e) {
        alert("HTML:SendGatewayControlEvent:catch (e)" +e.toString());
    }
    
}

function DisplayAlertMessage(param1) {
    alert("your are invoke method of javscript \n" + param1);
}
//calling Silverlight method

function Amital_PluginLoaded(sender, args) {
    //<param name="onLoad" value="Amital_PluginLoaded" />
    _SilverlightCtl = sender.getHost();
}
function CallSilverlightShowAlertPopup() {
    _SilverlightCtl.Content.SL2JS.ShowAlertPopup("Testing for Calling Silverlight Method\n From Javascript");
}

//_JavascriptGateway.Add = function (a, b) { alert("ddd"); return a + b; };
_JavascriptGateway.UnifaceRequest = function (SenderID, ReceiverID, MessageID, CompressItBase64, MessageXML, MoreParams) {
    if (_SilverlightCtl == null) {
        alert("SilverlightCtl is null,abort");
        return;
    }
    _JavascriptGateway.UnifaceRequestResult = new String("");
    //alert("_JavascriptGateway.UnifaceRequest");
    var result = _SilverlightCtl.Content.SL2JS.UnifaceRequest(SenderID, ReceiverID, MessageID, CompressItBase64, MessageXML, MoreParams);
    _JavascriptGateway.UnifaceRequestResult = result;
};
_JavascriptGateway.UnifaceRequestResult = new String("");

_JavascriptGateway.LogMeIn = function (usr, pass) {
    if (_LoginViewModel == null) {
        alert("_LoginViewModel is null,abort");
        return;
    }
    //alert("_JavascriptGateway.LogMeIn ");
    $("#Password").val(pass);
    $("#Email").val(usr);
    _LoginViewModel.validateMethod();
};

function GetJavascriptGateway() {
    return _JavascriptGateway;
}

window.onerror = function (msg, url, line, col, error) {
    // Note that col & error are new to the HTML 5 spec and may not be 
    // supported in every browser.  It worked for me in Chrome.
    var extra = !col ? '' : '\ncolumn: ' + col;
    extra += !error ? '' : '\nerror: ' + error;

    // You can view the information in an alert to see things working like this:
    //alert("Error: " + msg + "\nurl: " + url + "\nline: " + line + extra);

    // TODO: Report this error via ajax so you can keep track
    //       of what pages have JS issues

    var myWindow = window.open("", "MsgWindow", "width=800, height=600");
    myWindow.document.write("<p>error</p>");
    myWindow.document.write("<p>" + error + "</p>");

    var suppressErrorAlert = true;
    // If you return true, then error alerts (like in older versions of 
    // Internet Explorer) will be suppressed.
    return suppressErrorAlert;
};