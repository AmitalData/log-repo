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
var DeclarationRestoreRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/DeclarationRestoreRequestParams");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseRequestsSheetMassaging_1 = require("../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DeclarationDisplayOnlyChecks_1 = require("../../../../Customs/Utilities/DeclarationDisplayOnlyChecks");
var DeclarationRestoreComponent = /** @class */ (function (_super) {
    __extends(DeclarationRestoreComponent, _super);
    function DeclarationRestoreComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._DeclarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        _this._MyResponseObjectToShow = null;
        _this._UserMessagehidden = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.UIProperties.SetRequired("DeclarationNumber", _this.ObjectTableName, true);
        return _this;
    }
    DeclarationRestoreComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    DeclarationRestoreComponent.prototype.SetMenuArg = function (MenuArg) {
        this.RequestParams = MenuArg;
        this.OnMassageDisplayMethod();
        //this.CustomFileNo = MenuArg.CustomFileNo;
        //this.DeclarationNumber= MenuArg.DeclarationNumber;
    };
    DeclarationRestoreComponent.prototype.DueChangeClearChildField = function (sourceIsCostomFile) {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        }
        else {
            this.CustomFileNo = "";
        }
        this.DeclarationId = "";
        this.ResponseData = null;
        this._LastFetchDeclarationList = null;
        this.ValidationErrorsList = [];
    };
    DeclarationRestoreComponent.prototype.CustomFileNoTextChanged = function (searchtext) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }
        if (this._LastFetchDeclarationList != null) {
            if (this.CustomFileNo == this._LastFetchDeclarationList.CustomFileNo) {
                return;
            }
        }
        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, true);
        });
    };
    DeclarationRestoreComponent.prototype.DeclarationNumberTextChanged = function (DeclarationNumberText) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }
        if (this._LastFetchDeclarationList != null) {
            if (this.DeclarationNumber == this._LastFetchDeclarationList.DeclarationNumber) {
                return;
            }
        }
        this.DueChangeClearChildField(false);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.FetchDeclaration(myResponse, false);
        });
    };
    DeclarationRestoreComponent.prototype.FetchDeclaration = function (myResponse, sourceIsCostomFile) {
        this._LastFetchDeclarationList = myResponse.Result;
        if (this._LastFetchDeclarationList != null) {
            this.DeclarationId = this._LastFetchDeclarationList.Id;
            this.DeclarationNumber = this._LastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = this._LastFetchDeclarationList.CustomFileNo;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
        }
        else {
            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            }
            //else {
            //    this.SetValidityDeclarationNumber();
            //}
        }
    };
    DeclarationRestoreComponent.prototype.SetValidityDeclarationNumber = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    };
    DeclarationRestoreComponent.prototype.SetValidityCustomFileNo = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    };
    Object.defineProperty(DeclarationRestoreComponent.prototype, "CustomFileNo", {
        get: function () { return this.RequestParams ? this.RequestParams.CustomsFile : null; },
        set: function (value) {
            if (this.RequestParams.CustomsFile != value) {
                this.RequestParams.CustomsFile = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationRestoreComponent.prototype, "DeclarationNumber", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationNumber : null; },
        set: function (value) {
            if (this.RequestParams.DeclarationNumber != value) {
                this.RequestParams.DeclarationNumber = value;
                if (value) {
                    this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
                }
            }
            else {
                this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationRestoreComponent.prototype, "DeclarationId", {
        get: function () { return this.RequestParams ? this.RequestParams.DeclarationId : null; },
        set: function (value) {
            if (this.RequestParams.DeclarationId != value) {
                this.RequestParams.DeclarationId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    DeclarationRestoreComponent.prototype.RefreshScreen = function () {
        this._MyResponseObjectToShow = null;
        this._UserMessagehidden = true;
        if (this.ResponseData == null) {
            return;
        }
        this._UserMessagehidden = !this.ResponseData.IsShowUserMessage;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ResponseData.ResponseStatusXML)) {
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML);
            }
            catch (err) {
                console.log(err);
            }
        }
    };
    DeclarationRestoreComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
            this.RequestParams = new DeclarationRestoreRequestParams_1.DeclarationRestoreRequestParams();
        }
        this.RefreshScreen();
    };
    DeclarationRestoreComponent.prototype.CheckDeclarationPayment = function () {
        if (this._LastFetchDeclarationList.PaymentDate && Tools_1.AppTool.IsNullOrEmpty(this._LastFetchDeclarationList.CorrectionsXml)) { // if declaration was already paid
            var confirm = new ConfirmWindow_1.ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show("הצהרה זו כבר שולמה, האם למחוק נתוני הגשה ולשחזר אותם מחדש");
            confirm.WindowClosed.subscribe(function (event) {
                confirm.Close();
                if (confirm.Yes) {
                    //this.DeleteDeclarationPayment();
                }
            });
        }
    };
    DeclarationRestoreComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (Tools_1.AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
            this.ValidationErrorsList.push(msg);
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
            declarationDisplayOnlyChecks.CheckIfRequestInProgress("2715", this.CustomFileNo, SessionLocator_1.SessionLocator.Tenant)
                .subscribe(function (response) {
                if (!response.HasError) {
                    var requestSheets = response.Result;
                    var haveRS2715 = false;
                    if ((requestSheets == null || requestSheets.length == 0)
                        || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                        haveRS2715 = false;
                    }
                    else {
                        haveRS2715 = true;
                    }
                    if (haveRS2715) {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 150;
                        messageWindow.Title = "שיחזור מספר הצהרה";
                        messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הצהרה בתהליך ");
                        return;
                    }
                    declarationDisplayOnlyChecks.CheckIfRequestInProgress("2755", _this.CustomFileNo, SessionLocator_1.SessionLocator.Tenant)
                        .subscribe(function (response) {
                        if (!response.HasError) {
                            var requestSheets = response.Result;
                            var haveRS2755 = false;
                            if ((requestSheets == null || requestSheets.length == 0)
                                || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                                haveRS2755 = false;
                            }
                            else {
                                haveRS2755 = true;
                            }
                            if (haveRS2755) {
                                var messageWindow = new MessageWindow_1.MessageWindow();
                                messageWindow.Width = 400;
                                messageWindow.Height = 150;
                                messageWindow.Title = "שיחזור מספר הצהרה";
                                messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הגשת תשלום ");
                                return;
                            }
                            _this.SendDeclarationRestoreRequest(customSendOptionsArgs);
                        }
                    });
                }
            });
        }
        else {
            this.SendDeclarationRestoreRequest(customSendOptionsArgs);
        }
    };
    DeclarationRestoreComponent.prototype.SendDeclarationRestoreRequest = function (customSendOptionsArgs) {
        var _this = this;
        var currRequestParams = new DeclarationRestoreRequestParams_1.DeclarationRestoreRequestParams(); ///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.AppicationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationNumber = this.RequestParams.DeclarationNumber;
        currRequestParams.CustomsFile = this.RequestParams.CustomsFile;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא לשיחזור נתוני הצהרה", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._DeclarationMessagesService.PostDeclarationRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], DeclarationRestoreComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    DeclarationRestoreComponent = __decorate([
        core_1.Component({
            selector: 'DeclarationRestoreComponent',
            moduleId: module.id,
            templateUrl: './DeclarationRestoreComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeclarationRestoreComponent);
    return DeclarationRestoreComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.DeclarationRestoreComponent = DeclarationRestoreComponent;
//# sourceMappingURL=DeclarationRestoreComponent.js.map