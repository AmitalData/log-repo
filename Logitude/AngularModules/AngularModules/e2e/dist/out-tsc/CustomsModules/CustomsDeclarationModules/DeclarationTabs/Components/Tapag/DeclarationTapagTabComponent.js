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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var TapagMessagesService_1 = require("../../../../../Customs/Services/WebServices/TapagMessagesService");
var TapagPMService_1 = require("../../../../../Customs/Services/StandardPMs/TapagPMService");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
;
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DeclarationTapagTabComponent = /** @class */ (function (_super) {
    __extends(DeclarationTapagTabComponent, _super);
    function DeclarationTapagTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = "Customs.Declaration";
        _this.tapagMessagesService = new TapagMessagesService_1.TapagMessagesService;
        _this.tapagPMService = new TapagPMService_1.TapagPMService;
        _this.IsLoaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.tapagObslist = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.TapagConnectionTable").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.Tapag").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.Deposit").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.DepositCondition").subscribe(function (response) {
                            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
                                _this.EntityResourceService.getEntityResourceByTableName("Customs.Guarantee").subscribe(function (response) {
                                    _this.EntityResourceService.getEntityResourceByTableName("Customs.GuaranteeCondition").subscribe(function (response) {
                                        _this.EntityResourceService.getEntityResourceByTableName("Customs.RequiredGuaranteeType").subscribe(function (response) {
                                            _this.EntityResourceService.getEntityResourceByTableName("Customs.Deficit").subscribe(function (response) {
                                                _this.EntityPM = _this.entityArgs.EntityPM;
                                                _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                                                _this.LoadTapagsList();
                                                _this.Listen();
                                                _this.IsLoaded = true;
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
        return _this;
    }
    DeclarationTapagTabComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
    };
    DeclarationTapagTabComponent.prototype.ngOnDestroy = function () {
        console.log("DeclarationTapagTabComponent:ngOnDestroy");
        this.entityArgs = null;
    };
    DeclarationTapagTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.LoadTapagsList();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCTP") {
                        _this.LoadTapagsList();
                    }
                }
            }));
        }
    };
    DeclarationTapagTabComponent.prototype.LoadTapagsList = function () {
        var _this = this;
        this.tapagObslist = new ObservableCollection_1.ObservableCollection([]);
        this.tapagMessagesService.GetDeclarationTapagsLists(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.GetDeclarationTapagsListsOp_Completed(myResponse, false);
        });
    };
    DeclarationTapagTabComponent.prototype.GetDeclarationTapagsListsOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        if (myResponse.Result != null) {
            myResponse.Result.forEach(function (item) {
                _this.tapagObslist.Insert(item);
            });
        }
    };
    DeclarationTapagTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    DeclarationTapagTabComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.tapagPMService.get(item.Id).subscribe(function (response) {
                _this.CurrentSession.StopBusyIndicator();
                switch (item.TapagTypeCode) {
                    case "1":
                        {
                            var windowArgs = {};
                            windowArgs.EntityPM = response.Result;
                            windowArgs.declarationPM = _this.EntityPM;
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 750;
                            logWindow.Height = 700;
                            logWindow.ShowCloseButton = true;
                            logWindow.WindowArgs = windowArgs;
                            //logWindow.Title = TextCodeTranslator.Translate("Customs.PaymentOrder.TH.Deficits");
                            logWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/Tapag/Deficit/PaymentOrderDeficitComponent');
                            _this.CurrentSession.StopBusyIndicator();
                            break;
                        }
                    case "2":
                    case "5":
                        {
                            var windowArgs = {};
                            windowArgs.EntityPM = response.Result;
                            windowArgs.declarationPM = _this.EntityPM;
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 750;
                            logWindow.Height = 800;
                            logWindow.ShowCloseButton = true;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/Tapag/Deposit/PaymentOrderDepositDataComponent');
                            _this.CurrentSession.StopBusyIndicator();
                            break;
                        }
                    case "4":
                    case "6":
                        {
                            var windowArgs = {};
                            windowArgs.EntityPM = response.Result;
                            windowArgs.declarationPM = _this.EntityPM;
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 750;
                            logWindow.Height = 800;
                            logWindow.ShowCloseButton = true;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Guarantee.O.Guarantee");
                            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Tapag/GuaranteeDataComponent');
                            _this.CurrentSession.StopBusyIndicator();
                            break;
                        }
                    default:
                        {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Width = 350;
                            messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                            break;
                        }
                }
            });
        }
    };
    DeclarationTapagTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationTapagTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], DeclarationTapagTabComponent);
    return DeclarationTapagTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationTapagTabComponent = DeclarationTapagTabComponent;
//# sourceMappingURL=DeclarationTapagTabComponent.js.map