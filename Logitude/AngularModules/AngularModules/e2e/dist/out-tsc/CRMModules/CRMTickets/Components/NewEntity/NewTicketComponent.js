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
var TicketPM_1 = require("../../../../CRM/EntityPMs/TicketPM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var CRMDomainService_1 = require("../../../../CRM/Services/CRMDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var TicketClassificationListService_1 = require("../../../../CRM/Services/StandardLists/TicketClassificationListService");
var ContactInputTemplate_1 = require("../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var TicketPMInitService_1 = require("../../../../CRM/EntityPMInitServices/TicketPMInitService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var NewTicketComponent = /** @class */ (function (_super) {
    __extends(NewTicketComponent, _super);
    function NewTicketComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.OnCloseWindow = new core_1.EventEmitter();
        _this.ObjectTableName = "Ticket";
        _this.DataContext = _this;
        _this.EntityPM = new TicketPM_1.TicketPM();
        _this.ScreenCode = "Ticket.AdditionalFields";
        _this.imgNgStyle = null;
        _this.IsVisible = false;
        _this.QuickSearchItems = [];
        _this.Filters = null;
        _this.EntityList = [];
        _this.EntityNumberTitle = "Shipment Number";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.isDisabled = false;
        _this.isCardFinished = false;
        _this.isShipmentFinished = false;
        _this.SupportNotes = null;
        _this.FirstClassificationId = "";
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.Filters = new ApiQueryFilters_1.ApiQueryFilters();
        _this.Filters.PageIndex = 0;
        _this.Filters.PageSize = 10;
        if (!Tools_1.AppTool.IsNullOrEmpty(_this.CompanyId)) {
            _this.Filters.addAdditionalFilter("CustomerId", _this.CompanyId, null, null, "Equals", false, false, false, "string");
        }
        // this.RunComponent();
        _this.getGeneralClassification();
        return _this;
    }
    ;
    NewTicketComponent.prototype.ngOnInit = function () {
        //this.SetFieldsEnabled();
        this.CreateTicket();
        this.SetUIProperties();
        this.SetUIRequiredProperties();
        this.SetCustomerContactValue();
    };
    NewTicketComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewTicketComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewTicketComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        this.SetUIProperties();
        this.SetUIRequiredProperties();
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, _this.ScreenCode);
        });
    };
    NewTicketComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
            _this.WindowArgs = args;
        });
    };
    Object.defineProperty(NewTicketComponent.prototype, "IsShipmentIdDisabled", {
        get: function () {
            return this.isDisabled;
        },
        set: function (value) {
            this.isDisabled = value;
        },
        enumerable: true,
        configurable: true
    });
    NewTicketComponent.prototype.CreateTicket = function () {
        var _this = this;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new TicketPM_1.TicketPM();
        if (SessionLocator_1.SessionLocator.TenantPM.IsInternalTicketByDefault == true) {
            this.EntityPM.InternalMode = true;
        }
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByContactId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.EntityPM.IsCancelled = false;
        this.EntityPM.IsClosed = false;
        this.EntityPM.EntityType = window.ObjectTables.filter(function (d) { return d.Name === "Shipment"; })[0].Id;
        if (this.WindowArgs != null) {
            this.CompanyId = this.WindowArgs.CompanyId;
        }
        if (SessionLocator_1.SessionLocator.IsExternalParams) {
            if (SessionLocator_1.SessionLocator.ExternalParams) {
                if (SessionLocator_1.SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "tickets") {
                    if (SessionLocator_1.SessionLocator.ExternalParams.Action.toLocaleLowerCase() == "new") {
                        var cardVisible = false, shipmentVisible = false;
                        SessionLocator_1.SessionLocator.ExternalParams.Args.forEach(function (arg) {
                            _this.EntityPM[arg.FieldName] = arg.FieldValue;
                            if (arg.FieldName.toLocaleLowerCase() == "companycode") {
                                if (arg.FieldValue) {
                                    _this.GetCardForCode(arg.FieldValue);
                                    cardVisible = true;
                                }
                                else {
                                    //cardVisible = true;
                                    _this.isCardFinished = true;
                                }
                            }
                            if (arg.FieldName.toLocaleLowerCase() == "shipmentid") {
                                if (!Tools_1.AppTool.IsNullOrEmpty(arg.FieldValue)) {
                                    _this.GetShipmentById(arg.FieldValue);
                                    shipmentVisible = true;
                                }
                                else {
                                    //shipmentVisible = true;
                                    _this.isShipmentFinished = true;
                                }
                            }
                            _this.UIProperties.SetEnabled(arg.FieldName, _this.ObjectTableName, false);
                        });
                        if (!cardVisible || !shipmentVisible) {
                            this.IsVisible = true;
                        }
                        SessionLocator_1.SessionLocator.ClearExternalParams();
                    }
                }
            }
        }
        else {
            this.IsVisible = true;
        }
        TicketPMInitService_1.TicketPMInitService.InitValues(this.EntityPM, true);
    };
    NewTicketComponent.prototype.GetCardForCode = function (code) {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        var myService = new CardListService_1.CardListService();
        if (!Tools_1.AppTool.IsNullOrEmpty(code)) {
            filters.addAdditionalFilter("Code", code, null, null, "Equals", false, false, false, "string");
        }
        myService.getByFilters(filters).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var cards = myResponse.Result;
                if (cards != null && cards.length > 0) {
                    var card = cards[0];
                    _this.CompanyId = card.Id;
                    _this.UIProperties.SetEnabled("CompanyId", _this.ObjectTableName, false);
                }
                _this.isCardFinished = true;
            }
            else {
                _this.isCardFinished = true;
            }
            _this.SetIsVisible();
        });
    };
    NewTicketComponent.prototype.GetShipmentById = function (id) {
        var _this = this;
        this.ShipmentNumber = null;
        this.ShipmentId = null;
        //var service = new ShipmentDomainService();
        var service = new ShipmentPMService_1.ShipmentPMService();
        service.get(id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var shipment = myResponse.Result;
                if (shipment != null) {
                    _this.IsShipmentIdDisabled = true;
                    _this.ShipmentNumber = shipment.ShipmentNumber;
                    _this.ShipmentId = shipment.Id;
                }
                _this.isShipmentFinished = true;
            }
            else {
                _this.isShipmentFinished = true;
            }
            _this.SetIsVisible();
        });
    };
    NewTicketComponent.prototype.SetIsVisible = function () {
        if (this.isCardFinished && this.isShipmentFinished) {
            this.IsVisible = true;
        }
    };
    NewTicketComponent.prototype.SetUIProperties = function () {
        var descriptionIsRequired = Tools_1.AppTool.IsNullOrEmpty(this.TicketDescription) ? true : false;
        this.UIProperties.SetRequired("TicketDescription", this.ObjectTableName, descriptionIsRequired);
        if (this.InternalMode) {
            this.UIProperties.SetEnabled("CustomerContactId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
        }
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.MainClassificationId));
    };
    NewTicketComponent.prototype.SetUIRequiredProperties = function () {
        this.UIProperties.SetRequired("EmployeeGroupId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.EmployeeGroupId));
        this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OwnerId));
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
        this.UIProperties.SetRequired("ShipmentNumber", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
    };
    NewTicketComponent.prototype.SetFieldsEnabled = function () {
        this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.CompanyId));
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.MainClassificationId));
    };
    Object.defineProperty(NewTicketComponent.prototype, "IsAddContactEnabled", {
        // Properties 
        get: function () {
            return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CompanyId) ? false : true;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "SupportNotesVisibility", {
        get: function () {
            var myResult = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SupportNotes)) {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "CompanyId", {
        get: function () { return this.EntityPM.CompanyId; },
        set: function (newValue) {
            if (this.EntityPM.CompanyId != newValue) {
                this.EntityPM.CompanyId = newValue;
                this.GetCompanyCardData();
                this.SetUIRequiredProperties();
                this.UpdateCompanyContact();
                this.SetUIProperties();
                this.SetCustomerContactValue();
                this.Filters = new ApiQueryFilters_1.ApiQueryFilters();
                if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.Filters.addAdditionalFilter("CustomerId", newValue, null, null, "Equals", false, false, false, "string");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTicketComponent.prototype.GetCompanyCardData = function () {
        var _this = this;
        var card = null;
        var myService = new CardListService_1.CardListService();
        myService.getSingle(this.CompanyId).subscribe(function (myResult) {
            card = myResult.Result;
            if (card != null) {
                _this.SupportNotes = card.SupportNotes;
            }
            else {
                _this.SupportNotes = null;
            }
        });
    };
    NewTicketComponent.prototype.UpdateCompanyContact = function () {
        var _this = this;
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
    };
    Object.defineProperty(NewTicketComponent.prototype, "ShipmentId", {
        get: function () { return this.EntityPM.ShipmentId; },
        set: function (newValue) {
            //if (this.EntityPM.ShipmentId != newValue) {
            this.EntityPM.ShipmentId = newValue;
            // }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "ShipmentNumber", {
        get: function () { return this.EntityPM.ShipmentNumber; },
        set: function (newValue) {
            if (this.EntityPM.ShipmentNumber != newValue) {
                this.EntityPM.ShipmentNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "EntityType", {
        get: function () { return this.EntityPM.EntityType; },
        set: function (newValue) {
            if (this.EntityPM.EntityType != newValue) {
                this.EntityPM.EntityType = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "QuoteId", {
        get: function () { return this.EntityPM.QuoteId; },
        set: function (newValue) {
            this.EntityPM.QuoteId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "QuoteNumber", {
        get: function () { return this.EntityPM.QuoteNumber; },
        set: function (newValue) {
            if (this.EntityPM.QuoteNumber != newValue) {
                this.EntityPM.QuoteNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "ContactId", {
        get: function () { return this.EntityPM.ContactId; },
        set: function (newValue) {
            if (this.EntityPM.ContactId != newValue) {
                this.EntityPM.ContactId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "Subject", {
        get: function () { return this.EntityPM.Subject; },
        set: function (newValue) {
            if (this.EntityPM.Subject != newValue) {
                this.EntityPM.Subject = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "TicketDescription", {
        get: function () { return this.EntityPM.TicketDescription; },
        set: function (newValue) {
            if (this.EntityPM.TicketDescription != newValue) {
                this.EntityPM.TicketDescription = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "ClosureDescription", {
        get: function () { return this.EntityPM.ClosureDescription; },
        set: function (newValue) {
            if (this.EntityPM.ClosureDescription != newValue) {
                this.EntityPM.ClosureDescription = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "MainClassificationId", {
        get: function () { return this.EntityPM.MainClassificationId; },
        set: function (newValue) {
            if (this.EntityPM.MainClassificationId != newValue) {
                this.EntityPM.MainClassificationId = newValue;
                this.getGeneralClassification();
                this.SecondaryClassificationId = null;
                this.SetUIProperties();
                this.setDeafaultsValues();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTicketComponent.prototype.setDeafaultsValues = function () {
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
    Object.defineProperty(NewTicketComponent.prototype, "SecondaryClassificationId", {
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
    NewTicketComponent.prototype.getGeneralClassification = function () {
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
    Object.defineProperty(NewTicketComponent.prototype, "SecondClassificationId", {
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
    Object.defineProperty(NewTicketComponent.prototype, "SeverityId", {
        get: function () { return this.EntityPM.SeverityId; },
        set: function (newValue) {
            if (this.EntityPM.SeverityId != newValue) {
                this.EntityPM.SeverityId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "TicketTypeId", {
        get: function () { return this.EntityPM.TicketTypeId; },
        set: function (newValue) {
            if (this.EntityPM.TicketTypeId != newValue) {
                this.EntityPM.TicketTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "EmployeeGroupId", {
        get: function () { return this.EntityPM.EmployeeGroupId; },
        set: function (newValue) {
            if (this.EntityPM.EmployeeGroupId != newValue) {
                this.EntityPM.EmployeeGroupId = newValue;
                this.CheckOwnerEmployeeGroup();
                this.SetUIRequiredProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTicketComponent.prototype.CheckOwnerEmployeeGroup = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetOwnerEmployeeGroup(this.OwnerId, this.EmployeeGroupId).subscribe(function (myResult) {
            _this.OwnerId = myResult;
        });
    };
    Object.defineProperty(NewTicketComponent.prototype, "OwnerId", {
        get: function () { return this.EntityPM.OwnerId; },
        set: function (newValue) {
            var _this = this;
            this.EntityPM.OwnerId = newValue;
            this.SetUIRequiredProperties();
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
    Object.defineProperty(NewTicketComponent.prototype, "CustomerContactId", {
        get: function () { return this.EntityPM.CustomerContactId; },
        set: function (value) {
            if (this.EntityPM.CustomerContactId != value) {
                this.EntityPM.CustomerContactId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTicketComponent.prototype, "InternalMode", {
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
    NewTicketComponent.prototype.SetCustomerContactValue = function () {
        this.UIProperties.SetVisibility("CustomerContactId", this.ObjectTableName, this.InternalMode);
        if (this.InternalMode) {
            this.ContactIdCustomFilter = "ContactIdCustomFilter";
            this.CustomerContactId = this.ContactId;
            this.ContactId = null;
            this.CompanyIdCustomFilter = null;
        }
        else {
            this.ContactId = this.CustomerContactId;
            this.CustomerContactId = null;
            this.CompanyIdCustomFilter = this.CompanyId;
            this.ContactIdCustomFilter = null;
        }
        this.SetUIProperties();
    };
    // Commands
    NewTicketComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewTicketComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
            errors.push("Owner is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EmployeeGroupId)) {
            errors.push("Employee Group field is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CompanyId)) {
            errors.push("Company field is required");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitCreatingTicket();
        }
    };
    NewTicketComponent.prototype.SubmitCreatingTicket = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Creating...");
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.InserNewTicket(this.EntityPM).subscribe(function (myRespone) {
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    if (_this.WindowArgs != null) {
                        // Ticket Number
                        _this.WindowArgs.TicketNumber = _this.EntityPM.TicketNumber;
                    }
                    _this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
                else {
                    _this.ValidationErrorsList = myRespone.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    NewTicketComponent.prototype.AddContact = function () {
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
    NewTicketComponent.prototype.OnNewTicketWindowClosed = function (arg) {
        if (arg != 'cancel') {
            if (this.InternalMode) {
                this.EntityPM.CustomerContactId = arg;
            }
            else {
                this.EntityPM.ContactId = arg;
            }
        }
    };
    NewTicketComponent.prototype.QuickSearchTextChanged = function (entity) {
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
    NewTicketComponent.prototype.ChooseShipment = function () {
        var _this = this;
        if (!this.IsShipmentIdDisabled) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 800;
            logWindow.Height = 570;
            logWindow.Title = "Shipments Search";
            logWindow.WindowArgs = this.EntityPM;
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
    Object.defineProperty(NewTicketComponent.prototype, "EntityObjectTableName", {
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
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTicketComponent.prototype.ChooseEntity = function () {
        var _this = this;
        if (!this.IsShipmentIdDisabled && this.EntityType != null) {
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
    NewTicketComponent.prototype.AddButtonClicked = function () {
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
                    if (s) {
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
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], NewTicketComponent.prototype, "OnCloseWindow", void 0);
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewTicketComponent.prototype, "viewContainerRef", void 0);
    NewTicketComponent = __decorate([
        core_1.Component({
            selector: 'NewTicketComponent',
            moduleId: module.id,
            templateUrl: './NewTicketComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewTicketComponent);
    return NewTicketComponent;
}(BaseComponent_1.BaseComponent));
exports.NewTicketComponent = NewTicketComponent;
var EntityClass = /** @class */ (function () {
    function EntityClass() {
    }
    return EntityClass;
}());
exports.EntityClass = EntityClass;
//# sourceMappingURL=NewTicketComponent.js.map