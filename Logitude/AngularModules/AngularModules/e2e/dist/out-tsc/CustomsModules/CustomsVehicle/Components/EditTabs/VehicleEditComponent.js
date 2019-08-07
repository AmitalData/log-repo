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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var EntityPMService_1 = require("../../../../Infrastructure/Services/EntityPMService");
var VehiclePM_1 = require("../../../../Customs/EntityPMs/VehiclePM");
var UpdateDeleteVehicleRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/UpdateDeleteVehicleRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var IIGGeneralMessagesService_1 = require("../../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var VehicleEditComponent = /** @class */ (function (_super) {
    __extends(VehicleEditComponent, _super);
    function VehicleEditComponent(entityArgs, entityPMService, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.entityPMService = entityPMService;
        _this.EntityResourceService = EntityResourceService;
        _this.ObjectTableName = "Customs.Vehicle";
        _this.DataContext = _this;
        _this.TabsItemsSource = [];
        _this.IsNewEntity = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.GENERAL = null;
        _this.MORE = null;
        _this.SAFETY = null;
        _this.CustomsRequestsSheets = null;
        _this.EntityPM = new VehiclePM_1.VehiclePM();
        var t = _this.EntityPM.VehicleOwners.length; //force load ?!?!?
        var t1 = _this.EntityPM.AddVehicleSafetyAccessory.length; //force load ?!?!?
        //this.entityArgs = new EntityArgs();
        _this.entityArgs.EntityPM = _this.EntityPM;
        _this.entityArgs.ObjectTableName = "Customs.Vehicle";
        _this.BuildTabs();
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            // this._IsLoaded = true;
            /// alert("this._IsLoaded");
        });
        return _this;
    }
    VehicleEditComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            //ListComponent  --args = NewEntityArgs {Perspective: null, QueryNameTextCode: "Customs.Vehicle.Q.Vehicles" }
            //this.EntityPM = args.EntityPM;
            //this.IsNewEntity = args.IsNewEntity;
        }
    };
    VehicleEditComponent.prototype.BuildTabs = function () {
        var _this = this;
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("General", "Customs.Vehicle.TH.General"));
        this.TabsItemsSource.push(new TabItem("VehicleMoreDetailsTabComponent", "Customs.Vehicle.TH.MoreDetails"));
        this.TabsItemsSource.push(new TabItem("VehiclesOwnersAndSafetyTabComponent", "Customs.Vehicle.TH.OwnersAndSafety"));
        this.timerToken = setTimeout(function () {
            _this.SelectedTabCode = "General"; // to ensure the component was painted
        }, 100);
    };
    Object.defineProperty(VehicleEditComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    VehicleEditComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {
                    case "General": {
                        if (this.GENERAL == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsVehicle/Components/EditTabs/VehicleGeneralComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.GENERAL = cmpRef.instance;
                                _this.GENERAL.SetTabArgs({ EntityPM: _this.EntityPM, IsNewEntity: _this.IsNewEntity });
                                _this.GENERAL.FillValidationErrorList.subscribe(function (response) {
                                    _this.ValidationErrorsList = response;
                                });
                            });
                        }
                        break;
                    }
                    case "VehicleMoreDetailsTabComponent": {
                        if (this.MORE == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsVehicle/Components/EditTabs/VehicleMoreDetailsTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.MORE = cmpRef.instance;
                                _this.MORE.SetTabArgs({ EntityPM: _this.EntityPM, IsNewEntity: _this.IsNewEntity });
                                _this.MORE.FillValidationErrorList.subscribe(function (response) {
                                    _this.ValidationErrorsList = response;
                                });
                            });
                        }
                        break;
                    }
                    case "VehiclesOwnersAndSafetyTabComponent": {
                        if (this.SAFETY == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsVehicle/Components/EditTabs/VehiclesOwnersAndSafetyTabComponent', myLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.SAFETY = cmpRef.instance;
                                _this.SAFETY.SetTabArgs({ EntityPM: _this.EntityPM, IsNewEntity: _this.IsNewEntity });
                                _this.SAFETY.FillValidationErrorList.subscribe(function (response) {
                                    _this.ValidationErrorsList = response;
                                });
                            });
                        }
                        break;
                    }
                }
            }
        }
    };
    //#endregion
    VehicleEditComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        //ev.Option;
        //ev.ForcePersonalSign
        //ev.RequestVIA
        this.SaveEntityChanges(customSendOptionsArgs);
    };
    VehicleEditComponent.prototype.OkButtonClicked = function () {
        //this.CurrentSession.CloseCurrentWindowEmit("Ok");
        this.SaveEntityChanges(null);
    };
    VehicleEditComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    VehicleEditComponent.prototype.SaveEntityChanges = function (customSendOptionsArgs) {
        var _this = this;
        this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.CancelButtonClicked();
            return;
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        //if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);
        this.entityPMService.insert(this.ObjectTableName, this.EntityPM).then(function (res) {
            res.subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    //this.SaveCompleted.emit(false);
                }
                else {
                    _this.EntityPM = myResponse.Result;
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Id)) {
                        var myErrors = [];
                        myErrors.push("this.EntityPM.Id is null");
                        _this.ValidationErrorsList = myErrors;
                    }
                    else {
                        if (customSendOptionsArgs == null) {
                            _this.CancelButtonClicked();
                        }
                        else {
                            var currRequestParams = new UpdateDeleteVehicleRequestParams_1.UpdateDeleteVehicleRequestParams(); ///Force new GUID On Each Send !!
                            currRequestParams.LoggingEnabled = true;
                            currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                            currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            currRequestParams.VehicleId = _this.EntityPM.Id;
                            currRequestParams.IsDelete = false;
                            CustomMessageProgressComponent_1.CustomMessageProgressComponent
                                .ShowProgressBar(currRequestParams.PBId, "שליחת מסר עדכון פרטי רכב", true)
                                .then(function (res) {
                                console.log(res);
                                _this.CancelButtonClicked();
                            }).catch(function (err) {
                                _this.ValidationErrorsList.push(err);
                                _this.CancelButtonClicked();
                            });
                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
                            myIIGGeneralMessagesService.PostVehicleRequest(currRequestParams)
                                .subscribe(function (myServiceResponse) {
                                //this.CurrentSession.StopBusyIndicator();
                                //this.ResponseData = myServiceResponse.Result;
                                //this.OnMassageDisplayMethod();
                            });
                        }
                    }
                }
            }, function (error) {
                _this.CurrentSession.StopBusyIndicator();
                var myErrors = [];
                myErrors.push(error.message);
                _this.ValidationErrorsList = myErrors;
                //this.SaveCompleted.emit(false);
            });
        });
        //}
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], VehicleEditComponent.prototype, "AllLocations", void 0);
    VehicleEditComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VehicleEditComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityPMService_1.EntityPMService, EntityResourceService_1.EntityResourceService])
    ], VehicleEditComponent);
    return VehicleEditComponent;
}(BaseComponent_1.BaseComponent));
exports.VehicleEditComponent = VehicleEditComponent;
var TabItem = /** @class */ (function () {
    function TabItem(code, textCode) {
        this.code = code;
        this.textCode = textCode;
    }
    return TabItem;
}());
//# sourceMappingURL=VehicleEditComponent.js.map