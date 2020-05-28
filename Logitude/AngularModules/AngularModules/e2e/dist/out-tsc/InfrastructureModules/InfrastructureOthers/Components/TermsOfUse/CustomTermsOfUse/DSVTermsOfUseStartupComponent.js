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
var TermsofUseSignaturePMService_1 = require("../../../../../Common/Services/StandardPMs/TermsofUseSignaturePMService");
var TermsOfUseStartupComponent_1 = require("../TermsOfUseStartupComponent");
var DSVTermsOfUseStartupComponent = /** @class */ (function (_super) {
    __extends(DSVTermsOfUseStartupComponent, _super);
    function DSVTermsOfUseStartupComponent() {
        return _super.call(this) || this;
    }
    DSVTermsOfUseStartupComponent.prototype.ngOnInit = function () {
    };
    DSVTermsOfUseStartupComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DSVTermsOfUseStartupComponent',
            templateUrl: './DSVTermsOfUseStartupComponent.html',
            providers: [TermsofUseSignaturePMService_1.TermsofUseSignaturePMService]
        }),
        __metadata("design:paramtypes", [])
    ], DSVTermsOfUseStartupComponent);
    return DSVTermsOfUseStartupComponent;
}(TermsOfUseStartupComponent_1.TermsOfUseStartupComponent));
exports.DSVTermsOfUseStartupComponent = DSVTermsOfUseStartupComponent;
//# sourceMappingURL=DSVTermsOfUseStartupComponent.js.map