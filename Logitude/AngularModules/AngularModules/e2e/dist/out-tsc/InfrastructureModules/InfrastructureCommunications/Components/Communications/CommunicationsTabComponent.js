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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CommunicationLogListService_1 = require("../../../../Common/Services/StandardLists/CommunicationLogListService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CommunicationsTabComponent = /** @class */ (function () {
    function CommunicationsTabComponent(entityArgs, entityResourceService) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.ObjectTableName = "CommunicationLog";
        this.IsTitleHidden = false;
        this.IsResourcesReady = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SessionEvent = null;
        this.ItemsSource = [];
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            _this.Listen();
            _this.InitTab();
        });
    }
    CommunicationsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (event) {
                if (event == "CommunicationRefresh") {
                    _this.LoadData();
                }
            });
        }
    };
    CommunicationsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
    };
    CommunicationsTabComponent.prototype.InitTab = function () {
        var _this = this;
        this.EntityId = this.entityArgs.EntityPM.Id;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        if (this.ObjectTableName == "Master") {
            this.ObjectTableName = "Shipment";
        }
        // Ayman:
        // we need this for Translation
        // Please don't remove it
        this.TabHeaderTextCode = this.ObjectTableName + ".TH.Communications";
        this.ObjectTableId = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0].Id;
        this.LoadData();
    };
    CommunicationsTabComponent.prototype.LoadData = function () {
        var _this = this;
        if (this.EntityId != null && this.ObjectTableId != null) {
            if (this.myService == null) {
                this.myService = new CommunicationLogListService_1.CommunicationLogListService();
            }
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 100;
            filters.SortBy = "CreateDate";
            filters.SortDirection = "Descending";
            filters.Filter1Name = "EntityId";
            filters.Filter1Value = this.EntityId;
            filters.Filter1Operator = "Equals";
            filters.Filter2Name = "ObjectTableId";
            filters.Filter2Value = this.ObjectTableId;
            filters.Filter2Operator = "Equals";
            this.myService.getByFilters(filters).subscribe(function (myResponse) {
                if (myResponse == null) {
                    _this.ItemsSource = [];
                }
                else {
                    _this.ItemsSource = myResponse.Result;
                }
            });
        }
    };
    CommunicationsTabComponent.prototype.RefreshButtonClicked = function () {
        this.LoadData();
    };
    CommunicationsTabComponent.prototype.ViewXMLClicked = function (item) {
        DownloadManager_1.DownloadManager.DownloadCommunicationLogXML(item);
    };
    CommunicationsTabComponent.prototype.EditItemClicked = function (item) {
        var entityId = item.Id;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'CommunicationLog' });
        });
    };
    CommunicationsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CommunicationsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], CommunicationsTabComponent);
    return CommunicationsTabComponent;
}());
exports.CommunicationsTabComponent = CommunicationsTabComponent;
//# sourceMappingURL=CommunicationsTabComponent.js.map