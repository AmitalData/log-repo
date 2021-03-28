import { LoginComponent } from './Components/LoginComponent';
import { DSVLoginComponent } from './Components/DSVLoginComponent';
import { DSVMobileLoginComponent } from './Components/DSVMobileLoginComponent';
import { ChangePasswordComponent } from './Components/ChangePasswordComponent';
import { DSVChangePasswordComponent } from './Components/DSVChangePasswordComponent';
import { ResetPasswordComponent } from './Components/ResetPasswordComponent';
import { RootComponent } from './RootComponent';
import { DSVResetPasswordComponent } from './Components/DSVResetPasswordComponent';
import { PrivateLoginComponent } from './PrivateLabels/LoginComponents/PrivateLoginComponent';
import { PrivateResetPasswordComponent } from './PrivateLabels/LoginComponents/PrivateResetPasswordComponent';
import { PrivateChangePasswordComponent } from './PrivateLabels/LoginComponents/PrivateChangePasswordComponent';
export var LoginComponents = [
    LoginComponent,
    DSVLoginComponent,
    RootComponent,
    ChangePasswordComponent,
    DSVChangePasswordComponent,
    ResetPasswordComponent,
    DSVResetPasswordComponent,
    DSVMobileLoginComponent,
    PrivateLoginComponent,
    PrivateResetPasswordComponent,
    PrivateChangePasswordComponent
];
export var LoginModuleDeclarations = (function () {
    function LoginModuleDeclarations() {
    }
    LoginModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "LoginComponent": {
                myResult = LoginComponent;
                break;
            }
            case "DSVLoginComponent": {
                myResult = DSVLoginComponent;
                break;
            }
            case "RootComponent": {
                myResult = RootComponent;
                break;
            }
            case "ChangePasswordComponent": {
                myResult = ChangePasswordComponent;
                break;
            }
            case "DSVChangePasswordComponent": {
                myResult = DSVChangePasswordComponent;
                break;
            }
            case "ResetPasswordComponent": {
                myResult = ResetPasswordComponent;
                break;
            }
            case "DSVResetPasswordComponent": {
                myResult = DSVResetPasswordComponent;
                break;
            }
            case "DSVMobileLoginComponent": {
                myResult = DSVMobileLoginComponent;
                break;
            }
            case "PrivateLoginComponent": {
                myResult = PrivateLoginComponent;
                break;
            }
            case "PrivateResetPasswordComponent": {
                myResult = PrivateResetPasswordComponent;
                break;
            }
            case "PrivateChangePasswordComponent": {
                myResult = PrivateChangePasswordComponent;
                break;
            }
        }
        return myResult;
    };
    return LoginModuleDeclarations;
}());
//# sourceMappingURL=ModuleDeclarations.js.map