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
var DeclarationRestoreRequestParams_1 = require("../../../Customs/DataContract/RequestParams/DeclarationRestoreRequestParams");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var LuhnAlgorithm_1 = require("../../../Customs/Utilities/LuhnAlgorithm");
var DeclarationReshimonConversionComponent = /** @class */ (function (_super) {
    __extends(DeclarationReshimonConversionComponent, _super);
    function DeclarationReshimonConversionComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._MyResponseObjectToShow = null;
        _this._UserMessagehidden = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this._DeclarationTypeList =
            [
                { 'DeclarationConvertionDigits': "", 'Name': '' },
                { 'DeclarationConvertionDigits': "99", 'Name': 'יבוא' },
                { 'DeclarationConvertionDigits': "98", 'Name': 'יצוא' }
            ];
        _this.SelectedDeclarationConvertionDigits = _this._DeclarationTypeList[0].DeclarationConvertionDigits;
        return _this;
    }
    DeclarationReshimonConversionComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    DeclarationReshimonConversionComponent.prototype.DeclarationTypeListSelected = function ($event) {
        if (this.SelectedDeclarationConvertionDigits != $event) {
            this.ValidationErrorsList = [];
            this.DeclarationNumber = null;
            this.DeclarationNumberLast = null;
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;
        }
        this.SelectedDeclarationConvertionDigits = $event;
    };
    DeclarationReshimonConversionComponent.prototype.ReshimonNumberTextChanged = function (ReshimonNumbertext) {
        this.ValidationErrorsList = [];
        if (ReshimonNumbertext == this.ReshimonNumberLast) {
            return;
        }
        this.ReshimonNumber = this.ReshimonNumberLast = ReshimonNumbertext;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ReshimonNumber)) {
            this.DeclarationNumber = null;
            this.DeclarationNumberLast = null;
            return;
        }
        //check reshimon validity
        //9 digits
        if (this.ReshimonNumber.length != 9) {
            this.DeclarationNumber = null;
            this.DeclarationNumberLast = null;
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationReshimonConversion.O.ReshimonNumberLengthError"));
            return;
        }
        //DeclarationNumber = ConvertReshimonToDeclartion(this.ReshimonNumber);
        var declarationNumber = LuhnAlgorithm_1.LuhnAlgorithm.ConvertReshimonToDeclartion(this.ReshimonNumber, this.SelectedDeclarationConvertionDigits
        //.DeclarationConvertionDigits
        );
        if (Tools_1.AppTool.IsNullOrEmpty(declarationNumber)) {
            this.DeclarationNumber = null;
            this.DeclarationNumberLast = null;
            this.ValidationErrorsList.push("LuhnAlgorithm.ConvertReshimonToDeclartion Failed");
            return;
        }
        this.DeclarationNumber = declarationNumber;
        this.DeclarationNumberLast = this.DeclarationNumber;
    };
    DeclarationReshimonConversionComponent.prototype.DeclarationNumberTextChanged = function (DeclarationNumberText) {
        this.ValidationErrorsList = [];
        if (DeclarationNumberText == this.ReshimonNumberLast) {
            return;
        }
        this.ReshimonNumberLast = DeclarationNumberText;
        if (Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;
            return;
        }
        //check Declaration validity
        //14 digits
        if (this.DeclarationNumber.length != 14) {
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;
            //ErrorsList.Clear();
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationReshimonConversion.O.DeclarationNumberLengthError"));
            return;
        }
        //check digits 3-4 is 98 or 99 according to DclarationType
        if (this.DeclarationNumber.substr(2, 2) != "98" &&
            this.DeclarationNumber.substr(2, 2) != "99") {
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationReshimonConversion.O.DeclarationNumberConvertionDigitsError"));
            return;
        }
        //ReshimonNumber = ConvertDeclartionToReshimon(DeclarationNumber);
        var reshimonNumber = LuhnAlgorithm_1.LuhnAlgorithm
            .ConvertDeclartionToReshimon(this.DeclarationNumber);
        if (Tools_1.AppTool.IsNullOrEmpty(reshimonNumber)) {
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;
            this.ValidationErrorsList.push("LuhnAlgorithm.ConvertDeclartionToReshimon Failed");
            return;
        }
        this.ReshimonNumber = reshimonNumber;
        this.ReshimonNumberLast = this.ReshimonNumber;
    };
    Object.defineProperty(DeclarationReshimonConversionComponent.prototype, "ReshimonNumber", {
        get: function () { return this._ReshimonNumber; },
        set: function (value) {
            if (this._ReshimonNumber != value) {
                this._ReshimonNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationReshimonConversionComponent.prototype, "DeclarationNumber", {
        get: function () { return this._DeclarationNumber; },
        set: function (value) {
            if (this._DeclarationNumber != value) {
                this._DeclarationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    DeclarationReshimonConversionComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new DeclarationRestoreRequestParams_1.DeclarationRestoreRequestParams();
        }
        //this.RefreshScreen();
    };
    DeclarationReshimonConversionComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
    };
    DeclarationReshimonConversionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], DeclarationReshimonConversionComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    DeclarationReshimonConversionComponent = __decorate([
        core_1.Component({
            selector: 'DeclarationReshimonConversionComponent',
            moduleId: module.id,
            templateUrl: './DeclarationReshimonConversionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeclarationReshimonConversionComponent);
    return DeclarationReshimonConversionComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.DeclarationReshimonConversionComponent = DeclarationReshimonConversionComponent;
//# sourceMappingURL=DeclarationReshimonConversionComponent.js.map