"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
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
var LoginService_1 = require("../LoginService");
var PasswordChangeService_1 = require("../PasswordChangeService");
var ChangePasswordComponent_1 = require("./ChangePasswordComponent");
var DSVChangePasswordComponent = /** @class */ (function (_super) {
    __extends(DSVChangePasswordComponent, _super);
    function DSVChangePasswordComponent(ss, ll) {
        var _this = _super.call(this, ss, ll) || this;
        _this.ss = ss;
        _this.ll = ll;
        return _this;
    }
    DSVChangePasswordComponent = __decorate([
        core_1.Component({
            selector: 'DSVChangePasswordComponent',
            moduleId: './Login/Components/',
            templateUrl: 'DSVChangePasswordComponent.html',
            styleUrls: ['ChangePasswordComponent.css']
        }),
        __metadata("design:paramtypes", [PasswordChangeService_1.PasswordChangeService, LoginService_1.LoginService])
    ], DSVChangePasswordComponent);
    return DSVChangePasswordComponent;
}(ChangePasswordComponent_1.ChangePasswordComponent));
exports.DSVChangePasswordComponent = DSVChangePasswordComponent;
//# sourceMappingURL=DSVChangePasswordComponent.js.map