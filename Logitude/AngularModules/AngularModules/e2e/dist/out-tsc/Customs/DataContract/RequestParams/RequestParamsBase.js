"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var SendRequestVIA;
(function (SendRequestVIA) {
    SendRequestVIA[SendRequestVIA["Default"] = 0] = "Default";
    SendRequestVIA[SendRequestVIA["WebServiceInteractive"] = 1] = "WebServiceInteractive";
    SendRequestVIA[SendRequestVIA["WebServiceBatch"] = 2] = "WebServiceBatch";
    SendRequestVIA[SendRequestVIA["DCABatch"] = 3] = "DCABatch";
})(SendRequestVIA = exports.SendRequestVIA || (exports.SendRequestVIA = {}));
var RequestParamsBase = /** @class */ (function () {
    function RequestParamsBase() {
        //public readonly _PBId: string;
        //public readonly _IsAngularClient: boolean;
        //private _PBId: string = Guid.newGuid();;
        //private _IsAngularClient: boolean = true;
        //get PBId() { return this._PBId; }
        //get IsAngularClient() { return this._IsAngularClient; }
        this.PBId = Guid_1.Guid.newGuid();
        this.IsAngularClient = true;
    }
    ;
    return RequestParamsBase;
}());
exports.RequestParamsBase = RequestParamsBase;
var CustomSendOptionsArgs = /** @class */ (function () {
    function CustomSendOptionsArgs() {
    }
    return CustomSendOptionsArgs;
}());
exports.CustomSendOptionsArgs = CustomSendOptionsArgs;
//# sourceMappingURL=RequestParamsBase.js.map