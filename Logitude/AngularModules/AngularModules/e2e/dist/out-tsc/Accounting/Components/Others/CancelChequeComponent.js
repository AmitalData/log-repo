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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var PaymentChequePMService_1 = require("../../Services/StandardPMs/PaymentChequePMService");
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var JournalExtendedPMService_1 = require("../../Services/ExtendedPMs/JournalExtendedPMService");
var CancelChequeComponent = /** @class */ (function (_super) {
    __extends(CancelChequeComponent, _super);
    function CancelChequeComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "PaymentCheque";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.paymentChequePMService = new PaymentChequePMService_1.PaymentChequePMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.UIProperties.SetRequired("CancellationRemarks", "PaymentCheque", true);
        return _this;
    }
    CancelChequeComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.entityPM = args.PaymentChequePM;
        }
    };
    Object.defineProperty(CancelChequeComponent.prototype, "CancellationRemarks", {
        get: function () { return this.entityPM.CancellationRemarks; },
        set: function (value) {
            if (this.entityPM.CancellationRemarks != value) {
                this.entityPM.CancellationRemarks = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("CancellationRemarks", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("CancellationRemarks", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    CancelChequeComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CancelChequeComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.CancellationRemarks)) {
            var s = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("PaymentCheque.F.CancellationRemarks"));
            errors.push(s);
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.entityPM.CancellationRemarks = this.CancellationRemarks;
            this.entityPM.IsCancelled = true;
            this.entityPM.CancelledDate = new Date();
            this.entityPM.PaymentChequeStatusCode = "4";
            this.entityPM.CancelledByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
            this.paymentChequePMService.update(this.entityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;
                    var myJournalExtendedPMService = new JournalExtendedPMService_1.JournalExtendedPMService();
                    myJournalExtendedPMService
                        .VoidJournal(_this.entityPM.Tenant, _this.entityPM.JournalId, "", "", "")
                        .subscribe(function (res) {
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                        _this.CurrentSession.StopBusyIndicator();
                        if (res.HasError) {
                            _this.ValidationErrorsList = res.ErrorsArray;
                        }
                    });
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    CancelChequeComponent = __decorate([
        core_1.Component({
            selector: 'CancelChequeComponent',
            moduleId: module.id,
            templateUrl: './CancelChequeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CancelChequeComponent);
    return CancelChequeComponent;
}(BaseComponent_1.BaseComponent));
exports.CancelChequeComponent = CancelChequeComponent;
//# sourceMappingURL=CancelChequeComponent.js.map