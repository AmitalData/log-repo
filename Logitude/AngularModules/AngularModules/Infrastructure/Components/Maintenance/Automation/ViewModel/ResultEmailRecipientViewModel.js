"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Guid_1 = require('../../../../../Infrastructure/Utilities/Guid');
var BaseComponent_1 = require('../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent');
var ResultEmailRecipientViewModel = (function (_super) {
    __extends(ResultEmailRecipientViewModel, _super);
    function ResultEmailRecipientViewModel(entityPM, addEditAutomationsComponent) {
        _super.call(this);
        this.FieldName = entityPM.FieldName;
        this.Id = entityPM.Id;
        this.Tenant = entityPM.Tenant;
        this.FullName = entityPM.AutomateFieldDefaultText;
        this.Key = Guid_1.Guid.newGuid();
        this.EntityContactVariable = addEditAutomationsComponent.EntityContactVariable;
        this.AddEditAutomationsComponent = addEditAutomationsComponent;
        if (this.AddEditAutomationsComponent.EntityContactVariable && this.AddEditAutomationsComponent.EntityContactVariable.length > 0 && this.AddEditAutomationsComponent.EntityContactVariable.indexOf(entityPM.Id) != -1) {
            this.IsChecked = true;
        }
        else
            this.IsChecked = false;
    }
    ResultEmailRecipientViewModel.prototype.ngOnInit = function () {
    };
    ResultEmailRecipientViewModel.prototype.CheckedResultEmailRecipient = function (item) {
        if (this.AddEditAutomationsComponent.EntityContactVariable.indexOf(item.Id) == -1) {
            this.AddEditAutomationsComponent.EntityContactVariable.push(item.Id);
        }
        else {
            this.AddEditAutomationsComponent.EntityContactVariable = this.AddEditAutomationsComponent.EntityContactVariable.filter(function (d) { return d != item.Id; });
        }
        this.AddEditAutomationsComponent.IsChangeAutomation = true;
    };
    return ResultEmailRecipientViewModel;
}(BaseComponent_1.BaseComponent));
exports.ResultEmailRecipientViewModel = ResultEmailRecipientViewModel;
var Operator = (function () {
    function Operator(name, code) {
        this.Code = code;
        this.Name = name;
    }
    return Operator;
}());
//# sourceMappingURL=ResultEmailRecipientViewModel.js.map