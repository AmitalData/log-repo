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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SLAHeaderPM_1 = require("../../../../CRM/EntityPMs/SLAHeaderPM");
var SLALinePM_1 = require("../../../../CRM/EntityPMs/SLALinePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CRMDomainService_1 = require("../../../../CRM/Services/CRMDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TicketSeverityListService_1 = require("../../../../CRM/Services/StandardLists/TicketSeverityListService");
var SLAEscalationRecepientPM_1 = require("../../../../CRM/EntityPMs/SLAEscalationRecepientPM");
var SLAEscalationPM_1 = require("../../../../CRM/EntityPMs/SLAEscalationPM");
var SLAHeaderPMService_1 = require("../../../../CRM/Services/StandardPMs/SLAHeaderPMService");
var EscalationPreDefinitionListService_1 = require("../../../../CRM/Services/StandardLists/EscalationPreDefinitionListService");
var EscalationActionTimeIndicatorListService_1 = require("../../../../CRM/Services/StandardLists/EscalationActionTimeIndicatorListService");
var TimeUnitListService_1 = require("../../../../CRM/Services/StandardLists/TimeUnitListService");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var NewSLAComponent = /** @class */ (function (_super) {
    __extends(NewSLAComponent, _super);
    function NewSLAComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "SLAHeader";
        _this.DataContext = _this;
        _this.entityPM = new SLAHeaderPM_1.SLAHeaderPM();
        _this.ValidationErrorsList = [];
        _this.SLALinesList = [];
        _this.FirstResponseEscalationDataList = [];
        _this.ResolveEscalationDataList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsNew = false;
        _this.addResolveWithinEscalationEnabled = true;
        _this.addFirstResponseEscalationEnabled = true;
        _this.SLALinesList = [];
        _this.FirstResponseEscalationDataList = [];
        _this.ResolveEscalationDataList = [];
        _this.FillUsers();
        _this.FillPredefinitionList();
        return _this;
    }
    NewSLAComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.entityPM = args.EntityPM;
        this.IsNew = args.IsNewEntity;
        if (this.IsNew) {
            this.entityPM = new SLAHeaderPM_1.SLAHeaderPM();
            var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            this.entityPM.CreateDate = todayDateTime;
            this.entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.entityPM.UpdateDate = todayDateTime;
            this.entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.createSLALinesList();
            this.InitalizeData();
        }
        else {
            var service = new SLAHeaderPMService_1.SLAHeaderPMService();
            service.get(this.entityPM.Id).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.entityPM = myResponse.Result;
                    _this.InitalizeData();
                }
            });
        }
    };
    NewSLAComponent.prototype.InitalizeData = function () {
        this.FillSLALines();
        this.RefreshData();
        this.SetUIProperties();
    };
    NewSLAComponent.prototype.FillUsers = function () {
        var _this = this;
        this.UsersCachedList = [];
        var listService = new UserListService_1.UserListService();
        listService.getAllFromCache().subscribe(function (result) {
            _this.UsersCachedList = result.Result;
        });
    };
    NewSLAComponent.prototype.FillPredefinitionList = function () {
        var _this = this;
        this.EscalationPreDefinitionCachedList = [];
        var service = new EscalationPreDefinitionListService_1.EscalationPreDefinitionListService();
        service.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.EscalationPreDefinitionCachedList = myResponse.Result;
            }
        });
    };
    NewSLAComponent.prototype.SetUIProperties = function () {
        this.AddFirstResponseEscalationEnabled = (this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "FR"; }).length < 5 ? true : false);
        this.AddResolveWithinEscalationEnabled = (this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "RW"; }).length < 5 ? true : false);
    };
    NewSLAComponent.prototype.FillSLALines = function () {
        var _this = this;
        if (this.entityPM != null && this.entityPM.SLALines.length > 0) {
            this.SLALinesList = [];
            this.entityPM.SLALines.forEach(function (item) {
                _this.SLALinesList.push(new SLALineArgs(item));
            });
        }
    };
    NewSLAComponent.prototype.FillResponseEscalationList = function () {
        var _this = this;
        if (this.entityPM != null && this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "FR"; }).length > 0) {
            this.FirstResponseEscalationDataList = [];
            this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "FR"; }).forEach(function (item) {
                _this.FirstResponseEscalationDataList.push(new EscalationArgs(_this, item, item.EscalationFor, false));
            });
            this.SetUIProperties();
        }
    };
    NewSLAComponent.prototype.FillResolveEscalationList = function () {
        var _this = this;
        if (this.entityPM != null && this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "RW"; }).length > 0) {
            this.ResolveEscalationDataList = [];
            this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "RW"; }).forEach(function (item) {
                _this.ResolveEscalationDataList.push(new EscalationArgs(_this, item, item.EscalationFor, false));
            });
            this.SetUIProperties();
        }
    };
    //private IsNew = false;
    NewSLAComponent.prototype.getSLAHeaderEntityMethod = function () {
        var _this = this;
        var service = new CRMDomainService_1.CRMDomainService();
        service.GetSingleSLAHeaderPMByTenant().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                // this.IsNew = false;
                _this.entityPM = myResponse.Result;
                _this.FillSLALines();
                if (_this.entityPM == null) {
                    //this.IsNew = true;
                    _this.entityPM = new SLAHeaderPM_1.SLAHeaderPM();
                    var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    _this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    _this.entityPM.CreateDate = todayDateTime;
                    _this.entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    _this.entityPM.UpdateDate = todayDateTime;
                    _this.entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    _this.createSLALinesList();
                }
                _this.RefreshData();
                _this.SetUIProperties();
            }
        });
    };
    NewSLAComponent.prototype.RefreshData = function () {
        this.FillResponseEscalationList();
        this.FillResolveEscalationList();
    };
    NewSLAComponent.prototype.createSLALinesList = function () {
        var _this = this;
        var myService = new TicketSeverityListService_1.TicketSeverityListService();
        myService.getAllFromCache().subscribe(function (resp) {
            if (!resp.HasError) {
                var allSeverities = resp.Result;
                allSeverities.filter(function (a) { return a.Tenant == SessionLocator_1.SessionLocator.Tenant; }).forEach(function (item) {
                    var sLALine = new SLALinePM_1.SLALinePM(_this.entityPM.Id);
                    sLALine.SeverityName = item.Name;
                    sLALine.SLAHeaderId = _this.entityPM.Id;
                    sLALine.SeverityId = item.Id;
                    sLALine.ChangeSetOp = "Insert";
                    sLALine.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    _this.SLALinesList.push(new SLALineArgs(sLALine));
                    _this.entityPM.AddSLALine(sLALine);
                });
            }
        });
    };
    Object.defineProperty(NewSLAComponent.prototype, "Inactive", {
        // Properties
        get: function () { return this.entityPM.Inactive; },
        set: function (value) {
            if (this.entityPM.Inactive != value) {
                this.entityPM.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewSLAComponent.prototype, "Name", {
        get: function () { return this.entityPM.Name; },
        set: function (value) {
            if (this.entityPM.Name != value) {
                this.entityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewSLAComponent.prototype, "Description", {
        get: function () { return this.entityPM.Description; },
        set: function (value) {
            if (this.entityPM.Description != value) {
                this.entityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewSLAComponent.prototype, "AddResolveWithinEscalationEnabled", {
        get: function () { return this.addResolveWithinEscalationEnabled; },
        set: function (value) {
            if (this.addResolveWithinEscalationEnabled != value) {
                this.addResolveWithinEscalationEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewSLAComponent.prototype, "AddFirstResponseEscalationEnabled", {
        get: function () { return this.addFirstResponseEscalationEnabled; },
        set: function (value) {
            if (this.addFirstResponseEscalationEnabled != value) {
                this.addFirstResponseEscalationEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    NewSLAComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewSLAComponent.prototype.OkButtonClicked = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        this.entityPM.SLALines.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item, "SLALine", errors);
            if ((item.FirstResponseTime == null || Tools_1.AppTool.IsNullOrEmpty(item.FirstResponseTimeUnit))) {
                errors.push("First Response fields are required");
            }
            if ((item.ResolveWithinTime == null || Tools_1.AppTool.IsNullOrEmpty(item.ResolveWithinTimeUnit))) {
                errors.push("Resolve Within fields are required");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(item.BusinessHoursId)) {
                errors.push("Operational Hours are required");
            }
        });
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNew) {
                this.InsertSLA();
            }
            else {
                this.UpdateSLA();
            }
        }
    };
    NewSLAComponent.prototype.InsertSLA = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new SLAHeaderPMService_1.SLAHeaderPMService();
        service.insert(this.entityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    NewSLAComponent.prototype.UpdateSLA = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new SLAHeaderPMService_1.SLAHeaderPMService();
        service.update(this.entityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    NewSLAComponent.prototype.EditEscalation = function (item) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Edit Escalation";
        logWindow.DataContext = item;
        logWindow.Show('./CRMModules/CRMOthers/Components/SLA/AddEditEscalationComponent');
    };
    NewSLAComponent.prototype.DeleteEscalation = function (deletedItem) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this Escalation?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                var selectedPM = deletedItem.entityPM;
                if (selectedPM.EscalationFor == "FR") {
                    if (_this.entityPM.SLAEscalations.indexOf(selectedPM) != -1) {
                        _this.entityPM.RemoveSLAEscalation(selectedPM);
                    }
                    var itemIndex = _this.FirstResponseEscalationDataList.indexOf(deletedItem);
                    if (itemIndex > -1) {
                        _this.FirstResponseEscalationDataList.splice(itemIndex, 1);
                    }
                }
                else {
                    if (_this.entityPM.SLAEscalations.indexOf(selectedPM) != -1) {
                        _this.entityPM.RemoveSLAEscalation(selectedPM);
                    }
                    var itemIndex = _this.ResolveEscalationDataList.indexOf(deletedItem);
                    if (itemIndex > -1) {
                        _this.ResolveEscalationDataList.splice(itemIndex, 1);
                    }
                }
                //Refresh data 
                _this.SetUIProperties();
            }
        });
    };
    NewSLAComponent.prototype.AddResolveWithinEscalation = function () {
        var count = this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "RW"; }).length;
        if (count < 5) {
            var escalationFor = "RW"; // Resolve Within
            var myLineNumber = 1;
            if (count > 0) {
                this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "RW"; }).forEach(function (item) {
                    if (item.LineNumber > myLineNumber)
                        myLineNumber = item.LineNumber;
                });
                myLineNumber = myLineNumber + 1;
            }
            var slaEscalationPM = new SLAEscalationPM_1.SLAEscalationPM(null);
            slaEscalationPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            slaEscalationPM.SLAHeaderId = this.entityPM.Id;
            slaEscalationPM.LineNumber = myLineNumber;
            var context = new EscalationArgs(this, slaEscalationPM, escalationFor, true);
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.DataContext = context;
            logWindow.Title = "New Escalation";
            logWindow.Show('./CRMModules/CRMOthers/Components/SLA/AddEditEscalationComponent');
        }
        else {
            this.AddResolveWithinEscalationEnabled = false;
        }
    };
    NewSLAComponent.prototype.AddFirstResponseEscalation = function () {
        var count = this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "FR"; }).length;
        if (count < 5) {
            var escalationFor = "FR"; // First Response
            var myLineNumber = 1;
            if (count > 0) {
                this.entityPM.SLAEscalations.filter(function (a) { return a.EscalationFor == "FR"; }).forEach(function (item) {
                    if (item.LineNumber > myLineNumber)
                        myLineNumber = item.LineNumber;
                });
                myLineNumber = myLineNumber + 1;
            }
            var slaEscalationPM = new SLAEscalationPM_1.SLAEscalationPM(null);
            slaEscalationPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            slaEscalationPM.SLAHeaderId = this.entityPM.Id;
            slaEscalationPM.LineNumber = myLineNumber;
            var context = new EscalationArgs(this, slaEscalationPM, escalationFor, true);
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "New Escalation";
            logWindow.DataContext = context;
            logWindow.Show('./CRMModules/CRMOthers/Components/SLA/AddEditEscalationComponent');
        }
        else {
            this.AddFirstResponseEscalationEnabled = false;
        }
    };
    NewSLAComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewSLAComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewSLAComponent);
    return NewSLAComponent;
}(BaseComponent_1.BaseComponent));
exports.NewSLAComponent = NewSLAComponent;
var SLALineArgs = /** @class */ (function (_super) {
    __extends(SLALineArgs, _super);
    function SLALineArgs(entityList) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "SLALine";
        _this.DataContext = _this;
        _this.SLALine = entityList;
        return _this;
    }
    Object.defineProperty(SLALineArgs.prototype, "SeverityName", {
        // Properties
        get: function () { return this.SLALine.SeverityName; },
        set: function (value) {
            if (this.SLALine.SeverityName != value) {
                this.SLALine.SeverityName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLALineArgs.prototype, "FirstResponseTime", {
        get: function () { return this.SLALine.FirstResponseTime; },
        set: function (value) {
            if (this.SLALine.FirstResponseTime != value) {
                this.SLALine.FirstResponseTime = value;
                this.calculateFirstResponceTimeInMinutes();
            }
        },
        enumerable: true,
        configurable: true
    });
    SLALineArgs.prototype.calculateFirstResponceTimeInMinutes = function () {
        if (this.FirstResponseTimeUnit == "II") //Minutes
         {
            this.FirstResponseTimeInMinute = this.FirstResponseTime;
        }
        if (this.FirstResponseTimeUnit == "YY") //Days 
         {
            var numOfMinutes = 24 * 60;
            this.FirstResponseTimeInMinute = this.FirstResponseTime * numOfMinutes;
        }
        if (this.FirstResponseTimeUnit == "OO") //Hours
         {
            var numOfMinutes = 1 * 60;
            this.FirstResponseTimeInMinute = this.FirstResponseTime * numOfMinutes;
        }
    };
    Object.defineProperty(SLALineArgs.prototype, "FirstResponseTimeUnit", {
        get: function () { return this.SLALine.FirstResponseTimeUnit; },
        set: function (value) {
            if (this.SLALine.FirstResponseTimeUnit != value) {
                this.SLALine.FirstResponseTimeUnit = value;
                this.calculateFirstResponceTimeInMinutes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLALineArgs.prototype, "FirstResponseTimeInMinute", {
        get: function () { return this.SLALine.FirstResponseTimeInMinute; },
        set: function (value) {
            if (this.SLALine.FirstResponseTimeInMinute != value) {
                this.SLALine.FirstResponseTimeInMinute = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLALineArgs.prototype, "ResolveWithinTime", {
        get: function () { return this.SLALine.ResolveWithinTime; },
        set: function (value) {
            if (this.SLALine.ResolveWithinTime != value) {
                this.SLALine.ResolveWithinTime = value;
                this.calculateResolveWithinTimeInMinutes();
            }
        },
        enumerable: true,
        configurable: true
    });
    SLALineArgs.prototype.calculateResolveWithinTimeInMinutes = function () {
        if (this.ResolveWithinTimeUnit == "II") {
            this.ResolveWithinTimeInMinute = this.ResolveWithinTime;
        }
        if (this.ResolveWithinTimeUnit == "YY") {
            var numOfMinutes = 24 * 60;
            this.ResolveWithinTimeInMinute = this.ResolveWithinTime * numOfMinutes;
        }
        if (this.ResolveWithinTimeUnit == "OO") {
            var numOfMinutes = 1 * 60;
            this.ResolveWithinTimeInMinute = this.ResolveWithinTime * numOfMinutes;
        }
    };
    Object.defineProperty(SLALineArgs.prototype, "ResolveWithinTimeUnit", {
        get: function () { return this.SLALine.ResolveWithinTimeUnit; },
        set: function (value) {
            if (this.SLALine.ResolveWithinTimeUnit != value) {
                this.SLALine.ResolveWithinTimeUnit = value;
                this.calculateResolveWithinTimeInMinutes();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLALineArgs.prototype, "ResolveWithinTimeInMinute", {
        get: function () { return this.SLALine.ResolveWithinTimeInMinute; },
        set: function (value) {
            if (this.SLALine.ResolveWithinTimeInMinute != value) {
                this.SLALine.ResolveWithinTimeInMinute = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLALineArgs.prototype, "FirstResponseEscalate", {
        get: function () { return this.SLALine.FirstResponseEscalate; },
        set: function (value) {
            if (this.SLALine.FirstResponseEscalate != value) {
                this.SLALine.FirstResponseEscalate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLALineArgs.prototype, "ResolveWithinEscalate", {
        get: function () { return this.SLALine.ResolveWithinEscalate; },
        set: function (value) {
            if (this.SLALine.ResolveWithinEscalate != value) {
                this.SLALine.ResolveWithinEscalate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SLALineArgs.prototype, "BusinessHoursId", {
        get: function () { return this.SLALine.BusinessHoursId; },
        set: function (value) {
            if (this.SLALine.BusinessHoursId != value) {
                this.SLALine.BusinessHoursId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return SLALineArgs;
}(BaseComponent_1.BaseComponent));
exports.SLALineArgs = SLALineArgs;
var EscalationArgs = /** @class */ (function (_super) {
    __extends(EscalationArgs, _super);
    function EscalationArgs(trigger, entityPM, escalationFor, isNew) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "SLAEscalation";
        _this.DataContext = _this;
        _this.isUserFinised = false;
        _this.isPreDefinitionFinished = false;
        _this.TimeIndicator = "";
        _this.TimeUnitName = "";
        _this.trigger = trigger;
        _this.isNew = isNew;
        _this.entityPM = entityPM;
        _this.EscalationFor = escalationFor;
        _this.entityPM.EscalationFor = escalationFor;
        //Fill Users List
        _this.UsersCachedList = trigger.UsersCachedList;
        _this.EscalationPreDefinitionCachedList = trigger.EscalationPreDefinitionCachedList;
        _this.UserSelectedList = [];
        _this.PreDefinitionList = [];
        _this.FillPredefinitionList();
        return _this;
    }
    EscalationArgs.prototype.SetUIProperties = function () {
        var isEnabled = this.EscalationActionTimeIndicator == "IM" ? false : true;
        var isRequired = this.EscalationActionTimeIndicator == "IM" ? false : true;
        this.UIProperties.SetEnabled("EscalationTime", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("EscalationTimeUnit", this.ObjectTableName, isEnabled);
        this.UIProperties.SetRequired("EscalationTime", this.ObjectTableName, isRequired && Tools_1.AppTool.IsNullOrEmpty(this.EscalationTime));
        this.UIProperties.SetRequired("EscalationTimeUnit", this.ObjectTableName, isRequired && Tools_1.AppTool.IsNullOrEmpty(this.EscalationTimeUnit));
    };
    EscalationArgs.prototype.FillPredefinitionList = function () {
        var _this = this;
        this.PreDefinitionList = [];
        var service = new EscalationPreDefinitionListService_1.EscalationPreDefinitionListService();
        service.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                if (list != null) {
                    list.forEach(function (item) {
                        _this.PreDefinitionList.push(new EscalationPreDefinitionData(item));
                    });
                }
                _this.isPreDefinitionFinished = true;
                _this.Initialize();
            }
        });
    };
    EscalationArgs.prototype.Initialize = function () {
        if (this.isPreDefinitionFinished) {
            if (!this.isNew) {
                var preDefinitionIds = [];
                if (this.entityPM != null) {
                    var usersIds = [];
                    this.entityPM.SLAEscalationRecepients.filter(function (a) { return a.UserId != null; }).forEach(function (item) {
                        usersIds.push(item.UserId);
                    });
                    this.entityPM.SLAEscalationRecepients.filter(function (a) { return a.PreDefinitionId != null; }).forEach(function (item) {
                        preDefinitionIds.push(item.PreDefinitionId);
                    });
                    var emailslist = [];
                    this.UsersCachedList.filter(function (a) { return usersIds.indexOf(a.Id) > -1; }).forEach(function (item) {
                        emailslist.push(item.Email);
                    });
                    this.Users = emailslist.join(";");
                    //this.UserSelectedList = this.UsersCachedList.filter(a => usersIds.indexOf(a.Id) > -1);
                    var preDefinitionsemails = [];
                    this.EscalationPreDefinitionCachedList.filter(function (a) { return preDefinitionIds.indexOf(a.Code) > -1; }).forEach(function (item) {
                        preDefinitionsemails.push(item.Name);
                    });
                    this.PreDefinitions = preDefinitionsemails.join(",");
                }
                this.PreDefinitionList.forEach(function (item) {
                    if (preDefinitionIds.indexOf(item.Code) != -1) {
                        item.IsChecked = true;
                    }
                });
            }
            this.SetUIProperties();
            this.getEscalationActionTime();
            this.getTimeIndicator();
        }
    };
    Object.defineProperty(EscalationArgs.prototype, "EscalationActionTimeIndicator", {
        //Properties 
        get: function () { return this.entityPM.EscalationActionTimeIndicator; },
        set: function (value) {
            if (this.entityPM.EscalationActionTimeIndicator != value) {
                this.entityPM.EscalationActionTimeIndicator = value;
                this.EscalationTime = null;
                this.EscalationTimeUnit = null;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EscalationArgs.prototype, "EscalationTime", {
        get: function () { return this.entityPM.EscalationTime; },
        set: function (value) {
            if (this.entityPM.EscalationTime != value) {
                this.entityPM.EscalationTime = value;
                this.calculateEscalationInMinutes();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EscalationArgs.prototype, "EscalaitonTimeInMinutes", {
        get: function () { return this.entityPM.EscalaitonTimeInMinutes; },
        set: function (value) {
            if (this.entityPM.EscalaitonTimeInMinutes != value) {
                this.entityPM.EscalaitonTimeInMinutes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EscalationArgs.prototype, "EscalationTimeUnit", {
        get: function () { return this.entityPM.EscalationTimeUnit; },
        set: function (value) {
            if (this.entityPM.EscalationTimeUnit != value) {
                this.entityPM.EscalationTimeUnit = value;
                this.calculateEscalationInMinutes();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    EscalationArgs.prototype.calculateEscalationInMinutes = function () {
        if (this.EscalationTimeUnit == "II") {
            this.EscalaitonTimeInMinutes = this.EscalationTime;
        }
        if (this.EscalationTimeUnit == "YY") {
            var numOfMinutes = 24 * 60;
            this.EscalaitonTimeInMinutes = this.EscalationTime * numOfMinutes;
        }
        if (this.EscalationTimeUnit == "OO") {
            var numOfMinutes = 1 * 60;
            this.EscalaitonTimeInMinutes = this.EscalationTime * numOfMinutes;
        }
    };
    EscalationArgs.prototype.getEscalationActionTime = function () {
        var _this = this;
        var service = new EscalationActionTimeIndicatorListService_1.EscalationActionTimeIndicatorListService();
        service.getSingleFromCache(this.entityPM.EscalationActionTimeIndicator).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var timeIndicator = myResponse.Result;
                if (timeIndicator != null) {
                    _this.TimeIndicator = timeIndicator.Name;
                }
                else {
                    _this.TimeIndicator = "";
                }
            }
        });
    };
    EscalationArgs.prototype.getTimeIndicator = function () {
        var _this = this;
        var service = new TimeUnitListService_1.TimeUnitListService();
        service.getSingleFromCache(this.entityPM.EscalationTimeUnit).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var timeIndicator = myResponse.Result;
                if (timeIndicator != null) {
                    _this.TimeUnitName = timeIndicator.Name;
                }
                else {
                    _this.TimeUnitName = "";
                }
            }
        });
    };
    EscalationArgs.prototype.BuildEscalationRecepients = function () {
        var _this = this;
        this.entityPM.SLAEscalationRecepients.filter(function (a) { return a.SLAEscalationId == _this.entityPM.Id; }).forEach(function (item) {
            _this.entityPM.RemoveSLAEscalationRecepient(item);
        });
        if (this.UserSelectedList != null && this.UserSelectedList.length > 0) {
            this.UserSelectedList.forEach(function (item) {
                var escalationLine = new SLAEscalationRecepientPM_1.SLAEscalationRecepientPM(null);
                escalationLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
                escalationLine.SLAEscalationId = _this.entityPM.Id;
                escalationLine.UserId = item.Id;
                if (_this.entityPM.SLAEscalationRecepients.indexOf(escalationLine) == -1) {
                    _this.entityPM.AddSLAEscalationRecepient(escalationLine);
                }
            });
        }
        if (this.PreDefinitionList != null && this.PreDefinitionList.length > 0) {
            this.PreDefinitionList.forEach(function (item) {
                if (item.IsChecked) {
                    var escalationLine = new SLAEscalationRecepientPM_1.SLAEscalationRecepientPM(null);
                    escalationLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    escalationLine.SLAEscalationId = _this.entityPM.Id;
                    escalationLine.PreDefinitionId = item.Code;
                    if (_this.entityPM.SLAEscalationRecepients.indexOf(escalationLine) == -1) {
                        _this.entityPM.AddSLAEscalationRecepient(escalationLine);
                    }
                }
            });
        }
        //var usersIds = [];
        //this.entityPM.SLAEscalationRecepients.filter(a => a.UserId != null).forEach(item => {
        //    usersIds.push(item.UserId)
        //});
        var preDefinitionIds = [];
        this.entityPM.SLAEscalationRecepients.filter(function (a) { return a.PreDefinitionId != null; }).forEach(function (item) {
            preDefinitionIds.push(item.PreDefinitionId);
        });
        //var emailslist = [];
        //this.UsersCachedList.filter(a => usersIds.indexOf(a.Id) > -1).forEach(item => {
        //    emailslist.push(item.Email);
        //});
        //this.Users = emailslist.join(";");
        //this.UserSelectedList = this.UsersCachedList.filter(a => usersIds.indexOf(a.Id) > -1);
        var preDefinitionsemails = [];
        this.EscalationPreDefinitionCachedList.filter(function (a) { return preDefinitionIds.indexOf(a.Code) > -1; }).forEach(function (item) {
            preDefinitionsemails.push(item.Name);
        });
        this.PreDefinitions = preDefinitionsemails.join(",");
    };
    return EscalationArgs;
}(BaseComponent_1.BaseComponent));
exports.EscalationArgs = EscalationArgs;
var EscalationPreDefinitionData = /** @class */ (function (_super) {
    __extends(EscalationPreDefinitionData, _super);
    function EscalationPreDefinitionData(entityPM) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "EscalationPreDefinition";
        _this.isChecked = false;
        _this.entityPM = entityPM;
        return _this;
    }
    Object.defineProperty(EscalationPreDefinitionData.prototype, "Name", {
        // Properties
        get: function () { return this.entityPM.Name; },
        set: function (value) { this.entityPM.Name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EscalationPreDefinitionData.prototype, "Code", {
        get: function () { return this.entityPM.Code; },
        set: function (value) { this.entityPM.Code = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EscalationPreDefinitionData.prototype, "IsChecked", {
        get: function () {
            return this.isChecked;
        },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return EscalationPreDefinitionData;
}(BaseComponent_1.BaseComponent));
exports.EscalationPreDefinitionData = EscalationPreDefinitionData;
//# sourceMappingURL=NewSLAComponent.js.map