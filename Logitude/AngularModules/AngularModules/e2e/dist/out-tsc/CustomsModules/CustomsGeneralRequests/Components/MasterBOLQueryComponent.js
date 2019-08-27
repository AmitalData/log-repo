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
var DeclarationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var MasterBOLQueryRequestParams_1 = require("../../../Customs/DataContract/RequestParams/MasterBOLQueryRequestParams");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var RequestParamsBase_1 = require("../../../Customs/DataContract/RequestParams/RequestParamsBase");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var MasterBOLQueryComponent = /** @class */ (function (_super) {
    __extends(MasterBOLQueryComponent, _super);
    function MasterBOLQueryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this._IsFromDeclaration = false;
        _this._DeclarationId = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.InternalCargosList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    MasterBOLQueryComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    MasterBOLQueryComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new MasterBOLQueryRequestParams_1.MasterBOLQueryRequestParams();
            this.Date = new Date().getFullYear();
            this.ReturnAllInernalCargos = true; //task 44705 21.11.18
            this.UIProperties.SetRequired("MasterBillOfLading", null, true);
            this.UIProperties.SetRequired("InternalIdentifier", null, true);
        }
        if (this.ResponseData) {
            if (this.ResponseData.InternalCargosList) {
                this.InternalCargosList.InsertCollection(this.ResponseData.InternalCargosList);
            }
        }
    };
    MasterBOLQueryComponent.prototype.EditButtonClicked = function (item) {
        this.CurrentSession.CloseCurrentWindowEmit(item);
    };
    MasterBOLQueryComponent.prototype.SetMenuArg = function (MenuArg) {
        this.OnMassageDisplayMethod();
        this.IsFromDeclaration = true;
        this.CustomFileNo = MenuArg.CustomFileNo;
        this.Date = MenuArg.Date;
        this.MasterBillOfLading = MenuArg.MasterBillOfLading;
        this.InternalIdentifier = MenuArg.InternalIdentifier;
        this.ReturnAllInernalCargos = MenuArg.ReturnAllInernalCargos;
        this._DeclarationId = MenuArg.DeclarationId;
        this.UIProperties.SetEnabled("CustomFileNo", null, false);
        this.UIProperties.SetEnabled("Date", null, false);
        this.UIProperties.SetEnabled("MasterBillOfLading", null, false);
        this.UIProperties.SetEnabled("InternalIdentifier", null, false);
        this.UIProperties.SetEnabled("ReturnAllInernalCargos", null, false);
        var customSendOptionsArgs = new RequestParamsBase_1.CustomSendOptionsArgs();
        customSendOptionsArgs.ForcePersonalSign = false;
        customSendOptionsArgs.RequestVIA = RequestParamsBase_1.SendRequestVIA.WebServiceInteractive;
        this.OnCustomSendOptionsButtonClick(customSendOptionsArgs);
    };
    Object.defineProperty(MasterBOLQueryComponent.prototype, "Date", {
        //#region Properties
        get: function () { return this.RequestParams.Date; },
        set: function (value) {
            if (this.RequestParams.Date != value) {
                if (value) {
                    this.UIProperties.SetRequired("Date", null, false);
                }
                else {
                    this.UIProperties.SetRequired("Date", null, true);
                }
                this.RequestParams.Date = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterBOLQueryComponent.prototype, "MasterBillOfLading", {
        get: function () { return this.RequestParams.MasterBillOfLading; },
        set: function (value) {
            if (this.RequestParams.MasterBillOfLading != value) {
                this.RequestParams.MasterBillOfLading = value;
            }
            if (value) {
                this.UIProperties.SetRequired("MasterBillOfLading", null, false);
            }
            else {
                this.UIProperties.SetRequired("MasterBillOfLading", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterBOLQueryComponent.prototype, "InternalIdentifier", {
        get: function () { return this.RequestParams.InternalIdentifier; },
        set: function (value) {
            if (this.RequestParams.InternalIdentifier != value) {
                this.RequestParams.InternalIdentifier = value;
            }
            if (value) {
                this.UIProperties.SetRequired("InternalIdentifier", null, false);
            }
            else {
                this.UIProperties.SetRequired("InternalIdentifier", null, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterBOLQueryComponent.prototype, "ReturnAllInernalCargos", {
        get: function () { return this.RequestParams.ReturnAllInernalCargos; },
        set: function (value) {
            if (this.RequestParams.ReturnAllInernalCargos != value) {
                this.RequestParams.ReturnAllInernalCargos = value;
            }
            if (this.RequestParams.ReturnAllInernalCargos == true) {
                this.ExactMatch = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterBOLQueryComponent.prototype, "ExactMatch", {
        get: function () { return this.RequestParams.ExactMatch; },
        set: function (value) {
            if (this.RequestParams.ExactMatch != value) {
                this.RequestParams.ExactMatch = value;
            }
            if (this.RequestParams.ExactMatch == true) {
                this.ReturnAllInernalCargos = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterBOLQueryComponent.prototype, "IsFromDeclaration", {
        get: function () { return this._IsFromDeclaration; },
        set: function (value) {
            if (this._IsFromDeclaration != value) {
                this._IsFromDeclaration = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MasterBOLQueryComponent.prototype, "CustomFileNo", {
        get: function () { return this.RequestParams ? this.RequestParams.CustomFileNo : null; },
        set: function (value) {
            if (this.RequestParams.CustomFileNo != value) {
                this.RequestParams.CustomFileNo = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion Properties
    MasterBOLQueryComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        this.DueChangeClearChildField();
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, true);
        });
    };
    MasterBOLQueryComponent.prototype.DueChangeClearChildField = function () {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this._DeclarationId = null;
        this.Date = new Date().getFullYear();
        this.MasterBillOfLading = "";
        this.InternalIdentifier = "";
        this.ReturnAllInernalCargos = false;
        this.UIProperties.SetEnabled("Date", null, true);
        this.UIProperties.SetEnabled("MasterBillOfLading", null, true);
        this.UIProperties.SetEnabled("InternalIdentifier", null, true);
        this.UIProperties.SetEnabled("ReturnAllInernalCargos", null, true);
        this.ValidationErrorsList = [];
    };
    MasterBOLQueryComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            lastFetchDeclarationList = lastFetchDeclarationList[0];
            this._DeclarationId = lastFetchDeclarationList.DeclarationId;
            this.Date = lastFetchDeclarationList.ManifestNumber;
            this.MasterBillOfLading = lastFetchDeclarationList.SecondCargoID;
            this.InternalIdentifier = lastFetchDeclarationList.ThirdCargoID;
            //this.ReturnAllInernalCargos = lastFetchDeclarationList.ThirdCargoID ? false : true;
            this.ReturnAllInernalCargos = true; //task 44705 21.11.18
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetEnabled("Date", null, false);
            this.UIProperties.SetEnabled("MasterBillOfLading", null, false);
            this.UIProperties.SetEnabled("InternalIdentifier", null, false);
            this.UIProperties.SetEnabled("ReturnAllInernalCargos", null, false);
        }
        else {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
            this.ValidationErrorsList.push(msg);
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
        }
    };
    //#region General Commands
    MasterBOLQueryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    MasterBOLQueryComponent.prototype.FillErrors = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.Date)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.MasterBOLQuery.O.YearDateIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        else {
            if (((this.Date + '').length != 4) || isNaN(this.Date)) {
                var msg = "חובה להזין שנת טיסה בפורמט של 4 תווים";
                this.ValidationErrorsList.push(msg);
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.MasterBillOfLading)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.MasterBOLQuery.O.MasterBillOfLadingIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.InternalIdentifier) && this.ReturnAllInernalCargos != true) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.MasterBOLQuery.O.InternalIdentifierIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
    };
    MasterBOLQueryComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new MasterBOLQueryRequestParams_1.MasterBOLQueryRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.Date = this.RequestParams.Date;
        currRequestParams.MasterBillOfLading = this.RequestParams.MasterBillOfLading;
        currRequestParams.InternalIdentifier = this.RequestParams.InternalIdentifier;
        currRequestParams.ReturnAllInernalCargos = this.RequestParams.ReturnAllInernalCargos;
        currRequestParams.ExactMatch = this.RequestParams.ExactMatch;
        currRequestParams.DeclarationId = this._DeclarationId;
        currRequestParams.CustomFileNo = this.CustomFileNo;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לשטרי מטען", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostMasterBOLRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], MasterBOLQueryComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    MasterBOLQueryComponent = __decorate([
        core_1.Component({
            selector: 'MasterBOLQueryComponent',
            moduleId: module.id,
            templateUrl: './MasterBOLQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MasterBOLQueryComponent);
    return MasterBOLQueryComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.MasterBOLQueryComponent = MasterBOLQueryComponent;
//# sourceMappingURL=MasterBOLQueryComponent.js.map