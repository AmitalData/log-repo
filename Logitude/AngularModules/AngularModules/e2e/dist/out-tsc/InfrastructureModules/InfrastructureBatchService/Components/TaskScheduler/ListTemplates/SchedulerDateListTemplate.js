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
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var SchedulerExtendedPMService_1 = require("../../../../../Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var SchedulerDateListTemplate = /** @class */ (function () {
    function SchedulerDateListTemplate(CD) {
        this.CD = CD;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.schedulerExtendedPMService = new SchedulerExtendedPMService_1.SchedulerExtendedPMService();
    }
    SchedulerDateListTemplate.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        if (fieldName == "Duration") {
            var startDate = new Date(rowData["StartDateTime"]);
            var endDate = new Date(rowData["EndDateTime"]);
            var seconds = ((endDate.getTime() - startDate.getTime()) / 1000).toFixed(2);
            if (startDate.getFullYear() > 1970 && endDate.getFullYear() > 1970) {
                this.dateValue = seconds + " sec";
            }
        }
        else if (fieldName == "Log") {
            this.dateValue = rowData["LogFirstLine"];
            this.Type = rowData["LogType"];
        }
        else {
            var pmDate = new Date(rowData[fieldName]);
            if (pmDate.getFullYear() > 1970) {
                this.dateValue = pmDate;
            }
        }
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    SchedulerDateListTemplate.prototype.ShowFullLog = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.schedulerExtendedPMService.GetSchedulerHistoryLogs(this.rowData["Id"]).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var windowArgs = {};
                if (myResponse.Result) {
                    windowArgs.TextValue = myResponse.Result.Log;
                }
                else {
                    windowArgs.TextValue = "";
                }
                windowArgs.DisplayMode = true;
                var wind = new LogitudeWindow_1.LogitudeWindow();
                wind.Width = 960;
                wind.Height = 570;
                wind.WindowArgs = windowArgs;
                wind.Title = "";
                wind.Show("./Infrastructure/Component/LogitudeComponents/MultilineTextBoxWindow");
            }
            //else {
            //    this.ValidationErrorsList = myResponse.ErrorsArray;
            //}
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    SchedulerDateListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SchedulerDateListTemplate',
            templateUrl: './SchedulerDateListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], SchedulerDateListTemplate);
    return SchedulerDateListTemplate;
}());
exports.SchedulerDateListTemplate = SchedulerDateListTemplate;
//# sourceMappingURL=SchedulerDateListTemplate.js.map