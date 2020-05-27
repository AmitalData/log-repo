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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TapagMessagesService_1 = require("../../../../Customs/Services/WebServices/TapagMessagesService");
var GuaranteeCertificateRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/GuaranteeCertificateRequestParams");
var GuaranteeCertificateResponseData_1 = require("../../../../Customs/DataContract/ResponseData/GuaranteeCertificateResponseData");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var GuaranteeCertificateComponent = /** @class */ (function (_super) {
    __extends(GuaranteeCertificateComponent, _super);
    function GuaranteeCertificateComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._TapagMessagesService = new TapagMessagesService_1.TapagMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.AllocationObservableList = new ObservableCollection_1.ObservableCollection([]);
        _this.RequestObservableList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    GuaranteeCertificateComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    GuaranteeCertificateComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this.RequestParams == null) {
            this.RequestParams = new GuaranteeCertificateRequestParams_1.GuaranteeCertificateRequestParams();
        }
        if (this.ResponseData) {
            if (this.ResponseData.GeneralDetailsData == null) {
                this.ResponseData.GeneralDetailsData = new GuaranteeCertificateResponseData_1.GeneralDetails();
            }
            if (this.ResponseData.AllocationList) {
                this.AllocationObservableList.InsertCollection(this.ResponseData.AllocationList);
                this.AllocationVisibility = true;
            }
            if (this.ResponseData.RequestList) {
                this.ResponseData.RequestList.forEach(function (itemMess) {
                    _this.RequestObservableList.Insert(itemMess);
                    _this.RequestVisibility = true;
                });
            }
        }
        else {
            this.ResponseData = new GuaranteeCertificateResponseData_1.GuaranteeCertificateResponseData();
            this.ResponseData.GeneralDetailsData = new GuaranteeCertificateResponseData_1.GeneralDetails();
        }
    };
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GuaranteeCertificateType", {
        //#region Properties
        get: function () { return this.RequestParams ? this.RequestParams.guaranteeCertificateType : null; },
        set: function (value) {
            if (this.RequestParams.guaranteeCertificateType != value) {
                this.RequestParams.guaranteeCertificateType = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "CertificateID", {
        get: function () { return this.RequestParams.certificateID; },
        set: function (value) {
            if (this.RequestParams.certificateID != value) {
                this.RequestParams.certificateID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GuaranteeExternalCertificateNumber", {
        get: function () { return this.RequestParams.guaranteeExternalCertificateNumber; },
        set: function (value) {
            if (this.RequestParams.guaranteeExternalCertificateNumber != value) {
                this.RequestParams.guaranteeExternalCertificateNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GuarantorID", {
        get: function () { return this.RequestParams.guarantorID; },
        set: function (value) {
            if (this.RequestParams.guarantorID != value) {
                this.RequestParams.guarantorID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    GuaranteeCertificateComponent.prototype.SetAllocationVisibility = function (newValue) {
        this._RequestVisibility = newValue;
    };
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "AllocationVisibility", {
        get: function () { return this._AllocationVisibility; },
        set: function (newValue) {
            if (this._AllocationVisibility != newValue) {
                this._AllocationVisibility = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    GuaranteeCertificateComponent.prototype.SetRequestVisibility = function (newValue) {
        this._RequestVisibility = newValue;
    };
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "RequestVisibility", {
        get: function () { return this._RequestVisibility; },
        set: function (newValue) {
            if (this._RequestVisibility != newValue) {
                this._RequestVisibility = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GuaranteeTypeName", {
        //#endregion Properties
        //#region Response Properties
        get: function () { return this.ResponseData.GeneralDetailsData.guaranteeTypeName; },
        set: function (value) {
            if (this.ResponseData.GeneralDetailsData.guaranteeTypeName != value) {
                this.ResponseData.GeneralDetailsData.guaranteeTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GuaranteeExternalCertificateNumebr", {
        get: function () { return this.ResponseData.GeneralDetailsData.guaranteeExternalCertificateNumebr; },
        set: function (value) {
            if (this.ResponseData.GeneralDetailsData.guaranteeExternalCertificateNumebr != value) {
                this.ResponseData.GeneralDetailsData.guaranteeExternalCertificateNumebr = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GuaranteeValidityDate", {
        get: function () { return this.ResponseData.GeneralDetailsData.guaranteeValidityDate; },
        set: function (value) {
            if (this.ResponseData.GeneralDetailsData.guaranteeValidityDate != value) {
                this.ResponseData.GeneralDetailsData.guaranteeValidityDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "CertificateAvailableAmount", {
        get: function () { return this.ResponseData.GeneralDetailsData.certificateAvailableAmount; },
        set: function (value) {
            if (this.ResponseData.GeneralDetailsData.certificateAvailableAmount != value) {
                this.ResponseData.GeneralDetailsData.certificateAvailableAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GuaranteeCertificateStatusName", {
        get: function () { return this.ResponseData.GeneralDetailsData.GuaranteeCertificateStatusName; },
        set: function (value) {
            if (this.ResponseData.GeneralDetailsData.GuaranteeCertificateStatusName != value) {
                this.ResponseData.GeneralDetailsData.GuaranteeCertificateStatusName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GuaranteedName", {
        get: function () { return this.ResponseData.GeneralDetailsData.guaranteedName; },
        set: function (value) {
            if (this.ResponseData.GeneralDetailsData.guaranteedName != value) {
                this.ResponseData.GeneralDetailsData.guaranteedName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GeneralDetailsCertificateID", {
        get: function () { return this.ResponseData.GeneralDetailsData.certificateID; },
        set: function (value) {
            if (this.ResponseData.GeneralDetailsData.certificateID != value) {
                this.ResponseData.GeneralDetailsData.certificateID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "GuaranteeAmount", {
        get: function () { return this.ResponseData.GeneralDetailsData.guaranteeAmount; },
        set: function (value) {
            if (this.ResponseData.GeneralDetailsData.guaranteeAmount != value) {
                this.ResponseData.GeneralDetailsData.guaranteeAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GuaranteeCertificateComponent.prototype, "TotalCertificateAllocation", {
        get: function () { return this.ResponseData.GeneralDetailsData.totalCertificateAllocation; },
        set: function (value) {
            if (this.ResponseData.GeneralDetailsData.totalCertificateAllocation != value) {
                this.ResponseData.GeneralDetailsData.totalCertificateAllocation = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //#region Commands
    GuaranteeCertificateComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    GuaranteeCertificateComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        errors.forEach(function (err) { _this.ValidationErrorsList.push(err); });
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.AllocationObservableList.Clear();
        this.RequestObservableList.Clear();
        var currRequestParams = new GuaranteeCertificateRequestParams_1.GuaranteeCertificateRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.guaranteeCertificateType = this.GuaranteeCertificateType;
        currRequestParams.certificateID = this.CertificateID;
        currRequestParams.guaranteeExternalCertificateNumber = this.GuaranteeExternalCertificateNumber;
        currRequestParams.guarantorID = this.GuarantorID;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לנתוני כתב ערבות", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._TapagMessagesService.PostGuaranteeCertificateRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], GuaranteeCertificateComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    GuaranteeCertificateComponent = __decorate([
        core_1.Component({
            selector: 'GuaranteeCertificateComponent',
            moduleId: module.id,
            templateUrl: './GuaranteeCertificateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GuaranteeCertificateComponent);
    return GuaranteeCertificateComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.GuaranteeCertificateComponent = GuaranteeCertificateComponent;
//# sourceMappingURL=GuaranteeCertificateComponent.js.map