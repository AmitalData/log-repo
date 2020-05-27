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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var ObjectTableRulePMService_1 = require("../../../../../Infrastructure/Services/StandardPMs/ObjectTableRulePMService");
var ObjectTableRuleFieldPMService_1 = require("../../../../../Infrastructure/Services/StandardPMs/ObjectTableRuleFieldPMService");
var ObjectTableRulePM_1 = require("../../../../../Infrastructure/EntityPMs/ObjectTableRulePM");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var RulesMainComponent = /** @class */ (function () {
    function RulesMainComponent() {
        //        window.ObjectTableRules = [];
        this._objectTableRuleFieldPMService = new ObjectTableRuleFieldPMService_1.ObjectTableRuleFieldPMService();
        this._objectTableRulePMService = new ObjectTableRulePMService_1.ObjectTableRulePMService();
        this.IsResourcesReady = false;
        this.TableRulesItems = [];
        this.AllTableRules = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.entityResourceService = new EntityResourceService_1.EntityResourceService();
    }
    RulesMainComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.ObjectTableId = windowArgs.ObjectTableID;
        //this.AllTableRules = window.ObjectTableRules.filter(r => r.ObjectTableId === this.ObjectTableId && r.Internal === false);
        //this.TableRulesItems = this.AllTableRules;
        this.IsResourcesReady = true;
        this.LoadRules();
    };
    RulesMainComponent.prototype.LoadRules = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Loading"));
        this._objectTableRulePMService.getAllByTenant(SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            if (!response.HasError && response.Result) {
                window.ObjectTableRules = response.Result;
                _this.AllTableRules = response.Result.filter(function (r) { return r.ObjectTableId === _this.ObjectTableId && r.Internal === false; });
                _this.TableRulesItems = _this.AllTableRules;
            }
            _this._objectTableRuleFieldPMService.getAllByTenant(SessionLocator_1.SessionLocator.Tenant).subscribe(function (response2) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response2.HasError && response2.Result) {
                    window.ObjectTableRuleFields = response2.Result;
                }
                _this.IsResourcesReady = true;
            });
        });
    };
    RulesMainComponent.prototype.SearchTextChanged = function (searchText) {
        this.TableRulesItems = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(searchText)) {
            this.TableRulesItems = this.AllTableRules.filter(function (f) {
                return f.RuleCode.toLowerCase().indexOf(searchText.toLowerCase()) > -1
                    || f.Name != null && f.Name.toLowerCase().indexOf(searchText.toLowerCase()) > -1
                    || f.RuleTypeCode != null && f.RuleTypeCode.toLowerCase().indexOf(searchText.toLowerCase()) > -1
                    || f.RuleTypeName != null && f.RuleTypeName.toLowerCase().indexOf(searchText.toLowerCase()) > -1;
            });
        }
        else {
            this.TableRulesItems = this.AllTableRules;
        }
    };
    RulesMainComponent.prototype.OnEditRule = function (item) {
        var _this = this;
        //RulesMainComponent
        if (item) {
            var ObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
            this.entityResourceService.getEntityResourceByTableName(ObjectTable.Name, 0).subscribe(function (response) {
                var windowArgs = {};
                windowArgs.ObjectTableId = _this.ObjectTableId;
                windowArgs.EntityPM = item;
                windowArgs.IsNewEntity = false;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "Edit Rule";
                logWindow.IsFillScreen_115 = true;
                //logWindow.IsFillScreen_90
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/AddEditRuleComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                    if ($event === 'saved') {
                        _this.LoadRules();
                    }
                });
            });
        }
    };
    RulesMainComponent.prototype.OnAddRule = function () {
        var _this = this;
        var ObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
        this.entityResourceService.getEntityResourceByTableName(ObjectTable.Name, 0).subscribe(function (response) {
            var windowArgs = {};
            windowArgs.ObjectTableId = _this.ObjectTableId;
            windowArgs.EntityPM = new ObjectTableRulePM_1.ObjectTableRulePM();
            windowArgs.IsNewEntity = true;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = "New Rule";
            logWindow.IsFillScreen_115 = true;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/AddEditRuleComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event === 'saved') {
                    _this.LoadRules();
                }
            });
        });
    };
    RulesMainComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    RulesMainComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './RulesMainComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], RulesMainComponent);
    return RulesMainComponent;
}());
exports.RulesMainComponent = RulesMainComponent;
//# sourceMappingURL=RulesMainComponent.js.map