"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LoginComponent_1 = require("./Components/LoginComponent");
var DSVLoginComponent_1 = require("./Components/DSVLoginComponent");
var DSVMobileLoginComponent_1 = require("./Components/DSVMobileLoginComponent");
var ChangePasswordComponent_1 = require("./Components/ChangePasswordComponent");
var DSVChangePasswordComponent_1 = require("./Components/DSVChangePasswordComponent");
var ResetPasswordComponent_1 = require("./Components/ResetPasswordComponent");
var RootComponent_1 = require("./RootComponent");
var DSVResetPasswordComponent_1 = require("./Components/DSVResetPasswordComponent");
exports.LoginComponents = [
    LoginComponent_1.LoginComponent,
    DSVLoginComponent_1.DSVLoginComponent,
    RootComponent_1.RootComponent,
    ChangePasswordComponent_1.ChangePasswordComponent,
    DSVChangePasswordComponent_1.DSVChangePasswordComponent,
    ResetPasswordComponent_1.ResetPasswordComponent,
    DSVResetPasswordComponent_1.DSVResetPasswordComponent,
    DSVMobileLoginComponent_1.DSVMobileLoginComponent
];
var LoginModuleDeclarations = /** @class */ (function () {
    function LoginModuleDeclarations() {
    }
    LoginModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "LoginComponent": {
                myResult = LoginComponent_1.LoginComponent;
                break;
            }
            case "DSVLoginComponent": {
                myResult = DSVLoginComponent_1.DSVLoginComponent;
                break;
            }
            case "RootComponent": {
                myResult = RootComponent_1.RootComponent;
                break;
            }
            case "ChangePasswordComponent": {
                myResult = ChangePasswordComponent_1.ChangePasswordComponent;
                break;
            }
            case "DSVChangePasswordComponent": {
                myResult = DSVChangePasswordComponent_1.DSVChangePasswordComponent;
                break;
            }
            case "ResetPasswordComponent": {
                myResult = ResetPasswordComponent_1.ResetPasswordComponent;
                break;
            }
            case "DSVResetPasswordComponent": {
                myResult = DSVResetPasswordComponent_1.DSVResetPasswordComponent;
                break;
            }
            case "DSVMobileLoginComponent": {
                myResult = DSVMobileLoginComponent_1.DSVMobileLoginComponent;
                break;
            }
        }
        return myResult;
    };
    return LoginModuleDeclarations;
}());
exports.LoginModuleDeclarations = LoginModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map