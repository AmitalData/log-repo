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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var SLAEscalationRecepientPM_1 = require("../../../../CRM/EntityPMs/SLAEscalationRecepientPM");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AddEditEscalationComponent = /** @class */ (function (_super) {
    __extends(AddEditEscalationComponent, _super);
    function AddEditEscalationComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "SLAEscalation";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditEscalationComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.entityPM;
        this.Clone();
    };
    // Commands
    AddEditEscalationComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditEscalationComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var isTimeRequired = this.DataContext.EscalationActionTimeIndicator == "IM" ? false : true;
        if (isTimeRequired) {
            if (this.DataContext.EscalationTime == null || Tools_1.AppTool.IsNullOrEmpty(this.DataContext.EscalationTimeUnit)) {
                errors.push("Time field is required");
            }
        }
        var isPredefinitionEmpty = this.DataContext.PreDefinitionList.filter(function (a) { return a.IsChecked == true; })[0];
        if ((this.DataContext.UserSelectedList == null || this.DataContext.UserSelectedList.length == 0) && (!isPredefinitionEmpty)) {
            errors.push("User or Recipient is required");
        }
        // User Component work not done
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.isNew) {
                this.DataContext.isNew = false;
                if (this.DataContext.UserSelectedList != null && this.DataContext.UserSelectedList.length > 0) {
                    this.DataContext.UserSelectedList.forEach(function (item) {
                        var escalationLine = new SLAEscalationRecepientPM_1.SLAEscalationRecepientPM(_this.EntityPM);
                        escalationLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        escalationLine.SLAEscalationId = _this.EntityPM.Id;
                        escalationLine.UserId = item.Id;
                        if (_this.EntityPM.SLAEscalationRecepients.indexOf(escalationLine) == -1) {
                            _this.EntityPM.AddSLAEscalationRecepient(escalationLine);
                        }
                    });
                }
                if (this.DataContext.PreDefinitionList != null && this.DataContext.PreDefinitionList.length > 0) {
                    this.DataContext.PreDefinitionList.forEach(function (item) {
                        if (item.IsChecked) {
                            var escalationLine = new SLAEscalationRecepientPM_1.SLAEscalationRecepientPM(_this.EntityPM);
                            escalationLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            escalationLine.SLAEscalationId = _this.EntityPM.Id;
                            escalationLine.PreDefinitionId = item.Code;
                            if (_this.EntityPM.SLAEscalationRecepients.indexOf(escalationLine) == -1) {
                                _this.EntityPM.AddSLAEscalationRecepient(escalationLine);
                            }
                        }
                    });
                }
                if (this.DataContext.EscalationFor == "FR") {
                    if (this.DataContext.trigger.FirstResponseEscalationDataList.indexOf(this.DataContext) == -1) {
                        this.DataContext.trigger.FirstResponseEscalationDataList.push(this.DataContext);
                    }
                    if (this.DataContext.trigger.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "RW"; }).indexOf(this.EntityPM) == -1) {
                        this.DataContext.trigger.entityPM.AddSLAEscalation(this.EntityPM);
                    }
                }
                else {
                    if (this.DataContext.trigger.ResolveEscalationDataList.indexOf(this.DataContext) == -1) {
                        this.DataContext.trigger.ResolveEscalationDataList.push(this.DataContext);
                    }
                    if (this.DataContext.trigger.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "FR"; }).indexOf(this.EntityPM) == -1) {
                        this.DataContext.trigger.entityPM.AddSLAEscalation(this.EntityPM);
                    }
                }
            }
            else {
                this.DataContext.BuildEscalationRecepients();
            }
            if (this.DataContext.EscalationFor == "FR") {
                this.DataContext.trigger.FillResponseEscalationList();
            }
            if (this.DataContext.EscalationFor == "RW") {
                this.DataContext.trigger.FillResolveEscalationList();
            }
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    AddEditEscalationComponent.prototype.Clone = function () {
        var _this = this;
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('IsChecked');
        this.myCloner.AddField('Name');
        this.myCloner.AddField('EscalationActionTimeIndicator');
        this.myCloner.AddField('EscalationTime');
        this.myCloner.AddField('EscalationTimeUnit');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.entityPM);
        this.myCloner.AddEntity(this.DataContext.trigger.entityPM);
        this.DataContext.PreDefinitionList.forEach(function (item) {
            _this.myCloner.AddEntity(item);
        });
        this.myCloner.AddEntity(this.DataContext.PreDefinitionList);
        this.myCloner.AddEntity(this.DataContext.UserSelectedList);
    };
    AddEditEscalationComponent.prototype.RejectChanges = function () {
        this.DataContext.FillPredefinitionList();
        this.myCloner.RejectChanges();
    };
    AddEditEscalationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditEscalationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditEscalationComponent);
    return AddEditEscalationComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditEscalationComponent = AddEditEscalationComponent;
//# sourceMappingURL=AddEditEscalationComponent.js.map