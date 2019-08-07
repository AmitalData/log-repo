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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var PaymentOrderConnectionTableExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/PaymentOrderConnectionTableExtendedPMService");
var CustomsSettingListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var CH_NG_191_MSG2_ChangingTimeRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/CH_NG_191_MSG2_ChangingTimeRequestParams");
var RequestParamsBase_1 = require("../../../../../Customs/DataContract/RequestParams/RequestParamsBase");
var IIGGeneralMessagesService_1 = require("../../../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var CustomMessageProgressComponent_1 = require("../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var PhysicalCheckGeneralTabComponent = /** @class */ (function (_super) {
    __extends(PhysicalCheckGeneralTabComponent, _super);
    function PhysicalCheckGeneralTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.PhysicalCheck";
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.paymentOrderConnectionTableExtendedPMService = new PaymentOrderConnectionTableExtendedPMService_1.PaymentOrderConnectionTableExtendedPMService;
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this.AskForAnEarlierDate = false;
        _this.AskForAnLaterDate = false;
        _this.AvailableTimeChecked = false;
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this.XrayItems = [];
        _this.ValidationErrorsList = [];
        _this.IsSpotlightMode = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._ChooseDate = false;
        _this.EntityResourceService.getEntityResourceByTableName("Customs.PhysicalCheck").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(function (response) {
                _this.Init();
                //this.EntityPM = this.entityArgs.EntityPM;
                //this.ObjectTableName = this.entityArgs.ObjectTableName;
                ////this.ToDate = this.FromDate = new Date();
                //this.Listen();
            });
        });
        return _this;
    }
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "FromDate", {
        get: function () { return this._FromDate; },
        set: function (value) {
            this._FromDate = value;
            if (value) {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "ToDate", {
        get: function () { return this._ToDate; },
        set: function (value) {
            this._ToDate = value;
            if (value) {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "ErrorsList", {
        get: function () { return this.ValidationErrorsList; },
        set: function (val) {
            this.ValidationErrorsList = val;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "PhysicalCheckParam", {
        set: function (val) {
            this.entityArgs = this._InputParam = val;
            this.Init();
        },
        enumerable: true,
        configurable: true
    });
    PhysicalCheckGeneralTabComponent.prototype.Init = function () {
        if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null))
            return;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.Listen();
    };
    PhysicalCheckGeneralTabComponent.prototype.ngAfterViewInit = function () {
        // viewChildren is set
        // this.SetByAvailableTimeChecked();
    };
    PhysicalCheckGeneralTabComponent.prototype.ngAfterContentInit = function () {
        this.FromDate = Tools_1.DateTool.GetDateByDay(+0);
        this.ToDate = Tools_1.DateTool.GetDateByDay(+7);
        this.SetByAvailableTimeChecked();
        //alert(this.AllDates);
        //this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
    };
    PhysicalCheckGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.currentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    //this.BuildAccountingCustomFilesList();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.currentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DEGC") {
                        //this.DisplayOnlyCheck();
                    }
                }
            }));
        }
    };
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    PhysicalCheckGeneralTabComponent.prototype.SetWindowArgs = function (winArg) {
        this.EntityPM = winArg.EntityPM;
        //this.EntityPM = winArg.declarationPM;
    };
    PhysicalCheckGeneralTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    PhysicalCheckGeneralTabComponent.prototype.SetByAskForAnEarlierDate = function () {
        this.ResetAll();
        this.AskForAnEarlierDate = true;
    };
    PhysicalCheckGeneralTabComponent.prototype.SetByAskForAnLaterDate = function () {
        this.ResetAll();
        this.AskForAnLaterDate = true;
    };
    PhysicalCheckGeneralTabComponent.prototype.SetByAvailableTimeChecked = function () {
        this.ResetAll();
        this.AvailableTimeChecked = true;
        this.SetDateEnable(true);
        this.FromDate = this.FromDate;
        this.ToDate = this.ToDate;
    };
    PhysicalCheckGeneralTabComponent.prototype.ResetAll = function () {
        this.AskForAnEarlierDate = this.AskForAnLaterDate = this.AvailableTimeChecked = false;
        this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
        this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
        this.SetDateEnable(false);
    };
    PhysicalCheckGeneralTabComponent.prototype.SetDateEnable = function (enable) {
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, enable);
        //let from: LogDatePickerComponent = this.AllDates[0];
        //from.IsDisabled = !enable;
        //let to: LogDatePickerComponent = this.AllDates[1];
        //to.IsDisabled = !enable;
    };
    PhysicalCheckGeneralTabComponent.prototype.Choose = function (itemDate) {
        this.SelectedDateTime = itemDate;
        this._ChooseDate = true;
        this.SendCheckRequest();
    };
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "CheckId", {
        get: function () { return this.EntityPM.CheckId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "CheckSiteCode", {
        get: function () { return this.EntityPM.CheckSiteCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "QueueTypeCode", {
        get: function () { return this.EntityPM.QueueTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "CargoIdentifierKey1", {
        get: function () { return this.EntityPM.CargoIdentifierKey1; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "CargoIdentifierKey2", {
        get: function () { return this.EntityPM.CargoIdentifierKey2; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "CargoIdentifierKey3", {
        get: function () { return this.EntityPM.CargoIdentifierKey3; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "CargoIdentifierTypeCode", {
        get: function () { return this.EntityPM.CargoIdentifierTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PhysicalCheckGeneralTabComponent.prototype, "CargoIdentifierTypeName", {
        get: function () { return this.EntityPM.CargoIdentifierTypeName; },
        enumerable: true,
        configurable: true
    });
    PhysicalCheckGeneralTabComponent.prototype.SendCheckRequest = function () {
        this.RequestStatusMessage = null;
        this.ErrorsList = [];
        this.ValidateRequest();
        if (this.ErrorsList.length == 0) {
            this.SendRequest();
        }
    };
    PhysicalCheckGeneralTabComponent.prototype.SendRequest = function () {
        var _this = this;
        var requestTypeparam = "";
        var checkParams = new CH_NG_191_MSG2_ChangingTimeRequestParams_1.CH_NG_191_MSG2_ChangingTimeRequestParams();
        if (this._ChooseDate) {
            requestTypeparam = "2";
            checkParams.QueueDate = this.SelectedDateTime;
            checkParams.QueueDateSpecified = true;
            //case 2: // In case of approval / deny of a requested date
        }
        else {
            if (this.AvailableTimeChecked) {
                requestTypeparam = "1";
                //case 1: // In .Case of list of available dates & times 
                checkParams.DateSearchFrom = this.FromDate;
                checkParams.DateSearchTo = this.ToDate;
                checkParams.DateSearchFromSpecified = true;
                checkParams.DateSearchToSpecified = true;
            }
            else {
                //if (this.AskForAnEarlierDate) {
                //    this.bringQueueForwardIndicator = true
                //}
                requestTypeparam = "3";
                //case 3: // In Case of automatic update
            }
        }
        var objecttable = window.ObjectTables.filter(function (d) { return d.Name == "Customs.PhysicalCheck"; })[0];
        checkParams.RequestType = requestTypeparam;
        checkParams.PhysicalCheckId = this.EntityPM.Id;
        checkParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        checkParams.LoggingEnabled = true;
        checkParams.LoggingEntityId = this.EntityPM.Id;
        if (this.AskForAnEarlierDate) {
            checkParams.BringQueueForwardIndicator = true;
        }
        checkParams.LoggingObjectTableId = objecttable.Id;
        checkParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        checkParams.RequestName = "Changing Time Request";
        checkParams.ResponseName = "Approve Changing Time";
        checkParams.CheckTypeCode = this.EntityPM.CheckTypeCode;
        //if (sendOption == null) {
        checkParams.RequestVIA = RequestParamsBase_1.SendRequestVIA.Default;
        //}
        //else if (sendOption == "WI") {
        //    checkParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
        //}
        //else if (sendOption == "WB") {
        //    checkParams.RequestVIA = SendRequestVIA.WebServiceBatch;
        //}
        //else if (sendOption == "D") {
        //    checkParams.RequestVIA = SendRequestVIA.DCABatch;
        //}
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(checkParams.PBId, "שליחת בקשה- בדיקה פיזית", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.AnalyzeResponseMessage(_this.ResponseData);
        }).catch(function (err) {
            _this.ErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostChangingTimeRequestParams(checkParams)
            .subscribe(function () { });
        //customServiceReference.SendCheckRequestCompleted += customServiceReference_SendCheckRequestCompleted;
        //customServiceReference.SendCheckRequestAsync(bytearray);
        //SelectedTest2Index = 0;
        //SelectedTestIndex = 0;
        //SelectedTest = null;
    };
    PhysicalCheckGeneralTabComponent.prototype.AnalyzeResponseMessage = function (customCheckData) {
        var _this = this;
        var userMessage = "";
        if (!customCheckData.HasException) {
            var RequestStatusMessage = "";
            //switch (requestTypeIndex) {
            //    case 0:
            //        {
            //            RequestStatusMessage = "Customs.PhysicalCheck.F.AutomaticDateMessage";
            //            break;
            //        }
            //    case 1:
            //        {
            //            RequestStatusMessage = "Customs.PhysicalCheck.F.AvailableTimesMessage";
            //            break;
            //        }
            //    case 2:
            //        {
            //            RequestStatusMessage = "Customs.PhysicalCheck.F.ChooseDateMessage";
            //            break;
            //        }
            //}
            //userMessage = TextCodeTranslator.Translate(RequestStatusMessage);
            this.PiscalCheckItems = customCheckData.PiscalCheckItems;
            customCheckData.XrayItems.forEach(function (date) {
                _this.XrayItems.push(date);
            });
            if (!Tools_1.AppTool.IsNullOrEmpty(customCheckData.XrayItems)) {
                if (customCheckData.XrayItems.length > 0) {
                    try {
                        if (CustomMessageProgressComponent_1.CustomMessageProgressComponent.CurrCustomMessageProgressHelper) {
                            CustomMessageProgressComponent_1.CustomMessageProgressComponent.CurrCustomMessageProgressHelper.MessageArrived = true;
                        }
                    }
                    catch (err) {
                    }
                }
            }
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            //RefreshScreenEvent myEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<RefreshScreenEvent>();
            //myEvent.Publish(new RefreshScreenEventArgs("") { ScreenCode = "PhysicalCheckSpotlight" });
        }
        else {
            /*ValidationResult error = new ValidationResult(customCheckData.UserMessage);
            ErrorsList.Add(error);
            FillErrors(ErrorsList);*/
            userMessage = customCheckData.UserMessage;
            if (Tools_1.AppTool.IsNullOrEmpty(userMessage)) {
                userMessage = "שליחה נכשלה";
            }
        }
        //busyIndicatorStartEvent.Publish(new BusyIndicatorStartEventArgs() { Start = false, });
        if (this.IsSpotlightMode) {
            //spotlightSaveCompletedEvent.Publish(new SpotlightSaveCompletedEventArgs() { entityId = entityPM.Id });
        }
        //FirePropertyChanged("XrayItems");
        return userMessage;
    };
    PhysicalCheckGeneralTabComponent.prototype.ValidateRequest = function () {
        if (this.AvailableTimeChecked) {
            if (this._ChooseDate) {
                if (this.SelectedDateTime == null) {
                    this.ErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PhysicalCheck.O.SelectFromAvailableTimes"));
                }
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CheckId)) {
            this.ErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PhysicalCheck.O.CheckIdRequierd"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CheckSiteCode)) {
            this.ErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PhysicalCheck.O.CheckSiteRequierd"));
        }
    };
    PhysicalCheckGeneralTabComponent.prototype.Validate1stRequest = function () {
        if (this.FromDate == null) {
            this.ErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PhysicalCheck.O.SelectFromAvailableTimes"));
        }
        else if (this.ToDate == null) {
            this.ErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PhysicalCheck.O.SelectFromAvailableTimes"));
        }
    };
    PhysicalCheckGeneralTabComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        this._ChooseDate = false;
        this.XrayItems = [];
        this.ErrorsList = [];
        //Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        //errors.forEach((err) => { this.ValidationErrorsList.push(err); });
        this.Validate1stRequest();
        if (this.ErrorsList.length > 0) {
            return;
        }
        this.SendRequest();
        //if (this.MorningMessageObservableList.Length > 0) {
        //    this.MorningMessageObservableList.Clear();
        //}
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", EntityArgs_1.EntityArgs),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], PhysicalCheckGeneralTabComponent.prototype, "PhysicalCheckParam", null);
    PhysicalCheckGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'PhysicalCheckAvailableTimes',
            moduleId: module.id,
            templateUrl: './PhysicalCheckGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], PhysicalCheckGeneralTabComponent);
    return PhysicalCheckGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.PhysicalCheckGeneralTabComponent = PhysicalCheckGeneralTabComponent;
var XRayAvailableItem = /** @class */ (function () {
    function XRayAvailableItem() {
    }
    return XRayAvailableItem;
}());
exports.XRayAvailableItem = XRayAvailableItem;
//# sourceMappingURL=PhysicalCheckGeneralTabComponent.js.map