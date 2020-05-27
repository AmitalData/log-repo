"use strict";
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
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ContainersFUDomainService_1 = require("../../Services/ContainersFUDomainService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Args_1 = require("../../../Infrastructure/Args");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var ContainersFUsComponent = /** @class */ (function () {
    function ContainersFUsComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.ReloadUserQueries = new core_1.EventEmitter();
        this.IsResourcesReady = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsQueryVisible_InTransit = false;
        this.IsQueryVisible_ArrivedNotDelivered = false;
        this.IsQueryVisible_DeliveredNotReturned = false;
        this.IsQueryVisible_MyViewsGroup = false;
        this.myDomainService = new ContainersFUDomainService_1.ContainersFUDomainService();
        this.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.ContainersFU");
    }
    ContainersFUsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("ContainerFollowUp", 0).subscribe(function (response) {
            _this.IsResourcesReady = true;
            _this.LoadAllScreenData();
            _this.SetQueriesVisibility();
        });
    };
    ContainersFUsComponent.prototype.EditShipment = function (entity) {
    };
    ContainersFUsComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    ContainersFUsComponent.prototype.LoadAllScreenData = function () {
        this.LoadQueriesCounts();
        this.ReloadUsersQuery();
    };
    ContainersFUsComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    ContainersFUsComponent.prototype.SetQueriesVisibility = function () {
        this.IsQueryVisible_InTransit = FeatureLocator_1.FeatureLocator.HasFeaturePermession("ContainerFollowUp", "InTransit") ? true : false;
        this.IsQueryVisible_ArrivedNotDelivered = FeatureLocator_1.FeatureLocator.HasFeaturePermession("ContainerFollowUp", "ArrivedNotDelivered") ? true : false;
        this.IsQueryVisible_DeliveredNotReturned = FeatureLocator_1.FeatureLocator.HasFeaturePermession("ContainerFollowUp", "DeliveredNotReturned") ? true : false;
        this.IsQueryVisible_MyViewsGroup = FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
    };
    ContainersFUsComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this.myDomainService.GetQueriesCounts().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;
                    if (myResult != null) {
                        _this.InTransit = myResult.InTransit > 1000 ? "1000+" : myResult.InTransit.toString();
                        _this.ArrivedNotDelivered = myResult.ArrivedNotDelivered > 1000 ? "1000+" : myResult.ArrivedNotDelivered.toString();
                        _this.DeliveredNotReturned = myResult.DeliveredNotReturned > 1000 ? "1000+" : myResult.DeliveredNotReturned.toString();
                    }
                }
            }
        });
    };
    //filterAgrs: ApiQueryFilters;
    ContainersFUsComponent.prototype.ViewQuery = function (myQueryCode) {
        var _this = this;
        if (myQueryCode != null) {
            switch (myQueryCode) {
                case "ArrivedNotDelivered": {
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Container F/U", "Arrived Not Delivered View");
                    break;
                }
                case "DeliveredNotReturned": {
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Container F/U", "Delivered Not Returned View");
                    break;
                }
                case "InTransit": {
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Container F/U", "In Transit View");
                    break;
                }
            }
            var queryCode = myQueryCode;
            var objectTableName = "ContainerFollowUp";
            //var MethodName = null;
            //var displayTitle = null;
            //var backButtonTitle = TextCodeTranslator.Translate("General.MH.ContainersFU");
            //this.filterAgrs = new ApiQueryFilters();
            var listArgs = new Args_1.ListComponentArgs();
            //listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = objectTableName;
            //listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = this.BackButtonTitle;
            //listArgs.MethodName = MethodName;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.LoadAllScreenData();
                });
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ContainersFUsComponent.prototype, "ReloadUserQueries", void 0);
    ContainersFUsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContainersFUsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], ContainersFUsComponent);
    return ContainersFUsComponent;
}());
exports.ContainersFUsComponent = ContainersFUsComponent;
//# sourceMappingURL=ContainersFUsComponent.js.map