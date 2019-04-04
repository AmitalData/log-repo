 
import { Output, EventEmitter, Component, OnInit, ChangeDetectorRef, AfterViewInit } from '@angular/core';

import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TenantLoginPolicyPM } from '../../../../Common/EntityPMs/TenantLoginPolicyPM';
import { TenantLoginPolicyPMService } from  '../../../../Common/Services/StandardPMs/TenantLoginPolicyPMService';
import { UIProperties, UIProperty } from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { EntityPartner } from '../../../../Infrastructure/DataContracts/EntityPartner';
import { AppTool} from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {UserExtendedPMService} from '../../../../Common/Services/ExtendedPMs/UserExtendedPMService';


@Component({
    selector: 'TenantLoginPolicyComponent',
    moduleId: module.id,
    templateUrl: './TenantLoginPolicyComponent.html',
    providers: [TenantLoginPolicyPMService],
})
export class TenantLoginPolicyComponent extends BaseComponent {

    public DataContext = this;
    public ObjectTableName: string = "TenantLoginPolicy";
    public ValidationErrorsList: string[];
    public IsResourcesReady: boolean = false;
    public EntityPM: TenantLoginPolicyPM = null;
    private IsNew: boolean = false;
    @Output() OnCloseSendToContactsEvent: EventEmitter<any> = new EventEmitter();
    public EnabledForTypesList: EnabledForType[];
    userExtendedPMService: UserExtendedPMService;
    public EnabledForUsersCount: number = 0;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService, private TenantLoginPolicyPMService: TenantLoginPolicyPMService) {
        super();

        this.userExtendedPMService = new UserExtendedPMService();
        this.EnabledForTypesList = [];

        var type1: EnabledForType = new EnabledForType("ALL", "All Users");
        this.EnabledForTypesList.push(type1);

        var type2: EnabledForType = new EnabledForType("SPCF", "Specific Users");
        this.EnabledForTypesList.push(type2);

        this.LoadData();
    }


    LoadData() {
        this.TenantLoginPolicyPMService.get(SessionLocator.Tenant).subscribe(response => {
            if (!response.HasError) {
                if (response.Result) {
                    this.EntityPM = response.Result;

                    if (this.EntityPM.IsEnabledForSpecificUsers)
                        this.GetEnabledForUsersCount();
                    else
                        this.IsResourcesReady = true;
                    
                }
                else {
                    this.EntityPM = this.TenantLoginPolicyPMService.GetNewEntityPM();
                    this.EntityPM.Tenant = SessionLocator.Tenant;
                    this.EntityPM.LoginPolicyCode = "NOREST";
                    this.EntityPM.IsEnabledForSpecificUsers = false;
                    this.EntityPM.TwoFactorInternalIPs = "";
                    this.EntityPM.AllowedIPs = "";
                    this.EntityPM.ExcludeInternalIPs = false;
                    this.EntityPM.SessionTimeout = 999;
                    this.SelectedEnabledFor = this.EnabledForTypesList.filter(t => t.Code === "ALL")[0];
                    this.IsNew = true;
                    this.IsResourcesReady = true;
                  
                }

               
            }
        });
    }

    public GetEnabledForUsersCount() {
        this.userExtendedPMService.GetUsersTwoFactorAuthenticationEnabled(SessionLocator.Tenant).subscribe(resp => {

            if (resp.Result) {
                this.EnabledForUsersCount = resp.Result.length;
            }

            this.IsResourcesReady = true;
        });
    }

    public get SelectedEnabledFor() {
        if (this.EntityPM) {
            if (this.EntityPM.IsEnabledForSpecificUsers) {
                return this.EnabledForTypesList.filter(t => t.Code === "SPCF")[0];
            }
            else {
                return this.EnabledForTypesList.filter(t => t.Code === "ALL")[0];
            }
        }
    }
    public set SelectedEnabledFor(newValue: EnabledForType) {
        var isSpecific: boolean = false;
        if (newValue.Code === "SPCF") {
            isSpecific = true;
        }
        else {
            isSpecific = false;
        }

        if (this.EntityPM.IsEnabledForSpecificUsers != isSpecific) {
            this.EntityPM.IsEnabledForSpecificUsers = isSpecific;
        }
    }

    private loginPolicyCode: string = "";
    public get LoginPolicyCode() {
        if (this.EntityPM) {
            this.loginPolicyCode = this.EntityPM.LoginPolicyCode;
        }

        return this.loginPolicyCode;

    }
    public set LoginPolicyCode(newValue: string) {
        if (this.loginPolicyCode != newValue) {
            this.loginPolicyCode = newValue;
            this.EntityPM.LoginPolicyCode = newValue;

        }
    }



    private keepUserLoggedIn: boolean = false;
    public get KeepUserLoggedIn() {
        if (this.EntityPM) {
            this.keepUserLoggedIn = this.EntityPM.KeepUserLoggedIn;
        }

        return this.keepUserLoggedIn;

    }
    public set KeepUserLoggedIn(newValue: boolean) {
        if (this.keepUserLoggedIn != newValue) {
            this.keepUserLoggedIn = newValue;
            this.EntityPM.KeepUserLoggedIn = newValue;

        }
    }



    private IsSessionTimeoutValueHasError: boolean = false;
    private sessionTimeout: number;
    public get SessionTimeout() {
        if (this.EntityPM) {
            this.sessionTimeout = this.EntityPM.SessionTimeout;
        }

        return this.sessionTimeout;

    }

    public set SessionTimeout(newValue: number) {
        if (this.sessionTimeout != newValue) {
            this.sessionTimeout = newValue;
            this.EntityPM.SessionTimeout = newValue;
            this.IsSessionTimeoutValueHasError = false;

            if (newValue > 8) {
                this.IsSessionTimeoutValueHasError = true;
            }
   

        }
    }




    public InternalIps: string[] = [];
    private twoFactorInternalIPs: string = "";
    public get TwoFactorInternalIPs() {
        var value = "";
        if (this.EntityPM) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.TwoFactorInternalIPs)) {
                 
                this.InternalIps = this.EntityPM.TwoFactorInternalIPs.split(',');
                this.InternalIps.forEach(p => {
                    if (!AppTool.IsNullOrEmpty(p)) {
                        value += p + '\n';
                    }
                });
            }
            //else
                //this.twoFactorInternalIPs = "";
        }

        return value;

    }
    public set TwoFactorInternalIPs(newValue: string) {
        if (this.twoFactorInternalIPs != newValue) {
            this.twoFactorInternalIPs = newValue;
        }
    }


    public LoginAllowdedIps: string[] = [];
    private allowedIPs: string = "";
    public get AllowedIPs() {
        var value = "";
        if (this.EntityPM) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.AllowedIPs)) {

                this.LoginAllowdedIps = this.EntityPM.AllowedIPs.split(',');
                this.LoginAllowdedIps.forEach(p => {
                    if (!AppTool.IsNullOrEmpty(p)) {
                        value += p + '\n';
                    }
                });
            }
             
        }

        return value;

    }
    public set AllowedIPs(newValue: string) {
        if (this.allowedIPs != newValue) {
            this.allowedIPs = newValue;
        }
    }

    private iPField: string;
    public get IPField() { return this.iPField; }
    public set IPField(newValue: string) { if (this.iPField != newValue) { this.iPField = newValue;  } }


    private allowedIPField: string;
    public get AllowedIPField() { return this.allowedIPField; }
    public set AllowedIPField(newValue: string) { if (this.allowedIPField != newValue) { this.allowedIPField = newValue; } }

   
    DefineUsers() {

        //var logWindow = new LogitudeWindow();
        //logWindow.Width = 725;
        //logWindow.Height = 520;
        //logWindow.WindowArgs = this;
        //logWindow.Title = TextCodeTranslator.TranslateTablePlural("Contact") + " Search";;
        //logWindow.Show('./Common/Components/UsersSearch/SearchContactsComponent');


        //var logWindow = new LogitudeWindow();
        //logWindow.Title = "Contacts List";
        //logWindow.Width = window.innerWidth - 100;
        //logWindow.Height = window.innerHeight - 100;
        ///logWindow.WindowArgs = windowArgs;
        //logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SendMessageContacts/SendToContactsComponent");
        //logWindow.WindowClosed.subscribe(($event: any) => {
        //    //this.IsOpenWidnow = false;
        //});

        //var windowArgs: any = {};
        //windowArgs.PartnersObslist = this.PartnersObslist;
        //windowArgs.ToEmail = this.ToEmail;
        //windowArgs.Cc = this.Cc;
        //windowArgs.Bcc = this.Bcc;
        //windowArgs.EntityId = this.EntityId;
        //windowArgs.ObjectTableId = this.ObjectTableId;
        //windowArgs.OnCloseSendToContactsEvent = this.OnCloseSendToContactsEvent;

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Users List";
        logWindow.Width = 725;
        logWindow.Height = 520;
        //logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureUser/Components/UsersSearch/UserSearchComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.GetEnabledForUsersCount();
            //this.IsOpenWidnow = false;
        });
    }
    AddInternalIP() {
        this.ValidationErrorsList = [];
        if (!AppTool.IsNullOrEmpty(this.IPField)) {
            var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            if (this.IPField.match(ipformat)) {

                if (this.TwoFactorInternalIPs.indexOf(this.IPField.trim()) < 0) {
                    if (!AppTool.IsNullOrEmpty(this.EntityPM.TwoFactorInternalIPs)) {
                        this.EntityPM.TwoFactorInternalIPs  += ',' + this.IPField.trim();
                    } else
                        this.EntityPM.TwoFactorInternalIPs = this.IPField.trim();

                    this.EntityPM.TwoFactorInternalIPs = this.TrimEndAndStart(this.EntityPM.TwoFactorInternalIPs, ',');
                    this.TwoFactorInternalIPs = this.EntityPM.TwoFactorInternalIPs;

                }
                this.IPField = null;
            }
            else {
                this.ValidationErrorsList.push("You have entered an invalid IP address!");

            }
        }
        else {
            //this.ValidationErrorsList.push("You have entered an invalid IP address!");
        }
    }



    RemoveIP(ip) {
        if (this.TwoFactorInternalIPs.indexOf(ip.trim()) >= 0) {
            var ipsString = "";
            var ipsArray: string[] = this.EntityPM.TwoFactorInternalIPs.split(',');
            ipsArray.forEach(s => {
                if (ip != s && !AppTool.IsNullOrEmpty(s)) {
                    ipsString += s + ",";
                }
            });

            ipsString = this.TrimEndAndStart(ipsString, ',');
            this.EntityPM.TwoFactorInternalIPs = ipsString;
            this.TwoFactorInternalIPs = this.EntityPM.TwoFactorInternalIPs;

        }
    }


    AddAllowedIP() {
        this.ValidationErrorsList = [];
        if (!AppTool.IsNullOrEmpty(this.AllowedIPField)) {
            var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            if (this.AllowedIPField.match(ipformat)) {

                if (this.AllowedIPs.indexOf(this.AllowedIPField.trim()) < 0) {
                    if (!AppTool.IsNullOrEmpty(this.EntityPM.AllowedIPs)) {
                        this.EntityPM.AllowedIPs += ',' + this.AllowedIPField.trim();
                    } else
                        this.EntityPM.AllowedIPs = this.AllowedIPField.trim();

                    this.EntityPM.AllowedIPs = this.TrimEndAndStart(this.EntityPM.AllowedIPs, ',');
                    this.AllowedIPs = this.EntityPM.AllowedIPs;

                }
                this.AllowedIPField = null;
            }
            else {
                this.ValidationErrorsList.push("You have entered an invalid IP address!");

            }
        }
        else {
            //this.ValidationErrorsList.push("You have entered an invalid IP address!");
        }
    }

    RemoveAllowedIP(ip) {
        if (this.AllowedIPs.indexOf(ip.trim()) >= 0) {
            var ipsString = "";
            var ipsArray: string[] = this.EntityPM.AllowedIPs.split(',');
            ipsArray.forEach(s => {
                if (ip != s && !AppTool.IsNullOrEmpty(s)) {
                    ipsString += s + ",";
                }
            });

            ipsString = this.TrimEndAndStart(ipsString, ',');
            this.EntityPM.AllowedIPs = ipsString;
            this.AllowedIPs = this.EntityPM.AllowedIPs;

        }
    }

    TrimEndAndStart(myString: string, ch: string) {
        if (!AppTool.IsNullOrEmpty(myString)) {
            if (myString.charAt(myString.length - 1) == ch) {
                myString = myString.substr(0, myString.length - 1);
            }
            if (myString.charAt(0) == ch) {
                myString = myString.substr(0, 1);
            }
        }
        return myString;
    }
     

    //Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.EntityPM.LoginPolicyCode)) {
           // this.ObjectTableName + ".F." + myFieldName
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("TenantLoginPolicy.F.LoginPolicyCode") +  " field is required!");
        }

        if ((this.EntityPM.LoginPolicyCode === "TFAUTH") && this.EntityPM.ExcludeInternalIPs == true && AppTool.IsNullOrEmpty(this.EntityPM.TwoFactorInternalIPs)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("TenantLoginPolicy.F.TwoFactorInternalIPs") + " field is required!");
        }

        if ((this.EntityPM.LoginPolicyCode === "COMPIP") && AppTool.IsNullOrEmpty(this.EntityPM.AllowedIPs)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("TenantLoginPolicy.F.AllowedIPs") + " field is required!");
        }
   
        if (this.IsSessionTimeoutValueHasError) {
            this.ValidationErrorsList.push("Maximum session timeout 8 hours");
        }

        if (this.ValidationErrorsList.length > 0)
            return;


        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        if (this.IsNew) {
            this.TenantLoginPolicyPMService.insert(this.EntityPM).subscribe(response => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response.HasError) {
                    this.CurrentSession.CloseCurrentWindow();
                }
                else
                    this.ValidationErrorsList = response.ErrorsArray;
            });
        }
        else {
            this.TenantLoginPolicyPMService.update(this.EntityPM).subscribe(response => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response.HasError) {
                    this.CurrentSession.CloseCurrentWindow();
                }
                else
                    this.ValidationErrorsList = response.ErrorsArray;
            });
        }


    }


}

export class EnabledForType {
    constructor(public Code: string, public Name: string) { }
}

