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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var PrintingOptionsComponent = /** @class */ (function (_super) {
    __extends(PrintingOptionsComponent, _super);
    function PrintingOptionsComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ComboBoxIsDisabled = false;
        return _this;
    }
    PrintingOptionsComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            var iCode = this.EntityPM.Code;
            if (iCode) {
                iCode = iCode.toUpperCase();
                switch (iCode) {
                    case "999S":
                    case "999C":
                    case "999M":
                    case "999CI":
                    case "999MP":
                    case "999P":
                        {
                            if (SessionLocator_1.SessionLocator.TenantPM.CountryCode == "IL" && SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare == false) {
                                this.ComboBoxIsDisabled = true;
                                this.EntityPM.UIProperties.SetEnabled("IsDocumentOneTimePrintLimited", "DocumentType", false);
                            }
                            break;
                        }
                }
            }
            this.Run();
        }
    };
    PrintingOptionsComponent.prototype.Run = function () {
        var _this = this;
        if (this.EntityPM.DocumentTypeCopies) {
            this.DocumentTypeCopies = this.EntityPM.DocumentTypeCopies;
            this.SelectedCopy = this.DocumentTypeCopies.filter(function (d) { return d.Id == _this.EntityPM.LimitedPrintCopyId; })[0];
        }
    };
    PrintingOptionsComponent.prototype.DocumentTypeCopyValueChanged = function (Copy) {
        this.EntityPM.LimitedPrintCopyId = Copy.Id;
    };
    PrintingOptionsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedLogisticsTab',
            templateUrl: './PrintingOptionsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], PrintingOptionsComponent);
    return PrintingOptionsComponent;
}(BaseComponent_1.BaseComponent));
exports.PrintingOptionsComponent = PrintingOptionsComponent;
//# sourceMappingURL=PrintingOptionsComponent.js.map