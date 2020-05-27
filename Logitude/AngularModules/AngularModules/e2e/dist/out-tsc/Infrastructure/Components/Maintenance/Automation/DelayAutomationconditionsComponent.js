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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var AutomationCondition_1 = require("../../../../Infrastructure/DataContracts/AutomationCondition");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var AutomationConditionViewModel_1 = require("./ViewModel/AutomationConditionViewModel");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DelayAutomationconditionsComponent = /** @class */ (function (_super) {
    __extends(DelayAutomationconditionsComponent, _super);
    function DelayAutomationconditionsComponent() {
        var _this = _super.call(this) || this;
        _this.AutomationCondationOrList = [];
        _this.AutomationCondationAndList = [];
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    DelayAutomationconditionsComponent.prototype.ngOnInit = function () {
    };
    DelayAutomationconditionsComponent.prototype.SetDataContext = function (dataContext) {
        this.addEditAutomationsComponent = dataContext;
        this.BuildAutomationCondition();
    };
    DelayAutomationconditionsComponent.prototype.BuildAutomationCondition = function () {
        var _this = this;
        this.IsViewCondition = false;
        var automationConditionPMList = this.addEditAutomationsComponent.AutomatedBackupClass.DelayAautomationConditionLists;
        if (automationConditionPMList != null) {
            automationConditionPMList.forEach(function (item) {
                if (item.ConditionType == "And") {
                    _this.AutomationCondationAndList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(item, _this.addEditAutomationsComponent, _this));
                }
                else if (item.ConditionType == "Or") {
                    _this.AutomationCondationOrList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(item, _this.addEditAutomationsComponent, _this));
                }
            });
        }
    };
    DelayAutomationconditionsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DelayAutomationconditionsComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        var automationConditionList = [];
        this.AutomationCondationAndList.forEach(function (item) {
            item.CurrentEntityPM.UpdateDate = _this.addEditAutomationsComponent.CurrentEntityPM.UpdateDate;
            item.CurrentEntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            automationConditionList.push(item.CurrentEntityPM);
        });
        this.AutomationCondationOrList.forEach(function (item) {
            item.CurrentEntityPM.UpdateDate = _this.addEditAutomationsComponent.CurrentEntityPM.UpdateDate;
            item.CurrentEntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            automationConditionList.push(item.CurrentEntityPM);
        });
        this.addEditAutomationsComponent.AutomatedBackupClass.DelayAautomationConditionLists = automationConditionList;
        this.CurrentSession.CloseCurrentWindow();
    };
    DelayAutomationconditionsComponent.prototype.AddAutomationConditionMethod = function (conditionType) {
        var automationConditionPM = new AutomationCondition_1.AutomationCondition();
        automationConditionPM.ConditionType = conditionType;
        automationConditionPM.Tenant = SessionLocator_1.SessionLocator.TenantPM.Id;
        automationConditionPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        automationConditionPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        automationConditionPM.Value = "";
        automationConditionPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        automationConditionPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        automationConditionPM.OperatorCode = "Equals";
        automationConditionPM.ObjectFieldId = "";
        if (conditionType == "And") {
            this.AutomationCondationAndList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(automationConditionPM, this.addEditAutomationsComponent, this));
        }
        else {
            this.AutomationCondationOrList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(automationConditionPM, this.addEditAutomationsComponent, this));
        }
    };
    DelayAutomationconditionsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DelayAutomationconditionsComponent',
            templateUrl: './DelayAutomationconditionsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DelayAutomationconditionsComponent);
    return DelayAutomationconditionsComponent;
}(BaseComponent_1.BaseComponent));
exports.DelayAutomationconditionsComponent = DelayAutomationconditionsComponent;
//# sourceMappingURL=DelayAutomationconditionsComponent.js.map