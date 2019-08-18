"use strict";
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
var EntityChangeExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/EntityChangeExtendedPMService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AuditAutomationTabComponent = /** @class */ (function () {
    function AuditAutomationTabComponent(_entityChangeExtendedPMService, cd) {
        this._entityChangeExtendedPMService = _entityChangeExtendedPMService;
        this.cd = cd;
        this.ObjectTableName = "";
        this.ObjectTableId = "";
        this.AutomationTypeFilterSelectedValue = "All";
        this.ConditionChangedSelectedValue = "All";
        this.IsConditionAll = true;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.AutomationChangedEvent = null;
        this.IsCustomerCareUser = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsCustomerCareUser = true;
        }
        this.Listen();
    }
    AuditAutomationTabComponent.prototype.Listen = function () {
        var _this = this;
        if (!this.AutomationChangedEvent) {
            this.AutomationChangedEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == _this.ObjectTableName) {
                    _this.LoadData();
                }
            });
        }
    };
    AuditAutomationTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.AutomationChangedEvent);
    };
    AuditAutomationTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
            if (table) {
                _this.ObjectTableId = table.Id;
                _this.LoadData();
            }
        });
        this.cd.detectChanges();
    };
    AuditAutomationTabComponent.prototype.ngAfterViewInit = function () {
    };
    AuditAutomationTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.EntityChangeLists = [];
        this.AutomationList = [];
        this.ChangeFieldsList = [];
        this._entityChangeExtendedPMService.getEntityChangePMsByEntityIdAndObjectTable(this.EntityId, this.ObjectTableId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            _this.IsConditionAll = true;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                _this.EntityChangeLists = myResult;
                _this.SelectedTabCode = "FCH";
                if (_this.EntityChangeLists && _this.EntityChangeLists.length > 0) {
                    _this.EntityChangeListSelected = _this.EntityChangeLists[0];
                    _this.LoadAutomationAndChangeFelids();
                }
            }
        });
    };
    AuditAutomationTabComponent.prototype.EntityChangeListsChangeSelected = function (item) {
        if (item != this.EntityChangeListSelected) {
            this.EntityChangeListSelected = item;
            this.LoadAutomationAndChangeFelids();
        }
    };
    AuditAutomationTabComponent.prototype.LoadAutomationAndChangeFelids = function () {
        var _this = this;
        this.AutomationList = [];
        this.ChangeFieldsList = [];
        this.CurrentSession.StartBusyIndicator("Loading...");
        this._entityChangeExtendedPMService.getEntityChangeAutomationsSummaryByEntityChangeId(this.EntityChangeListSelected.Id, this.ObjectTableName, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                var entityChangeAutomationsSummary = myResult;
                if (entityChangeAutomationsSummary) {
                    _this.ChangeFieldsList = entityChangeAutomationsSummary.ChangeFieldsList;
                    _this.AutomationList = _this.EntityAutomationList = entityChangeAutomationsSummary.EntityChangeAutomationList;
                }
            }
        });
    };
    AuditAutomationTabComponent.prototype.AutomationTypeChangedMethod = function (type) {
        this.AutomationTypeFilterSelectedValue = type;
        this.RefreshEntityAutomationList();
    };
    AuditAutomationTabComponent.prototype.ConditionChangedMethod = function (type) {
        this.ConditionChangedSelectedValue = type;
        this.IsConditionAll = false;
        this.IsCondition = false;
        switch (type) {
            case 'All':
                this.IsConditionAll = true;
                break;
            case 'true':
                this.IsCondition = true;
                break;
            case 'false':
                this.IsCondition = false;
                break;
        }
        this.RefreshEntityAutomationList();
    };
    AuditAutomationTabComponent.prototype.RefreshButtonClicked = function () {
        this.LoadData();
    };
    AuditAutomationTabComponent.prototype.RefreshEntityAutomationList = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.AutomationTypeFilterSelectedValue) || this.AutomationTypeFilterSelectedValue == "All") {
            this.EntityAutomationList = this.AutomationList;
        }
        else
            this.EntityAutomationList = this.AutomationList.filter(function (d) { return d.AutomationType == _this.AutomationTypeFilterSelectedValue; });
        if (!this.IsConditionAll) {
            this.EntityAutomationList = this.EntityAutomationList.filter(function (d) { return d.IsConditionTrue == _this.IsCondition; });
        }
    };
    AuditAutomationTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AuditAutomationTabComponent',
            templateUrl: './AuditAutomationTabComponent.html',
            inputs: ['ObjectTableName', 'EntityId'],
            providers: [EntityChangeExtendedPMService_1.EntityChangeExtendedPMService],
        }),
        __metadata("design:paramtypes", [EntityChangeExtendedPMService_1.EntityChangeExtendedPMService, core_1.ChangeDetectorRef])
    ], AuditAutomationTabComponent);
    return AuditAutomationTabComponent;
}());
exports.AuditAutomationTabComponent = AuditAutomationTabComponent;
//# sourceMappingURL=AuditAutomationTabComponent.js.map