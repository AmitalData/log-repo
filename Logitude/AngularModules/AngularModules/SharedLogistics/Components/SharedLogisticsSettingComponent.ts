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

    public get IsCargoTrackWebAccessActivated() {

        if (this.TenantPM) {
            return this.TenantPM.IsCargoTrackWebAccessActivated;
        }
        else return false;
    }
    public set IsCargoTrackWebAccessActivated(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.IsCargoTrackWebAccessActivated = value;
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

    public get SharedLogisticsMasterMessageLink() {

        if (this.TenantPM) {
            return this.TenantPM.SharedLogisMasterMessageLink;
        }
        else return false;
    }
    public set SharedLogisticsMasterMessageLink(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.SharedLogisMasterMessageLink = value;
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
    SharedLogisticsMasterMessageLinkEnable: boolean = true;


    IsSharedLogisticsActivatedCheckboxBoxId: string;
    IsMobileActivatedCheckboxBoxId: string;
    IsWebAccessActivatedCheckboxBoxId: string;
    IsCargoTrackWebAccessActivatedCheckboxBoxId; string;
    SharedLogisticsMessageLinkCheckboxBoxId: string;
    SharedLogisticsMasterMessageLinkCheckboxBoxId: string;

    IsShowActivateWebAccessArea: boolean = true;

    IsShowMobileActivateArea: boolean = true;
    TenantPM: TenantPM;
    SharedTitleType: string;
    public tenantPMService: TenantPMService;
    OnCloseWindowEvent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;

    IsShowQuotesRequestActivatedInSharedLogistics: boolean = false;

    constructor() {
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService();
        }

        var quotesRequestActivatedInSharedLogisticsfeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "QRA" && d.TenantNumber == SessionLocator.Tenant)[0];
        if (quotesRequestActivatedInSharedLogisticsfeatureToggle) {
            this.IsShowQuotesRequestActivatedInSharedLogistics = true;
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
         this.IsCargoTrackWebAccessActivated = this.TenantPM.IsCargoTrackWebAccessActivated;
         this.IsMobileActivated = this.TenantPM.IsMobileActivated;
         this.SharedLogisticsMessageLink = this.TenantPM.SharedLogisticsMessageLink;
         this.SharedLogisticsMasterMessageLink = this.TenantPM.SharedLogisMasterMessageLink;
        this.IsQuotesRequestActivatedInSharedLogistics = this.TenantPM.IsQuotesRequestActivatedInShared;

        if (!FeatureLocator.HasFeaturePermession("General", "MOBILE") || this.SharedTitleType == "CargoTracking") {
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

        if (!FeatureLocator.HasFeaturePermession("General", "MASTERSDOCUMENTSLINK")) {
            this.SharedLogisticsMasterMessageLinkEnable = false;
        }



        this.IsSharedLogisticsActivatedCheckboxBoxId = Guid.newGuid();
        this.IsMobileActivatedCheckboxBoxId = Guid.newGuid();
        this.IsWebAccessActivatedCheckboxBoxId = Guid.newGuid();
        this.IsCargoTrackWebAccessActivatedCheckboxBoxId = Guid.newGuid();
        this.SharedLogisticsMessageLinkCheckboxBoxId = Guid.newGuid();
        this.SharedLogisticsMasterMessageLinkCheckboxBoxId = Guid.newGuid();

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
            this.tenantPMService.update(this.TenantPM).subscribe((res:any)=> {

                this.CloseButtonClicked();
            });
        }
        else this.CloseButtonClicked();

    }



    SetWindowArgs(args: any) {

        this.TenantPM = args.TenantPM;
        this.SharedTitleType = args.SharedTitleType;
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
        this.myCloner.AddField('DisplayDocumentsAndEvents');
        this.myCloner.AddField('SharedLogisticsMasterMessageLink');
        this.myCloner.AddField('IsQuotesRequestActivatedInShared');
        this.myCloner.AddField('IsCargoTrackWebAccessActivated');

        this.myCloner.AddEntity(this.TenantPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }




    public get DisplayDocumentsAndEvents() {

        if (this.TenantPM) {
            return this.TenantPM.DisplayDocumentsAndEvents;
        }
        else return false;
    }
    public set DisplayDocumentsAndEvents(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.DisplayDocumentsAndEvents = value;
        }

    }







    public get IsQuotesRequestActivatedInSharedLogistics() {

     if (!this.TenantPM) return false
        return this.TenantPM.IsQuotesRequestActivatedInShared;

    }
    public set IsQuotesRequestActivatedInSharedLogistics(value: boolean) {
        if (this.TenantPM) {
            this.TenantPM.IsQuotesRequestActivatedInShared = value;
        }

    }





}

