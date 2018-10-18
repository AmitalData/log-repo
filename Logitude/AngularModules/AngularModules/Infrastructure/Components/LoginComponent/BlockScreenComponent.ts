import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {SessionInfo} from '../../Utilities/SessionInfo';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {LoginService} from '../../Services/LoginService';
import {Headers} from '@angular/http';
import {TenantManagementPMService} from '../../Services/StandardPMs/TenantManagementPMService';
import {AppTool} from '../../Tools';
import {Environment} from '../../Locators/Environment';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './BlockScreenComponent.html',
})

export class BlockScreenComponent {

    @Output() BackToLoginCompleted: EventEmitter<any> = new EventEmitter();
    public BlockMessagePart1: string = "";
    public BlockMessagePart2: string = "";
    public BlockMessagePart3: string = "";

    public IsProduction: boolean = false;

    public isCompanyAndUser: boolean = false;
    public LogoURL: string = "./Images/LoginScreen/header.jpg";
    public SampleLogoURL: string = "./Images/ApplicationLogo/Angular/AngularLogo.png";
    public authHeader;
    constructor(private loginService: LoginService) {
        this.authHeader = new Headers();
        this.authHeader.append('Content-Type', 'application/json');
        this.authHeader.append('Accept', 'application/json');
        this.authHeader.append('token', SessionInfo.Token);
        this.loginService.AuthHeader = this.authHeader;

        //var temp = window.sessionStorage.getItem("LogoURL");
        //var LogoCode = window.sessionStorage.getItem("LogoCode");
        //if (temp) {
        //    this.LogoURL = temp;
        //    this.SampleLogoURL = temp;
        //}
        //else {
        this.loginService.GetGlobalSetting().subscribe(Setting => {
            if (Setting) {
                ObjectsLocator.GlobalSetting = Setting;
                this.loginService.GetTenantManagement().subscribe(TenantManagement => {
                    var myTenantManagementPMService = new TenantManagementPMService();
                    if (TenantManagement) {
                        var temptenant = myTenantManagementPMService.MapJsonToEntityPM(TenantManagement);
                        if (temptenant) {
                            this.loginService.GetPrivateLableById(temptenant.PrivateLabelId).subscribe(Result => {
                                SessionLocator.PrivateLableSettings = Result;
                                if (SessionLocator.PrivateLableSettings) {
                                    this.LogoURL = "data:image/JPEG;base64," + SessionLocator.PrivateLableSettings.MainLogo;
                                    this.SampleLogoURL = "data:image/JPEG;base64," + SessionLocator.PrivateLableSettings.MainLogo;
                                }
                                else {
                                    this.LogoURL = AppTool.GetEnvironmentLogo(Setting.LogoCode);
                                    this.SampleLogoURL = AppTool.GetEnvironmentLogo(Setting.LogoCode);
                                }
                                this.SetBlockMessage();
                            });
                        }
                        else {
                            this.LogoURL = AppTool.GetEnvironmentLogo(Setting.LogoCode);
                            this.SampleLogoURL = AppTool.GetEnvironmentLogo(Setting.LogoCode);
                            this.SetBlockMessage();
                        }
                    }
                    else {
                        this.LogoURL = AppTool.GetEnvironmentLogo(Setting.LogoCode);
                        this.SampleLogoURL = AppTool.GetEnvironmentLogo(Setting.LogoCode);
                        this.SetBlockMessage();
                    }
                });
                
            }
            else {
                this.LogoURL = "./Images/LoginScreen/header.jpg";
                this.SampleLogoURL = "./Images/ApplicationLogo/Angular/AngularLogo.png";
                this.SetBlockMessage();
            }
           
        }); 
        //}
        //this.SetBlockMessage();
    }
    SetBlockMessage() {
        this.IsProduction = SessionLocator.IsProduction;
        var Email: string = "";
        if (SessionLocator.PrivateLableSettings) {
            Email = SessionLocator.PrivateLableSettings.ContactUsEmail;
        }
        else {
            var tempmail = Environment.GetContactUsEmail();
            if (tempmail) {
                Email = tempmail;
            }
            else {
                Email = "info@logitudeworld.com";
            }
        }

        if (SessionLocator.BlockType == "company") {
            this.BlockMessagePart1 = "Your company subscription has expired.";
            this.BlockMessagePart2 = "To renew please contact " + Email;
            this.isCompanyAndUser = true;
        }
        else if (SessionLocator.BlockType == "user") {
            this.BlockMessagePart1 = "Your temporary access has expired";
            this.BlockMessagePart2 = "To renew please contact " + Email;
            this.isCompanyAndUser = true;

        }
        else if (SessionLocator.BlockType == "suspend") {
            this.BlockMessagePart1 = "Your company subscription has expired. The recurring renew has failed due to credit";
            this.BlockMessagePart2 = "card authorization error.";
            this.BlockMessagePart3 = "Please contact your e-commerce vendor or " + Email;
        }
    }


    BackToLoginClicked() {

        this.BackToLoginCompleted.emit('true');

    }


}