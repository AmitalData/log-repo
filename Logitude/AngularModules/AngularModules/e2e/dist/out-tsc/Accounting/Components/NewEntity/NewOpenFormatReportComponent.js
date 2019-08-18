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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var OpenFormatReportPM_1 = require("../../EntityPMs/OpenFormatReportPM");
var OpenFormatReportPMService_1 = require("../../Services/StandardPMs/OpenFormatReportPMService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var NewOpenFormatReportComponent = /** @class */ (function (_super) {
    __extends(NewOpenFormatReportComponent, _super);
    function NewOpenFormatReportComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "OpenFormatReport";
        _this.DataContext = _this;
        _this.entityPM = new OpenFormatReportPM_1.OpenFormatReportPM();
        _this.OpenFormatReportPMService = new OpenFormatReportPMService_1.OpenFormatReportPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrorsList = [];
        _this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'OpenFormatReport'; })[0];
        _this.testingMode = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "TestingMode") && f.ObjectTableId == table.Id; })[0];
        return _this;
    }
    Object.defineProperty(NewOpenFormatReportComponent.prototype, "FromDate", {
        //get DateTypeCode() { return this.entityPM.DateTypeCode; }
        //set DateTypeCode(value: string) {
        //    if (this.entityPM.DateTypeCode != value) {
        //        this.entityPM.DateTypeCode = value;
        //    }
        //}
        get: function () { return this.entityPM.FromDate; },
        set: function (value) {
            if (this.entityPM.FromDate != value) {
                this.entityPM.FromDate = value;
                if (this.ToDate < value) {
                    this.entityPM.UIProperties.SetValidity("FromoDate", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpenFormatReportComponent.prototype, "ToDate", {
        get: function () { return this.entityPM.ToDate; },
        set: function (value) {
            if (this.entityPM.ToDate != value) {
                this.entityPM.ToDate = value;
                if (this.FromDate > value) {
                    this.entityPM.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
                }
                if (value > Tools_1.DateTool.GetCurrentDateTimeAsUtc()) {
                    this.entityPM.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FutureDateNotAllowed"));
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpenFormatReportComponent.prototype, "TestingMode", {
        get: function () { return this.entityPM.TestingMode; },
        set: function (value) {
            if (this.entityPM.TestingMode != value) {
                this.entityPM.TestingMode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewOpenFormatReportComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = new Date();
        var errors = [];
        //this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (this.ToDate < this.FromDate) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("");
            this.OpenFormatReportPMService.insert(this.entityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: _this.ObjectTableName });
                        cmpRef.instance.BackCompleted.subscribe(function ($event) {
                            _this.CancelButtonClicked();
                        });
                    });
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    NewOpenFormatReportComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewOpenFormatReportComponent = __decorate([
        core_1.Component({
            selector: 'NewOpenFormatReportComponent',
            moduleId: module.id,
            templateUrl: './NewOpenFormatReportComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewOpenFormatReportComponent);
    return NewOpenFormatReportComponent;
}(BaseComponent_1.BaseComponent));
exports.NewOpenFormatReportComponent = NewOpenFormatReportComponent;
//# sourceMappingURL=NewOpenFormatReportComponent.js.map