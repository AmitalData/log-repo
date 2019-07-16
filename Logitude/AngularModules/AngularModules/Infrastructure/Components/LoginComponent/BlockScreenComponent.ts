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
import { HomeComponent } from '../HomeComponent/HomeComponent';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './BlockScreenComponent.html',
})

export class BlockScreenComponent {

    @Output() BackToLoginCompleted: EventEmitter<any> = new EventEmitter();
    public BlockMessagePart1: string = "";
    public BlockMessagePart2: string = "";
    public BlockMessagePart3: string = "";
    public BlockMessagePart4: string = "";
    public BlockMessagePart5: string = "You can also manage your bluesnap account ";
    public ExistManage: boolean = false;
    public IsProduction: boolean = false;

    public isCompanyAndUser: boolean = false;
    public LogoURL: string = "./Images/LoginScreen/header.jpg";
    public SampleLogoURL: string = "./Images/ApplicationLogo/Angular/AngularLogo.png";
    public authHeader;
    GoToManage() {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetBlueSnapToken(SessionLocator.TenantManagementJS.BluesnapAccount, SessionLocator.TenantManagementJS.CountryName).subscribe((myResult) => {
            var temp = myResult.Result;
            temp = temp.Token;
            this.setCookie("CurrentTenant", SessionLocator.Tenant.toString(), 1);
            var link = "https://cp.bluesnap.com/jsp/account_login.jsp";
            if (!AppTool.IsNullOrEmpty(temp)) {
                link = "https://ws.bluesnap.com/jsp/entrance.jsp?target=cp&token=" + temp + "&pageToShow=my_account.jsp"
            }
            var win = window.open(link, '_blank');
            win.focus();
        });

    }


    private setCookie(name: string, value: string, expireDays: number, path: string = '') {
        let d: Date = new Date();
        d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
        let expires: string = `expires=${d.toUTCString()}`;
        let cpath: string = path ? `; path=${path}` : '';
        document.cookie = `${name}=${value}; ${expires}${cpath}`;
    }



    constructor(private loginService: LoginService) {
        this.authHeader = new Headers();
        this.authHeader.append('Content-Type', 'application/json');
        this.authHeader.append('Accept', 'application/json');
        this.authHeader.append('token', SessionInfo.Token);
        this.loginService.AuthHeader = this.authHeader;
        if (!AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.BluesnapAccount)) {
            this.ExistManage = true;
        }
        else {
            this.ExistManage = false;
        }
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
    public Email: string = "";
    SetBlockMessage() {
        this.IsProduction = SessionLocator.IsProduction;
        this.Email = "";
        if (SessionLocator.PrivateLableSettings) {
            this.Email = SessionLocator.PrivateLableSettings.ContactUsEmail;
        }
        else {
            var tempmail = Environment.GetContactUsEmail();
            if (tempmail) {
                this.Email = tempmail;
            }
            else {
                this.Email = "info@logitudeworld.com";
            }
        }

        if (SessionLocator.BlockType == "company") {
            this.BlockMessagePart1 = "Your company subscription has expired.";
            this.BlockMessagePart2 = "To renew or subscribe please use the links under the billing icon (marked with a $ sign) above";
            this.BlockMessagePart3 = "For more information and help please contact " ;
        }
        else if (SessionLocator.BlockType == "user") {
            this.BlockMessagePart1 = "Your temporary access has expired";
            this.BlockMessagePart2 = "To renew or subscribe please use the links under the billing icon (marked with a $ sign) above";
            this.BlockMessagePart3 = "For more information and help please contact " ;

        }
        else if (SessionLocator.BlockType == "suspend") {
            this.BlockMessagePart1 = "Your company subscription has expired. The recurring renew has failed due to credit";
            this.BlockMessagePart2 = "card authorization error.To renew or subscribe please use the links under the billing icon (marked with a $ sign) above";
            this.BlockMessagePart3 = "Please contact your e-commerce vendor or ";
           
        }
    }


    BackToLoginClicked() {

        this.BackToLoginCompleted.emit('true');

    }


}
