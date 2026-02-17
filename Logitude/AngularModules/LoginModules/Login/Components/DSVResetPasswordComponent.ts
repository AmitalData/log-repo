import { Component } from '@angular/core';
import {LoginService, LoginParameters} from '../LoginService';
import {Headers} from '@angular/http';
import {SessionInfo} from '../SessionInfo';
import {PasswordChangeService} from '../PasswordChangeService';
import {Tools} from '../Utilities/Tools';
import {ResetPasswordComponent} from './ResetPasswordComponent'; 

@Component({
    selector: 'DSVResetPasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'DSVResetPasswordComponent.html',
    styleUrls: ['ChangePasswordComponent.css']
})
export class DSVResetPasswordComponent extends ResetPasswordComponent { 
    constructor(public ss: LoginService) {
        super(ss);
    }

}
