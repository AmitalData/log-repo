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
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var InvoiceComponent = /** @class */ (function () {
    function InvoiceComponent() {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.isRTL = false;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.Page_AR = null;
        this.Page_AP = null;
        this.Page_AT = null;
        this.Page_SET = null;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        this.RunComponent();
    }
    InvoiceComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.SelectedItem = "RECEIVABLE";
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    InvoiceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    Object.defineProperty(InvoiceComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            if (this.selectedItem != newValue) {
                this.selectedItem = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    InvoiceComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {
                var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedItem; })[0];
                if (myLocation_1 != null) {
                    switch (this.SelectedItem) {
                        case "RECEIVABLE": {
                            if (this.Page_AR == null) {
                                this._entityResourceService.getEntityResourceByTableName("ARInvoice", 0).subscribe(function (response) {
                                    _this._entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe(function (response) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Invoice/Components/Workspaces/AccountReceivablesComponent", myLocation_1.viewContainerRef)
                                            .then(function (cmpRef) {
                                            _this.Page_AR = cmpRef.instance;
                                            _this.Page_AR.InitComponent();
                                        });
                                    });
                                });
                            }
                            break;
                        }
                        case "PAYABLE": {
                            if (this.Page_AP == null) {
                                this._entityResourceService.getEntityResourceByTableName("APInvoice", 0).subscribe(function (response) {
                                    _this._entityResourceService.getEntityResourceByTableName("APPayment", 0).subscribe(function (response) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Invoice/Components/Workspaces/AccountPayablesComponent", myLocation_1.viewContainerRef)
                                            .then(function (cmpRef) {
                                            _this.Page_AP = cmpRef.instance;
                                            _this.Page_AP.InitComponent();
                                        });
                                    });
                                });
                            }
                            break;
                        }
                        case "TRANSFER": {
                            if (this.Page_AT == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Invoice/Components/Workspaces/AccountingTransferComponent", myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_AT = cmpRef.instance;
                                    //this.Page_AT.InitComponent();
                                });
                            }
                            break;
                        }
                        case "SETTINGS": {
                            if (this.Page_SET == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load("./Invoice/Components/Workspaces/SettingsComponent", myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_SET = cmpRef.instance;
                                });
                            }
                            break;
                        }
                    }
                }
            }
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], InvoiceComponent.prototype, "AllLocations", void 0);
    InvoiceComponent = __decorate([
        core_1.Component({
            selector: 'OperationsComponent',
            moduleId: module.id,
            templateUrl: './InvoiceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], InvoiceComponent);
    return InvoiceComponent;
}());
exports.InvoiceComponent = InvoiceComponent;
//# sourceMappingURL=InvoiceComponent.js.map