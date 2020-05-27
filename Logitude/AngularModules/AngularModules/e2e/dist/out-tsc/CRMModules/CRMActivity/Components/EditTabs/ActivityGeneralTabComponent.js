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
var ActivityPM_1 = require("../../../../CRM/EntityPMs/ActivityPM");
var ActivityNotePM_1 = require("../../../../CRM/EntityPMs/ActivityNotePM");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Args_1 = require("../../../../CRM/Args");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var CommunicationLogPMService_1 = require("../../../../Common/Services/StandardPMs/CommunicationLogPMService");
var DocsOutDataViewModel_1 = require("../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel");
var CommunicationLogPMViewModel_1 = require("../../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/CommunicationLogPMViewModel");
var ActivityGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ActivityGeneralTabComponent, _super);
    function ActivityGeneralTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.EntityPM = new ActivityPM_1.ActivityPM();
        _this.ObjectTableName = "Activity";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        //View Communication Log
        _this.communicationLogVisibility = false;
        _this.IsSendClose = false;
        _this.IsDataLoaded = false;
        _this.IsEditToolVisible = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ActivityNotesObslist = [];
        _this.RunComponent();
        _this.Listen();
        return _this;
    }
    ActivityGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.ActivityInputTemplate.entityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.SetFieldsEnabled();
                    _this.ActivityInputTemplate.SetFieldsEnabled();
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.ActivityInputTemplate.entityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.SetFieldsEnabled();
                    _this.ActivityInputTemplate.SetFieldsEnabled();
                }
            });
        }
    };
    ActivityGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ActivityGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ActivityGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./CRMModules/CRMActivity/Components/NewEntity/ActivityInputTemplate", this.viewContainerRef)
            .then(function (cmpRef) {
            _this.ActivityInputTemplate = cmpRef.instance;
            var args = new Args_1.ActivityInputArgs();
            args.Activity = _this.EntityPM;
            args.TypeCode = _this.EntityPM.ActivityTypeCode;
            args.IsEditMode = true;
            args.IsAddCustomerAllowed = true;
            _this.ActivityInputTemplate.InitTemplate(args);
            _this.Initialize();
        });
    };
    ActivityGeneralTabComponent.prototype.Initialize = function () {
        this.BuildNotes();
        this.CommunicationLogVisibility = !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CommunicationLogId) ? true : false;
        this.RefreshScreen();
    };
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "CommunicationLogVisibility", {
        get: function () { return this.communicationLogVisibility; },
        set: function (value) { this.communicationLogVisibility = value; },
        enumerable: true,
        configurable: true
    });
    ActivityGeneralTabComponent.prototype.ViewCommunicationLogClicked = function () {
        var _this = this;
        var service = new CommunicationLogPMService_1.CommunicationLogPMService();
        service.get(this.EntityPM.CommunicationLogId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var log = myResponse.Result;
                if (log != null) {
                    var selectedInternalDocument = new DocsOutDataViewModel_1.DocsOutDataViewModel(null, _this.EntityPM.Id, "", "", "", "", null, null, null, _this.EntityPM, _this.ObjectTableName);
                    selectedInternalDocument.SelectedCommunicationLogViewMode = new CommunicationLogPMViewModel_1.CommunicationLogPMViewModel(log);
                    _this.SendHtmlDocument(selectedInternalDocument);
                }
            }
        });
    };
    ActivityGeneralTabComponent.prototype.SendHtmlDocument = function (SelectedInternalDocument) {
        this.IsSendClose = false;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var percentagewidthwindow = widthwindow * 0.252;
        var percentageHeightwindow = heighthwindow * 0.1764705;
        var sendWindowHeight = heighthwindow - percentageHeightwindow;
        var sendWindowWidth = widthwindow - percentagewidthwindow;
        if (sendWindowWidth < 1000)
            sendWindowWidth = 1000;
        if (sendWindowHeight < 600)
            sendWindowHeight = 600;
        SelectedInternalDocument.EntityId = this.EntityPM.Id;
        SelectedInternalDocument.ModeSendDocument = "preview";
        SelectedInternalDocument.IsViewGeneralAttachment = true;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = SelectedInternalDocument.WindowWidth = sendWindowWidth;
        logWindow.Height = SelectedInternalDocument.WindowHeight = sendWindowHeight;
        logWindow.Title = "Document Editor";
        logWindow.DataContext = SelectedInternalDocument;
        logWindow.NotifyOnClose = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SendDocumentComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
        });
    };
    // Screens 
    ActivityGeneralTabComponent.prototype.RefreshScreen = function () {
        this.SetUIProperties();
        this.SetFieldsEnabled();
        this.ActivityInputTemplate.SetFieldsEnabled();
        this.BuildNotes();
    };
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "ActivityNotesVisibility", {
        get: function () {
            var myResult = false;
            if (this.EntityPM != null) {
                if (this.EntityPM.ActivityTypeCode == "AP" || this.EntityPM.ActivityTypeCode == "CL" || this.EntityPM.ActivityTypeCode == "TS") {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "EmailControlsVisibility", {
        get: function () {
            var myResult = false;
            if (this.EntityPM != null) {
                if (this.EntityPM.ActivityTypeCode == "EO" || this.EntityPM.ActivityTypeCode == "EI") {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "OnEditModeVisibility", {
        get: function () { return true; },
        enumerable: true,
        configurable: true
    });
    ActivityGeneralTabComponent.prototype.SetUIProperties = function () {
        if (this.EntityPM.ActivityTypeCode == "EI" || this.EntityPM.ActivityTypeCode == "EO") {
            this.UIProperties.SetEnabled("CustomerName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
                this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("OpportunitySubject", this.ObjectTableName, false);
                this.ActivityInputTemplate.AddCustomerEnabled = false;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            this.UIProperties.SetEnabled("QuoteNumber", this.ObjectTableName, false);
        }
        switch (this.EntityPM.ActivityTypeCode) {
            case "AP":
                {
                    this.UIProperties.SetRequired("StartDateTime", this.ObjectTableName, this.EntityPM.StartDateTime == null);
                    this.UIProperties.SetRequired("EndDateTime", this.ObjectTableName, this.EntityPM.EndDateTime == null);
                    break;
                }
            case "CL":
                {
                    this.UIProperties.SetRequired("CallWithId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CallWithId));
                    break;
                }
        }
        var isOppertunityVisible = !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId);
        this.UIProperties.SetVisibility("OpportunityId", this.ObjectTableName, isOppertunityVisible);
        var isQuoteVisible = !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.QuoteId) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId);
        this.UIProperties.SetVisibility("QuoteId", this.ObjectTableName, isQuoteVisible);
    };
    ActivityGeneralTabComponent.prototype.SetFieldsEnabled = function () {
        var fieldIsEnabled = this.EntityPM.IsOpen;
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
        this.ActivityInputTemplate.AddCustomerEnabled = fieldIsEnabled;
        this.ActivityInputTemplate.AddContactEnabled = fieldIsEnabled;
        this.ActivityNotesObslist.forEach(function (item) {
            item.Refresh();
        });
        this.IsDataLoaded = true;
    };
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "IsControlsEnabled", {
        get: function () {
            var result = true;
            if (this.EntityPM != null) {
                if (!this.EntityPM.IsOpen) {
                    result = false;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "IsControlReadOnly", {
        get: function () {
            var myResult = false;
            if (this.EntityPM != null) {
                if (!this.EntityPM.IsOpen) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "ViewOpportunityVisibility", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "ViewCustomerVisibility", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "ViewQuoteVisibility", {
        get: function () {
            var result = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
                    result = true;
                }
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "CustomerName", {
        get: function () {
            return this.EntityPM == null ? null : this.EntityPM.CustomerName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "QuoteNumber", {
        get: function () {
            return this.EntityPM == null ? null : this.EntityPM.QuoteNumber;
        },
        set: function (value) {
            if (this.EntityPM.QuoteNumber != value) {
                this.EntityPM.QuoteNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "OpportunitySubject", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.OpportunitySubject; },
        set: function (value) {
            if (this.EntityPM.OpportunitySubject != value) {
                this.EntityPM.OpportunitySubject = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.CustomerId; },
        set: function (value) {
            if (this.EntityPM.CustomerId != value) {
                this.EntityPM.CustomerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "OpportunityId", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.OpportunityId; },
        set: function (value) {
            if (this.EntityPM.OpportunityId != value) {
                this.EntityPM.OpportunityId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "QuoteId", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.QuoteId; },
        set: function (value) {
            if (this.EntityPM.QuoteId != value) {
                this.EntityPM.QuoteId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityGeneralTabComponent.prototype, "NoNotesVisibility", {
        //Notes 
        get: function () {
            var myResult = false;
            if (this.ActivityNotesObslist.length == 0) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ActivityGeneralTabComponent.prototype.BuildNotes = function () {
        var _this = this;
        this.ActivityNotesObslist = [];
        this.EntityPM.ActivityNotes.forEach(function (item) {
            _this.ActivityNotesObslist.push(new ActivityNoteItem(_this.EntityPM, item, false, _this));
        });
    };
    ActivityGeneralTabComponent.prototype.EditNoteClicked = function (item) {
        if (item != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "Edit Activity Note";
            var datacontext = new ActivityNoteItem(item.activityPM, item.entityPM, false, this);
            logWindow.DataContext = datacontext;
            logWindow.ShowHelpIcon = true;
            logWindow.HelpText = "The maximum number of characters allowed in this field is 500.";
            logWindow.Width = 450;
            logWindow.Height = 320;
            logWindow.Show('./CRMModules/CRMActivity/Components/EditTabs/AddEditActivityNotesComponent');
        }
    };
    ActivityGeneralTabComponent.prototype.DeleteNoteClicked = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this note?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                if (item != null) {
                    if (_this.EntityPM.ActivityNotes.indexOf(item.entityPM) != -1) {
                        _this.EntityPM.RemoveActivityNote(item.entityPM);
                        _this.BuildNotes();
                    }
                }
            }
        });
    };
    ActivityGeneralTabComponent.prototype.AddNote = function () {
        var maxDate = new Date();
        var nowData = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (this.EntityPM.ActivityNotes.length > 0) {
            this.EntityPM.ActivityNotes.forEach(function (item) {
                if (item.UpdateDate.valueOf > maxDate.valueOf) {
                    maxDate = item.UpdateDate;
                }
            });
        }
        if (maxDate == null) {
            maxDate = nowData;
        }
        else if (maxDate < nowData) {
            maxDate = nowData;
        }
        else {
            maxDate.setUTCHours(maxDate.getUTCHours() + 1);
        }
        var newNote = new ActivityNotePM_1.ActivityNotePM(null);
        newNote.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newNote.ActivityId = this.EntityPM.Id;
        newNote.CreateDate = maxDate;
        newNote.UpdateDate = maxDate;
        newNote.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newNote.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newNote.CreatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        newNote.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        var datacontext = new ActivityNoteItem(this.EntityPM, newNote, true, this);
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Note";
        logWindow.DataContext = datacontext;
        logWindow.ShowHelpIcon = true;
        logWindow.HelpText = "The maximum number of characters allowed in this field is 500.";
        logWindow.Width = 450;
        logWindow.Height = 320;
        logWindow.Show('./CRMModules/CRMActivity/Components/EditTabs/AddEditActivityNotesComponent');
    };
    // Commands 
    ActivityGeneralTabComponent.prototype.ViewEntityClicked = function (m) {
        if (m == "OPP") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
                this.ViewEntity("Opportunity", this.EntityPM.OpportunityId);
            }
        }
        else if (m == "CUS") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
                this.ViewEntity("Customer", this.EntityPM.CustomerId);
            }
        }
        else if (m == "QUT") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
                this.ViewEntity("Quote", this.EntityPM.QuoteId);
            }
        }
    };
    ActivityGeneralTabComponent.prototype.ViewEntity = function (tableName, entityId) {
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: tableName, });
        });
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ActivityGeneralTabComponent.prototype, "viewContainerRef", void 0);
    ActivityGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ActivityGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], ActivityGeneralTabComponent);
    return ActivityGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ActivityGeneralTabComponent = ActivityGeneralTabComponent;
var ActivityNoteItem = /** @class */ (function (_super) {
    __extends(ActivityNoteItem, _super);
    function ActivityNoteItem(activityPM, entityPM, isNew, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.activityPM = new ActivityPM_1.ActivityPM();
        _this.entityPM = new ActivityNotePM_1.ActivityNotePM(null);
        _this.DataContext = _this;
        _this.IsControlsEnabled = false;
        _this.activityPM = activityPM;
        _this.entityPM = entityPM;
        _this.isNew = isNew;
        _this.GetIsControlsEnabled();
        return _this;
    }
    ActivityNoteItem.prototype.GetIsControlsEnabled = function () {
        var result = true;
        if (!this.activityPM.IsOpen) {
            result = false;
        }
        return result;
    };
    Object.defineProperty(ActivityNoteItem.prototype, "PostToFollowersVisibility", {
        get: function () {
            var myResult = false;
            if (this.isNew) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityNoteItem.prototype, "PostToFollowers", {
        get: function () { return this.entityPM.PostToFollowers; },
        set: function (value) {
            if (this.entityPM.PostToFollowers != value) {
                this.entityPM.PostToFollowers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityNoteItem.prototype, "UpdateDate", {
        // Properties
        get: function () { return this.entityPM.UpdateDate; },
        set: function (value) {
            if (this.entityPM.UpdateDate != value) {
                this.entityPM.UpdateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityNoteItem.prototype, "UpdatedByUserId", {
        get: function () { return this.entityPM.UpdatedByUserId; },
        set: function (value) {
            if (this.entityPM.UpdatedByUserId != value) {
                this.entityPM.UpdatedByUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityNoteItem.prototype, "UpdatedByUserName", {
        get: function () { return this.entityPM.UpdatedByUserName; },
        set: function (value) {
            if (this.entityPM.UpdatedByUserName != value) {
                this.entityPM.UpdatedByUserName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityNoteItem.prototype, "CreateDate", {
        get: function () { return this.entityPM.CreateDate; },
        set: function (value) {
            if (this.entityPM.CreateDate != value) {
                this.entityPM.CreateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityNoteItem.prototype, "CreatedByUserName", {
        get: function () { return this.entityPM.CreatedByUserName; },
        set: function (value) {
            if (this.entityPM.CreatedByUserName != value) {
                this.entityPM.CreatedByUserName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityNoteItem.prototype, "Notes", {
        get: function () { return this.entityPM.Notes; },
        set: function (value) {
            if (this.entityPM.Notes != value) {
                this.entityPM.Notes = value;
                this.isDataEdited = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityNoteItem.prototype, "DateLabel", {
        get: function () {
            var myResult = "Created by ";
            if (this.entityPM.CreateDate != this.entityPM.UpdateDate) {
                myResult = "Modified by ";
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ActivityNoteItem.prototype.Refresh = function () {
        this.GetIsControlsEnabled();
    };
    return ActivityNoteItem;
}(BaseComponent_1.BaseComponent));
exports.ActivityNoteItem = ActivityNoteItem;
//# sourceMappingURL=ActivityGeneralTabComponent.js.map