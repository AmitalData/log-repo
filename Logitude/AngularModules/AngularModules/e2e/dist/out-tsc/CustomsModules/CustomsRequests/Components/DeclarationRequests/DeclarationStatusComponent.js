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
var DeclarationExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var DeclarationMessagesService_1 = require("../../../../Customs/Services/WebServices/DeclarationMessagesService");
var DeclarationStatusRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/DeclarationStatusRequestParams");
var DeclarationStatusResponseData_1 = require("../../../../Customs/DataContract/ResponseData/DeclarationStatusResponseData");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var DeclarationStatusComponent = /** @class */ (function (_super) {
    __extends(DeclarationStatusComponent, _super);
    function DeclarationStatusComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.IsShowAvailabiltyQuantitiesList = false;
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._DeclarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.AvailabiltyQuantitiesList = [];
        return _this;
    }
    DeclarationStatusComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    DeclarationStatusComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new DeclarationStatusRequestParams_1.DeclarationStatusRequestParams();
            this.SetIsByDeclarationNumber(true);
            this.UIProperties.SetRequired("DeclarationNumber", "Customs.Declaration", true);
        }
        if (this.ResponseData == null) {
            this.ResponseData = new DeclarationStatusResponseData_1.DeclarationStatusResponseData();
        }
        else {
            if (this.ResponseData.AvailabiltyQuantitiesList != null && this.ResponseData.AvailabiltyQuantitiesList.length > 0) {
                this.IsShowAvailabiltyQuantitiesList = true;
                this.AvailabiltyQuantitiesList = this.ResponseData.AvailabiltyQuantitiesList;
            }
        }
    };
    DeclarationStatusComponent.prototype.SetMenuArg = function (MenuArg) {
        this.OnMassageDisplayMethod();
        this.DeclarationNumber = MenuArg.DeclarationNumber;
        this.CustomFileNo = MenuArg.CustomsFile;
    };
    //#region Properties
    DeclarationStatusComponent.prototype.SetIsByDeclarationNumber = function (newValue) {
        this.IsByDeclarationNumber = newValue;
    };
    Object.defineProperty(DeclarationStatusComponent.prototype, "IsByDeclarationNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationRadio : null; },
        set: function (newValue) {
            if (this.RequestParams.DeclarationRadio != newValue) {
                this.RequestParams.DeclarationRadio = newValue;
                if (newValue == true) {
                    this.IsByCargo = false;
                    this.IsByOldReshimon = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    DeclarationStatusComponent.prototype.SetIsByCargo = function (newValue) {
        this.IsByCargo = newValue;
    };
    Object.defineProperty(DeclarationStatusComponent.prototype, "IsByCargo", {
        get: function () { return this.RequestParams ? this.RequestParams.CargoRadio : null; },
        set: function (newValue) {
            if (this.RequestParams.CargoRadio != newValue) {
                this.RequestParams.CargoRadio = newValue;
                if (newValue == true) {
                    this.IsByDeclarationNumber = false;
                    this.IsByOldReshimon = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    DeclarationStatusComponent.prototype.SetIsByOldReshimon = function (newValue) {
        this.IsByOldReshimon = newValue;
    };
    Object.defineProperty(DeclarationStatusComponent.prototype, "IsByOldReshimon", {
        get: function () { return this.RequestParams ? this.RequestParams.OldReshimonRadio : null; },
        set: function (newValue) {
            if (this.RequestParams.OldReshimonRadio != newValue) {
                this.RequestParams.OldReshimonRadio = newValue;
            }
            if (newValue == true) {
                this.IsByDeclarationNumber = false;
                this.IsByCargo = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "CustomFileNo", {
        get: function () { return this.RequestParams ? this.RequestParams.CustomFileNo : null; },
        set: function (value) {
            if (this.RequestParams.CustomFileNo != value) {
                this.RequestParams.CustomFileNo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "DeclarationNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationNumber : null; },
        set: function (value) {
            if (this.RequestParams.DeclarationNumber != value) {
                this.RequestParams.DeclarationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "CargoTypeCode", {
        get: function () { return this.RequestParams ? this.RequestParams.CargoTypeCode : null; },
        set: function (value) {
            if (this.RequestParams.CargoTypeCode != value) {
                this.RequestParams.CargoTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "ManifestNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.ManifestNumber : null; },
        set: function (value) {
            if (this.RequestParams.ManifestNumber != value) {
                this.RequestParams.ManifestNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "SecondCargoID", {
        get: function () { return this.RequestParams ? this.RequestParams.SecondCargoID : null; },
        set: function (value) {
            if (this.RequestParams.SecondCargoID != value) {
                this.RequestParams.SecondCargoID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "ThirdCargoID", {
        get: function () { return this.RequestParams ? this.RequestParams.ThirdCargoID : null; },
        set: function (value) {
            if (this.RequestParams.ThirdCargoID != value) {
                this.RequestParams.ThirdCargoID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "OldReshimonNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.OldReshimonNumber : null; },
        set: function (value) {
            if (this.RequestParams.OldReshimonNumber != value) {
                this.RequestParams.OldReshimonNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "WarningMessage", {
        get: function () { return this.ResponseData ? this.ResponseData.WarningMessage : null; },
        set: function (value) {
            if (this.ResponseData.WarningMessage != value) {
                this.ResponseData.WarningMessage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "DeclarationID", {
        get: function () { return this.ResponseData ? this.ResponseData.DeclarationID : null; },
        set: function (value) {
            if (this.ResponseData.DeclarationID != value) {
                this.ResponseData.DeclarationID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "DeclarationVersion", {
        get: function () { return this.ResponseData ? this.ResponseData.DeclarationVersion : null; },
        set: function (value) {
            if (this.ResponseData.DeclarationVersion != value) {
                this.ResponseData.DeclarationVersion = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "DeclarationStatusCode", {
        get: function () { return this.ResponseData ? this.ResponseData.DeclarationStatusCode : null; },
        set: function (value) {
            if (this.ResponseData.DeclarationStatusCode != value) {
                this.ResponseData.DeclarationStatusCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "DeclarationStatusText", {
        get: function () { return this.ResponseData ? this.ResponseData.DeclarationStatusText : null; },
        set: function (value) {
            if (this.ResponseData.DeclarationStatusText != value) {
                this.ResponseData.DeclarationStatusText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "LogisticStatusText", {
        get: function () { return this.ResponseData ? this.ResponseData.LogisticStatusText : null; },
        set: function (value) {
            if (this.ResponseData.LogisticStatusText != value) {
                this.ResponseData.LogisticStatusText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "DeclarationOfficeText", {
        get: function () { return this.ResponseData ? this.ResponseData.DeclarationOfficeText : null; },
        set: function (value) {
            if (this.ResponseData.DeclarationOfficeText != value) {
                this.ResponseData.DeclarationOfficeText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "TaxationDateTime", {
        get: function () { return this.ResponseData ? this.ResponseData.TaxationDateTime : null; },
        set: function (value) {
            if (this.ResponseData.TaxationDateTime != value) {
                this.ResponseData.TaxationDateTime = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "FinancialStatusText", {
        get: function () { return this.ResponseData ? this.ResponseData.FinancialStatusText : null; },
        set: function (value) {
            if (this.ResponseData.FinancialStatusText != value) {
                this.ResponseData.FinancialStatusText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "HandeledWroker", {
        get: function () { return this.ResponseData ? this.ResponseData.HandeledWroker : null; },
        set: function (value) {
            if (this.ResponseData.HandeledWroker != value) {
                this.ResponseData.HandeledWroker = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "ReleaseDateTime", {
        get: function () { return this.ResponseData ? this.ResponseData.ReleaseDateTime : null; },
        set: function (value) {
            if (this.ResponseData.ReleaseDateTime != value) {
                this.ResponseData.ReleaseDateTime = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationStatusComponent.prototype, "SubmitDateTime", {
        get: function () { return this.ResponseData ? this.ResponseData.SubmitDateTime : null; },
        set: function (value) {
            if (this.ResponseData.SubmitDateTime != value) {
                this.ResponseData.SubmitDateTime = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#region Commands
    DeclarationStatusComponent.prototype.DueChangeClearChildField = function (sourceIsCostomFile) {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        }
        else {
            this.CustomFileNo = "";
        }
        //this.ResponseData = null;
        this.ValidationErrorsList = [];
    };
    DeclarationStatusComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }
        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, true);
        });
    };
    DeclarationStatusComponent.prototype.DeclarationNumberTextChanged = function (DeclarationNumberText) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }
        this.DueChangeClearChildField(false);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, false);
        });
    };
    DeclarationStatusComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            this.DeclarationNumber = lastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = lastFetchDeclarationList.CustomFileNo;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        }
        else {
            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            }
            else {
                this.SetValidityDeclarationNumber();
            }
        }
    };
    DeclarationStatusComponent.prototype.SetValidityDeclarationNumber = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    };
    DeclarationStatusComponent.prototype.SetValidityCustomFileNo = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    };
    DeclarationStatusComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DeclarationStatusComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.IsByDeclarationNumber && Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (this.IsByCargo) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.CargoTypeCode)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CargoTypeCodeIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ManifestNumber)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.FirstCargoIdIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
        if (this.IsByOldReshimon && Tools_1.AppTool.IsNullOrEmpty(this.OldReshimonNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.OldReshimonIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    DeclarationStatusComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicator("");
        var currRequestParams = new DeclarationStatusRequestParams_1.DeclarationStatusRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.RequestOrigin = "DeclarationStatusRequestViewModel";
        if (this.IsByDeclarationNumber == true) {
            currRequestParams.DeclarationNumber = this.RequestParams.DeclarationNumber;
            currRequestParams.CustomFileNo = this.RequestParams.CustomFileNo;
            currRequestParams.DeclarationRadio = true;
        }
        else if (this.IsByCargo == true) {
            currRequestParams.CargoTypeCode = this.RequestParams.CargoTypeCode;
            currRequestParams.ManifestNumber = this.RequestParams.ManifestNumber;
            currRequestParams.SecondCargoID = this.RequestParams.SecondCargoID;
            currRequestParams.ThirdCargoID = this.RequestParams.ThirdCargoID;
            currRequestParams.CargoRadio = true;
        }
        else if (this.IsByOldReshimon == true) {
            currRequestParams.OldReshimonNumber = this.OldReshimonNumber;
            currRequestParams.OldReshimonRadio = true;
        }
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לסטטוס הצהרה", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._DeclarationMessagesService.PostDeclarationStatusRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], DeclarationStatusComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    DeclarationStatusComponent = __decorate([
        core_1.Component({
            selector: 'DeclarationStatusComponent',
            moduleId: module.id,
            templateUrl: './DeclarationStatusComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeclarationStatusComponent);
    return DeclarationStatusComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.DeclarationStatusComponent = DeclarationStatusComponent;
//# sourceMappingURL=DeclarationStatusComponent.js.map