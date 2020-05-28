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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ActivityPM_1 = require("../../../../CRM/EntityPMs/ActivityPM");
var Args_1 = require("../../../../CRM/Args");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ActivityTypeListService_1 = require("../../../../CRM/Services/StandardLists/ActivityTypeListService");
var Tools_2 = require("../../../../CRM/Tools");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ContactInputTemplate_1 = require("../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate");
var CallTypeListService_1 = require("../../../../CRM/Services/StandardLists/CallTypeListService");
var ActivityInviteePM_1 = require("../../../../CRM/EntityPMs/ActivityInviteePM");
var Args_2 = require("../../../../Infrastructure/Args");
var ActivityPriorityListService_1 = require("../../../../CRM/Services/StandardLists/ActivityPriorityListService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var GeneralDomainService_1 = require("../../../../Infrastructure/Services/GeneralDomainService");
var ActivityInputTemplate = /** @class */ (function (_super) {
    __extends(ActivityInputTemplate, _super);
    function ActivityInputTemplate() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Activity";
        _this.DataContext = _this;
        _this.CallTypesList = [];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.TypeCode = "";
        _this.CustomerVisibility = false;
        _this.AddContactEnabled = true;
        _this.OnEditModeVisibility = false;
        _this.IsMettingSummeryVisibile = true;
        _this.Retries = 0;
        _this.ApTimeVisibility = true;
        _this.callWithContact = null;
        _this.Email = "";
        _this.BusinessPhone = "";
        _this.Mobile = "";
        _this.owner = null;
        _this.addCustomerVisibility = false;
        _this.MeetingSummaryFlowDirection = "ltr";
        _this.MeetingSummaryBackgroundAlignLeft = "transparent";
        _this.MeetingSummaryBackgroundAlignRight = "transparent";
        _this.DescriptionFlowDirection = "ltr";
        _this.DescriptionBackgroundAlignRight = "transparent";
        _this.DescriptionBackgroundAlignLeft = "transparent";
        _this.Invitees_Optional = [];
        _this.Invitees_Required = [];
        _this.errors = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DocumentDatasList = [];
        _this.entityPM = new ActivityPM_1.ActivityPM();
        //ActivityPMInitService.InitValues(this.entityPM, true);
        _this.CallTypesList = [];
        return _this;
    }
    ActivityInputTemplate.prototype.InitTemplate = function (args) {
        if (args != null) {
            this.entityPM = args.Activity;
            if (this.entityPM.ActivityTypeCode == "CL") {
                this.CallWithId = this.entityPM.CallWithId;
            }
            this.TypeCode = this.entityPM.ActivityTypeCode;
            this.CustomerVisibility = args.IsAddCustomerAllowed;
            this.AddCustomerVisibility = this.CustomerVisibility;
            this.AddContactEnabled = args.IsEnabled;
            this.OnEditModeVisibility = args.IsEditMode;
            this.AddCustomerEnabled = args.IsEnabled;
        }
        this.InitializeData();
    };
    ActivityInputTemplate.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.entityPM = args.Activity;
            if (this.entityPM.ActivityTypeCode == "CL") {
                this.CallWithId = args.Activity.CallWithId;
            }
            this.TypeCode = this.entityPM.ActivityTypeCode;
            this.CustomerVisibility = args.IsAddCustomerAllowed;
            this.AddCustomerVisibility = this.CustomerVisibility;
            this.AddContactEnabled = args.IsEnabled;
            this.OnEditModeVisibility = args.IsEditMode;
            this.AddCustomerEnabled = args.IsEnabled;
        }
        this.InitializeData();
    };
    ActivityInputTemplate.prototype.InitializeData = function () {
        var _this = this;
        this.Durations = [];
        Tools_2.CRMTool.GetDurationsList().forEach(function (item) {
            _this.Durations.push(item);
        });
        if (!this.OnEditModeVisibility) {
            this.SetActivityTypeName();
            if (this.TypeCode == "AP") {
                this.StartDateTime = Tools_2.CRMTool.RoundTimeForwardByMinutes(Tools_1.DateTool.GetCurrentDateTimeAsUtc(), 30);
                this.Duration = 30;
                this.entityPM.ActivityTimeTypeCode = "BS";
                this.OnDurationChanged();
            }
            else if (this.TypeCode == "CL") {
                this.CallTypeCode = "O";
            }
            this.SetUIProperties_New();
            this.InitializeRightToLeft();
        }
        else {
            this.UIProperties.SetEnabled("DueDateOffset", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DueDateDateField", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DueDateDefaultText", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BusinessProcessQueueId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("TeamId", this.ObjectTableName, false);
        }
        this.GetDueDateObjectField();
        this.GetDescriptionFlowDirection();
        this.GetMeetingSummaryFlowDirection();
        this.RefreshTextAlgimentVariables();
        this.RefreshDescriptionTextAlgimentVariables();
        if ((this.TypeCode == "EO" || this.TypeCode == "EI") && this.OnEditModeVisibility == true) {
            this.BuildInternalAttachmentList();
        }
        if (this.OnEditModeVisibility) {
            this.BuildInviteesControl();
        }
        this.SetUIProperties_Qwner();
    };
    ActivityInputTemplate.prototype.GetDueDateObjectField = function () {
        var _this = this;
        if (this.entityPM.ActivityTypeCode == "TX") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.DueDateDateField)) {
                var shipmentObjecttableId = window.ObjectTables.filter(function (f) { return f.Name == "Shipment"; })[0].Id;
                if (!Tools_1.AppTool.IsNullOrEmpty(shipmentObjecttableId)) {
                    var service = new GeneralDomainService_1.GeneralDomainService();
                    service.GetSingleObjectFieldByFieldNameAndTableId(this.entityPM.DueDateDateField, shipmentObjecttableId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myField = myResponse.Result;
                            if (myField != null) {
                                _this.DueDateDefaultText = myField.FullNameTextCodeDefaultText;
                            }
                        }
                    });
                }
            }
        }
    };
    ActivityInputTemplate.prototype.InitializeRightToLeft = function () {
        if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            if (this.entityPM != null) {
                this.entityPM.DescriptionRightToLeft = true;
                this.entityPM.MeetingSummaryRightToLeft = true;
            }
        }
    };
    ActivityInputTemplate.prototype.SetActivityTypeName = function () {
        var _this = this;
        var service = new ActivityTypeListService_1.ActivityTypeListService();
        service.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var typeList = myResponse.Result;
                if (typeList != null && typeList.length > 0) {
                    var type = typeList.filter(function (d) { return d.Code == _this.TypeCode; })[0];
                    _this.entityPM.ActivityTypeName = typeList.Name;
                }
            }
        });
    };
    ActivityInputTemplate.prototype.SetUIProperties_New = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
            this.UIProperties.SetEnabled("CustomerId", "Activity", false);
        }
        this.UIProperties.SetVisibility("OpportunityId", this.ObjectTableName, false);
        this.UIProperties.SetVisibility("QuoteId", this.ObjectTableName, false);
        switch (this.entityPM.ActivityTypeCode) {
            case "AP":
                {
                    this.UIProperties.SetRequired("StartDateTime", this.ObjectTableName, this.StartDateTime == null);
                    this.UIProperties.SetRequired("EndDateTime", this.ObjectTableName, this.EndDateTime == null);
                    this.UIProperties.SetVisibility("MeetingSummary", this.ObjectTableName, false);
                    this.IsMettingSummeryVisibile = false;
                    break;
                }
            case "CL":
                {
                    this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CallWithId));
                    break;
                }
        }
    };
    ActivityInputTemplate.prototype.SetUIProperties_Edit = function () {
        if (this.entityPM.ActivityTypeCode == "EI" || this.entityPM.ActivityTypeCode == "EO") {
            this.UIProperties.SetEnabled("CustomerName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Subject", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ArchiveDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CreateDate", this.ObjectTableName, false);
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
                this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.QuoteId)) {
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
        }
        switch (this.entityPM.ActivityTypeCode) {
            case "AP":
                {
                    this.UIProperties.SetRequired("StartDateTime", this.ObjectTableName, this.StartDateTime == null);
                    this.UIProperties.SetRequired("EndDateTime", this.ObjectTableName, this.EndDateTime == null);
                    break;
                }
            case "CL":
                {
                    this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.entityPM.CallWithId));
                    break;
                }
        }
        var isOppertunityVisible = !Tools_1.AppTool.IsNullOrEmpty(this.entityPM.OpportunityId);
        this.UIProperties.SetVisibility("OpportunityId", this.ObjectTableName, isOppertunityVisible);
        var isQuoteVisible = !Tools_1.AppTool.IsNullOrEmpty(this.entityPM.QuoteId) && Tools_1.AppTool.IsNullOrEmpty(this.entityPM.OpportunityId);
        this.UIProperties.SetVisibility("QuoteId", this.ObjectTableName, isQuoteVisible);
    };
    ActivityInputTemplate.prototype.SetUIProperties_Qwner = function () {
        var isQwnerRequired = false;
        if (this.TypeCode != "TX") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
                isQwnerRequired = true;
            }
        }
        this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, isQwnerRequired);
    };
    // Additional Feilds
    ActivityInputTemplate.prototype.ngOnInit = function () {
        this.RunComponent();
        this.FillCallTypeList();
    };
    ActivityInputTemplate.prototype.FillCallTypeList = function () {
        var _this = this;
        var myService = new CallTypeListService_1.CallTypeListService();
        myService.getAllFromCache().subscribe(function (resp) {
            if (!resp.HasError) {
                var list = resp.Result;
                _this.CallTypesList = list.sort(function (a, b) { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1; });
                //if (this.TypeCode == "CL") {
                //    this.SelectedCallType = this.CallTypesList.filter(d => d.Code == "O")[0];
                //}
            }
        });
    };
    ActivityInputTemplate.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ActivityInputTemplate.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ActivityInputTemplate.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            _this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.LoadCompleted.subscribe(function (s) {
                _this.SetUIProperties_GeneratedComponent();
            });
            if (_this.TypeCode == "AP") {
                cmpRef.instance.LabelWidth = 120;
            }
            else {
                cmpRef.instance.LabelWidth = 110;
            }
            cmpRef.instance.Run(_this.entityPM, _this.ObjectTableName, "Activity.AdditionalFields");
        });
    };
    ActivityInputTemplate.prototype.SetUIProperties_GeneratedComponent = function () {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.entityPM.IsOpen);
        }
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "IsControlsEnabled", {
        //Duration
        get: function () {
            var result = true;
            if (this.entityPM != null) {
                if (!this.entityPM.IsOpen) {
                    result = false;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "SelectedDuration", {
        get: function () {
            var _this = this;
            var result = null;
            if (this.Duration != null) {
                result = this.Durations.filter(function (d) { return d.Minuts == _this.Duration; })[0];
                if (result == null) {
                    result = new Tools_2.ActivtyDuration(this.Duration, "", false);
                    if (this.Duration < 60) {
                        var minutes = Math.floor(this.Duration);
                        result.Name = this.Duration + " minutes";
                    }
                    else if (this.Duration < 1440) {
                        var hours = Math.floor(this.Duration / 60);
                        result.Name = hours + " hours";
                    }
                    else {
                        var days = Math.floor(this.Duration / 1440);
                        result.Name = days + " days";
                        result.IsDay = true;
                    }
                    if (this.Durations.indexOf(result) == -1) {
                        this.Durations.push(result);
                    }
                }
            }
            return result;
        },
        set: function (value) {
            if (this.selectedDuration != value) {
                this.selectedDuration = value;
                if (value == null) {
                    this.Duration = null;
                }
                else {
                    this.Duration = this.selectedDuration.Minuts;
                }
                this.OnDurationChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "Duration", {
        get: function () { return this.entityPM.Duration; },
        set: function (value) {
            if (this.entityPM.Duration != value) {
                this.entityPM.Duration = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "StartDateTime", {
        get: function () { return this.entityPM.StartDateTime; },
        set: function (value) {
            if (this.entityPM.StartDateTime != value) {
                this.entityPM.StartDateTime = value;
                if (this.OnEditModeVisibility) {
                    this.SetUIProperties_Edit();
                }
                else {
                    this.SetUIProperties_New();
                }
                this.ComputeEndDate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "EndDateTime", {
        get: function () { return this.entityPM.EndDateTime; },
        set: function (value) {
            if (this.entityPM.EndDateTime != value) {
                this.entityPM.EndDateTime = value;
                if (this.OnEditModeVisibility) {
                    this.SetUIProperties_Edit();
                }
                else {
                    this.SetUIProperties_New();
                }
                this.OnEndDateTimeChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.OnEndDateTimeChanged = function () {
        this.Duration = null;
        if (this.EndDateTime != null && this.StartDateTime != null && this.EndDateTime.valueOf() > this.StartDateTime.valueOf()) {
            var totalmins = this.GetMinutesBetweenDates(this.StartDateTime, this.EndDateTime);
            if (totalmins != 0) {
                this.Duration = totalmins;
            }
        }
    };
    ActivityInputTemplate.prototype.GetMinutesBetweenDates = function (date1, date2) {
        var myResult = 0;
        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {
                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = Math.abs(d2.getTime() - d1.getTime());
                var Daysdiff = Math.ceil(timeDiff / (1000 * 60));
                myResult = Daysdiff;
            }
        }
        return myResult;
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "AllDayEvent", {
        get: function () { return this.entityPM.AllDayEvent; },
        set: function (value) {
            if (this.entityPM.AllDayEvent != value) {
                this.entityPM.AllDayEvent = value;
                this.OnAllDayEventChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.OnDurationChanged = function () {
        if ((this.SelectedDuration != null) && !this.SelectedDuration.IsDay) {
            this.entityPM.AllDayEvent = false;
        }
        this.ComputeEndDate();
    };
    ActivityInputTemplate.prototype.OnAllDayEventChanged = function () {
        if (this.AllDayEvent) {
            var days = 1;
            if (this.StartDateTime != null) {
                this.StartDateTime = Tools_1.DateTool.TruncateTime(this.StartDateTime);
            }
            if (this.EndDateTime != null) {
                this.EndDateTime = Tools_1.DateTool.TruncateTime(this.EndDateTime);
            }
            if (this.StartDateTime != null && this.EndDateTime != null && this.EndDateTime.valueOf() > this.StartDateTime.valueOf()) {
                days = Tools_1.DateTool.GetDaysBetweenDates(this.StartDateTime, this.EndDateTime);
            }
            this.Duration = (days * 24 * 60);
        }
        this.ComputeEndDate();
        this.ApTimeVisibility = this.IsTimeControlEnabled;
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "IsTimeControlEnabled", {
        get: function () { return !this.AllDayEvent; },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.ComputeEndDate = function () {
        if (this.Duration == null) {
            this.EndDateTime = this.StartDateTime;
        }
        else {
            if (this.StartDateTime != null) {
                var startDateTime = Tools_1.DateTool.GetDateFormats(this.StartDateTime).DateParts.DateObject;
                var date = new Date();
                date.setUTCFullYear(startDateTime.getUTCFullYear());
                date.setUTCMonth(startDateTime.getUTCMonth());
                date.setUTCDate(startDateTime.getUTCDate());
                date.setUTCHours(startDateTime.getUTCHours());
                date.setUTCMinutes(startDateTime.getUTCMinutes());
                date.setUTCSeconds(startDateTime.getUTCSeconds());
                date.setUTCMilliseconds(startDateTime.getUTCMilliseconds());
                date.setUTCMinutes(date.getUTCMinutes() + this.Duration);
                this.EndDateTime = date;
            }
        }
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "CallTypeCode", {
        // Call Type
        //get SelectedCallType() { return this.CallTypesList.filter(d => d.Code == this.entityPM.CallTypeCode)[0]; }
        //set SelectedCallType(value: CallTypeList) {
        //    if (value == null) {
        //        this.CallTypeCode = null;
        //    }
        //    else {
        //        this.CallTypeCode = value.Code;
        //    }
        //}
        get: function () { return this.entityPM.CallTypeCode; },
        set: function (value) {
            if (this.entityPM.CallTypeCode != value) {
                this.entityPM.CallTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "CallWithId", {
        get: function () { return this.entityPM.CallWithId; },
        set: function (value) {
            if (this.entityPM.CallWithId != value) {
                this.entityPM.CallWithId = value;
                this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(value));
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "IsLeftVoiceMail", {
        get: function () { return this.entityPM.IsLeftVoiceMail; },
        set: function (value) {
            if (this.entityPM.IsLeftVoiceMail != value) {
                this.entityPM.IsLeftVoiceMail = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "CallWithContact", {
        get: function () { return this.callWithContact; },
        set: function (value) {
            if (this.callWithContact != value) {
                this.callWithContact = value;
                this.OnContactChanged(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.OnContactChanged = function (contact) {
        this.Email = "";
        this.BusinessPhone = "";
        this.Mobile = "";
        if (contact) {
            this.Email = contact.Email;
            this.BusinessPhone = contact.BusinessPhone;
            this.Mobile = contact.Mobile;
        }
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "Subject", {
        //Properties
        get: function () { return this.entityPM.Subject; },
        set: function (value) {
            if (this.entityPM.Subject != value) {
                this.entityPM.Subject = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "Location", {
        get: function () { return this.entityPM.Location; },
        set: function (value) {
            if (this.entityPM.Location != value) {
                this.entityPM.Location = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "Description", {
        get: function () { return this.entityPM.Description; },
        set: function (value) {
            if (this.entityPM.Description != value) {
                this.entityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "OwnerId", {
        get: function () { return this.entityPM.OwnerId; },
        set: function (value) {
            var _this = this;
            if (this.entityPM.OwnerId != value) {
                this.entityPM.OwnerId = value;
                if (value == null) {
                    this.entityPM.BusinessUnitId = null;
                }
                else {
                    var listService = new UserListService_1.UserListService();
                    listService.getSingleFromCache(value).subscribe(function (result) {
                        var list = result.Result;
                        if (list != null)
                            _this.entityPM.BusinessUnitId = list.BusinessUnitId;
                    });
                }
                this.SetUIProperties_Qwner();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "Owner", {
        get: function () { return this.owner; },
        set: function (value) {
            if (this.owner != value) {
                this.owner = value;
                //if (value == null) {
                //    this.entityPM.BusinessUnitId = null;
                //}
                //else {
                //    this.entityPM.BusinessUnitId = (this.owner == null ? null : this.owner.BusinessUnitId);
                //}      
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "DueDate", {
        get: function () { return this.entityPM.DueDate; },
        set: function (value) {
            this.entityPM.DueDate = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "PriorityCode", {
        get: function () { return this.entityPM.PriorityCode; },
        set: function (value) {
            var _this = this;
            if (this.entityPM.PriorityCode != value) {
                this.entityPM.PriorityCode = value;
                var name = "";
                if (value != null) {
                    var service = new ActivityPriorityListService_1.ActivityPriorityListService();
                    service.getAllFromCache().subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null && list.length > 0) {
                                var priority = list.filter(function (d) { return d.Code == value; })[0];
                                _this.entityPM.PriorityName = priority.Name;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "ActivityTimeTypeCode", {
        get: function () { return this.entityPM.ActivityTimeTypeCode; },
        set: function (value) {
            if (this.entityPM.ActivityTimeTypeCode != value) {
                this.entityPM.ActivityTimeTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "PhoneNumber", {
        get: function () { return this.entityPM.PhoneNumber; },
        set: function (value) {
            if (this.entityPM.PhoneNumber != value) {
                this.entityPM.PhoneNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "CustomerId", {
        get: function () { return this.entityPM.CustomerId; },
        set: function (value) {
            if (this.entityPM.CustomerId != value) {
                this.entityPM.CustomerId = value;
                this.CallWithId = null;
                this.UpdateCustomerName(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.UpdateCustomerName = function (value) {
        var _this = this;
        var myContactId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            var cardService = new CardListService_1.CardListService();
            cardService.getSingle(value).subscribe(function (result) {
                var card = result.Result;
                if (card != null) {
                    _this.CustomerName = card.EnglishName;
                }
            });
        }
        else {
            this.CustomerName = null;
        }
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "OpportunityId", {
        get: function () { return this.entityPM.OpportunityId; },
        set: function (value) { this.entityPM.OpportunityId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "QuoteId", {
        get: function () { return this.entityPM.QuoteId; },
        set: function (value) { this.entityPM.QuoteId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "DueDateOffset", {
        get: function () { return this.entityPM.DueDateOffset; },
        set: function (value) {
            if (this.entityPM.DueDateOffset != value) {
                this.entityPM.DueDateOffset = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "DueDateDateField", {
        //get DueDateDateFieldDefaultText() {
        //    if (!AppTool.IsNullOrEmpty(this.entityPM.DueDateDateField)) {
        //        var myField = window.ObjectFields.filter(f => f.FieldName == this.entityPM.DueDateDateField)[0];
        //        if (myField != null) {
        //            return myField.FullNameTextCodeDefaultText;
        //        }
        //    }  
        //}
        get: function () { return this.entityPM.DueDateDateField; },
        set: function (value) {
            if (this.entityPM.DueDateDateField != value) {
                this.entityPM.DueDateDateField = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "BusinessProcessQueueId", {
        get: function () { return this.entityPM.BusinessProcessQueueId; },
        set: function (value) {
            if (this.entityPM.BusinessProcessQueueId != value) {
                this.entityPM.BusinessProcessQueueId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "TeamId", {
        get: function () { return this.entityPM.TeamId; },
        set: function (value) {
            if (this.entityPM.TeamId != value) {
                this.entityPM.TeamId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "AddCustomerVisibility", {
        get: function () { return this.addCustomerVisibility; },
        set: function (value) { this.addCustomerVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "ViewCustomerEnabled", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.SetFieldsEnabled = function () {
        var fieldIsEnabled = this.entityPM.IsOpen;
        this.UIProperties.SetEnabled("CallWithId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("EndDateTime", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("StartDateTime", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("Location", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("EntityId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("OwnerId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("OrganizerId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("DueDate", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("PriorityCode", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("ActivityTimeTypeCode", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("PhoneNumber", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("Duration", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("AllDayEvent", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, fieldIsEnabled);
        this.UIProperties.SetEnabled("MeetingSummary", this.ObjectTableName, fieldIsEnabled);
        if (this.entityPM.ActivityTypeCode == "TS" || this.entityPM.ActivityTypeCode == "CL") {
            this.UIProperties.SetEnabled("Description", this.ObjectTableName, true);
        }
        if (this.entityPM.ActivityTypeCode == "AP") {
            this.UIProperties.SetEnabled("MeetingSummary", this.ObjectTableName, true);
        }
        this.AddCustomerEnabled = fieldIsEnabled;
        this.AddContactEnabled = fieldIsEnabled;
        this.SetUIPropertiesInEditMode();
        this.SetUIProperties_GeneratedComponent();
    };
    ActivityInputTemplate.prototype.SetUIPropertiesInEditMode = function () {
        if (this.entityPM.ActivityTypeCode == "EI" || this.entityPM.ActivityTypeCode == "EO") {
            this.UIProperties.SetEnabled("CustomerName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Subject", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ArchiveDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CreateDate", this.ObjectTableName, false);
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
                this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
                this.AddCustomerEnabled = false;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.QuoteId)) {
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
        }
        switch (this.entityPM.ActivityTypeCode) {
            case "AP":
                {
                    this.UIProperties.SetRequired("StartDateTime", this.ObjectTableName, this.StartDateTime == null);
                    this.UIProperties.SetRequired("EndDateTime", this.ObjectTableName, this.EndDateTime == null);
                    break;
                }
            case "CL":
                {
                    this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CallWithId));
                    break;
                }
        }
        var isOppertunityVisible = !Tools_1.AppTool.IsNullOrEmpty(this.entityPM.OpportunityId);
        this.UIProperties.SetVisibility("OpportunityId", this.ObjectTableName, isOppertunityVisible);
        var isQuoteVisible = !Tools_1.AppTool.IsNullOrEmpty(this.entityPM.QuoteId) && Tools_1.AppTool.IsNullOrEmpty(this.entityPM.OpportunityId);
        this.UIProperties.SetVisibility("QuoteId", this.ObjectTableName, isQuoteVisible);
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "ViewOpportunityVisibility", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "ViewCustomerVisibility", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.CustomerId)) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "ViewQuoteVisibility", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.QuoteId)) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.OpportunityId)) {
                    result = true;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "QuoteNumber", {
        get: function () {
            return this.entityPM == null ? null : this.entityPM.QuoteNumber;
        },
        set: function (value) {
            if (this.entityPM.QuoteNumber != value) {
                this.entityPM.QuoteNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "OpportunitySubject", {
        get: function () { return this.entityPM == null ? null : this.entityPM.OpportunitySubject; },
        set: function (value) {
            if (this.entityPM.OpportunitySubject != value) {
                this.entityPM.OpportunitySubject = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "MeetingSummary", {
        //RightToLeft
        get: function () { return this.entityPM.MeetingSummary; },
        set: function (value) {
            if (this.entityPM.MeetingSummary != value) {
                this.entityPM.MeetingSummary = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "IsDescriptionRightToLeftEnabled", {
        get: function () {
            var myResult = false;
            if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "IsMeetingSummaryRightToLeftEnabled", {
        get: function () {
            var myResult = false;
            if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true && this.IsMettingSummeryVisibile) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "MeetingSummaryRightToLeft", {
        get: function () { return this.entityPM.MeetingSummaryRightToLeft; },
        set: function (value) {
            if (this.entityPM.MeetingSummaryRightToLeft != value) {
                this.entityPM.MeetingSummaryRightToLeft = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.GetMeetingSummaryFlowDirection = function () {
        var myResult = "ltr";
        if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            myResult = "ltr";
            if (this.entityPM.MeetingSummaryRightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }
        this.MeetingSummaryFlowDirection = myResult;
    };
    ActivityInputTemplate.prototype.GetMeetingSummaryBackgroundAlignLeft = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MeetingSummaryFlowDirection)) {
            this.MeetingSummaryBackgroundAlignLeft = this.MeetingSummaryFlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    };
    ActivityInputTemplate.prototype.GetMeetingSummaryBackgroundAlignRight = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MeetingSummaryFlowDirection)) {
            this.MeetingSummaryBackgroundAlignRight = this.MeetingSummaryFlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    };
    ActivityInputTemplate.prototype.AlignMeetingSummaryLeftClicked = function () {
        this.entityPM.MeetingSummaryRightToLeft = false;
        this.GetMeetingSummaryFlowDirection();
        this.RefreshTextAlgimentVariables();
    };
    ActivityInputTemplate.prototype.AlignMeetingSummaryRightClicked = function () {
        this.entityPM.MeetingSummaryRightToLeft = true;
        this.GetMeetingSummaryFlowDirection();
        this.RefreshTextAlgimentVariables();
    };
    ActivityInputTemplate.prototype.RefreshTextAlgimentVariables = function () {
        this.GetMeetingSummaryBackgroundAlignLeft();
        this.GetMeetingSummaryBackgroundAlignRight();
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "DescriptionRightToLeft", {
        get: function () { return this.entityPM.DescriptionRightToLeft; },
        set: function (value) {
            if (this.entityPM.DescriptionRightToLeft != value) {
                this.entityPM.DescriptionRightToLeft = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.GetDescriptionFlowDirection = function () {
        var myResult = "ltr";
        if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
            myResult = "rtl";
            if (this.entityPM.DescriptionRightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }
        this.DescriptionFlowDirection = myResult;
    };
    ActivityInputTemplate.prototype.GetDescriptionBackgroundAlignRight = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DescriptionFlowDirection)) {
            this.DescriptionBackgroundAlignRight = this.DescriptionFlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    };
    ActivityInputTemplate.prototype.GetDescriptionBackgroundAlignLeft = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DescriptionFlowDirection)) {
            this.DescriptionBackgroundAlignLeft = this.DescriptionFlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    };
    ActivityInputTemplate.prototype.AlignDescriptionLeftClicked = function () {
        this.entityPM.DescriptionRightToLeft = false;
        this.GetDescriptionFlowDirection();
        this.RefreshDescriptionTextAlgimentVariables();
    };
    ActivityInputTemplate.prototype.AlignDescriptionRightClicked = function () {
        this.entityPM.DescriptionRightToLeft = true;
        this.GetDescriptionFlowDirection();
        this.RefreshDescriptionTextAlgimentVariables();
    };
    ActivityInputTemplate.prototype.RefreshDescriptionTextAlgimentVariables = function () {
        this.GetDescriptionBackgroundAlignLeft();
        this.GetDescriptionBackgroundAlignRight();
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "RequiredBoxText", {
        get: function () { return this.requiredBoxText; },
        set: function (value) {
            this.requiredBoxText = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "OptionalBoxText", {
        get: function () { return this.optionalBoxText; },
        set: function (value) {
            this.optionalBoxText = value;
        },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.BuildInviteesControl = function () {
        if (this.entityPM != null) {
            this.BuildRequiredEmailList();
            this.BuildOptionalEmailList();
        }
    };
    ActivityInputTemplate.prototype.BuildRequiredEmailList = function () {
        var _this = this;
        this.Invitees_Required = [];
        this.RequiredBoxText = "";
        var list = this.entityPM.ActivityInvitees.filter(function (d) { return d.IsRequired == true; });
        list.forEach(function (item) {
            _this.Invitees_Required.push(new RequiredData(item));
        });
    };
    ActivityInputTemplate.prototype.BuildOptionalEmailList = function () {
        var _this = this;
        this.Invitees_Optional = [];
        this.OptionalBoxText = "";
        var list = this.entityPM.ActivityInvitees.filter(function (d) { return d.IsRequired == false; });
        list.forEach(function (item) {
            _this.Invitees_Optional.push(new OptionalData(item));
        });
    };
    ActivityInputTemplate.prototype.AddInviteeClicked = function () {
        var _this = this;
        var windowArgs = new Args_1.InviteeArgs();
        windowArgs.Entity = this.entityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Contacts List";
        logWindow.Width = window.innerWidth - 100;
        logWindow.Height = window.innerHeight - 100;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./CRMModules/CRMActivity/Components/NewEntity/AddEditInviteesComponent");
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                if (d == "OK") {
                    _this.errors = [];
                    _this.CheckIsValidEmails(s.RequiredEmailBoxText);
                    _this.CheckIsValidEmails(s.OptionalEmailBoxText);
                    if (_this.errors.length == 0) {
                        var myRequiredEmailsList = [];
                        if (!Tools_1.AppTool.IsNullOrEmpty(s.RequiredEmailBoxText)) {
                            var requiredEmails = s.RequiredEmailBoxText.split(';');
                            if (requiredEmails != null) {
                                myRequiredEmailsList = requiredEmails.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d); });
                            }
                        }
                        var myOptionalEmailsList = [];
                        if (!Tools_1.AppTool.IsNullOrEmpty(s.OptionalEmailBoxText)) {
                            var optionalEmails = s.OptionalEmailBoxText.split(';');
                            if (optionalEmails != null) {
                                myOptionalEmailsList = optionalEmails.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d); });
                            }
                        }
                        var removedList = [];
                        s.entityPM.ActivityInvitees.forEach(function (item) {
                            if (item.IsRequired) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(item.ContactId)) {
                                    var list = s.RequiredList.filter(function (d) { return d.Email == item.Email && d.ContactId == item.ContactId; })[0];
                                    if (list == null) {
                                        //this.entityPM.RemoveActivityInvitee(item);
                                        removedList.push(item);
                                    }
                                }
                                else {
                                    var check = myRequiredEmailsList.filter(function (d) { return d == item.Email; })[0];
                                    if (check == null) {
                                        //this.entityPM.RemoveActivityInvitee(item);
                                        removedList.push(item);
                                    }
                                }
                            }
                            else {
                                if (!Tools_1.AppTool.IsNullOrEmpty(item.ContactId)) {
                                    var list = s.OptionalList.filter(function (d) { return d.Email == item.Email && d.ContactId == item.ContactId; })[0];
                                    if (list == null) {
                                        //this.entityPM.RemoveActivityInvitee(item);
                                        removedList.push(item);
                                    }
                                }
                                else {
                                    var check = myOptionalEmailsList.filter(function (d) { return d == item.Email; })[0];
                                    if (check == null) {
                                        //this.entityPM.RemoveActivityInvitee(item);
                                        removedList.push(item);
                                    }
                                }
                            }
                        });
                        removedList.forEach(function (item) {
                            _this.entityPM.RemoveActivityInvitee(item);
                        });
                        s.RequiredList.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.ContactId); }).forEach(function (item) {
                            var check = _this.entityPM.ActivityInvitees.filter(function (d) { return d.Email == item.Email && d.ContactId == item.ContactId && d.IsRequired == true; })[0];
                            if (check == null) {
                                var entity = new ActivityInviteePM_1.ActivityInviteePM(null);
                                entity.ActivityId = _this.entityPM.Id;
                                entity.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                entity.Email = item.Email;
                                entity.ContactId = item.ContactId;
                                entity.ContactName = item.ContactName;
                                entity.IsRequired = true;
                                _this.entityPM.AddActivityInvitee(entity);
                            }
                        });
                        s.OptionalList.filter(function (d) { return !Tools_1.AppTool.IsNullOrEmpty(d.ContactId); }).forEach(function (item) {
                            var check = _this.entityPM.ActivityInvitees.filter(function (d) { return d.Email == item.Email && d.ContactId == item.ContactId && d.IsRequired == false; })[0];
                            if (check == null) {
                                var entity = new ActivityInviteePM_1.ActivityInviteePM(null);
                                entity.ActivityId = _this.entityPM.Id;
                                entity.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                entity.Email = item.Email;
                                entity.ContactId = item.ContactId;
                                entity.ContactName = item.ContactName;
                                entity.IsRequired = false;
                                _this.entityPM.AddActivityInvitee(entity);
                            }
                        });
                        myRequiredEmailsList.forEach(function (email) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(email)) {
                                var check = _this.entityPM.ActivityInvitees.filter(function (d) { return d.Email == email && Tools_1.AppTool.IsNullOrEmpty(d.ContactId) && d.IsRequired == true; })[0];
                                if (check == null) {
                                    var entity = new ActivityInviteePM_1.ActivityInviteePM(null);
                                    entity.ActivityId = _this.entityPM.Id;
                                    entity.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                    entity.Email = email;
                                    entity.ContactId = null;
                                    entity.ContactName = null;
                                    entity.IsRequired = true;
                                    _this.entityPM.AddActivityInvitee(entity);
                                }
                            }
                        });
                        myOptionalEmailsList.forEach(function (email) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(email)) {
                                var check = _this.entityPM.ActivityInvitees.filter(function (d) { return d.Email == email && Tools_1.AppTool.IsNullOrEmpty(d.ContactId) && d.IsRequired == false; })[0];
                                if (check == null) {
                                    var entity = new ActivityInviteePM_1.ActivityInviteePM(null);
                                    entity.ActivityId = _this.entityPM.Id;
                                    entity.Tenant = SessionLocator_1.SessionLocator.Tenant;
                                    entity.Email = email;
                                    entity.ContactId = null;
                                    entity.ContactName = null;
                                    entity.IsRequired = false;
                                    _this.entityPM.AddActivityInvitee(entity);
                                }
                            }
                        });
                        _this.BuildInviteesControl();
                    }
                }
            });
        });
    };
    ActivityInputTemplate.prototype.CheckIsValidEmails = function (mailsList) {
        var _this = this;
        var EMAIL_REGEXP = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var IsOk = true;
        if (mailsList) {
            var emails = mailsList.split(';');
            emails.forEach(function (item) {
                if (item) {
                    if (!EMAIL_REGEXP.test(item)) {
                        IsOk = false;
                        _this.errors.push(item + " has invalid format");
                        return;
                    }
                }
            });
        }
        return IsOk;
    };
    // Commands 
    ActivityInputTemplate.prototype.AddContactClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Contact";
        var args = new ContactInputTemplate_1.ContactInputTemplateArgs();
        args.CustomerId = this.CustomerId;
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                var contact = s.EntityPM;
                if (contact != null) {
                    _this.CallWithId = contact.Id;
                }
            });
        });
    };
    ActivityInputTemplate.prototype.AddCustomerClicked = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Customer").subscribe(function (response) {
            var str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
            str = "New Potential Customer";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = str;
            var args = new Args_2.NewEntityArgs();
            logWindow.WindowArgs = args;
            logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");
            logWindow.ComponentLoaded.subscribe(function (s) {
                logWindow.WindowClosed.subscribe(function (d) {
                    if (d) {
                        var customer = s.EntityPM;
                        if (customer != null) {
                            _this.CustomerId = customer.Id;
                        }
                    }
                });
            });
        });
    };
    ActivityInputTemplate.prototype.Validate = function () {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.BusinessUnitId)) {
            this.entityPM.BusinessUnitId = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
        }
        Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (this.entityPM.ActivityTypeCode != "TX") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.OwnerId")));
            }
        }
        switch (this.entityPM.ActivityTypeCode) {
            case "AP":
                {
                    if (this.StartDateTime == null) {
                        errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.StartDateTime")));
                    }
                    if (this.EndDateTime == null) {
                        errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.EndDateTime")));
                    }
                    if (this.StartDateTime != null && this.EndDateTime != null) {
                        if (this.EndDateTime.valueOf() <= this.StartDateTime.valueOf()) {
                            var text = TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.StartDateTime") + " must be less than " + TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.EndDateTime");
                            errors.push(text);
                        }
                    }
                    break;
                }
            case "TS":
                {
                    if (this.StartDateTime != null && this.entityPM.DueDate != null) {
                        var date1 = Tools_1.DateTool.GetDateFormats(this.entityPM.DueDate).DateParts.DateObject;
                        var date2 = Tools_1.DateTool.GetDateFormats(this.StartDateTime).DateParts.DateObject;
                        if (date1 != null && date2 != null && (date1.valueOf() <= date2.valueOf())) {
                            var text = TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.StartDateTime") + " must be less than " + TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.DueDate");
                            errors.push(text);
                        }
                    }
                    break;
                }
            case "CL":
                {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.CallWithId)) {
                        errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Activity.F.CallWithId")));
                    }
                    break;
                }
        }
        return errors;
    };
    ActivityInputTemplate.prototype.ViewEntityClicked = function (m) {
        if (m == "OPP") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.OpportunityId)) {
                this.ViewEntity("Opportunity", this.OpportunityId);
            }
        }
        else if (m == "CUS") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
                this.ViewEntity("Customer", this.CustomerId);
            }
        }
        else if (m == "QUT") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.QuoteId)) {
                this.ViewEntity("Quote", this.QuoteId);
            }
        }
    };
    ActivityInputTemplate.prototype.ViewEntity = function (tableName, entityId) {
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: tableName, });
        });
    };
    Object.defineProperty(ActivityInputTemplate.prototype, "ArchiveDate", {
        //Email Out | In
        get: function () {
            return this.entityPM.ArchiveDate;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "CreateDate", {
        get: function () {
            return this.entityPM.CreateDate;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "SortingDate", {
        get: function () {
            return this.entityPM.SortingDate;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "SenderEmail", {
        get: function () { return this.entityPM == null ? null : this.entityPM.SenderEmail; },
        set: function (value) {
            if (this.entityPM.SenderEmail != value) {
                this.entityPM.SenderEmail = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "SenderContactName", {
        get: function () {
            return this.entityPM == null ? null : this.entityPM.SenderContactName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "EmailFrom", {
        get: function () {
            var myResult = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SenderContactName)) {
                myResult = this.SenderContactName;
            }
            else {
                myResult = this.SenderEmail;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "SenderEmailForeground", {
        get: function () {
            var myResult = "#282E30";
            if (this.entityPM != null) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.entityPM.SenderContactId)) {
                    myResult = "#E53030";
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "EmailTO", {
        get: function () {
            var myResult = "";
            this.entityPM.ActivityEmailRecipients.filter(function (d) { return d.RecipientTypeCode.toUpperCase() == "TO"; }).forEach(function (item) {
                if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                    myResult += item.Email;
                }
                else {
                    myResult += ";" + item.Email;
                }
            });
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "EmailCC", {
        get: function () {
            var myResult = "";
            this.entityPM.ActivityEmailRecipients.filter(function (d) { return d.RecipientTypeCode.toUpperCase() == "CC"; }).forEach(function (item) {
                if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                    myResult += item.Email;
                }
                else {
                    myResult += ";" + item.Email;
                }
            });
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "EmailBCC", {
        get: function () {
            var myResult = "";
            this.entityPM.ActivityEmailRecipients.filter(function (d) { return d.RecipientTypeCode.toUpperCase() == "BCC"; }).forEach(function (item) {
                if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                    myResult += item.Email;
                }
                else {
                    myResult += ";" + item.Email;
                }
            });
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "OwnerName", {
        get: function () {
            return this.entityPM == null ? null : this.entityPM.OwnerName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "CustomerName", {
        get: function () {
            return this.entityPM == null ? null : this.entityPM.CustomerName;
        },
        set: function (value) {
            if (this.entityPM.CustomerName != value) {
                this.entityPM.CustomerName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "CCVisibility", {
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EmailCC)) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "BCCVisibility", {
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EmailBCC)) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityInputTemplate.prototype, "AttachmentsVisibility", {
        get: function () {
            return this.entityPM.ActivityDocumentDatas.length > 0 ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    ActivityInputTemplate.prototype.BuildInternalAttachmentList = function () {
        var _this = this;
        if (this.entityPM != null) {
            var list = [];
            this.entityPM.ActivityDocumentDatas.forEach(function (item) {
                _this.DocumentDatasList.push(new AttachmentsArgs(item));
            });
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ActivityInputTemplate.prototype, "viewContainerRef", void 0);
    ActivityInputTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ActivityInputTemplate.html',
        }),
        __metadata("design:paramtypes", [])
    ], ActivityInputTemplate);
    return ActivityInputTemplate;
}(BaseComponent_1.BaseComponent));
exports.ActivityInputTemplate = ActivityInputTemplate;
var AttachmentsArgs = /** @class */ (function () {
    function AttachmentsArgs(documentDataPM) {
        this.DocumentDataPM = documentDataPM;
    }
    Object.defineProperty(AttachmentsArgs.prototype, "DocumentTypeName", {
        get: function () { return this.DocumentDataPM.FileName; },
        enumerable: true,
        configurable: true
    });
    AttachmentsArgs.prototype.ViewAttachment = function () {
        DownloadManager_1.DownloadManager.DownloadPage(this.DocumentDataPM.DocumentId);
    };
    return AttachmentsArgs;
}());
exports.AttachmentsArgs = AttachmentsArgs;
var OptionalData = /** @class */ (function () {
    function OptionalData(entity) {
        this.OptionalBoxText = "";
        this.OptionalBoxTextColor = "";
        this.Tooltip = "";
        this.entity = entity;
        this.check();
    }
    OptionalData.prototype.check = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.entity.ContactId)) {
            this.OptionalBoxText += this.entity.Email + ";";
            this.Tooltip = "";
            this.OptionalBoxTextColor = "rgb(229,48,48)";
        }
        else {
            this.OptionalBoxText += this.entity.ContactName + ";";
            this.Tooltip = this.entity.Email;
            this.OptionalBoxTextColor = "rgb(40,46,48)";
        }
    };
    return OptionalData;
}());
exports.OptionalData = OptionalData;
var RequiredData = /** @class */ (function () {
    function RequiredData(entity) {
        this.RequiredBoxText = "";
        this.RequiredBoxTextColor = "";
        this.Tooltip = "";
        this.entity = entity;
        this.check();
    }
    RequiredData.prototype.check = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.entity.ContactId)) {
            this.RequiredBoxText += this.entity.Email + ";";
            this.Tooltip = "";
            this.RequiredBoxTextColor = "rgb(229,48,48)";
        }
        else {
            this.RequiredBoxText += this.entity.ContactName + ";";
            this.Tooltip = this.entity.Email;
            this.RequiredBoxTextColor = "rgb(40,46,48)";
        }
    };
    return RequiredData;
}());
exports.RequiredData = RequiredData;
//# sourceMappingURL=ActivityInputTemplate.js.map