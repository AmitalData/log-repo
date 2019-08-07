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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CustomNumbersPipe_1 = require("../../../../Infrastructure/Pipes/CustomNumbersPipe");
var ShipmentPM_1 = require("../../../../Shipment/EntityPMs/ShipmentPM");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ShipmentPMService_1 = require("../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var ShipmentAdditionalCloudDataService_1 = require("../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService");
var DocumentsFilingExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DocumentTypeMetaDataExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var ECommercePaymentRequestMobileComponent = /** @class */ (function (_super) {
    __extends(ECommercePaymentRequestMobileComponent, _super);
    function ECommercePaymentRequestMobileComponent(cd) {
        var _this = _super.call(this) || this;
        _this.cd = cd;
        _this.DataContext = _this;
        _this.messageWindow = new MessageWindow_1.MessageWindow();
        _this.EntityPm = new ShipmentPM_1.ShipmentPM();
        _this.AdditionalData = {};
        _this.externalDocs = [];
        _this.isAccepted = false;
        _this.ShowFinalMessage = false;
        _this.SecurityKey = "";
        _this.Tenant = null;
        _this.companyLogo = "";
        _this.totalAmount = 0;
        _this.ValidationWarningsList = null;
        _this.FinalMessage = "גרסה זו אושרה";
        _this.ecommerceSupportEmail = "";
        _this.ShowPaymentDetailsScreen = false;
        _this.MyAdditionalData = null;
        _this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        _this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService_1.ShipmentAdditionalCloudDataService();
        _this._ImageLibraryService = new ImageLibraryService_1.ImageLibraryService();
        _this._DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService_1.DocumentTypeMetaDataExtendedService();
        _this._ShipmentPMService = new ShipmentPMService_1.ShipmentPMService();
        _this.AdditionalData.RequestPaymentData = {};
        return _this;
        //this.AdditionalData.RequestPaymentData.ServiceTypes = [];
    }
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "IsAccepted", {
        get: function () { return this.isAccepted; },
        set: function (newValue) { this.isAccepted = newValue; },
        enumerable: true,
        configurable: true
    });
    ECommercePaymentRequestMobileComponent.prototype.IsAcceptedChanged = function ($event) {
        this.IsAccepted = $event;
    };
    ECommercePaymentRequestMobileComponent.prototype.ngOnInit = function () {
    };
    ECommercePaymentRequestMobileComponent.prototype.ngAfterViewInit = function () {
    };
    ECommercePaymentRequestMobileComponent.prototype.RunComponent = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.IsExternalParams) {
            if (SessionLocator_1.SessionLocator.ExternalParams) {
                if (SessionLocator_1.SessionLocator.ExternalParams.Menu && SessionLocator_1.SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "preq") {
                    var me = SessionLocator_1.SessionLocator.ExternalParams;
                    if (me.SecurityKey) {
                        this.SecurityKey = me.SecurityKey;
                    }
                    if (me.Tenant) {
                        this.Tenant = me.Tenant;
                    }
                    //SessionLocator.ExternalParams.Args.forEach(arg => {
                    //    if (arg.FieldName == 'ShipmentId') {
                    //        ShipmentId = arg.FieldValue; 
                    //    }
                    //});
                    SessionLocator_1.SessionLocator.ClearExternalParams();
                }
            }
        }
        this._ShipmentPMService.getSingleBySecurityKeyTenantWithoutToken(this.SecurityKey, this.Tenant).subscribe(function (MyResult) {
            if (MyResult.Result) {
                //this.EntityPm = MyResult.Result;
                //this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.EntityPm.Id,Tenant).subscribe(AdditionalResult => {
                _this.AdditionalData = MyResult.Result; //AdditionalResult.Result
                if (_this.AdditionalData.IsPaymentRequired) {
                    if (_this.EntityPm) {
                        var ammount = 0;
                        _this.AdditionalData.RequestPaymentData.ServiceTypes.forEach(function (item, key) {
                            ammount += +(item.AmountInNIS);
                        });
                        _this.TotalAmount = ammount;
                    }
                }
                else {
                    _this.FinalMessage = "קובץ זה אינו נדרש לתשלום";
                    _this.ShowFinalMessage = true;
                }
                //});
                var service = new CommonDomainService_1.CommonDomainService();
                service.GetTenantLogoUri(_this.Tenant).subscribe(function (myLogoResult) {
                    _this.CompanyLogo = myLogoResult.Result;
                });
                //GetTenantEcommerceSupportEmail
                service.GetTenantEcommerceSupportEmail(_this.Tenant).subscribe(function (myTenant) {
                    if (myTenant.Result) {
                        _this.EcommerceSupportEmail = myTenant.Result;
                    }
                });
                if (_this.RefreshTimer) {
                    clearTimeout(_this.RefreshTimer);
                }
                _this.RefreshTimer = setInterval(function () { return _this.ReloadPage(); }, 1200000); //1200000
            }
            else {
                _this.FinalMessage = "התיק לא קיים בסביבה הזו";
                _this.ShowFinalMessage = true;
            }
        });
    };
    ECommercePaymentRequestMobileComponent.prototype.ReloadPage = function () {
        var _this = this;
        var ConfirmResult = confirm("The page has expired. Do you want to refresh it ?");
        if (ConfirmResult == true || ConfirmResult == false) {
            if (this.RefreshTimer) {
                clearTimeout(this.RefreshTimer);
            }
            this._ShipmentPMService.getSingleBySecurityKeyTenantWithoutToken(this.SecurityKey, this.Tenant).subscribe(function (MyResult) {
                if (MyResult.Result) {
                    //this.EntityPm = MyResult.Result;
                    //this._ShipmentAdditionalCloudDataService.getSingleWithoutToken(this.EntityPm.Id,Tenant).subscribe(AdditionalResult => {
                    _this.AdditionalData = MyResult.Result; //AdditionalResult.Result
                    if (_this.AdditionalData.IsPaymentRequired) {
                        if (_this.EntityPm) {
                            var ammount = 0;
                            _this.AdditionalData.RequestPaymentData.ServiceTypes.forEach(function (item, key) {
                                ammount += +(item.AmountInNIS);
                            });
                            _this.TotalAmount = ammount;
                        }
                    }
                    else {
                        _this.FinalMessage = "קובץ זה אינו נדרש לתשלום";
                        _this.ShowFinalMessage = true;
                    }
                    _this.RefreshTimer = setInterval(function () { return _this.ReloadPage(); }, 1200000); //1200000
                }
                else {
                    _this.FinalMessage = "התיק לא קיים בסביבה הזו";
                    _this.ShowFinalMessage = true;
                }
            });
        }
    };
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "CompanyLogo", {
        get: function () { return this.companyLogo; },
        set: function (newValue) { this.companyLogo = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "TotalAmount", {
        get: function () { return this.totalAmount; },
        set: function (newValue) { this.totalAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "EcommerceSupportEmail", {
        get: function () { return this.ecommerceSupportEmail; },
        set: function (newValue) { this.ecommerceSupportEmail = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "CustomerName", {
        get: function () { return this.AdditionalData.RequestPaymentData.CustomerName; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.CustomerName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "CustomerAddress", {
        get: function () { return this.AdditionalData.RequestPaymentData.CustomerAddress; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.CustomerAddress = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "Master", {
        get: function () { return this.AdditionalData.RequestPaymentData.Master; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.Master = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "Hawb", {
        get: function () { return this.AdditionalData.RequestPaymentData.Hawb; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.Hawb = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "DeclarationNumber", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.RequestPaymentData.DeclarationNumber, 0); },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.DeclarationNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "ShipmentValueInNIS", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.RequestPaymentData.ShipmentValueInNIS, 0); },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.ShipmentValueInNIS = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "SenderDetails", {
        get: function () { return this.AdditionalData.RequestPaymentData.SenderDetails; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.SenderDetails = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "GoodsDescritpion", {
        get: function () { return this.AdditionalData.RequestPaymentData.GoodsDescritpion; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.GoodsDescritpion = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "IsImporterApprovalRequried", {
        get: function () { return this.AdditionalData.RequestPaymentData.IsImporterApprovalRequried; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.IsImporterApprovalRequried = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "Quantity", {
        get: function () { return this.AdditionalData.RequestPaymentData.Quantity; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.Quantity = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "Weight", {
        get: function () { return this.AdditionalData.RequestPaymentData.Weight; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.Weight = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "TotalChargesInNIS", {
        get: function () { return this.AdditionalData.RequestPaymentData.TotalChargesInNIS; },
        set: function (newValue) { this.AdditionalData.RequestPaymentData.TotalChargesInNIS = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "sum", {
        get: function () { return this.AdditionalData.PaymentData.sum; },
        set: function (newValue) { this.AdditionalData.PaymentData.sum = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "currency", {
        get: function () { return this.AdditionalData.PaymentData.currency; },
        set: function (newValue) { this.AdditionalData.PaymentData.currency = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "op", {
        get: function () { return this.AdditionalData.PaymentData.op; },
        set: function (newValue) { this.AdditionalData.PaymentData.op = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "DCdisable", {
        get: function () { return this.AdditionalData.PaymentData.DCdisable; },
        set: function (newValue) { this.AdditionalData.PaymentData.DCdisable = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "DclickTK", {
        get: function () { return this.AdditionalData.PaymentData.DclickTK; },
        set: function (newValue) { this.AdditionalData.PaymentData.DclickTK = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ECommercePaymentRequestMobileComponent.prototype, "thtk", {
        get: function () { return this.AdditionalData.PaymentData.thtk; },
        set: function (newValue) { this.AdditionalData.PaymentData.thtk = newValue; },
        enumerable: true,
        configurable: true
    });
    ECommercePaymentRequestMobileComponent.prototype.PaymentDetailsClick = function () {
        this.ShowPaymentDetailsScreen = true;
    };
    ECommercePaymentRequestMobileComponent.prototype.ClosePaymentDetailsButtonClicked = function () {
        this.ShowPaymentDetailsScreen = false;
    };
    ECommercePaymentRequestMobileComponent.prototype.OnPayClick = function () {
        //alert("Yes");
        document.forms["form"].submit();
    };
    ECommercePaymentRequestMobileComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ECommercePaymentRequestMobileComponent.html'
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ECommercePaymentRequestMobileComponent);
    return ECommercePaymentRequestMobileComponent;
}(BaseComponent_1.BaseComponent));
exports.ECommercePaymentRequestMobileComponent = ECommercePaymentRequestMobileComponent;
//# sourceMappingURL=ECommercePaymentRequestMobileComponent.js.map