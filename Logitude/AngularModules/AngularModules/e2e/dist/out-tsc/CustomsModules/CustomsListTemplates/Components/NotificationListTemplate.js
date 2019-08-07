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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var NotificationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/NotificationExtendedListService");
var NotificationListTemplate = /** @class */ (function () {
    function NotificationListTemplate(CD) {
        this.CD = CD;
        this.IconeVisibility = false;
        this.BlueIconVisibility = false;
        this.ShowGreenTick = true;
        this.notificationExtendedListService = new NotificationExtendedListService_1.NotificationExtendedListService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ClosedByAssignee = null;
        this.IsClosed = false;
    }
    NotificationListTemplate.prototype.setVariables = function (rowData, fieldName, additionalData) {
        var _this = this;
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = additionalData;
        this.AdditionalData.RowOverEvent.subscribe(function (index) {
            if (_this.IsClosed && fieldName == 'IsClosedByAssignee') {
                var template = document.getElementById('Text' + index);
                var template1 = document.getElementById('Tick' + index);
                if (template) {
                    template.style.display = 'block';
                }
                if (template1) {
                    template1.style.display = 'none';
                }
            }
            //alert("RowOverEvent");
        });
        this.AdditionalData.RowOutEvent.subscribe(function (index) {
            if (_this.IsClosed && fieldName == 'IsClosedByAssignee') {
                var template = document.getElementById('Text' + index);
                var template1 = document.getElementById('Tick' + index);
                if (template) {
                    template.style.display = 'none';
                }
                if (template1) {
                    template1.style.display = 'block';
                }
            }
        });
        this.IsSeenByAssignee = rowData.IsSeenByAssignee;
        if (rowData.IsClosedByAssignee) {
            this.IsClosed = true;
        }
        if (this.IsSeenByAssignee) {
            console.log("seen");
        }
        if (this.rowData.AssigneToNotificationTypeCode == "A") {
            this.IconeVisibility = true;
            this.BlueIconVisibility = false;
        }
        else {
            this.IconeVisibility = false;
        }
        if (this.rowData.NotificationDefinitionCode == "5101N") {
            this.BlueIconVisibility = true;
            this.IconeVisibility = false;
        }
        var valueDate = new Date(rowData.DueDate.valueOf()).valueOf();
        var today = Tools_1.DateTool.GetCurrentDateAsUtc().valueOf();
        if (valueDate != null && valueDate < today) {
            this.datecolor = "#ff6a00";
            this.fontcolor = "#ffffff";
        }
        else {
            this.datecolor = "#F0F0F0";
            this.fontcolor = "#6E7172";
        }
        var isDestroyed = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    };
    NotificationListTemplate.prototype.ClosedByAssigneeClicked = function () {
        var _this = this;
        this.CurrentSession.FireEvent({ Name: 'ClosedByAssigneeClicked', rowIndex: this.AdditionalData.rowIndex, gridId: this.AdditionalData.gridId });
        this.notificationExtendedListService.PutNotificationsStatus(this.rowData).subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                    _this.IsClosed = true;
                    _this.ClosedByAssignee = SessionLocator_1.SessionLocator.LoggedUserPM.LocalName;
                }
            }
        });
    };
    NotificationListTemplate.prototype.FirePreventSelect = function () {
        this.CurrentSession.PseventRowSelectEvent.emit("select");
    };
    NotificationListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NotificationListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], NotificationListTemplate);
    return NotificationListTemplate;
}());
exports.NotificationListTemplate = NotificationListTemplate;
//# sourceMappingURL=NotificationListTemplate.js.map