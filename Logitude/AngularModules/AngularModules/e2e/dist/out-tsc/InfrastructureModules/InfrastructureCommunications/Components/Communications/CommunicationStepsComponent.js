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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CommunicationLogStepDataViewModel_1 = require("../CommunicationLog/ViewModel/CommunicationLogStepDataViewModel");
var CommunicationLogStepListService_1 = require("../../../../Common/Services/ExtendedLists/CommunicationLogStepListService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var CommunicationStepsComponent = /** @class */ (function (_super) {
    __extends(CommunicationStepsComponent, _super);
    function CommunicationStepsComponent(entityArgs, cd) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.ObjectTableName = "Customs.CommunicationLogStep";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.columns = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._showGrid = false;
        _this._communicationLogStepListService = new CommunicationLogStepListService_1.CommunicationLogStepListService();
        _this._CommunicationLogStepDataViewModelList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    CommunicationStepsComponent.prototype.ngOnInit = function () {
        ///this.BuildColumns();
        this.LoadCommunicationLogSteps();
    };
    CommunicationStepsComponent.prototype.SetTabArgs = function (args) {
        this.EntityPM = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
    };
    CommunicationStepsComponent.prototype.ViewLogMethod = function (item) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("CommunicationLogSteps.O.Log");
        logitudeWindow.WindowArgs = item.Log;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/LogFieldComponent');
    };
    CommunicationStepsComponent.prototype.ViewXMLClicked = function (item) {
        if (item.SecurityId) {
            DownloadManager_1.DownloadManager.DownloadPage("", item.SecurityId);
        }
        else {
            DownloadManager_1.DownloadManager.DownloadPage(item.DocumentId, null);
        }
        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(link);
        //    return;
        //}
    };
    CommunicationStepsComponent.prototype.RefreshButtonClicked = function () {
        this._CommunicationLogStepDataViewModelList.Clear();
        this.LoadCommunicationLogSteps();
    };
    CommunicationStepsComponent.prototype.LoadCommunicationLogSteps = function () {
        var _this = this;
        this._communicationLogStepListService.getCommunicationLogStepsListsByLogId(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    var communicationLogStepDataViewModelList = [];
                    result
                        //.sort(d => d.StepNumber)
                        .forEach(function (item) {
                        communicationLogStepDataViewModelList.push(new CommunicationLogStepDataViewModel_1.CommunicationLogStepDataViewModel(item));
                    });
                    _this._CommunicationLogStepDataViewModelList.InsertCollection(communicationLogStepDataViewModelList);
                    //this._CommunicationLogStepDataViewModelList.InsertCollection(result);
                    _this.cd.detectChanges();
                    _this._showGrid = true;
                }
                // this.CurrentSession.StopBusyIndicator();
            }
            else {
                // this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    CommunicationStepsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'communication-steps',
            templateUrl: './CommunicationStepsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], CommunicationStepsComponent);
    return CommunicationStepsComponent;
}(BaseComponent_1.BaseComponent));
exports.CommunicationStepsComponent = CommunicationStepsComponent;
//# sourceMappingURL=CommunicationStepsComponent.js.map