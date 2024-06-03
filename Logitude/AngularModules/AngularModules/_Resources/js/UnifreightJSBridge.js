
/* ITZIK :
this code is needed for bridge messages from uniface to angular
i hope it will not affect on cloud 
code review : by mohammad+ ihab
*/

if (window.location.href.includes("AmitalSSOAngular") && undefined ==
    window.AngularRecivedRequestFromUnifaceMethod) {
    // alert("sarya >>localhost!!!");
    window.AngularRecivedRequestFromUnifaceMethod = function (SenderID, ReceiverID, MessageID, CompressItBase64, MessageJSON, MoreParams) {
        //alert('**************Recived event from Uniface !!!**********');
        //alert(SenderID);
        //alert(ReceiverID);
        //alert(MessageID);
        //alert(CompressItBase64);
        //alert(MessageXML);
        //alert(MoreParams);


        let unifreightMessage = JSON.parse(MessageJSON);
        _JavascriptGateway.LastRequestFromAngular.UnifreightMessage = unifreightMessage;
        //RefreshResponseDiv(_JavascriptGateway.LastRequestFromAngular);
        this.setTimeout(() => {
            //RaiseUnifaceRequestEvent(unifreightMessage);
            var event = new CustomEvent('UnifaceRequestEvent', { 'detail': unifreightMessage, });
            window.dispatchEvent(event);
        }, 100)

    }

    var _JavascriptGateway = new Object();
    _JavascriptGateway.LastRequestFromAngular = new Object();
    _JavascriptGateway.SendRequestJSONToUnifreightAsync = function (myRequestWrapperJSON) {


        _JavascriptGateway.LastRequestFromAngular = JSON.parse(myRequestWrapperJSON);
        //---RefreshResponseDiv(_JavascriptGateway.LastRequestFromAngular);
        if (JSBridge) {

            if (JSBridge.AngularSendEvent2Uniface) {
                if (!_JavascriptGateway.LastRequestFromAngular) {
                    return;
                }
                let requestFromAngular = _JavascriptGateway.LastRequestFromAngular;//||{"SenderID":"","ReceiverID":"","MessageID":"",};
                if (!requestFromAngular.MessageID) {
                    return;
                }
                try {

                    //AngularSendEvent2UnifaceTester();
                    //alert("AngularSendEvent2UnifaceTester()");
                    let MessageID = requestFromAngular.MessageID;


                    let SenderID = "";
                    if (requestFromAngular.SenderID) {
                        SenderID = requestFromAngular.SenderID;
                    }
                    let ReceiverID = "";
                    if (requestFromAngular.ReceiverID) {
                        ReceiverID = requestFromAngular.ReceiverID;
                    }
                    let MessageCompress = false;
                    let MessageXML = "";
                    let UnifreightMessage = requestFromAngular.UnifreightMessage;
                    //alert("UnifreightMessage" + UnifreightMessage);
                    if (UnifreightMessage) {
                        MessageXML = JSON.stringify(UnifreightMessage);
                    }

                    let MoreParams = "";
                    if (requestFromAngular.MoreParams) {
                        MoreParams = _JavascriptGateway.LastRequestFromAngular.MoreParams;
                    }

                    JSBridge.AngularSendEvent2Uniface(SenderID, ReceiverID, MessageID, MessageCompress, MessageXML, MoreParams);

                } catch (err) {
                    alert("err " + err);
                }
            }
        }
        if (_JavascriptGateway.LastRequestFromAngular.MessageID == "RaiseCFIFILMLockReturnCFIFILMAlreadyLockMessage") {
            //alert("toCancell? or Lock/UnLock??")
        } else if (_JavascriptGateway.LastRequestFromAngular.MessageID == "NoteUnifreightIamReady") {
            // ShowDeclarationByIdReturnCloseSave();
        }


    };




    function RaiseUnifaceRequestEvent(objParams) {
        //var event = new CustomEvent('UnifaceRequestEvent', { 'detail': 'heeellos', 'data2': '1111' });
        var event = new CustomEvent('UnifaceRequestEvent', { 'detail': objParams, });
        let dueObjIsArrivedWithoutRequestList = false;
        if (dueObjIsArrivedWithoutRequestList) {
            event = new CustomEvent('UnifaceRequestEvent', { 'detail': JSON.stringify(objParams), });
        }


        //document.getElementById("myFrame").contentWindow.dispatchEvent(event);
        window.dispatchEvent(event);
    }
}

