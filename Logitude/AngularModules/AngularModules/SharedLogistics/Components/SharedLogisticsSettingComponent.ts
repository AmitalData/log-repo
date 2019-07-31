import {Component, OnInit, EventEmitter}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../Infrastructure/Locators/ObjectsLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';
import {TenantPMService} from '../../Common/Services/StandardPMs/TenantPMService';
import {TenantPM} from '../../Common/EntityPMs/TenantPM';
import {Cloner} from '../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    selector: 'SharedLogisticsSetting',
    templateUrl: './SharedLogisticsSettingComponent.html',
    inputs: ['TenantPM', 'OnCloseWindowEvent'],
    providers: [SharedLogisticsService],
})
export class SharedLogisticsSettingComponent implements OnInit {





    public get IsSharedLogisticsActivated() {

        if (this.TenantPM) {
            return this.TenantPM.IsSharedLogisticsActivated;
        }
        else return false;
    }
    public set IsSharedLogisticsActivated(value: boolean) {
        if (this.TenantPM != null) {
            if (this.TenantPM.IsSharedLogisticsActivated !=value)
            {
                this.TenantPM.IsSharedLogisticsActivated = value;
                this.SetPropertiesEnable();
            }
        }

    }


    public get IsWebAccessActivated() {

        if (this.TenantPM) {
            return this.TenantPM.IsWebAccessActivated;
        }
        else return false;
    }
    public set IsWebAccessActivated(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.IsWebAccessActivated = value;
        }

    }


    public get IsMobileActivated() {

        if (this.TenantPM) {
            return this.TenantPM.IsMobileActivated;
        }
        else return false;
    }
    public set IsMobileActivated(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.IsMobileActivated = value;
        }

    }



    public get SharedLogisticsMessageLink() {

        if (this.TenantPM) {
            return this.TenantPM.SharedLogisticsMessageLink;
        }
        else return false;
    }
    public set SharedLogisticsMessageLink(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.SharedLogisticsMessageLink = value;
        }

    }


    IsShowAreaColseAndCancelButton: boolean;
    IsSharedLogisticsActivatedEnable: boolean;
    //  IsSharedLogisticsActivated: boolean;

    // IsWebAccessActivated: boolean;

    IsMobileActivatedEnable: boolean;
    //  IsMobileActivated: boolean;
    // SharedLogisticsMessageLink: boolean;

    
    SharedLogisticsMessageLinkEnable: boolean;


    IsSharedLogisticsActivatedCheckboxBoxId: string;
    IsMobileActivatedCheckboxBoxId: string;
    IsWebAccessActivatedCheckboxBoxId: string;
    SharedLogisticsMessageLinkCheckboxBoxId: string;

    IsShowActivateWebAccessArea: boolean = true;

    IsShowMobileActivateArea: boolean = true;
    TenantPM: TenantPM;
    public tenantPMService: TenantPMService;
    OnCloseWindowEvent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService();

        } 
    }

    ngOnInit(

    ) {

        if (this.TenantPM) {
            this.Run();
        }

        this.OnCloseWindowEvent.subscribe(($event: any) => {
            this.SaveButtonClicked();

        });

    }


    Run() {

         this.IsSharedLogisticsActivated = this.TenantPM.IsSharedLogisticsActivated;
         this.IsWebAccessActivated = this.TenantPM.IsWebAccessActivated;
         this.IsMobileActivated = this.TenantPM.IsMobileActivated;
         this.SharedLogisticsMessageLink = this.TenantPM.SharedLogisticsMessageLink;

        if (!FeatureLocator.HasFeaturePermession("General", "MOBILE")) {
            this.IsShowMobileActivateArea = false;
        }
        else {
            this.IsShowMobileActivateArea = true;
        }


        if (!FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS")) {
            this.IsShowActivateWebAccessArea = false;
        }
        else {
            this.IsShowActivateWebAccessArea = true;
        }

    




        this.IsSharedLogisticsActivatedCheckboxBoxId = Guid.newGuid();
        this.IsMobileActivatedCheckboxBoxId = Guid.newGuid();
        this.IsWebAccessActivatedCheckboxBoxId = Guid.newGuid();
        this.SharedLogisticsMessageLinkCheckboxBoxId = Guid.newGuid();

        this.SetPropertiesEnable();

    }


    SetPropertiesEnable() {

        if (this.IsSharedLogisticsActivated) {
            this.IsSharedLogisticsActivatedEnable = false;
            this.SharedLogisticsMessageLinkEnable = true;
            if (ObjectsLocator.GlobalSetting.WorkEnvironment != "cloud" && this.TenantPM.CountryCode == "IL") {
                this.IsMobileActivatedEnable = false;
            }
            else {
                this.IsMobileActivatedEnable = true;
            }


        }
        else {

            this.IsSharedLogisticsActivatedEnable = true;
            this.IsMobileActivatedEnable = false;
            this.SharedLogisticsMessageLinkEnable = false;
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();

        if (this.TenantPM.IsDirty) {
            this.RejectChanges();
        }

        this.CurrentSession.CloseCurrentWindow();
    }


    OnShardLogisticChange() {
        this.TenantPM.IsSharedLogisticsActivated = !this.TenantPM.IsSharedLogisticsActivated;
        this.IsSharedLogisticsActivated = !this.IsSharedLogisticsActivated;

        if (this.IsSharedLogisticsActivated) {
            this.SetPropertiesEnable();
        }
    }
    SaveButtonClicked() {

        if (this.TenantPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.tenantPMService.update(this.TenantPM).subscribe(res=> {

                this.CloseButtonClicked();
            });
        }
        else this.CloseButtonClicked();

    }



    SetWindowArgs(args: any) {

        this.TenantPM = args.TenantPM;
        this.IsShowAreaColseAndCancelButton = true;
        this.Clone();
        // this.Run();

    }



    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.TenantPM);
        this.myCloner.AddField('IsWebAccessActivated');
        this.myCloner.AddField('IsSharedLogisticsActivated');
        this.myCloner.AddField('IsMobileActivated');
        this.myCloner.AddField('SharedLogisticsMessageLink');
        this.myCloner.AddEntity(this.TenantPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }







}

