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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ClientPM_1 = require("../../../../Customs/EntityPMs/ClientPM");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ClientSearchRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/ClientSearchRequestParams");
var ClientSearchResponseData_1 = require("../../../../Customs/DataContract/ResponseData/ClientSearchResponseData");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CustomMessageProgressComponent_1 = require("../../../CustomsControls/Components/CustomMessageProgressComponent");
var ClientMessagesService_1 = require("../../../../Customs/Services/WebServices/ClientMessagesService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var RequestParamsBase_1 = require("../../../../Customs/DataContract/RequestParams/RequestParamsBase");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var NewClientComponent = /** @class */ (function (_super) {
    __extends(NewClientComponent, _super);
    function NewClientComponent(EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.EntityResourceService = EntityResourceService;
        _this.ObjectTableName = "Customs.Client";
        _this.DataContext = _this;
        _this._MyResponseObjectToShow = null;
        _this.ValidationErrorsList = [];
        _this.clientMessagesService = new ClientMessagesService_1.ClientMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // #region properties
        _this._IsFromDeclaration = false;
        _this.clientPM = new ClientPM_1.ClientPM();
        _this.clientPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.IsExternalId = true;
        _this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", false);
        _this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", false);
        _this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", false);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Client").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(function (response) {
            });
        });
        return _this;
    }
    NewClientComponent.prototype.SetWindowArgs = function (menuArg) {
        this.OnMassageDisplayMethod();
        if (menuArg.Mode == "DeclarationGeneralComponent") {
            this.IsFromDeclaration = true;
            this.Code = menuArg.ImporterCode;
            this.IsExternalId = menuArg.IsExternalId;
            this.IsPassport = menuArg.IsPassport;
            this.PassportNumber = menuArg.PassportNumber;
            this.PassportTypeCode = menuArg.PassportTypeCode;
            this.PassportCountryCode = menuArg.PassportCountryCode;
            //if (!AppTool.IsNullOrEmpty(this.Code) && AppTool.IsNullOrEmpty(this.PassportNumber)) {
            //    this.UIProperties.SetEnabled("Code", "Customs.Client", false);
            //    this.UIProperties.SetEnabled("FullName", "Customs.Client", false);
            //    this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", false);
            //    this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", false);
            //    this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", false);
            //}
            var customSendOptionsArgs = new RequestParamsBase_1.CustomSendOptionsArgs();
            customSendOptionsArgs.ForcePersonalSign = false;
            customSendOptionsArgs.RequestVIA = RequestParamsBase_1.SendRequestVIA.WebServiceInteractive;
            this.OnCustomSendOptionsButtonClick(customSendOptionsArgs);
        }
    };
    Object.defineProperty(NewClientComponent.prototype, "IsFromDeclaration", {
        get: function () { return this._IsFromDeclaration; },
        set: function (value) {
            if (this._IsFromDeclaration != value) {
                this._IsFromDeclaration = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewClientComponent.prototype, "IsPassport", {
        get: function () { return this.isPassport; },
        set: function (value) {
            if (this.isPassport != value) {
                this.isPassport = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewClientComponent.prototype, "Code", {
        get: function () { return this.clientPM.Code; },
        set: function (value) {
            if (this.clientPM.Code != value) {
                this.clientPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewClientComponent.prototype, "PassportNumber", {
        get: function () { return this.clientPM.PassportNumber; },
        set: function (value) {
            if (this.clientPM.PassportNumber != value) {
                this.clientPM.PassportNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewClientComponent.prototype, "PassportTypeCode", {
        get: function () { return this.clientPM.PassportTypeCode; },
        set: function (value) {
            if (this.clientPM.PassportTypeCode != value) {
                this.clientPM.PassportTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewClientComponent.prototype, "PassportCountryCode", {
        get: function () { return this.clientPM.PassportCountryCode; },
        set: function (value) {
            if (this.clientPM.PassportCountryCode != value) {
                this.clientPM.PassportCountryCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewClientComponent.prototype, "FullName", {
        get: function () { return this.clientPM.FullName; },
        set: function (value) {
            if (this.clientPM.FullName != value) {
                this.clientPM.FullName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewClientComponent.prototype, "IsExternalId", {
        get: function () { return this.isExternalId; },
        set: function (value) {
            if (this.isExternalId != value) {
                this.isExternalId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    //        public string ResponseStatusXML
    //{
    //    get { return GetXml(); }
    //    get { return ResponseData.ResponseStatusXML; }
    //    set { FirePropertyChanged("ResponseStatusXML"); }
    //}
    //#endregion
    NewClientComponent.prototype.UseExternalId = function () {
        this.UIProperties.SetEnabled("Code", "Customs.Client", true);
        //  this.UIProperties.SetEnabled("FullName", "Customs.Client", true);
        this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", false);
        this.IsExternalId = true;
        this.IsPassport = false;
    };
    NewClientComponent.prototype.UsePassportRadio = function () {
        this.UIProperties.SetEnabled("Code", "Customs.Client", false);
        // this.UIProperties.SetEnabled("FullName", "Customs.Client", false);
        this.UIProperties.SetEnabled("PassportCountryCode", "Customs.Client", true);
        this.UIProperties.SetEnabled("PassportTypeCode", "Customs.Client", true);
        this.UIProperties.SetEnabled("PassportNumber", "Customs.Client", true);
        this.IsExternalId = false;
        this.IsPassport = true;
    };
    NewClientComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        //  alert(customSendOptionsArgs);
        var _this = this;
        var errors = [];
        this.ValidationErrorsList = [];
        Validator_1.Validator.TryValidateObject(this.clientPM, this.ObjectTableName, errors);
        if (this.IsExternalId && Tools_1.AppTool.IsNullOrEmpty(this.Code)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.CodeRequired");
            this.ValidationErrorsList.push(msg);
        }
        if (this.IsPassport && Tools_1.AppTool.IsNullOrEmpty(this.PassportNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.PassportRequired");
            this.ValidationErrorsList.push(msg);
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new ClientSearchRequestParams_1.ClientSearchRequestParams(); ///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.LoggingEntityId = this.clientPM.Id;
        currRequestParams.LoggingEntityReference = this.PassportNumber;
        currRequestParams.LoggingObjectTableId = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Client'; })[0].Id;
        currRequestParams.ExternalId = this.Code;
        currRequestParams.PassportCountryCode = this.PassportCountryCode;
        currRequestParams.PassportNumber = this.PassportNumber;
        currRequestParams.PassportTypeCode = this.PassportTypeCode;
        currRequestParams.Tenant = this.clientPM.Tenant;
        currRequestParams.RequestName = "Client Search";
        currRequestParams.ResponseName = "Client Search";
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לשליפת לקוח", false)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this.clientMessagesService.PostClientRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
            if (myServiceResponse.Result) {
                var response = myServiceResponse.Result;
                if (!response.HasException && response.Succeeded || response.CanContinue) {
                    if (response.CanContinue) {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Width = 400;
                        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.ClientScreen");
                        confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.Cancel");
                        confirmWindow.Height = 180;
                        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.BuildClient");
                        confirmWindow.ShowNoButton = true;
                        confirmWindow.Show(response.UserMessage);
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                _this.CreateNewClient();
                                confirmWindow.Close();
                            }
                        });
                    }
                }
            }
        });
    };
    NewClientComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewClientComponent.prototype.OkButtonClicked = function () {
        if (this.IsFromDeclaration) {
            if (this._MyResponseObjectToShow == null) {
                this.CurrentSession.CloseCurrentWindowEmit("");
            }
            else {
                this.CurrentSession.CloseCurrentWindowEmit(this._MyResponseObjectToShow.FullName);
            }
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    NewClientComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.requestParams == null) {
            this.requestParams = new ClientSearchRequestParams_1.ClientSearchRequestParams();
        }
        if (this.ResponseData == null) {
            this.ResponseData = new ClientSearchResponseData_1.ClientSearchResponseData();
        }
        this.RefreshScreen();
    };
    NewClientComponent.prototype.RefreshScreen = function () {
        this._MyResponseObjectToShow = null;
        //     this._UserMessagehidden = true;
        if (this.ResponseData == null) {
            return;
        }
        //  this._UserMessagehidden = !this.ResponseData.IsShowUserMessage;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ResponseData.ResponseStatusXML)) {
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML);
            }
            catch (err) {
                console.log(err);
            }
        }
    };
    NewClientComponent.prototype.CreateNewClient = function () {
        //var item = new ClientPM();
        //   item.Tenant = SessionLocator.Tenant;
        var windowArgs = {};
        windowArgs.isNewClient = true;
        windowArgs.CurrentEntity = this.clientPM;
        windowArgs.IsExternalId = this.IsExternalId;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Client.O.BuildClient");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/ClientEditComponent');
    };
    NewClientComponent = __decorate([
        core_1.Component({
            selector: 'NewClientComponent',
            moduleId: module.id,
            templateUrl: './NewClientComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewClientComponent);
    return NewClientComponent;
}(BaseComponent_1.BaseComponent));
exports.NewClientComponent = NewClientComponent;
//# sourceMappingURL=NewClientComponent.js.map