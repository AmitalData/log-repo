"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SendToComponentArgs = /** @class */ (function () {
    function SendToComponentArgs() {
    }
    SendToComponentArgs.AddComponent = function (sendToComponent) {
        if (SendToComponentArgs.SendToComponentLists == null) {
            SendToComponentArgs.SendToComponentLists = new Array();
        }
        var item = SendToComponentArgs.SendToComponentLists.filter(function (d) { return d.key == sendToComponent.key; })[0];
        if (!item) {
            SendToComponentArgs.SendToComponentLists.push(sendToComponent);
        }
        else {
            item.Component = sendToComponent.Component;
        }
    };
    return SendToComponentArgs;
}());
exports.SendToComponentArgs = SendToComponentArgs;
//# sourceMappingURL=ComponentArgs.js.map