import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { LoginService, LoginParameters } from '../LoginService';
import { Headers } from '@angular/http';
import { SessionInfo } from '../SessionInfo';
import { LoginComponent } from './LoginComponent';
import { DynamicLoaderTSC } from '../Utilities/DynamicLoaderTSC';
import { Tools } from '../Utilities/Tools';

@Component({
    selector: 'DSVLoginComponent',
    moduleId: './Login/Components/',
    templateUrl: 'DSVLoginComponent.html',
    styleUrls: ['DSVLoginComponent.css']
})
export class DSVLoginComponent extends LoginComponent implements OnInit {

    public authHeader;
    constructor(private ss: LoginService) {
        super(ss);
    }
    ngOnInit() {
        console.log("ngOnInit");
        this.get_cookie_data();
    }

    private ClearLocation() {
        if (SessionInfo.MainLocation) {
            SessionInfo.MainLocation.clear();
        }
    }
    ForgotPasswordClicked() {
        this.ClearLocation();
        Tools.DynamicLoader.Load("./Login/Components/DSVResetPasswordComponent", SessionInfo.MainLocation)
            .then(cmpRef => {
            });
    }
}
