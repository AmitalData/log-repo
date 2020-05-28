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
var CustomMessageWrapperComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ClientMessagesService_1 = require("../../../Customs/Services/WebServices/ClientMessagesService");
var ClientSearchRequestParams_1 = require("../../../Customs/DataContract/RequestParams/ClientSearchRequestParams");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var ImageParameter_1 = require("../../../Infrastructure/DataContracts/ImageParameter");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var RecallClientsForCutoms = /** @class */ (function (_super) {
    __extends(RecallClientsForCutoms, _super);
    function RecallClientsForCutoms() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Client";
        _this.UploadButtonIsEnabled = true;
        _this._ClientMessagesService = new ClientMessagesService_1.ClientMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        //#region Properties
        _this.message = "";
        return _this;
    }
    RecallClientsForCutoms.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    RecallClientsForCutoms.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new ClientSearchRequestParams_1.ClientSearchRequestParams();
        }
        //if (this.ResponseData) {
        //    if (this.ResponseData.TransactionsList) {
        //        this.TransactionsResultList.InsertCollection(this.ResponseData.TransactionsList);
        //    }
        //}
    };
    Object.defineProperty(RecallClientsForCutoms.prototype, "Message", {
        get: function () { return this.message; },
        set: function (value) {
            if (this.message != value) {
                this.message = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#region General Commands
    RecallClientsForCutoms.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    RecallClientsForCutoms.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        this.SendRecallMessageToServer();
    };
    RecallClientsForCutoms.prototype.ShowMessage = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Show(message);
    };
    RecallClientsForCutoms.prototype.SendRecallMessageToServer = function () {
        var _this = this;
        this.ProgressBarPercentText = "0%";
        this.filterImageParameter = new ImageParameter_1.ImageParameter();
        this.filterImageParameter.Key = Guid_1.Guid.newGuid();
        this.filterImageParameter.IsFirstTry = true;
        this.filterImageParameter.UploadMode = "Block";
        this.filterImageParameter.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var myCustomMessageProgressHelper = new CustomMessageProgressComponent_1.CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(this.filterImageParameter.Key, 5, true);
        this._ClientMessagesService.PutRecallClientsForCutomsRequest(this.filterImageParameter).subscribe(function (myServiceResponse) {
            console.log("[Send] Response/PutRecallClientsForCutomsRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;
            myCustomMessageProgressHelper.MessageArrived = true;
            _this.CurrentSession.StopBusyIndicator();
            if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
            }
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], RecallClientsForCutoms.prototype, "SuperCustomMessageWrapperComponent", void 0);
    RecallClientsForCutoms = __decorate([
        core_1.Component({
            selector: 'RecallClientsForCutoms',
            moduleId: module.id,
            templateUrl: './RecallClientsForCutoms.html',
        }),
        __metadata("design:paramtypes", [])
    ], RecallClientsForCutoms);
    return RecallClientsForCutoms;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.RecallClientsForCutoms = RecallClientsForCutoms;
//# sourceMappingURL=RecallClientsForCutoms.js.map