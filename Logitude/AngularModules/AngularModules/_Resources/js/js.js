function attachmentUploader(id) {
    var file = document.querySelector('#' + id).files[0];
    return file;
}

function ResultAsArray(e) {
    return e.target.result;
}

function htmlComponentProparitiesTrue(a) {
    a.IsShowAttachmentList = true;
}

function htmlComponentProparitiesFalse(a) {
    a.IsShowAttachmentList = false;
}

function ElementProperities(_thisComponent) {
    _thisComponent.IsEditMode = false;
    _thisComponent.IsDisplayMode = true;
}

function MainMenuProperties(MainElement) {
    MainElement.IsDisplayMode = true;
    MainElement.IsEditMode = false;
    MainElement.CD.detectChanges();
}

function base64ToArrayBuffer(base64) {
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}

function saveByteArray(reportName, byte, type) {
    var blob = new Blob([byte], { type: 'application/octet-stream' });
    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    var timeNow = new Date();
    var fileName = reportName + type;
    link.download = fileName;
    link.click();
}

function insertAtSubject(areaId, text) {
    var txtarea = document.getElementById(areaId);
    if (!txtarea) { return; }

    var scrollPos = txtarea.scrollTop;
    var strPos = 0;
    var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ?
        "ff" : (document.selection ? "ie" : false));
    if (br == "ie") {
        txtarea.focus();
        var range = document.selection.createRange();
        range.moveStart('character', -txtarea.value.length);
        strPos = range.text.length;
    } else if (br == "ff") {
        strPos = txtarea.selectionStart;
    }

    var front = (txtarea.value).substring(0, strPos);
    var back = (txtarea.value).substring(strPos, txtarea.value.length);
    txtarea.value = front + text + back;
    strPos = strPos + text.length;
    if (br == "ie") {
        txtarea.focus();
        var ieRange = document.selection.createRange();
        ieRange.moveStart('character', -txtarea.value.length);
        ieRange.moveStart('character', strPos);
        ieRange.moveEnd('character', 0);
        ieRange.select();
    } else if (br == "ff") {
        txtarea.selectionStart = strPos;
        txtarea.selectionEnd = strPos;
        txtarea.focus();
    }

    txtarea.scrollTop = scrollPos;
    return txtarea.value;
}

function styleDisplay(d) {
    d.style.display = "none";
}

function itemStyling(a) {
    a.style.color = 'white';
    a.style.background = '-moz-linear-gradient(50% 100% 90deg,rgba(112, 112, 112, 1) 0%,rgba(168, 168, 168, 1) 100%)';
    a.style.background = '-webkit-linear-gradient(90deg, rgba(112, 112, 112, 1) 0%, rgba(168, 168, 168, 1) 100%)';
    a.style.background = '-webkit-gradient(linear,50% 100%,50% 0%,color-stop(0,rgba(112, 112, 112, 1) ),color-stop(1,rgba(168, 168, 168, 1) ))';
    a.style.background = '-o-linear-gradient(90deg, rgba(112, 112, 112, 1) 0%, rgba(168, 168, 168, 1) 100%)';
    a.style.background = 'linear-gradient(0deg, rgba(112, 112, 112, 1) 0%, rgba(168, 168, 168, 1) 100%)';
}

function EditgriditemStyling(a) {
    //a.style.color = 'white';
    a.style.background = '-moz-linear-gradient(50% 100% 90deg,rgba(255, 255, 255, 1) 0%,rgba(186, 206, 227, 1) 100%)';
    a.style.background = '-webkit-linear-gradient(90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%)';
    a.style.background = '-webkit-gradient(linear,50% 100%,50% 0%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(186, 206, 227, 1)  ))';
    a.style.background = '-o-linear-gradient(90deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%)';
    a.style.background = 'linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(186, 206, 227, 1) 100%)';
}

function itemWidth(a) {
    a.style.minWidth = (this.ViewWidth) + 'px';
}

function SelectingElement(input) {
    input.select();
}

function OrginalError(e) {
    return e.originalError;
}

function Contexting(e) {
    return e.context;
}

function logLoveReturnWhich(keyboardEvent) {
    return keyboardEvent.which;
}

function Selection(input) {
    if (input) {
        input.select();
    }
}

function keyBoardWhich(keyboardEvent) {
    return keyboardEvent.which;
}

function keyBoardKey(keyboardEvent) {
    return keyboardEvent.key;
}

function selectionStart(input) {
    return input.selectionStart;
}


function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function querySelection(id) {
    return document.querySelector("#" + id).files[0];
}

function resultToUnitArray(e) {
    return e.target.result;
}

function SelectionInput(textarea) {
    textarea.SelectInput = textarea;
}

function HTMLID(id) {
    return $("#" + id);
}

