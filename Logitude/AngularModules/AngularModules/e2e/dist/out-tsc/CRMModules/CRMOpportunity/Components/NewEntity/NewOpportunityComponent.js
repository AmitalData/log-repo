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
var OpportunityPM_1 = require("../../../../CRM/EntityPMs/OpportunityPM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var OpportunityPMService_1 = require("../../../../CRM/Services/StandardPMs/OpportunityPMService");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var OpportunityTypeListService_1 = require("../../../../CRM/Services/StandardLists/OpportunityTypeListService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var OpportunityPMInitService_1 = require("../../../../CRM/EntityPMInitServices/OpportunityPMInitService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../../Infrastructure/Args");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var NewOpportunityComponent = /** @class */ (function (_super) {
    __extends(NewOpportunityComponent, _super);
    function NewOpportunityComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.ObjectTableName = "Opportunity";
        _this.DataContext = _this;
        _this.EntityPM = new OpportunityPM_1.OpportunityPM();
        _this.SalesNotesObsList = [];
        _this.ValidationErrorsList = [];
        _this.ScreenCode = "Opportunity.AdditionalFields";
        _this.addCustomerVisibility = true;
        _this.IsNew = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AgentIdVisibility = false;
        _this.ForeignClientIdVisibility = false;
        _this.LeadUserIdVisibility = false;
        _this.LeadSourceIdVisibility = false;
        _this.LeadPartnerIdVisibility = false;
        _this.LeadDescriptionVisibility = false;
        _this.Retries = 0;
        _this._entityResourceService.getEntityResourceByTableName("Card", 0).subscribe(function (response) {
        });
        _this.RunComponentTimer();
        return _this;
    }
    Object.defineProperty(NewOpportunityComponent.prototype, "AddCustomerVisibility", {
        get: function () { return this.addCustomerVisibility; },
        set: function (value) { this.addCustomerVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "Subject", {
        get: function () { return this.EntityPM.Subject; },
        set: function (value) { if (this.EntityPM.Subject != value)
            this.EntityPM.Subject = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "OwnerId", {
        get: function () {
            if (this.EntityPM.OwnerId != null)
                return this.EntityPM.OwnerId;
        },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.OwnerId != value) {
                this.EntityPM.OwnerId = value;
                if (value == null)
                    this.EntityPM.BusinessUnitId = null;
                else {
                    var listService = new UserListService_1.UserListService();
                    listService.getAllFromCache().subscribe(function (result) {
                        var list = result.Result.filter(function (p) { return p.Id == value; })[0];
                        if (list != null)
                            _this.EntityPM.BusinessUnitId = list.BusinessUnitId;
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewOpportunityComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewOpportunityComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        this.myService = new OpportunityPMService_1.OpportunityPMService();
        this.myService.insert(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewOpportunityComponent.prototype.AddCustomerWindow = function () {
        var _this = this;
        var windowTitle = "New Potential Customer";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = new Args_1.NewEntityArgs();
        logWindow.Width = 960;
        logWindow.Height = 580;
        logWindow.Title = windowTitle;
        this._entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(function (response) {
            logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent');
            logWindow.ComponentLoaded.subscribe(function (s) {
                logWindow.WindowClosed.subscribe(function (d) {
                    if (s.EntityPM != null)
                        if (s.EntityPM.Id != null) {
                            _this.EntityPM.CustomerId = s.EntityPM.Id;
                            _this.ContactId = s.EntityPM.PrimaryContactId;
                            _this.GetCustomerSalesNotes();
                            _this.UIProperties.SetEnabled("ContactId", _this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.CustomerId));
                        }
                });
            });
        });
    };
    NewOpportunityComponent.prototype.GetCustomerSalesNotes = function () {
        var _this = this;
        this.SalesNotesObsList = [];
        var domainService = new PartnersDomainService_1.PartnersDomainService();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId))
            domainService.GetCustomerSalesNotes(this.CustomerId).subscribe(function (result) {
                _this.SalesNotesObsList = result.sort(function (a, b) { return (a.UpdateDate === b.UpdateDate) ? 0 : (a.UpdateDate < b.UpdateDate) ? -1 : 1; });
                console.log(_this.SalesNotesObsList);
            });
    };
    Object.defineProperty(NewOpportunityComponent.prototype, "HasSalesNotes", {
        get: function () { return this.SalesNotesObsList.length > 0; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (value) {
            if (this.EntityPM.CustomerId != value) {
                this.EntityPM.CustomerId = value;
                this.UpdateCustomerContact();
                this.GetCustomerSalesNotes();
                this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(value));
            }
        },
        enumerable: true,
        configurable: true
    });
    NewOpportunityComponent.prototype.UpdateCustomerContact = function () {
        var _this = this;
        var myContactId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
            var cardService = new CardListService_1.CardListService();
            cardService.getAll().subscribe(function (result) {
                var list = result.Result.filter(function (p) { return p.Id == _this.CustomerId; })[0];
                if (list != null) {
                    myContactId = list.PrimaryContactId;
                }
                _this.ContactId = myContactId;
            });
        }
    };
    Object.defineProperty(NewOpportunityComponent.prototype, "ContactId", {
        get: function () { return this.EntityPM.ContactId; },
        set: function (value) { if (this.EntityPM.ContactId != value)
            this.EntityPM.ContactId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "RatingCode", {
        get: function () { return this.EntityPM.RatingCode; },
        set: function (value) { if (this.EntityPM.RatingCode != value)
            this.EntityPM.RatingCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "NumberOfShipments", {
        get: function () { return this.EntityPM.NumberOfShipments; },
        set: function (value) { if (this.EntityPM.NumberOfShipments != value)
            this.EntityPM.NumberOfShipments = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "EstimatedClosingDate", {
        get: function () { return this.EntityPM.EstimatedClosingDate; },
        set: function (value) { if (this.EntityPM.EstimatedClosingDate != value)
            this.EntityPM.EstimatedClosingDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) { if (this.EntityPM.Notes != value)
            this.EntityPM.Notes = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "OpportunityTypeId", {
        get: function () { return this.EntityPM.OpportunityTypeId; },
        set: function (value) {
            if (this.EntityPM.OpportunityTypeId != value) {
                this.OnOpportunityTypeChanging(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    NewOpportunityComponent.prototype.OnOpportunityTypeChanging = function (newValue) {
        var _this = this;
        var isConfirmNeeded = false;
        this.newOpportunityTypeId = newValue;
        var oppTypeListService = new OpportunityTypeListService_1.OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe(function (result) {
            var typeList = result.Result.filter(function (d) { return d.Id == newValue; })[0];
            if (typeList != null) {
                _this.newOpportunityTypeCode = typeList.Code;
            }
            if (_this.newOpportunityTypeCode == "T") {
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.LeadUserId) || !Tools_1.AppTool.IsNullOrEmpty(_this.LeadSourceId) || !Tools_1.AppTool.IsNullOrEmpty(_this.LeadPartnerId) || !Tools_1.AppTool.IsNullOrEmpty(_this.LeadDescription)) {
                    isConfirmNeeded = true;
                }
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.AgentId) || !Tools_1.AppTool.IsNullOrEmpty(_this.ForeignClientId)) {
                    isConfirmNeeded = true;
                }
            }
            if (isConfirmNeeded) {
                if (_this.confirm != null) {
                    _this.confirm.Close();
                }
                _this.confirm = new ConfirmWindow_1.ConfirmWindow();
                _this.confirm.WindowClosed.subscribe(function (event) { _this.confirm_Unloaded(event); });
                _this.confirm.ShowCancelButton = false;
                if (_this.newOpportunityTypeCode == "T") {
                    _this.confirm.Show("Lead Source details will be lost");
                }
                else {
                    _this.confirm.Show("Routing Order details will be lost");
                }
            }
            else {
                _this.EntityPM.OpportunityTypeId = newValue;
                _this.SetUIProperties();
                _this.SetSubject();
            }
        });
    };
    NewOpportunityComponent.prototype.confirm_Unloaded = function (event) {
        if (this.confirm.Yes) {
            this.EntityPM.OpportunityTypeId = this.newOpportunityTypeId;
            this.SetSubject();
            if (this.newOpportunityTypeCode == "T") {
                this.LeadUserId = null;
                this.LeadSourceId = null;
                this.LeadPartnerId = null;
                this.LeadDescription = null;
            }
            else {
                this.AgentId = null;
                this.ForeignClientId = null;
            }
        }
        this.SetUIProperties();
    };
    NewOpportunityComponent.prototype.SetSubject = function () {
        var _this = this;
        var oppTypeListService = new OpportunityTypeListService_1.OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe(function (result) {
            var type = result.Result.filter(function (d) { return d.Id == _this.OpportunityTypeId; })[0];
            if (type != null) {
                _this.Subject = type.Name;
            }
        });
    };
    NewOpportunityComponent.prototype.SetUIProperties = function () {
        var _this = this;
        var oppTypeListService = new OpportunityTypeListService_1.OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe(function (result) {
            var typeList = result.Result.filter(function (d) { return d.Id == _this.EntityPM.OpportunityTypeId; })[0];
            var typeCode = typeList == null ? null : typeList.Code;
            if (typeCode == "T") {
                _this.UIProperties.SetVisibility("AgentId", "Opportunity", true);
                _this.AgentIdVisibility = true;
                _this.UIProperties.SetVisibility("ForeignClientId", "Opportunity", true);
                _this.ForeignClientIdVisibility = true;
                _this.UIProperties.SetVisibility("LeadUserId", "Opportunity", false);
                _this.LeadUserIdVisibility = false;
                _this.UIProperties.SetVisibility("LeadSourceId", "Opportunity", false);
                _this.LeadSourceIdVisibility = false;
                _this.UIProperties.SetVisibility("LeadPartnerId", "Opportunity", false);
                _this.LeadPartnerIdVisibility = false;
                _this.UIProperties.SetVisibility("LeadDescription", "Opportunity", false);
                _this.LeadDescriptionVisibility = false;
            }
            else {
                _this.UIProperties.SetVisibility("AgentId", "Opportunity", false);
                _this.AgentIdVisibility = false;
                _this.UIProperties.SetVisibility("ForeignClientId", "Opportunity", false);
                _this.ForeignClientIdVisibility = false;
                _this.UIProperties.SetVisibility("LeadUserId", "Opportunity", true);
                _this.LeadUserIdVisibility = true;
                _this.UIProperties.SetVisibility("LeadSourceId", "Opportunity", true);
                _this.LeadSourceIdVisibility = true;
                _this.UIProperties.SetVisibility("LeadPartnerId", "Opportunity", true);
                _this.LeadPartnerIdVisibility = true;
                _this.UIProperties.SetVisibility("LeadDescription", "Opportunity", true);
                _this.LeadDescriptionVisibility = true;
            }
            _this.UIProperties.SetEnabled("ContactId", "Opportunity", !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.CustomerId));
        });
    };
    Object.defineProperty(NewOpportunityComponent.prototype, "LeadSourceId", {
        get: function () { return this.EntityPM.LeadSourceId; },
        set: function (value) { if (this.EntityPM.LeadSourceId != value)
            this.EntityPM.LeadSourceId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "LeadDescription", {
        get: function () { return this.EntityPM.LeadDescription; },
        set: function (value) { if (this.EntityPM.LeadDescription != value)
            this.EntityPM.LeadDescription = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "LeadUserId", {
        get: function () { return this.EntityPM.LeadUserId; },
        set: function (value) { if (this.EntityPM.LeadUserId != value)
            this.EntityPM.LeadUserId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "LeadPartnerId", {
        get: function () { return this.EntityPM.LeadPartnerId; },
        set: function (value) { if (this.EntityPM.LeadPartnerId != value)
            this.EntityPM.LeadPartnerId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "AgentId", {
        get: function () { return this.EntityPM.AgentId; },
        set: function (value) { if (this.EntityPM.AgentId != value)
            this.EntityPM.AgentId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewOpportunityComponent.prototype, "ForeignClientId", {
        get: function () { return this.EntityPM.ForeignClientId; },
        set: function (value) { if (this.EntityPM.ForeignClientId != value)
            this.EntityPM.ForeignClientId = value; },
        enumerable: true,
        configurable: true
    });
    NewOpportunityComponent.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.EntityPM = args.Entity;
            this.IsNew = args.IsNew;
            this.addCustomerVisibility = args.IsAddCustomerVisible;
        }
    };
    NewOpportunityComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewOpportunityComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.SetUIProperties();
            this.AddCustomerVisibility = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CustomerId) ? true : false;
            OpportunityPMInitService_1.OpportunityPMInitService.InitValues(this.EntityPM, this.IsNew);
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewOpportunityComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, _this.ScreenCode);
        });
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewOpportunityComponent.prototype, "viewContainerRef", void 0);
    NewOpportunityComponent = __decorate([
        core_1.Component({
            selector: 'NewOpportunityComponent',
            moduleId: module.id,
            templateUrl: './NewOpportunityComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewOpportunityComponent);
    return NewOpportunityComponent;
}(BaseComponent_1.BaseComponent));
exports.NewOpportunityComponent = NewOpportunityComponent;
//# sourceMappingURL=NewOpportunityComponent.js.map