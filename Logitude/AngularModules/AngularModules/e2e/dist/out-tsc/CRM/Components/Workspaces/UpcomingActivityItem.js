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
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CRMDomainService_1 = require("../../Services/CRMDomainService");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ActivityValidator_1 = require("../../Validators/ActivityValidator");
var ActivityPMService_1 = require("../../Services/StandardPMs/ActivityPMService");
var Tools_2 = require("../../Tools");
var UpcomingActivityItem = /** @class */ (function (_super) {
    __extends(UpcomingActivityItem, _super);
    function UpcomingActivityItem(entityList, ActivityWorkspaceComponent, OverviewWorkspaceComponent) {
        var _this = _super.call(this) || this;
        _this.ActivityWorkspaceComponent = ActivityWorkspaceComponent;
        _this.OverviewWorkspaceComponent = OverviewWorkspaceComponent;
        _this.ObjectTableName = "Activity";
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsCompleteVisible = false;
        _this.IsReopenVisible = false;
        _this.IsCompleteToggleVisible = false;
        _this.entityList = entityList;
        _this.EntityId = entityList.Id;
        _this.ImageSrc = Tools_2.CRMTool.GetActivityImageSrc(_this.entityList.ActivityTypePathCode);
        _this.SetCompleteButtonsVisibility();
        _this.GetCompletedBackground();
        if (_this.entityList.ActivityTypeCode == "AP") {
            _this.IsCompleted = true;
        }
        return _this;
    }
    Object.defineProperty(UpcomingActivityItem.prototype, "PriorityCode", {
        get: function () { return this.entityList.PriorityCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "PriorityName", {
        get: function () { return this.entityList.PriorityName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "ActivityTypeName", {
        get: function () { return this.entityList.ActivityTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "Subject", {
        get: function () { return this.entityList.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "UpcomingDate", {
        get: function () { return this.entityList.UpcomingDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "CustomerName", {
        get: function () { return this.entityList.CustomerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "OwnerName", {
        get: function () { return this.entityList.OwnerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "OpportunityId", {
        get: function () { return this.entityList.OpportunityId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "CustomerId", {
        get: function () { return this.entityList.CustomerId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "QuoteId", {
        get: function () { return this.entityList.QuoteId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "MeetingSummary", {
        get: function () { return this.entityList.MeetingSummary; },
        set: function (value) {
            if (this.entityList.MeetingSummary != value) {
                this.entityList.MeetingSummary = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "PostToFollowers", {
        get: function () { return this.entityList.PostToFollowers; },
        set: function (value) {
            if (this.entityList.PostToFollowers != value) {
                this.entityList.PostToFollowers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "IsCompleted", {
        get: function () { return this.isCompleted; },
        set: function (value) {
            this.isCompleted = value;
            if (value) {
                this.PostToFollowers = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpcomingActivityItem.prototype, "IsOpen", {
        get: function () { return this.entityList.IsOpen; },
        enumerable: true,
        configurable: true
    });
    UpcomingActivityItem.prototype.SetCompleteButtonsVisibility = function () {
        if (this.entityList.IsOpen) {
            if (this.entityList.ActivityTypeCode == "AP") {
                this.IsReopenVisible = false;
                this.IsCompleteToggleVisible = true;
                this.IsCompleteVisible = false;
            }
            else {
                this.IsReopenVisible = false;
                this.IsCompleteToggleVisible = false;
                this.IsCompleteVisible = true;
            }
        }
        else {
            this.IsReopenVisible = true;
            this.IsCompleteToggleVisible = false;
            this.IsCompleteVisible = false;
        }
    };
    UpcomingActivityItem.prototype.GetCompletedBackground = function () {
        if (this.IsOpen) {
            this.CompletedBackground = "rgb(255,255,255)";
        }
        else {
            this.CompletedBackground = "rgba(0,0,0,0.1)";
        }
    };
    UpcomingActivityItem.prototype.ViewConnectedEntity = function (code) {
        var _this = this;
        var tableName = null;
        var entityId = null;
        switch (code) {
            case "OPP": {
                tableName = "Opportunity";
                entityId = this.entityList.OpportunityId;
                break;
            }
            case "CUS": {
                tableName = "Customer";
                entityId = this.entityList.CustomerId;
                break;
            }
            case "QUT": {
                tableName = "Quote";
                entityId = this.entityList.QuoteId;
                break;
            }
        }
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: tableName, BackButtonLabel: 'Activity' });
            var isEditComponentSaved = false;
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                if (isEditComponentSaved) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            });
            cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
        });
    };
    UpcomingActivityItem.prototype.CompleteClicked = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetCompleteActivity(this.entityList.Id, this.PostToFollowers, this.MeetingSummary).subscribe(function (resp) {
            if (!resp.HasError) {
                _this.entityList = resp.Result;
                if (_this.ActivityWorkspaceComponent) {
                    _this.ActivityWorkspaceComponent.LoadAllScreenData();
                }
                if (_this.OverviewWorkspaceComponent) {
                    //this.OverviewWorkspaceComponent.LoadActivitiesSummary();
                    _this.SetCompleteButtonsVisibility();
                    _this.GetCompletedBackground();
                }
            }
        });
    };
    UpcomingActivityItem.prototype.ReopenClicked = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetReopenActivity(this.entityList.Id).subscribe(function (resp) {
            if (!resp.HasError) {
                _this.entityList = resp.Result;
                if (_this.ActivityWorkspaceComponent) {
                    _this.ActivityWorkspaceComponent.LoadAllScreenData();
                }
                if (_this.OverviewWorkspaceComponent) {
                    //this.OverviewWorkspaceComponent.LoadActivitiesSummary();
                    _this.SetCompleteButtonsVisibility();
                    _this.GetCompletedBackground();
                }
            }
        });
    };
    UpcomingActivityItem.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    UpcomingActivityItem.prototype.OkButtonClicked = function () {
        var errors = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MeetingSummary)) {
            if (this.MeetingSummary.length > 5000) {
                errors.push("Meeting Summary must be less\nthan 5000 char");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.LoadActivityPM();
        }
    };
    UpcomingActivityItem.prototype.LoadActivityPM = function () {
        var _this = this;
        var myService = new ActivityPMService_1.ActivityPMService();
        myService.get(this.entityList.Id).subscribe(function (resp) {
            if (!resp.HasError) {
                var entityPM = resp.Result;
                entityPM.IsOpen = false;
                entityPM.MeetingSummary = _this.MeetingSummary;
                var activityValidator = new ActivityValidator_1.ActivityValidator();
                var validEntry = activityValidator.Validate(entityPM);
                if (validEntry && validEntry.length == 0) {
                    _this.CompleteClicked();
                }
                else {
                    entityPM.IsOpen = true;
                }
            }
        });
    };
    return UpcomingActivityItem;
}(BaseComponent_1.BaseComponent));
exports.UpcomingActivityItem = UpcomingActivityItem;
//# sourceMappingURL=UpcomingActivityItem.js.map