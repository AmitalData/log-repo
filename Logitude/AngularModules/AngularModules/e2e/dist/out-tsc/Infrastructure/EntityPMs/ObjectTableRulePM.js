"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UIProperties_1 = require("../../Infrastructure/Components/LogitudeComponents/UIProperties");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var ObjectTableRulePM = /** @class */ (function () {
    function ObjectTableRulePM() {
        this.UIProperties = new UIProperties_1.UIProperties();
        this.IsDirty = false;
    }
    Object.defineProperty(ObjectTableRulePM.prototype, "RuleConditionFields", {
        get: function () {
            if (this.ruleConditionFields == null) {
                this.ruleConditionFields = [];
            }
            return this.ruleConditionFields;
        },
        set: function (newValue) {
            if (this.ruleConditionFields != newValue) {
                this.ruleConditionFields = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ObjectTableRulePM.prototype.AddRuleConditionField = function (item) {
        if (item != null) {
            var index = this.RuleConditionFields.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.RuleConditionFields.push(item);
                this.MarkAsDirty();
            }
        }
    };
    ObjectTableRulePM.prototype.RemoveRuleConditionField = function (item) {
        if (item != null) {
            var index = this.RuleConditionFields.indexOf(item);
            if (index > -1) {
                this.RuleConditionFields.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    };
    ObjectTableRulePM.prototype.MarkAsDirty = function () {
        this.IsDirty = true;
    };
    ObjectTableRulePM.prototype.CloneMe = function () {
        ServiceHelper_1.ServiceHelper.CloneEntityPM(this);
    };
    ObjectTableRulePM.prototype.RejectChanges = function () {
        ServiceHelper_1.ServiceHelper.RejectEntityPMChanges(this);
    };
    return ObjectTableRulePM;
}());
exports.ObjectTableRulePM = ObjectTableRulePM;
//# sourceMappingURL=ObjectTableRulePM.js.map