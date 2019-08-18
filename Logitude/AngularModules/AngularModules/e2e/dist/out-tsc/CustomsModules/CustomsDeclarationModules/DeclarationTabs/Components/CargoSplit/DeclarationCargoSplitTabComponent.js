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
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var DeclarationCargoSplitWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationCargoSplitWebService");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
;
var DeclarationCargoSplitPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationCargoSplitPMService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService"); //test4
var CargoSplitRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/CargoSplitRequestParams");
var INF_MSG_GenericResponseData_1 = require("../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData");
var EntityPMService_1 = require("../../../../../Infrastructure/Services/EntityPMService");
var DeclarationCargoSplitTabComponent = /** @class */ (function (_super) {
    __extends(DeclarationCargoSplitTabComponent, _super);
    function DeclarationCargoSplitTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = "Customs.Declaration";
        _this.DeclarationCargoSplitWebService = new DeclarationCargoSplitWebService_1.DeclarationCargoSplitWebService;
        _this.DeclarationCargoSplitPMService = new DeclarationCargoSplitPMService_1.DeclarationCargoSplitPMService;
        _this._DeclarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.requestParams = new CargoSplitRequestParams_1.CargoSplitRequestParams();
        _this.responseData = new INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData();
        _this.IsLoaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DeclarationCargoSplitList = new ObservableCollection_1.ObservableCollection([]);
        _this._EntityPMService = new EntityPMService_1.EntityPMService();
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(function (response) {
                _this.EntityPM = _this.entityArgs.EntityPM;
                _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                _this.LoadDeclarationCargoSplits();
                _this.Listen();
                _this.IsLoaded = true;
            });
        });
        return _this;
    }
    DeclarationCargoSplitTabComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
    };
    DeclarationCargoSplitTabComponent.prototype.Listen = function () {
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
                    _this.LoadDeclarationCargoSplits();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCCS") {
                        _this.LoadDeclarationCargoSplits();
                    }
                }
            }));
        }
    };
    DeclarationCargoSplitTabComponent.prototype.LoadDeclarationCargoSplits = function () {
        var _this = this;
        this.DeclarationCargoSplitList = new ObservableCollection_1.ObservableCollection([]);
        //this.DeclarationCargoSplitWebService.GetDeclarationCargoSplitByDeclarationIdLists(this.EntityPM.Id, this.EntityPM.Tenant)
        this._DeclarationWebService.GetDeclarationCargoSplitByDeclarationIdList(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.GetDeclarationCargoSplitByDeclarationIdListsOp_Completed(myResponse, false);
        });
    };
    DeclarationCargoSplitTabComponent.prototype.GetDeclarationCargoSplitByDeclarationIdListsOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        if (myResponse.Result != null) {
            //this.DeclarationCargoSplitList.InsertCollection(myResponse.Result);
            myResponse.Result.forEach(function (item) {
                _this.DeclarationCargoSplitList.Insert(item);
            });
        }
    };
    DeclarationCargoSplitTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    DeclarationCargoSplitTabComponent.prototype.EditButtonClickedOld = function (item) {
        var windowArgs = {};
        windowArgs.CurrentEntity = item;
        windowArgs.declarationPM = this.EntityPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 770;
        logWindow.Height = 750;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "בקשת פיצול מטען ";
        if (item != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.RequestNumber)) {
                logWindow.Title = logWindow.Title + item.RequestNumber;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(item.ResponseStatusName)) {
                logWindow.Title = logWindow.Title + " - " + item.ResponseStatusName;
            }
        }
        logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent');
        this.CurrentSession.StopBusyIndicator();
        /*if (!AppTool.IsNullOrEmpty(item)) {
            //this.CurrentSession.StartBusyIndicator("");
    
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    //this.showAlert = false;
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: 'Customs.DeclararionCargoSplit', BackButtonLabel: 'Declararion' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this.RefreshEntity();
                    });
                });
            
    
        }*/
    };
    DeclarationCargoSplitTabComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        var windowArgs = {};
        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this._EntityPMService.getSingle("Customs.DeclarationCargoSplit", item.Id).then(function (res) {
                    res.subscribe(function (myResponse) {
                        if (myResponse.HasError) {
                            console.log("Error while getting EntityPM", myResponse);
                        }
                        else {
                            windowArgs.CurrentEntity = myResponse.Result;
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 770;
                            logWindow.Height = 750;
                            //logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditDeclarationCargoSplit");
                            logWindow.Title = "בקשת פיצול מטען "; // + myResponse.Result != null ? ((!AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber) ? myResponse.Result.RequestNumber : null) + ((!AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName) ? " - " + myResponse.Result.ResponseStatusName : null))) : null;
                            if (myResponse.Result != null) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber)) {
                                    logWindow.Title = logWindow.Title + myResponse.Result.RequestNumber;
                                }
                                if (!Tools_1.AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName)) {
                                    logWindow.Title = logWindow.Title + " - " + myResponse.Result.ResponseStatusName;
                                }
                            }
                            logWindow.WindowArgs = windowArgs;
                            logWindow.ShowCloseButton = true;
                            //logWindow.IsHideHeader = true;
                            logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent');
                            logWindow.WindowClosed.subscribe(function ($event1) {
                                //this.isEditControlOpened = false;
                                //this.OnBackFromEdit(selectedEntityId, $event);
                            });
                        }
                    });
                });
            });
        });
    };
    DeclarationCargoSplitTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationCargoSplitTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], DeclarationCargoSplitTabComponent);
    return DeclarationCargoSplitTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationCargoSplitTabComponent = DeclarationCargoSplitTabComponent;
//# sourceMappingURL=DeclarationCargoSplitTabComponent.js.map