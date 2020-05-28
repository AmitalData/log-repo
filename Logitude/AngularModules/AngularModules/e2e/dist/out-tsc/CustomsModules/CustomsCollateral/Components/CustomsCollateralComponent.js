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
var LogTabsComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var CustomsCollateralsAnswerPM_1 = require("../../../Customs/EntityPMs/CustomsCollateralsAnswerPM");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var CustomsCollateralPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsCollateralPMService");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CollateralRequestParams_1 = require("../../../Customs/DataContract/RequestParams/CollateralRequestParams");
var INF_MSG_GenericResponseData_1 = require("../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var DeclarationMessagesService_1 = require("../../../Customs/Services/WebServices/DeclarationMessagesService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var CustomsCollateralComponent = /** @class */ (function (_super) {
    __extends(CustomsCollateralComponent, _super);
    function CustomsCollateralComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Customs.CustomsCollateral";
        _this.DataContext = _this;
        _this.AnswersTabs = [];
        _this.customsCollateralPMService = new CustomsCollateralPMService_1.CustomsCollateralPMService();
        _this.ValidationErrorsList = [];
        _this.requestParams = new CollateralRequestParams_1.CollateralRequestParams();
        _this.responseData = new INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData();
        _this.declarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.GridHeight = 55;
        _this.Total = 0;
        _this.IsClosedButtonEnabled = true;
        _this.IsUnClosedButtonEnabled = true;
        return _this;
    }
    CustomsCollateralComponent.prototype.SetWindowArgs = function (args) {
        this.CurrentEntity = args.CurrentEntity;
        this.ItemsSource = [];
        if (this.CurrentEntity.IsClosed) {
            this.IsClosedCollateral = true;
            this.IsClosedButtonEnabled = false;
            this.IsUnClosedButtonEnabled = true;
        }
        else {
            this.IsClosedCollateral = false;
            this.IsClosedButtonEnabled = true;
            this.IsUnClosedButtonEnabled = false;
        }
        this.BuildConditionsList();
        this.BuildAnswersTabs();
        this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
    };
    CustomsCollateralComponent.prototype.BuildConditionsList = function () {
        this.ItemsSource = [];
        for (var _i = 0, _a = this.CurrentEntity.CustomsCollateralsConditions; _i < _a.length; _i++) {
            var item = _a[_i];
            this.ItemsSource.push(item);
            this.Total = this.Total + item.RequestedAmount;
            this.GridHeight += 25;
        }
    };
    CustomsCollateralComponent.prototype.BuildAnswersTabs = function () {
        var tab;
        this.AnswersTabs = [];
        if (this.CurrentEntity.CustomsCollateralsAnswers.length > 0) {
            var items = this.CurrentEntity.CustomsCollateralsAnswers.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; });
            for (var _i = 0, items_1 = items; _i < items_1.length; _i++) {
                var item = items_1[_i];
                tab = new LogTabsComponent_1.LogTab();
                tab.EntityPM = item;
                tab.Code = item.LineNumber;
                tab.Header = item.LineNumber;
                tab.Parent = this.CurrentEntity;
                tab.ComponentPath = "./CustomsModules/CustomsCollateral/Components/CustomsCollateralAnswerComponent";
                this.AnswersTabs.push(tab);
            }
        }
        else {
            this.AddAnswer(null);
        }
        this.SelectedTab = this.AnswersTabs[0];
    };
    CustomsCollateralComponent.prototype.AddAnswer = function (event) {
        this.answerIndex = 0;
        if (this.AnswersTabs.length > 0) {
            var maxObj = this.CurrentEntity.CustomsCollateralsAnswers.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current; });
            if (maxObj != null) {
                if (this.answerIndex <= maxObj.LineNumber)
                    this.answerIndex = maxObj.LineNumber;
            }
        }
        var answer = new CustomsCollateralsAnswerPM_1.CustomsCollateralsAnswerPM(this.CurrentEntity);
        answer.CustomsCollateralId = this.CurrentEntity.Id,
            answer.Tenant = this.CurrentEntity.Tenant;
        answer.LineNumber = this.answerIndex + 1;
        this.CurrentEntity.AddCustomsCollateralsAnswer(answer);
        // new tab
        var tab = new LogTabsComponent_1.LogTab();
        tab.EntityPM = answer;
        tab.Code = answer.LineNumber.toString();
        tab.Header = answer.LineNumber.toString();
        tab.Parent = this.CurrentEntity;
        tab.ComponentPath = "./CustomsModules/CustomsCollateral/Components/CustomsCollateralAnswerComponent";
        this.AnswersTabs.push(tab);
        // select the tab
        this.SelectedTab = tab;
    };
    CustomsCollateralComponent.prototype.DeleteAnswer = function (tab) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(tab)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeleteCollateralAnswer");
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 300;
            confirmWindow.Height = 150;
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.No");
            confirmWindow.Show(msg);
            var t = tab;
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) { // YES
                    tab = t;
                    var index = _this.AnswersTabs.indexOf(tab);
                    if (index < 0) {
                        console.log("The tab was not found, could not delete it :( ", tab);
                        return;
                    }
                    _this.CurrentEntity.RemoveCustomsCollateralsAnswer(tab.EntityPM);
                    _this.AnswersTabs.splice(index, 1);
                    for (var i = 0; i < _this.CurrentEntity.CustomsCollateralsAnswers.length; i++) {
                        var answer = _this.CurrentEntity.CustomsCollateralsAnswers[i];
                        answer.LineNumber = i + 1;
                    }
                    for (var i = 0; i < _this.AnswersTabs.length; i++) {
                        var collateralAnswer = _this.AnswersTabs[i].EntityPM;
                        collateralAnswer.LineNumber = i + 1;
                        _this.AnswersTabs[i].Code = collateralAnswer.LineNumber.toString();
                        _this.AnswersTabs[i].Header = collateralAnswer.LineNumber.toString();
                    }
                    // select the last tab
                    var tab = _this.AnswersTabs[0];
                    _this.SelectedTab = tab;
                }
            });
        }
    };
    CustomsCollateralComponent.prototype.OnSelectedChanged = function (tab) {
        if (!Tools_1.AppTool.IsNullOrEmpty(tab)) {
            this.SelectedTab = tab;
        }
    };
    Object.defineProperty(CustomsCollateralComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "FileNo", {
        get: function () { return this.CurrentEntity.FileNo; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "WorkerName", {
        get: function () { return this.CurrentEntity.WorkerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "EntityIdKey1", {
        get: function () { return this.CurrentEntity.EntityIdKey1; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "CollateralRequestNumber", {
        get: function () { return this.CurrentEntity.CollateralRequestNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "RequestedCollateralTypeName", {
        get: function () { return this.CurrentEntity.RequestedCollateralTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "CollateralRequestStatusName", {
        get: function () { return this.CurrentEntity.CollateralRequestStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "RequestValidityDate", {
        get: function () { return this.CurrentEntity.RequestValidityDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "CollateralValidityDate", {
        get: function () { return this.CurrentEntity.CollateralValidityDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "IncludingThirdPartyGuarantee", {
        get: function () { return this.CurrentEntity.IncludingThirdPartyGuarantee; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsCollateralComponent.prototype, "Remarks", {
        get: function () { return this.CurrentEntity.Remarks; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    CustomsCollateralComponent.prototype.CloseClicked = function () {
        var _this = this;
        this.CurrentEntity.IsClosed = true;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsCollateral.O.CloseCollateral"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.customsCollateralPMService.update(_this.CurrentEntity).subscribe(function (response) {
                    var result = response.Result;
                    _this.CurrentSession.CollateralAnswerRefreshEvent.emit({ IsClosed: _this.CurrentEntity.IsClosed });
                    _this.IsUnClosedButtonEnabled = true;
                    _this.IsClosedButtonEnabled = false;
                    _this.IsClosedCollateral = true;
                });
            }
        });
    };
    CustomsCollateralComponent.prototype.UnCloseClicked = function () {
        var _this = this;
        this.CurrentEntity.IsClosed = false;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsCollateral.O.ReOpenCollateral"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.customsCollateralPMService.update(_this.CurrentEntity).subscribe(function (response) {
                    var result = response.Result;
                    _this.CurrentSession.CollateralAnswerRefreshEvent.emit({ IsClosed: _this.CurrentEntity.IsClosed });
                    _this.IsUnClosedButtonEnabled = false;
                    _this.IsClosedButtonEnabled = true;
                    _this.IsClosedCollateral = false;
                });
            }
        });
    };
    CustomsCollateralComponent.prototype.GetRequierdFieldErrorText = function (fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate(fieldName));
    };
    CustomsCollateralComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        //  alert(customSendOptionsArgs);
        var _this = this;
        var errors = [];
        this.ValidationErrorsList = [];
        for (var _i = 0, _a = this.CurrentEntity.CustomsCollateralsAnswers; _i < _a.length; _i++) {
            var item = _a[_i];
            if (!item.NewFileRequest) {
                if (Tools_1.AppTool.IsNullOrEmpty(item.AnswerEntityTypeCode)) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.AnswerEntityTypeCode"));
                }
                if (item.AllocatedAmount == null) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.AllocatedAmount"));
                }
                if (item.CustomsTapgFile == null) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.CustomsTapgFile"));
                }
            }
            else {
                var sum = Tools_1.ArrayTool.Sum(item.CollateralsRequestFileConds, "RequestedAmount");
                if (item.RequestFileAmount != sum) {
                    errors.push("הסכום שהזנת שונה מסכום הדרישה יש לשנות במסך מענה לבטוחה");
                }
                if (item.RequestFileAmount == null) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.RequestFileAmount"));
                }
                if (item.RequestFileTypeCode == null) {
                    errors.push(this.GetRequierdFieldErrorText("Customs.CustomsCollateralsAnswer.F.RequestFileTypeCode"));
                }
            }
        }
        if (this.CurrentEntity.CustomsCollateralsAnswers.length > 0) {
            var nullVM = this.CurrentEntity.CustomsCollateralsAnswers.filter(function (vm) { return vm.IsClosed != true; });
            if (nullVM.length == 0) {
                this.ValidationErrorsList.push("כל המענים סגורים- לא ניתן לבצע שליחה");
                return;
            }
        }
        // this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.customsCollateralPMService.update(this.CurrentEntity).subscribe(function (response) {
                if (_this.CurrentEntity.CustomsCollateralsAnswers.length > 0) {
                    _this.CurrentSession.StartBusyIndicator("");
                    var answerParams = [];
                    for (var _i = 0, _a = _this.CurrentEntity.CustomsCollateralsAnswers; _i < _a.length; _i++) {
                        var tab = _a[_i];
                        var answerParam = new CollateralRequestParams_1.CustomsCollateralsAnswerParams();
                        answerParam.AllocatedAmount = tab.AllocatedAmount;
                        answerParam.AnswerEntityTypeCode = tab.AnswerEntityTypeCode;
                        answerParam.AnswerForCollateralStatusCode = tab.AnswerForCollateralStatusCode;
                        answerParam.CustomsCollateralId = _this.CurrentEntity.Id;
                        answerParam.CustomsNumeral = tab.CustomsNumeral;
                        answerParam.CustomsTapgFile = tab.CustomsTapgFile;
                        answerParam.LineNumber = tab.LineNumber;
                        answerParam.Tenant = tab.Tenant;
                        answerParam.Remarks = tab.Remarks;
                        answerParams.push(new CollateralRequestParams_1.CustomsCollateralsAnswerParams());
                    }
                    var LoggingObjectTableId = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0].Id;
                    var requestParams = new CollateralRequestParams_1.CollateralRequestParams();
                    requestParams.LoggingEnabled = true;
                    requestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    requestParams.CustomCollateralId = _this.CurrentEntity.Id;
                    requestParams.CustomsCollateralsAnswers = answerParams;
                    requestParams.Tenant = _this.CurrentEntity.Tenant;
                    requestParams.RequestName = "Send Collateral Request";
                    requestParams.ResponseName = "Send Collateral Response";
                    requestParams.LoggingEntityId = _this.CurrentEntity.Id;
                    LoggingObjectTableId = LoggingObjectTableId;
                    CustomMessageProgressComponent_1.CustomMessageProgressComponent
                        .ShowProgressBar(requestParams.PBId, "שליחת מענה לדרישת בטוחה", true)
                        .then(function (res) {
                        _this.responseData = res;
                        _this.OnMassageDisplayMethod();
                    }).catch(function (err) {
                        //this.ValidationErrorsList = [];
                        //     this.ValidationErrorsList.push(err);
                    });
                    _this.declarationMessagesService.PostSendCollateralAnswers(requestParams)
                        .subscribe(function (response) {
                        if (response) {
                            if (!response.HasError) {
                                if (response.Result.Succeeded) {
                                    _this.customsCollateralPMService.get(_this.CurrentEntity.Id).subscribe(function (response) {
                                        if (response) {
                                            if (!response.HasError) {
                                                _this.CurrentEntity = response.Result;
                                                _this.BuildAnswersTabs();
                                            }
                                        }
                                    });
                                }
                            }
                        }
                    });
                }
                else {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Width = 450;
                    messageWindow.Height = 190;
                    messageWindow.Show("Add at least an answer to send.");
                }
            });
        }
        else {
            this.ValidationErrorsList = errors;
        }
    };
    CustomsCollateralComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.requestParams == null) {
            this.requestParams = new CollateralRequestParams_1.CollateralRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData();
        }
    };
    CustomsCollateralComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CustomsCollateralComponent.prototype.ViewDocumentsComponent = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.EntityPM = this.CurrentEntity;
        windowArgs.ObjectTableName = this.ObjectTableName;
        var windowTitle = "Customs.Declaration.TH.Documents";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) { return _this.OnDocumentsWindowClosed($event); });
        this.entityArgs.SkipCtor = true;
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent');
    };
    CustomsCollateralComponent.prototype.OnDocumentsWindowClosed = function (event) {
        this.entityArgs.SkipCtor = false;
    };
    CustomsCollateralComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsCollateralComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomsCollateralComponent);
    return CustomsCollateralComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsCollateralComponent = CustomsCollateralComponent;
//# sourceMappingURL=CustomsCollateralComponent.js.map