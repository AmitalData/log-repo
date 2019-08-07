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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("../../../../../Infrastructure/Locators/ObjectsLocator");
var TaxReportPMService_1 = require("../../../../Services/StandardPMs/TaxReportPMService");
var TaxReportLinePMService_1 = require("../../../../Services/StandardPMs/TaxReportLinePMService");
var EditTaxReportLineComponent = /** @class */ (function (_super) {
    __extends(EditTaxReportLineComponent, _super);
    function EditTaxReportLineComponent() {
        var _this = _super.call(this) || this;
        _this.TaxReportPM = null;
        _this.TaxReportLinePM = null;
        _this.ObjectTableName = "TaxReportLine";
        _this.DataContext = _this;
        _this.isRTL = false;
        _this.ValidationErrorsList = [];
        _this._TaxReportPMService = new TaxReportPMService_1.TaxReportPMService();
        _this._TaxReportLinePMService = new TaxReportLinePMService_1.TaxReportLinePMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        return _this;
    }
    EditTaxReportLineComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.TaxReportPM = args.TaxReportPM;
            this.TaxReportLinePM = args.TaxReportLinePM;
            // Copy original values
            this.OldTransmitStatusCode = this.TransmitStatusCode;
            this.OldVatNumber = this.VatNumber;
            this.OldReference = this.Reference;
            this.OldReferecneGroup = this.ReferecneGroup;
            this.OldReferenceDate = this.ReferenceDate;
            this.SetUIProperties();
        }
    };
    Object.defineProperty(EditTaxReportLineComponent.prototype, "TransmitStatusCode", {
        //DeferredGLAccount
        get: function () { return this.TaxReportLinePM.TransmitStatusCode; },
        set: function (value) {
            if (this.TaxReportLinePM.TransmitStatusCode != value) {
                this.TaxReportLinePM.TransmitStatusCode = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditTaxReportLineComponent.prototype, "VatNumber", {
        //VatNumber
        get: function () { return this.TaxReportLinePM.VatNumber; },
        set: function (value) {
            if (this.TaxReportLinePM.VatNumber != value) {
                this.TaxReportLinePM.VatNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditTaxReportLineComponent.prototype, "Reference", {
        //Reference
        get: function () { return this.TaxReportLinePM.Reference; },
        set: function (value) {
            if (this.TaxReportLinePM.Reference != value) {
                this.TaxReportLinePM.Reference = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditTaxReportLineComponent.prototype, "ReferecneGroup", {
        //ReferecneGroup
        get: function () { return this.TaxReportLinePM.ReferecneGroup; },
        set: function (value) {
            if (this.TaxReportLinePM.ReferecneGroup != value) {
                this.TaxReportLinePM.ReferecneGroup = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditTaxReportLineComponent.prototype, "ReferenceDate", {
        //ReferenceDate
        get: function () { return this.TaxReportLinePM.ReferenceDate; },
        set: function (value) {
            if (this.TaxReportLinePM.ReferenceDate != value) {
                this.TaxReportLinePM.ReferenceDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    EditTaxReportLineComponent.prototype.SetUIProperties = function () {
        if (this.TaxReportLinePM.OutputOrInput == "O") {
            this.UIProperties.SetEnabled("TransmitStatusCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Reference", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferecneGroup", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferenceDate", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("Reference", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferecneGroup", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ReferenceDate", this.ObjectTableName, false);
        }
        this.UIProperties.SetRequired("TransmitStatusCode", this.ObjectTableName, !this.TransmitStatusCode);
    };
    //#region Buttons
    EditTaxReportLineComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (!this.TaxReportLinePM.TransmitStatusCode) {
            var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate('TaxReportLine.F.TransmitStatusCode');
            var translatedRequiredError = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var fieldError = translatedRequiredError.replace("%FieldName", fieldName);
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(fieldError);
            return;
        }
        // update line
        this.TaxReportLinePM.IsManuallyChanged = true;
        //update report
        this.TaxReportPM.NeedsRebulid = true;
        // save(reprot)
        this.CurrentSession.StartBusyIndicatorSaving();
        this._TaxReportLinePMService.update(this.TaxReportLinePM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                // this.CurrentSession.CloseCurrentWindowEmit("ok");
                _this._TaxReportPMService.update(_this.TaxReportPM).subscribe(function (myResult) {
                    var mm = myResult;
                    if (!mm.HasError) {
                        // this._TaxReportLinePMService.update(this.TaxReportLinePM).subscribe(myResult => {
                        //     var mm: ServiceResponse = myResult;
                        //     if (!mm.HasError) {
                        //         this.CurrentSession.CloseCurrentWindowEmit("ok");
                        //     }
                        //     else {
                        //         this.ValidationErrorsList = mm.ErrorsArray;
                        //         this.CurrentSession.StopBusyIndicator();
                        //     }
                        // });
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                        _this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    EditTaxReportLineComponent.prototype.CancelButtonClicked = function () {
        this.TransmitStatusCode = this.OldTransmitStatusCode;
        this.VatNumber = this.OldVatNumber;
        this.Reference = this.OldReference;
        this.ReferecneGroup = this.OldReferecneGroup;
        this.ReferenceDate = this.OldReferenceDate;
        this.CurrentSession.CloseCurrentWindow();
    };
    EditTaxReportLineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditTaxReportLineComponent.html'
        }),
        __metadata("design:paramtypes", [])
    ], EditTaxReportLineComponent);
    return EditTaxReportLineComponent;
}(BaseComponent_1.BaseComponent));
exports.EditTaxReportLineComponent = EditTaxReportLineComponent;
//# sourceMappingURL=EditTaxReportLineComponent.js.map