function GetPercentageImageHeight(imageUrl) {

    var i = new Image();

    i.onload = function () {
        return (i.height / i.width);
    };

    i.src = imageUrl;
}

function SetNewValue(textarea, value, type) {
    if (type == "text") {

        textarea.value = value;
    }
    else if (type == "checkbox") {
        textarea.checked = value == "true" ? true : false;

    }
}

function UploadLogoFile(id) {
    var file = document.querySelector('#' + id).files[0];
    return file;
}

function SetImage(id, myResult, setTitle) {
    $("#" + id).css("display", "block")
    $("#" + id).attr('src', myResult);

    //if (setTitle) $("#" + id).attr('title', "Click to Change the photo");
}

function HideImage(id, myResult) {
    $("#" + id).css("display", "none")
}

function ArrayBufferToBase64(e) {
    return e.target.result;
}

function ShowHideProgressDownload(show, id) {
    if (show) {
        $("#" + id).css("display", "block")
    }

    else {
        $("#" + id).css("display", "none")
    }
}

function getHTMLID(id) {
    return ($("#" + id))
}

function SetHtmlToFrame(id, html) {
    var element = document.getElementById(id);
    if (element && element.contentWindow) element.contentWindow.document.body.innerHTML = html;
 
    // element.contentWindow.document.write(html);
}

function StringToBase64(str) {
    return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g, function (match, p1) {
        return String.fromCharCode('0x' + p1);
    }));
}

function Base64ToString(b64) {
    return decodeURIComponent(escape(window.atob(b64)));
    // return atob(decodeURIComponent(escape(b64)));
}

function GetPlainTextFromHtml(el) {
    var sel, range, innerText = "";
    if (typeof document.selection != "undefined" && typeof document.body.createTextRange != "undefined") {
        range = document.body.createTextRange();
        range.moveToElementText(el);
        innerText = range.text;
    } else if (typeof window.getSelection != "undefined" && typeof document.createRange != "undefined") {
        sel = window.getSelection();
        sel.selectAllChildren(el);
        innerText = "" + sel;
        sel.removeAllRanges();
    }

    return innerText;
}

var changeDirection = function (dir, align) {
    this.selection.save();
    var elements = this.selection.blocks();
    for (var i = 0; i < elements.length; i++) {
        var element = elements[i];
        if (element != this.$el.get(0)) {
            $(element)
              .css('direction', dir)
              .css('text-align', align);
        }
    }

    this.selection.restore();
}

function RegisterCustomFroalaEditorButtom() {
    $.FroalaEditor.DefineIcon('rightToLeft', { NAME: 'long-arrow-left' });
    $.FroalaEditor.RegisterCommand('rightToLeft', {
        title: 'RTL',
        focus: true,
        undo: true,
        refreshAfterCallback: true,
        callback: function () {
            changeDirection.apply(this, ['rtl', 'right']);
        }
    })

    $.FroalaEditor.DefineIcon('leftToRight', { NAME: 'long-arrow-right' });
    $.FroalaEditor.RegisterCommand('leftToRight', {
        title: 'LTR',
        focus: true,
        undo: true,
        refreshAfterCallback: true,
        callback: function () {
            changeDirection.apply(this, ['ltr', 'left']);
        }
    })
}

function GetHtmlFromFrame(id) {
    var element = document.getElementById(id);
    if (element && element.contentWindow) return element.contentWindow.document.body.innerHTML;
    else return "";
}

var dragger = function () {
    return {
        move: function (element, xpos, ypos) {
            if (element) {
                element.style.left = xpos + 'px';
                element.style.top = ypos + 'px';
            }
        },

        startMoving: function (elementId, containerId, evt) {

            var element = document.getElementById(elementId);
            var container = document.getElementById(containerId);
            var rect = container.getBoundingClientRect();


            evt = evt || window.event;
            var posX = evt.clientX,
                posY = evt.clientY,
                divTop = element.style.top,
                divLeft = element.style.left,
                eWi = parseInt(element.style.width),
                eHe = parseInt(element.style.height),
                cWi = rect.width,
                cHe = rect.height;

            divTop = divTop.replace('px', '');
            divLeft = divLeft.replace('px', '');

            var diffX = posX - divLeft,
                diffY = posY - divTop;

            container.onmousemove = function (evt) {
                evt = evt || window.event;
                var posX = evt.clientX,
                    posY = evt.clientY,
                    aX = posX - diffX,
                    aY = posY - diffY;

                if (aX < 0) aX = 0;
                if (aY < 0) aY = 0;
                if (aX + eWi > cWi) aX = cWi - eWi;
                if (aY + eHe > cHe) aY = cHe - eHe;

                dragger.move(element, aX, aY);
            }
        },

        stopMoving: function (containerId) {
            var container = document.getElementById(containerId);
            if (container) {
                container.onmousemove = function () { }
            }            
        },
    }
}();
