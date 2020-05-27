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
var PhysicalCheckWebService_1 = require("../../../../../Customs/Services/WebServices/PhysicalCheckWebService");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
;
var PhysicalCheckPMService_1 = require("../../../../../Customs/Services/StandardPMs/PhysicalCheckPMService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DeclarationPhysicalCheckTabComponent = /** @class */ (function (_super) {
    __extends(DeclarationPhysicalCheckTabComponent, _super);
    function DeclarationPhysicalCheckTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = "Customs.Declaration";
        _this.physicalCheckWebService = new PhysicalCheckWebService_1.PhysicalCheckWebService;
        _this.physicalCheckPMService = new PhysicalCheckPMService_1.PhysicalCheckPMService;
        _this.IsLoaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.physicalCheckList = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PhysicalCheck").subscribe(function (response) {
                _this.EntityPM = _this.entityArgs.EntityPM;
                _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                _this.LoadPhysicalChecks();
                _this.Listen();
                _this.IsLoaded = true;
            });
        });
        return _this;
    }
    DeclarationPhysicalCheckTabComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
    };
    DeclarationPhysicalCheckTabComponent.prototype.Listen = function () {
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
                    _this.LoadPhysicalChecks();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCPC") {
                        //this.LoadPhysicalChecks();
                    }
                }
            }));
        }
    };
    DeclarationPhysicalCheckTabComponent.prototype.LoadPhysicalChecks = function () {
        var _this = this;
        this.physicalCheckWebService.GetPhysicalCheckByDeclarationIdLists(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.GetPhysicalCheckByDeclarationIdListsOp_Completed(myResponse, false);
        });
    };
    DeclarationPhysicalCheckTabComponent.prototype.GetPhysicalCheckByDeclarationIdListsOp_Completed = function (myResponse, sourceIsCostomFile) {
        if (myResponse.Result != null) {
            this.physicalCheckList.InsertCollection(myResponse.Result);
            //myResponse.Result.forEach((item) => {
            //    this.physicalCheckList.Insert(item);
            //});
        }
    };
    DeclarationPhysicalCheckTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    DeclarationPhysicalCheckTabComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            //this.CurrentSession.StartBusyIndicator("");
            var miri = false;
            if (miri) {
                this.physicalCheckPMService.get(item.Id).subscribe(function (response) {
                    var windowArgs = {};
                    windowArgs.EntityPM = response.Result;
                    windowArgs.declarationPM = _this.EntityPM;
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 1030;
                    logWindow.Height = 600;
                    //windowArgs.IsDisplayOnly = this.IsDisplayOnly; /// to check?
                    //logWindow.ShowCloseButton = false;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.WindowClosed.subscribe(function ($event) {
                        _this.RefreshEntity();
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    });
                    logWindow.Show('./CustomsModules/CustomsPhysicalCheck/Components/EditTabs/General/PhysicalCheckGeneralTabComponent');
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            //var logWindow = new LogitudeWindow();
            //logWindow.Width = 1030;
            //logWindow.Height = 600;
            //logWindow.ShowEditComponent(item.id, "Customs.PhysicalCheck");
            ////logWindow.InjectEditComponent(item.id, "Customs.PhysicalCheck", logWindow);
            //logWindow.WindowClosed.subscribe(($event: any) => {
            //    this.RefreshEntity();
            //    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            //});
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                //this.showAlert = false;
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: 'Customs.PhysicalCheck', BackButtonLabel: 'Declararion' });
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    _this.RefreshEntity();
                });
            });
        }
    };
    DeclarationPhysicalCheckTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationPhysicalCheckTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], DeclarationPhysicalCheckTabComponent);
    return DeclarationPhysicalCheckTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationPhysicalCheckTabComponent = DeclarationPhysicalCheckTabComponent;
//# sourceMappingURL=DeclarationPhysicalCheckTabComponent.js.map