var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import { Component } from '@angular/core';
import { AutomationCondition } from '../../../../Infrastructure/DataContracts/AutomationCondition';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AutomationConditionViewModel } from './ViewModel/AutomationConditionViewModel';
import { DateTool } from '../../../../Infrastructure/Tools';
export var DelayAutomationconditionsComponent = (function (_super) {
    __extends(DelayAutomationconditionsComponent, _super);
    function DelayAutomationconditionsComponent() {
        _super.call(this);
        this.AutomationCondationOrList = [];
        this.AutomationCondationAndList = [];
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
                    _this.AutomationCondationAndList.push(new AutomationConditionViewModel(item, _this.addEditAutomationsComponent, _this));
                }
                else if (item.ConditionType == "Or") {
                    _this.AutomationCondationOrList.push(new AutomationConditionViewModel(item, _this.addEditAutomationsComponent, _this));
                }
            });
        }
    };
    DelayAutomationconditionsComponent.prototype.CloseButtonClicked = function () {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    };
    DelayAutomationconditionsComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        var automationConditionList = [];
        this.AutomationCondationAndList.forEach(function (item) {
            item.CurrentEntityPM.AutomationsId = _this.addEditAutomationsComponent.CurrentEntityPM.Id;
            item.CurrentEntityPM.UpdateDate = _this.addEditAutomationsComponent.CurrentEntityPM.UpdateDate;
            item.CurrentEntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            automationConditionList.push(item.CurrentEntityPM);
        });
        this.AutomationCondationOrList.forEach(function (item) {
            item.CurrentEntityPM.AutomationsId = _this.addEditAutomationsComponent.CurrentEntityPM.Id;
            item.CurrentEntityPM.UpdateDate = _this.addEditAutomationsComponent.CurrentEntityPM.UpdateDate;
            item.CurrentEntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            automationConditionList.push(item.CurrentEntityPM);
        });
        this.addEditAutomationsComponent.AutomatedBackupClass.DelayAautomationConditionLists = automationConditionList;
        SessionLocator.CurrentSession.CloseCurrentWindow();
    };
    DelayAutomationconditionsComponent.prototype.AddAutomationConditionMethod = function (conditionType) {
        var automationConditionPM = new AutomationCondition();
        automationConditionPM.ConditionType = conditionType;
        automationConditionPM.Tenant = SessionLocator.TenantPM.Id;
        automationConditionPM.CreatedByUserId = SessionLocator.LoggedUserId;
        automationConditionPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        automationConditionPM.Value = "";
        automationConditionPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        automationConditionPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        automationConditionPM.OperatorCode = "Equals";
        automationConditionPM.ObjectFieldId = "";
        automationConditionPM.AutomationsId = this.addEditAutomationsComponent.CurrentEntityPM.Id;
        if (conditionType == "And") {
            this.AutomationCondationAndList.push(new AutomationConditionViewModel(automationConditionPM, this.addEditAutomationsComponent, this));
        }
        else {
            this.AutomationCondationOrList.push(new AutomationConditionViewModel(automationConditionPM, this.addEditAutomationsComponent, this));
        }
    };
    DelayAutomationconditionsComponent.decorators = [
        { type: Component, args: [{
                    moduleId: module.id,
                    selector: 'DelayAutomationconditionsComponent',
                    templateUrl: './DelayAutomationconditionsComponent.html',
                },] },
    ];
    /** @nocollapse */
    DelayAutomationconditionsComponent.ctorParameters = [];
    return DelayAutomationconditionsComponent;
}(BaseComponent));
//# sourceMappingURL=DelayAutomationconditionsComponent.js.map