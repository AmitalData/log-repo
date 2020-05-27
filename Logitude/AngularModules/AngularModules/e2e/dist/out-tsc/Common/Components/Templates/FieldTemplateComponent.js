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
var Tools_1 = require("../../../Infrastructure/Tools");
var FieldTemplateComponent = /** @class */ (function () {
    function FieldTemplateComponent(cd) {
        this.cd = cd;
        this.Entity = null;
        this.FieldName = null;
        this.FieldValue = null;
        this.ObjectTableName = null;
        this.SpotlightDataTemplate = null;
        this.IsSpotLightTemplate = false;
        this.isPotentialCustomer = false;
        this.IsHeaderScreenTemplate = false;
        // Rank
        this.RankSource1 = null;
        this.RankSource2 = null;
        this.RankSource3 = null;
        this.SharedLogisticsInvitationStatusColor = null;
        this.LastShipmentDateColor = null;
        this.LastShipmentDateValue = null;
        this.AgentSharedManifestStatusColor = null;
        this.StartWorkingDateValue = null;
    }
    FieldTemplateComponent.prototype.Run = function (args) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];
        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];
            if (this.ObjectTableName == "Customer") {
                if (this.FieldName == "RankCode") {
                    this.SetRanksSource();
                }
                else if (this.FieldName == "LastShipmentDate") {
                    this.SetLastShipmentDateTemplate();
                }
                else if (this.FieldName == "StartWorkingDate") {
                    this.SetStartWorkingDateTemplate();
                }
                else if (this.FieldName == "SharedLogisticsInvitationStatusName") {
                    this.SetSharedLogisticsInvitationStatusTemplate();
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.Entity.CustomerStatusTemplateCode)) {
                    this.isPotentialCustomer = true;
                }
            }
            if (this.ObjectTableName == "AgentSharedManifest") {
                if (this.FieldName == "StatusName") {
                    this.SetAgentSharedManifestStatusColorTemplate();
                }
            }
            if (this.ObjectTableName == "CustomsShipper") {
                if (this.FieldName == "ValidityEndDate") {
                    this.CustomsShipperValidityEndDateBackgroudColor = this.transform(this.FieldValue);
                }
            }
        }
    };
    FieldTemplateComponent.prototype.transform = function (fieldValue) {
        var myFieldDate = new Date(fieldValue);
        var myResult = Tools_1.FontTool.Green;
        if (myFieldDate) {
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            if (myFieldDate.valueOf() <= todayDate.valueOf()) {
                myResult = Tools_1.FontTool.Red;
            }
            else if (Tools_1.DateTool.AddDays(todayDate, 30).valueOf() > myFieldDate.valueOf()) {
                myResult = Tools_1.FontTool.Orange;
            }
        }
        return myResult;
    };
    FieldTemplateComponent.prototype.SetRanksSource = function () {
        var RankCode = this.Entity['RankCode'];
        switch (RankCode) {
            case "1": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarGray.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
                break;
            }
            case "2": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarOrange.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
                break;
            }
            case "3": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarOrange.png";
                this.RankSource3 = "./Images/Icons/StarOrange.png";
                break;
            }
            default: {
                this.RankSource1 = "./Images/Icons/StarGray.png";
                this.RankSource2 = "./Images/Icons/StarGray.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
            }
        }
    };
    FieldTemplateComponent.prototype.SetSharedLogisticsInvitationStatusTemplate = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FieldValue)) {
            if (this.FieldValue == "Activated") {
                this.SharedLogisticsInvitationStatusColor = "Green";
            }
            else if (this.FieldValue == "Invited") {
                this.SharedLogisticsInvitationStatusColor = "Orange";
            }
            else if (this.FieldValue == "Not Invited") {
                this.SharedLogisticsInvitationStatusColor = "Red";
            }
        }
    };
    FieldTemplateComponent.prototype.SetLastShipmentDateTemplate = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FieldValue)) {
            var myFieldDate = new Date(this.FieldValue);
            if (myFieldDate.valueOf() >= Tools_1.DateTool.GetDateByDay(-7).valueOf()) {
                this.LastShipmentDateColor = "Green";
            }
            else if (myFieldDate.valueOf() >= Tools_1.DateTool.GetDateByDay(-30).valueOf()) {
                this.LastShipmentDateColor = "Orange";
            }
            else {
                this.LastShipmentDateColor = "Red";
            }
            var days = Tools_1.DateTool.GetDaysBetweenDates(myFieldDate, Tools_1.DateTool.GetCurrentDateAsUtc(), true);
            if (days == 0) {
                this.LastShipmentDateValue = "Today";
            }
            if (days == 1) {
                this.LastShipmentDateValue = "Yesterday";
            }
            if (days > 1 && days < 31) {
                this.LastShipmentDateValue = days + " Days";
            }
            if (days >= 31 && days < 1095) {
                var months = +(days / 31).toString().split(".")[0];
                if (months == 1) {
                    this.LastShipmentDateValue = months + " Month";
                }
                else {
                    this.LastShipmentDateValue = months + " Months";
                }
            }
            if (days > 1095) {
                var years = +(days / 365).toString().split(".")[0];
                if (years == 1) {
                    this.LastShipmentDateValue = years + " Year";
                }
                else {
                    this.LastShipmentDateValue = years + " Years";
                }
            }
        }
    };
    FieldTemplateComponent.prototype.SetAgentSharedManifestStatusColorTemplate = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FieldValue)) {
            if (this.FieldValue == "Cancelled") {
                this.AgentSharedManifestStatusColor = "Black";
            }
            else if (this.FieldValue == "Waiting") {
                this.AgentSharedManifestStatusColor = "Orange";
            }
            else {
                this.AgentSharedManifestStatusColor = "Green";
            }
        }
    };
    FieldTemplateComponent.prototype.SetStartWorkingDateTemplate = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FieldValue)) {
            var myFieldDate = new Date(this.FieldValue);
            var LastDate = Tools_1.DateTool.GetDateParts(myFieldDate).DateObject;
            LastDate.setHours(0);
            var TodayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            TodayDate.setHours(0);
            if (LastDate != null ? LastDate.valueOf() > TodayDate.valueOf() : true)
                return null;
            var days = Tools_1.DateTool.GetDaysBetweenDates(LastDate, TodayDate);
            if (days == 0) {
                this.StartWorkingDateValue = "Today";
            }
            if (days == 1) {
                this.StartWorkingDateValue = "Yesterday";
            }
            if (days > 1 && days < 31) {
                this.StartWorkingDateValue = days + " Days";
            }
            if (days > 31 && days < 1095) {
                var months = +(days / 31).toString().split(".")[0];
                if (months == 1) {
                    this.StartWorkingDateValue = months + " Month";
                }
                else {
                    this.StartWorkingDateValue = months + " Months";
                }
            }
            if (days > 1095) {
                var years = +(days / 365).toString().split(".")[0];
                if (years == 1) {
                    this.StartWorkingDateValue = years + " Year";
                }
                else {
                    this.StartWorkingDateValue = years + " Years";
                }
            }
        }
    };
    FieldTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FieldTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], FieldTemplateComponent);
    return FieldTemplateComponent;
}());
exports.FieldTemplateComponent = FieldTemplateComponent;
//# sourceMappingURL=FieldTemplateComponent.js.map