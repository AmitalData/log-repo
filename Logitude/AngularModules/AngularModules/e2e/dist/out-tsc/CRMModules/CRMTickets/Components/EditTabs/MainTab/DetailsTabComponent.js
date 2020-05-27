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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var UserListService_1 = require("../../../../../Common/Services/StandardLists/UserListService");
var CardListService_1 = require("../../../../../Common/Services/StandardLists/CardListService");
var TicketClassificationListService_1 = require("../../../../../CRM/Services/StandardLists/TicketClassificationListService");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var CRMDomainService_1 = require("../../../../../CRM/Services/CRMDomainService");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Tools_2 = require("../../../../../CRM/Tools");
var TicketSeverityListService_1 = require("../../../../../CRM/Services/StandardLists/TicketSeverityListService");
var ContactListService_1 = require("../../../../../Common/Services/StandardLists/ContactListService");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ContactsTabComponent_1 = require("../../../../../CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent");
var ContactPMService_1 = require("../../../../../Common/Services/StandardPMs/ContactPMService");
var CachedDataManager_1 = require("../../../../../Infrastructure/Utilities/CachedDataManager");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DetailsTabComponent = /** @class */ (function (_super) {
    __extends(DetailsTabComponent, _super);
    function DetailsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.Filters = null;
        _this.QuickSearchItems = [];
        _this.IsFromOutSide = false;
        _this.IsShowConnectContact = false;
        _this.EntityList = [];
        _this.EntityNumberTitle = "Shipment Number";
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsTicketEditEnabled = false;
        _this.FirstClassificationId = "";
        _this.EntityLinkNumberVisibility = false;
        //this.Listen();
        _this.InitializeServices();
        return _this;
    }
    DetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }
    };
    DetailsTabComponent.prototype.ClearFilterData = function () {
        this.ShipmentId = null;
        this.ShipmentNumber = null;
        this.QuoteId = null;
        this.QuoteNumber = null;
    };
    DetailsTabComponent.prototype.InitializeServices = function () {
        this.ContactListService = new ContactListService_1.ContactListService();
        this.TicketSeverityListService = new TicketSeverityListService_1.TicketSeverityListService();
        this.TicketClassificationListService = new TicketClassificationListService_1.TicketClassificationListService();
    };
    DetailsTabComponent.prototype.ngAfterViewInit = function () {
        //if (this.Trigger.EntityPM != null && this.Trigger.EntityPM.TicketCorrespondence != null && this.Trigger.EntityPM.TicketCorrespondence[0].Direction == "O") {
        if (this.Trigger.EntityPM.Source == "MAL") {
            this.IsFromOutSide = true;
        }
        else {
            this.IsFromOutSide = false;
        }
    };
    DetailsTabComponent.prototype.InitTab = function (trigger) {
        var _this = this;
        this.Trigger = trigger;
        this.EntityPM = this.Trigger.EntityPM;
        var objectTable = window.ObjectTables.filter(function (d) { return d.Id === _this.Trigger.EntityPM.EntityType; })[0];
        if (objectTable != null) {
            var objectTableName = window.ObjectTables.filter(function (d) { return d.Id === _this.Trigger.EntityPM.EntityType; })[0].Name;
            if (objectTableName == "Quote") {
                this.EntityNumberTitle = "Quote Number";
            }
        }
        this.Filters = new ApiQueryFilters_1.ApiQueryFilters();
        this.Filters.PageIndex = 0;
        this.Filters.PageSize = 10;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CompanyId)) {
            this.Filters.addAdditionalFilter("CustomerId", this.CompanyId, null, null, "Equals", false, false, false, "string");
        }
        this.ObjectTableName = this.Trigger.ObjectTableName;
        this.SetUIProperties();
        this.getGeneralClassification();
    };
    DetailsTabComponent.prototype.RefreshTab = function (trigger) {
        this.EntityPM = this.Trigger.EntityPM;
    };
    DetailsTabComponent.prototype.SetUIProperties = function () {
        var descriptionIsRequired = Tools_1.AppTool.IsNullOrEmpty(this.TicketDescription) ? true : false;
        this.UIProperties.SetRequired("TicketDescription", this.ObjectTableName, descriptionIsRequired);
        if (this.InternalMode) {
            this.ContactIdCustomFilter = "ContactIdCustomFilter";
            //this.CustomerContactId = this.ContactId;
            this.CompanyIdCustomFilter = null;
        }
        else {
            this.ContactIdCustomFilter = null;
            this.CompanyIdCustomFilter = this.CompanyId;
        }
        this.UIProperties.SetVisibility("CustomerContactId", this.ObjectTableName, this.InternalMode);
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.MainClassificationId));
        this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OwnerId));
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
        this.GetEntityLinkNumberVisibility();
        this.SetUIProperties_EntityClosed();
    };
    DetailsTabComponent.prototype.SetUIProperties_EntityClosed = function () {
        this.IsTicketEditEnabled = Tools_2.CRMTool.IsTicketEditEnabled(this.Trigger.EntityPM);
        this.UIProperties.SetEnabled("ShipmentNumber", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("ShipmentNumber", this.ObjectTableName, this.IsTicketEditEnabled);
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
        this.UIProperties.SetEnabled("InternalMode", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("TicketDescription", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("ClosureDescription", this.ObjectTableName, this.IsTicketEditEnabled);
    };
    Object.defineProperty(DetailsTabComponent.prototype, "CompanyId", {
        // Properties 
        get: function () { return this.Trigger.EntityPM.CompanyId; },
        set: function (newValue) {
            if (this.Trigger.EntityPM.CompanyId != newValue) {
                this.Trigger.EntityPM.CompanyId = newValue;
                this.SetUIProperties();
                this.UpdateCompanyContact();
                this.Filters = new ApiQueryFilters_1.ApiQueryFilters();
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.Filters.addAdditionalFilter("CustomerId", newValue, null, null, "Equals", false, false, false, "string");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    DetailsTabComponent.prototype.UpdateCompanyContact = function () {
        var _this = this;
        if (this.IsFromOutSide) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CompanyId) && !Tools_1.AppTool.IsNullOrEmpty(this.ContactId)) {
                // Check if connected or not 
                var myDomainService = new CRMDomainService_1.CRMDomainService();
                myDomainService.GetContactCards(this.CompanyId, this.ContactId).subscribe(function (myResult) {
                    if (myResult == null) {
                        _this.IsShowConnectContact = true;
                    }
                    else {
                        _this.IsShowConnectContact = false;
                    }
                });
            }
        }
        else {
            if (this.InternalMode) {
                this.CustomerContactId = null;
            }
            else {
                this.ContactId = null;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CompanyId)) {
                var myService = new CardListService_1.CardListService();
                myService.getSingle(this.CompanyId).subscribe(function (myResult) {
                    if (!myResult.HasError) {
                        var list = myResult.Result;
                        if (list != null) {
                            if (_this.InternalMode) {
                                _this.CustomerContactId = list.PrimaryContactId;
                            }
                            else {
                                _this.ContactId = list.PrimaryContactId;
                            }
                        }
                    }
                });
            }
        }
    };
    Object.defineProperty(DetailsTabComponent.prototype, "ShipmentId", {
        get: function () { return this.Trigger.EntityPM.ShipmentId; },
        set: function (newValue) {
            if (this.Trigger.EntityPM.ShipmentId != newValue) {
                this.Trigger.EntityPM.ShipmentId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "ShipmentNumber", {
        get: function () { return this.Trigger.EntityPM.ShipmentNumber; },
        set: function (newValue) {
            this.Trigger.EntityPM.ShipmentNumber = newValue;
            this.GetEntityLinkNumberVisibility();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "QuoteId", {
        get: function () { return this.Trigger.EntityPM.QuoteId; },
        set: function (newValue) {
            this.Trigger.EntityPM.QuoteId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "QuoteNumber", {
        get: function () { return this.Trigger.EntityPM.QuoteNumber; },
        set: function (newValue) {
            if (this.Trigger.EntityPM.QuoteNumber != newValue) {
                this.Trigger.EntityPM.QuoteNumber = newValue;
                this.GetEntityLinkNumberVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "ContactId", {
        get: function () { return this.Trigger.EntityPM.ContactId; },
        set: function (newValue) {
            var _this = this;
            //if (this.Trigger.EntityPM.ContactId != newValue) {
            this.Trigger.EntityPM.ContactId = newValue;
            this.ContactListService.getSingle(newValue).subscribe(function (resp) {
                if (!resp.HasError) {
                    var result = resp.Result;
                    if (result != null) {
                        _this.Trigger.EntityPM.ContactName = result.EnglishName;
                        _this.Trigger.EntityPM.ContactEmail = result.Email;
                        _this.Trigger.EntityPM.ContactPhone = result.BusinessPhone;
                    }
                }
            });
            //}
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "Subject", {
        get: function () { return this.Trigger.EntityPM.Subject; },
        set: function (newValue) {
            if (this.Trigger.EntityPM.Subject != newValue) {
                this.Trigger.EntityPM.Subject = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "TicketDescription", {
        get: function () { return this.Trigger.EntityPM.TicketDescription; },
        set: function (newValue) {
            if (this.Trigger.EntityPM.TicketDescription != newValue) {
                this.Trigger.EntityPM.TicketDescription = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "ClosureDescription", {
        get: function () { return this.Trigger.EntityPM.ClosureDescription; },
        set: function (newValue) {
            if (this.Trigger.EntityPM.ClosureDescription != newValue) {
                this.Trigger.EntityPM.ClosureDescription = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "MainClassificationId", {
        get: function () { return this.Trigger.EntityPM.MainClassificationId; },
        set: function (newValue) {
            var _this = this;
            if (this.Trigger.EntityPM.MainClassificationId != newValue) {
                this.Trigger.EntityPM.MainClassificationId = newValue;
                this.getGeneralClassification();
                this.SecondaryClassificationId = null;
                this.SetUIProperties();
                this.setDeafaultsValues();
                this.TicketClassificationListService.getSingleFromCache(newValue).subscribe(function (resp) {
                    if (!resp.HasError) {
                        var result = resp.Result;
                        if (result != null) {
                            _this.Trigger.EntityPM.MainClassificationName = result.Name;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    DetailsTabComponent.prototype.setDeafaultsValues = function () {
        var _this = this;
        if (this.SecondaryClassificationId != null) {
            this.TicketClassificationListService.getSingleFromCache(this.SecondaryClassificationId).subscribe(function (resp) {
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
                this.TicketClassificationListService.getSingleFromCache(this.MainClassificationId).subscribe(function (resp) {
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
    Object.defineProperty(DetailsTabComponent.prototype, "SecondaryClassificationId", {
        get: function () { return this.Trigger.EntityPM.SecondaryClassificationId; },
        set: function (newValue) {
            var _this = this;
            if (this.Trigger.EntityPM.SecondaryClassificationId != newValue) {
                this.Trigger.EntityPM.SecondaryClassificationId = newValue;
                this.setDeafaultsValues();
                this.TicketClassificationListService.getSingleFromCache(newValue).subscribe(function (resp) {
                    if (!resp.HasError) {
                        var result = resp.Result;
                        if (result != null) {
                            _this.Trigger.EntityPM.SecondaryClassificationName = result.Name;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    DetailsTabComponent.prototype.getGeneralClassification = function () {
        var _this = this;
        this.FirstClassificationId = "";
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        this.TicketClassificationListService.getAllFromCache(filters).subscribe(function (resp) {
            if (!resp.HasError) {
                var result = resp.Result;
                var myClassification = result.filter(function (d) { return d.Name == "General" && d.Tenant == SessionLocator_1.SessionLocator.TenantPM.Id; })[0];
                if (myClassification != null) {
                    var filter = "!F";
                    _this.FirstClassificationId = myClassification.Id.concat(filter);
                }
            }
        });
    };
    Object.defineProperty(DetailsTabComponent.prototype, "SecondClassificationId", {
        get: function () {
            var myGeneralId = "";
            if (this.MainClassificationId != null) {
                var filter = "!S";
                myGeneralId = this.MainClassificationId.concat(filter);
            }
            return myGeneralId;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "SeverityId", {
        get: function () { return this.Trigger.EntityPM.SeverityId; },
        set: function (newValue) {
            var _this = this;
            if (this.Trigger.EntityPM.SeverityId != newValue) {
                this.Trigger.EntityPM.SeverityId = newValue;
                this.TicketSeverityListService.getSingleFromCache(newValue).subscribe(function (result) {
                    var severity = result.Result;
                    if (severity != null)
                        _this.Trigger.EntityPM.SeverityName = severity.Name;
                    else
                        _this.Trigger.EntityPM.SeverityName = null;
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "EmployeeGroupId", {
        get: function () { return this.Trigger.EntityPM.EmployeeGroupId; },
        set: function (newValue) {
            if (this.Trigger.EntityPM.EmployeeGroupId != newValue) {
                this.Trigger.EntityPM.EmployeeGroupId = newValue;
                this.CheckOwnerEmployeeGroup();
            }
        },
        enumerable: true,
        configurable: true
    });
    DetailsTabComponent.prototype.CheckOwnerEmployeeGroup = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetOwnerEmployeeGroup(this.OwnerId, this.EmployeeGroupId).subscribe(function (myResult) {
            _this.OwnerId = myResult;
        });
    };
    Object.defineProperty(DetailsTabComponent.prototype, "OwnerId", {
        get: function () { return this.Trigger.EntityPM.OwnerId; },
        set: function (newValue) {
            var _this = this;
            this.Trigger.EntityPM.OwnerId = newValue;
            this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OwnerId));
            if (newValue == null) {
                this.Trigger.EntityPM.BusinessUnitId = null;
                this.Trigger.EntityPM.OwnerName = null;
            }
            else {
                var myService = new UserListService_1.UserListService();
                myService.getSingleFromCache(newValue).subscribe(function (resp) {
                    if (!resp.HasError) {
                        var result = resp;
                        var list = result.Result;
                        if (list != null) {
                            _this.Trigger.EntityPM.BusinessUnitId = list.BusinessUnitId;
                            _this.Trigger.EntityPM.OwnerName = list.EnglishName;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "CreateDate", {
        get: function () { return this.Trigger.EntityPM.CreateDate; },
        set: function (newValue) {
            if (this.Trigger.EntityPM.CreateDate != newValue) {
                this.Trigger.EntityPM.CreateDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "CustomerContactId", {
        get: function () { return this.Trigger.EntityPM.CustomerContactId; },
        set: function (value) {
            if (this.Trigger.EntityPM.CustomerContactId != value) {
                this.Trigger.EntityPM.CustomerContactId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "IsEditContactEnabled", {
        get: function () {
            return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ContactId) ? false : true;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "InternalMode", {
        get: function () { return this.Trigger.EntityPM.InternalMode; },
        set: function (value) {
            if (this.Trigger.EntityPM.InternalMode != value) {
                this.Trigger.EntityPM.InternalMode = value;
                this.SetCustomerContactValue();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    DetailsTabComponent.prototype.SetCustomerContactValue = function () {
        this.UIProperties.SetVisibility("CustomerContactId", this.ObjectTableName, this.InternalMode);
        if (this.InternalMode) {
            this.ContactIdCustomFilter = "ContactIdCustomFilter";
            this.CustomerContactId = this.ContactId;
            if (!this.IsFromOutSide) {
                this.ContactId = null;
            }
            this.CompanyIdCustomFilter = null;
        }
        else {
            if (!this.IsFromOutSide) {
                this.ContactId = this.CustomerContactId;
            }
            this.CustomerContactId = null;
            this.CompanyIdCustomFilter = this.CompanyId;
            this.ContactIdCustomFilter = null;
        }
        this.SetUIProperties();
    };
    Object.defineProperty(DetailsTabComponent.prototype, "EntityObjectTableName", {
        get: function () { return this.entityObjectTableName; },
        set: function (value) {
            if (this.entityObjectTableName != value) {
                this.entityObjectTableName = value;
                if (value == "Shipment") {
                    this.EntityNumberTitle = "Shipment Number";
                }
                else {
                    this.EntityNumberTitle = "Quote Number";
                }
                this.GetEntityLinkNumberVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DetailsTabComponent.prototype, "EntityType", {
        get: function () { return this.Trigger.EntityPM.EntityType; },
        set: function (value) {
            if (this.Trigger.EntityPM.EntityType != value) {
                this.Trigger.EntityPM.EntityType = value;
                this.ShipmentNumber = null;
                this.QuoteNumber = null;
                this.GetEntityLinkNumberVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    DetailsTabComponent.prototype.GetEntityLinkNumberVisibility = function () {
        var myResult = false;
        if ((this.EntityObjectTableName == "Shipment" && !Tools_1.AppTool.IsNullOrEmpty(this.ShipmentNumber)) || (this.EntityObjectTableName == "Quote" && !Tools_1.AppTool.IsNullOrEmpty(this.QuoteNumber))) {
            myResult = true;
        }
        this.EntityLinkNumberVisibility = myResult;
    };
    DetailsTabComponent.prototype.ChooseShipment = function () {
        var _this = this;
        if (this.IsTicketEditEnabled) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 800;
            logWindow.Height = 570;
            logWindow.Title = "Shipments Search";
            logWindow.WindowArgs = this.Trigger.EntityPM;
            logWindow.Show('./CRMModules/CRMTickets/Components/NewEntity/ChooseShipmentComponent');
            logWindow.ComponentLoaded.subscribe(function (s) {
                logWindow.WindowClosed.subscribe(function (d) {
                    var shipmentList = s.SelectedShipment;
                    if (shipmentList != null) {
                        _this.ShipmentId = shipmentList.Id;
                        _this.ShipmentNumber = shipmentList.ShipmentNumber;
                        _this.CompanyId = shipmentList.CustomerId;
                    }
                });
            });
        }
    };
    DetailsTabComponent.prototype.QuickSearchTextChanged = function (entity) {
        if (this.EntityObjectTableName == "Shipment") {
            this.ShipmentNumber = entity.ShipmentNumber;
            this.ShipmentId = entity.Id;
            this.CompanyId = entity.CustomerId;
        }
        else {
            this.QuoteNumber = entity.QuoteNumber;
            this.QuoteId = entity.Id;
            this.CompanyId = entity.CustomerId;
        }
    };
    DetailsTabComponent.prototype.ChooseEntity = function () {
        var _this = this;
        if (this.IsTicketEditEnabled) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 800;
            logWindow.Height = 570;
            var args = {};
            args.IsFromTicket = true;
            if (this.EntityObjectTableName == "Shipment") {
                logWindow.Title = "Shipments Search";
            }
            else {
                logWindow.Title = "Quotes Search";
            }
            args.EntityObjectTableName = this.EntityObjectTableName;
            logWindow.WindowArgs = args;
            logWindow.Show('./CommonModules/CommonFilingInbox/Components/ChooseEntityComponent');
            logWindow.ComponentLoaded.subscribe(function (s) {
                logWindow.WindowClosed.subscribe(function (d) {
                    var entityList = null;
                    if (s.EntityObjectTableName == "Shipment") {
                        entityList = s.SelectedShipment;
                    }
                    else {
                        entityList = s.SelectedQuote;
                    }
                    if (entityList != null) {
                        if (s.EntityObjectTableName == "Shipment") {
                            _this.ShipmentNumber = entityList.ShipmentNumber;
                            _this.ShipmentId = entityList.Id;
                            _this.CompanyId = entityList.CustomerId;
                        }
                        else {
                            _this.QuoteNumber = entityList.QuoteNumber;
                            _this.QuoteId = entityList.Id;
                            _this.CompanyId = entityList.CustomerId;
                        }
                    }
                });
            });
        }
    };
    DetailsTabComponent.prototype.AddButtonClicked = function () {
        var _this = this;
        var path = './Quote/ComponentsNewEntity/NewQuoteComponent';
        var windowTitle = "New Quote";
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(function (response) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = windowTitle;
            logWindow.Show(path);
            logWindow.ComponentLoaded.subscribe(function (s) {
                logWindow.WindowClosed.subscribe(function (d) {
                    if (s && d == "OK") {
                        var quote = s.EntityPM;
                        if (quote != null) {
                            _this.QuoteNumber = quote.QuoteNumber;
                            _this.QuoteId = quote.Id;
                            _this.CompanyId = quote.CustomerId;
                        }
                    }
                });
            });
        });
    };
    DetailsTabComponent.prototype.ViewShipmentClicked = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.ShipmentId, ObjectTableName: 'Shipment', BackButtonLabel: "Tickets" });
        });
    };
    DetailsTabComponent.prototype.ViewQuoteClicked = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.QuoteId, ObjectTableName: 'Quote', BackButtonLabel: "Tickets" });
        });
    };
    DetailsTabComponent.prototype.DeleteEntity = function () {
        if (this.IsTicketEditEnabled) {
            this.ShipmentId = null;
            this.QuoteId = null;
            this.ShipmentNumber = null;
            this.QuoteNumber = null;
            this.EntityLinkNumberVisibility = false;
        }
    };
    DetailsTabComponent.prototype.ConectContactClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Connecting");
        var myDomainService = new CRMDomainService_1.CRMDomainService();
        myDomainService.GetConnectContactCards(this.CompanyId, this.ContactId).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.IsShowConnectContact = false;
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    DetailsTabComponent.prototype.AddCompanyClicked = function () {
        var _this = this;
        var str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
        str = "New Potential Customer";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 660;
        logWindow.Height = 570;
        logWindow.Title = str;
        var args = {};
        args.ShowContactPart = true;
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                if (d) {
                    var customer = s.EntityPM;
                    if (customer != null) {
                        _this.CompanyId = customer.Id;
                    }
                }
            });
        });
    };
    DetailsTabComponent.prototype.EditContactClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "Update Contact";
        var myService = new ContactPMService_1.ContactPMService();
        myService.get(this.ContactId).subscribe(function (myResult) {
            if (!myResult.HasError) {
                var pm = myResult.Result;
                var itemComponent = new ContactsTabComponent_1.ContactItemClass(pm, null, false);
                logWindow.DataContext = itemComponent;
                var args = {};
                logWindow.WindowArgs = args;
                logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/AddEditContactComponent');
                logWindow.WindowClosed.subscribe(function ($event) { return _this.OnEditContactWindowClosed($event); });
            }
        });
    };
    DetailsTabComponent.prototype.OnEditContactWindowClosed = function (arg) {
        if (arg != 'cancel') {
            // Refresh user table
            CachedDataManager_1.CachedDataManager.RefreshTableData("User", true);
            this.ContactId = arg;
        }
    };
    DetailsTabComponent = __decorate([
        core_1.Component({
            selector: 'DetailsTabComponent',
            moduleId: module.id,
            templateUrl: './DetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], DetailsTabComponent);
    return DetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DetailsTabComponent = DetailsTabComponent;
var EntityClass = /** @class */ (function () {
    function EntityClass() {
    }
    return EntityClass;
}());
exports.EntityClass = EntityClass;
//# sourceMappingURL=DetailsTabComponent.js.map