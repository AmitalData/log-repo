"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var AutomationItemViewModel = /** @class */ (function () {
    //private order: number =0;
    //get Order() {
    //    if (this.EntityPM) {
    //        this.order = this.EntityPM.Order;
    //    }
    //    return this.order;
    //}
    //set Order(newValue: number) {
    //    if (this.Order != newValue) {
    //        this.Order = newValue;
    //        this.EntityPM.Order = newValue;
    //    }
    //}
    function AutomationItemViewModel(entityPM) {
        this.IsShowArrowUpDown = false;
        //public KeyInActive: string;
        //public KeyInRequired: string;
        this.description = "";
        this.name = "";
        this.EntityPM = entityPM;
        this.Id = entityPM.Id;
        this.CreateDate = entityPM.CreateDate;
        this.UpdateDate = entityPM.UpdateDate;
        this.CreatedByUserName = entityPM.CreatedByUserName;
        this.UpdatedByUserName = entityPM.UpdatedByUserName;
        this.ResultCode = entityPM.ResultCode;
        this.From = entityPM.From;
        this.FromEmail = entityPM.FromEmail;
        this.Inactive = entityPM.Inactive;
        this.Name = entityPM.Name;
        this.Description = entityPM.Description;
        this.Order = entityPM.Order;
        this.Version = entityPM.Version;
        this.Tenant = entityPM.Tenant;
        //"DOCOUTFOLLOWUP" || this.CurrentEntityPM.ResultCode == "DOCINFOLLOWUP"
        if (entityPM && !Tools_1.AppTool.IsNullOrEmpty(entityPM.ResultCode)) {
            if (entityPM.ResultCode == "EMAIL")
                this.ResultName = "E-mail";
            else if (entityPM.ResultCode == "FIELDSET")
                this.ResultName = "Set Fields Value";
            else if (entityPM.ResultCode == "FOLLOWUP")
                this.ResultName = "F/U Creation";
            else if (entityPM.ResultCode == "DOCINFOLLOWUP")
                this.ResultName = "Doc In F/U Creation";
            else if (entityPM.ResultCode == "DOCOUTFOLLOWUP")
                this.ResultName = "Doc Out F/U Creation";
            else if (entityPM.ResultCode == "SETSLA")
                this.ResultName = "Set SLA";
            else if (entityPM.ResultCode == "QUEUE")
                this.ResultName = "Queued Task";
        }
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Automation", "UPDATE"))
            this.IsEditAtomationEnable = false;
        else
            this.IsEditAtomationEnable = true;
    }
    Object.defineProperty(AutomationItemViewModel.prototype, "Description", {
        get: function () {
            if (this.EntityPM) {
                this.description = this.EntityPM.Description;
            }
            return this.description;
        },
        set: function (newValue) {
            if (this.Description != newValue) {
                this.Description = newValue;
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AutomationItemViewModel.prototype, "Name", {
        get: function () {
            if (this.EntityPM) {
                this.name = this.EntityPM.Name;
            }
            return this.name;
        },
        set: function (newValue) {
            if (this.Name != newValue) {
                this.Name = newValue;
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    return AutomationItemViewModel;
}());
exports.AutomationItemViewModel = AutomationItemViewModel;
//# sourceMappingURL=AutomationItemViewModel.js.map