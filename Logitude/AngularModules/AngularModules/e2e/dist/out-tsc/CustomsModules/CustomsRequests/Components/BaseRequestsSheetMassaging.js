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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var CustomMessageWrapperComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var BaseRequestsSheetMassaging = /** @class */ (function (_super) {
    __extends(BaseRequestsSheetMassaging, _super);
    //private _callBackOnMassageDisplay: () => void;
    function BaseRequestsSheetMassaging() {
        var _this = _super.call(this) || this;
        _this._MyCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        //this._callBackOnMassageDisplay = callBackOnMassageDisplay;
        _this.CallOnMassageDisplayMethod();
        return _this;
    }
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "MyCustomMessageWrapperComponent", {
        get: function () { return this._MyCustomMessageWrapperComponent; },
        set: function (val) {
            console.log("MyCustomMessageWrapperComponent is settt!!!!");
            this._MyCustomMessageWrapperComponent = val;
        },
        enumerable: true,
        configurable: true
    });
    BaseRequestsSheetMassaging.prototype.ngOnInit = function () {
        //alert("BaseRequestsSheetMassaging:ngOnInit")
        //this.CallOnMassageDisplayMethod();
        ///alert(this.MyCustomMessageWrapperComponent);
    };
    BaseRequestsSheetMassaging.prototype.ngAfterContentInit = function () {
        if (this._MyCustomMessageWrapperComponent == null) {
            console.warn("BaseRequestsSheetMassaging.ngAfterContentInit this.MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("BaseRequestsSheetMassaging.ngAfterContentInit this.MyCustomMessageWrapperComponent != null");
        }
        //this.subscribeWrapperComponent()
    };
    BaseRequestsSheetMassaging.prototype.ngAfterViewInit = function () {
        if (this._MyCustomMessageWrapperComponent == null) {
            console.warn("BaseRequestsSheetMassaging.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("BaseRequestsSheetMassaging.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.subscribeWrapperComponent();
    };
    BaseRequestsSheetMassaging.prototype.subscribeWrapperComponent = function () {
        var _this = this;
        this._MyCustomMessageWrapperComponent.MyCustomSendOptionsComponent
            .SendButtonClicked.subscribe(function (myCustomSendOptionsArgs) {
            _this.CallOnCustomSendOptionsButtonClick(myCustomSendOptionsArgs);
        });
    };
    BaseRequestsSheetMassaging.prototype.CallOnCustomSendOptionsButtonClick = function (myCustomSendOptionsArgs) {
        var myIRequestsSheetMassagingComponent = this;
        if (myIRequestsSheetMassagingComponent) {
            myIRequestsSheetMassagingComponent.OnCustomSendOptionsButtonClick(myCustomSendOptionsArgs);
            this.OnMassageDisplayBase();
        }
        else {
            console.warn("Please implements IRequestsSheetMassagingComponent");
        }
    };
    BaseRequestsSheetMassaging.prototype.MassageDisplay = function (RequestParamsXml, ResponseDataXml) {
        this._RequestParamsXml = RequestParamsXml;
        this._ResponseDataXml = ResponseDataXml;
        if (Tools_1.AppTool.IsNullOrEmpty(this._RequestParamsXml)) {
            return;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this._ResponseDataXml)) {
            return;
        }
        this.RequestParams = JSON.parse(RequestParamsXml); //XmlGenericUtil<TRequestParams>.DeSerializeObject(_RequestParamsXml);
        this.ResponseData = JSON.parse(ResponseDataXml); // DeSerializeResponse(_ResponseDataXml);
        //this.OnMassageDisplay.emit();
        this.CallOnMassageDisplayMethod();
    };
    BaseRequestsSheetMassaging.prototype.CallOnMassageDisplayMethod = function () {
        console.log("CallOnMassageDisplayMethod()");
        var myIRequestsSheetMassagingComponent = this;
        if (myIRequestsSheetMassagingComponent) {
            myIRequestsSheetMassagingComponent.OnMassageDisplayMethod();
            this.OnMassageDisplayBase();
        }
        else {
            console.warn("Please implements IRequestsSheetMassagingComponent");
        }
    };
    BaseRequestsSheetMassaging.prototype.OnMassageDisplayBase = function () {
    };
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "CustomResponseContentIsDisable", {
        get: function () { return this._MyCustomMessageWrapperComponent.CustomResponseContentIsDisable; },
        set: function (val) {
            this._MyCustomMessageWrapperComponent.CustomResponseContentIsDisable = val;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "CustomRequestContentIsDisable", {
        get: function () { return this._MyCustomMessageWrapperComponent.CustomRequestContentIsDisable; },
        set: function (val) {
            this._MyCustomMessageWrapperComponent.CustomRequestContentIsDisable = val;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "CustomSendOptionsButtonIsDisable", {
        get: function () { return this._MyCustomMessageWrapperComponent.CustomSendOptionsButtonIsDisable; },
        set: function (val) {
            this._MyCustomMessageWrapperComponent.CustomSendOptionsButtonIsDisable = val;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "ValidationErrorsList", {
        get: function () { return this._MyCustomMessageWrapperComponent.ValidationErrorsList; },
        set: function (val) {
            this._MyCustomMessageWrapperComponent.ValidationErrorsList = val;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "CustomSendOptionsButtonCanForcePersonalSign", {
        get: function () { return this._MyCustomMessageWrapperComponent.CustomSendOptionsButtonCanForcePersonalSign; },
        set: function (newValue) {
            this._MyCustomMessageWrapperComponent.CustomSendOptionsButtonCanForcePersonalSign = newValue;
            ;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "IsShowCustomResponseContent", {
        get: function () { return this._MyCustomMessageWrapperComponent.IsShowCustomResponseContent; },
        set: function (newValue) {
            if (this._MyCustomMessageWrapperComponent.IsShowCustomResponseContent != newValue) {
                this._MyCustomMessageWrapperComponent.IsShowCustomResponseContent = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "MyCustomsMenuItem", {
        get: function () { return this._MyCustomMessageWrapperComponent.MyCustomsMenuItem; },
        set: function (newValue) {
            if (this._MyCustomMessageWrapperComponent.MyCustomsMenuItem != newValue) {
                this._MyCustomMessageWrapperComponent.MyCustomsMenuItem = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "MyCommunicationLogId", {
        get: function () { return this._MyCustomMessageWrapperComponent.MyCommunicationLogId; },
        set: function (newValue) {
            if (this._MyCustomMessageWrapperComponent.MyCommunicationLogId != newValue) {
                this._MyCustomMessageWrapperComponent.MyCommunicationLogId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "MyLastCustomsRequestSheetId", {
        get: function () { return this._MyCustomMessageWrapperComponent.MyLastCustomsRequestSheetId; },
        set: function (newValue) {
            if (this._MyCustomMessageWrapperComponent.MyLastCustomsRequestSheetId != newValue) {
                this._MyCustomMessageWrapperComponent.MyLastCustomsRequestSheetId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "RequestParams", {
        get: function () { return this._RequestParams; },
        set: function (newValue) {
            this._RequestParams = newValue;
            ;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BaseRequestsSheetMassaging.prototype, "ResponseData", {
        get: function () { return this._ResponseData; },
        set: function (newValue) {
            this._ResponseData = newValue;
        },
        enumerable: true,
        configurable: true
    });
    BaseRequestsSheetMassaging.prototype.DisposeMyState = function () {
        this._RequestParams = null;
        this._ResponseData = null;
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:paramtypes", [CustomMessageWrapperComponent_1.CustomMessageWrapperComponent])
    ], BaseRequestsSheetMassaging.prototype, "MyCustomMessageWrapperComponent", null);
    return BaseRequestsSheetMassaging;
}(BaseComponent_1.BaseComponent));
exports.BaseRequestsSheetMassaging = BaseRequestsSheetMassaging;
//# sourceMappingURL=BaseRequestsSheetMassaging.js.map