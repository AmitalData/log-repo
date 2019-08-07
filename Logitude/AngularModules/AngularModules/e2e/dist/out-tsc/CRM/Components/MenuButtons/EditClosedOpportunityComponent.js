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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var OpportunityTypeListService_1 = require("../../Services/StandardLists/OpportunityTypeListService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var OpportunityPMService_1 = require("../../Services/StandardPMs/OpportunityPMService");
var EditClosedOpportunityComponent = /** @class */ (function (_super) {
    __extends(EditClosedOpportunityComponent, _super);
    function EditClosedOpportunityComponent() {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ObjectTableName = "Opportunity";
        _this.DataContext = _this;
        _this.AgentIdVisibility = false;
        _this.ForeignClientIdVisibility = false;
        _this.LeadUserIdVisibility = false;
        _this.LeadSourceIdVisibility = false;
        _this.LeadPartnerIdVisibility = false;
        _this.LeadDescriptionVisibility = false;
        _this.ContactIdVisibility = false;
        _this.ValidationErrorsList = [];
        _this.Retries = 0;
        return _this;
    }
    Object.defineProperty(EditClosedOpportunityComponent.prototype, "Subject", {
        get: function () { return this.EntityPM.Subject; },
        set: function (value) { if (this.EntityPM.Subject != value)
            this.EntityPM.Subject = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditClosedOpportunityComponent.prototype, "OpportunityTypeId", {
        get: function () { return this.EntityPM.OpportunityTypeId; },
        set: function (value) {
            if (this.EntityPM.OpportunityTypeId != value) {
                this.OnOpportunityTypeChanging(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditClosedOpportunityComponent.prototype, "LeadSourceId", {
        get: function () { return this.EntityPM.LeadSourceId; },
        set: function (value) { if (this.EntityPM.LeadSourceId != value)
            this.EntityPM.LeadSourceId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditClosedOpportunityComponent.prototype, "LeadDescription", {
        get: function () { return this.EntityPM.LeadDescription; },
        set: function (value) { if (this.EntityPM.LeadDescription != value)
            this.EntityPM.LeadDescription = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditClosedOpportunityComponent.prototype, "LeadUserId", {
        get: function () { return this.EntityPM.LeadUserId; },
        set: function (value) { if (this.EntityPM.LeadUserId != value)
            this.EntityPM.LeadUserId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditClosedOpportunityComponent.prototype, "LeadPartnerId", {
        get: function () { return this.EntityPM.LeadPartnerId; },
        set: function (value) { if (this.EntityPM.LeadPartnerId != value)
            this.EntityPM.LeadPartnerId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditClosedOpportunityComponent.prototype, "AgentId", {
        get: function () { return this.EntityPM.AgentId; },
        set: function (value) { if (this.EntityPM.AgentId != value)
            this.EntityPM.AgentId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditClosedOpportunityComponent.prototype, "ForeignClientId", {
        get: function () { return this.EntityPM.ForeignClientId; },
        set: function (value) { if (this.EntityPM.ForeignClientId != value)
            this.EntityPM.ForeignClientId = value; },
        enumerable: true,
        configurable: true
    });
    EditClosedOpportunityComponent.prototype.SetUIProperties = function () {
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
            _this.ContactIdVisibility = !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.CustomerId);
        });
    };
    EditClosedOpportunityComponent.prototype.CancelButtonClicked = function () {
        this.SetUIProperties_GeneratedComponent(false);
        this.CurrentSession.CloseCurrentWindowEmit("cancle");
    };
    EditClosedOpportunityComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        this.myService = new OpportunityPMService_1.OpportunityPMService();
        this.myService.update(this.EntityPM).subscribe(function (myResult) {
            var mm = myResult;
            if (!mm.HasError) {
                _this.SetUIProperties_GeneratedComponent(false);
                _this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.ValidationErrorsList = mm.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    EditClosedOpportunityComponent.prototype.OnOpportunityTypeChanging = function (newValue) {
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
            }
        });
    };
    EditClosedOpportunityComponent.prototype.confirm_Unloaded = function (event) {
        if (this.confirm.Yes) {
            this.EntityPM.OpportunityTypeId = this.newOpportunityTypeId;
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
    EditClosedOpportunityComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    EditClosedOpportunityComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.SetUIProperties();
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    EditClosedOpportunityComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            _this.GeneratedComponent = cmpRef.instance;
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, "Opportunity.AdditionalFields");
            _this.SetUIProperties_GeneratedComponent(true);
        });
    };
    EditClosedOpportunityComponent.prototype.SetUIProperties_GeneratedComponent = function (value) {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(value);
        }
    };
    EditClosedOpportunityComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args;
        //this.SetUIProperties();
    };
    EditClosedOpportunityComponent.prototype.ngOnInit = function () {
        this.RunComponent();
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], EditClosedOpportunityComponent.prototype, "viewContainerRef", void 0);
    EditClosedOpportunityComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditClosedOpportunityComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditClosedOpportunityComponent);
    return EditClosedOpportunityComponent;
}(BaseComponent_1.BaseComponent));
exports.EditClosedOpportunityComponent = EditClosedOpportunityComponent;
//# sourceMappingURL=EditClosedOpportunityComponent.js.map