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
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var CountersDomainService_1 = require("../../../../Common/Services/CountersDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var CountersComponent = /** @class */ (function () {
    function CountersComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.Counters = [];
        this.IsDemoTenant = false;
        this.IsResourcesReady = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com") {
                this.IsDemoTenant = true;
            }
        }
    }
    CountersComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("CounterDefinition").subscribe(function (res1) {
            _this.IsResourcesReady = true;
            _this.LoadCounters();
        });
    };
    CountersComponent.prototype.LoadCounters = function () {
        var _this = this;
        var myService = new CountersDomainService_1.CountersDomainService();
        myService.GetTenantCounters().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var myCounters = [];
                var list = myResponse.Result;
                list.forEach(function (item) {
                    var ObjectTable = window.ObjectTables.filter(function (x) { return x.Id === item.ObjectTableId; })[0];
                    if (ObjectTable) {
                        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(ObjectTable.Name, "Module")) {
                            if (item.Code == "CNST") {
                                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent")) {
                                    myCounters.push(new CounterItem(item));
                                }
                            }
                            else if (item.Code == "CUST") {
                                if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM != null) {
                                    if (ObjectsLocator_1.ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                                        myCounters.push(new CounterItem(item));
                                    }
                                }
                            }
                            else {
                                myCounters.push(new CounterItem(item));
                            }
                        }
                    }
                });
                _this.Counters = myCounters.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
            }
        });
    };
    CountersComponent.prototype.ItemClicked = function (item) {
        if (item) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = { CounterId: item.CounterPM.Id, FatherComponent: this };
            logWindow.Title = item.Name + " Counters";
            switch (item.Code) {
                case "HAWB": {
                    logWindow.Width = 885;
                    logWindow.Height = 520;
                    logWindow.Title = "HAWB Counters";
                    logWindow.Show("./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CounterHAWBComponent");
                    break;
                }
                case "INVC": {
                    logWindow.IsFillScreen = true;
                    logWindow.Show("./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CounterInvoiceComponent");
                    break;
                }
                case "SHIP":
                case "MAST":
                case "QUOT": {
                    logWindow.IsFillScreen = true;
                    logWindow.Show("./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CounterAdvancedComponent");
                    break;
                }
                default: {
                    logWindow.Show("./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CounterTableComponent");
                    break;
                }
            }
        }
    };
    CountersComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CountersComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CountersComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], CountersComponent);
    return CountersComponent;
}());
exports.CountersComponent = CountersComponent;
var CounterItem = /** @class */ (function () {
    function CounterItem(itemPM) {
        this.Code = itemPM.Code;
        this.Name = itemPM.Name;
        this.CounterPM = itemPM;
    }
    return CounterItem;
}());
exports.CounterItem = CounterItem;
//# sourceMappingURL=CountersComponent.js.map