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
var DeclarationExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var DeclarationMessagesService_1 = require("../../../../Customs/Services/WebServices/DeclarationMessagesService");
var PrintRequestRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/PrintRequestRequestParams");
var PrintRequestResponseData_1 = require("../../../../Customs/DataContract/ResponseData/PrintRequestResponseData");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var PrintRequestComponent = /** @class */ (function (_super) {
    __extends(PrintRequestComponent, _super);
    function PrintRequestComponent(_CD) {
        var _this = _super.call(this) || this;
        _this._CD = _CD;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.isSuccessMessageVisible = false;
        _this._DeclarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        return _this;
    }
    PrintRequestComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    PrintRequestComponent.prototype.RemoveDeclarationPrint = function (declarationPrintItem) {
        var index = this.DeclarationPrintList.indexOf(declarationPrintItem, 0);
        if (index > -1) {
            this.DeclarationPrintList.splice(index, 1);
        }
        if (this.DeclarationPrintList.length == 0) {
            this.AddDeclarationPrint(null);
        }
    };
    PrintRequestComponent.prototype.AddDeclarationPrint = function (declarationPrintItem) {
        if (declarationPrintItem != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(declarationPrintItem.CustomFileNo) && Tools_1.AppTool.IsNullOrEmpty(declarationPrintItem.DeclarationNumber)) {
                return;
            }
        }
        var my = new DeclarationPrintVM(this._CD);
        this.DeclarationPrintList.push(my);
    };
    PrintRequestComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this.RequestParams == null) {
            this.RequestParams = new PrintRequestRequestParams_1.PrintRequestRequestParams();
            this.SetIsByDeclarationNumber(true);
        }
        if (this.ResponseData) {
            if (this.ResponseData.DeclarationPrintAnswer) {
                if (this.DeclarationPrintList == null || this.DeclarationPrintList.length == 0) {
                    this.DeclarationPrintList = [];
                    this.ResponseData.DeclarationPrintAnswer.forEach(function (item) {
                        _this.AddDeclarationPrint(null);
                        _this.DeclarationPrintList[item.SequenceNumber - 1].CustomFileNo = item.CustomFileNo;
                        _this.DeclarationPrintList[item.SequenceNumber - 1].DeclarationNumber = item.DeclarationNumber;
                    });
                }
                this.LoadResponsData();
            }
        }
        else {
            this.ResponseData = new PrintRequestResponseData_1.PrintRequestResponseData();
        }
    };
    //#region Properties
    PrintRequestComponent.prototype.SetIsByDeclarationNumber = function (newValue) {
        this.ClearOldValues();
        this.IsSearchByDeclarationRadio = newValue;
    };
    PrintRequestComponent.prototype.ClearOldValues = function () {
        this.DeclarationPrintList = [];
        this.AddDeclarationPrint(null);
        this.CargoTypeCode = null;
        this.ManifestNumber = null;
        this.SecondCargoID = null;
        this.ThirdCargoID = null;
        return;
    };
    Object.defineProperty(PrintRequestComponent.prototype, "IsSearchByDeclarationRadio", {
        get: function () { return this.RequestParams ? this.RequestParams.IsSearchByDeclarationRadio : null; },
        set: function (newValue) {
            if (this.RequestParams.IsSearchByDeclarationRadio != newValue) {
                this.RequestParams.IsSearchByDeclarationRadio = newValue;
                if (newValue == true) {
                    this.IsSearchByCargoRadio = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrintRequestComponent.prototype, "IsSuccessMessageVisible", {
        get: function () { return this.isSuccessMessageVisible; },
        set: function (newValue) {
            if (this.isSuccessMessageVisible != newValue) {
                this.isSuccessMessageVisible = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    PrintRequestComponent.prototype.SetIsByCargo = function (newValue) {
        this.IsSearchByCargoRadio = newValue;
    };
    Object.defineProperty(PrintRequestComponent.prototype, "IsSearchByCargoRadio", {
        get: function () { return this.RequestParams ? this.RequestParams.IsSearchByCargoRadio : null; },
        set: function (newValue) {
            if (this.RequestParams.IsSearchByCargoRadio != newValue) {
                this.RequestParams.IsSearchByCargoRadio = newValue;
                if (newValue == true) {
                    this.IsSearchByDeclarationRadio = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrintRequestComponent.prototype, "CargoTypeCode", {
        get: function () { return this.RequestParams ? this.RequestParams.CargoTypeCode : null; },
        set: function (value) {
            if (this.RequestParams.CargoTypeCode != value) {
                this.RequestParams.CargoTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrintRequestComponent.prototype, "ManifestNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.ManifestNumber : null; },
        set: function (value) {
            if (this.RequestParams.ManifestNumber != value) {
                this.RequestParams.ManifestNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrintRequestComponent.prototype, "SecondCargoID", {
        get: function () { return this.RequestParams ? this.RequestParams.SecondCargoID : null; },
        set: function (value) {
            if (this.RequestParams.SecondCargoID != value) {
                this.RequestParams.SecondCargoID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrintRequestComponent.prototype, "ThirdCargoID", {
        get: function () { return this.RequestParams ? this.RequestParams.ThirdCargoID : null; },
        set: function (value) {
            if (this.RequestParams.ThirdCargoID != value) {
                this.RequestParams.ThirdCargoID = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrintRequestComponent.prototype, "ResponseMessage", {
        get: function () { return this.ResponseData ? this.ResponseData.ResponseMessage : null; },
        set: function (value) {
            if (this.ResponseData.ResponseMessage != value) {
                this.ResponseData.ResponseMessage = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    //#region Commands
    PrintRequestComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PrintRequestComponent.prototype.FillErrors = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.IsSearchByDeclarationRadio) {
            if (this.DeclarationPrintList.length == 0) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
            else {
                this.DeclarationPrintList.forEach(function (item) {
                    if (Tools_1.AppTool.IsNullOrEmpty(item.DeclarationNumber)) {
                        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
                        _this.ValidationErrorsList.push(msg);
                    }
                });
            }
        }
        else {
            if (Tools_1.AppTool.IsNullOrEmpty(this.CargoTypeCode)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CargoTypeCodeIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.ManifestNumber)) {
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.FirstCargoIdIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
    };
    PrintRequestComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new PrintRequestRequestParams_1.PrintRequestRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.IsSearchByDeclarationRadio = this.IsSearchByDeclarationRadio;
        currRequestParams.IsSearchByCargoRadio = this.IsSearchByCargoRadio;
        currRequestParams.DeclarationNumber = [];
        if (this.IsSearchByDeclarationRadio) {
            this.DeclarationPrintList.forEach(function (item) {
                currRequestParams.DeclarationNumber.push(item.DeclarationNumber);
            });
        }
        else {
            currRequestParams.CargoTypeCode = this.CargoTypeCode;
            currRequestParams.ManifestNumber = this.ManifestNumber;
            currRequestParams.SecondCargoID = this.SecondCargoID;
            currRequestParams.ThirdCargoID = this.ThirdCargoID;
        }
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא להדפסת הצהרה", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._DeclarationMessagesService.PostPrintRequestRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    PrintRequestComponent.prototype.LoadResponsData = function () {
        var _this = this;
        if (this.ResponseData) {
            if (this.ResponseData.DeclarationPrintAnswer && this.IsSearchByDeclarationRadio) {
                this.ResponseData.DeclarationPrintAnswer.forEach(function (item) {
                    if (item.IsFiled == true) {
                        _this.DeclarationPrintList[item.SequenceNumber - 1].VGreenVisibility = true;
                        _this.DeclarationPrintList[item.SequenceNumber - 1].XRedVisibility = false;
                    }
                    else {
                        _this.DeclarationPrintList[item.SequenceNumber - 1].PrintErrorText = item.ErrorText;
                        _this.DeclarationPrintList[item.SequenceNumber - 1].XRedVisibility = true;
                        _this.DeclarationPrintList[item.SequenceNumber - 1].VGreenVisibility = false;
                    }
                });
            }
            if (this.ResponseData.Succeeded == true && this.ResponseData.HasException == false) {
                this.IsSuccessMessageVisible = true;
            }
        }
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], PrintRequestComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    PrintRequestComponent = __decorate([
        core_1.Component({
            selector: 'PrintRequestComponent',
            moduleId: module.id,
            templateUrl: './PrintRequestComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], PrintRequestComponent);
    return PrintRequestComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.PrintRequestComponent = PrintRequestComponent;
var DeclarationPrintVM = /** @class */ (function (_super) {
    __extends(DeclarationPrintVM, _super);
    function DeclarationPrintVM(_CD) {
        var _this = _super.call(this) || this;
        _this._CD = _CD;
        _this.ObjectTableName = "Customs.Declaration";
        _this.PrintErrorText = null;
        _this.VGreenVisibility = false;
        _this.XRedVisibility = false;
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    DeclarationPrintVM.prototype.DeclarationNumberTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }
        if (this.OldDeclarationNo == this.DeclarationNumber) {
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
    DeclarationPrintVM.prototype.FetchDeclaration = function (myResponse, sourceIsCustomFile) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            this.DeclarationNumber = lastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = lastFetchDeclarationList.CustomFileNo;
            this.OldCustomFile = this.CustomFileNo;
            this.OldDeclarationNo = this.DeclarationNumber;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
            //this._CD.detectChanges();
        }
        else {
            if (sourceIsCustomFile) {
                this.SetValidityCustomFileNo();
            }
            else {
                this.SetValidityDeclarationNumber();
            }
        }
    };
    DeclarationPrintVM.prototype.SetValidityDeclarationNumber = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    };
    DeclarationPrintVM.prototype.SetValidityCustomFileNo = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    };
    DeclarationPrintVM.prototype.DueChangeClearChildField = function (sourceIsCostomFile) {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        }
        else {
            this.CustomFileNo = "";
        }
    };
    DeclarationPrintVM.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }
        if (this.OldCustomFile == this.CustomFileNo) {
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
    return DeclarationPrintVM;
}(BaseComponent_1.BaseComponent));
exports.DeclarationPrintVM = DeclarationPrintVM;
//# sourceMappingURL=PrintRequestComponent.js.map