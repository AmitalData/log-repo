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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var CRMDomainService_1 = require("../../../../../CRM/Services/CRMDomainService");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ContactInputTemplate_1 = require("../../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate");
var UserListService_1 = require("../../../../../Common/Services/StandardLists/UserListService");
var CardListService_1 = require("../../../../../Common/Services/StandardLists/CardListService");
var TicketClassificationListService_1 = require("../../../../../CRM/Services/StandardLists/TicketClassificationListService");
var Tools_2 = require("../../../../../CRM/Tools");
var TicketSeverityListService_1 = require("../../../../../CRM/Services/StandardLists/TicketSeverityListService");
var ContactListService_1 = require("../../../../../Common/Services/StandardLists/ContactListService");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ContactsTabComponent_1 = require("../../../../../CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent");
var ContactPMService_1 = require("../../../../../Common/Services/StandardPMs/ContactPMService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var TicketDetailsTabComponent = /** @class */ (function (_super) {
    __extends(TicketDetailsTabComponent, _super);
    function TicketDetailsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.LabelColumnWidth = 153;
        _this.ControlColumnWidth = 180;
        _this.DataContext = _this;
        _this.ObjectTableName = "Ticket";
        _this.IsFromOutSide = false;
        _this.IsShowConnectContact = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.EntityList = [];
        _this.EntityNumberTitle = "Shipment Number";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TabSelectedEvent = null;
        _this.Retries = 0;
        _this.selectedFilter = null;
        _this.FirstClassificationId = "";
        _this.EntityLinkNumberVisibility = false;
        _this.EntityPM = _this.entityArgs.EntityPM;
        var objectTable = window.ObjectTables.filter(function (d) { return d.Id === _this.EntityPM.EntityType; })[0];
        if (objectTable != null) {
            var objectTableName = window.ObjectTables.filter(function (d) { return d.Id === _this.EntityPM.EntityType; })[0].Name;
            if (objectTableName == "Quote") {
                _this.EntityNumberTitle = "Quote Number";
            }
        }
        _this.CreateEntities();
        _this.SetUIProperties();
        _this.getGeneralClassification();
        _this.Listen();
        _this.InitializeServices();
        _this.RunComponent();
        return _this;
    }
    TicketDetailsTabComponent.prototype.ngAfterViewInit = function () {
        //if (this.EntityPM != null && this.EntityPM.TicketCorrespondence != null && this.EntityPM.TicketCorrespondence[0].Direction == "O") {
        if (this.EntityPM.Source == "MAL") {
            this.IsFromOutSide = true;
        }
        else {
            this.IsFromOutSide = false;
        }
    };
    TicketDetailsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.SetUIProperties_EntityClosed();
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.SetUIProperties_EntityClosed();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "TIGE") {
                    _this.GetEntityLinkNumberVisibility();
                    _this.CreateEntities();
                    _this.UpdateEntityDetails();
                    _this.SetUIProperties();
                }
            });
        }
    };
    TicketDetailsTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    Object.defineProperty(TicketDetailsTabComponent.prototype, "SelectedFilter", {
        get: function () {
            return this.selectedFilter;
        },
        set: function (value) {
            if (this.selectedFilter != value) {
                this.selectedFilter = value;
                this.UpdateEntityDetails();
                this.ClearFilterData();
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketDetailsTabComponent.prototype.UpdateEntityDetails = function () {
        if (this.SelectedFilter != null && this.SelectedFilter.Code == "1") {
            this.EntityObjectTableName = "Shipment";
            this.EntityNumberTitle = "Shipment Number";
        }
        else {
            this.EntityObjectTableName = "Quote";
            this.EntityNumberTitle = "Quote Number";
        }
    };
    TicketDetailsTabComponent.prototype.ClearFilterData = function () {
        this.ShipmentId = null;
        this.ShipmentNumber = null;
        this.QuoteId = null;
        this.QuoteNumber = null;
    };
    TicketDetailsTabComponent.prototype.CreateEntities = function () {
        this.EntityList = [];
        var s_entity = new EntityClass();
        s_entity.Code = "1";
        s_entity.Name = "Shipment";
        this.EntityList.push(s_entity);
        var q_entity = new EntityClass();
        q_entity.Code = "2";
        q_entity.Name = "Quote";
        this.EntityList.push(q_entity);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.QuoteId)) {
            this.EntityNumberTitle = "Quote Number";
            this.EntityObjectTableName = "Quote";
            this.selectedFilter = q_entity;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentId)) {
            this.EntityNumberTitle = "Shipment Number";
            this.EntityObjectTableName = "Shipment";
            this.selectedFilter = s_entity;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.QuoteId) && Tools_1.AppTool.IsNullOrEmpty(this.QuoteId)) {
            this.EntityNumberTitle = "Shipment Number";
            this.EntityObjectTableName = "Shipment";
            this.selectedFilter = s_entity;
        }
    };
    TicketDetailsTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    TicketDetailsTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            _this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.LoadCompleted.subscribe(function (s) {
                _this.SetUIProperties_GeneratedComponent();
            });
            cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, "Ticket.AdditionalFields");
        });
    };
    TicketDetailsTabComponent.prototype.SetUIProperties_GeneratedComponent = function () {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.IsTicketEditEnabled);
        }
    };
    TicketDetailsTabComponent.prototype.InitializeServices = function () {
        this.ContactListService = new ContactListService_1.ContactListService();
        this.TicketSeverityListService = new TicketSeverityListService_1.TicketSeverityListService();
        this.TicketClassificationListService = new TicketClassificationListService_1.TicketClassificationListService();
    };
    TicketDetailsTabComponent.prototype.SetUIProperties = function () {
        if (this.InternalMode) {
            this.ContactIdCustomFilter = "ContactIdCustomFilter";
            this.CompanyIdCustomFilter = null;
            //this.CustomerContactId = this.ContactId;
            this.UIProperties.SetEnabled("CustomerContactId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, true);
        }
        else {
            this.CompanyIdCustomFilter = this.CompanyId;
            this.ContactIdCustomFilter = null;
            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
        }
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.MainClassificationId));
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentNumber) || !Tools_1.AppTool.IsNullOrEmpty(this.QuoteNumber)) {
            this.EntityLinkNumberVisibility = true;
        }
        this.UIProperties.SetVisibility("CustomerContactId", this.ObjectTableName, this.InternalMode);
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
        this.SetUIProperties_EntityClosed();
    };
    TicketDetailsTabComponent.prototype.SetUIProperties_EntityClosed = function () {
        this.IsTicketEditEnabled = Tools_2.CRMTool.IsTicketEditEnabled(this.EntityPM);
        if (!this.IsTicketEditEnabled) {
            this.AddContactEnabled = false;
        }
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
        this.UIProperties.SetEnabled("InternalMode", this.ObjectTableName, this.IsTicketEditEnabled);
    };
    Object.defineProperty(TicketDetailsTabComponent.prototype, "IsTicketEditEnabled", {
        get: function () { return this.isTicketEditEnabled; },
        set: function (value) {
            if (this.isTicketEditEnabled != value) {
                this.isTicketEditEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "CompanyId", {
        // Properties 
        get: function () { return this.EntityPM.CompanyId; },
        set: function (newValue) {
            if (this.EntityPM.CompanyId != newValue) {
                this.EntityPM.CompanyId = newValue;
                this.GetEntityLinkNumberVisibility();
                this.SetUIProperties();
                this.UpdateCompanyContact();
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketDetailsTabComponent.prototype.UpdateCompanyContact = function () {
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
    Object.defineProperty(TicketDetailsTabComponent.prototype, "ContactId", {
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
                            _this.EntityPM.ContactPhone = result.BusinessPhone;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "AddContactEnabled", {
        get: function () {
            this.addContactEnabled = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CompanyId) && this.IsTicketEditEnabled) {
                this.addContactEnabled = true;
            }
            return this.addContactEnabled;
        },
        set: function (value) {
            if (this.addContactEnabled != value) {
                this.addContactEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "Subject", {
        get: function () { return this.EntityPM.Subject; },
        set: function (newValue) {
            if (this.EntityPM.Subject != newValue) {
                this.EntityPM.Subject = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "MainClassificationId", {
        get: function () { return this.EntityPM.MainClassificationId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.MainClassificationId != newValue) {
                this.EntityPM.MainClassificationId = newValue;
                this.getGeneralClassification();
                this.SecondaryClassificationId = null;
                this.SetUIProperties();
                this.setDeafaultsValues();
                this.TicketClassificationListService.getSingleFromCache(newValue).subscribe(function (resp) {
                    if (!resp.HasError) {
                        var result = resp.Result;
                        if (result != null) {
                            _this.EntityPM.MainClassificationName = result.Name;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketDetailsTabComponent.prototype.setDeafaultsValues = function () {
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
    Object.defineProperty(TicketDetailsTabComponent.prototype, "SecondaryClassificationId", {
        get: function () { return this.EntityPM.SecondaryClassificationId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.SecondaryClassificationId != newValue) {
                this.EntityPM.SecondaryClassificationId = newValue;
                this.setDeafaultsValues();
                this.TicketClassificationListService.getSingleFromCache(newValue).subscribe(function (resp) {
                    if (!resp.HasError) {
                        var result = resp.Result;
                        if (result != null) {
                            _this.EntityPM.SecondaryClassificationName = result.Name;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketDetailsTabComponent.prototype.getGeneralClassification = function () {
        var _this = this;
        this.FirstClassificationId = "";
        var myService = new TicketClassificationListService_1.TicketClassificationListService();
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        myService.getAllFromCache(filters).subscribe(function (resp) {
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
    Object.defineProperty(TicketDetailsTabComponent.prototype, "SecondClassificationId", {
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
    Object.defineProperty(TicketDetailsTabComponent.prototype, "SeverityId", {
        get: function () { return this.EntityPM.SeverityId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.SeverityId != newValue) {
                this.EntityPM.SeverityId = newValue;
                this.TicketSeverityListService.getSingleFromCache(newValue).subscribe(function (result) {
                    var severity = result.Result;
                    if (severity != null)
                        _this.EntityPM.SeverityName = severity.Name;
                    else
                        _this.EntityPM.SeverityName = null;
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "EmployeeGroupId", {
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
    TicketDetailsTabComponent.prototype.CheckOwnerEmployeeGroup = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetOwnerEmployeeGroup(this.OwnerId, this.EmployeeGroupId).subscribe(function (myResult) {
            _this.OwnerId = myResult;
        });
    };
    Object.defineProperty(TicketDetailsTabComponent.prototype, "OwnerId", {
        get: function () { return this.EntityPM.OwnerId; },
        set: function (newValue) {
            var _this = this;
            this.EntityPM.OwnerId = newValue;
            if (newValue == null) {
                this.EntityPM.BusinessUnitId = null;
                this.EntityPM.OwnerName = null;
            }
            else {
                var myService = new UserListService_1.UserListService();
                myService.getSingleFromCache(newValue).subscribe(function (resp) {
                    if (!resp.HasError) {
                        var result = resp;
                        var list = result.Result;
                        if (list != null) {
                            _this.EntityPM.BusinessUnitId = list.BusinessUnitId;
                            _this.EntityPM.OwnerName = list.EnglishName;
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "EntityType", {
        get: function () { return this.EntityPM.EntityType; },
        set: function (newValue) {
            if (this.EntityPM.EntityType != newValue) {
                this.EntityPM.EntityType = newValue;
                this.GetEntityLinkNumberVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "ShipmentId", {
        get: function () { return this.EntityPM.ShipmentId; },
        set: function (newValue) {
            if (this.EntityPM.ShipmentId != newValue) {
                this.EntityPM.ShipmentId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "ShipmentNumber", {
        get: function () { return this.EntityPM.ShipmentNumber; },
        set: function (newValue) {
            this.EntityPM.ShipmentNumber = newValue;
            this.GetEntityLinkNumberVisibility();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "QuoteId", {
        get: function () { return this.EntityPM.QuoteId; },
        set: function (newValue) {
            this.EntityPM.QuoteId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "QuoteNumber", {
        get: function () { return this.EntityPM.QuoteNumber; },
        set: function (newValue) {
            if (this.EntityPM.QuoteNumber != newValue) {
                this.EntityPM.QuoteNumber = newValue;
                this.GetEntityLinkNumberVisibility();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "CreateDate", {
        get: function () { return this.EntityPM.CreateDate; },
        set: function (newValue) {
            if (this.EntityPM.CreateDate != newValue) {
                this.EntityPM.CreateDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "CustomerContactId", {
        get: function () { return this.EntityPM.CustomerContactId; },
        set: function (value) {
            if (this.EntityPM.CustomerContactId != value) {
                this.EntityPM.CustomerContactId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "InternalMode", {
        get: function () { return this.EntityPM.InternalMode; },
        set: function (value) {
            if (this.EntityPM.InternalMode != value) {
                this.EntityPM.InternalMode = value;
                this.SetCustomerContactValue();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "InternalUsers", {
        get: function () { return this.EntityPM.InternalUsers; },
        set: function (value) {
            if (this.EntityPM.InternalUsers != value) {
                this.EntityPM.InternalUsers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketDetailsTabComponent.prototype, "CCs", {
        get: function () { return this.EntityPM.CCs; },
        set: function (value) {
            if (this.EntityPM.CCs != value) {
                this.EntityPM.CCs = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketDetailsTabComponent.prototype.SetCustomerContactValue = function () {
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
    Object.defineProperty(TicketDetailsTabComponent.prototype, "IsEditContactEnabled", {
        get: function () {
            return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ContactId) ? false : true;
        },
        enumerable: true,
        configurable: true
    });
    TicketDetailsTabComponent.prototype.GetEntityLinkNumberVisibility = function () {
        var myResult = false;
        if ((this.EntityObjectTableName == "Shipment" && !Tools_1.AppTool.IsNullOrEmpty(this.ShipmentNumber)) || (this.EntityObjectTableName == "Quote" && !Tools_1.AppTool.IsNullOrEmpty(this.QuoteNumber))) {
            myResult = true;
        }
        this.EntityLinkNumberVisibility = myResult;
    };
    TicketDetailsTabComponent.prototype.QuickSearchTextChanged = function (entity) {
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
    Object.defineProperty(TicketDetailsTabComponent.prototype, "EntityObjectTableName", {
        get: function () { return this.entityObjectTableName; },
        set: function (value) {
            if (this.entityObjectTableName != value) {
                this.entityObjectTableName = value;
                if (this.EntityObjectTableName == "Shipment") {
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
    TicketDetailsTabComponent.prototype.ChooseEntity = function () {
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
            logWindow.Show('./CommonModules/CommonFilingInbox/FilingInbox/ChooseEntityComponent');
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
    TicketDetailsTabComponent.prototype.AddButtonClicked = function () {
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
    TicketDetailsTabComponent.prototype.ViewShipmentClicked = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.ShipmentId, ObjectTableName: 'Shipment', BackButtonLabel: "Tickets" });
        });
    };
    TicketDetailsTabComponent.prototype.ViewQuoteClicked = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.QuoteId, ObjectTableName: 'Quote', BackButtonLabel: "Tickets" });
        });
    };
    TicketDetailsTabComponent.prototype.DeleteEntity = function () {
        if (this.IsTicketEditEnabled) {
            this.ShipmentId = null;
            this.QuoteId = null;
            this.ShipmentNumber = null;
            this.QuoteNumber = null;
            this.EntityLinkNumberVisibility = false;
        }
    };
    TicketDetailsTabComponent.prototype.AddContact = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Contact";
        var args = new ContactInputTemplate_1.ContactInputTemplateArgs();
        args.CustomerId = this.CompanyId;
        args.CardDependencyProperty1 = "AG,AL,CC,CG,CO,CS,FL,OT,SG,SL,TR,VD,WH";
        args.CustomerLable = "Company";
        args.CardDependencyProperty1IsList = true;
        args.ComponentName = "Ticket";
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewTicketWindowClosed($event); });
    };
    TicketDetailsTabComponent.prototype.OnNewTicketWindowClosed = function (arg) {
        if (arg != 'cancel') {
            if (this.InternalMode) {
                this.EntityPM.CustomerContactId = arg;
            }
            else {
                this.EntityPM.ContactId = arg;
            }
        }
    };
    TicketDetailsTabComponent.prototype.ConectContactClicked = function () {
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
    TicketDetailsTabComponent.prototype.AddCompanyClicked = function () {
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
    TicketDetailsTabComponent.prototype.EditContactClicked = function () {
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
    TicketDetailsTabComponent.prototype.OnEditContactWindowClosed = function (arg) {
        if (arg != 'cancel') {
            if (this.InternalMode) {
                this.CustomerContactId = arg;
            }
            else {
                this.ContactId = arg;
            }
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], TicketDetailsTabComponent.prototype, "viewContainerRef", void 0);
    TicketDetailsTabComponent = __decorate([
        core_1.Component({
            selector: 'DetailsTabComponent',
            moduleId: module.id,
            templateUrl: './TicketDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TicketDetailsTabComponent);
    return TicketDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TicketDetailsTabComponent = TicketDetailsTabComponent;
var EntityClass = /** @class */ (function () {
    function EntityClass() {
    }
    return EntityClass;
}());
exports.EntityClass = EntityClass;
//# sourceMappingURL=TicketDetailsTabComponent.js.map