"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AmitalGatewayUtil_1 = require("../../Infrastructure/Utilities/AmitalGatewayUtil");
var Tools_1 = require("../../Infrastructure/Tools");
var UnifreightController = /** @class */ (function () {
    function UnifreightController(_DeclarationPM, _ViewModelName) {
        this._DeclarationPM = _DeclarationPM;
        this._ViewModelName = _ViewModelName;
    }
    UnifreightController.prototype.SendRequestPrintStimulToUnifreightAsync = function (PrintParamsXml) {
        var declarationId = this._DeclarationPM.Id;
        var declarationNumber = this._DeclarationPM.DeclarationNumber;
        var customFileNo = this._DeclarationPM.CustomFileNo;
        AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging
            .RaisePrintStimulReturnCanIContinue(customFileNo, declarationId, this._ViewModelName, PrintParamsXml);
    };
    UnifreightController.prototype.SendRequestInstructionToUnifreightAsync = function (ViewPlace) {
        var declarationId = this._DeclarationPM.Id;
        var declarationNumber = this._DeclarationPM.DeclarationNumber;
        var customFileNo = this._DeclarationPM.CustomFileNo;
        AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseInstructionReturnCanIContinue(customFileNo, declarationId, this._ViewModelName, ViewPlace);
    };
    UnifreightController.prototype.GetPromise = function () {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var sub = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(function (mess) {
                var IsMatchUnifreightCallbackCommand = (mess.LogitudeEntity == AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                    mess.LogitudeEntityNumber == _this._DeclarationPM.Id &&
                    mess.LogitudeViewModel == _this._ViewModelName);
                if (IsMatchUnifreightCallbackCommand) {
                    sub.unsubscribe();
                    var sBool = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                    if (!Tools_1.AppTool.IsNullOrEmpty(sBool)) {
                        var bcanContinue = (sBool.toLowerCase() == 'true');
                        //if (bcanContinue) {
                        var unifreightResponseEventArgs = new UnifreightResponseEventArgs(mess, bcanContinue);
                        resolve(unifreightResponseEventArgs);
                        //}
                    }
                    else {
                        reject("Unifreight didn't send param " + AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                    }
                }
            });
        });
    };
    return UnifreightController;
}());
exports.UnifreightController = UnifreightController;
var UnifreightResponseEventArgs = /** @class */ (function () {
    function UnifreightResponseEventArgs(UnifreightMessage, UnifreightResponseStatus) {
        this.UnifreightMessage = UnifreightMessage;
        this.UnifreightResponseStatus = UnifreightResponseStatus;
    }
    return UnifreightResponseEventArgs;
}());
exports.UnifreightResponseEventArgs = UnifreightResponseEventArgs;
//# sourceMappingURL=UnifreightController.js.map