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
var TicketPMService_1 = require("../../../../../CRM/Services/StandardPMs/TicketPMService");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CRMDomainService_1 = require("../../../../../CRM/Services/CRMDomainService");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var LocationDirective_1 = require("../../../../../Infrastructure/Utilities/LocationDirective");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ServiceHelper_1 = require("../../../../../Infrastructure/Utilities/ServiceHelper");
var Tools_2 = require("../../../../../CRM/Tools");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var UserListService_1 = require("../../../../../Common/Services/StandardLists/UserListService");
var CardListService_1 = require("../../../../../Common/Services/StandardLists/CardListService");
var TicketClassificationListService_1 = require("../../../../../CRM/Services/StandardLists/TicketClassificationListService");
var Args_1 = require("../../../../../CRM/Args");
var TicketValidator_1 = require("../../../../../CRM/Validators/TicketValidator");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DateTimePipe_1 = require("../../../../../Controls/Pipes/DateTimePipe");
var ContactListService_1 = require("../../../../../Common/Services/StandardLists/ContactListService");
var TicketMainTabComponent = /** @class */ (function (_super) {
    __extends(TicketMainTabComponent, _super);
    function TicketMainTabComponent(entityArgs, _entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.LinkColor = "#1E4AC4";
        _this.TicketCommunicationLogs = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        // Fill Correspondence Lines From Ticket
        _this.CorrespondenceList = [];
        _this.FirstCorrespondence = null;
        // View Shipment 
        _this.shipmentLinkNumberVisibility = false;
        // Send Email Command 
        _this.code = "";
        //Tabs 
        _this.PageChild_DS = null;
        _this.PageChild_OA = null;
        _this.PageChild_PT = null;
        _this.openActivitiesHeader = "Open Activities (0)";
        _this.ActivitiesContent = "";
        _this.ActivitiesList = [];
        _this.IsRefreshButton = false;
        _this.IsReload = false;
        _this.EntityPM = _this.entityArgs.EntityPM;
        if (_this.EntityPM) {
            _this.EntityId = _this.EntityPM.Id;
        }
        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
        _this.TicketCommunicationLogs = [];
        _this.ContactListService = new ContactListService_1.ContactListService();
        _this.Listen();
        _this.SetUIProperties();
        return _this;
    }
    TicketMainTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    //this.ReloadEntityPM();
                    if (_this.PageChild_DS != null) {
                        _this.PageChild_DS.RefreshTab(_this);
                    }
                    if (_this.IsRefreshButton) {
                        _this.IsRefreshButton = false;
                        _this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                    if (_this.IsReload) {
                        _this.IsReload = false;
                        _this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                    _this.ReloadhData();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.ReloadhData();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "TIMN") {
                    if (_this.PageChild_DS != null) {
                        _this.PageChild_DS.GetEntityLinkNumberVisibility();
                        _this.PageChild_DS.SetUIProperties();
                    }
                }
            });
        }
    };
    TicketMainTabComponent.prototype.ReloadEntityPM = function () {
        var _this = this;
        var myService = new TicketPMService_1.TicketPMService();
        myService.get(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.EntityPM = myResponse.Result;
                _this.CurrentSession.CurrentEditComponent.EntityPM = _this.EntityPM;
                _this.entityArgs.EditComponent.ReloadEntityPM();
                if (_this.PageChild_DS != null) {
                    _this.PageChild_DS.RefreshTab(_this);
                }
                if (_this.IsRefreshButton) {
                    _this.IsRefreshButton = false;
                    //this.entityArgs.EditComponent.ReloadEntityPM();
                }
                if (_this.IsReload) {
                    _this.IsReload = false;
                    //this.entityArgs.EditComponent.ReloadEntityPM();
                }
                _this.ReloadhData();
            }
        });
    };
    TicketMainTabComponent.prototype.ReloadhData = function () {
        this.LoadCommunicationLogsList();
        this.LoadActivities();
        this.SetUIProperties();
    };
    TicketMainTabComponent.prototype.ngOnInit = function () {
        this.LoadCommunicationLogsList();
        this.LoadActivities();
        this.SetSelectedTab();
    };
    TicketMainTabComponent.prototype.ngAfterViewInit = function () {
        this.SelectionChanged();
    };
    // UI Properties
    TicketMainTabComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.MainClassificationId));
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentNumber)) {
            this.shipmentLinkNumberVisibility = true;
        }
        this.SetUIProperties_EntityClosed();
    };
    TicketMainTabComponent.prototype.SetUIProperties_EntityClosed = function () {
        this.IsTicketEditEnabled = Tools_2.CRMTool.IsTicketEditEnabled(this.EntityPM);
        this.LinkColor = this.IsTicketEditEnabled ? "#1E4AC4" : "gray";
        this.IsTicketReplyEnabled = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "TicketReply") && Tools_2.CRMTool.IsTicketEditEnabled(this.EntityPM);
        this.IsTicketActivityEnabled = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "TicketActivities") && Tools_2.CRMTool.IsTicketEditEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("ShipmentNumber", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("EntityType", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CompanyId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CustomerContactId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("SeverityId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CreateDate", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("MainClassificationId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("EmployeeGroupId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("OwnerId", this.ObjectTableName, this.IsTicketEditEnabled);
        if (this.PageChild_DS != null) {
            this.PageChild_DS.SetUIProperties();
        }
    };
    TicketMainTabComponent.prototype.SetUIRequiredProperties = function () {
        this.UIProperties.SetRequired("EmployeeGroupId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.EmployeeGroupId));
        this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OwnerId));
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
        this.UIProperties.SetRequired("ShipmentNumber", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
    };
    Object.defineProperty(TicketMainTabComponent.prototype, "IsResolveDue", {
        get: function () {
            return this.EntityPM.IsResolveDue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "ResolveColor", {
        get: function () {
            return this.EntityPM.ResolveColor;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "IsResolveExamination", {
        get: function () {
            return this.EntityPM.IsResolveExamination;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "IsResponseDue", {
        get: function () {
            return this.EntityPM.IsResponseDue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "TicketFirstResponseTime", {
        get: function () {
            return this.EntityPM.TicketFirstResponseTime;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "TicketFirstResolveTime", {
        get: function () {
            return this.EntityPM.TicketFirstResolveTime;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "IsResponseExamination", {
        get: function () {
            return this.EntityPM.IsResponseExamination;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "ResponseColor", {
        get: function () {
            return this.EntityPM.ResponseColor;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "IsTicketEditEnabled", {
        get: function () { return this.isTicketEditEnabled; },
        set: function (value) {
            if (this.isTicketEditEnabled != value) {
                this.isTicketEditEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "IsTicketReplyEnabled", {
        get: function () {
            return this.isTicketReplyEnabled;
        },
        set: function (value) {
            if (this.isTicketReplyEnabled != value) {
                this.isTicketReplyEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "IsTicketActivityEnabled", {
        get: function () {
            return this.isTicketActivityEnabled;
        },
        set: function (value) {
            if (this.isTicketActivityEnabled != value) {
                this.isTicketActivityEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketMainTabComponent.prototype.FillTicketCorrespondenceLines = function () {
        var _this = this;
        this.CorrespondenceList = [];
        this.EntityPM.TicketCorrespondence.sort(function (a, b) { return (Tools_1.DateTool.GetDateParts(a.CreateDate).DateObject.valueOf() === Tools_1.DateTool.GetDateParts(b.CreateDate).DateObject.valueOf()) ? 0 : (Tools_1.DateTool.GetDateParts(a.CreateDate).DateObject.valueOf() > Tools_1.DateTool.GetDateParts(b.CreateDate).DateObject.valueOf()) ? -1 : 1; }).forEach(function (item) {
            _this.CorrespondenceList.push(new CorrespondenceViewModelData(item, _this));
        });
        this.FirstCorrespondence = this.CorrespondenceList[0];
    };
    TicketMainTabComponent.prototype.LoadCommunicationLogsList = function () {
        var _this = this;
        this.TicketCommunicationLogs = [];
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetCommunicationLogs(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var logs = myResponse.Result;
                logs.forEach(function (item) {
                    _this.TicketCommunicationLogs.push(item);
                });
                _this.FillTicketCorrespondenceLines();
            }
        });
    };
    Object.defineProperty(TicketMainTabComponent.prototype, "Subject", {
        // Properties
        get: function () { return this.EntityPM.Subject; },
        set: function (value) {
            if (this.EntityPM.Subject != value) {
                this.EntityPM.Subject = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "EmployeeGroupId", {
        get: function () { return this.EntityPM.EmployeeGroupId; },
        set: function (newValue) {
            if (this.EntityPM.EmployeeGroupId != newValue) {
                this.EntityPM.EmployeeGroupId = newValue;
                this.CheckOwnerEmployeeGroup();
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketMainTabComponent.prototype.CheckOwnerEmployeeGroup = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetOwnerEmployeeGroup(this.OwnerId, this.EmployeeGroupId).subscribe(function (myResult) {
            _this.OwnerId = myResult;
        });
    };
    Object.defineProperty(TicketMainTabComponent.prototype, "OwnerId", {
        get: function () { return this.EntityPM.OwnerId; },
        set: function (newValue) {
            var _this = this;
            this.EntityPM.OwnerId = newValue;
            if (newValue == null) {
                this.EntityPM.BusinessUnitId = null;
            }
            else {
                var myService = new UserListService_1.UserListService();
                myService.getSingleFromCache(newValue).subscribe(function (resp) {
                    if (!resp.HasError) {
                        var result = resp;
                        var list = result.Result;
                        if (list != null) {
                            _this.EntityPM.BusinessUnitId = list.BusinessUnitId;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "TicketTypeId", {
        get: function () { return this.EntityPM.TicketTypeId; },
        set: function (newValue) {
            if (this.EntityPM.TicketTypeId != newValue) {
                this.EntityPM.TicketTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "ContactId", {
        get: function () { return this.EntityPM.ContactId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.ContactId != newValue) {
                this.EntityPM.ContactId = newValue;
                this.ContactListService.getSingle(newValue).subscribe(function (resp) {
                    if (!resp.HasError) {
                        var result = resp.Result;
                        if (result != null) {
                            _this.EntityPM.ContactName = result.EnglishName;
                            _this.EntityPM.ContactEmail = result.Email;
                            _this.EntityPM.ContactPhone = result.BusinessPhone;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "CompanyId", {
        get: function () { return this.EntityPM.CompanyId; },
        set: function (newValue) {
            if (this.EntityPM.CompanyId != newValue) {
                this.EntityPM.CompanyId = newValue;
                this.UpdateCompanyContact();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketMainTabComponent.prototype.UpdateCompanyContact = function () {
        var myContactId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CompanyId)) {
            var myService = new CardListService_1.CardListService();
            myService.getSingle(this.CompanyId).subscribe(function (myResult) {
                if (!myResult.HasError) {
                    var list = myResult.Result;
                    if (list != null) {
                        myContactId = list.PrimaryContactId;
                    }
                }
            });
        }
        this.ContactId = myContactId;
    };
    Object.defineProperty(TicketMainTabComponent.prototype, "ShipmentId", {
        get: function () { return this.EntityPM.ShipmentId; },
        set: function (newValue) {
            if (this.EntityPM.ShipmentId != newValue) {
                this.EntityPM.ShipmentId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "ShipmentNumber", {
        get: function () { return this.EntityPM.ShipmentNumber; },
        set: function (newValue) {
            this.EntityPM.ShipmentNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "ContactName", {
        get: function () { return this.EntityPM.ContactName; },
        set: function (newValue) {
            this.EntityPM.ContactName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "TicketDescription", {
        get: function () { return this.EntityPM.TicketDescription; },
        set: function (newValue) {
            this.EntityPM.TicketDescription = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "MainClassificationId", {
        get: function () { return this.EntityPM.MainClassificationId; },
        set: function (newValue) {
            if (this.EntityPM.MainClassificationId != newValue) {
                this.EntityPM.MainClassificationId = newValue;
                this.SecondaryClassificationId = null;
                this.SetUIProperties();
                this.setDeafaultsValues();
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketMainTabComponent.prototype.setDeafaultsValues = function () {
        var _this = this;
        if (this.SecondaryClassificationId != null) {
            var myService = new TicketClassificationListService_1.TicketClassificationListService();
            myService.getSingleFromCache(this.SecondaryClassificationId).subscribe(function (resp) {
                if (!resp.HasError) {
                    var result = resp.Result;
                    if (result != null) {
                        _this.SeverityId = result.DefaultSeverityId;
                        _this.EmployeeGroupId = result.EmployeeGroupId;
                    }
                }
            });
        }
        else {
            if (this.MainClassificationId != null) {
                var myService = new TicketClassificationListService_1.TicketClassificationListService();
                myService.getSingleFromCache(this.MainClassificationId).subscribe(function (resp) {
                    if (!resp.HasError) {
                        var result = resp.Result;
                        if (result != null) {
                            _this.SeverityId = result.DefaultSeverityId;
                            _this.EmployeeGroupId = result.EmployeeGroupId;
                        }
                    }
                });
            }
            else {
                this.SeverityId = null;
                this.EmployeeGroupId = null;
            }
        }
    };
    Object.defineProperty(TicketMainTabComponent.prototype, "SecondaryClassificationId", {
        get: function () { return this.EntityPM.SecondaryClassificationId; },
        set: function (newValue) {
            if (this.EntityPM.SecondaryClassificationId != newValue) {
                this.EntityPM.SecondaryClassificationId = newValue;
                this.setDeafaultsValues();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "StageId", {
        get: function () { return this.EntityPM.StageId; },
        set: function (newValue) {
            this.EntityPM.StageId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "StageCode", {
        get: function () { return this.EntityPM.StageCode; },
        set: function (newValue) {
            this.EntityPM.StageCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "StageName", {
        get: function () { return this.EntityPM.StageName; },
        set: function (newValue) {
            this.EntityPM.StageName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "SeverityId", {
        get: function () { return this.EntityPM.SeverityId; },
        set: function (newValue) {
            if (this.EntityPM.SeverityId != newValue) {
                this.EntityPM.SeverityId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "CreateDate", {
        get: function () { return this.EntityPM.CreateDate; },
        set: function (newValue) {
            if (this.EntityPM.CreateDate != newValue) {
                this.EntityPM.CreateDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "SubmitButtonEnabled", {
        get: function () {
            var myResult = true;
            if (Tools_1.AppTool.IsNullOrEmpty(this.CorrespondenceLine)) {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "CorrespondenceLine", {
        get: function () { return this.correspondenceLine; },
        set: function (value) {
            if (this.correspondenceLine != value) {
                this.correspondenceLine = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "AddShipmentEnabled", {
        get: function () {
            return this.EntityPM.CompanyId == null ? false : true;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketMainTabComponent.prototype, "DeleteShipmentVisibility", {
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentNumber)) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    TicketMainTabComponent.prototype.SendEmailCommand = function (code) {
        this.code = code;
        this.SendEmail();
    };
    TicketMainTabComponent.prototype.SendEmail = function () {
        var _this = this;
        var validator = new TicketValidator_1.TicketValidator();
        var errors = validator.ValidateCurrenctEntity(this.EntityPM);
        if (errors.length == 0) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = null;
            if (this.code != null) {
                var isInternal = false;
                var windowTitle;
                if (this.code == "Ex") {
                    windowTitle = "Reply";
                    isInternal = false;
                }
                else {
                    windowTitle = "Add Note";
                    isInternal = true;
                }
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 850;
                logWindow.Height = 700;
                logWindow.Title = windowTitle;
                var args = new Args_1.SendEmailArgs();
                args.Ticket = this.EntityPM;
                args.IsInternal = isInternal;
                args.InternalCorrespondenceLinesCount = this.CorrespondenceList.length;
                logWindow.WindowArgs = args;
                logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewLineWindowClosed($event); });
                logWindow.Show('./CRMModules/CRMTickets/Components/EditTabs/MainTab/SendEmailComponent');
            }
        }
        else {
            if (this.CurrentSession.CurrentEditComponent != null) {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            }
        }
    };
    TicketMainTabComponent.prototype.OnNewLineWindowClosed = function (arg) {
        if (arg == "OK") {
            this.RefreshData();
        }
    };
    Object.defineProperty(TicketMainTabComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    // Selected Tab
    TicketMainTabComponent.prototype.SetSelectedTab = function () {
        this.selectedTabCode = "DS";
    };
    TicketMainTabComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.SelectedTabCode != null) {
            var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {
                    case "DS": {
                        if (this.PageChild_DS == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRMModules/CRMTickets/Components/EditTabs/MainTab/DetailsTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.PageChild_DS = cmpRef.instance;
                                _this.PageChild_DS.InitTab(_this);
                            });
                        }
                        else {
                            this.PageChild_DS.RefreshTab(this);
                        }
                        break;
                    }
                    case "OA": {
                        if (this.PageChild_OA == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRMModules/CRMTickets/Components/EditTabs/MainTab/ActivitiesTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.PageChild_OA = cmpRef.instance;
                                _this.PageChild_OA.InitTab(_this);
                            });
                        }
                        else {
                            this.PageChild_OA.InitTab(this);
                        }
                        break;
                    }
                    case 'PT': {
                        if (this.PageChild_PT == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRMModules/CRMTickets/Components/EditTabs/MainTab/PostsTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.PageChild_PT = cmpRef.instance;
                            });
                        }
                        break;
                    }
                }
            }
        }
    };
    //  Activities
    TicketMainTabComponent.prototype.AddActivity = function (code) {
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
            windowArgs.CallWithId = this.EntityPM.ContactId;
            windowArgs.IsOpen = false;
            windowArgs.IsMarkedCompleted = true;
        }
        windowArgs.CustomerId = this.EntityPM.CompanyId;
        windowArgs.TicketId = this.EntityPM.Id;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;
        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.RefreshData();
                }
            });
        });
    };
    Object.defineProperty(TicketMainTabComponent.prototype, "OpenActivitiesHeader", {
        get: function () {
            return this.openActivitiesHeader;
        },
        set: function (value) {
            this.openActivitiesHeader = value;
        },
        enumerable: true,
        configurable: true
    });
    TicketMainTabComponent.prototype.GetActivitiesContent = function () {
        var count = this.ActivitiesList.length;
        this.OpenActivitiesHeader = "Open Activities (" + count + ")";
        ;
    };
    TicketMainTabComponent.prototype.LoadActivities = function () {
        var _this = this;
        this.ActivitiesList = [];
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetActivitiesByTicketId(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var dataResult = myResponse.Result;
                if (dataResult.length > 0) {
                    dataResult.filter(function (d) { return d.IsOpen; }).sort(function (a, b) {
                        return (Tools_1.DateTool.GetDateParts(a.DueDate).DateObject === Tools_1.DateTool.GetDateParts(b.DueDate).DateObject) ? 0 : (Tools_1.DateTool.GetDateParts(a.DueDate).DateObject > Tools_1.DateTool.GetDateParts(b.DueDate).DateObject) ? 1 : -1;
                    }).forEach(function (item) {
                        _this.ActivitiesList.push(new ActivityItemClass(item, _this));
                    });
                }
                _this.GetActivitiesContent();
                if (_this.PageChild_OA != null) {
                    _this.PageChild_OA.InitTab(_this);
                }
            }
        });
    };
    TicketMainTabComponent.prototype.RefreshData = function () {
        this.IsReload = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    TicketMainTabComponent.prototype.RefreshButtonClicked = function () {
        this.IsRefreshButton = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    TicketMainTabComponent.prototype.ReLoadTicket = function () {
        this.LoadCommunicationLogsList();
        this.LoadActivities();
        this.SetUIProperties();
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], TicketMainTabComponent.prototype, "AllLocations", void 0);
    TicketMainTabComponent = __decorate([
        core_1.Component({
            selector: 'MainTabComponent',
            moduleId: module.id,
            templateUrl: './TicketMainTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], TicketMainTabComponent);
    return TicketMainTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TicketMainTabComponent = TicketMainTabComponent;
var CorrespondenceViewModelData = /** @class */ (function (_super) {
    __extends(CorrespondenceViewModelData, _super);
    function CorrespondenceViewModelData(item, trigger) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myBaseHight = "90px";
        _this.LineMaxHeight = "auto";
        _this.LineHeight = "auto";
        _this.LineDisplay = "initial";
        _this.LineTextOverflow = "initial";
        _this.LineOverflow = "auto";
        _this.IsLineVisible = false;
        _this.actualHeight = 0;
        _this.Retries = 0;
        _this.showMoreVisibility = false;
        _this.showLessVisibility = false;
        // Properties
        _this.SourceText = null;
        _this.Recipients = "";
        // Align Commands 
        _this.FlowDirection = "ltr";
        _this.BackgroundAlignRight = "transparent";
        _this.BackgroundAlignLeft = "transparent";
        _this.entityPM = item;
        _this.trigger = trigger;
        _this.Height = _this.myBaseHight;
        _this.ImageSrc = Tools_2.CRMTool.GetActivityImageSrc(_this.entityPM.ActivityTypeCode);
        _this.AttachmentsList = _this.FillAttachments();
        //this.OnTextBoxLoaded();
        _this.RunComponentTimer();
        _this.GetFlowDirection();
        _this.RefreshTextAlgimentVariables();
        var idIndex = _this.CurrentSession.GetNewId("TextArea");
        _this._TextAreaId = "TextArea_" + idIndex;
        _this._TextAreaId2 = "TextArea_2" + idIndex;
        _this.timerToken = setTimeout(function () { return _this.GetRecipients(); }, 1);
        _this.SourceText = _this.GetSourceText();
        return _this;
        //this.GetRecipients();
    }
    CorrespondenceViewModelData.prototype.OnTextBoxLoaded = function () {
        var textarea = document.getElementById(this._TextAreaId);
        var textarea2 = document.getElementById(this._TextAreaId2);
        this.IsLineVisible = true;
        var div = document.createElement("div");
        div.innerText = this.Description;
        div.style.position = "absolute";
        div.style.visibility = "hidden";
        div.style.whiteSpace = "nowrap";
        div.style.width = "auto";
        div.style.height = "auto";
        document.body.appendChild(div);
        this.actualHeight = div.clientHeight;
        var height = 90;
        if (this.actualHeight > height) {
            this.ShowMoreVisibility = true;
            textarea.style.height = this.myBaseHight;
            textarea.style.maxHeight = this.myBaseHight;
        }
        else {
            this.ShowMoreVisibility = false;
            if (textarea != null && textarea.style != null) {
                textarea.style.height = textarea.scrollHeight + "px";
                textarea.style.maxHeight = textarea.scrollHeight + "px";
            }
        }
        //textarea2.style.display = "none";
        document.body.removeChild(div);
    };
    CorrespondenceViewModelData.prototype.GetTextHeight = function (myString, fontSize) {
        if (fontSize === void 0) { fontSize = 12; }
        var myResult = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(myString)) {
            var canvas = document.createElement('canvas');
            var ctx = canvas.getContext("2d");
            ctx.font = fontSize + "px Lucida Sans Unicode";
            var txtHeight = parseInt(ctx.font);
            myResult = canvas.height;
        }
        return myResult;
    };
    CorrespondenceViewModelData.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.OnTextBoxLoaded(); }, 1);
        }
    };
    Object.defineProperty(CorrespondenceViewModelData.prototype, "Height", {
        get: function () {
            return this.height;
        },
        set: function (value) {
            this.height = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "ShowMoreVisibility", {
        get: function () {
            return this.showMoreVisibility;
        },
        set: function (value) {
            this.showMoreVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "ShowLessVisibility", {
        get: function () {
            return this.showLessVisibility;
        },
        set: function (value) {
            this.showLessVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "CorrespondenceHTMLBodyLinkVisibility", {
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.HTMLFullBody)) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "TextAlignRegionVisibility", {
        get: function () {
            var myResult = false;
            if (SessionLocator_1.SessionLocator.TenantPM.IsCorrespondenceRightToLeftEnabled == true) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    CorrespondenceViewModelData.prototype.ShowMore = function () {
        var item = document.getElementById(this._TextAreaId);
        item.style.height = this.actualHeight + "px";
        item.style.maxHeight = this.actualHeight + "px";
        this.ShowMoreVisibility = false;
        this.ShowLessVisibility = true;
    };
    CorrespondenceViewModelData.prototype.ShowLess = function () {
        var item = document.getElementById(this._TextAreaId);
        item.style.height = this.myBaseHight;
        item.style.maxHeight = this.myBaseHight;
        this.ShowMoreVisibility = true;
        this.ShowLessVisibility = false;
    };
    CorrespondenceViewModelData.prototype.GetSourceText = function () {
        var _this = this;
        var sourceText = null;
        if (this.entityPM.Direction != 'I' && this.trigger.TicketCommunicationLogs != null) {
            var list = this.trigger.TicketCommunicationLogs.filter(function (a) { return a.ChildEntityId == _this.entityPM.Id; });
            this.CorrespondenceCommunicationToolTip = "";
            if (list.length > 0) {
                if (list.filter(function (a) { return a.EmailDeliveryError != null; })[0]) {
                    sourceText = "./Images/Orange_ball.png";
                }
                else if (list.filter(function (a) { return a.CommunicationStatusTypeCode == "W"; })[0]) {
                    sourceText = "./Images/Orange_ball.png";
                }
                else if (list.filter(function (a) { return a.CommunicationStatusTypeCode == "F"; })[0]) {
                    sourceText = "./Images/Red_ball.png";
                }
                else if (list.filter(function (a) { return a.CommunicationStatusTypeCode == "C"; })[0]) {
                    sourceText = "./Images/Orange_ball.png";
                }
                else {
                    sourceText = "./Images/Green_ball.png";
                }
                list.forEach(function (item) {
                    if (!(item.CommunicationStatusTypeCode == "W" || item.CommunicationStatusTypeCode == "F" || item.CommunicationStatusTypeCode == "C")) {
                        var communicationLog = item;
                        var error = communicationLog != null ? communicationLog.EmailDeliveryError : "";
                        if (Tools_1.AppTool.IsNullOrEmpty(error)) {
                            if (Tools_1.AppTool.IsNullOrEmpty(_this.CorrespondenceCommunicationToolTip)) {
                                _this.CorrespondenceCommunicationToolTip = "Sent Successfully";
                            }
                            //sourceText =  "./Images/Green_ball.png";
                        }
                        else {
                            _this.CorrespondenceCommunicationToolTip += error;
                            _this.CorrespondenceCommunicationToolTip = _this.CorrespondenceCommunicationToolTip.replace("Sent Successfully", "");
                            //sourceText = "./Images/Orange_ball.png";
                        }
                    }
                    else if (list.filter(function (a) { return a.CommunicationStatusTypeCode == "F"; })[0]) {
                        _this.CorrespondenceCommunicationToolTip = "Error at Sending";
                        //sourceText = "./Images/Red_ball.png";
                    }
                    else if (item.CommunicationStatusTypeCode == "C") {
                        communicationLog = item;
                        var error = communicationLog != null ? communicationLog.EmailDeliveryError : "";
                        if (Tools_1.AppTool.IsNullOrEmpty(error)) {
                            _this.CorrespondenceCommunicationToolTip = "Processing";
                        }
                        else {
                            _this.CorrespondenceCommunicationToolTip += error;
                        }
                        //sourceText = "./Images/Orange_ball.png";
                    }
                    else {
                        _this.CorrespondenceCommunicationToolTip = "Waiting";
                        //sourceText = "./Images/Orange_ball.png";
                    }
                });
            }
            return sourceText;
        }
    };
    Object.defineProperty(CorrespondenceViewModelData.prototype, "Description", {
        get: function () {
            var myResult = this.entityPM.Description;
            return Tools_1.AppTool.IsNullOrEmpty(myResult) ? "" : myResult.trim();
        },
        set: function (value) {
            this.entityPM.Description = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "ActivityTypeCode", {
        get: function () {
            return this.entityPM.ActivityTypeCode;
        },
        set: function (value) {
            this.entityPM.ActivityTypeCode = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "ActivityIconVisibility", {
        get: function () {
            var result = false;
            if (this.ActivityTypeCode != null) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "TextAlignVisibility", {
        get: function () {
            var result = true;
            if (this.ActivityTypeCode != null) {
                result = false;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "ContactName", {
        get: function () {
            return this.entityPM.ContactName;
        },
        set: function (value) {
            this.entityPM.ContactName = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "Id", {
        get: function () {
            return this.entityPM.Id;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "IsInternal", {
        get: function () {
            return this.entityPM.IsInternal;
        },
        set: function (value) {
            this.entityPM.IsInternal = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "CreateDate", {
        get: function () {
            return this.entityPM.CreateDate;
        },
        enumerable: true,
        configurable: true
    });
    CorrespondenceViewModelData.prototype.GetRecipients = function () {
        var recipients = "";
        if (this.entityPM != null) {
            var list = [];
            this.trigger.CorrespondenceList.forEach(function (item) {
                list.push(item);
            });
            this.timerToken = setTimeout(function () { return ""; }, 1);
            var correspondence = list.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.CreateDate).valueOf() === Tools_1.DateTool.GetDateFromDate(b.CreateDate).valueOf()) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.CreateDate).valueOf() < Tools_1.DateTool.GetDateFromDate(b.CreateDate).valueOf()) ? -1 : 1; })[0];
            if (correspondence) {
                var firstCorrespondenceId = correspondence.Id;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.trigger.EntityPM.ContactEmail) && !Tools_1.AppTool.IsNullOrEmpty(this.trigger.EntityPM.ContactEmail.trim()) && !this.entityPM.IsInternal) {
                    if (this.entityPM.ActivityId == null && this.entityPM.Direction == "O") {
                        if (firstCorrespondenceId != this.Id) {
                            recipients += "To: " + this.trigger.EntityPM.ContactEmail + " ";
                        }
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.CCs) && !Tools_1.AppTool.IsNullOrEmpty(this.entityPM.CCs.trim()) && !this.entityPM.IsInternal) {
                    if (firstCorrespondenceId != this.Id) {
                        var myCC = this.entityPM.CCs.replace(";{2;}", ";").trim();
                        while (myCC.charAt(0) == ";")
                            myCC = myCC.substr(1);
                        if (myCC != ";") {
                            recipients += " | " + "CC: " + myCC + " ";
                        }
                    }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.entityPM.InternalUsers) && !Tools_1.AppTool.IsNullOrEmpty(this.entityPM.InternalUsers.trim())) {
                    if (firstCorrespondenceId != this.Id) {
                        var myInternalUsers = this.entityPM.InternalUsers.replace(";{2;}", ";").trim();
                        while (myInternalUsers.charAt(0) == ";")
                            myInternalUsers = myInternalUsers.substr(1);
                        if (myInternalUsers != ";") {
                            recipients += " | " + "Internal Users: " + this.entityPM.InternalUsers + " ";
                        }
                    }
                }
            }
        }
        this.Recipients = recipients;
    };
    Object.defineProperty(CorrespondenceViewModelData.prototype, "LineBoxBackground", {
        get: function () {
            var lineBoxBackground = "rgb(255, 255, 255)";
            if (this.entityPM != null && this.entityPM.ActivityId != null) {
                lineBoxBackground = "rgb(230,230,230)";
            }
            else if (this.entityPM != null && this.entityPM.IsInternal) {
                lineBoxBackground = "rgb(255, 250, 220)";
            }
            else {
                lineBoxBackground = "rgb(255, 255, 255)";
            }
            return lineBoxBackground;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CorrespondenceViewModelData.prototype, "LoggedUserName", {
        get: function () {
            if (this.entityPM.Direction == "O") {
                return SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
            }
            else {
                return this.entityPM.ContactName;
            }
        },
        enumerable: true,
        configurable: true
    });
    CorrespondenceViewModelData.prototype.GetFlowDirection = function () {
        var myResult = "ltr";
        if (SessionLocator_1.SessionLocator.TenantPM.IsCorrespondenceRightToLeftEnabled == true) {
            myResult = "rtl";
            if (this.entityPM.RightToLeft) {
                myResult = "rtl";
            }
            else {
                myResult = "ltr";
            }
        }
        this.FlowDirection = myResult;
    };
    CorrespondenceViewModelData.prototype.GetBackgroundAlignRight = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FlowDirection)) {
            this.BackgroundAlignRight = this.FlowDirection == "rtl" ? "#FDD59D" : "transparent";
        }
    };
    CorrespondenceViewModelData.prototype.GetBackgroundAlignLeft = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FlowDirection)) {
            this.BackgroundAlignLeft = this.FlowDirection == "ltr" ? "#FDD59D" : "transparent";
        }
    };
    CorrespondenceViewModelData.prototype.AlignLeftClicked = function () {
        this.entityPM.RightToLeft = false;
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
        this.UpdateCorrespondence();
    };
    CorrespondenceViewModelData.prototype.AlignRightClicked = function () {
        this.entityPM.RightToLeft = true;
        this.GetFlowDirection();
        this.RefreshTextAlgimentVariables();
        this.UpdateCorrespondence();
    };
    CorrespondenceViewModelData.prototype.RefreshTextAlgimentVariables = function () {
        this.GetBackgroundAlignLeft();
        this.GetBackgroundAlignRight();
    };
    CorrespondenceViewModelData.prototype.UpdateCorrespondence = function () {
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetUpdateCorrespondence(this.entityPM.Id, this.entityPM.RightToLeft).subscribe(function (resp) {
            if (!resp.HasError) {
                //this.trigger.FillTicketCorrespondenceLines();
            }
        });
    };
    CorrespondenceViewModelData.prototype.FillAttachments = function () {
        var _this = this;
        if (this.entityPM == null) {
            return [];
        }
        else {
            var list = [];
            var line = this.trigger.EntityPM.TicketDocumentData.filter(function (a) { return a.CorrespondenceId == _this.entityPM.Id; })[0];
            if (line != null) {
                this.trigger.EntityPM.TicketDocumentData.filter(function (a) { return a.CorrespondenceId == _this.entityPM.Id; }).forEach(function (pm) {
                    var viewmodel = new TicketDocumentDataArgs(pm);
                    list.push(viewmodel);
                });
            }
            return list;
        }
    };
    // Commands 
    CorrespondenceViewModelData.prototype.ViewHTMLBody = function () {
        var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
        var link = "/WebPages/CorrespondenceDisplayPage.aspx?id=" + this.entityPM.Id + "&tempId=" + token;
        window.open(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + link);
    };
    CorrespondenceViewModelData.prototype.EditActivity = function () {
        var _this = this;
        this.trigger._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.entityPM.ActivityId, ObjectTableName: "Activity", BackButtonLabel: "Tickets" });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.trigger.LoadActivities();
                });
            });
        });
    };
    return CorrespondenceViewModelData;
}(BaseComponent_1.BaseComponent));
exports.CorrespondenceViewModelData = CorrespondenceViewModelData;
var TicketDocumentDataArgs = /** @class */ (function () {
    function TicketDocumentDataArgs(documentDataPM) {
        this.DocumentDataPM = documentDataPM;
    }
    Object.defineProperty(TicketDocumentDataArgs.prototype, "FileName", {
        get: function () {
            return this.DocumentDataPM.FileName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDocumentDataArgs.prototype, "FileExtension", {
        get: function () {
            return this.DocumentDataPM.FileExtension;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDocumentDataArgs.prototype, "Tenant", {
        get: function () {
            return this.DocumentDataPM.Tenant;
        },
        enumerable: true,
        configurable: true
    });
    TicketDocumentDataArgs.prototype.ViewAttachment = function () {
        var documentSecurity = this.DocumentDataPM.SecurityId;
        var link = "/WebPages/CorrespondenceDownloadpage.aspx?id=" + documentSecurity + "~" + this.Tenant;
        window.open(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + link);
    };
    return TicketDocumentDataArgs;
}());
exports.TicketDocumentDataArgs = TicketDocumentDataArgs;
var ActivityItemClass = /** @class */ (function (_super) {
    __extends(ActivityItemClass, _super);
    function ActivityItemClass(item, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
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
        if (this.DueDate != null && Tools_1.DateTool.GetDateParts(this.DueDate) < Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateTimeAsUtc())) {
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
                _this.ReopenButtonVisibility = true;
                _this.CompleteButtonVisibility = false;
                _this.CompleteToggleButtonVisibility = false;
                _this.Background = "rgba(0,0,0,0.1)";
                _this.father.LoadActivities();
            }
        });
    };
    ActivityItemClass.prototype.ReopenClicked = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetReopenActivity(this.EntityId).subscribe(function (resp) {
            if (!resp.HasError) {
                _this.entity = resp.Result;
                _this.ReopenButtonVisibility = false;
                if (_this.entity.ActivityTypeCode == "AP") {
                    _this.CompleteToggleButtonVisibility = true;
                }
                else {
                    _this.CompleteButtonVisibility = true;
                }
                _this.Background = "rgb(255,255,255)";
                _this.father.LoadActivities();
            }
        });
    };
    ActivityItemClass.prototype.ViewEntity = function (entity) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this.father._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: "Activity", BackButtonLabel: "Tickets" });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.father.LoadActivities();
                    });
                });
            });
        }
    };
    return ActivityItemClass;
}(BaseComponent_1.BaseComponent));
exports.ActivityItemClass = ActivityItemClass;
//# sourceMappingURL=TicketMainTabComponent.js.map