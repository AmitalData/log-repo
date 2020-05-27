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
var OccasionPM_1 = require("../../../../CRM/EntityPMs/OccasionPM");
var OccasionPMService_1 = require("../../../../CRM/Services/StandardPMs/OccasionPMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var NewOccasionComponent = /** @class */ (function (_super) {
    __extends(NewOccasionComponent, _super);
    function NewOccasionComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Occasion";
        _this.DataContext = _this;
        _this.EntityPM = new OccasionPM_1.OccasionPM();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrorsList = [];
        _this.InitiateOccasion();
        _this.SetUIProperties();
        return _this;
    }
    NewOccasionComponent.prototype.InitiateOccasion = function () {
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new OccasionPM_1.OccasionPM();
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.EntityPM.OwnerId = SessionLocator_1.SessionLocator.LoggedUserId;
    };
    NewOccasionComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("OccasionTypeId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OccasionTypeId));
    };
    Object.defineProperty(NewOccasionComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) { if (this.EntityPM.Name != value)
            this.EntityPM.Name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOccasionComponent.prototype, "StartDateTime", {
        get: function () { return this.EntityPM.StartDateTime; },
        set: function (value) { if (this.EntityPM.StartDateTime != value)
            this.EntityPM.StartDateTime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOccasionComponent.prototype, "EndDateTime", {
        get: function () { return this.EntityPM.EndDateTime; },
        set: function (value) { if (this.EntityPM.EndDateTime != value)
            this.EntityPM.EndDateTime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOccasionComponent.prototype, "Location", {
        get: function () { return this.EntityPM.Location; },
        set: function (value) { if (this.EntityPM.Location != value)
            this.EntityPM.Location = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOccasionComponent.prototype, "Goal", {
        get: function () { return this.EntityPM.Goal; },
        set: function (value) { if (this.EntityPM.Goal != value)
            this.EntityPM.Goal = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOccasionComponent.prototype, "IndustryId", {
        get: function () { return this.EntityPM.IndustryId; },
        set: function (value) { if (this.EntityPM.IndustryId != value)
            this.EntityPM.IndustryId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOccasionComponent.prototype, "OwnerId", {
        get: function () { return this.EntityPM.OwnerId; },
        set: function (value) { if (this.EntityPM.OwnerId != value)
            this.EntityPM.OwnerId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOccasionComponent.prototype, "OccasionStatusId", {
        get: function () { return this.EntityPM.OccasionStatusId; },
        set: function (value) { if (this.EntityPM.OccasionStatusId != value)
            this.EntityPM.OccasionStatusId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOccasionComponent.prototype, "OccasionTypeId", {
        get: function () { return this.EntityPM.OccasionTypeId; },
        set: function (value) {
            if (this.EntityPM.OccasionTypeId != value) {
                this.EntityPM.OccasionTypeId = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewOccasionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewOccasionComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.OccasionTypeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Occasion.F.OccasionTypeId")));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorCreating();
            var service = new OccasionPMService_1.OccasionPMService();
            service.insert(this.EntityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    NewOccasionComponent = __decorate([
        core_1.Component({
            selector: 'NewOccasionComponent',
            moduleId: module.id,
            templateUrl: './NewOccasionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewOccasionComponent);
    return NewOccasionComponent;
}(BaseComponent_1.BaseComponent));
exports.NewOccasionComponent = NewOccasionComponent;
//# sourceMappingURL=NewOccasionComponent.js.map