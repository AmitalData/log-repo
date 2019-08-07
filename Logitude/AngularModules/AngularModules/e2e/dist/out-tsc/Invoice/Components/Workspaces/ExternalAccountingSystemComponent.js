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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var AccountingSettingPMService_1 = require("../../../Common/Services/StandardPMs/AccountingSettingPMService");
var GlobalDomainService_1 = require("../../../Common/Services/GlobalDomainService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ObjectsUpdater_1 = require("../../../Infrastructure/Locators/ObjectsUpdater");
var ExternalAccountingSystemComponent = /** @class */ (function (_super) {
    __extends(ExternalAccountingSystemComponent, _super);
    function ExternalAccountingSystemComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "AccountingSetting";
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isQBO = false;
        _this.isLogedInQBO = false;
        _this.IsQuickBooksWindowOpened = false;
        entityResourceService.getEntityResourceByTableName("AccountingSetting").subscribe(function (res) {
            _this.InitializeServices();
            _this.LoadData();
        });
        return _this;
    }
    ExternalAccountingSystemComponent.prototype.InitializeServices = function () {
        this.entityPMService = new AccountingSettingPMService_1.AccountingSettingPMService();
        this.myGlobalDomainService = new GlobalDomainService_1.GlobalDomainService();
    };
    ExternalAccountingSystemComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.entityPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.EntityPM = myResponse.Result;
            }
            _this.IsResourcesReady = true;
            _this.SetUIProperties();
            _this.SetQuickBookProperties();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ExternalAccountingSystemComponent.prototype.SetUIProperties = function () {
    };
    Object.defineProperty(ExternalAccountingSystemComponent.prototype, "AccountingSystemCode", {
        get: function () { return this.EntityPM.AccountingSystemCode; },
        set: function (value) {
            if (this.EntityPM.AccountingSystemCode != value) {
                this.EntityPM.AccountingSystemCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ExternalAccountingSystemComponent.prototype.SetQuickBookProperties = function () {
        if (this.EntityPM.QBOAccessToken != null) {
            this.isLogedInQBO = true;
        }
        else {
            this.isLogedInQBO = false;
        }
    };
    ExternalAccountingSystemComponent.prototype.DissConnectQBO = function () {
        var _this = this;
        this.EntityPM.QBOrealMeID = null;
        this.EntityPM.QBOAccessToken = null;
        this.EntityPM.QBOAccessTokenSecret = null;
        this.CurrentSession.StartBusyIndicator("Disconnecting..");
        this.entityPMService.update(this.EntityPM).subscribe(function (myResponse1) {
            if (myResponse1.HasError) {
                _this.ValidationErrorsList = myResponse1.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                ObjectsUpdater_1.ObjectsUpdater.UpdateAccountingSettingPM(_this.EntityPM);
                _this.myGlobalDomainService.GetAccountingSystem(_this.AccountingSystemCode).subscribe(function (myResponse2) {
                    if (!myResponse2.HasError) {
                        SessionLocator_1.SessionLocator.AccountingSystemPM = myResponse2.Result;
                    }
                });
                _this.SetQuickBookProperties();
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    ExternalAccountingSystemComponent.prototype.ViewXMLClicked = function () {
        var link = Tools_1.AppTool.GetLogitudeURL() + "Quickbooksonline.aspx?connect=true&tenant=" + SessionLocator_1.SessionLocator.Tenant;
        window.open(link, '_blank', "location = 1, status = 1, scrollbars = 1, width = 400, height = 400");
        this.IsQuickBooksWindowOpened = true;
    };
    ExternalAccountingSystemComponent.prototype.SelectedItemChanged = function (AccountingSystem) {
        if (AccountingSystem.Code == "QBO") {
            this.isQBO = true;
        }
        else {
            this.isQBO = false;
        }
    };
    ExternalAccountingSystemComponent.prototype.RunConnectQuickBooks = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "";
        logWindow.Show('./Invoice/Components/Workspaces/QuickBooksLogin');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                //this.LoadAllScreenData();
            }
        });
    };
    ExternalAccountingSystemComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        if (this.AccountingSystemCode != "QBO") {
            if (this.EntityPM.QBOrealMeID) {
                this.EntityPM.QBOrealMeID = null;
            }
            if (this.EntityPM.QBOAccessToken) {
                this.EntityPM.QBOAccessToken = null;
            }
            if (this.EntityPM.QBOAccessTokenSecret) {
                this.EntityPM.QBOAccessTokenSecret = null;
            }
        }
        if (this.IsQuickBooksWindowOpened && this.AccountingSystemCode == "QBO") {
            this.entityPMService.get(SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    var loadedEntity = myResponse.Result;
                    if (_this.EntityPM.QBOrealMeID != loadedEntity.QBOrealMeID) {
                        _this.EntityPM.QBOrealMeID = loadedEntity.QBOrealMeID;
                    }
                    if (_this.EntityPM.QBOAccessToken != loadedEntity.QBOAccessToken) {
                        _this.EntityPM.QBOAccessToken = loadedEntity.QBOAccessToken;
                    }
                    if (_this.EntityPM.QBOAccessTokenSecret != loadedEntity.QBOAccessTokenSecret) {
                        _this.EntityPM.QBOAccessTokenSecret = loadedEntity.QBOAccessTokenSecret;
                    }
                    _this.SaveChanges();
                }
            });
        }
        else {
            this.SaveChanges();
        }
    };
    ExternalAccountingSystemComponent.prototype.SaveChanges = function () {
        var _this = this;
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            this.entityPMService.update(this.EntityPM).subscribe(function (myResponse1) {
                if (myResponse1.HasError) {
                    _this.ValidationErrorsList = myResponse1.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    ObjectsUpdater_1.ObjectsUpdater.UpdateAccountingSettingPM(_this.EntityPM);
                    _this.myGlobalDomainService.GetAccountingSystem(_this.AccountingSystemCode).subscribe(function (myResponse2) {
                        if (!myResponse2.HasError) {
                            SessionLocator_1.SessionLocator.AccountingSystemPM = myResponse2.Result;
                        }
                    });
                    _this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            });
        }
    };
    ExternalAccountingSystemComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ExternalAccountingSystemComponent = __decorate([
        core_1.Component({
            selector: 'AccountingTransferComponent',
            moduleId: module.id,
            templateUrl: './ExternalAccountingSystemComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], ExternalAccountingSystemComponent);
    return ExternalAccountingSystemComponent;
}(BaseComponent_1.BaseComponent));
exports.ExternalAccountingSystemComponent = ExternalAccountingSystemComponent;
//# sourceMappingURL=ExternalAccountingSystemComponent.js.map