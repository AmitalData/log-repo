"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var OutOfDepositMessage = /** @class */ (function (_super) {
    __extends(OutOfDepositMessage, _super);
    function OutOfDepositMessage() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        return _this;
    }
    OutOfDepositMessage.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            //this.RecoPM = args.ReconciliationPM;
        }
    };
    Object.defineProperty(OutOfDepositMessage.prototype, "CancellationRemarks", {
        get: function () {
            return this._OODMSG;
        },
        set: function (value) {
            this._OODMSG = value;
        },
        enumerable: true,
        configurable: true
    });
    OutOfDepositMessage.prototype.CustomerButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Customer;" + this._OODMSG);
    };
    OutOfDepositMessage.prototype.CashbookButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cashbook;" + this._OODMSG);
    };
    OutOfDepositMessage.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    OutOfDepositMessage = __decorate([
        core_1.Component({
            selector: 'OutOfDepositMessage',
            template: "\n    <style>\n    .ConfirmIcon {\n        width: 70px;\n        height: 70px;\n        line-height: 70px;\n        color: white;\n        font-size: 45px;\n        font-weight: bold;\n        font-family: Arial;\n        text-align: center;\n        vertical-align: middle;\n        -webkit-border-radius: 50px;\n        -moz-border-radius: 50px;\n        border-radius: 50px;\n        background: -moz-linear-gradient(50% 0% -90deg,rgba(108, 132, 153, 1) 0%,rgba(112, 136, 156, 1) 18.27%,rgba(125, 147, 166, 1) 37.99%,rgba(147, 166, 182, 1) 58.38%,rgba(177, 192, 205, 1) 79.19%,rgba(215, 225, 234, 1) 100%);\n        background: -webkit-linear-gradient(-90deg, rgba(108, 132, 153, 1) 0%, rgba(112, 136, 156, 1) 18.27%, rgba(125, 147, 166, 1) 37.99%, rgba(147, 166, 182, 1) 58.38%, rgba(177, 192, 205, 1) 79.19%, rgba(215, 225, 234, 1) 100%);\n        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(108, 132, 153, 1) ),color-stop(0.1827,rgba(112, 136, 156, 1) ),color-stop(0.3799,rgba(125, 147, 166, 1) ),color-stop(0.5838,rgba(147, 166, 182, 1) ),color-stop(0.7919,rgba(177, 192, 205, 1) ),color-stop(1,rgba(215, 225, 234, 1) ));\n        background: -o-linear-gradient(-90deg, rgba(108, 132, 153, 1) 0%, rgba(112, 136, 156, 1) 18.27%, rgba(125, 147, 166, 1) 37.99%, rgba(147, 166, 182, 1) 58.38%, rgba(177, 192, 205, 1) 79.19%, rgba(215, 225, 234, 1) 100%);\n        background: linear-gradient(180deg, rgba(108, 132, 153, 1) 0%, rgba(112, 136, 156, 1) 18.27%, rgba(125, 147, 166, 1) 37.99%, rgba(147, 166, 182, 1) 58.38%, rgba(177, 192, 205, 1) 79.19%, rgba(215, 225, 234, 1) 100%);\n    }\n    .RedButton{\n        position: absolute;\n        right: 10px;\n        bottom: 10px;\n        width: 65px;\n    }\n    </style>\n\n    <!--<div class=\"LeftCenter ConfirmIcon\" >?</div>-->\n\n    <div style= \"padding: 10px 15px;font-size: 12px;white-space: normal;\" >\n       {{'Accounting.O.OutOfDepositMSG' | TextCodeTranslationPipe }}\n    </div>\n\n    <div style= \"padding: 15px 10px 10px 15px;font-size: 12px;white-space: normal;\" >\n            <LogTextBox [Placeholder]=\"'Accounting.General.O.Notes' | TextCodeTranslationPipe\" [IsFreeText]=\"true\" [ObjectTableName]=\"'PaymentCheque'\" [IsMultiline]=\"true\" [ObjectFieldName]=\"'CancellationRemarks'\" [DataContext]=\"DataContext\"></LogTextBox>\n    </div>\n\n    <div style=\"width:100%;height:22px;position: absolute; bottom:0;\">\n        <!--<button  [style.float]=\"isRTL ? 'left' : 'right'\"  style=\"width: 80px;position: relative; display: inline-block;top:0;bottom:0;right:0;margin: 0 5px;\" class=\"RedButton\" (click)=\"CustomerButtonClicked()\">{{'Accounting.O.Customer' | TextCodeTranslationPipe }}</button>-->\n        <button  [style.float]=\"isRTL ? 'left' : 'right'\"  style=\"width: 60px;position: relative; display: inline-block;top:0;bottom:0;right:0;margin: 0 5px;\" class=\"RedButton\" (click)=\"CashbookButtonClicked()\">{{'Accounting.General.B.OK' | TextCodeTranslationPipe }}</button>\n        <button  [style.float]=\"isRTL ? 'left' : 'right'\"  style=\"width: 70px;position: relative; display: inline-block;top:0;bottom:0;right:0;margin: 0 5px;\" class=\"Button\" (click)=\"OkButtonClicked()\">{{'Accounting.General.B.Cancel' | TextCodeTranslationPipe }}</button>\n    </div>\n            "
        }),
        __metadata("design:paramtypes", [])
    ], OutOfDepositMessage);
    return OutOfDepositMessage;
}(BaseComponent_1.BaseComponent));
exports.OutOfDepositMessage = OutOfDepositMessage;
//# sourceMappingURL=OutOfDepositMessage.js.map