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
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var QuoteUtilities_1 = require("../../../../Quote/Utilities/QuoteUtilities");
var DateTimePipe_1 = require("../../../../Controls/Pipes/DateTimePipe");
var QuoteDomainService_1 = require("../../../../Quote/Services/QuoteDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Args_1 = require("../../../../CRM/Args");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CRMDomainService_1 = require("../../../../CRM/Services/CRMDomainService");
var GeneralEmailSender_1 = require("../../../../Infrastructure/Helpers/GeneralEmailSender");
var Tools_2 = require("../../../../CRM/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var OverviewTabComponent = /** @class */ (function (_super) {
    __extends(OverviewTabComponent, _super);
    function OverviewTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "Quote";
        _this.ActivitiesList = [];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.RegardingEntity = "";
        _this.EntityId = "";
        _this.EntityDescription = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SessionEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.IsQuoteEditEnabled = true;
        _this.IsClosingReasonVisible = false;
        _this.IsAddActivityEnabled = true;
        //Social 
        _this.FollowersCount = 0;
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.EntityId = _this.EntityPM.Id;
        _this.EntityDescription = _this.EntityPM.Subject;
        _this.RegardingEntity = "Regarding Quote : " + _this.EntityPM.QuoteNumber;
        _this.Listen();
        _this.Initialize();
        _this.LoadActivities();
        return _this;
    }
    OverviewTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.SetUIProperties();
        }
    };
    OverviewTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "LoadActivity") {
                    _this.LoadActivities();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                }
            });
        }
    };
    OverviewTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    OverviewTabComponent.prototype.Initialize = function () {
        this.myQuoteDomainService = new QuoteDomainService_1.QuoteDomainService();
    };
    OverviewTabComponent.prototype.SetUIProperties = function () {
        this.IsQuoteEditEnabled = QuoteUtilities_1.QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("StageId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("StageDueDate", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("RatingCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("QuoteClosingReasonCode", this.ObjectTableName, false);
        var isClosingReasonVisibile = this.EntityPM.IsClosed && !Tools_1.AppTool.IsNullOrEmpty(this.QuoteClosingReasonCode);
        this.UIProperties.SetVisibility("QuoteClosingReasonCode", this.ObjectTableName, isClosingReasonVisibile);
        this.IsClosingReasonVisible = isClosingReasonVisibile;
    };
    Object.defineProperty(OverviewTabComponent.prototype, "StageId", {
        //Properties
        get: function () { return this.EntityPM.StageId; },
        set: function (newValue) {
            if (this.EntityPM.StageId != newValue) {
                this.EntityPM.StageId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "StageDueDate", {
        get: function () { return this.EntityPM.StageDueDate; },
        set: function (newValue) {
            if (this.EntityPM.StageDueDate != newValue) {
                this.EntityPM.StageDueDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "RatingCode", {
        get: function () { return this.EntityPM.RatingCode; },
        set: function (newValue) {
            if (this.EntityPM.RatingCode != newValue) {
                this.EntityPM.RatingCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "QuoteClosingReasonCode", {
        get: function () { return this.EntityPM.QuoteClosingReasonCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "ControlIsEnabled", {
        get: function () {
            var myResult = true;
            if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewTabComponent.prototype, "ActivitiesContent", {
        //Activities
        get: function () {
            return TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.S.Overview.Activities") + " (" + this.ActivitiesList.length + ")";
        },
        enumerable: true,
        configurable: true
    });
    OverviewTabComponent.prototype.AddActivity = function (code) {
        var _this = this;
        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        switch (code.toUpperCase()) {
            case "TS":
                {
                    windowTitle = "New Task";
                    windowTitleIcon = "./Images/Activities/TS.png";
                    break;
                }
            case "CL": {
                windowTitle = "New Phone Call";
                windowTitleIcon = "./Images/Activities/CL.png";
                break;
            }
            case "AP": {
                windowTitle = "New Appointment";
                windowTitleIcon = "./Images/Activities/AP.png";
                logWindow.Width = 800;
                logWindow.Height = 600;
                break;
            }
            default: {
                break;
            }
        }
        var windowArgs = new Args_1.ActivityInputArgs();
        windowArgs.TypeCode = code;
        if (code.toUpperCase() == "CL") {
            windowArgs.CallWithId = this.EntityPM.CustomerContactId;
            windowArgs.IsOpen = false;
            windowArgs.IsMarkedCompleted = true;
        }
        //windowArgs.IsAddCustomerAllowed = true;
        windowArgs.CustomerId = this.EntityPM.CustomerId;
        windowArgs.QuoteId = this.EntityPM.Id;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;
        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.LoadActivities();
                }
            });
        });
    };
    OverviewTabComponent.prototype.AddEmail = function () {
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender_1.GeneralEmailSender("Quote", "QUOT", this.EntityPM.Id, this.EntityPM.QuoteNumber, "", "", "", "", null, "LoadActivity", this.EntityPM, true, "QEMO");
            this.EmailSender.ShowFullSendControll();
        }
    };
    OverviewTabComponent.prototype.LoadActivities = function () {
        var _this = this;
        this.ActivitiesList = [];
        this.myQuoteDomainService.GetActivitiesByQuoteId(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var dataResult = myResponse.Result;
                if (dataResult.length > 0) {
                    dataResult.filter(function (d) { return d.IsOpen == true; }).sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.SortingDate) === Tools_1.DateTool.GetDateFromDate(b.SortingDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.SortingDate) > Tools_1.DateTool.GetDateFromDate(b.SortingDate)) ? -1 : 1; }).forEach(function (item) {
                        _this.ActivitiesList.push(new ActivityItemClass(item, _this));
                    });
                    dataResult.filter(function (d) { return d.IsOpen == false; }).sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.SortingDate) === Tools_1.DateTool.GetDateFromDate(b.SortingDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.SortingDate) > Tools_1.DateTool.GetDateFromDate(b.SortingDate)) ? -1 : 1; }).forEach(function (item) {
                        _this.ActivitiesList.push(new ActivityItemClass(item, _this));
                    });
                }
            }
        });
    };
    OverviewTabComponent.prototype.ViewEntity = function (entity) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(entity.Id)) {
            this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: "Activity", BackButtonLabel: "Quotes" });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.LoadActivities();
                    });
                });
            });
        }
    };
    OverviewTabComponent.prototype.Updated = function (arg) {
        if (arg) {
            this.LoadActivities();
        }
    };
    OverviewTabComponent = __decorate([
        core_1.Component({
            selector: 'OverviewTabComponent',
            moduleId: module.id,
            templateUrl: './OverviewTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], OverviewTabComponent);
    return OverviewTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OverviewTabComponent = OverviewTabComponent;
var ActivityItemClass = /** @class */ (function (_super) {
    __extends(ActivityItemClass, _super);
    function ActivityItemClass(item, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        // Properties
        _this.background = "rgb(255,255,255)";
        _this.DateLable = "";
        _this.DateValue = "";
        _this.DueDateForeground = "";
        // Action Fields
        _this.Action = "";
        _this.ActionBy = "";
        _this.completeVisi = false;
        _this.reopenVisi = false;
        _this.completetogVisi = false;
        _this.entity = item;
        _this.EntityId = item.Id;
        _this.ImageSrc = Tools_2.CRMTool.GetActivityImageSrc(_this.entity.ActivityTypePathCode);
        _this.GetDueDateForeground();
        _this.GetDateValue();
        _this.GetDateLable();
        _this.GetAction();
        _this.GetActionBy();
        _this.getActionDate();
        return _this;
    }
    Object.defineProperty(ActivityItemClass.prototype, "Background", {
        get: function () {
            if (!this.entity.IsOpen) {
                this.background = "rgba(0,0,0,0.1)";
            }
            return this.background;
        },
        set: function (value) {
            this.background = value;
        },
        enumerable: true,
        configurable: true
    });
    ActivityItemClass.prototype.ControlIsEnabled = function () { return this.entity.IsOpen; };
    Object.defineProperty(ActivityItemClass.prototype, "ActivityTypePathCode", {
        get: function () { return this.entity.ActivityTypePathCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "Id", {
        get: function () { return this.entity.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "CallWithId", {
        get: function () { return this.entity.CallWithId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "ActivityTypeName", {
        get: function () { return this.entity.ActivityTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "ActivityTypeCode", {
        get: function () { return this.entity.ActivityTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "Subject", {
        get: function () { return this.entity.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "Owner", {
        get: function () { return this.entity.OwnerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "CustomerId", {
        get: function () { return this.entity.CustomerId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "SortingBy", {
        get: function () { return this.entity.SortingBy; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "DueDate", {
        get: function () { return this.entity.DueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "StartDate", {
        get: function () { return this.entity.StartDateTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "SortingDate", {
        get: function () { return this.entity.SortingDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "MeetingSummary", {
        get: function () { return this.entity.MeetingSummary; },
        set: function (value) {
            if (this.entity.MeetingSummary != value) {
                this.entity.MeetingSummary = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "PostToFollowers", {
        get: function () { return this.entity.PostToFollowers; },
        set: function (value) {
            if (this.entity.PostToFollowers != value) {
                this.entity.PostToFollowers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityItemClass.prototype.GetDateLable = function () {
        var myResult = "Due Date";
        switch (this.ActivityTypeCode) {
            case "TS":
                {
                    if (this.DueDate != null) {
                        myResult = "Due Date";
                    }
                    else {
                        myResult = "Start Date";
                    }
                    break;
                }
            case "AP":
                {
                    myResult = "Start Date";
                    break;
                }
            case "EO":
                {
                    myResult = "To";
                    break;
                }
            case "EI":
                {
                    myResult = "From";
                    break;
                }
        }
        this.DateLable = myResult;
    };
    ActivityItemClass.prototype.GetDateValue = function () {
        var DatePipe = new DateTimePipe_1.DateTimePipe();
        var myResult = "";
        if (this.DueDate != null) {
            myResult = DatePipe.transform(this.DueDate, "SD");
        }
        switch (this.ActivityTypeCode) {
            case "TS":
                {
                    if (this.DueDate != null) {
                        myResult = DatePipe.transform(this.DueDate, "SD");
                    }
                    else if (this.StartDate != null) {
                        myResult = DatePipe.transform(this.StartDate, "SD");
                    }
                    break;
                }
            case "AP":
                {
                    if (this.StartDate != null) {
                        myResult = DatePipe.transform(this.StartDate, "SD");
                    }
                    break;
                }
            case "EO":
                {
                    myResult = this.entity.RecipientsEmails;
                    break;
                }
            case "EI":
                {
                    break;
                }
        }
        this.DateValue = myResult;
    };
    ActivityItemClass.prototype.GetDueDateForeground = function () {
        var result = "rgb(40,46,48)";
        if (this.DueDate != null && this.DueDate.valueOf() < Tools_1.DateTool.GetCurrentDateTimeAsUtc().valueOf()) {
            result = "Red";
        }
        this.DueDateForeground = result;
    };
    ActivityItemClass.prototype.GetAction = function () {
        var myResult = "Modified by";
        if (this.entity.ActivityTypeCode == "EI") {
            myResult = "Recorded by";
        }
        else if (this.entity.ActivityTypeCode == "EO") {
            myResult = "Sent by";
        }
        else {
            switch (this.entity.ActivityStatusCode) {
                case "C":
                    {
                        myResult = "Completed by";
                        break;
                    }
                case "X":
                    {
                        myResult = "Closed by";
                        break;
                    }
                default:
                    {
                        myResult = "Modified by";
                        break;
                    }
            }
        }
        this.Action = myResult;
    };
    ActivityItemClass.prototype.GetActionBy = function () {
        var myResult = this.entity.UpdatedByUserName;
        if (!this.entity.IsOpen) {
            if (this.entity.ActivityTypeCode == "EO") {
                myResult = this.entity.CreatedByUserName;
            }
        }
        this.ActionBy = myResult;
    };
    ActivityItemClass.prototype.getActionDate = function () {
        var myResult = this.entity.UpdateDate;
        if (!this.entity.IsOpen) {
            if (this.entity.ActivityStatusCode == "C") {
                myResult = this.entity.CompleteDate;
            }
        }
        this.ActionDate = myResult;
    };
    Object.defineProperty(ActivityItemClass.prototype, "CompleteButtonVisibility", {
        get: function () {
            if (this.entity.IsOpen && this.entity.ActivityTypeCode != "AP") {
                this.completeVisi = true;
            }
            return this.completeVisi;
        },
        set: function (value) { this.completeVisi = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "ReopenButtonVisibility", {
        get: function () {
            if (!this.entity.IsOpen) {
                if (this.entity.ActivityTypeCode != "EI" && this.entity.ActivityTypeCode != "EO") {
                    this.reopenVisi = true;
                }
            }
            return this.reopenVisi;
        },
        set: function (value) { this.reopenVisi = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "CompleteToggleButtonVisibility", {
        get: function () {
            if (this.entity.IsOpen && this.entity.ActivityTypeCode == "AP") {
                this.completetogVisi = true;
            }
            return this.completetogVisi;
        },
        set: function (value) { this.completetogVisi = value; },
        enumerable: true,
        configurable: true
    });
    // Commands
    ActivityItemClass.prototype.CompleteClicked = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetCompleteActivity(this.EntityId, this.PostToFollowers, this.MeetingSummary).subscribe(function (resp) {
            if (!resp.HasError) {
                _this.entity = resp.Result;
                _this.father.LoadActivities();
                _this.ReopenButtonVisibility = true;
                _this.CompleteButtonVisibility = false;
                _this.CompleteToggleButtonVisibility = false;
                _this.Background = "rgba(0,0,0,0.1)";
            }
        });
    };
    ActivityItemClass.prototype.ReopenClicked = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetReopenActivity(this.EntityId).subscribe(function (resp) {
            if (!resp.HasError) {
                _this.entity = resp.Result;
                _this.father.LoadActivities();
                _this.ReopenButtonVisibility = false;
                if (_this.entity.ActivityTypeCode == "AP") {
                    _this.CompleteToggleButtonVisibility = true;
                }
                else {
                    _this.CompleteButtonVisibility = true;
                }
                _this.Background = "rgb(255,255,255)";
            }
        });
    };
    return ActivityItemClass;
}(BaseComponent_1.BaseComponent));
exports.ActivityItemClass = ActivityItemClass;
//# sourceMappingURL=OverviewTabComponent.js.map