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
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var CustomItemLegalDemandsQueryRequestParams_1 = require("../../../Customs/DataContract/RequestParams/CustomItemLegalDemandsQueryRequestParams"); //TODO
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var LuhnAlgorithm_1 = require("../../../Customs/Utilities/LuhnAlgorithm");
var CustomItemLegalDemandsQueryComponent = /** @class */ (function (_super) {
    __extends(CustomItemLegalDemandsQueryComponent, _super);
    function CustomItemLegalDemandsQueryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration"; //TODO
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.CustomItemLegalDemandsQueryObservableList = new ObservableCollection_1.ObservableCollection([]);
        _this.CountriesExclusionListObservableList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    CustomItemLegalDemandsQueryComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    CustomItemLegalDemandsQueryComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new CustomItemLegalDemandsQueryRequestParams_1.CustomItemLegalDemandsQueryRequestParams();
            this.ValidToDate = new Date();
            this.UIProperties.SetRequired("CustomsBookType", this.ObjectTableName, true);
            this.UIProperties.SetRequired("ClassificationCode", this.ObjectTableName, true);
        }
        if (this.ResponseData) {
            if (this.ResponseData.CustomsLegalDemandsList) {
                this.CustomItemLegalDemandsQueryObservableList.InsertCollection(this.ResponseData.CustomsLegalDemandsList);
            }
            if (this.ResponseData.CountriesExclusionList) {
                this.CountriesExclusionListObservableList.InsertCollection(this.ResponseData.CountriesExclusionList);
            }
        }
    };
    Object.defineProperty(CustomItemLegalDemandsQueryComponent.prototype, "ValidToDate", {
        get: function () { return this.RequestParams.ValidToDate; },
        set: function (value) {
            if (this.RequestParams.ValidToDate != value) {
                this.RequestParams.ValidToDate = value;
                if (value) {
                    this.UIProperties.SetRequired("ValidToDate", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("ValidToDate", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomItemLegalDemandsQueryComponent.prototype, "ClassificationCode", {
        get: function () { return this.RequestParams.ClassificationCode; },
        set: function (value) {
            if (this.RequestParams.ClassificationCode != value) {
                this.RequestParams.ClassificationCode = value;
                if (value) {
                    this.UIProperties.SetRequired("ClassificationCode", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("ClassificationCode", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomItemLegalDemandsQueryComponent.prototype, "CustomsBookTypeName", {
        get: function () { return this.RequestParams.CustomsBookTypeName; },
        set: function (value) {
            if (this.RequestParams.CustomsBookTypeName != value) {
                this.RequestParams.CustomsBookTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomItemLegalDemandsQueryComponent.prototype, "CustomsBookType", {
        get: function () { return this.RequestParams.CustomsBookType; },
        set: function (value) {
            if (this.RequestParams.CustomsBookType != value) {
                this.RequestParams.CustomsBookType = value;
                if (value) {
                    this.UIProperties.SetRequired("CustomsBookType", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("CustomsBookType", this.ObjectTableName, true);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomItemLegalDemandsQueryComponent.prototype.OnCustomsItemLostFocus = function (customsItemTextBox) {
        //var newValue = this.CustomsItem;
        var newValue = customsItemTextBox.textValue;
        var valid = true;
        this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, true, "");
        if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            return;
        }
        //if (newValue.toString().length == 12) {
        //    if (newValue.toString().substring(10, 11) == "/") {
        //        newValue = newValue.toString().substring(0, 10) + newValue.toString().substring(11);
        //    }
        //}
        if (newValue.toString().length > 11) {
            valid = false;
            this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));
        }
        else if (newValue.toString().length < 8) {
            valid = false;
            this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));
        }
        else if (newValue.toString().length == 8) {
            newValue = newValue + "00";
            var checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + checkDigit;
            valid = true;
            ;
        }
        else if (newValue.toString().length == 9) {
            var digit = newValue.toString().substring(8);
            newValue = newValue.toString().substring(0, 8) + "00" + newValue.toString().substring(8);
            var checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.substring(0, 10));
            if (digit != checkDigit.toString()) {
                valid = false;
                this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
            }
            else {
                valid = true;
            }
        }
        else if (newValue.toString().length == 10) {
            var checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + "" + checkDigit;
            valid = true;
        }
        else if (newValue.toString().length == 11) {
            var digit = newValue.toString().substring(10);
            var checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.toString().substring(0, 10));
            if (digit != checkDigit.toString()) {
                valid = false;
                this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
            }
            else {
                valid = true;
            }
        }
        else {
            valid = true;
            this.UIProperties.SetValidity("ClassificationCode", this.ObjectTableName, true, "");
        }
        this.ClassificationCode = newValue;
        if (valid) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = false;
        }
        else {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, LogTextBoxId: customsItemTextBox.InputId });
        }
    };
    CustomItemLegalDemandsQueryComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ClassificationCode)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomItemLegalDemandsQuery.O.ClassificationCodeMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomsBookType)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomItemLegalDemandsQuery.O.CustomsBookTypeMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ValidToDate)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomItemLegalDemandsQuery.O.ValidToDateMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    CustomItemLegalDemandsQueryComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        //alert(customSendOptionsArgs.Option);
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CustomItemLegalDemandsQueryObservableList.Clear();
        this.CountriesExclusionListObservableList.Clear();
        var currRequestParams = new CustomItemLegalDemandsQueryRequestParams_1.CustomItemLegalDemandsQueryRequestParams(); ///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.CustomsBookType = this.CustomsBookType;
        currRequestParams.ClassificationCode = this.ClassificationCode;
        currRequestParams.ValidToDate = this.ValidToDate;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.CustomItemLegalDemandsQuery"), true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostCustomItemLegalDemandsQuery(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], CustomItemLegalDemandsQueryComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    CustomItemLegalDemandsQueryComponent = __decorate([
        core_1.Component({
            selector: 'CustomItemLegalDemands',
            moduleId: module.id,
            templateUrl: './CustomItemLegalDemandsQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomItemLegalDemandsQueryComponent);
    return CustomItemLegalDemandsQueryComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.CustomItemLegalDemandsQueryComponent = CustomItemLegalDemandsQueryComponent;
//# sourceMappingURL=CustomItemLegalDemandsQueryComponent.js.map