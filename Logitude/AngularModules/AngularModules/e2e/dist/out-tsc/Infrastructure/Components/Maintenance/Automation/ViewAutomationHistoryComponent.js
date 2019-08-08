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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var AutomationConditionViewModel_1 = require("./ViewModel/AutomationConditionViewModel");
var AutomationHistoryExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/AutomationHistoryExtendedPMService");
var ViewAutomationHistoryComponent = /** @class */ (function (_super) {
    __extends(ViewAutomationHistoryComponent, _super);
    function ViewAutomationHistoryComponent(_automationHistoryExtendedPMService) {
        var _this = _super.call(this) || this;
        _this._automationHistoryExtendedPMService = _automationHistoryExtendedPMService;
        _this.DataContext = _this;
        _this.AutomationCondationOrList = [];
        _this.AutomationCondationAndList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    ViewAutomationHistoryComponent.prototype.ngOnInit = function () {
    };
    ViewAutomationHistoryComponent.prototype.SetWindowArgs = function (args) {
        this.CurrentEntityPM = args.AutomationHistoryPM;
        this.DataViewModel = args.DataViewModel;
        if (this.CurrentEntityPM) {
            if (this.CurrentEntityPM.AutomatedDataBackup) {
                this.BuildAutomationCondition();
            }
            else {
                this.LoadAutomatedDataBackup();
            }
        }
    };
    ViewAutomationHistoryComponent.prototype.LoadAutomatedDataBackup = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this._automationHistoryExtendedPMService.getAutomationBackupDataByAutomationId(this.CurrentEntityPM.AutomationsId, this.CurrentEntityPM.Version, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                _this.CurrentEntityPM.AutomatedDataBackup = myResult;
                _this.BuildAutomationCondition();
                _this.DataViewModel.UpdateCurrentAutomationHository(_this.CurrentEntityPM);
            }
        });
    };
    ViewAutomationHistoryComponent.prototype.BuildAutomationCondition = function () {
        var _this = this;
        if (this.CurrentEntityPM.AutomatedDataBackup && this.CurrentEntityPM.AutomatedDataBackup.AautomationConditionLists) {
            this.IsViewCondition = false;
            var automationConditionPMList = this.CurrentEntityPM.AutomatedDataBackup.AautomationConditionLists;
            if (automationConditionPMList != null) {
                automationConditionPMList.forEach(function (item) {
                    if (item.ConditionType == "And") {
                        _this.AutomationCondationAndList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(item, _this.DataViewModel, _this));
                    }
                    else if (item.ConditionType == "Or") {
                        _this.AutomationCondationOrList.push(new AutomationConditionViewModel_1.AutomationConditionViewModel(item, _this.DataViewModel, _this));
                    }
                });
            }
        }
    };
    ViewAutomationHistoryComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ViewAutomationHistoryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ViewAutomationHistoryComponent',
            templateUrl: './ViewAutomationHistoryComponent.html',
            providers: [AutomationHistoryExtendedPMService_1.AutomationHistoryExtendedPMService],
        }),
        __metadata("design:paramtypes", [AutomationHistoryExtendedPMService_1.AutomationHistoryExtendedPMService])
    ], ViewAutomationHistoryComponent);
    return ViewAutomationHistoryComponent;
}(BaseComponent_1.BaseComponent));
exports.ViewAutomationHistoryComponent = ViewAutomationHistoryComponent;
//# sourceMappingURL=ViewAutomationHistoryComponent.js.map