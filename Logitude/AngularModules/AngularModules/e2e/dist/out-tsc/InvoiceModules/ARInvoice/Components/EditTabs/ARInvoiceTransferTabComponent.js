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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ARInvoiceTransferTabComponent = /** @class */ (function (_super) {
    __extends(ARInvoiceTransferTabComponent, _super);
    function ARInvoiceTransferTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityPM = null;
        _this.ObjectTableName = "ARInvoice";
        _this.DataContext = _this;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.Retries = 0;
        _this.EntityPM = entityArgs.EntityPM;
        _this.IsConstituentInvoice = _this.EntityPM.IsConstituentInvoice;
        _this.RunComponent();
        _this.Listen();
        return _this;
    }
    ARInvoiceTransferTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.InputTemplate) {
                        _this.InputTemplate.EntityPM = _this.EntityPM;
                        _this.InputTemplate.BuildList();
                    }
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.InputTemplate) {
                        _this.InputTemplate.EntityPM = _this.EntityPM;
                        _this.InputTemplate.BuildList();
                    }
                }
            });
        }
    };
    ARInvoiceTransferTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ARInvoiceTransferTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ARInvoiceTransferTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ARInvoiceTransferTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./InvoiceModules/ARInvoice/Components/NewEntity/ARInvoiceTransferTemplate", this.viewContainerRef)
            .then(function (cmpRef) {
            _this.InputTemplate = cmpRef.instance;
            _this.InputTemplate = cmpRef.instance;
            _this.InputTemplate.InitTemplate(_this.EntityPM);
        });
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ARInvoiceTransferTabComponent.prototype, "viewContainerRef", void 0);
    ARInvoiceTransferTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoiceTransferTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ARInvoiceTransferTabComponent);
    return ARInvoiceTransferTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ARInvoiceTransferTabComponent = ARInvoiceTransferTabComponent;
//# sourceMappingURL=ARInvoiceTransferTabComponent.js.map