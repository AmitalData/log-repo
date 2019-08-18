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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CourierMasterPM_1 = require("../../../../Customs/EntityPMs/CourierMasterPM");
var CourierMasterPMService_1 = require("../../../../Customs/Services/StandardPMs/CourierMasterPMService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CourierMasterService_1 = require("../../../../Customs/Services/Others/CourierMasterService");
var NewCourierComponent = /** @class */ (function (_super) {
    __extends(NewCourierComponent, _super);
    function NewCourierComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.CourierMaster";
        _this.DataContext = _this;
        _this.CourierMasterService = new CourierMasterService_1.CourierMasterService();
        _this.CourierMasterPMService = new CourierMasterPMService_1.CourierMasterPMService();
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ValidationErrorsList = [];
        _this.QueryNameText = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new CourierMasterPM_1.CourierMasterPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.MAWBTypeCode = "740";
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        return _this;
    }
    NewCourierComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.QueryNameText = Tools_1.AppTool.IsNullOrEmpty(args.QueryNameTextCode) ? "Back" : TextCodeTranslator_1.TextCodeTranslator.Translate(args.QueryNameTextCode);
        }
    };
    NewCourierComponent.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    Object.defineProperty(NewCourierComponent.prototype, "AirlineId", {
        get: function () { return this.EntityPM.AirlineId; },
        set: function (value) {
            if (this.EntityPM.AirlineId != value) {
                this.EntityPM.AirlineId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCourierComponent.prototype, "MAWB", {
        get: function () { return this.EntityPM.MAWB; },
        set: function (value) {
            if (this.EntityPM.MAWB != value) {
                this.EntityPM.MAWB = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCourierComponent.prototype, "HAWB", {
        get: function () { return this.EntityPM.HAWB; },
        set: function (value) {
            if (this.EntityPM.HAWB != value) {
                this.EntityPM.HAWB = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewCourierComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        // Custom Validation
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CourierMasterService.GetIfCourierMasterExists(this.EntityPM.Id, this.AirlineId, this.HAWB, this.MAWB).subscribe(function (Result) {
                var mm = Result;
                if (!mm.HasError) {
                    if (!mm.Result) {
                        _this.SubmitChanges();
                    }
                    else {
                        errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.CourierAlreadyExist")); //"There is already master courier with the same values");
                        _this.ValidationErrorsList = errors; //.push(TextCodeTranslator.Translate("Customs.General.O.CourierAlreadyExist"));//"There is already master courier with the same values");
                    }
                }
            });
        }
    };
    NewCourierComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewCourierComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        this.CourierMasterPMService.insert(this.EntityPM).subscribe(function (Result) {
            var mm = Result;
            if (!mm.HasError) {
                _this.CurrentSession.StopBusyIndicator();
                _this.CurrentSession.CloseCurrentWindowEmit("ok");
                var entity = mm.Result;
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: _this.ObjectTableName, BackButtonLabel: _this.QueryNameText });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.CancelButtonClicked();
                    });
                });
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewCourierComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewCourierComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewCourierComponent);
    return NewCourierComponent;
}(BaseComponent_1.BaseComponent));
exports.NewCourierComponent = NewCourierComponent;
//# sourceMappingURL=NewCourierComponent.js.map