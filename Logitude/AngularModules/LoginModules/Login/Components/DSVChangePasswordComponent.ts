import { Component } from '@angular/core';
import {LoginService, LoginParameters} from '../LoginService';
import {Headers} from '@angular/http';
import {SessionInfo} from '../SessionInfo';
import {PasswordChangeService} from '../PasswordChangeService';
import {Tools} from '../Utilities/Tools'; 
import {ChangePasswordComponent} from './ChangePasswordComponent';

@Component({
    selector: 'DSVChangePasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'DSVChangePasswordComponent.html',
    styleUrls: ['ChangePasswordComponent.css']
})
export class DSVChangePasswordComponent extends ChangePasswordComponent {
     
    constructor(public ss: PasswordChangeService, public ll: LoginService) {
        super(ss,ll);
    }  
}
