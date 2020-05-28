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
var CustomMessageWrapperComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DeclarationMessagesService_1 = require("../../../../Customs/Services/WebServices/DeclarationMessagesService");
var StorageEntranceUnloadingRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/StorageEntranceUnloadingRequestParams");
//import { StorageEntranceUnloadingResponseData } from '../../../../Customs/DataContract/ResponseData/StorageEntranceUnloadingResponseData';
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var DeclarationExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DeclarationConsAcceptancePMService_1 = require("../../../../Customs/Services/StandardPMs/DeclarationConsAcceptancePMService");
var DeclarationConsAcceptancePM_1 = require("../../../../Customs/EntityPMs/DeclarationConsAcceptancePM");
var StorageEntranceComponent = /** @class */ (function (_super) {
    __extends(StorageEntranceComponent, _super);
    function StorageEntranceComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationConsAcceptance";
        _this._DeclarationConsAcceptancePMService = new DeclarationConsAcceptancePMService_1.DeclarationConsAcceptancePMService();
        _this._DeclarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.StorageEntranceObservableList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    StorageEntranceComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    StorageEntranceComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new StorageEntranceUnloadingRequestParams_1.StorageEntranceUnloadingRequestParams();
        }
        //if (this.ResponseData) {
        //    if (this.ResponseData.CourierBOLDetailsList) {
        //        this.StorageEntranceObservableList.InsertCollection(this.ResponseData.CourierBOLDetailsList);
        //    }
        //}
    };
    StorageEntranceComponent.prototype.EditButtonClicked = function (item) {
        this.CurrentSession.CloseCurrentWindowEmit(item.cargoIdentifierKey3);
    };
    //#region Commands
    StorageEntranceComponent.prototype.CheckStorageEntranceOcc = function () {
        if (this.StorageEntranceObservableList != null && this.StorageEntranceObservableList.Length >= 1) {
            var lastOccCounter = this.StorageEntranceObservableList.Length - 1;
            if (this.CheckIfEnterAllDetails(this.StorageEntranceObservableList.Collection[lastOccCounter]) != true) {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show("ראשית יש להזין את כל הנתונים בשורה הקודמת");
                return false;
            }
        }
        return true;
    };
    StorageEntranceComponent.prototype.SaveAndSendStorageEntranceOcc = function (item, customSendOptionsArgs) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this._DeclarationConsAcceptancePMService.insert(item.entityPM).subscribe(function (response) {
            var result = response.Result;
            _this.CurrentSession.StopBusyIndicator();
            console.log("[Response] DeclarationConsAcceptancePMService.insert ", result);
            if (!response.HasError) {
                _this.SendStorageEntrance(item, customSendOptionsArgs);
            }
            else {
                _this.SubmitCompleted(response);
            }
        });
    };
    StorageEntranceComponent.prototype.SubmitCompleted = function (response) {
        if (!response.HasError) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            // To Check????
            var errors = [];
            //Validator.TryValidateObject(this.DeclarationPM, "Customs.DeclarationConsAcceptance", errors);
            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
            }
        }
    };
    StorageEntranceComponent.prototype.AddStorageEntranceCommand = function () {
        if (this.CheckStorageEntranceOcc() == true) {
            this.StorageEntranceObservableList.Insert(new DeclarationConsignmentAcceptanceComponent());
        }
    };
    StorageEntranceComponent.prototype.DeleteStorageEntranceCommand = function (item) {
        this.StorageEntranceObservableList.Remove(item);
    };
    StorageEntranceComponent.prototype.OnRowEnded = function ($event) {
        if (($event) == this.StorageEntranceObservableList.Length) {
            if (this.CheckStorageEntranceOcc() == true) {
                //this.SaveStorageEntranceOcc(this.StorageEntranceObservableList.Collection[$event-1]);
                this.AddStorageEntranceCommand();
            }
        }
    };
    StorageEntranceComponent.prototype.CheckIfEnterAllDetails = function (item) {
        if (Tools_1.AppTool.IsNullOrEmpty(item.DeclarationNumber)) {
            return false;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(item.ConsignmentNumber)) {
            return false;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(item.ManifestNumber)) {
            return false;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(item.EntryDate)) {
            return false;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(item.Quantity)) {
            return false;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(item.GrossWeight)) {
            return false;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(item.PackageTypeCode)) {
            return false;
        }
        return true;
    };
    StorageEntranceComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    StorageEntranceComponent.prototype.FillErrors = function (storageEntranceItem) {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(storageEntranceItem.DeclarationNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.FromDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(storageEntranceItem.ConsignmentNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(storageEntranceItem.EntryDate)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(storageEntranceItem.GrossWeight)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(storageEntranceItem.Quantity)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.ExchangeRate.O.ToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    StorageEntranceComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        if (this.StorageEntranceObservableList == null || this.StorageEntranceObservableList.Length == 0) {
            this.ValidationErrorsList.push("חובה להזין לפחות שורה אחת");
            return;
        }
        this.StorageEntranceObservableList.Collection.forEach(function (storageEntranceItem) {
            _this.FillErrors(storageEntranceItem);
            if (_this.ValidationErrorsList.length > 0) {
                return;
            }
            _this.SaveAndSendStorageEntranceOcc(storageEntranceItem, customSendOptionsArgs);
        });
    };
    StorageEntranceComponent.prototype.SendStorageEntrance = function (storageEntranceItem, customSendOptionsArgs) {
        var _this = this;
        var currRequestParams = new StorageEntranceUnloadingRequestParams_1.StorageEntranceUnloadingRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.DeclarationNumber = storageEntranceItem.DeclarationNumber;
        currRequestParams.ConsignmentNumber = storageEntranceItem.ConsignmentNumber.toString();
        currRequestParams.ManifestNumber = storageEntranceItem.ManifestNumber;
        currRequestParams.DeclarationId = storageEntranceItem.DeclarationId;
        currRequestParams.EntryDate = storageEntranceItem.EntryDate;
        currRequestParams.GrossWeight = storageEntranceItem.GrossWeight.toString();
        currRequestParams.Quantity = storageEntranceItem.Quantity.toString();
        currRequestParams.PackageTypeCode = storageEntranceItem.PackageTypeCode;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת מסר זמינות כניסה למחסן", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._DeclarationMessagesService.PostStorageEntranceUnloadingRequest(currRequestParams)
            .subscribe(function () { });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], StorageEntranceComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    StorageEntranceComponent = __decorate([
        core_1.Component({
            selector: 'StorageEntranceComponent',
            moduleId: module.id,
            templateUrl: './StorageEntranceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], StorageEntranceComponent);
    return StorageEntranceComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.StorageEntranceComponent = StorageEntranceComponent;
var DeclarationConsignmentAcceptanceComponent = /** @class */ (function (_super) {
    __extends(DeclarationConsignmentAcceptanceComponent, _super);
    function DeclarationConsignmentAcceptanceComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationConsAcceptance";
        _this.entityPM = new DeclarationConsAcceptancePM_1.DeclarationConsAcceptancePM();
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SetScreenFieldsEditability(false);
        return _this;
    }
    Object.defineProperty(DeclarationConsignmentAcceptanceComponent.prototype, "DeclarationId", {
        //#region Properties
        get: function () { return this.entityPM.DeclarationId; },
        set: function (value) {
            if (this.entityPM.DeclarationId != value) {
                this.entityPM.DeclarationId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationConsignmentAcceptanceComponent.prototype, "DeclarationNumber", {
        get: function () { return this.declarationNumber; },
        set: function (value) {
            if (this.declarationNumber != value) {
                this.declarationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationConsignmentAcceptanceComponent.prototype, "ConsignmentNumber", {
        get: function () { return this.entityPM.ConsignmentNumber; },
        set: function (value) {
            if (this.entityPM.ConsignmentNumber != value) {
                this.entityPM.ConsignmentNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationConsignmentAcceptanceComponent.prototype, "ManifestNumber", {
        get: function () { return this.manifestNumber; },
        set: function (value) {
            if (this.manifestNumber != value) {
                this.manifestNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationConsignmentAcceptanceComponent.prototype, "EntryDate", {
        get: function () { return this.entityPM.EntryDate; },
        set: function (value) {
            if (this.entityPM.EntryDate != value) {
                this.entityPM.EntryDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationConsignmentAcceptanceComponent.prototype, "Quantity", {
        get: function () { return this.entityPM.Quantity; },
        set: function (value) {
            if (this.entityPM.Quantity != value) {
                this.entityPM.Quantity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationConsignmentAcceptanceComponent.prototype, "GrossWeight", {
        get: function () { return this.entityPM.GrossWeight; },
        set: function (value) {
            if (this.entityPM.GrossWeight != value) {
                this.entityPM.GrossWeight = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationConsignmentAcceptanceComponent.prototype, "PackageTypeCode", {
        get: function () { return this.entityPM.PackageTypeCode; },
        set: function (value) {
            if (this.entityPM.PackageTypeCode != value) {
                this.entityPM.PackageTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationConsignmentAcceptanceComponent.prototype, "PackageTypeName", {
        get: function () { return this.packageTypeName; },
        set: function (newValue) { this.packageTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    DeclarationConsignmentAcceptanceComponent.prototype.SetScreenFieldsEditability = function (isDisplayOnly) {
        this.UIProperties.SetEnabled("ConsignmentNumber", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("EntryDate", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("PackageTypecode", this.ObjectTableName, isDisplayOnly);
        this.UIProperties.SetEnabled("PackageTypeName", this.ObjectTableName, isDisplayOnly);
    };
    DeclarationConsignmentAcceptanceComponent.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    DeclarationConsignmentAcceptanceComponent.prototype.DeclarationNumberTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }
        //this.DueChangeClearChildField(false);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse);
        });
    };
    DeclarationConsignmentAcceptanceComponent.prototype.FetchDeclaration = function (myResponse) {
        var _this = this;
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            if (lastFetchDeclarationList.IsCourierDeclaration != true) {
                //this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, "ההצהרה לא מסוג בלדר");
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show("ההצהרה לא מסוג בלדר");
                return;
            }
            this.CurrentSession.StartBusyIndicator("");
            this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(lastFetchDeclarationList.CustomFileNo)
                .subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                _this.FetchConsignment(myResponse, false);
            });
            this.EntryDate = Tools_1.DateTool.GetDateByDay(+0);
            this.SetScreenFieldsEditability(true);
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        }
        else {
            this.SetValidityDeclarationNumber();
        }
    };
    DeclarationConsignmentAcceptanceComponent.prototype.FetchConsignment = function (myResponse, sourceIsCostomFile) {
        var lastFetchConsignmentPMList = myResponse.Result;
        if (lastFetchConsignmentPMList != null) {
            var pm = lastFetchConsignmentPMList[0];
            this.entityPM.Tenant = pm.Tenant;
            this.entityPM.DeclarationId = pm.DeclarationId;
            this.ManifestNumber = pm.ManifestNumber;
            this.entityPM.ConsignmentNumber = pm.ConsignmentNumber;
            this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, false);
        }
    };
    DeclarationConsignmentAcceptanceComponent.prototype.SetValidityDeclarationNumber = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    };
    return DeclarationConsignmentAcceptanceComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationConsignmentAcceptanceComponent = DeclarationConsignmentAcceptanceComponent;
//# sourceMappingURL=StorageEntranceComponent.js.map