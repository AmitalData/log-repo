"use strict";
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
var CustomerSalesNotePM_1 = require("../../EntityPMs/CustomerSalesNotePM");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var CustomerHelperComponent = /** @class */ (function () {
    function CustomerHelperComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.IsEditingEnabled = false;
        this.IsSalesNotesVisible = false;
        this.IsSupportNotesVisible = false;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.Listen();
            this.BuildComponent();
        }
    }
    CustomerHelperComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    }
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    }
                });
            }
        }
    };
    CustomerHelperComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    CustomerHelperComponent.prototype.BuildComponent = function () {
        var isSalesNotesVisible = false;
        var isSupportNotesVisible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "SALES")) {
            isSalesNotesVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "TICKET")) {
            isSupportNotesVisible = true;
        }
        this.IsSalesNotesVisible = isSalesNotesVisible;
        this.IsSupportNotesVisible = isSupportNotesVisible;
    };
    Object.defineProperty(CustomerHelperComponent.prototype, "SupportNotes", {
        get: function () { return this.EntityPM.SupportNotes; },
        set: function (value) {
            if (this.EntityPM.SupportNotes != value) {
                this.EntityPM.SupportNotes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerHelperComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerHelperComponent.prototype.AddSalesNoteClicked = function () {
        var itemPM = new CustomerSalesNotePM_1.CustomerSalesNotePM(null);
        var maxDate = null;
        var nowDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        var maxDateTicks = 0;
        var nowDateTicks = Tools_1.DateTool.GetDateParts(nowDate).DateTicks;
        if (this.EntityPM.SalesNotes.length > 0) {
            this.EntityPM.SalesNotes.forEach(function (item) {
                var itemDateTicks = Tools_1.DateTool.GetDateParts(item.UpdateDate).DateTicks;
                if (itemDateTicks > maxDateTicks) {
                    maxDateTicks = itemDateTicks;
                    maxDate = item.UpdateDate;
                }
            });
        }
        if (maxDate == null) {
            maxDate = nowDate;
        }
        else if (maxDateTicks < nowDateTicks) {
            maxDate = nowDate;
        }
        else {
            maxDate = Tools_1.DateTool.AddHours(maxDate, 1);
        }
        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        itemPM.CustomerId = this.EntityPM.Id;
        itemPM.CreateDate = maxDate;
        itemPM.UpdateDate = maxDate;
        itemPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        itemPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        itemPM.CreatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        itemPM.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        this.RunAddEditSalesNoteWindow(itemPM, true);
    };
    CustomerHelperComponent.prototype.EditSalesNoteClicked = function (itemPM) {
        if (itemPM) {
            this.RunAddEditSalesNoteWindow(itemPM, false);
        }
    };
    CustomerHelperComponent.prototype.RunAddEditSalesNoteWindow = function (itemPM, isNewEntity) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Height = 320;
        logitudeWindow.Width = 450;
        logitudeWindow.Title = isNewEntity ? "New Sales Note" : "Edit Sales Note";
        logitudeWindow.HelpText = "The maximum number of characters allowed in this field is 500.";
        logitudeWindow.ShowHelpIcon = true;
        logitudeWindow.WindowArgs = { CustomerPM: this.EntityPM, EntityPM: itemPM, IsNewEntity: isNewEntity };
        logitudeWindow.Show('./Common/Components/Helpers/AddEditCustomerSalesNoteComponent');
    };
    CustomerHelperComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./CustomerHelperComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomerHelperComponent);
    return CustomerHelperComponent;
}());
exports.CustomerHelperComponent = CustomerHelperComponent;
//# sourceMappingURL=CustomerHelperComponent.js.map