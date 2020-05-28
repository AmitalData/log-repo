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
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var OpportunityTypeListService_1 = require("../../../../CRM/Services/StandardLists/OpportunityTypeListService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var OpportunityPMInitService_1 = require("../../../../CRM/EntityPMInitServices/OpportunityPMInitService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ContactListService_1 = require("../../../../Common/Services/StandardLists/ContactListService");
var OpportunityGeneralTabComponent = /** @class */ (function (_super) {
    __extends(OpportunityGeneralTabComponent, _super);
    function OpportunityGeneralTabComponent(_entityResourceService, entityArgs) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Opportunity";
        _this.DataContext = _this;
        _this.EntityPM = new OpportunityPM_1.OpportunityPM();
        _this.SalesNotesObsList = [];
        _this.ValidationErrorsList = [];
        _this.ResetOpportunitiy = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ScreenCode = "Opportunity.AdditionalFields";
        _this.OtherFieldsChild = true;
        _this.isConfirmTestRequired = true;
        _this.AgentIdVisibility = false;
        _this.ForeignClientIdVisibility = false;
        _this.LeadUserIdVisibility = false;
        _this.LeadSourceIdVisibility = false;
        _this.LeadPartnerIdVisibility = false;
        _this.LeadDescriptionVisibility = false;
        _this.ContactIdVisibility = false;
        _this.Retries = 0;
        _this._entityResourceService.getEntityResourceByTableName("Card", 0).subscribe(function (response) {
        });
        _this.EntityPM = entityArgs.EntityPM;
        _this.Listen();
        _this.RunComponent();
        return _this;
    }
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "Subject", {
        get: function () { return this.EntityPM.Subject; },
        set: function (value) { if (this.EntityPM.Subject != value)
            this.EntityPM.Subject = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "OwnerId", {
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
                        if (list != null) {
                            _this.EntityPM.BusinessUnitId = list.BusinessUnitId;
                            _this.EntityPM.OwnerName = list.EnglishName;
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    OpportunityGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    OpportunityPMInitService_1.OpportunityPMInitService.InitValues(_this.EntityPM, false);
                    _this.SetFieldsEnabled();
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.SetFieldsEnabled();
                }
            });
        }
    };
    OpportunityGeneralTabComponent.prototype.SetFieldsEnabled = function () {
        var fieldIsEnabled = true;
        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
            fieldIsEnabled = false;
        }
        this.UIProperties.SetEnabled("Subject", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("OwnerId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("CustomerId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("ContactId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("RatingCode", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("EstimatedClosingDate", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("Description", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("OpportunityTypeId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("LeadSourceId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("LeadDescription", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("LeadUserId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("LeadPartnerId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("AgentId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("ForeignClientId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("Notes", "Opportunity", fieldIsEnabled);
        if (fieldIsEnabled) {
            this.UIProperties.SetEnabled("ContactId", "Opportunity", !Tools_1.AppTool.IsNullOrEmpty(this.CustomerId));
        }
        this.OtherFieldsChild = fieldIsEnabled;
        this.SetUIProperties_GeneratedComponent();
    };
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (value) {
            if (this.EntityPM.CustomerId != value) {
                this.EntityPM.CustomerId = value;
                this.UpdateCustomerContact();
                if (!this.EntityPM.IsClosed && !this.EntityPM.IsCancelled) {
                    this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(value));
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    OpportunityGeneralTabComponent.prototype.UpdateCustomerContact = function () {
        var _this = this;
        var myContactId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
            var cardService = new CardListService_1.CardListService();
            cardService.getAll().subscribe(function (result) {
                var list = result.Result.filter(function (p) { return p.Id == _this.CustomerId; })[0];
                if (list != null) {
                    myContactId = list.PrimaryContactId;
                    _this.ContactId = myContactId;
                }
            });
        }
    };
    OpportunityGeneralTabComponent.prototype.OnOpportunityTypeChanging = function (newValue) {
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
    OpportunityGeneralTabComponent.prototype.confirm_Unloaded = function (event) {
        var _this = this;
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
        else {
            setTimeout(function () { return _this.RejectChanges(); }, 10);
        }
        this.SetUIProperties();
    };
    OpportunityGeneralTabComponent.prototype.RejectChanges = function () {
        this.isConfirmTestRequired = false;
        var isEntityDirty = this.EntityPM.IsDirty;
        var oldId = this.EntityPM.OpportunityTypeId;
        this.OpportunityTypeId = null;
        this.OpportunityTypeId = oldId;
        this.EntityPM.IsDirty = isEntityDirty;
        this.isConfirmTestRequired = true;
    };
    OpportunityGeneralTabComponent.prototype.SetSubject = function () {
        var _this = this;
        var oppTypeListService = new OpportunityTypeListService_1.OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe(function (result) {
            var type = result.Result.filter(function (d) { return d.Name == _this.Subject; })[0];
            if (type != null) {
                _this.Subject = type.Name;
            }
        });
    };
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "ContactId", {
        get: function () { return this.EntityPM.ContactId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ContactId != value) {
                this.EntityPM.ContactId = value;
                var cardService = new ContactListService_1.ContactListService();
                cardService.getAll().subscribe(function (contactResult) {
                    var contact = contactResult.Result.filter(function (p) { return p.Id == _this.ContactId; })[0];
                    if (contact != null)
                        _this.EntityPM.ContactName = contact.EnglishName;
                    else
                        _this.EntityPM.ContactName = null;
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "EstimatedClosingDate", {
        get: function () { return this.EntityPM.EstimatedClosingDate; },
        set: function (value) { if (this.EntityPM.EstimatedClosingDate != value)
            this.EntityPM.EstimatedClosingDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "OpportunityTypeId", {
        get: function () { return this.EntityPM.OpportunityTypeId; },
        set: function (value) {
            if (this.EntityPM.OpportunityTypeId != value) {
                if (this.isConfirmTestRequired) {
                    this.OnOpportunityTypeChanging(value);
                }
                else {
                    this.EntityPM.OpportunityTypeId = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    OpportunityGeneralTabComponent.prototype.SetUIProperties = function () {
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
            // this.UIProperties.SetEnabled("ContactId", "Opportunity", !AppTool.IsNullOrEmpty(this.EntityPM.CustomerId));
            _this.ContactIdVisibility = !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.CustomerId);
        });
    };
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "LeadSourceId", {
        get: function () { return this.EntityPM.LeadSourceId; },
        set: function (value) { if (this.EntityPM.LeadSourceId != value)
            this.EntityPM.LeadSourceId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "LeadDescription", {
        get: function () { return this.EntityPM.LeadDescription; },
        set: function (value) { if (this.EntityPM.LeadDescription != value)
            this.EntityPM.LeadDescription = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "LeadUserId", {
        get: function () { return this.EntityPM.LeadUserId; },
        set: function (value) { if (this.EntityPM.LeadUserId != value)
            this.EntityPM.LeadUserId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "LeadPartnerId", {
        get: function () { return this.EntityPM.LeadPartnerId; },
        set: function (value) { if (this.EntityPM.LeadPartnerId != value)
            this.EntityPM.LeadPartnerId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "AgentId", {
        get: function () { return this.EntityPM.AgentId; },
        set: function (value) { if (this.EntityPM.AgentId != value)
            this.EntityPM.AgentId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityGeneralTabComponent.prototype, "ForeignClientId", {
        get: function () { return this.EntityPM.ForeignClientId; },
        set: function (value) { if (this.EntityPM.ForeignClientId != value)
            this.EntityPM.ForeignClientId = value; },
        enumerable: true,
        configurable: true
    });
    OpportunityGeneralTabComponent.prototype.SetClosingReasonFields = function () {
        this.UIProperties.SetEnabled("ClosingReasonId", "Opportunity", false);
        this.UIProperties.SetVisibility("ClosingReasonId", "Opportunity", false);
        if (this.EntityPM.IsClosed) {
            if (this.EntityPM.IsClosedLost) {
                this.UIProperties.SetVisibility("ClosingReasonId", "Opportunity", true);
            }
        }
    };
    OpportunityGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            _this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.LoadCompleted.subscribe(function (s) {
                _this.SetUIProperties_GeneratedComponent();
            });
            cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, _this.ScreenCode);
        });
    };
    OpportunityGeneralTabComponent.prototype.SetUIProperties_GeneratedComponent = function () {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.OtherFieldsChild);
        }
    };
    OpportunityGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    OpportunityGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    OpportunityGeneralTabComponent.prototype.ngOnInit = function () {
        this.SetFieldsEnabled();
        this.SetUIProperties();
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], OpportunityGeneralTabComponent.prototype, "viewContainerRef", void 0);
    OpportunityGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'OpportunityGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './OpportunityGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, EntityArgs_1.EntityArgs])
    ], OpportunityGeneralTabComponent);
    return OpportunityGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OpportunityGeneralTabComponent = OpportunityGeneralTabComponent;
//# sourceMappingURL=OpportunityGeneralTabComponent.js.map