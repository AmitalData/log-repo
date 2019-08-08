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
var VendorEditComponent = /** @class */ (function (_super) {
    __extends(VendorEditComponent, _super);
    function VendorEditComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Customs.CustomsVendor";
        _this.DataContext = _this;
        _this.TabsItemsSource = [];
        _this.IsNewEntity = false;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.GENERAL = null;
        _this.COMMUNICATION = null;
        _this.EVENTS = null;
        _this.REQUESTSHEET = null;
        _this.CustomsRequestsSheets = null;
        _this.BuildTabs();
        return _this;
    }
    VendorEditComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.IsNewEntity = args.IsNewEntity;
        }
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.CustomsVendor";
    };
    VendorEditComponent.prototype.BuildTabs = function () {
        var _this = this;
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("General", "Customs.Vendor.TH.General"));
        this.TabsItemsSource.push(new TabItem("COMMUNICATION", "Customs.Vendor.TH.Communications"));
        this.TabsItemsSource.push(new TabItem("EVENTS", "Customs.Vendor.TH.Events"));
        this.TabsItemsSource.push(new TabItem("REQUESTSHEET", "General.MH.CustomsRequestsSheets"));
        this.timerToken = setTimeout(function () {
            _this.SelectedTabCode = "General"; // to ensure the component was painted
        }, 100);
    };
    Object.defineProperty(VendorEditComponent.prototype, "SelectedTabCode", {
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
    VendorEditComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedTabCode; })[0];
            if (myLocation_1 != null) {
                switch (this.SelectedTabCode) {
                    case "General": {
                        if (this.GENERAL == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsVendor/Components/EditTabs/General/VendorGeneralTabComponent', myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.GENERAL = cmpRef.instance;
                                _this.GENERAL.SetTabArgs({ EntityPM: _this.EntityPM, IsNewEntity: _this.IsNewEntity });
                                _this.GENERAL.FillValidationErrorList.subscribe(function (response) {
                                    _this.ValdationErrorList = response;
                                });
                            });
                        }
                        break;
                    }
                    //case "Communications": {
                    //    if (this.Communications == null) {
                    //        SessionLocator.DynamicLoader.Load('./Customs/Components/Vendors/EditTabs/Communications/VendorCommunicationsTabComponent',
                    //            myLocation.viewContainerRef)
                    //            .then(cmpRef => {
                    //                this.Communications = cmpRef.instance;
                    //                this.Communications.SetTabArgs({ EntityPM: this.EntityPM, IsNewEntity: this.IsNewEntity});
                    //            });
                    //    }
                    //    break;
                    //}
                    case "COMMUNICATION": {
                        if (this.COMMUNICATION == null) {
                            this.entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe(function (response) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent", myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.COMMUNICATION = cmpRef.instance;
                                    _this.COMMUNICATION.IsTitleHidden = false;
                                    _this.COMMUNICATION.InitTab();
                                });
                            });
                        }
                        break;
                    }
                    case "EVENTS": {
                        if (this.EVENTS == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Common/Components/Events/EventsTabComponent", myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.EVENTS = cmpRef.instance;
                            });
                        }
                        break;
                    }
                    case "REQUESTSHEET": {
                        if (this.REQUESTSHEET == null) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load("./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent", myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.REQUESTSHEET = cmpRef.instance;
                                _this.REQUESTSHEET.IsTitleHidden = false;
                                _this.REQUESTSHEET.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                            });
                        }
                        break;
                    }
                }
            }
        }
    };
    //#endregion
    VendorEditComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    };
    VendorEditComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], VendorEditComponent.prototype, "AllLocations", void 0);
    VendorEditComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VendorEditComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], VendorEditComponent);
    return VendorEditComponent;
}(BaseComponent_1.BaseComponent));
exports.VendorEditComponent = VendorEditComponent;
var TabItem = /** @class */ (function () {
    function TabItem(code, textCode) {
        this.code = code;
        this.textCode = textCode;
    }
    return TabItem;
}());
//# sourceMappingURL=VendorEditComponent.js.map