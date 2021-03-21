import {LoginComponent}   from './Components/LoginComponent';
import {DSVLoginComponent} from './Components/DSVLoginComponent';
import {DSVMobileLoginComponent} from './Components/DSVMobileLoginComponent';
import {ChangePasswordComponent} from './Components/ChangePasswordComponent';
import {DSVChangePasswordComponent} from './Components/DSVChangePasswordComponent';
import {ResetPasswordComponent} from './Components/ResetPasswordComponent'
import {RootComponent}   from './RootComponent';
import {DSVResetPasswordComponent} from './Components/DSVResetPasswordComponent'
import { HybridLoginComponent } from './HybridLabels/Components/HybridLoginComponent';  
import { HybridResetPasswordComponent } from './HybridLabels/Components/HybridResetPasswordComponent'; 
export const LoginComponents =
    [
        LoginComponent,
        DSVLoginComponent,
        RootComponent,
        ChangePasswordComponent,
        DSVChangePasswordComponent,
        ResetPasswordComponent,
        DSVResetPasswordComponent,
        DSVMobileLoginComponent,
        HybridLoginComponent,
        HybridResetPasswordComponent, 
    ];

export class LoginModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            
            case "LoginComponent": { myResult = LoginComponent; break; }
            case "DSVLoginComponent": { myResult = DSVLoginComponent; break; }
            case "RootComponent": { myResult = RootComponent; break; }
            case "ChangePasswordComponent": { myResult = ChangePasswordComponent; break; }
            case "DSVChangePasswordComponent": { myResult = DSVChangePasswordComponent; break; }
            case "ResetPasswordComponent": { myResult = ResetPasswordComponent; break; }
            case "DSVResetPasswordComponent": { myResult = DSVResetPasswordComponent; break; }
            case "DSVMobileLoginComponent": { myResult = DSVMobileLoginComponent; break; }
            case "HybridLoginComponent": { myResult = HybridLoginComponent; break; }
            case "HybridResetPasswordComponent": { myResult = HybridResetPasswordComponent; break; }  

        }

        return myResult;
    }
}