import { Output, EventEmitter, Component, OnInit, ChangeDetectorRef, AfterViewInit } from '@angular/core';

import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EmailAlertSettingPM } from '../../../../Infrastructure/EntityPMs/EmailAlertSettingPM';
import { EmailAlertSettingPMService } from '../../../../Infrastructure/Services/ExtendedPMs/EmailAlertSettingPMService';
import { UIProperties, UIProperty } from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { EntityPartner } from '../../../../Infrastructure/DataContracts/EntityPartner';
import { AppTool} from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    selector: 'EmailNotificationsSettingsComponent',
    moduleId: module.id,
    templateUrl: './EmailNotificationsSettingsComponent.html',
    providers: [EmailAlertSettingPMService],
})
export class EmailNotificationsSettingsComponent extends BaseComponent{
    //public EntityPM: EmailAlertSettingPM;
    public DataContext = this;
    public ObjectTableName: string = "EmailAlertSetting";
    public ValidationErrorsList: string[];
    public IsResourcesReady: boolean = false;
    public OwnerAlerts: Array<EmailAlertSettingDataViewModel> = [];
    public GeneralAlerts: Array<EmailAlertSettingDataViewModel> = [];
    public AllAlerts: EmailAlertSettingPM[];
    @Output() OnCloseSendToContactsEvent: EventEmitter<any> = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService, private emailAlertSettingPMService: EmailAlertSettingPMService) {
        super();
        this.InitializeServices();
        this.LoadData();
    }

    LoadData() {
        this.emailAlertSettingPMService.getAllEmailAlerts(SessionInfo.LoggedUserTenant).subscribe(response => {
            if (!response.HasError) {
                this.AllAlerts = response.Result;
                var ownerArr = this.AllAlerts.filter(a => a.SettingLevelCode == "OWNR");
                for (var k in ownerArr) {
                    this.OwnerAlerts.push(new EmailAlertSettingDataViewModel(ownerArr[k]));
                }

                var generalArr = this.AllAlerts.filter(a => a.SettingLevelCode == "GNRL");
                for (var k in generalArr) {
                    this.GeneralAlerts.push(new EmailAlertSettingDataViewModel(generalArr[k]));
                }
               
                
                this.IsResourcesReady = true;
            }
        });
    }
    //private myTenantPMService: TenantPMService;
   // private myAccountingSettingPMService: AccountingSettingPMService;
    InitializeServices() {
      //  this.myTenantPMService = new TenantPMService();
       // this.myAccountingSettingPMService = new AccountingSettingPMService();
    }

    //Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
         
        for (var k in this.GeneralAlerts)
        {
            var alert = this.GeneralAlerts[k];
            if (alert.IsActive && AppTool.IsNullOrEmpty(alert.To)) {
                this.ValidationErrorsList.push(alert.Description + " must have destination contacts emails");
            }
        }

        if (this.ValidationErrorsList.length > 0)
            return;

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        this.emailAlertSettingPMService.updateAllAlerts(this.AllAlerts, SessionInfo.LoggedUserTenant).subscribe(response => {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (response.HasError)
            {
                this.ValidationErrorsList = response.ErrorsArray;
            }
            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        });
        //if (this.EntityPM.IsDirty) {
        //  }
    }

    ToEmail: string;
    IsCloseSendToContact: boolean = false;
    PartnersObslist: EntityPartner[] = [];
    SelectedEmailAlertSettingDataViewModel: EmailAlertSettingDataViewModel;
    ShowSendToEmail(item) {
        this.SelectedEmailAlertSettingDataViewModel = item;
        //this.PartnersObslist.push(new EntityPartner("All", "1", false))
        this.IsCloseSendToContact = false;
        this.OnCloseSendToContactsEvent.subscribe(($event: any) => {
            if (!this.IsCloseSendToContact && $event) {
                this.IsCloseSendToContact = true;

                this.ToEmail = "";


                if ($event.ToEmailLists && $event.ToEmailLists.length > 0) {
                    $event.ToEmailLists.forEach((item) => {
                        this.ToEmail += item + ";";

                    });

                    if (this.SelectedEmailAlertSettingDataViewModel) {
                        this.SelectedEmailAlertSettingDataViewModel.To = this.ToEmail;
                    }
                }
                else if (this.SelectedEmailAlertSettingDataViewModel) {
                    this.SelectedEmailAlertSettingDataViewModel.To = null;
                }
            }
        });
        var windowArgs: any = {};
        windowArgs.PartnersObslist = this.PartnersObslist;
        windowArgs.ToEmail = item.To;//this.ToEmail;
        windowArgs.Cc = "";
        windowArgs.Bcc = "";
        windowArgs.HideCC = true;
        windowArgs.HideBCC = true;
        //windowArgs.EntityId = item.Id;
        //windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.OnCloseSendToContactsEvent = this.OnCloseSendToContactsEvent;
        windowArgs.ObjectTableName = "User";
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Users List";
        logWindow.Width = window.innerWidth - 100;
        logWindow.Height = window.innerHeight - 100;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/SendMessageContacts/SendToContactsComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {

        });


    }
}

export class EmailAlertSettingDataViewModel {

    public SettingPM: EmailAlertSettingPM;
    public UIProperties: UIProperties;
    constructor(settingpm: EmailAlertSettingPM)
    {
        this.SettingPM = settingpm;
        this.UIProperties = new UIProperties;
    }
    
   
    
    public get Description() { return this.SettingPM.Description; }
    public set Description(newValue: string) { this.SettingPM.Description = newValue;  }

    
    public get To() { return this.SettingPM.To; }
    public set To(newValue: string) { this.SettingPM.To = newValue;  }


   
     
    public get IsActive() { return !this.SettingPM.InActive; }
    public set IsActive(newValue: boolean) { this.SettingPM.InActive = !newValue;}



     
    public get Code() { return this.SettingPM.Code; }
    public set Code(newValue: string) { this.SettingPM.Code = newValue;}





}
